# MotionFramework
MotionFramework是一个基于Unity3D引擎的游戏框架。框架采用模块化注册管理，框架整体保持轻量化、扩展性、低耦合、易用性的特点，代码结构简单清晰，没有设计模式泛滥，方便学习和使用。

## 支持的Unity版本
Unity2017.4 && Unity2018.4

## MotionEngine.Runtime
由多个模块组成，每个模块互相独立，开发者可以灵活选择需要的模块。

1. **Base**  
核心部分

2. **Engine.AI**  
AI模块：有限状态机。

3. **Engine.Event**  
事件模块

4. **Engine.IO**  
IO模块

5. **Engine.Net**  
网络模块：异步IOCP SOCKET支持高并发，自定义协议解析器。

6. **Engine.Patch**  
补丁模块

7. **Engine.Res**  
资源模块：基于引用计数的资源系统，基于面向对象的资源加载方式。

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
内含AudioManager, CfgManager, EventManager, FsmManager, NetManager, PoolManager, ResManager，ILRManager。
其中新引入了ILRuntime库，来支持C#编写业务逻辑并实现热更新。协议解析器使用protobuf库来做包体序列化，并可以和ET 5.0服务器通信。
