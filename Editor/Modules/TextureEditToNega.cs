using UnityEngine;

namespace Yorozu.EditorTool
{
	internal class TextureEditToNega : TextureEditModule
	{
		internal override string Name => "Nega";

		internal override void OnGUI()
		{
		}

		internal override void Edit(Texture2D src, ref Texture2D dest)
		{
			for (var x = 0; x < src.width; x++)
			{
				for (var y = 0; y < src.height; y++)
				{
					var color = src.GetPixel(x, y);
					for (var i = 0; i < 3; i++)
						color[i] = 1 - color[i];

					dest.SetPixel(x, y, color);
				}
			}
		}
	}
}
