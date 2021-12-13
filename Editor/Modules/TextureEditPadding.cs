using System;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool.TextureEdit
{
	[Serializable]
	internal class TextureEditPadding : TextureEditModule
	{
		internal override string Name => "Padding";
		internal override string Description => "上下左右にスペースを追加削除する";

		protected int _top;
		protected int _left;
		protected int _right;
		protected int _bottom;

		internal override void OnGUI()
		{
			_top = EditorGUILayout.IntField("Top", _top);
			_left = EditorGUILayout.IntField("Left", _left);
			_right = EditorGUILayout.IntField("Right", _right);
			_bottom = EditorGUILayout.IntField("Bottom", _bottom);
		}

		internal override void Edit(Texture2D src, ref Texture2D dst)
		{
			// 0保証
			if (src.width - _left - _right < 0 || src.height - _top - _bottom < 0)
			{
				dst.SetPixels(src.GetPixels(0));
				Debug.LogError("Illegal Size");
				return;
			}

			for (var y = 0; y < src.height; y++)
			{
				if (y < _bottom || src.height - y < _top)
					continue;

				for (var x = 0; x < src.width; x++)
				{
					if (x < _left || src.width - x < _right)
						continue;

					var color = src.GetPixel(x, y);
					dst.SetPixel(x - _left, y - _bottom, color);
				}
			}
		}

		internal override Vector2Int GetSize(Texture2D src)
		{
			return new Vector2Int(src.width - _left - _right, src.height - _top - _bottom);
		}
	}
}
