### 资源系统 (AssetSystem)

**资源系统加载模式**  
资源系统提供三种加载模式
1. Editor Mode : 使用UnityEditor.AssetDatabase加载资源，该模式仅支持编辑器下。
2. Resource Mode : 使用UnityEngine.Resources加载资源。
3. Bundle Mode : 使用UnityEngine.AssetBundle加载资源。

```C#
using MotionFramework.Resource;

public class Test
{
	public void Start()
	{
		// 设置资源系统加载模式
		AssetSystem.AssetLoadMode = EAssetLoadMode.EditorMode;
	}
}
```

**资源加载**  
资源分为三类
1. AssetObject : 只能获取AssetBundle里的主资源对象
2. AssetPackage : 可以获取AssetBundle里所有资源对象
3. AssetScene : 可以加载流场景文件
````C#
// AssetObject
AssetObject assetObject = new AssetObject();
assetObject.Load("Audio/bgMusic", OnAssetLoad);
...
AudioClip audioClip = assetObject.GetMainAsset<AudioClip>();
AudioClip audioClip = assetObject.GetMainAsset() as AudioClip;
````

````C#
// AssetPackage
AssetPackage assetPackage = new AssetPackage();
assetPackage.Load("Audio/heroSound", OnAssetLoad);
...
// 从资源包里加载出生音效
assetPackage.GetAsset("bornSound", typeof(AudioClip), (UnityEngine.Object asset) => 
{
	AudioClip audioClip = asset as AudioClip;
});
// 从资源包里加载死亡音效
assetPackage.GetAsset("deadSound", typeof(AudioClip), (UnityEngine.Object asset) => 
{
	AudioClip audioClip = asset as AudioClip;
});
````

````C#
// AssetScene
// 请参考SceneManager.cs
````

**资源系统根路径**  
所有通过代码加载的资源文件都需要放在资源系统根路径下，在加载这些资源的时候只需要提供相对路径即可。  

```C#
using MotionFramework.Resource;

public class Test
{
	public void Start()
	{
		// 设置资源系统根路径
		AssetSystem.AssetRootPath = "Assets/Works/MyResource";
	}
}
```

**AssetBundle**  
在使用AssetBundle加载模式的时候，我们需要设置AssetSystem.BundleMethod接口，这个接口主要是提供了资源间依赖关系的查询工作。我们可以使用官方提供的AssetBundleManifest文件，也可以使用自己的依赖关系文件。  

定义Bundle接口
```C#
using MotionFramework.Resource;
using UnityEngine;

public class PatchBundleMethod : IBundleMethod
{
	private AssetBundleManifest _manifest;

	public void LoadManifestFile()
	{
		string loadPath = "manifest file path"; //可能从沙盒内加载或者从流文件夹内加载
		AssetBundle bundle = AssetBundle.LoadFromFile(loadPath);
		if (bundle == null)
			throw new System.Exception($"AssetBundleManifest file load failed : {loadPath}");

		_manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
		if(_manifest == null)
			throw new System.Exception("AssetBundleManifest object load failed.");

		// 最后卸载AssetBundle
		bundle.Unload(false);
	}
	public string GetAssetBundleLoadPath(string manifestPath)
	{
		// 可能从沙盒内加载或者从流文件夹内加载
		// 范例代码统一从流文件夹内加载
		return AssetPathHelper.MakeStreamingLoadPath(manifestPath);
	}
	public string[] GetDirectDependencies(string assetBundleName)
	{
		return _manifest.GetDirectDependencies(assetBundleName);
	}
	public string[] GetAllDependencies(string assetBundleName)
	{
		return _manifest.GetAllDependencies(assetBundleName);
	}
}
```

设置Bundle接口
```C#
using MotionFramework.Resource;

public class Test
{
	public void Start()
	{
		PatchBundleMethod method = new PatchBundleMethod();
		method.LoadManifestFile();

		// 设置Bundle接口
		AssetSystem.BundleMethod = method;
	}
}
```