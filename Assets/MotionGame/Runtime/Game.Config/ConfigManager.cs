﻿//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Resource;

namespace MotionFramework.Config
{
	/// <summary>
	/// 配表管理器
	/// </summary>
	public sealed class ConfigManager : IModule
	{
		public static readonly ConfigManager Instance = new ConfigManager();

		/// <summary>
		/// 配表集合
		/// </summary>
		private Dictionary<string, AssetConfig> _cfgs = new Dictionary<string, AssetConfig>();

		/// <summary>
		/// 配表文件夹的相对路径
		/// </summary>
		public string ParentFolderPath = "Config/";


		private ConfigManager()
		{
		}
		public void Awake()
		{
		}
		public void Start()
		{
		}
		public void Update()
		{
		}
		public void LateUpdate()
		{
		}
		public void OnGUI()
		{
		}

		/// <summary>
		/// 加载配表
		/// </summary>
		/// <param name="cfgName">配表文件名称</param>
		public void Load(string cfgName, System.Action<Asset> prepareCallback)
		{
			// 防止重复加载
			if (_cfgs.ContainsKey(cfgName))
			{
				LogSystem.Log(ELogType.Error, $"Config {cfgName} is already exist.");
				return;
			}

			AssetConfig config = ConfigHandler.Handle(cfgName);
			if (config != null)
			{
				_cfgs.Add(cfgName, config);
				config.Load(ParentFolderPath + cfgName, prepareCallback);
			}
			else
			{
				LogSystem.Log(ELogType.Error, $"Config {cfgName} calss is invalid.");
			}
		}

		/// <summary>
		/// 加载结果
		/// </summary>
		/// <param name="cfgName">配表文件名称</param>
		public EAssetResult Result(string cfgName)
		{
			if (_cfgs.ContainsKey(cfgName))
			{
				return _cfgs[cfgName].Result;
			}
			return EAssetResult.None;
		}

		/// <summary>
		/// 获取配表
		/// </summary>
		/// <param name="cfgName">配表文件名称</param>
		public AssetConfig GetConfig(string cfgName)
		{
			if (_cfgs.ContainsKey(cfgName))
			{
				return _cfgs[cfgName];
			}

			LogSystem.Log(ELogType.Error, $"Not found config {cfgName}");
			return null;
		}
	}
}