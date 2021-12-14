using System;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool.TextureEdit
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

		protected override bool ValidPreview => true;

		protected override void Draw()
		{
			_targetColor = EditorGUILayout.ColorField("Target", _targetColor);
			_thresholdColor = EditorGUILayout.Slider("Color threshold", _thresholdColor, 0f, 1f);
			_toColor = EditorGUILayout.ColorField("To", _toColor);
			_isChangeAlpha = EditorGUILayout.Toggle("IsChangeAlpha", _isChangeAlpha);
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
			{
				for (var i = 0; i < 4; i++)
				{
					if (!_isChangeAlpha && i == 3)
						continue;

					color[i] = _toColor[i];
				}
			}

			return color;
		}
	}
}
