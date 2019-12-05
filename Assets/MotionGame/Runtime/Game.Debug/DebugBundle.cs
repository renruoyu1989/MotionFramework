using System;
using System.Collections;
using System.Collections.Generic;
using MotionEngine.Res;
using MotionEngine.Debug;
using MotionEngine.Reference;
using UnityEngine;

namespace MotionGame
{
	[DebugAttribute("资源列表", 103)]
	public class DebugBundle : IDebug
	{
		private class InfoWrapper : IReference , IComparer<InfoWrapper>, IComparable<InfoWrapper>
		{
			public string Info;
			public EAssetFileLoadState LoadState;

			public void OnRelease()
			{
				Info = string.Empty;
				LoadState = EAssetFileLoadState.None;
			}
			public int CompareTo(InfoWrapper other)
			{
				return Compare(this, other);
			}
			public int Compare(InfoWrapper a, InfoWrapper b)
			{
				return string.CompareOrdinal(a.Info, b.Info);
			}
		}

		// 加载器总数
		private int _loaderTotalCount = 0;

		// 显示信息集合
		private List<InfoWrapper> _cacheInfos = new List<InfoWrapper>(1000);

		// 过滤的关键字
		private string _filterKey = string.Empty;

		// GUI相关
		private Vector2 _scrollPos = Vector2.zero;


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

			DebugConsole.GUILable($"加载器总数：{_loaderTotalCount}");

			_scrollPos = DebugConsole.GUIBeginScrollView(_scrollPos, 80);
			for (int i = 0; i < _cacheInfos.Count; i++)
			{
				var element = _cacheInfos[i];
				if (element.LoadState == EAssetFileLoadState.LoadAssetFileFailed)
					DebugConsole.GUIRedLable(element.Info);
				else
					DebugConsole.GUILable(element.Info);
			}
			DebugConsole.GUIEndScrollView();
		}
		private void FilterInfos()
		{
			// 回收引用
			ReferenceSystem.Release(_cacheInfos);

			// 清空列表
			_cacheInfos.Clear();

			// 绘制显示列表
			var fileLoaders = AssetSystem.DebugAllLoaders();
			_loaderTotalCount = fileLoaders.Count;
			foreach (var loader in fileLoaders)
			{
				// 只搜索关键字
				if (string.IsNullOrEmpty(_filterKey) == false)
				{
					if (loader.LoadPath.Contains(_filterKey) == false)
						continue;
				}

				string info = GameTools.Substring(loader.LoadPath, "/assets/", false);
				info = info.Replace(".unity3d", string.Empty);
				info = $"{info} = {loader.RefCount}";

				InfoWrapper element = ReferenceSystem.Spawn<InfoWrapper>();
				element.Info = info;
				element.LoadState = loader.LoadState;

				// 添加到显示列表
				_cacheInfos.Add(element);
			}

			// 重新排序
			_cacheInfos.Sort();
		}
	}
}