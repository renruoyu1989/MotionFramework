﻿//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionFramework.Config
{
	public class ConfigDefine
	{
		public const int CfgStreamMaxLen = 1024 * 1024 * 128; //文件最大128MB
		public const int TabStreamMaxLen = 1024 * 256; //单行最大256K
		public const short TabStreamHead = 0x2B2B; //文件标记
	}
}