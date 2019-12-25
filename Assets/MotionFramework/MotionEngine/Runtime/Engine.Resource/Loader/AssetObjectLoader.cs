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
	/// 资源对象加载器
	/// </summary>
	public class AssetObjectLoader
	{
		private AssetBundle _cacheBundle;

		/// <summary>
		/// 资源对象的名称
		/// </summary>
		public string AssetName { private set; get; }

		/// <summary>
		/// 资源对象的类型
		/// </summary>
		public System.Type AssetType { private set; get; }

		/// <summary>
		/// 最终获取的资源对象
		/// </summary>
		public UnityEngine.Object AssetObject { private set; get; }

		/// <summary>
		/// 资源对象的加载结果
		/// </summary>
		public EAssetObjectLoadState LoadState { private set; get; }

		/// <summary>
		/// 结果回调
		/// </summary>
		public System.Action<UnityEngine.Object> LoadCallback;

		/// <summary>
		/// Request对象
		/// </summary>
		private AssetBundleRequest _cacheRequest;


		public AssetObjectLoader(AssetBundle bundle, string assetName, System.Type assetType)
		{
			_cacheBundle = bundle;
			AssetName = assetName;
			AssetType = assetType;
			LoadState = EAssetObjectLoadState.None;
		}

		/// <summary>
		/// 轮询更新方法
		/// </summary>
		public void Update()
		{
			if (IsDone())
				return;

			if(LoadState == EAssetObjectLoadState.None)
			{
				LoadState = EAssetObjectLoadState.LoadAssetObject;
			}

			// 1. 加载主资源对象
			if (LoadState == EAssetObjectLoadState.LoadAssetObject)
			{
				if(AssetType == null)
					_cacheRequest = _cacheBundle.LoadAssetAsync(AssetName);
				else
					_cacheRequest = _cacheBundle.LoadAssetAsync(AssetName, AssetType);
				LoadState = EAssetObjectLoadState.CheckAssetObject;
			}

			// 2. 检测AssetObject加载结果
			if (LoadState == EAssetObjectLoadState.CheckAssetObject)
			{
				if (_cacheRequest.isDone == false)
					return;
				AssetObject = _cacheRequest.asset;
				LoadState = AssetObject == null ? EAssetObjectLoadState.LoadAssetObjectFailed : EAssetObjectLoadState.LoadAssetObjectOK;
				LoadCallback?.Invoke(AssetObject);
			}
		}

		/// <summary>
		/// 是否完毕（成功或失败）
		/// </summary>
		public bool IsDone()
		{
			return LoadState == EAssetObjectLoadState.LoadAssetObjectOK || LoadState == EAssetObjectLoadState.LoadAssetObjectFailed;
		}
	}
}