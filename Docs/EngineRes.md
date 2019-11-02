
资源使用范例
```C#
private AssetObject _model;

void Start()
{
  // 加载模型
  _model = new AssetObject();
  _model.Load("Model/npc001", OnModelLoad);
}

void OnDestroy()
{
  // 卸载模型
  if(_model != null)
  {
    _model.UnLoad();
    _model = null;
  }
}

private void OnModelLoad(Asset asset, EAssetResult result)
{
  if (result != EAssetResult.OK)
    return;
  
  // 模型已经加载完毕，我们可以在这里做任何处理
  _model.GameObj.transform.position = Vector3.zero;
}
```

更详细的教程请参考示例代码
1. [MotionEngine/Runtime/Engine.Res/Asset](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionEngine/Runtime/Engine.Res/Asset)
