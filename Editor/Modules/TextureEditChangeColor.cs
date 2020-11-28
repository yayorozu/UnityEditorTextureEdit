using System;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTools
{
	[Serializable]
	internal class TextureEditChangeColor : TextureEditModule
	{
		internal override string Name => "ChangeColor";

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

		internal override void Edit(Texture2D src, ref Texture2D dest)
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

					dest.SetPixel(x, y, c);
				}
			}
		}
	}
}
