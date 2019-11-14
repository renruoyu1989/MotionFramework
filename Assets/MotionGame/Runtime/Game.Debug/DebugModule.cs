﻿using System.Collections;
using System.Collections.Generic;
using MotionEngine;
using MotionEngine.Debug;

namespace MotionGame
{
	[DebugAttribute("模块信息", 101)]
	public class DebugModule : IDebug
	{
		public void OnInit()
		{
		}
		public void OnGUI()
		{
			// 显示游戏模块数据
			Engine.Instance.OnGUI();
		}
	}
}