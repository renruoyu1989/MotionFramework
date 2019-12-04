using System.Collections;
using System.Collections.Generic;
using MotionEngine;
using MotionEngine.Debug;
using MotionEngine.Reference;
using UnityEngine;

namespace MotionGame
{
	[DebugAttribute("引用池", 104)]
	public class DebugRefer : IDebug
	{
		// GUI相关
		private Vector2 _scrollPos = Vector2.zero;

		public void OnInit()
		{
		}
		public void OnGUI()
		{
			var pools = ReferenceSystem.DebugAllPools;
			DebugConsole.GUILable($"池总数：{pools.Count}");

			_scrollPos = DebugConsole.GUIBeginScrollView(_scrollPos, 30);
			foreach (var pair in pools)
			{
				DebugConsole.GUILable($"[{pair.Value.ClassType.FullName}] CacheCount = {pair.Value.Count} SpwanCount = {pair.Value.SpawnCount}");
			}
			DebugConsole.GUIEndScrollView();
		}
	}
}