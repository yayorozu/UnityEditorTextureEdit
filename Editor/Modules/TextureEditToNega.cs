using System;
using UnityEngine;

namespace Yorozu.EditorTool.TextureEdit
{
	[Serializable]
	internal class TextureEditToNega : TextureEditModule
	{
		internal override string Name => "ToNega";
		internal override string Description => "ネガポジ反転";

		protected override bool ValidPreview => true;

		protected override Color Convert(int x, int y, Color color)
		{
			for (var i = 0; i < 3; i++)
				color[i] = 1 - color[i];
			
			return color;
		}
	}
}
