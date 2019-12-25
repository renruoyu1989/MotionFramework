﻿//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

namespace MotionFramework.AI
{
	public abstract class FsmState
	{
		/// <summary>
		/// 状态类型
		/// </summary>
		public int Type { get; private set; }

		/// <summary>
		/// 关系转换列表
		/// </summary>
		private readonly List<int> _changeToStates = new List<int>();

		/// <summary>
		/// 全局状态标签
		/// 说明：全局状态不需要添加可转换状态类型，可以任意跳转
		/// </summary>
		public bool IsGlobalState { get; set; } = false;

		public FsmState(int type)
		{
			Type = type;
		}
		public abstract void Enter();
		public abstract void Execute();
		public abstract void Exit();
		public virtual void OnMessage(object msg) { }

		/// <summary>
		/// 添加可转换状态类型
		/// </summary>
		public void AddChangeToState(int stateType)
		{
			if(_changeToStates.Contains(stateType) == false)
			{
				_changeToStates.Add(stateType);
			}
		}

		/// <summary>
		/// 检测是否可以转换到该状态
		/// </summary>
		public bool CanChangeTo(int stateType)
		{
			return _changeToStates.Contains(stateType);
		}
	}
}