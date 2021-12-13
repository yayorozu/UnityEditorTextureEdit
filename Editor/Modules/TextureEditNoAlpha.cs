using System;
using UnityEngine;

namespace Yorozu.EditorTool.TextureEdit
{
	[Serializable]
	internal class TextureEditNoAlpha : TextureEditModule
	{
		internal override string Name => "NoAlpha";
		internal override string Description => "アルファをすべて1にする";

		internal override void OnGUI()
		{
		}

		internal override void Edit(Texture2D src, ref Texture2D dst)
		{
			for (var x = 0; x < src.width; x++)
			{
				for (var y = 0; y < src.height; y++)
				{
					var color = src.GetPixel(x, y);
					color.a = 1f;
					dst.SetPixel(x, y, color);
				}
			}
		}
	}
}
