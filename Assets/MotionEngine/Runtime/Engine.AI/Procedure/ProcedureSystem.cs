using System.Collections.Generic;

namespace MotionEngine.AI
{
	/// <summary>
	/// 流程系统
	/// </summary>
	public class ProcedureSystem
	{
		/// <summary>
		/// 类型列表
		/// </summary>
		private List<int> _types = new List<int>();

		/// <summary>
		/// 状态机系统
		/// </summary>
		private FsmSystem _system = new FsmSystem();


		public ProcedureSystem()
		{
			// 流程系统不用检测转换关系
			_system.IsCheckRelation = false;
		}

		/// <summary>
		/// 运行流程系统
		/// </summary>
		public void Run()
		{
			if (_types.Count > 0)
				_system.Run(_types[0]);
			else
				LogSystem.Log(ELogType.Warning, "Procedure system dont has any state.");
		}

		/// <summary>
		/// 更新流程系统
		/// </summary>
		public void Update()
		{
			_system.Update();
		}

		/// <summary>
		/// 添加一个流程
		/// 注意添加的先后顺序
		/// </summary>
		public void AddProcedure(FsmState procedure)
		{
			_types.Add(procedure.Type);
			_system.AddState(procedure);
		}

		/// <summary>
		/// 当前流程类型
		/// </summary>
		public int CurrentProcedure()
		{
			return _system.RunStateType;
		}

		/// <summary>
		/// 切换流程
		/// </summary>
		public void SwitchProcedure(int procedure)
		{
			_system.ChangeState(procedure);
		}

		/// <summary>
		/// 切换至下一流程
		/// </summary>
		public void SwitchNextProcedure()
		{
			int index = _types.IndexOf(_system.RunStateType);
			if (index >= _types.Count - 1)
			{
				LogSystem.Log(ELogType.Warning, $"Current procedure {_system.RunStateType} is the final procedure.");
			}
			else
			{
				SwitchProcedure(_types[index + 1]);
			}
		}

		/// <summary>
		/// 切换至上一流程
		/// </summary>
		public void SwitchLastProcedure()
		{
			int index = _types.IndexOf(_system.RunStateType);
			if (index <= 0)
			{
				LogSystem.Log(ELogType.Warning, $"Current procedure {_system.RunStateType} is the first procedure.");
			}
			else
			{
				SwitchProcedure(_types[index - 1]);
			}
		}
	}
}