﻿//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using UnityEditor;

/// <summary>
/// 资源信息类
/// </summary>
public class AssetInfo
{
	public string AssetPath { private set; get; }
	public bool IsCollectAsset { private set; get; }
	public bool IsSceneAsset { private set; get; }
	public bool IsVideoAsset { private set; get; }

	public int DependCount = 0; //被依赖次数
	public string AssetBundleName = null;
	public string AssetBundleVariant = null;

	public AssetInfo(string assetPath)
	{
		AssetPath = assetPath;
		IsCollectAsset = BuildSettingData.IsCollectAsset(assetPath);
		IsSceneAsset = AssetDatabase.GetMainAssetTypeAtPath(assetPath) == typeof(SceneAsset);
		IsVideoAsset = AssetDatabase.GetMainAssetTypeAtPath(assetPath) == typeof(UnityEngine.Video.VideoClip);
	}
}