using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool.TextureEdit
{
    internal class TextureEditToNPOT : TextureEditResize
    {
        internal override string Name => "NPOT";
        internal override string Description => "2のべき乗サイズに変換";

        private static int[] _pots =
        {
            8, 16, 32, 64, 128, 256, 512, 1024, 2048,
        };

        private string[] _potTexts = _pots.Select(p => p.ToString()).ToArray();

        internal override void OnGUI()
        {
            EditorGUILayout.LabelField($"Current Size. Width: {_width} Height:{_height}", EditorStyles.boldLabel);
            EditorGUILayout.Space(10);
            _size.x = EditorGUILayout.IntPopup("Width", _size.x, _potTexts, _pots);
            _size.y = EditorGUILayout.IntPopup("Height", _size.y, _potTexts, _pots);
        }

        protected override void CheckTexture(Texture2D src)
        {
            base.CheckTexture(src);
            _size.x = GetIndex(src.width);
            _size.y = GetIndex(src.height);
        }

        private int GetIndex(int size)
        {
            for (var i = 0; i < _pots.Length; i++)
            {
                if (size < _pots[i])
                    return _pots[i];
            }

            return _pots.Last();
        }
    }
}
