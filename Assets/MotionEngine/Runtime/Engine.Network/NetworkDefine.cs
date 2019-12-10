//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionFramework.Network
{
	public class NetworkDefine
	{
		public const int PackageMaxSize = ushort.MaxValue; // 网络包最大长度
		public const int ByteBufferSize = PackageMaxSize * 4; // 缓冲区长度（注意：推荐4倍最大包体长度）
		public const int WebRequestTimeout = 30; //网络请求的超时时间（单位：秒）
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