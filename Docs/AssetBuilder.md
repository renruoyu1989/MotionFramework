
资源打包工具  
![image](https://github.com/gmhevinci/MotionFramework/raw/master/Docs/Image/img101.png)

**界面说明**  
```
Build Version : 补丁包的版本号
Build Pack Path : 打包路径，工具会以根路径内的资源为单位进行依赖分析并打包
Build Output Path : 打包完成后的输出路径（在工程目录下）。该路径无法修改！
Force Rebuild : 强制重建会删除当前平台下所有的补丁文件，并重新生成补丁文件

Compression : Assetbundle的压缩格式
Append Hash : 生成的AssetBundle文件名称添加Hash信息
Disable Write Type Tree : 禁止写入TypeTree，建议不勾选
Ignore Type Tree Chanages : 忽略TypeTree变化，建议勾选
```

**生成结果**  
生成成功后会在输出目录下找到新生成的补丁文件夹。  
![image](https://github.com/gmhevinci/MotionFramework/raw/master/Docs/Image/img106.png)

**Jenkins支持**
```
安卓打包调用静态方法：BuildPackage.BuildAndroid
苹果打包调用静态方法：BuildPackage.BuildIOS

命令行参数
"packPath=Assets/Works/MyResource"
"forceBuild=false"
"buildVersion=100"
```

更详细的教程请参考示例代码
1. [MotionGame/Editor/Builder/BuildPatch.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionGame/Editor/Builder/BuildPatch.cs)