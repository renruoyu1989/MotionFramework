//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionEngine.Res
{
	public class ResDefine
	{
		public const int WebRequestTimeout = 30; //网络请求的超时时间（单位：秒）
	}

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

	/// <summary>
	/// 网络加载状态
	/// </summary>
	public enum EWebLoadState
	{
		None = 0,
		Loading, //加载中
		LoadSucceed, //加载成功
		LoadFailed, //加载失败
	}
}