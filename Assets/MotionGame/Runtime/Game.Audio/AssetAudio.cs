//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using UnityEngine;
using MotionFramework.Resource;

namespace MotionFramework.Audio
{
	/// <summary>
	/// 音频资源类
	/// </summary>
	internal class AssetAudio : AssetObject
	{
		/// <summary>
		/// 标签
		/// </summary>
		public int AudioTag { private set; get; }

		/// <summary>
		/// 资源对象
		/// </summary>
		public AudioClip Clip { private set; get; }

		public AssetAudio(int audioTag)
		{
			AudioTag = audioTag;
		}
		protected override bool OnPrepare(UnityEngine.Object mainAsset)
		{
			if (base.OnPrepare(mainAsset) == false)
				return false;

			Clip = mainAsset as AudioClip;
			if (Clip == null)
				return false;

			return true;
		}
	}
}