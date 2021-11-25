using System;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool
{
	[Serializable]
	internal class TextureEditChangeColor : TextureEditModule
	{
		internal override string Name => "ChangeColor";
		internal override string Description => "指定の色を他の色に変換する";

		[SerializeField]
		private Color _targetColor = Color.white;
		[SerializeField]
		private float _thresholdColor;
		[SerializeField]
		private Color _toColor = Color.white;
		[SerializeField]
		private bool _isChangeAlpha = true;

		internal override void OnGUI()
		{
			_targetColor = EditorGUILayout.ColorField("Target", _targetColor);
			_thresholdColor = EditorGUILayout.Slider("Color threshold", _thresholdColor, 0f, 1f);
			_toColor = EditorGUILayout.ColorField("To", _toColor);
			_isChangeAlpha = EditorGUILayout.Toggle("IsChangeAlpha", _isChangeAlpha);
		}

		internal override void Edit(Texture2D src, ref Texture2D dst)
		{
			for (var x = 0; x < src.width; x++)
			{
				for (var y = 0; y < src.height; y++)
				{
					var c = src.GetPixel(x, y);
					var isReplace = true;
					for (var i = 0; i < 3; i++)
					{
						if (!(c[i] >= _targetColor[i] - _thresholdColor) || !(c[i] <= _targetColor[i] + _thresholdColor))
						{
							isReplace = false;
							break;
						}
					}

					if (isReplace)
					{
						for (var i = 0; i < 4; i++)
						{
							if (!_isChangeAlpha && i == 3)
								continue;

							c[i] = _toColor[i];
						}
					}

					dst.SetPixel(x, y, c);
				}
			}
		}
	}
}
