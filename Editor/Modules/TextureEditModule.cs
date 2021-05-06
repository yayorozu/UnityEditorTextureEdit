using System;
using UnityEngine;

namespace Yorozu.EditorTool
{
	[Serializable]
	internal abstract class TextureEditModule
	{
		internal abstract string Name { get; }
		internal abstract void OnGUI();
		internal abstract void Edit(Texture2D src, ref Texture2D dest);

		internal virtual Vector2Int GetSize(Texture2D src)
		{
			return new Vector2Int(src.width, src.height);
		}

		internal virtual void CheckTexture(Texture2D src)
		{
		}
	}
}
