using System;
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
		private int _toolIndex;
		[SerializeField]
		private string[] _tabToggles;

		private TextureEditModule _current => _tools[_toolIndex];

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

			_tabToggles = _tools.Select(t => t.Name).ToArray();
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
					_toolIndex = GUILayout.Toolbar(_toolIndex, _tabToggles, new GUIStyle(EditorStyles.toolbarButton), GUI.ToolbarButtonSize.FitToContents);
					if (check.changed)
					{
						if (_src != null)
							_current.Active(_src);
					}
				}
			}

			_src = EditorGUILayout.ObjectField("Target Texture", _src, typeof(Texture2D), false) as Texture2D;

			using (new EditorGUI.DisabledScope(_src == null))
			{
				using (new EditorGUILayout.VerticalScope("box"))
				{
					_current.OnGUI();
				}
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Edit"))
				{
					EditTexture(_src);
				}
			}
		}

		private void EditTexture(Texture2D src)
		{
			var size = _current.GetSize(src);
			var texture = new Texture2D(size.x, size.y, src.format, src.mipmapCount == -1);
			var path = AssetDatabase.GetAssetPath(src);
			var importer = AssetImporter.GetAtPath(path) as TextureImporter;

			// 特定の設定だと変更できないのでキャッシュ
			var isChange1 = !importer.isReadable;
			var prevTT = importer.textureType;
			if (isChange1 || prevTT != TextureImporterType.Sprite)
			{
				if (isChange1)
					importer.isReadable = true;
				if (prevTT != TextureImporterType.Sprite)
					importer.textureType = TextureImporterType.Sprite;

				importer.SaveAndReimport();
				AssetDatabase.Refresh();
			}

			_current.Edit(src, ref texture);

			texture.Apply();

			System.IO.File.WriteAllBytes(path.Replace('/', System.IO.Path.DirectorySeparatorChar), texture.EncodeToPNG());
			AssetDatabase.Refresh();

			// 変更した設定を戻す
			if (isChange1 || prevTT != TextureImporterType.Sprite)
			{
				var importer2 = AssetImporter.GetAtPath(path) as TextureImporter;
				if (isChange1)
					importer2.isReadable = false;
				if (prevTT != TextureImporterType.Sprite)
					importer.textureType = prevTT;

				importer2.SaveAndReimport();
			}
		}
	}
}
