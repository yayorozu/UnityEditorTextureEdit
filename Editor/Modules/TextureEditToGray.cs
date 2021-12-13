using System;
using UnityEngine;

namespace Yorozu.EditorTool.TextureEdit
{
    [Serializable]
    internal class TextureEditToGray : TextureEditModule
    {
        internal override string Name => "ToGray";
        internal override string Description => "グレースケールに変換する";

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
                    var v = color.r * 0.3f + color.g * 0.59f + color.b * 0.11f;
                    dst.SetPixel(x, y, new Color(v, v, v, color.a));
                }
            }
        }
    }
}
