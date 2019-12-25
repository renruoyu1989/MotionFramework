//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotionFramework.Resource
{
	/// <summary>
	/// AssetBundle接口加载器
	/// </summary>
	public class AssetBundleLoader : AssetFileLoader
	{
		/// <summary>
		/// 依赖加载器列表
		/// </summary>
		private readonly List<AssetFileLoader> _depends = new List<AssetFileLoader>(10);

		/// <summary>
		/// Manifest路径
		/// </summary>
		private string _manifestPath = string.Empty;

		/// <summary>
		/// AssetBundle对象
		/// </summary>
		private AssetBundle _cacheBundle;

		/// <summary>
		/// Request对象
		/// </summary>
		private AssetBundleCreateRequest _cacheRequest;


		public AssetBundleLoader(bool isStreamScene, string loadPath, string manifestPath)
			: base(isStreamScene, loadPath)
		{
			_manifestPath = manifestPath;
		}
		public override void Update()
		{
			// 轮询更新所有对象加载器
			if (LoadState == EAssetFileLoadState.LoadAssetFileOK)
				UpdateAllAssetObjectLoader();

			if (IsDone())
				return;

			if (LoadState == EAssetFileLoadState.None)
			{
				LoadState = EAssetFileLoadState.LoadDepends;
			}

			// 1. 加载所有依赖项
			if (LoadState == EAssetFileLoadState.LoadDepends)
			{
				string[] dependencies = AssetSystem.BundleMethod.GetDirectDependencies(_manifestPath);
				if (dependencies.Length > 0)
				{
					foreach (string dpManifestPath in dependencies)
					{
						string dpLoadPath = AssetSystem.BundleMethod.GetAssetBundleLoadPath(dpManifestPath);
						AssetFileLoader dpLoader = AssetSystem.GetFileLoader(false, dpLoadPath, null, dpManifestPath);
						_depends.Add(dpLoader);
					}
				}
				LoadState = EAssetFileLoadState.CheckDepends;
			}

			// 2. 检测所有依赖完成状态
			if (LoadState == EAssetFileLoadState.CheckDepends)
			{
				foreach (var dpLoader in _depends)
				{
					if (dpLoader.IsDone() == false)
						return;
				}
				LoadState = EAssetFileLoadState.LoadAssetFile;
			}

			// 3. 加载AssetBundle
			if (LoadState == EAssetFileLoadState.LoadAssetFile)
			{
#if UNITY_EDITOR
				// 注意：Unity2017.4编辑器模式下，如果AssetBundle文件不存在会导致编辑器崩溃，这里做了预判。
				if (System.IO.File.Exists(LoadPath) == false)
				{
					LogSystem.Log(ELogType.Warning, $"Not found assetBundle file : {LoadPath}");
					LoadState = EAssetFileLoadState.LoadAssetFileFailed;
					LoadCallback?.Invoke(this);
					return;
				}
#endif

				// Load assetBundle file
				_cacheRequest = AssetBundle.LoadFromFileAsync(LoadPath);
				LoadState = EAssetFileLoadState.CheckAssetFile;
			}

			// 4. 检测AssetBundle加载结果
			if (LoadState == EAssetFileLoadState.CheckAssetFile)
			{
				if (_cacheRequest.isDone == false)
					return;
				_cacheBundle = _cacheRequest.assetBundle;

				// Check scene
				if (IsStreamScene)
				{
					LoadState = EAssetFileLoadState.LoadAssetFileOK;
					LoadCallback?.Invoke(this);
					return;
				}

				// Check error
				if (_cacheBundle == null)
				{
					LogSystem.Log(ELogType.Warning, $"Failed to load assetBundle file : {LoadPath}");
					LoadState = EAssetFileLoadState.LoadAssetFileFailed;
					LoadCallback?.Invoke(this);
				}
				else
				{
					LoadState = EAssetFileLoadState.LoadAssetFileOK;
					LoadCallback?.Invoke(this);
				}
			}
		}
		public override void LoadMainAsset(System.Type assetType, System.Action<UnityEngine.Object> callback)
		{
			// Check error
			if (LoadState != EAssetFileLoadState.LoadAssetFileOK)
			{
				LogSystem.Log(ELogType.Error, $"Can not load asset object, {nameof(AssetBundleLoader)} is not ok : {LoadPath}");
				callback?.Invoke(null);
				return;
			}

			string assetName = AssetSystem.GetCacheFileName(LoadPath);
			LoadAsset(assetName, assetType, callback);
		}

		public override void Reference()
		{
			base.Reference();

			// 同时引用一遍所有依赖资源
			for (int i = 0; i < _depends.Count; i++)
			{
				_depends[i].Reference();
			}
		}
		public override void Release()
		{
			base.Release();

			// 同时释放一遍所有依赖资源
			for (int i = 0; i < _depends.Count; i++)
			{
				_depends[i].Release();
			}
		}
		public override void UnLoad(bool force)
		{
			// Check fatal
			if (RefCount > 0)
				throw new Exception($"Bundle file loader ref is not zero : {LoadPath}");
			if (IsDone() == false)
				throw new Exception($"Bundle file loader is not done : {LoadPath}");

			// 卸载AssetBundle
			if (_cacheBundle != null)
			{
				_cacheBundle.Unload(force);
				_cacheBundle = null;
			}

			_depends.Clear();
		}
		public override bool IsDone()
		{
			if (base.IsDone() == false)
				return false;

			return CheckAllAssetObjectLoaderIsDone();
		}

		#region 资源对象相关
		// 对象加载器列表
		private readonly List<AssetObjectLoader> _assetObjectLoaders = new List<AssetObjectLoader>();

		/// <summary>
		/// 检测所有对象加载器是否完毕
		/// </summary>
		public bool CheckAllAssetObjectLoaderIsDone()
		{
			bool isAllLoadDone = true;
			for (int i = 0; i < _assetObjectLoaders.Count; i++)
			{
				var loader = _assetObjectLoaders[i];
				if (loader.IsDone() == false)
				{
					isAllLoadDone = false;
					break;
				}
			}
			return isAllLoadDone;
		}

		/// <summary>
		/// 加载资源对象
		/// </summary>
		/// <param name="assetName">资源对象名称</param>
		/// <param name="assetType">资源对象类型</param>
		/// <param name="callback">结果回调</param>
		public void LoadAsset(string assetName, System.Type assetType, System.Action<UnityEngine.Object> callback)
		{
			// 如果已经提交相同请求
			AssetObjectLoader loader = TryGetAssetObjectLoaderInternal(assetName);
			if (loader != null)
			{
				if (loader.IsDone())
					callback?.Invoke(loader.AssetObject);
				else
					loader.LoadCallback += callback;
			}
			else
			{
				// 创建加载器
				AssetObjectLoader newLoader = new AssetObjectLoader(_cacheBundle, assetName, assetType);

				// 新增下载需求
				_assetObjectLoaders.Add(newLoader);
				newLoader.LoadCallback = callback;
				newLoader.Update(); //立刻轮询
			}
		}

		// 获取一个对象加载器
		private AssetObjectLoader TryGetAssetObjectLoaderInternal(string assetName)
		{
			AssetObjectLoader loader = null;
			for (int i = 0; i < _assetObjectLoaders.Count; i++)
			{
				AssetObjectLoader temp = _assetObjectLoaders[i];
				if (temp.AssetName.Equals(assetName))
				{
					loader = temp;
					break;
				}
			}
			return loader;
		}

		// 更新所有对象加载器
		private void UpdateAllAssetObjectLoader()
		{
			for (int i = 0; i < _assetObjectLoaders.Count; i++)
			{
				var loader = _assetObjectLoaders[i];
				loader.Update();
			}
		}
		#endregion
	}
}