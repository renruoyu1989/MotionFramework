using MotionEngine;
using MotionEngine.AI;

namespace MotionGame
{
	/// <summary>
	/// 状态机管理器
	/// </summary>
	public sealed class FsmManager : IModule
	{
		public static readonly FsmManager Instance = new FsmManager();

		/// <summary>
		/// 状态机系统
		/// </summary>
		public readonly FsmSystem System = new FsmSystem();

		/// <summary>
		/// 初始运行状态
		/// </summary>
		private int _runState;


		private FsmManager()
		{
		}
		public void Awake()
		{
		}
		public void Start()
		{
			System.Run(_runState);
		}
		public void Update()
		{
			System.Update();
		}
		public void LateUpdate()
		{
		}
		public void OnGUI()
		{
			Engine.GUILable($"FSM : {System.RunStateType}");
		}

		/// <summary>
		/// 添加一个状态节点
		/// </summary>
		public void AddState(FsmState state)
		{
			System.AddState(state);
		}

		/// <summary>
		/// 改变状态机状态
		/// </summary>
		public void ChangeState(int stateType)
		{
			System.ChangeState(stateType);
		}

		/// <summary>
		/// 设置初始运行状态
		/// </summary>
		public void SetDefaultRunState(int runStateType)
		{
			_runState = runStateType;
		}
	}
}