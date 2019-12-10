### 资源管理器 (ResourceManager)

资源使用范例
```C#
using MotionFramework.Resource;

public class Test
{
  private AssetObject _model;

  public void Start()
  {
    // 加载模型
    _model = new AssetObject();
    _model.Load("Model/npc001", OnAssetPrepare);
  }

  public void OnDestroy()
  {
    // 卸载模型
    if(_model != null)
    {
      _model.UnLoad();
      _model = null;
    }
  }

  private void OnAssetPrepare(Asset asset)
  {
    if (asset.Result != EAssetResult.OK)
      return;
    
    // 模型已经加载完毕，我们可以在这里做任何处理
    GameObject go = _model.GetMainAsset<GameObject>();
    go.transform.position = Vector3.zero;
  }
}
```

更详细的教程请参考示例代码
1. [MotionEngine/Runtime/Engine.Res/Asset](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionEngine/Runtime/Engine.Resource/Asset)
