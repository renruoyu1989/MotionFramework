using System.Collections;
using System.Collections.Generic;
using MotionEngine;
using MotionEngine.Debug;
using UnityEngine;

namespace MotionGame
{
	[DebugAttribute("实体池", 105)]
	public class DebugPool : IDebug
	{
		// GUI相关
		private Vector2 _scrollPos = Vector2.zero;

		public void OnInit()
		{
		}
		public void OnGUI()
		{
			var pools = PoolManager.Instance.DebugAllPools;
			DebugConsole.GUILable($"池总数：{pools.Count}");

			_scrollPos = DebugConsole.GUIBeginScrollView(_scrollPos, 30);
			foreach (var pair in pools)
			{
				string content = $"[{pair.Value.ResName}] CacheCount = {pair.Value.Count} SpwanCount = {pair.Value.SpawnCount}";
				if (pair.Value.LoadResult == MotionEngine.Res.EAssetResult.Failed)
					DebugConsole.GUIRedLable(content);
				else
					DebugConsole.GUILable(content);
			}
			DebugConsole.GUIEndScrollView();
		}
	}
}