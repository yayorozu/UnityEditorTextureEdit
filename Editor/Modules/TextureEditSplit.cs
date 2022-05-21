using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool.TextureEdit
{
    [Serializable]
    internal class TextureEditSplit : TextureEditModule
    {
        internal override string Name => "Split";
        internal override string Description => "指定サイズで分割";
        internal override bool IsOverride => false;

        private Vector2Int _srcSize;
        private Vector2Int _splitCount;
        private Vector2Int _remainderPixel;
        private Vector2Int _size = new Vector2Int(1, 1);
        private Vector2Int _space = new Vector2Int(0, 0);
        internal override bool Disable => !(_size.x > 0 && _size.y > 0 && _space.x >= 0 && _space.y >= 0);

        protected override void Draw()
        {
            EditorGUILayout.LabelField($"TextureSize", _srcSize.ToString());
            EditorGUILayout.LabelField($"Split Count", _splitCount.ToString());
            EditorGUILayout.LabelField($"Remainder Count", _remainderPixel.ToString());
            EditorGUILayout.Space(5);

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                _size = EditorGUILayout.Vector2IntField("Size", _size);
                _space = EditorGUILayout.Vector2IntField("Space", _space);
                if (check.changed)
                {
                    // 縦横分割数
                    for (var i = 0; i < 2; i++)
                    {
                        if (_size[i] > _srcSize[i])
                            _size[i] = _srcSize[i];
                        
                        _splitCount[i] = Mathf.FloorToInt((_srcSize[i] + _space[i]) / (float)(_size[i] + _space[i]));
                        _remainderPixel[i] = _srcSize[i] - (_splitCount[i] * (_size[i] + _space[i]) - _space[i]);
                    }
                }
            }
        }

        protected override void CheckTexture(Texture2D src)
        {
            _srcSize = new Vector2Int(src.width, src.height);
        }

        internal override void Edit(Texture2D src, ref Texture2D dst, string path)
        {
            var parent = Directory.GetParent(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var rootPath = Path.Combine(parent.FullName, fileName);
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            var index = 0;
            var y = 0; 
            while (y < src.height)
            {
                var x = 0;
                while (x < src.width)
                {
                    var texture = new Texture2D(_size.x, _size.y, src.format, false);
                    for (var x2 = 0; x2 < _size.x; x2++)
                    {
                        for (var y2 = 0; y2 < _size.x; y2++)
                        {
                            var color =  dst.GetPixel(x + x2, y + y2);
                            texture.SetPixel(x2, y2, color);
                        }
                    }
                    texture.Apply();
                    var savePath = Path.Combine(rootPath, $"{fileName}_{index++}{Path.GetExtension(path)}");
                    File.WriteAllBytes(savePath, texture.EncodeToPNG());
                    x += _space.x + _size.x;
                }
                
                y += _space.y + _size.y;
            }
        }
    }
}
