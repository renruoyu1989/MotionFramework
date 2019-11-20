using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using MotionEngine;
using MotionEngine.Debug;

namespace MotionGame
{
	[DebugAttribute("系统", 100)]
	public class DebugSystem : IDebug
	{
		public void OnInit()
		{
		}
		public void OnGUI()
		{
			int space = 15;

			DebugConsole.GUILable($"OS : {SystemInfo.operatingSystem}");
			DebugConsole.GUILable($"OS Memory : {SystemInfo.systemMemorySize / 1000}GB");
			DebugConsole.GUILable($"CPU : {SystemInfo.processorType}");
			DebugConsole.GUILable($"CPU Core : {SystemInfo.processorCount}");

			GUILayout.Space(space);
			DebugConsole.GUILable($"Device Model : {SystemInfo.deviceModel}");
			DebugConsole.GUILable($"Device Name : {SystemInfo.deviceName}");
			DebugConsole.GUILable($"Device Type : {SystemInfo.deviceType}");

			GUILayout.Space(space);
			DebugConsole.GUILable($"Graphics Device Name : {SystemInfo.graphicsDeviceName}");
			DebugConsole.GUILable($"Graphics Device Type : {SystemInfo.graphicsDeviceType}");
			DebugConsole.GUILable($"Graphics Memory : {SystemInfo.graphicsMemorySize / 1000}GB");
			DebugConsole.GUILable($"Graphics Shader Level : {SystemInfo.graphicsShaderLevel}");
			DebugConsole.GUILable($"Multi-threaded Rendering : {SystemInfo.graphicsMultiThreaded}");
			DebugConsole.GUILable($"Max Cubemap Size : {SystemInfo.maxCubemapSize}");
			DebugConsole.GUILable($"Max Texture Size : {SystemInfo.maxTextureSize}");

			GUILayout.Space(space);
			DebugConsole.GUILable($"Supports Accelerometer : {SystemInfo.supportsAccelerometer}"); //加速计硬件
			DebugConsole.GUILable($"Supports Gyroscope : {SystemInfo.supportsGyroscope}"); //陀螺仪硬件
			DebugConsole.GUILable($"Supports Audio : {SystemInfo.supportsAudio}"); //音频硬件
			DebugConsole.GUILable($"Supports GPS : {SystemInfo.supportsLocationService}"); //GPS硬件

			GUILayout.Space(space);
			DebugConsole.GUILable($"Battery Level : {SystemInfo.batteryLevel}");
			DebugConsole.GUILable($"Battery Status : {SystemInfo.batteryStatus}");

			GUILayout.Space(space);
			long memory = Profiler.GetTotalReservedMemoryLong() / 1000000;
			DebugConsole.GUILable($"Total Memory : {memory}MB");
			memory = Profiler.GetTotalAllocatedMemoryLong() / 1000000;
			DebugConsole.GUILable($"Used Memory : {memory}MB");
			memory = Profiler.GetTotalUnusedReservedMemoryLong() / 1000000;
			DebugConsole.GUILable($"Free Memory : {memory}MB");
			memory = Profiler.GetMonoHeapSizeLong() / 1000000;
			DebugConsole.GUILable($"Total Mono Memory : {memory}MB");
			memory = Profiler.GetMonoUsedSizeLong() / 1000000;
			DebugConsole.GUILable($"Used Mono Memory : {memory}MB");
		}
	}
}