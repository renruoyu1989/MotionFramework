//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotionFramework.Pool
{
	/// <summary>
	/// 实体资源池管理器
	/// </summary>
	public sealed class PoolManager
	{
		public static readonly PoolManager Instance = new PoolManager();

		/// <summary>
		/// 资源池的ROOT
		/// </summary>
		private readonly GameObject _root;

		/// <summary>
		/// 资源池对象集合
		/// </summary>
		private readonly Dictionary<string, AssetObjectPool> _pools = new Dictionary<string, AssetObjectPool>();


		private PoolManager()
		{
			_root = new GameObject("[PoolManager]");
			_root.transform.position = Vector3.zero;
			_root.transform.eulerAngles = Vector3.zero;
			UnityEngine.Object.DontDestroyOnLoad(_root);
		}

		/// <summary>
		/// 创建一种实体资源的对象池
		/// </summary>
		public AssetObjectPool CreatePool(string resName, int capacity)
		{
			if (_pools.ContainsKey(resName))
				return _pools[resName];

			AssetObjectPool pool = new AssetObjectPool(_root.transform, resName, capacity);
			_pools.Add(resName, pool);
			return pool;
		}

		/// <summary>
		/// 是否都已经准备完毕
		/// </summary>
		public bool IsAllPrepare()
		{
			foreach (var pair in _pools)
			{
				if (pair.Value.IsPrepare == false)
					return false;
			}
			return true;
		}

		/// <summary>
		/// 销毁所有对象池及其资源
		/// </summary>
		public void DestroyAll()
		{
			foreach (var pair in _pools)
			{
				pair.Value.Destroy();
			}
			_pools.Clear();
		}

		/// <summary>
		/// 异步方式获取一个实体对象
		/// </summary>
		public void Spawn(string resName, Action<GameObject> callbcak)
		{
			if (_pools.ContainsKey(resName))
			{
				_pools[resName].Spawn(callbcak);
			}
			else
			{
				// 如果不存在创建该实体的对象池
				AssetObjectPool pool = CreatePool(resName, 0);
				pool.Spawn(callbcak);
			}
		}

		/// <summary>
		/// 同步方式获取一个实体对象
		/// </summary>
		public GameObject Spawn(string resName)
		{
			if (_pools.ContainsKey(resName))
			{
				return _pools[resName].Spawn();
			}
			else
			{
				// 如果不存在创建该实体的对象池
				AssetObjectPool pool = CreatePool(resName, 0);
				return pool.Spawn();
			}
		}

		/// <summary>
		/// 回收一个实体对象
		/// </summary>
		public void Restore(string resName, GameObject obj)
		{
			if (obj == null)
				return;

			if (_pools.ContainsKey(resName))
			{
				_pools[resName].Restore(obj);
			}
			else
			{
				LogSystem.Log(ELogType.Error, $"Should never get here. ResName is {resName}");
			}
		}

		/// <summary>
		/// 调试专属方法
		/// </summary>
		public Dictionary<string, AssetObjectPool> DebugAllPools
		{
			get { return _pools; }
		}
	}
}