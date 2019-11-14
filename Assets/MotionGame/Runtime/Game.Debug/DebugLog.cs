﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionEngine;
using MotionEngine.Debug;

namespace MotionGame
{
	[DebugAttribute("日志列表", 102)]
	public class DebugLog : IDebug
	{
		private class LogWrapper
		{
			public LogType Type;
			public string Log;
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


		public void OnInit()
		{
			// 注册MotionEngine日志系统
			LogSystem.RegisterCallback(HandleMotionEngineLog);

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
		}

		private void HandleUnityEngineLog(string logString, string stackTrace, LogType type)
		{
			LogWrapper wrapper = new LogWrapper();
			wrapper.Type = type;
			wrapper.Log = $"[{type}] {logString}";

			_logs.Add(wrapper);
			if (_logs.Count > LOG_MAX_COUNT)
				_logs.RemoveAt(0);
		}
		private void HandleMotionEngineLog(ELogType logType, string log)
		{
			if (logType == ELogType.Log)
			{
				UnityEngine.Debug.Log(log);
			}
			else if (logType == ELogType.Error)
			{
				UnityEngine.Debug.LogError(log);
			}
			else if (logType == ELogType.Warning)
			{
				UnityEngine.Debug.LogWarning(log);
			}
			else if (logType == ELogType.Exception)
			{
				UnityEngine.Debug.LogError(log);
			}
			else
			{
				throw new NotImplementedException($"{logType}");
			}
		}
	}
}