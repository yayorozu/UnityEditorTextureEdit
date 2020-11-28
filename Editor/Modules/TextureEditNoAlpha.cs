using UnityEngine;

namespace Yorozu.EditorTools
{
	internal class TextureEditNoAlpha : TextureEditModule
	{
		internal override string Name => "NoAlpha";

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
					color.a = 1f;
					dest.SetPixel(x, y, color);
				}
			}
		}
	}
}
