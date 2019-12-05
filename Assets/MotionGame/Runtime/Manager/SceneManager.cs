using MotionEngine;
using MotionEngine.Debug;
using MotionEngine.Res;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotionGame
{
	internal class GameScene : AssetScene
	{
		/// <summary>
		/// 场景加载进度（0-100）
		/// </summary>
		public int Progress { private set; get; } = 0;

		/// <summary>
		/// 场景是否加载完毕
		/// </summary>
		public bool IsDone { private set; get; } = false;

		/// <summary>
		/// 是否是附加场景
		/// </summary>
		public bool IsAdditionScene { private set; get; }

		/// <summary>
		/// 场景加载完毕回调
		/// </summary>
		public System.Action<Asset> OnSceneLoad;


		public GameScene(bool isAdditionScene)
		{
			IsAdditionScene = isAdditionScene;
		}
		protected override bool OnPrepare(UnityEngine.Object mainAsset)
		{
			// 开始异步加载场景
			string[] splits = ResName.Split('/');
			string sceneName = splits[splits.Length - 1];
			AppEngine.Instance.StartCoroutine(StartLoading(sceneName));
			return true;
		}

		/// <summary>
		/// 异步加载场景
		/// </summary>
		private IEnumerator StartLoading(string name)
		{
			AsyncOperation asyncLoad;
			if(IsAdditionScene)
				asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name, UnityEngine.SceneManagement.LoadSceneMode.Additive);
			else
				asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name, UnityEngine.SceneManagement.LoadSceneMode.Single);
			WaitForEndOfFrame waitForEnd = new WaitForEndOfFrame();

			// Wait until the asynchronous scene fully loads
			while (!asyncLoad.isDone)
			{
				int percent = (int)(asyncLoad.progress * 100);
				while (Progress < percent)
				{
					Progress++;
					yield return waitForEnd;
				}
				yield return null;
			}

			Progress = 100;
			IsDone = true;
			OnSceneLoad?.Invoke(this);
		}
	}

	/// <summary>
	/// 场景管理器
	/// </summary>
	public sealed class SceneManager : IModule
	{
		public static readonly SceneManager Instance = new SceneManager();

		// 主场景
		private GameScene _mainScene;
		// 附加场景列表
		private readonly List<GameScene> _additionScenes = new List<GameScene>();


		private SceneManager()
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
			string mainSceneName = string.Empty;
			if (_mainScene != null)
				mainSceneName = _mainScene.ResName;
			DebugConsole.GUILable($"[{nameof(SceneManager)}] Main scene : {mainSceneName}");
			DebugConsole.GUILable($"[{nameof(SceneManager)}] Addition scene count : {_additionScenes.Count}");
		}

		/// <summary>
		/// 切换主场景，之前的主场景以及附加场景将会被卸载
		/// </summary>
		/// <param name="resName">场景资源名称</param>
		/// <param name="callback">场景加载完毕的回调</param>
		public void ChangeMainScene(string resName, System.Action<Asset> callback)
		{
			if (_mainScene != null)
			{
				UnLoadAllAdditionScenes();
				_mainScene.UnLoad();
				_mainScene = null;
			}

			_mainScene = new GameScene(false);
			_mainScene.OnSceneLoad = callback;
			_mainScene.Load(resName, null);
		}

		/// <summary>
		/// 在当前主场景的基础上加载附加场景
		/// </summary>
		/// <param name="resName">场景资源名称</param>
		/// <param name="callback">场景加载完毕的回调</param>
		public void LoadAdditionScene(string resName, System.Action<Asset> callback)
		{
			GameScene scene = TryGetAdditionScene(resName);
			if (scene != null)
			{
				LogSystem.Log(ELogType.Warning, $"The addition scene {resName} is already load.");
				return;
			}

			GameScene newScene = new GameScene(true);
			_additionScenes.Add(newScene);
			newScene.OnSceneLoad = callback;
			newScene.Load(resName, null);
		}

		/// <summary>
		/// 获取场景当前的加载进度，如果场景不存在返回0
		/// </summary>
		public int GetSceneLoadProgress(string resName)
		{
			if (_mainScene != null)
			{
				if (_mainScene.ResName == resName)
					return _mainScene.Progress;
			}

			GameScene scene = TryGetAdditionScene(resName);
			if (scene != null)
				return scene.Progress;

			LogSystem.Log(ELogType.Warning, $"Not found scene {resName}");
			return 0;
		}

		/// <summary>
		/// 检测场景是否加载完毕，如果场景不存在返回false
		/// </summary>
		public bool CheckSceneIsDone(string resName)
		{
			if (_mainScene != null)
			{
				if (_mainScene.ResName == resName)
					return _mainScene.IsDone;
			}

			GameScene scene = TryGetAdditionScene(resName);
			if (scene != null)
				return scene.IsDone;

			LogSystem.Log(ELogType.Warning, $"Not found scene {resName}");
			return false;
		}


		// 卸载所有附加场景
		private void UnLoadAllAdditionScenes()
		{
			for (int i = 0; i < _additionScenes.Count; i++)
			{
				_additionScenes[i].UnLoad();
			}
			_additionScenes.Clear();
		}

		// 尝试获取一个附加场景，如果不存在返回NULL
		private GameScene TryGetAdditionScene(string resName)
		{
			for (int i = 0; i < _additionScenes.Count; i++)
			{
				if (_additionScenes[i].ResName == resName)
					return _additionScenes[i];
			}
			return null;
		}
	}
}