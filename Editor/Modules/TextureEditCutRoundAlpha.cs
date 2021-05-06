using System;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool
{
	[Serializable]
	internal class TextureEditCutRoundAlpha : TextureEditPadding
	{
		internal override string Name => "CutRoundAlpha";

		internal override void OnGUI()
		{
			using (new EditorGUI.DisabledScope(true))
			{
				base.OnGUI();
			}
		}

		internal override void CheckTexture(Texture2D src)
 		{
	        var path = AssetDatabase.GetAssetPath(src);
	        var importer = AssetImporter.GetAtPath(path) as TextureImporter;

	        // 特定の設定だと変更できないのでキャッシュ
	        var isChange1 = !importer.isReadable;
	        var prevTT = importer.textureType;
	        if (isChange1 || prevTT != TextureImporterType.Sprite)
	        {
		        if (isChange1)
			        importer.isReadable = true;
		        if (prevTT != TextureImporterType.Sprite)
			        importer.textureType = TextureImporterType.Sprite;

		        importer.SaveAndReimport();
		        AssetDatabase.Refresh();
	        }

			var pixels = src.GetPixels(0);
			CheckTop(src.width, src.height, pixels);
			CheckBottom(src.width, src.height, pixels);
			CheckLeft(src.width, src.height, pixels);
			CheckRight(src.width, src.height, pixels);

			base.CheckTexture(src);

			// 変更した設定を戻す
			if (isChange1 || prevTT != TextureImporterType.Sprite)
			{
				var importer2 = AssetImporter.GetAtPath(path) as TextureImporter;
				if (isChange1)
					importer2.isReadable = false;
				if (prevTT != TextureImporterType.Sprite)
					importer.textureType = prevTT;

				importer2.SaveAndReimport();
			}
		}

		private void CheckTop(int srcWidth, int srcHeight, Color[] pixels)
		{
			_top = 0;

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

				_top++;
			}
		}

		private void CheckBottom(int srcWidth, int srcHeight, Color[] pixels)
		{
			_bottom = 0;
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
