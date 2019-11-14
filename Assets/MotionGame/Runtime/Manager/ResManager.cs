using System;
using MotionEngine;
using MotionEngine.Res;
using MotionEngine.Debug;

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
			DebugConsole.GUILable($"[{nameof(ResManager)}] AssetLoadMode : {AssetSystem.AssetLoadMode}");
			DebugConsole.GUILable($"[{nameof(ResManager)}] Asset loader total count : {totalCount}");
			DebugConsole.GUILable($"[{nameof(ResManager)}] Asset loader failed count : {failedCount}");
		}
	}
}