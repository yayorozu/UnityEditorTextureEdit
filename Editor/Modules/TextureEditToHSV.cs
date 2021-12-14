using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.AssetBundlePatching;

namespace Yorozu.EditorTool.TextureEdit
{
    [Serializable]
    internal class TextureEditToHSV : TextureEditModule
    {
        internal override string Name => "ToHSV";
        internal override string Description => "HSV変換";
        protected override bool ValidPreview => true;

        private float _h = 0f;
        private float _s = 1f;
        private float _v = 1f;


        protected override void Draw()
        {
            _h = EditorGUILayout.Slider("Hue", _h, 0f, 1f);
            _s = EditorGUILayout.Slider("Saturation", _s, 0f, 1f);
            _v = EditorGUILayout.Slider("Value", _v, 0f, 1f);
        }

        /// <summary>
        /// HSVShiftを行う
        /// </summary>
        protected override Color Convert(int x, int y, Color color)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);

            h += _h;
            if (1f <= h)
            {
                h -= 1f;
            }

            s *= _s;
            v *= _v;

            var convertColor = Color.HSVToRGB(h, s, v);

            return new Color(convertColor.r, convertColor.g, convertColor.b, color.a);
        }
    }
}
