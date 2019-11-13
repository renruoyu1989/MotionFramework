//--------------------------------------------------
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ParticleProfilerWindow : EditorWindow
{
	static ParticleProfilerWindow _thisInstance;

	[MenuItem("MotionTools/Particle Profiler", false, 201)]
	static void ShowWindow()
	{
		if (_thisInstance == null)
		{
			_thisInstance = EditorWindow.GetWindow(typeof(ParticleProfilerWindow), false, "特效分析器", true) as ParticleProfilerWindow;
			_thisInstance.minSize = new Vector2(600, 600);
		}

		_thisInstance.Show();
	}

	/// <summary>
	/// 特效预制体
	/// </summary>
	private UnityEngine.Object _effectPrefab = null;

	/// <summary>
	/// 粒子测试类
	/// </summary>
	private ParticleTester _tester = new ParticleTester();

	// GUI相关
	private bool _isPause = false;
	private double _lastTime = 0;
	private Vector2 _scrollPos1 = Vector2.zero;
	private Vector2 _scrollPos2 = Vector2.zero;
	private Vector2 _scrollPos3 = Vector2.zero;
	private bool _isShowCurves = true;
	private bool _isShowTextures = false;
	private bool _isShowMeshs = false;
	private bool _isShowTips = false;
	private Texture2D _texTips = null;

	private void Awake()
	{
		_lastTime = EditorApplication.timeSinceStartup;
		_texTips = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/MotionEngine/Editor/ParticleProfiler/GUI/tips.png");
		if (_texTips == null)
			Debug.LogWarning("Not found ParticleProfilerWindows tips texture.");
	}
	private void OnGUI()
	{
		EditorGUILayout.Space();

		_effectPrefab = EditorGUILayout.ObjectField($"请选择特效", _effectPrefab, typeof(UnityEngine.Object), false);

		// 测试按钮
		if (GUILayout.Button("测试"))
		{
			if (_effectPrefab == null)
			{
				Debug.LogWarning("需要一个特效预制体！");
				return;
			}

			// 焦点锁定游戏窗口
			var gameViewType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
			EditorWindow gameView = EditorWindow.GetWindow(gameViewType);
			gameView.Focus();

			// 开始测试
			_isPause = false;
			_tester.Test(_effectPrefab);
			Debug.Log($"开始测试特效：{_effectPrefab.name}");
		}

		// 暂停按钮
		if (_isPause)
		{
			if (GUILayout.Button("点击按钮恢复"))
				_isPause = false;
		}
		else
		{
			if (GUILayout.Button("点击按钮暂停"))
				_isPause = true;
		}

		// 粒子基本信息
		EditorGUILayout.Space();
		EditorGUILayout.LabelField($"材质数量：{_tester.MaterialCount}");
		EditorGUILayout.LabelField($"纹理数量：{_tester.TextureCount}");
		EditorGUILayout.LabelField($"纹理内存：{EditorUtility.FormatBytes(_tester.TextureMemory)}");
		EditorGUILayout.LabelField($"粒子系统组件：{_tester.ParticleSystemComponentCount} 个");

		// 粒子动态信息
		EditorGUILayout.Space();
		EditorGUILayout.LabelField($"DrawCall：{_tester.DrawCallCurrentNum}  最大：{_tester.DrawCallMaxNum}");
		EditorGUILayout.LabelField($"粒子数量：{_tester.ParticleCurrentCount}  最大：{_tester.ParticleMaxCount}");
		EditorGUILayout.LabelField($"三角面数：{_tester.TriangleCurrentCount}  最大：{_tester.TriangleMaxCount}");

		// 错误信息
		if(_tester.Errors.Count > 0)
		{
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox($"请修正以下错误提示", MessageType.Error, true);
			EditorGUI.indentLevel = 1;
			foreach (var error in _tester.Errors)
			{
				GUIStyle style = new GUIStyle();
				style.normal.textColor = new Color(0.8f, 0, 0);
				EditorGUILayout.LabelField(error, style);
			}
			EditorGUI.indentLevel = 0;
		}

		// 曲线图
		EditorGUILayout.Space();
		using (new EditorGUI.DisabledScope(false))
		{
			_isShowCurves = EditorGUILayout.Foldout(_isShowCurves, "时间曲线");
			if (_isShowCurves)
			{
				float curveHeight = 80;
				EditorGUI.indentLevel = 1;
				EditorGUILayout.LabelField($"采样时长 {_tester.CurveSampleTime} 秒");
				EditorGUILayout.CurveField("DrawCall", _tester.DrawCallCurve, GUILayout.Height(curveHeight));
				EditorGUILayout.CurveField("粒子数量", _tester.ParticleCountCurve, GUILayout.Height(curveHeight));
				EditorGUILayout.CurveField("三角面数", _tester.TriangleCountCurve, GUILayout.Height(curveHeight));
				EditorGUI.indentLevel = 0;
			}
		}

		// 纹理列表
		EditorGUILayout.Space();
		using (new EditorGUI.DisabledScope(false))
		{
			_isShowTextures = EditorGUILayout.Foldout(_isShowTextures, "纹理列表");
			if (_isShowTextures)
			{
				EditorGUI.indentLevel = 1;
				_scrollPos1 = EditorGUILayout.BeginScrollView(_scrollPos1);
				{
					List<Texture> textures = _tester.AllTextures;
					foreach (var tex in textures)
					{
						EditorGUILayout.LabelField($"{tex.name}  尺寸:{tex.height }*{tex.width}  格式:{ParticleTester.GetTextureFormatString(tex)}");
						EditorGUILayout.ObjectField("", tex, typeof(Texture), false, GUILayout.Width(80));
					}
				}
				EditorGUILayout.EndScrollView();
				EditorGUI.indentLevel = 0;
			}
		}

		// 网格列表
		EditorGUILayout.Space();
		using (new EditorGUI.DisabledScope(false))
		{
			_isShowMeshs = EditorGUILayout.Foldout(_isShowMeshs, "网格列表");
			if (_isShowMeshs)
			{
				EditorGUI.indentLevel = 1;
				_scrollPos2 = EditorGUILayout.BeginScrollView(_scrollPos2);
				{
					List<Mesh> meshs = _tester.AllMeshs;
					foreach (var mesh in meshs)
					{
						EditorGUILayout.ObjectField($"三角面数 : {mesh.triangles.Length / 3}", mesh, typeof(MeshFilter), false, GUILayout.Width(300));
					}
				}
				EditorGUILayout.EndScrollView();
				EditorGUI.indentLevel = 0;
			}
		}

		// 过程化检测结果
		EditorGUILayout.Space();
		using (new EditorGUI.DisabledScope(false))
		{
			_isShowTips = EditorGUILayout.Foldout(_isShowTips, "过程化检测结果");
			if (_isShowTips)
			{
				EditorGUI.indentLevel = 1;	
				_scrollPos3 = EditorGUILayout.BeginScrollView(_scrollPos3);
				{
					GUILayout.Button(_texTips); //绘制提示图片
					EditorGUILayout.HelpBox($"以下粒子系统组件不支持过程化模式！具体原因查看气泡提示", MessageType.Warning, true);
#if UNITY_2018_4_OR_NEWER
					List<ParticleSystem> particleList = _tester.AllParticles;
					foreach (var ps in particleList)
					{
						if (ps.proceduralSimulationSupported == false)
							EditorGUILayout.ObjectField($"{ps.gameObject.name}", ps.gameObject, typeof(GameObject), false, GUILayout.Width(300));
					}
#else
					EditorGUILayout.LabelField("当前版本不支持过程化检测，请升级至2018.4版本或最新版本");
#endif
				}
				EditorGUILayout.EndScrollView();
				EditorGUI.indentLevel = 0;
			}
		}
	}
	private void OnDestroy()
	{
		_tester.DestroyPrefab();
	}
	private void Update()
	{
		float deltaTime = (float)(EditorApplication.timeSinceStartup - _lastTime);
		_lastTime = EditorApplication.timeSinceStartup;

		// 更新测试特效
		if (_isPause == false)
			_tester.Update(deltaTime);

		// 刷新窗口界面
		this.Repaint();
	}
}