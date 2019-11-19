using System.Collections;
using System.Collections.Generic;
using MotionEngine;
using MotionEngine.Debug;

namespace MotionGame
{
	[DebugAttribute("实体资源池", 105)]
	public class DebugPool : IDebug
	{
		public void OnInit()
		{
		}
		public void OnGUI()
		{
			var pools = PoolManager.Instance.DebugAllPools;
			DebugConsole.GUILable($"池总数：{pools.Count}");
			foreach (var pair in pools)
			{
				DebugConsole.GUILable($"[{pair.Value.ResName}] CacheCount = {pair.Value.Count} SpwanCount = {pair.Value.SpawnCount}");
			}
		}
	}
}