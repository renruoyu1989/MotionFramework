using System;
using MotionEngine;
using MotionEngine.Res;

namespace MotionGame
{
	/// <summary>
	/// 资源管理器
	/// </summary>
	public sealed class ResManager : IModule
	{
		public static readonly ResManager Instance = new ResManager();


		private ResManager()
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
			AssetSystem.UpdatePoll();
		}
		public void LateUpdate()
		{
			AssetSystem.Release();
		}
		public void OnGUI()
		{
			int totalCount = AssetSystem.GetFileLoaderCount();
			int failedCount = AssetSystem.GetFileLoaderFailedCount();
			Engine.GUILable($"AssetLoadMode : {AssetSystem.AssetLoadMode}");
			Engine.GUILable($"Asset loader total count : {totalCount}");
			Engine.GUILable($"Asset loader failed count : {failedCount}");
		}
	}
}