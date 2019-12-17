﻿//--------------------------------------------------
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
	/// Resources接口加载器
	/// </summary>
	public class AssetResourceLoader : AssetFileLoader
	{
		/// <summary>
		/// 主资源对象
		/// </summary>
		private UnityEngine.Object _mainAsset;

		/// <summary>
		/// Request对象
		/// </summary>
		private ResourceRequest _cacheRequest;


		public AssetResourceLoader(bool isStreamScene, string loadPath)
			: base(isStreamScene, loadPath)
		{
		}
		public override void Update()
		{
			if (IsDone())
				return;

			if (LoadState == EAssetFileLoadState.None)
			{
				LoadState = EAssetFileLoadState.LoadAssetFile;
			}

			// 1. 加载主资源对象
			if (LoadState == EAssetFileLoadState.LoadAssetFile)
			{
				// Load resource folder file		
				_cacheRequest = Resources.LoadAsync(LoadPath);
				LoadState = EAssetFileLoadState.CheckAssetFile;
			}

			// 2. 检测AssetObject加载结果
			if (LoadState == EAssetFileLoadState.CheckAssetFile)
			{
				if (_cacheRequest.isDone == false)
					return;
				_mainAsset = _cacheRequest.asset;

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
					LogSystem.Log(ELogType.Warning, $"Failed to load resource file : {LoadPath}");
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
				LogSystem.Log(ELogType.Error, $"Can not load asset object, {nameof(AssetResourceLoader)} is not ok : {LoadPath}");
				callback?.Invoke(null);
				return;
			}

			callback?.Invoke(_mainAsset);
		}
	}
}