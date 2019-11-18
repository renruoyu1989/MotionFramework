//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotionEngine.Res
{
	/// <summary>
	/// 资源系统
	/// </summary>
	public static class AssetSystem
	{
		/// <summary>
		/// 资源加载模式
		/// </summary>
		public static EAssetLoadMode AssetLoadMode { private set; get; } = EAssetLoadMode.ResourceMode;

		/// <summary>
		/// Bundle接口
		/// </summary>
		public static IBundleMethod BundleMethod { private set; get; }

		/// <summary>
		/// 加载器集合
		/// </summary>
		private static readonly List<AssetFileLoader> _fileLoaders = new List<AssetFileLoader>(1000);

		/// <summary>
		/// 文件名称缓存集合
		/// </summary>
		private static readonly Dictionary<string, string> _cacheFileName = new Dictionary<string, string>(10000);

		/// <summary>
		/// 资源卸载辅助集合
		/// </summary>
		private static readonly List<string> _removeKeys = new List<string>(100);


		/// <summary>
		/// 设置资源加载模式
		/// </summary>
		public static void SetAssetLoadMode(EAssetLoadMode loadMode)
		{
			AssetLoadMode = loadMode;
		}

		/// <summary>
		/// 设置Bundle接口
		/// </summary>
		public static void SetBundleMethod(IBundleMethod bundleMethod)
		{
			BundleMethod = bundleMethod;
		}

		/// <summary>
		/// 轮询更新
		/// </summary>
		public static void UpdatePoll()
		{
			for (int i = 0; i < _fileLoaders.Count; i++)
			{
				_fileLoaders[i].Update();
			}
		}

		/// </summary>
		/// 加载资源
		/// </summary>
		/// <param name="resName">资源的相对路径名称</param>
		/// <param name="assetType">资源类型</param>
		/// <param name="callback">完成回调</param>
		/// <returns>返回该资源唯一的加载器</returns>
		public static AssetFileLoader LoadAssetFile(string resName, EAssetType assetType, OnAssetFileLoad callback)
		{
			if (AssetLoadMode == EAssetLoadMode.EditorMode)
			{
				string loadPath = resName;
				return GetFileLoader(assetType, loadPath, callback, null);
			}
			else if (AssetLoadMode == EAssetLoadMode.ResourceMode)
			{
				string loadPath = resName;
				return GetFileLoader(assetType, loadPath, callback, null);
			}
			else if (AssetLoadMode == EAssetLoadMode.BundleMode)
			{
				if (BundleMethod == null)
					throw new Exception("IBundleMethod is null. use AssetSystem.SetBundleMethod()");

				string manifestPath = AssetPathHelper.ConvertResourcePathToManifestPath(resName);
				string loadPath = BundleMethod.GetAssetBundleLoadPath(manifestPath);
				return GetFileLoader(assetType, loadPath, callback, manifestPath);
			}
			else
			{
				throw new NotImplementedException($"{AssetLoadMode}");
			}
		}

		/// <summary>
		/// 从缓存列表里获取加载器，如果不存在创建一个新的加载器并添加到列表
		/// </summary>
		public static AssetFileLoader GetFileLoader(EAssetType assetType, string loadPath, OnAssetFileLoad callback, string manifestPath)
		{
			// 如果已经提交相同请求
			AssetFileLoader loader = TryGetFileLoaderInternal(loadPath);
			if (loader != null)
			{
				loader.Reference(); //引用计数
				if (loader.IsDone())
				{
					if (callback != null)
						callback.Invoke(loader);
				}
				else
				{
					if (callback != null)
						loader.LoadCallback += callback;
				}
				return loader;
			}

			// 创建加载器
			AssetFileLoader newLoader = null;
			if (AssetLoadMode == EAssetLoadMode.EditorMode)
				newLoader = new AssetDatabaseLoader(assetType, loadPath);
			else if (AssetLoadMode == EAssetLoadMode.ResourceMode)
				newLoader = new AssetResourceLoader(assetType, loadPath);
			else if (AssetLoadMode == EAssetLoadMode.BundleMode)
				newLoader = new AssetBundleLoader(assetType, loadPath, manifestPath);
			else
				throw new NotImplementedException($"{AssetLoadMode}");

			// 新增下载需求
			_fileLoaders.Add(newLoader);
			newLoader.LoadCallback = callback;
			newLoader.Reference(); //引用计数
			newLoader.Update(); //立刻轮询
			return newLoader;
		}
		private static AssetFileLoader TryGetFileLoaderInternal(string assetPath)
		{
			AssetFileLoader loader = null;
			for (int i = 0; i < _fileLoaders.Count; i++)
			{
				AssetFileLoader temp = _fileLoaders[i];
				if (temp.LoadPath.Equals(assetPath))
				{
					loader = temp;
					break;
				}
			}
			return loader;
		}

		/// <summary>
		/// 资源回收
		/// 卸载引用计数为零的资源
		/// </summary>
		public static void Release()
		{
			for (int i = _fileLoaders.Count - 1; i >= 0; i--)
			{
				AssetFileLoader loader = _fileLoaders[i];
				if (loader.IsDone() && loader.RefCount <= 0)
				{
					loader.UnLoad(true);
					_fileLoaders.RemoveAt(i);
				}
			}
		}

		/// <summary>
		/// 强制回收所有资源
		/// </summary>
		public static void ForceReleaseAll()
		{
			for (int i = 0; i < _fileLoaders.Count; i++)
			{
				AssetFileLoader loader = _fileLoaders[i];
				loader.UnLoad(true);
			}
			_fileLoaders.Clear();

			// 释放所有资源
			Resources.UnloadUnusedAssets();
		}

		/// <summary>
		/// 获取资源的系统类型
		/// </summary>
		public static System.Type MakeSystemType(EAssetType assetType)
		{
			if (assetType == EAssetType.Scene)
				return null;
			else if (assetType == EAssetType.Text)
				return typeof(UnityEngine.TextAsset);
			else if (assetType == EAssetType.Audio)
				return typeof(UnityEngine.AudioClip);
			else if (assetType == EAssetType.Texture)
				return typeof(UnityEngine.Texture);
			else if (assetType == EAssetType.Sprite)
				return typeof(UnityEngine.Sprite);
			else if (assetType == EAssetType.Atlas)
				return typeof(UnityEngine.U2D.SpriteAtlas);
			else if (assetType == EAssetType.Video)
				return typeof(UnityEngine.Video.VideoClip);
			else if (assetType == EAssetType.Object)
				return typeof(UnityEngine.GameObject);
			else
				throw new NotImplementedException($"{assetType}");
		}

		/// <summary>
		/// 解析路径里的文件名称，并优化缓存在字典里
		/// </summary>
		public static string GetCacheFileName(string path)
		{
			if (_cacheFileName.ContainsKey(path))
				return _cacheFileName[path];

			string name = Path.GetFileNameWithoutExtension(path);
			_cacheFileName.Add(path, name);
			return name;
		}

		// 获取加载器相关数据
		public static int GetFileLoaderCount()
		{
			return _fileLoaders.Count;
		}
		public static int GetFileLoaderFailedCount()
		{
			int count = 0;
			for (int i = 0; i < _fileLoaders.Count; i++)
			{
				AssetFileLoader temp = _fileLoaders[i];
				if (temp.LoadState == EAssetFileLoadState.LoadAssetFileFailed)
					count++;
			}
			return count;
		}

		/// <summary>
		/// 调试专属方法
		/// </summary>
		public static List<AssetFileLoader> GetFileLoaders()
		{
			return _fileLoaders;
		}
	}
}