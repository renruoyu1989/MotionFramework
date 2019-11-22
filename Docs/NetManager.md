
```C#
using MotionEngine.Net;

public class Test
{
	public void Start()
	{
		// 注意：NetProtoPackageParser是我们自定义的网络包解析器
		NetManager.Instance.ConnectServer("127.0.0.1", 10002, typeof(NetProtoPackageParser));
		NetManager.Instance.MonoProtoCallback += OnHandleMonoMsg;
	}

	public void Send()
	{
		// 在网络连接成功之后可以发送消息
		if(NetManager.Instance.State == ENetworkState.Connected)
		{
			int msgType = 1001;
			C2R_Login msg = new C2R_Login();
			msg.Account = "test";
			msg.Password = "1234567";
			NetManager.Instance.SendMsg(msgType, msg);
		}
	}

	private void OnHandleMonoMsg(NetReceivePackage package)
	{
		// 注意：NetReceivePackage是我们自定义网络包解析器里的接收类。
		Debug.Log($"Handle net message : {package.Type}");

		R2C_Login msg = package.ProtoObj as R2C_Login;
		if(msg != null)
		{
			Debug.Log(msg.Address);
			Debug.Log(msg.Key);
		}
	}
}
```

更详细的教程请参考示例代码
1. [MotionGame/Runtime/Manager/NetManager.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionGame/Runtime/Manager/NetManager.cs)
2. [MotionGame/Runtime/Game.Net/NetProtoPackageParser.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionGame/Runtime/Game.Net/NetProtoPackageParser.cs)