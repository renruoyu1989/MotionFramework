定义类
```C#
using MotionEngine.Reference;

public class ReferClass : IReference
{
	public int Value = 0;

	// 在回收的时候该方法会被执行
	public void OnRelease()
	{
		Value = 0;
	}
}
```

单个回收范例
```C#
using MotionEngine.Reference;

public class Test
{
	public void Start()
	{
		// 获取对象
		ReferClass refer = ReferenceSystem.Spawn(typeof(ReferClass))
		// 回收对象
		ReferenceSystem.Release(refer)
	}
}
```

批量回收范例
```C#
using System.Collections.Generic;
using MotionEngine.Reference;

public class Test
{
	private List<ReferClass> _referList = new List<ReferClass>();
	private ReferClass[] _referArray = new ReferClass[10];

	public void Start()
	{
		// 回收列表对象集合
		ReferenceSystem.Release<ReferClass>(_referList)

		// 回收数组对象集合
		ReferenceSystem.Release<ReferClass>(_referArray)
	}
}
```