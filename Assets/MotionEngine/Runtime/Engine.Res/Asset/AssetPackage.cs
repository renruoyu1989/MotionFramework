//--------------------------------------------------
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using UnityEngine;

namespace MotionEngine.Res
{
	public class AssetPackage : Asset
	{
		/// <summary>
		/// 从包裹里加载一个资源对象
		/// </summary>
		/// <param name="assetName">资源对象名称</param>
		/// <param name="assetType">资源对象类型</param>
		/// <param name="callback">结果回调</param>
		public void GetAsset(string assetName, System.Type assetType, System.Action<UnityEngine.Object> callback)
		{
			if (Result != EAssetResult.OK)
			{
				LogSystem.Log(ELogType.Warning, $"Asset file is not prepare : {ResName}");
				return ;
			}

			if(_cacheLoader is AssetBundleLoader)
			{
				AssetBundleLoader loader = _cacheLoader as AssetBundleLoader;
				loader.LoadAsset(assetName, assetType, callback);
			}
			else
			{
				LogSystem.Log(ELogType.Error, $"{nameof(AssetPackage)} only support AssetBundle load mode.");
			}
		}
	}
}