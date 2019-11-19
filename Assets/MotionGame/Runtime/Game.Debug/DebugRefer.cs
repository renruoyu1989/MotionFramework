using System.Collections;
using System.Collections.Generic;
using MotionEngine;
using MotionEngine.Debug;
using MotionEngine.Reference;

namespace MotionGame
{
	[DebugAttribute("引用池", 104)]
	public class DebugRefer : IDebug
	{
		public void OnInit()
		{
		}
		public void OnGUI()
		{
			var pools = ReferenceSystem.DebugAllPools;
			DebugConsole.GUILable($"池总数：{pools.Count}");
			foreach(var pair in pools)
			{
				DebugConsole.GUILable($"[{pair.Value.ClassType.FullName}] CacheCount = {pair.Value.Count} SpwanCount = {pair.Value.SpawnCount}");
			}
		}
	}
}