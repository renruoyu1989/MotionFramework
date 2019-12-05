
namespace UnityEngine
{
	public static class UnityEngine_Object_Extention
	{
		public static bool IsDestroyed(this UnityEngine.Object o)
		{
			return o == null;
		}
	}
}