### 配表管理器 (ConfigManager)

[FlashExcel](https://github.com/gmhevinci/FlashExcel)导表工具会自动生成表格相关的CS脚本和二进制数据文件
```C#
using MotionFramework.Config;

// 这里扩展了获取数据的方法
public partial class CfgHero
{
	public static CfgHeroTab GetCfgTab(int key)
	{
		CfgHero cfg = ConfigManager.Instance.GetConfig(EConfigType.Hero.ToString()) as CfgHero;
		return cfg.GetTab(key) as CfgHeroTab;
	}
}
```

加载表格
```C#
using MotionFramework.Config;

public class Test
{
	public void Start()
	{
		// 优先加载多语言表
		ConfigManager.Instance.Load("AutoGenerateLanguage", OnLanguagePrepare);	
	}

	private void OnLanguagePrepare(Asset asset, EAssetResult result)
	{
		if (result != EAssetResult.OK)
			return;

		// 多语言表加载完毕后，加载剩余其它表格
		ConfigManager.Instance.Load("Hero", OnConfigPrepare);
	}
	
	private void OnConfigPrepare(Asset asset, EAssetResult result)
	{
		if (result != EAssetResult.OK)
			return;

		// 打印表格数据
		if (asset is CfgHero)
		{
			CfgHeroTab tab1 = CfgHero.GetCfgTab(1001);
			Debug.Log($"{tab1.Name}");
			CfgHeroTab tab2 = CfgHero.GetCfgTab(1002);
			Debug.Log($"{tab2.Name}");
		}
	}
}
```