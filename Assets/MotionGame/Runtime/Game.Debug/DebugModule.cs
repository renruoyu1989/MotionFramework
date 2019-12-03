using System.Collections;
using System.Collections.Generic;
using MotionEngine;
using MotionEngine.Debug;

namespace MotionGame
{
	[DebugAttribute("模块", 100)]
	public class DebugModule : IDebug
	{
		public void OnInit()
		{
		}
		public void OnGUI()
		{
			// 显示游戏模块数据
			AppEngine.Instance.OnGUI();
		}
	}
}