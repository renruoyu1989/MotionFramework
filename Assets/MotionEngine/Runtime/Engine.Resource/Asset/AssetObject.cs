//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using UnityEngine;

namespace MotionFramework.Resource
{
	public class AssetObject : Asset
	{
		// 主资源对象
		private UnityEngine.Object _mainAsset;

		protected override bool OnPrepare(UnityEngine.Object asset)
		{
			_mainAsset = asset;
			return true;
		}

		/// <summary>
		/// 获取主资源对象
		/// </summary>
		public UnityEngine.Object GetMainAsset()
		{
			return _mainAsset;
		}

		/// <summary>
		/// 获取主资源对象
		/// UnityEngine.TextAsset
		/// UnityEngine.AudioClip
		/// UnityEngine.Texture
		/// UnityEngine.Sprite
		/// UnityEngine.U2D.SpriteAtlas
		/// UnityEngine.Video.VideoClip
		/// UnityEngine.GameObject
		/// UnityEngine.Font
		/// UnityEngine.Shader
		/// UnityEngine.Material
		/// </summary>
		public T GetMainAsset<T>() where T : UnityEngine.Object
		{
			if (_mainAsset is GameObject)
			{
				GameObject go = Object.Instantiate(_mainAsset) as GameObject;
				return go as T;
			}

			return _mainAsset as T;
		}
	}
}