### 网络系统 (Network)

定义网络包解析器
```C#
using System;
using MotionFramework.Network;

public class CustomPackageParser : NetPackageParser
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
```