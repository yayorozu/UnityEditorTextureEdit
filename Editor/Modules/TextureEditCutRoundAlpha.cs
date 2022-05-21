using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Yorozu.EditorTool.TextureEdit
{
	[Serializable]
	internal class TextureEditCutRoundAlpha : TextureEditPadding
	{
		internal override string Name => "CutRoundAlpha";
		internal override string Description => "上下左右のアルファが0の領域を削除する";

		protected override void Draw()
		{
			using (new EditorGUI.DisabledScope(true))
			{
				base.Draw();
			}
		}

		protected override void CheckTexture(Texture2D src)
 		{
	        var src2 = new Texture2D(src.width, src.height, src.format, src.mipmapCount > 1);
	        Graphics.CopyTexture(src, src2);

			var pixels = src2.GetPixels(0);
			CheckTop(src.width, src.height, pixels);
			CheckBottom(src.width, src.height, pixels);
			CheckLeft(src.width, src.height, pixels);
			CheckRight(src.width, src.height, pixels);

			Object.DestroyImmediate(src2);
        }

		private void CheckTop(int srcWidth, int srcHeight, Color[] pixels)
		{
			_top = 0;

			for (var y = srcHeight - 1; y >= 0; y--)
			{
				var isBreak = false;
				for (var x = 0; x < srcWidth; x++)
				{
					var index = y * srcWidth + x;
					if (pixels[index].a > 0f)
					{
						isBreak = true;
						break;
					}
				}

				if (isBreak)
					break;

				_top++;
			}
		}

		private void CheckBottom(int srcWidth, int srcHeight, Color[] pixels)
		{
			_bottom = 0;
			for (var y = 0; y < srcHeight; y++)
			{
				var isBreak = false;
				for (var x = 0; x < srcWidth; x++)
				{
					var index = y * srcWidth + x;
					if (pixels[index].a > 0f)
					{
						isBreak = true;
						break;
					}
				}

				if (isBreak)
					break;

				_bottom++;
			}
		}

		private void CheckLeft(int srcWidth, int srcHeight, Color[] pixels)
		{
			_left = 0;
			for (var x = 0; x < srcWidth; x++)
			{
				var isBreak = false;
				for (var y = 0; y < srcHeight; y++)
				{
					var index = y * srcWidth + x;
					if (pixels[index].a > 0f)
					{
						isBreak = true;
						break;
					}
				}

				if (isBreak)
					break;

				_left++;
			}
		}

		private void CheckRight(int srcWidth, int srcHeight, Color[] pixels)
		{
			_right = 0;
			for (var x = srcWidth - 1; x >= 0; x--)
			{
				var isBreak = false;
				for (var y = 0; y < srcHeight; y++)
				{
					var index = y * srcWidth + x;
					if (pixels[index].a > 0f)
					{
						isBreak = true;
						break;
					}
				}

				if (isBreak)
					break;

				_right++;
			}
		}


	}
}
