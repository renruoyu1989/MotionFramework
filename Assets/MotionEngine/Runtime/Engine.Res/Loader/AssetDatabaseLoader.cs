//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotionEngine.Res
{
	public class AssetDatabaseLoader : AssetFileLoader
	{
		/// <summary>
		/// 主资源对象
		/// </summary>
		private UnityEngine.Object _mainAsset;


		public AssetDatabaseLoader(EAssetType assetType, string loadPath)
			: base(assetType, loadPath)
		{
		}
		public override void Update()
		{
#if UNITY_EDITOR
			if (IsDone())
				return;

			if (LoadState == EAssetFileLoadState.None)
			{
				LoadState = EAssetFileLoadState.LoadAssetFile;
			}

			// 1. 加载主资源对象
			if (LoadState == EAssetFileLoadState.LoadAssetFile)
			{
				// Load from database
				System.Type systemType = AssetSystem.MakeSystemType(AssetType);
				string assetPath = GetDatabaseAssetPath(LoadPath);
				_mainAsset = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, systemType);
				LoadState = EAssetFileLoadState.CheckAssetFile;

				// 注意：为了模拟异步加载效果，这里直接返回
				return;
			}

			// 2. 检测AssetObject加载结果
			if (LoadState == EAssetFileLoadState.CheckAssetFile)
			{
				// Check scene
				if (AssetType == EAssetType.Scene)
				{
					LoadState = EAssetFileLoadState.LoadAssetFileOK;
					LoadCallback?.Invoke(this);
					return;
				}

				// Check error
				if (_mainAsset == null)
				{
					LogSystem.Log(ELogType.Warning, $"Failed to load database file : {LoadPath}");
					LoadState = EAssetFileLoadState.LoadAssetFileFailed;
					LoadCallback?.Invoke(this);
				}
				else
				{
					LoadState = EAssetFileLoadState.LoadAssetFileOK;
					LoadCallback?.Invoke(this);
				}
			}
#else
			throw new Exception("AssetDatabaseLoader only support unity editor.");
#endif
		}
		public override void LoadMainAsset(EAssetType mainAssetType, OnAssetObjectLoad callback)
		{
			// Check error
			if (LoadState != EAssetFileLoadState.LoadAssetFileOK)
			{
				LogSystem.Log(ELogType.Error, $"Can not load asset object, {nameof(AssetDatabaseLoader)} is not ok : {LoadPath}");
				callback?.Invoke(null, false);
				return;
			}

			callback?.Invoke(_mainAsset, LoadState == EAssetFileLoadState.LoadAssetFileOK);
		}

		/// <summary>
		/// 获取AssetDatabase的加载路径
		/// </summary>
		public static string GetDatabaseAssetPath(string loadPath)
		{
#if UNITY_EDITOR
			// 注意：AssetDatabase加载资源需要提供文件后缀格式，然而LoadPath并没有文件格式信息
			// 所以我们通过查找该文件所在文件夹内同名的首个文件来确定AssetDatabase的加载路径。
			string fileName = System.IO.Path.GetFileName(loadPath);
			string folderPath = $"{AssetSystem.AssetRootPath}/{System.IO.Path.GetDirectoryName(loadPath)}";
			string[] guids = UnityEditor.AssetDatabase.FindAssets(string.Empty, new[] { folderPath });
			for (int i = 0; i < guids.Length; i++)
			{
				string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
				string assetName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
				if (assetName == fileName)
					return assetPath;
			}

			LogSystem.Log(ELogType.Warning, $"Not found asset database file : {loadPath}");
			return string.Empty;
#else
			throw new Exception("AssetDatabaseLoader only support unity editor.");
#endif
		}
	}
}
