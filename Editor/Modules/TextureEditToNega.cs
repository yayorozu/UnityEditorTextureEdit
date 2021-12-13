using System;
using UnityEngine;

namespace Yorozu.EditorTool.TextureEdit
{
	[Serializable]
	internal class TextureEditToNega : TextureEditModule
	{
		internal override string Name => "ToNega";
		internal override string Description => "ネガポジ反転";

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
					for (var i = 0; i < 3; i++)
						color[i] = 1 - color[i];

					dst.SetPixel(x, y, color);
				}
			}
		}
	}
}
