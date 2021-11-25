﻿using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool
{
	internal class TextureEditWindow : EditorWindow
	{
		[MenuItem("Tools/TextureEdit")]
		private static void ShowWindow()
		{
			var window = GetWindow<TextureEditWindow>("TextureEdit");
			window.Show();
		}

		[SerializeReference]
		private TextureEditModule[] _tools;
		[SerializeField]
		private Texture2D _src;
		[SerializeField]
		private int _moduleIndex;
		[SerializeField]
		private string[] _moduleNames;

		/// <summary>
		/// 加工ミスったときように前のやつをキャッシュ
		/// </summary>
		[SerializeField]
		private Texture2D _cache;

		private TextureEditModule _currentModule => _tools[_moduleIndex];

		private void OnEnable()
		{
			if (_tools == null)
			{
				_tools = AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany(a => a.GetTypes())
					.Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(TextureEditModule)))
					.Select(t => Activator.CreateInstance(t, true) as TextureEditModule)
					.ToArray();
			}

			_moduleNames = _tools.Select(t => t.Name).ToArray();
		}

		private void OnDisable()
		{
			DestroyImmediate(_cache);
			_cache = null;
		}

		private void OnGUI()
		{
			if (_tools == null || _tools.Length <= 0)
			{
				EditorGUILayout.HelpBox("Texture Edit Module Not Found", MessageType.Error);
				return;
			}

			using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
			{
				using (var check = new EditorGUI.ChangeCheckScope())
				{
					_moduleIndex = EditorGUILayout.Popup("Select Module", _moduleIndex, _moduleNames, EditorStyles.toolbarPopup);
					if (check.changed)
					{
						if (_src != null)
							_currentModule.CheckTexture(_src);
					}
				}
			}

			using (var check = new EditorGUI.ChangeCheckScope())
			{
				_src = EditorGUILayout.ObjectField("Target Texture", _src, typeof(Texture2D), false) as Texture2D;
				if (check.changed)
				{
					DestroyImmediate(_cache);
					_cache = null;
					if (_src != null)
					{
						_currentModule.CheckTexture(_src);
						_cache = new Texture2D(_src.width, _src.height, _src.format, _src.mipmapCount > 1);
						Graphics.CopyTexture(_src, _cache);
					}
				}
			}

			EditorGUILayout.HelpBox(_currentModule.Description, MessageType.Info);
			using (new EditorGUI.DisabledScope(_src == null))
			{
				using (new EditorGUILayout.VerticalScope("box"))
				{
					_currentModule.OnGUI();
				}
				GUILayout.FlexibleSpace();
				using (new EditorGUILayout.HorizontalScope())
				{
					using (new EditorGUI.DisabledScope(_cache == null))
					{
						// セットしたタイミングのテクスチャに戻す
						if (GUILayout.Button("Undo"))
						{
							Undo(_src);
						}
					}
					using (new EditorGUI.DisabledScope(_currentModule.Disable))
					{
						if (GUILayout.Button("Apply"))
						{
							EditTexture(_src);
						}
					}
				}
			}
		}

		/// <summary>
		/// モジュールに合わせたテクスチャの加工
		/// </summary>
		private void EditTexture(Texture2D src)
		{
			var size = _currentModule.GetSize(src);
			var src2 = new Texture2D(src.width, src.height, src.format, src.mipmapCount > 1);
			var dst = new Texture2D(size.x, size.y, src.format, src.mipmapCount > 1);
			// ピクセル読み込みできるようにコピー
			Graphics.CopyTexture(src, src2);
			var path = AssetDatabase.GetAssetPath(src);

			_currentModule.Edit(src2, ref dst);

			dst.Apply();

			if (_currentModule.IsOverride)
			{
				System.IO.File.WriteAllBytes(path, dst.EncodeToPNG());
			}
			AssetDatabase.Refresh();
			DestroyImmediate(dst);
		}

		private void Undo(Texture2D src)
		{
			if (_cache == null)
				return;

			var path = AssetDatabase.GetAssetPath(src);
			System.IO.File.WriteAllBytes(path, _cache.EncodeToPNG());
			AssetDatabase.Refresh();
		}
	}
}
