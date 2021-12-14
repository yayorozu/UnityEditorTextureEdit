using System;
using UnityEngine;

namespace Yorozu.EditorTool.TextureEdit
{
	[Serializable]
	internal class TextureEditNoAlpha : TextureEditModule
	{
		internal override string Name => "NoAlpha";
		internal override string Description => "アルファをすべて1にする";

		protected override bool ValidPreview => true;

		protected override Color Convert(int x, int y, Color color)
		{
			color.a = 1f;
			return color;
		}
	}
}
