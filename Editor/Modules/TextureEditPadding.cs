using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool
{
	internal class TextureEditPadding : TextureEditModule
	{
		internal override string Name => "Padding";

		private int _top;
		private int _left;
		private int _right;
		private int _bottom;

		internal override void OnGUI()
		{
			_top = EditorGUILayout.IntField("Top", _top);
			_left = EditorGUILayout.IntField("Left", _left);
			_right = EditorGUILayout.IntField("Right", _right);
			_bottom = EditorGUILayout.IntField("Bottom", _bottom);
		}

		internal override void Edit(Texture2D src, ref Texture2D dest)
		{
			// 0保証
			if (src.width - _left - _right < 0 || src.height - _top - _bottom < 0)
			{
				dest.SetPixels(src.GetPixels(0));
				Debug.LogError("Illegal Size");
				return;
			}

			var pixels = src.GetPixels(0);
			var destPixels = new List<Color>(dest.width * dest.height);
			for (var y = 0; y < src.height; y++)
			{
				if (y < _top || src.height - y < _bottom)
					continue;

				for (var x = 0; x < src.width; x++)
				{
					if (x < _left || src.height - x < _right)
						continue;

					var index = y * src.width + x;
					destPixels.Add(pixels[index]);
				}
			}

			dest.SetPixels(destPixels.ToArray());
		}

		internal override Vector2Int GetSize(Texture2D src)
		{
			return new Vector2Int(src.width - _left - _right, src.height - _top - _bottom);
		}
	}
}
