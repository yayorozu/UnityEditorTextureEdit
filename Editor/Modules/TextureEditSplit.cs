using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool
{
    [Serializable]
    internal class TextureEditSplit : TextureEditModule
    {
        internal override string Name => "Split";
        internal override string Description => "指定数で分割";

        private int _horizontalSplit = 1;
        private int _verticalSplit = 1;
        internal override bool Disable => _horizontalSplit <= 1 && _verticalSplit <= 1;

        internal override void OnGUI()
        {
            _horizontalSplit = EditorGUILayout.IntField("Horizontal", _horizontalSplit);
            _verticalSplit = EditorGUILayout.IntField("Vertical", _verticalSplit);
        }

        internal override void Edit(Texture2D src, ref Texture2D dst)
        {
            var path = AssetDatabase.GetAssetPath(src);
            var parent = Directory.GetParent(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var rootPath = Path.Combine(parent.FullName, fileName);
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            Graphics.CopyTexture(src, dst);
            var size = new Vector2Int(src.width / _horizontalSplit, src.height / _verticalSplit);
            for (var xIndex = 0; xIndex < _horizontalSplit; xIndex++)
            {
                for (var yIndex = 0; yIndex < _verticalSplit; yIndex++)
                {
                    var texture = new Texture2D(size.x, size.y, src.format, false);
                    for (var x = 0; x < size.x; x++)
                    {
                        for (var y = 0; y < size.x; y++)
                        {
                            var color =  dst.GetPixel(xIndex * size.x + x, yIndex * size.y + y);
                            texture.SetPixel(x, y, color);
                        }
                    }
                    texture.Apply();

                    var savePath = Path.Combine(rootPath, fileName + "_" + xIndex + "_" + yIndex + Path.GetExtension(path));
                    File.WriteAllBytes(savePath, texture.EncodeToPNG());
                }
            }
        }
    }
}
