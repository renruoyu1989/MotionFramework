//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;

namespace MotionFramework.Debug
{
	[AttributeUsage(AttributeTargets.Class)]
	public class DebugAttribute : Attribute
	{
		/// <summary>
		/// 控制台里显示的名称
		/// </summary>
		public string Title;

		/// <summary>
		/// 控制台里显示的顺序
		/// </summary>
		public int Order;

		public DebugAttribute(string title, int order)
		{
			Title = title;
			Order = order;
		}
	}
}