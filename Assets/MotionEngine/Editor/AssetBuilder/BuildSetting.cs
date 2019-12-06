//--------------------------------------------------
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuildSetting : ScriptableObject
{
	/// <summary>
	/// 文件夹打包规则
	/// </summary>
	[Serializable]
	public enum EFolderPackRule
	{
		/// <summary>
		/// 收集该文件夹
		/// </summary>
		Collect,

		/// <summary>
		/// 忽略该文件夹
		/// </summary>
		Ignore,
	}

	/// <summary>
	/// AssetBundle命名规则
	/// </summary>
	[Serializable]
	public enum EBundleNameRule
	{
		/// <summary>
		/// 以文件路径命名
		/// </summary>
		ByFilePath,

		/// <summary>
		/// 以文件夹路径命名（该文件夹下所有文件被打到一个Bundle文件里）
		/// </summary>
		ByFolderPath,

		/// <summary>
		/// 以文件名称命名
		/// </summary>
		ByFileName,

		/// <summary>
		/// 以文件夹名称命名（该文件夹下所有文件被打到一个Bundle文件里）
		/// </summary>
		ByFolderName,
	}

	[Serializable]
	public class Wrapper
	{
		public string FolderPath = string.Empty;
		public EFolderPackRule PackRule = EFolderPackRule.Collect;
		public EBundleNameRule NameRule = EBundleNameRule.ByFilePath;
	}

	/// <summary>
	/// 打包的根路径
	/// </summary>
	[SerializeField]
	public Wrapper RootPath = new Wrapper();
	
	/// <summary>
	/// 附加的打包路径列表
	/// </summary>
	[SerializeField]
	public List<Wrapper> AddtionPaths = new List<Wrapper>();
}