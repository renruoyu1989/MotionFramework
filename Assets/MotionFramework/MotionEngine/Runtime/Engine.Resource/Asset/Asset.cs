//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionFramework.Resource
{
	public abstract class Asset
	{
		/// <summary>
		/// 资源名称
		/// 注意：资源在工程内相对于AssetSystem.AssetRootPath的相对路径
		/// </summary>
		public string ResName { private set; get; }

		/// <summary>
		/// 最终结果
		/// </summary>
		public EAssetResult Result { private set; get; }

		/// <summary>
		/// 用户回调
		/// </summary>
		private System.Action<Asset> _userCallback;

		/// <summary>
		/// 缓存的加载器
		/// </summary>
		protected AssetFileLoader _cacheLoader;


		/// <summary>
		/// 异步加载
		/// </summary>
		public void Load(string resName, System.Action<Asset> userCallbcak)
		{
			// 防止重复加载
			if (Result != EAssetResult.None)
			{
				LogSystem.Log(ELogType.Warning, $"Asset {ResName} is already load.");
				return;
			}

			if (_cacheLoader != null)
			{
				LogSystem.Log(ELogType.Warning, $"Asset  {ResName}  loader must null.");
				return;
			}

			ResName = resName;
			Result = EAssetResult.Loading;
			_userCallback = userCallbcak;
			bool isStreamScene = this is AssetScene;
			_cacheLoader = AssetSystem.LoadAssetFile(ResName, isStreamScene, OnAssetFileLoad);
		}

		/// <summary>
		/// 卸载
		/// </summary>
		public void UnLoad()
		{
			Result = EAssetResult.None;
			_userCallback = null;
			if (_cacheLoader != null)
			{
				_cacheLoader.Release();
				_cacheLoader = null;
			}
		}

		/// <summary>
		/// 是否加载完毕（无论成功失败）
		/// </summary>
		public bool IsLoadDone()
		{
			return Result == EAssetResult.Failed || Result == EAssetResult.OK;
		}


		/// <summary>
		/// 准备过程
		/// </summary>
		/// <param name="mainAsset">主资源对象</param>
		protected virtual bool OnPrepare(UnityEngine.Object mainAsset)
		{
			return true;
		}

		// 当资源文件加载完毕
		private void OnAssetFileLoad(AssetFileLoader loader)
		{
			// 注意 : 如果在加载过程中调用UnLoad，等资源文件加载完毕时不再执行后续准备工作。
			if (Result != EAssetResult.Loading)
				return;

			// Check error
			if (loader.LoadState != EAssetFileLoadState.LoadAssetFileOK)
			{
				Result = EAssetResult.Failed;
				_userCallback?.Invoke(this);
				return;
			}

			if (this is AssetScene || this is AssetPackage)
			{
				bool result = OnPrepare(null);
				Result = result ? EAssetResult.OK : EAssetResult.Failed;
				_userCallback?.Invoke(this);
			}
			else if (this is AssetObject)
			{
				loader.LoadMainAsset(null, OnMainAssetLoad);
			}
			else
			{
				throw new System.NotImplementedException($"Not support invalid asset class.");
			}
		}

		// 当主资源对象加载完毕
		private void OnMainAssetLoad(UnityEngine.Object mainAsset)
		{
			// 注意 : 如果在加载过程中调用UnLoad，等资源对象加载完毕时不再执行后续准备工作。
			if (Result != EAssetResult.Loading)
				return;

			// Check result
			if (mainAsset == null)
			{
				Result = EAssetResult.Failed;
			}
			else
			{
				bool result = OnPrepare(mainAsset);
				Result = result ? EAssetResult.OK : EAssetResult.Failed;
			}

			_userCallback?.Invoke(this);
		}
	}
}