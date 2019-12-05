using System;
using System.Security;
using Mono.Xml;
using MotionEngine;
using MotionEngine.Res;
using UnityEngine;

namespace MotionGame
{
	public abstract class AssetXml : AssetObject
	{
		protected SecurityElement _xml;


		protected override bool OnPrepare(UnityEngine.Object mainAsset)
		{
			if (base.OnPrepare(mainAsset) == false)
				return false;

			TextAsset temp = mainAsset as TextAsset;
			if (temp == null)
				return false;

			SecurityParser sp = new SecurityParser();
			sp.LoadXml(temp.text);
			_xml = sp.ToXml();

			if (_xml == null)
			{
				LogSystem.Log(ELogType.Error, $"SecurityParser.LoadXml failed. {ResName}");
				return false;
			}

			try
			{
				// 解析数据
				ParseData();
			}
			catch (Exception ex)
			{
				LogSystem.Log(ELogType.Error, $"Failed to parse xml {ResName}. Exception : {ex.ToString()}");
				return false;
			}

			// 注意：为了节省内存这里立即释放了资源
			UnLoad();

			return true;
		}

		/// <summary>
		/// 序列化数据的接口
		/// </summary>
		protected abstract void ParseData();
	}
}