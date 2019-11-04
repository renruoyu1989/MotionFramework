
定义状态类
```C#
using MotionEngine.AI;
using MotionGame;

public enum EFsmStateType
{
	Start,
	Running,
}

public class FsmStart : FsmState
{
	public FsmStart() : base((int)EFsmStateType.Start)
	{
	}
	public override void Enter()
	{
	}
	public override void Execute()
	{
		// 转换状态
		FsmManager.Instance.ChangeState((int)EFsmStateType.Running);
	}
	public override void Exit()
	{
	}
	public override void OnMessage(object msg)
	{
	}
}

public class FsmRunning : FsmState
{
	public FsmRunning() : base((int)EFsmStateType.Running)
	{
	}
	public override void Enter()
	{
	}
	public override void Execute()
	{
	}
	public override void Exit()
	{
	}
	public override void OnMessage(object msg)
	{
	}
}
```

创建状态类并设置状态机
```C#
using MotionEngine;
using MotionEngine.AI;
using MotionGame;

public class Test
{
	public void Start()
	{
		// 创建状态类
	 	FsmStart start = new FsmStart();
	 	FsmRunning running = new FsmRunning();

	 	// 设置转换关系
	 	start.AddChangeToState((int)EFsmStateType.Running);

	 	// 注册状态
	 	FsmManager.Instance.AddState(start)
	 	FsmManager.Instance.AddState(running);
	 	FsmManager.Instance.SetDefaultRunState((int)EFsmStateType.Start);
	}
}
```

更详细的教程请参考示例代码
1. [MotionGame/Runtime/Manager/FsmManager.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionGame/Runtime/Manager/FsmManager.cs)