using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using MotionEngine;
using MotionEngine.Net;
using MotionEngine.Debug;

namespace MotionGame
{
	/// <summary>
	/// 网络管理器
	/// </summary>
	public sealed class NetManager : IModule
	{
		public static readonly NetManager Instance = new NetManager();

		/// <summary>
		/// 服务端
		/// </summary>
		private TServer _server;

		/// <summary>
		/// 通信频道
		/// </summary>
		private TChannel _channel;

		/// <summary>
		/// 当前的网络状态
		/// </summary>
		public ENetworkState State { private set; get; } = ENetworkState.Disconnect;

		/// <summary>
		/// Mono层网络消息接收回调
		/// </summary>
		public Action<INetPackage> MonoPackageCallback;

		/// <summary>
		/// 热更层网络消息接收回调
		/// </summary>
		public Action<INetPackage> HotfixPackageCallback;


		private NetManager()
		{
		}
		public void Awake()
		{
		}
		public void Start()
		{
			_server = new TServer();
			_server.Start(false, null);
		}
		public void Update()
		{
			if (_server != null)
				_server.Update();

			UpdatePickMsg();
			UpdateNetworkState();
		}
		public void LateUpdate()
		{
		}
		public void OnGUI()
		{
			DebugConsole.GUILable($"[{nameof(NetManager)}] Network state : {State}");
		}

		private void UpdatePickMsg()
		{
			if (_channel != null)
			{
				INetPackage package = (INetPackage)_channel.PickMsg();
				if (package != null)
				{
					if (package.IsMonoPackage)
						MonoPackageCallback.Invoke(package);
					else
						HotfixPackageCallback.Invoke(package);
				}
			}
		}
		private void UpdateNetworkState()
		{
			if (State == ENetworkState.Connected)
			{
				if (_channel != null && _channel.IsConnected() == false)
				{
					State = ENetworkState.Disconnect;
					LogSystem.Log(ELogType.Warning, "Server disconnect.");
				}
			}
		}

		/// <summary>
		/// 连接服务器
		/// </summary>
		public void ConnectServer(string host, int port, Type packageParseType)
		{
			if (State == ENetworkState.Disconnect)
			{
				State = ENetworkState.Connecting;
				IPEndPoint remote = new IPEndPoint(IPAddress.Parse(host), port);
				_server.ConnectAsync(remote, OnConnectServer, packageParseType);
			}
		}
		private void OnConnectServer(TChannel channel, SocketError error)
		{
			LogSystem.Log(ELogType.Log, $"Server connect result : {error}");
			if (error == SocketError.Success)
			{
				_channel = channel;
				State = ENetworkState.Connected;
			}
			else
			{
				State = ENetworkState.Disconnect;
			}
		}

		/// <summary>
		/// 断开连接
		/// </summary>
		public void DisconnectServer()
		{
			State = ENetworkState.Disconnect;
			if (_channel != null)
			{
				_server.ReleaseChannel(_channel);
				_channel = null;
			}
		}

		/// <summary>
		/// 发送消息
		/// </summary>
		public void SendMsg(INetPackage package)
		{
			if (State != ENetworkState.Connected)
			{
				LogSystem.Log(ELogType.Warning, "Network is not connected.");
				return;
			}

			if (_channel != null)
				_channel.SendMsg(package);
		}
	}
}