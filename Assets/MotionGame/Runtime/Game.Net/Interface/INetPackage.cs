
public interface INetPackage
{
	/// <summary>
	/// 消息ID
	/// </summary>
	ushort MsgID { set; get; }

	/// <summary>
	/// 消息体
	/// </summary>
	System.Object MsgObj { set; get; }

	/// <summary>
	/// 字节数据
	/// </summary>
	byte[] MsgBytes { set; get; }

	/// <summary>
	/// 是否是Mono消息，否则为热更消息
	/// </summary>
	bool IsMonoPackage { set; get; }
}