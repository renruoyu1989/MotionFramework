
定义网络包解析器
```C#
using System;
using System.Collections;
using System.Collections.Generic;
using MotionEngine.Net;

namespace MotionGame
{
  public class NetCustomPackageParser : NetPackageParser
  {
    public override void Encode(System.Object msg)
    {
      //网络消息编码
    }
    public override void Decode(List<System.Object> msgList)
    {
      //网络消息解码
    }
  }
}
```

注册网络包解析器
```C#
void Start
{
  NetManager.Instance.SetPackageParseType(typeof(NetCustomPackageParser));
}
```

更详细的教程请参考示例代码
1. [MotionGame/Runtime/Game.Net/NetProtoPackageParser.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionGame/Runtime/Game.Net/NetProtoPackageParser.cs)
