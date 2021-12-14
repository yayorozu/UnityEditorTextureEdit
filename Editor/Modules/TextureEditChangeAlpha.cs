using System;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool.TextureEdit
{
	[Serializable]
	internal class TextureEditChangeAlpha : TextureEditModule
	{
		internal override string Name => "ChangeAlpha";
		internal override string Description => "指定色のアルファを0にする";

		[SerializeField]
		private Color _targetColor = Color.white;
		[SerializeField]
		private float _thresholdColor;

		protected override bool ValidPreview => true;

		protected override void Draw()
		{
			_targetColor = EditorGUILayout.ColorField("Target", _targetColor);
			_thresholdColor = EditorGUILayout.Slider("Color threshold", _thresholdColor, 0f, 1f);
		}

		protected override Color Convert(int x, int y, Color color)
		{
			var isReplace = true;
			for (var i = 0; i < 3; i++)
			{
				if (!(color[i] >= _targetColor[i] - _thresholdColor) || !(color[i] <= _targetColor[i] + _thresholdColor))
				{
					isReplace = false;
					break;
				}
			}

			if (isReplace)
				color.a = 0f;

			return color;
		}
	}
}
