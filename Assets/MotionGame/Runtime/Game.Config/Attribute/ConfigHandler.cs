//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Utility;

namespace MotionFramework.Config
{
	internal class ConfigHandler
	{
		private static Dictionary<string, Type> _cfgTypes = new Dictionary<string, Type>();

		static ConfigHandler()
		{
			List<Type> result = AssemblyUtility.GetAssignableAttributeTypes(typeof(AssetConfig), typeof(ConfigAttribute));
			for (int i = 0; i < result.Count; i++)
			{
				Type type = result[i];

				// 判断是否重复
				ConfigAttribute attribute = (ConfigAttribute)Attribute.GetCustomAttribute(type, typeof(ConfigAttribute));
				if (_cfgTypes.ContainsKey(attribute.CfgType))
					throw new Exception($"Config {type} has same value : {attribute.CfgType}");

				// 添加到集合
				_cfgTypes.Add(attribute.CfgType, type);
			}
		}

		public static AssetConfig Handle(string configName)
		{
			AssetConfig config = null;

			Type type;
			if (_cfgTypes.TryGetValue(configName, out type))
				config = (AssetConfig)Activator.CreateInstance(type);

			if (config == null)
				throw new KeyNotFoundException($"AssetConfig {configName} is not define.");

			return config;
		}
	}
}