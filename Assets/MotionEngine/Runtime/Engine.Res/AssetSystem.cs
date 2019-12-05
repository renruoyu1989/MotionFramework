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
		/// 资源系统根路径
		/// </summary>
		public static string AssetRootPath { set; get; }

		/// <summary>
		/// 资源系统加载模式
		/// </summary>
		public static EAssetLoadMode AssetLoadMode { set; get; }

		/// <summary>
		/// Bundle接口
		/// </summary>
		public static IBundleMethod BundleMethod { set; get; }


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
		public static AssetFileLoader LoadAssetFile(string resName, bool isStreamScene, System.Action<AssetFileLoader> callback)
		{
			if (AssetLoadMode == EAssetLoadMode.EditorMode)
			{
				string loadPath = GetDatabaseAssetPath(resName);
				return GetFileLoader(isStreamScene, loadPath, callback, null);
			}
			else if (AssetLoadMode == EAssetLoadMode.ResourceMode)
			{
				string loadPath = resName;
				return GetFileLoader(isStreamScene, loadPath, callback, null);
			}
			else if (AssetLoadMode == EAssetLoadMode.BundleMode)
			{
				if (BundleMethod == null)
					throw new Exception("AssetSystem.BundleMethod is null.");

				string manifestPath = AssetPathHelper.ConvertResourcePathToManifestPath(resName);
				string loadPath = BundleMethod.GetAssetBundleLoadPath(manifestPath);
				return GetFileLoader(isStreamScene, loadPath, callback, manifestPath);
			}
			else
			{
				throw new NotImplementedException($"{AssetLoadMode}");
			}
		}

		/// <summary>
		/// 从缓存列表里获取加载器，如果不存在创建一个新的加载器并添加到列表
		/// </summary>
		public static AssetFileLoader GetFileLoader(bool isStreamScene, string loadPath, System.Action<AssetFileLoader> callback, string manifestPath)
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
				newLoader = new AssetDatabaseLoader(isStreamScene, loadPath);
			else if (AssetLoadMode == EAssetLoadMode.ResourceMode)
				newLoader = new AssetResourceLoader(isStreamScene, loadPath);
			else if (AssetLoadMode == EAssetLoadMode.BundleMode)
				newLoader = new AssetBundleLoader(isStreamScene, loadPath, manifestPath);
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
		public static List<AssetFileLoader> DebugAllLoaders()
		{
			return _fileLoaders;
		}

		/// <summary>
		/// 获取AssetDatabase的加载路径
		/// </summary>
		public static string GetDatabaseAssetPath(string resName)
		{
#if UNITY_EDITOR
			// 注意：AssetDatabase加载资源需要提供文件后缀格式，然而LoadPath并没有文件格式信息
			// 所以我们通过查找该文件所在文件夹内同名的首个文件来确定AssetDatabase的加载路径。
			string fileName = System.IO.Path.GetFileName(resName);
			string folderPath = $"{AssetSystem.AssetRootPath}/{System.IO.Path.GetDirectoryName(resName)}";
			string[] guids = UnityEditor.AssetDatabase.FindAssets(string.Empty, new[] { folderPath });
			for (int i = 0; i < guids.Length; i++)
			{
				string loadPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
				string assetName = System.IO.Path.GetFileNameWithoutExtension(loadPath);
				if (assetName == fileName)
					return loadPath;
			}

			LogSystem.Log(ELogType.Warning, $"Not found asset database file : {resName}");
			return string.Empty;
#else
			throw new Exception("AssetDatabaseLoader only support unity editor.");
#endif
		}
	}
}