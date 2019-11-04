
定义事件类
```C#
using MotionEngine.Event;

public class TestEventMsg : IEventMessage
{
  public string Value;
}
```

监听事件
```C#
using UnityEngine;
using MotionEngine.Event;
using MotionGame;

public class Test
{
  public void Start()
  {
    EventManager.Instance.AddListener("customEventTag", OnHandleEventMsg);
  }

  private void OnHandleEventMsg(IEventMessage msg)
  {
    if(msg is TestEventMsg)
    {
      TestEventMsg temp = msg as TestEventMsg;
      Debug.Log($"{temp.Value}");
    }
  }
}
```

发送事件
```C#
using UnityEngine;
using MotionEngine.Event;
using MotionGame;

public class Test
{
  public void Start()
  {
    TestEventMsg msg = new TestEventMsg()
    {
      Value = $"hello world",
    };
    EventManager.Instance.Send("customEventTag", msg);
  }
}
```

更详细的教程请参考示例代码
1. [MotionGame/Runtime/_Script_/GameTest.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionGame/Runtime/_Script_/GameTest.cs)
2. [MotionGame/Runtime/_Script_/Event](https://github.com/gmhevinci/MotionFramework/tree/master/Assets/MotionGame/Runtime/_Script_/Event)
