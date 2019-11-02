# MotionFramework
MotionFramework是一套基于Unity3D引擎的游戏框架。框架整体遵循**轻量化、易用性、低耦合、扩展性强**的设计理念。工程结构清晰，代码注释详细，是作为无框架经验的公司、独立开发者、以及初学者们推荐的游戏框架。

![image](https://github.com/gmhevinci/MotionFramework/raw/master/Docs/Image/img1.png)

## 支持的Unity版本
Unity2017.4 && Unity2018.4

## MotionEngine.Runtime
MotionFramework核心代码

1. **Base**  
[核心部分](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Module.md)

2. **Engine.AI**  
AI模块：有限状态机。

3. **Engine.Event**  
[事件模块](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/EngineEvent.md)

4. **Engine.IO**  
IO模块

5. **Engine.Net**  
[网络模块](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/EngineNet.md)：异步IOCP SOCKET支持高并发，自定义协议解析器。

6. **Engine.Patch**  
补丁模块

7. **Engine.Res**  
[资源模块](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/EngineRes.md)：基于引用计数的资源系统，基于面向对象的资源加载方式。

8. **Engine.Utility**  
工具模块

## MotionEngine.Editor
扩展的相关工具

1. **AssetBrowser**  
资源对象总览工具

2. **AssetBuilder**  
资源打包工具

3. **AssetImporter**  
资源导入工具

4. **AssetSearch**  
资源引用搜索工具

## MotionGame.Runtime
这里已经内置了游戏开发过程中常用的管理器：AudioManager声音管理器, CfgManager配表管理器, EventManager事件管理器, FsmManager状态机管理器, NetManager网络管理器, PoolManager对象池管理器, ResManager资源管理器，ILRManager热更管理器。  

其中引入了ILRuntime库，来支持使用C#脚本编写游戏业务逻辑并实现热更新。自定义的网络包解析器使用protobuf库来做包体序列化，并可以和ET 5.0服务器通信。
