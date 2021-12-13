using System;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool.TextureEdit
{
    [Serializable]
    internal class TextureEditToHSV : TextureEditModule
    {
        internal override string Name => "ToHSV";
        internal override string Description => "HSV変換";

        private float _h = 0f;
        private float _s = 1f;
        private float _v = 1f;

        [SerializeField]
        private Texture2D _copyTexture;
        [SerializeField]
        private Texture2D _displayTexture;

        internal override void OnGUI()
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                _h = EditorGUILayout.Slider("Hue", _h, 0f, 1f);
                _s = EditorGUILayout.Slider("Saturation", _s, 0f, 1f);
                _v = EditorGUILayout.Slider("Value", _v, 0f, 1f);
                if (check.changed)
                {
                    Apply();
                }
            }

            var rect = GUILayoutUtility.GetRect(
                0,
                0,
                GUILayout.MaxWidth(200),
                GUILayout.MaxHeight(200)
            );
            EditorGUI.DrawPreviewTexture(rect, _displayTexture);
        }

        private void Apply()
        {
            for (var x = 0; x < _copyTexture.width; x++)
            {
                for (var y = 0; y < _copyTexture.height; y++)
                {
                    var color = _copyTexture.GetPixel(x, y);
                    _displayTexture.SetPixel(x, y, HSVShift(color));
                }
            }
            _displayTexture.Apply();
        }

        internal override void Edit(Texture2D src, ref Texture2D dst)
        {
            for (var x = 0; x < src.width; x++)
            {
                for (var y = 0; y < src.height; y++)
                {
                    var color = src.GetPixel(x, y);
                    dst.SetPixel(x, y, HSVShift(color));
                }
            }
        }

        private Color HSVShift(Color color)
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

        internal override void CheckTexture(Texture2D src)
        {
            _copyTexture = new Texture2D(src.width, src.height, src.format, src.mipmapCount > 1);
            _displayTexture = new Texture2D(src.width, src.height, src.format, src.mipmapCount > 1);
            Graphics.CopyTexture(src, _copyTexture);
            Apply();
        }
    }
}
