
音频管理器使用范例
```C#
using MotionGame;

public class Test
{
	public void Start()
	{
		// 播放短音效
		AudioManager.Instance.PlaySound("UISound/click");

		// 播放背景音乐
		bool loop = true;
		AudioManager.Instance.PlayMusic("Music/cityBgMusic", loop);
	}
}
```

更详细的教程请参考示例代码
1. [MotionGame/Runtime/Manager/AudioManager.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionGame/Runtime/Manager/AudioManager.cs)