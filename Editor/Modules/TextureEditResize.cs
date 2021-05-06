using System;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool
{
	[Serializable]
	internal class TextureEditResize : TextureEditModule
	{
		internal override string Name => "Resize";

		[SerializeField]
		private Vector2Int _size;

		internal override void OnGUI()
		{
			_size = EditorGUILayout.Vector2IntField("Size", _size);
		}

		internal override void Edit(Texture2D src, ref Texture2D dest)
		{
			var pixels = dest.GetPixels(0);
			for(var i = 0; i < pixels.Length; i++)
			{
				pixels[i] = src.GetPixelBilinear(
					i % _size.x * (1 / (float) _size.x),
					Mathf.Floor(i / (float)_size.x) * (1 / (float) _size.y)
				);
			}

			dest.SetPixels(pixels);
		}

		internal override Vector2Int GetSize(Texture2D src) => _size;

		internal override void Active(Texture2D src)
		{
			_size.x = src.width;
			_size.y = src.height;
		}
	}
}
