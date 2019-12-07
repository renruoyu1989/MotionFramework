//--------------------------------------------------
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 资源打包规则设置窗口
/// </summary>
public class BuildSettingWindow : EditorWindow
{
	static BuildSettingWindow _thisInstance;

	[MenuItem("MotionTools/Asset BuildSetting", false, 105)]
	static void ShowWindow()
	{
		if (_thisInstance == null)
		{
			_thisInstance = EditorWindow.GetWindow(typeof(BuildSettingWindow), false, "资源打包规则设置工具", true) as BuildSettingWindow;
			_thisInstance.minSize = new Vector2(800, 600);
		}

		_thisInstance.Show();
	}

	/// <summary>
	/// 上次打开的文件夹路径
	/// </summary>
	private string _lastOpenFolderPath = "Assets/";

	private GUIStyle _titleStyle;
	private void OnGUI()
	{
		// 根路径
		EditorGUILayout.Space();
		EditorGUILayout.LabelField($"Pack Root Path");
		EditorGUILayout.BeginHorizontal();
		{
			string folderPath = BuildSettingData.Setting.RootPath.FolderPath;
			BuildSetting.EFolderPackRule packRule = BuildSettingData.Setting.RootPath.PackRule;
			BuildSetting.EBundleNameRule nameRule = BuildSettingData.Setting.RootPath.NameRule;

			EditorGUILayout.LabelField(folderPath);

			BuildSetting.EFolderPackRule newPackRule = (BuildSetting.EFolderPackRule)EditorGUILayout.EnumPopup(packRule, GUILayout.MaxWidth(150));
			if (newPackRule != packRule)
				BuildSettingData.ModifyRootPath(folderPath, newPackRule, nameRule);

			BuildSetting.EBundleNameRule newNameRule = (BuildSetting.EBundleNameRule)EditorGUILayout.EnumPopup(nameRule, GUILayout.MaxWidth(150));
			if (newNameRule != nameRule)
				BuildSettingData.ModifyRootPath(folderPath, packRule, newNameRule);

			if (GUILayout.Button("Set", GUILayout.MaxWidth(40)))
			{
				string resultPath = EditorTools.OpenFolderPanel("+", folderPath);
				if (resultPath != null)
				{
					folderPath = EditorTools.AbsolutePathToAssetPath(resultPath);
					BuildSettingData.ModifyRootPath(folderPath, packRule, nameRule);
				}
			}
		}
		EditorGUILayout.EndHorizontal();

		// 列表显示
		EditorGUILayout.Space();
		EditorGUILayout.LabelField($"Pack Addtion Path List");
		for (int i = 0; i < BuildSettingData.Setting.AddtionPaths.Count; i++)
		{
			string folderPath = BuildSettingData.Setting.AddtionPaths[i].FolderPath;
			BuildSetting.EFolderPackRule packRule = BuildSettingData.Setting.AddtionPaths[i].PackRule;
			BuildSetting.EBundleNameRule nameRule = BuildSettingData.Setting.AddtionPaths[i].NameRule;

			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(folderPath);

				BuildSetting.EFolderPackRule newPackRule = (BuildSetting.EFolderPackRule)EditorGUILayout.EnumPopup(packRule, GUILayout.MaxWidth(150));
				if (newPackRule != packRule)
					BuildSettingData.ModifyElement(folderPath, newPackRule);

				BuildSetting.EBundleNameRule newNameRule = (BuildSetting.EBundleNameRule)EditorGUILayout.EnumPopup(nameRule, GUILayout.MaxWidth(150));
				if (newNameRule != nameRule)
					BuildSettingData.ModifyElement(folderPath, newNameRule);

				if (GUILayout.Button("-", GUILayout.MaxWidth(40)))
				{
					BuildSettingData.RemoveElement(folderPath);
					break;
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		// 添加按钮
		if (GUILayout.Button("+"))
		{
			string resultPath = EditorTools.OpenFolderPanel("+", _lastOpenFolderPath);
			if (resultPath != null)
			{
				_lastOpenFolderPath = EditorTools.AbsolutePathToAssetPath(resultPath);
				BuildSettingData.AddElement(_lastOpenFolderPath);
			}
		}
	}
}