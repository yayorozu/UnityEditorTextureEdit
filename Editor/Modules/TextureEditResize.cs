using System;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool.TextureEdit
{
	[Serializable]
	internal class TextureEditResize : TextureEditModule
	{
		internal override string Name => "Resize";
		internal override string Description => "サイズを変更する";

		[SerializeField]
		protected Vector2Int _size;
		protected int _width;
		protected int _height;


		internal override void OnGUI()
		{
			EditorGUILayout.LabelField($"Current Size. Width: {_width} Height:{_height}", EditorStyles.boldLabel);
			EditorGUILayout.Space(10);
			_size = EditorGUILayout.Vector2IntField("Size", _size);
		}

		internal override void Edit(Texture2D src, ref Texture2D dst)
		{
			var pixels = dst.GetPixels(0);
			for(var i = 0; i < pixels.Length; i++)
			{
				pixels[i] = src.GetPixelBilinear(
					i % _size.x * (1 / (float) _size.x),
					Mathf.Floor(i / (float)_size.x) * (1 / (float) _size.y)
				);
			}

			dst.SetPixels(pixels);
		}

		internal override Vector2Int GetSize(Texture2D src) => _size;

		protected override void CheckTexture(Texture2D src)
		{
			_width = src.width;
			_height = src.height;
			_size.x = src.width;
			_size.y = src.height;
		}
	}
}
