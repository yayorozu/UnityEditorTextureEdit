using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Yorozu.EditorTool.TextureEdit
{
	[Serializable]
	internal abstract class TextureEditModule
	{
		internal abstract string Name { get; }
		internal abstract string Description { get; }
		
		/// <summary>
		/// 加工後に上書きするか
		/// </summary>
		internal virtual bool IsOverride => true;

		internal virtual bool Disable => false;

		protected virtual bool ValidPreview => false;
		
		[SerializeField]
		private Texture2D _srcTexture;
		[SerializeField]
		private Texture2D _previewTexture;

		/// <summary>
		/// Previewに表示する最大サイズ
		/// </summary>
		private const float MaxPreviewSize = 150;
		
		internal void OnGUI()
		{
			using (var check = new EditorGUI.ChangeCheckScope())
			{
				Draw();
				if (ValidPreview && check.changed)
				{
					UpdatePreview();
				}
			}

			DrawPreview();
		}
		
		protected virtual void Draw(){}
		
		/// <summary>
		/// 加工処理
		/// </summary>
		internal virtual void Edit(Texture2D src, ref Texture2D dst)
		{
			for (var x = 0; x < src.width; x++)
			{
				for (var y = 0; y < src.height; y++)
				{
					var c = src.GetPixel(x, y);
					dst.SetPixel(x, y, Convert(x, y, c));
				}
			}			
		}

		/// <summary>
		/// 色の変換処理
		/// </summary>
		protected virtual Color Convert(int x, int y, Color color)
		{
			return color;
		}

		internal virtual Vector2Int GetSize(Texture2D src)
		{
			return new Vector2Int(src.width, src.height);
		}

		internal void Exit()
		{
			DestroyPreviewTexture();
		}

		private void DestroyPreviewTexture()
		{
			if (_srcTexture != null)
			{
				Object.DestroyImmediate(_srcTexture);
				_srcTexture = null;
			}
			if (_previewTexture != null)
			{
				Object.DestroyImmediate(_previewTexture);
				_previewTexture = null;
			}
		}
		
		/// <summary>
		/// 新しいテクスチャに差し替わった
		/// </summary>
		internal void ApplyNewTexture(Texture2D src)
		{
			if (src == null)
			{
				DestroyPreviewTexture();
				return;
			}

			CheckTexture(src);
			
			// Previewが必要であればコピーしておく
			if (ValidPreview)
			{
				CreatePreviewTexture(src);
			}
		}

		private void CreatePreviewTexture(Texture2D src)
		{
			try
			{
				var readableTexture = new Texture2D(src.width, src.height, src.format, src.mipmapCount > 1);
				Graphics.CopyTexture(src, readableTexture);
				var size = new Vector2(MaxPreviewSize, MaxPreviewSize);
				// 大きい方をベースに計算
				if (src.width > src.height)
					size.y = src.height / (src.width / MaxPreviewSize);
				else
					size.x = src.width / (src.height / MaxPreviewSize);

				_srcTexture = new Texture2D((int) size.x, (int) size.y, TextureFormat.RGBA32, false);
				TextureEditResize.Resize(readableTexture, ref _srcTexture);
				// 表示するやつはエラーが出ないようにフォーマット指定
				_previewTexture = new Texture2D(_srcTexture.width, _srcTexture.height, TextureFormat.RGBA32, false);
				UpdatePreview();
			}
			catch (Exception e)
			{
				Debug.LogError(e);
				_srcTexture = null;
				_previewTexture = null;
			}
		}

		protected virtual void CheckTexture(Texture2D src)
		{
		}
		
		protected void DrawPreview()
		{
			if (_previewTexture == null) 
				return;

			EditorGUILayout.Space(10);
			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.FlexibleSpace();
				using (new EditorGUILayout.VerticalScope("box"))
				{
					EditorGUILayout.LabelField("Preview");
					var rect = GUILayoutUtility.GetRect(
						_previewTexture.width,
						_previewTexture.height
					);

					EditorGUI.DrawTextureTransparent(rect, _previewTexture, ScaleMode.ScaleToFit);
				}
			}

		}

		/// <summary>
		/// PreviewTextureに反映
		/// </summary>
		protected void UpdatePreview()
		{
			if (_srcTexture == null ||
			    _previewTexture == null
			   )
				return;

			try
			{
				for (var x = 0; x < _srcTexture.width; x++)
				{
					for (var y = 0; y < _srcTexture.height; y++)
					{
						var color = _srcTexture.GetPixel(x, y);
						_previewTexture.SetPixel(x, y, Convert(x, y, color));
					}
				}
				_previewTexture.Apply();
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}
	}
}
