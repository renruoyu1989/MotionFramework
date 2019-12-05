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


		public AssetDatabaseLoader(bool isStreamScene, string loadPath)
			: base(isStreamScene, loadPath)
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
				System.Type assetType = UnityEditor.AssetDatabase.GetMainAssetTypeAtPath(LoadPath);
				_mainAsset = UnityEditor.AssetDatabase.LoadAssetAtPath(LoadPath, assetType);
				LoadState = EAssetFileLoadState.CheckAssetFile;

				// 注意：为了模拟异步加载效果，这里直接返回
				return;
			}

			// 2. 检测AssetObject加载结果
			if (LoadState == EAssetFileLoadState.CheckAssetFile)
			{
				// Check scene
				if (IsStreamScene)
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
		public override void LoadMainAsset(System.Type assetType, System.Action<UnityEngine.Object> callback)
		{
			// Check error
			if (LoadState != EAssetFileLoadState.LoadAssetFileOK)
			{
				LogSystem.Log(ELogType.Error, $"Can not load asset object, {nameof(AssetDatabaseLoader)} is not ok : {LoadPath}");
				callback?.Invoke(null);
				return;
			}

			callback?.Invoke(_mainAsset);
		}
	}
}
