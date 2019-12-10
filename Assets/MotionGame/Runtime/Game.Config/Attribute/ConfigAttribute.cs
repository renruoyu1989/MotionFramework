//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;

namespace MotionFramework.Config
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ConfigAttribute : Attribute
	{
		public string CfgType;

		public ConfigAttribute(string cfgType)
		{
			CfgType = cfgType;
		}
	}
}