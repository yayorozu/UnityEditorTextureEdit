using System;
using UnityEngine;

namespace Yorozu.EditorTool.TextureEdit
{
    [Serializable]
    internal class TextureEditToGray : TextureEditModule
    {
        internal override string Name => "ToGray";
        internal override string Description => "グレースケールに変換する";

        protected override bool ValidPreview => true;
        
        protected override Color Convert(int x, int y, Color color)
        {
            var v = color.r * 0.3f + color.g * 0.59f + color.b * 0.11f;
            return new Color(v, v, v, color.a);
        }
    }
}
