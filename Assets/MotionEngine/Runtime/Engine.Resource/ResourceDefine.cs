//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionFramework.Resource
{
	/// <summary>
	/// 资源加载模式
	/// </summary>
	public enum EAssetLoadMode
	{
		/// <summary>
		/// AssetDatabase加载模式
		/// </summary>
		EditorMode,

		/// <summary>
		/// Resource加载模式
		/// </summary>
		ResourceMode,

		/// <summary>
		/// AssetBundle加载模式
		/// </summary>
		BundleMode,
	}

	/// <summary>
	/// 资源加载结果
	/// </summary>
	public enum EAssetResult
	{
		None,
		Loading,
		Failed,
		OK,
	}

	/// <summary>
	/// 资源文件加载状态
	/// </summary>
	public enum EAssetFileLoadState
	{
		None = 0,
		LoadDepends,
		CheckDepends,
		LoadAssetFile,
		CheckAssetFile,
		LoadAssetFileOK,
		LoadAssetFileFailed,
	}

	/// <summary>
	/// 资源对象加载状态
	/// </summary>
	public enum EAssetObjectLoadState
	{
		None = 0,
		LoadAssetObject,
		CheckAssetObject,
		LoadAssetObjectOK,
		LoadAssetObjectFailed,
	}
}