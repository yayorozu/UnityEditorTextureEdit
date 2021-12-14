# UnityEditorTextureEdit

UnityEditor 上で Texture を加工するツール

※Texture2D.SetPixels を利用している都合上 RGBA32, ARGB32, RGB24, Alpha8 の フォーマットのテクスチャにのみ利用できます<br>
他にPixel に書き込む方法がわかれば対応していきます

以下の機能を有する

|  モード  |  機能  |
| ---- | ---- |
|  ChangeAlpha  |  指定した色のアルファを0にする  |
|  ChangeColor  |  指定した色を別の色に変換する  |
|  CutRoundAlpha  |  上下左右の アルファが0の部分を削除する  |
|  NoAplha  |  アルファをすべて1にする  |
|  Padding  |  上下左右にスペースを追加削除する  |
|  Resize  |  サイズを変換する  |
|  Split  |  指定した数で縦横分割して出力する  |
|  ToGray  |  グレースケールに変換する  |
|  Resize  |  サイズを変換する  |
|  ToHSV  |  HSVを指定して変換を行う  |
|  NPOT  |  2のべき乗サイズに変更する  |
|  ToNega  |  ネガポジ反転を行う  |
