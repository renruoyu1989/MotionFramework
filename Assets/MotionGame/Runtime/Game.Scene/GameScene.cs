using System.Collections;
using UnityEngine;
using MotionEngine;
using MotionEngine.Res;

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
		if (IsAdditionScene)
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