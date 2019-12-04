using System.Collections;
using System.Collections.Generic;
using MotionEngine;
using MotionEngine.Debug;
using UnityEngine;

namespace MotionGame
{
	[DebugAttribute("模块", 100)]
	public class DebugModule : IDebug
	{
		// GUI相关
		private Vector2 _scrollPos = Vector2.zero;

		public void OnInit()
		{
		}
		public void OnGUI()
		{
			_scrollPos = DebugConsole.GUIBeginScrollView(_scrollPos, 0);
			AppEngine.Instance.OnGUI();
			DebugConsole.GUIEndScrollView();
		}
	}
}