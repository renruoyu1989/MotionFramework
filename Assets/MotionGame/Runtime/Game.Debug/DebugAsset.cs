using System;
using System.Collections;
using System.Collections.Generic;
using MotionEngine.Res;
using MotionEngine.Debug;
using UnityEngine;

namespace MotionGame
{
	[DebugAttribute("资源列表", 103)]
	public class DebugAsset : IDebug
	{
		// 加载器总数
		private int _loaderTotalCount = 0;

		// 显示信息集合
		private List<string> _cacheInfos = new List<string>(1000);

		// 过滤的关键字
		private string _filterKey = string.Empty;


		public void OnInit()
		{
		}
		public void OnGUI()
		{
			// 过滤信息
			FilterInfos();

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("搜索关键字 : ", DebugConsole.GUILableStyle, GUILayout.Width(140));
				_filterKey = GUILayout.TextField(_filterKey, DebugConsole.GUITextFieldStyle, GUILayout.Width(400));
			}
			GUILayout.EndHorizontal();

			GUILayout.Space(10);
			DebugConsole.GUILable($"加载器总数：{_loaderTotalCount}");
			for (int i = 0; i < _cacheInfos.Count; i++)
			{
				DebugConsole.GUILable(_cacheInfos[i]);
			}
		}
		private void FilterInfos()
		{
			// 清空列表
			_cacheInfos.Clear();

			// 绘制显示列表
			var fileLoaders = AssetSystem.GetFileLoaders();
			_loaderTotalCount = fileLoaders.Count;
			foreach (var loader in fileLoaders)
			{
				// 只搜索关键字
				if (string.IsNullOrEmpty(_filterKey) == false)
				{
					if (loader.LoadPath.Contains(_filterKey) == false)
						continue;
				}

				string showInfo = GameUtility.Substring(loader.LoadPath, "/assets/", false);
				showInfo = showInfo.Replace(".unity3d", string.Empty);
				showInfo = $"{showInfo} = {loader.RefCount}";

				// 添加到显示列表
				_cacheInfos.Add(showInfo);
			}

			// 重新排序
			_cacheInfos.Sort(string.CompareOrdinal);
		}
	}
}