//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionFramework.Resource
{
	/// <summary>
	/// 资源加载器基类
	/// </summary>
	public abstract class AssetFileLoader
	{
		/// <summary>
		/// 引用计数
		/// </summary>
		public int RefCount { get; private set; }

		/// <summary>
		/// 是否是流场景资源
		/// </summary>
		public bool IsStreamScene { get; private set; }

		/// <summary>
		/// 加载路径
		/// </summary>
		public string LoadPath { get; private set; }

		/// <summary>
		/// 加载状态
		/// </summary>
		public EAssetFileLoadState LoadState { get; protected set; }

		/// <summary>
		/// 完成回调
		/// </summary>
		public System.Action<AssetFileLoader> LoadCallback { get; set; }


		public AssetFileLoader(bool isStreamScene, string loadPath)
		{
			RefCount = 0;
			IsStreamScene = isStreamScene;
			LoadPath = loadPath;
			LoadState = EAssetFileLoadState.None;
			LoadCallback = null;
		}

		/// <summary>
		/// 轮询更新方法
		/// </summary>
		public abstract void Update();

		/// <summary>
		/// 加载主资源对象
		/// </summary>
		public abstract void LoadMainAsset(System.Type assetType, System.Action<UnityEngine.Object> callback);
		
		/// <summary>
		/// 引用接口（引用计数递加）
		/// </summary>
		public virtual void Reference()
		{
			RefCount++;
		}

		/// <summary>
		/// 释放接口（引用计数递减）
		/// </summary>
		public virtual void Release()
		{
			RefCount--;
		}

		/// <summary>
		/// 卸载接口
		/// </summary>
		public virtual void UnLoad(bool force)
		{
		}

		/// <summary>
		/// 是否完毕（无论成功失败）
		/// </summary>
		public virtual bool IsDone()
		{
			return LoadState == EAssetFileLoadState.LoadAssetFileOK || LoadState == EAssetFileLoadState.LoadAssetFileFailed;
		}
	}
}