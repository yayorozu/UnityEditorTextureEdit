using System;
using UnityEngine;
using EditorGUILayout = UnityEditor.EditorGUILayout;

namespace Yorozu.EditorTools
{
	[Serializable]
	internal class TextureEditChangeFormat : TextureEditModule
	{
		internal override string Name => "ChangeFormat";

		[SerializeField]
		private TextureFormat _format;

		internal override void OnGUI()
		{
			_format = (TextureFormat) EditorGUILayout.EnumPopup("Format", _format);
		}

		internal override void Edit(Texture2D src, ref Texture2D dest)
		{
			dest = new Texture2D(dest.width, dest.height, _format, src.mipmapCount == -1);
		}

		internal override void Active(Texture2D src)
		{
			_format = src.format;
		}
	}
}
