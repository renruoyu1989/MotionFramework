//--------------------------------------------------
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class BuildSettingData
{
	public static BuildSetting Setting;

	static BuildSettingData()
	{
		// 加载配置文件
		Setting = AssetDatabase.LoadAssetAtPath<BuildSetting>(EditorDefine.BuilderSettingFilePath);
		if (Setting == null)
		{
			Debug.LogWarning($"Create new BuildSetting.asset : {EditorDefine.BuilderSettingFilePath}");
			Setting = ScriptableObject.CreateInstance<BuildSetting>();
			EditorTools.CreateFileDirectory(EditorDefine.BuilderSettingFilePath);
			AssetDatabase.CreateAsset(Setting, EditorDefine.BuilderSettingFilePath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
		else
		{
			Debug.Log("Load BuildSetting.asset ok");
		}
	}

	/// <summary>
	/// 存储文件
	/// </summary>
	public static void SaveFile()
	{
		if (Setting != null)
		{
			EditorUtility.SetDirty(Setting);
			AssetDatabase.SaveAssets();
		}
	}

	/// <summary>
	/// 添加元素
	/// </summary>
	public static void AddElement(string folderPath)
	{
		if (IsContainsElement(folderPath) == false)
		{
			BuildSetting.Wrapper element = new BuildSetting.Wrapper();
			element.FolderPath = folderPath;
			Setting.AddtionPaths.Add(element);
			SaveFile();
		}
	}

	/// <summary>
	/// 移除元素
	/// </summary>
	public static void RemoveElement(string folderPath)
	{
		for (int i = 0; i < Setting.AddtionPaths.Count; i++)
		{
			if (Setting.AddtionPaths[i].FolderPath == folderPath)
			{
				Setting.AddtionPaths.RemoveAt(i);
				break;
			}
		}
		SaveFile();
	}

	/// <summary>
	/// 编译根路径
	/// </summary>
	public static void ModifyRootPath(string folderPath, BuildSetting.EFolderPackRule packRule, BuildSetting.EBundleNameRule nameRule)
	{
		Setting.RootPath.FolderPath = folderPath;
		Setting.RootPath.PackRule = packRule;
		Setting.RootPath.NameRule = nameRule;
	}

	/// <summary>
	/// 编译元素
	/// </summary>
	public static void ModifyElement(string folderPath, BuildSetting.EFolderPackRule packRule)
	{
		for (int i = 0; i < Setting.AddtionPaths.Count; i++)
		{
			if (Setting.AddtionPaths[i].FolderPath == folderPath)
			{
				Setting.AddtionPaths[i].PackRule = packRule;
				break;
			}
		}
		SaveFile();
	}

	/// <summary>
	/// 编译元素
	/// </summary>
	public static void ModifyElement(string folderPath, BuildSetting.EBundleNameRule nameRule)
	{
		for (int i = 0; i < Setting.AddtionPaths.Count; i++)
		{
			if (Setting.AddtionPaths[i].FolderPath == folderPath)
			{
				Setting.AddtionPaths[i].NameRule = nameRule;
				break;
			}
		}
		SaveFile();
	}

	/// <summary>
	/// 是否包含元素
	/// </summary>
	public static bool IsContainsElement(string folderPath)
	{
		for (int i = 0; i < Setting.AddtionPaths.Count; i++)
		{
			if (Setting.AddtionPaths[i].FolderPath == folderPath)
				return true;
		}
		return false;
	}
}