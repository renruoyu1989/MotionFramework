﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionEngine;
using MotionEngine.Debug;
using MotionEngine.Reference;

namespace MotionGame
{
	[DebugAttribute("日志", 101)]
	public class DebugLog : IDebug
	{
		private class LogWrapper : IReference
		{
			public LogType Type;
			public string Log;
			public void OnRelease()
			{
				Log = string.Empty;
			}
		}

		/// <summary>
		/// 日志最大显示数量
		/// </summary>
		private const int LOG_MAX_COUNT = 500;

		/// <summary>
		/// 日志集合
		/// </summary>
		private List<LogWrapper> _logs = new List<LogWrapper>();

		// GUI相关
		private bool _showLog = true;
		private bool _showWarning = true;
		private bool _showError = true;
		private Vector2 _scrollPos = Vector2.zero;

		public void OnInit()
		{
			// 注册UnityEngine日志系统
			Application.logMessageReceived += HandleUnityEngineLog;
		}
		public void OnGUI()
		{
			GUILayout.BeginHorizontal();
			_showLog = DebugConsole.GUIToggle("Log", _showLog);
			_showWarning = DebugConsole.GUIToggle("Warning", _showWarning);
			_showError = DebugConsole.GUIToggle("Error", _showError);
			GUILayout.EndHorizontal();

			_scrollPos = DebugConsole.GUIBeginScrollView(_scrollPos, 40);
			for (int i = 0; i < _logs.Count; i++)
			{
				LogWrapper wrapper = _logs[i];
				if (wrapper.Type == LogType.Log)
				{
					if (_showLog)
						DebugConsole.GUILable(wrapper.Log);
				}
				else if (wrapper.Type == LogType.Warning)
				{
					if (_showWarning)
						DebugConsole.GUIYellowLable(wrapper.Log);
				}
				else
				{
					if (_showError)
						DebugConsole.GUIRedLable(wrapper.Log);
				}
			}
			DebugConsole.GUIEndScrollView();
		}

		private void HandleUnityEngineLog(string logString, string stackTrace, LogType type)
		{
			LogWrapper wrapper = ReferenceSystem.Spawn<LogWrapper>();
			wrapper.Type = type;
			wrapper.Log = $"[{type}] {logString}";
			_logs.Add(wrapper);

			if (_logs.Count > LOG_MAX_COUNT)
			{
				ReferenceSystem.Release(_logs[0]);
				_logs.RemoveAt(0);
			}
		}
	}
}