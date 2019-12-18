//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

namespace MotionFramework.AI
{
	public class FsmSystem
	{
		/// <summary>
		/// 状态集合
		/// </summary>
		private readonly List<FsmState> _states = new List<FsmState>();

		/// <summary>
		/// 当前运行状态
		/// </summary>
		private FsmState _runState;

		/// <summary>
		/// 之前运行状态
		/// </summary>
		private FsmState _preState;


		/// <summary>
		/// 是否检测转换关系
		/// </summary>
		public bool IsCheckRelation { set; get; } = true;

		/// <summary>
		/// 当前运行的状态类型
		/// </summary>
		public int RunStateType
		{
			get { return _runState != null ? _runState.Type : 0; }
		}

		/// <summary>
		/// 之前运行的状态类型
		/// </summary>
		public int PreStateType
		{
			get { return _preState != null ? _preState.Type : 0; }
		}


		/// <summary>
		/// 启动状态机
		/// </summary>
		/// <param name="runStateType">初始状态类型</param>
		public void Run(int runStateType)
		{
			_runState = GetState(runStateType);
			_preState = GetState(runStateType);

			if (_runState != null)
				_runState.Enter();
			else
				LogSystem.Log(ELogType.Error, $"Not found run state : {runStateType}");
		}

		/// <summary>
		/// 更新状态机
		/// </summary>
		public void Update()
		{
			if (_runState != null)
				_runState.Execute();
		}

		/// <summary>
		/// 接收消息
		/// </summary>
		public void HandleMessage(object msg)
		{
			if (_runState != null)
				_runState.OnMessage(msg);
		}

		/// <summary>
		/// 添加一个状态节点
		/// </summary>
		public void AddState(FsmState state)
		{
			if (state == null)
				throw new ArgumentNullException();

			if (_states.Contains(state) == false)
			{
				_states.Add(state);
			}
			else
			{
				LogSystem.Log(ELogType.Warning, $"State {state.Type} already existed");
			}
		}

		/// <summary>
		/// 改变状态
		/// </summary>
		public void ChangeState(int stateType)
		{
			FsmState state = GetState(stateType);
			if (state == null)
			{
				LogSystem.Log(ELogType.Error, $"Can not found state {stateType}");
				return;
			}

			// 检测转换关系
			if (IsCheckRelation)
			{
				// 全局状态不需要检测转换关系
				if (_runState.IsGlobalState == false && state.IsGlobalState == false)
				{
					if (_runState.CanChangeTo(stateType) == false)
					{
						LogSystem.Log(ELogType.Error, $"Can not change state {_runState} to {state}");
						return;
					}
				}
			}

			LogSystem.Log(ELogType.Log, $"Change state {_runState} to {state}");
			_preState = _runState;
			_runState.Exit();
			_runState = state;
			_runState.Enter();
		}

		/// <summary>
		/// 返回之前状态
		/// </summary>
		public void RevertToPreState()
		{
			int stateType = _preState != null ? _preState.Type : 0;
			ChangeState(stateType);
		}


		/// <summary>
		/// 查询是否包含状态类型
		/// </summary>
		private bool IsContains(int stateType)
		{
			for (int i = 0; i < _states.Count; i++)
			{
				if (_states[i].Type == stateType)
					return true;
			}
			return false;
		}

		/// <summary>
		/// 获取状态类
		/// </summary>
		private FsmState GetState(int stateType)
		{
			for (int i = 0; i < _states.Count; i++)
			{
				if (_states[i].Type == stateType)
					return _states[i];
			}
			return null;
		}
	}
}
