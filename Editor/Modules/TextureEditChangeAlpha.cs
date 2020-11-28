using System;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTools
{
	[Serializable]
	internal class TextureEditChangeAlpha : TextureEditModule
	{
		internal override string Name => "ChangeAlpha";

		[SerializeField]
		private Color _targetColor = Color.white;
		[SerializeField]
		private float _thresholdColor;

		internal override void OnGUI()
		{
			_targetColor = EditorGUILayout.ColorField("Target", _targetColor);
			_thresholdColor = EditorGUILayout.Slider("Color threshold", _thresholdColor, 0f, 1f);
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
						c.a = 0f;

					dest.SetPixel(x, y, c);
				}
			}
		}
	}
}
