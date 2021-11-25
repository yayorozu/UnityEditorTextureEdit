using System;
using UnityEngine;
using EditorGUILayout = UnityEditor.EditorGUILayout;

namespace Yorozu.EditorTool
{
	[Serializable]
	internal class TextureEditChangeFormat : TextureEditModule
	{
		internal override string Name => "ChangeFormat";
		internal override string Description => "Format を変換";

		[SerializeField]
		private TextureFormat _format;

		internal override void OnGUI()
		{
			_format = (TextureFormat) EditorGUILayout.EnumPopup("Format", _format);
		}

		internal override void Edit(Texture2D src, ref Texture2D dst)
		{
			dst = new Texture2D(dst.width, dst.height, _format, src.mipmapCount > 1);
		}

		internal override void CheckTexture(Texture2D src)
		{
			_format = src.format;
		}
	}
}
