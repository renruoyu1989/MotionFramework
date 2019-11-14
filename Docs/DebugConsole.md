
控制台系统可以帮助开发者在游戏运行时显示一些关键数据或帮助调试游戏。

框架默认自带几个控制台节点：模块信息，日志列表，资源列表
![image](https://github.com/gmhevinci/MotionFramework/raw/master/Docs/Image/img2.png)  

定义自己的控制台节点  
```C#
using MotionEngine;
using MotionEngine.Debug;

namespace MotionGame
{
	[DebugAttribute("控制台节点显示名称", 201)]
	public class CustomModule : IDebug
	{
		public void OnInit()
		{
		}
		public void OnGUI()
		{
			DebugConsole.GUILable("在这里编写GUI代码");
		}
	}
}
```

更详细的教程请参考示例代码
1. [MotionEngine/Runtime/Game.Debug](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionGame/Runtime/Game.Debug)