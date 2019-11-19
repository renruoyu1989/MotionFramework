using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionEngine.Res;

namespace MotionGame
{
	/// <summary>
	/// 实体资源对象池
	/// </summary>
	public class AssetObjectPool
	{
		// 池子
		private readonly Stack<GameObject> _pool;

		// 实体资源类
		private AssetObject _asset;

		// 实体对象
		private GameObject _go;

		// 对象池Root
		private Transform _root;

		// 资源加载完毕回调
		private Action<GameObject> _callbacks;


		/// <summary>
		/// 实体资源名称
		/// </summary>
		public string ResName { private set; get; }

		/// <summary>
		/// 对象池容量
		/// </summary>
		public int Capacity { private set; get; }

		/// <summary>
		/// 是否准备完毕
		/// </summary>
		public bool IsPrepare
		{
			get
			{
				if (_asset == null)
					return false;
				return _asset.IsLoadDone();
			}
		}

		/// <summary>
		/// 加载结果
		/// </summary>
		public EAssetResult LoadResult
		{
			get
			{
				if (_asset == null)
					return EAssetResult.None;
				return _asset.Result;
			}
		}

		/// <summary>
		/// 内部缓存总数
		/// </summary>
		public int Count
		{
			get { return _pool.Count; }
		}

		/// <summary>
		/// 外部使用总数
		/// </summary>
		public int SpawnCount { private set; get; }


		public AssetObjectPool(Transform root, string resName, int capacity)
		{
			_root = root;
			ResName = resName;
			Capacity = capacity;

			// 创建缓存池
			_pool = new Stack<GameObject>(capacity);

			// 加载资源
			_asset = new AssetObject();
			_asset.Load(resName, OnAssetPrepare);
		}

		// 当资源加载完毕
		private void OnAssetPrepare(object assetClass, EAssetResult result)
		{
			// 如果加载失败，创建临时对象
			if (result == EAssetResult.Failed)
				_go = new GameObject(ResName);
			else
				_go = _asset.GameObj;

			// 设置游戏对象
			_go.SetActive(false);
			_go.transform.SetParent(_root);
			_go.transform.localPosition = Vector3.zero;

			// 创建初始对象
			for (int i = 0; i < Capacity; i++)
			{
				GameObject obj = GameObject.Instantiate(_go) as GameObject;
				InternalRestore(obj);
			}

			// 最后返回结果
			if (_callbacks != null)
			{
				Delegate[] actions = _callbacks.GetInvocationList();
				for (int i = 0; i < actions.Length; i++)
				{
					var action = (Action<GameObject>)actions[i];
					Spawn(action);
				}
				_callbacks = null;
			}
		}

		/// <summary>
		/// 存储一个对象
		/// </summary>
		public void Restore(GameObject go)
		{
			if (go == null)
				return;

			SpawnCount--;
			InternalRestore(go);
		}
		private void InternalRestore(GameObject go)
		{
			go.SetActive(false);
			go.transform.SetParent(_root);
			go.transform.localPosition = Vector3.zero;
			_pool.Push(go);
		}

		/// <summary>
		/// 异步的方式获取一个对象
		/// </summary>
		public void Spawn(Action<GameObject> callback)
		{
			// 如果对象池还没有准备完毕
			if (IsPrepare == false)
			{
				_callbacks += callback;
				return;
			}
	
			if (_pool.Count > 0)
			{
				GameObject go = _pool.Pop();
				go.SetActive(true);
				go.transform.parent = null;
				callback.Invoke(go);
			}
			else
			{
				GameObject obj = GameObject.Instantiate(_go);
				obj.SetActive(true);
				callback.Invoke(obj);
			}
			SpawnCount++;
		}

		/// <summary>
		/// 同步的方式获取一个对象
		/// </summary>
		public GameObject Spawn()
		{
			// 如果对象池还没有准备完毕
			if (IsPrepare == false)
				throw new Exception($"{_asset.ResName} is not prepare");

			GameObject go = null;
			if (_pool.Count > 0)
			{
				go = _pool.Pop();
				go.SetActive(true);
				go.transform.parent = null;
			}
			else
			{
				go = GameObject.Instantiate(_go);
				go.SetActive(true);
			}
			SpawnCount++;
			return go;
		}

		/// <summary>
		/// 销毁对象池
		/// </summary>
		public void Destroy()
		{
			// 卸载资源对象
			if (_asset != null)
			{
				_asset.UnLoad();
				_asset = null;
			}

			// 销毁游戏对象
			foreach (var item in _pool)
			{
				GameObject.Destroy(item);
			}
			_pool.Clear();

			// 清空回调
			_callbacks = null;
			SpawnCount = 0;
		}
	}
}