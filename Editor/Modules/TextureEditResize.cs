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


		protected override void Draw()
		{
			EditorGUILayout.LabelField($"Current Size. Width: {_width} Height:{_height}", EditorStyles.boldLabel);
			EditorGUILayout.Space(10);
			_size = EditorGUILayout.Vector2IntField("Size", _size);
		}

		internal override void Edit(Texture2D src, ref Texture2D dst)
		{
			Resize(src, ref dst);
		}

		internal static void Resize(Texture2D src, ref Texture2D dst)
		{
			var width = dst.width;
			var height = dst.height;
			
			var pixels = dst.GetPixels(0);
			for(var i = 0; i < pixels.Length; i++)
			{
				pixels[i] = src.GetPixelBilinear(
					i % width * (1 / (float) width),
					Mathf.Floor(i / (float) width) * (1 / (float) height)
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
