### 资源系统 (AssetSystem)

**资源系统加载模式**  
资源系统提供三种加载模式  
Editor Mode : 使用UnityEditor.AssetDatabase加载资源，该模式只支持编辑器下。  
Resource Mode : 使用UnityEngine.Resources加载资源。  
Bundle Mode : 使用UnityEngine.AssetBundle加载资源。  

在GameLauncher预制体里提供了快速设置方式
![image](https://github.com/gmhevinci/MotionFramework/raw/master/Docs/Image/img3.png)  

**资源系统根路径**  
所有通过代码加载的资源文件都需要放在资源系统根路径下，在加载这些资源的时候只需要提供相对路径即可。  

```C#
using MotionEngine.Res;

public class Test
{
	public void Start()
	{
		// 设置资源系统加载模式
		AssetSystem.AssetLoadMode = EAssetLoadMode.EditorMode;

		// 设置资源系统根路径
		AssetSystem.AssetRootPath = "Assets/Works/MyResource";
	}
}
```

**AssetBundle**  
在使用AssetBundle加载模式的时候，我们需要设置AssetSystem.BundleMethod接口，这个接口主要是提供了资源间依赖关系的查询工作。我们可以使用官方提供的AssetBundleManifest文件，也可以使用自己的依赖关系文件。  

```C#
using MotionEngine.Res;
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