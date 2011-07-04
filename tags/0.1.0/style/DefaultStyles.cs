using System;
using UnityEngine;

namespace GUILib
{
	/// <summary>
	/// Facilitates loading of default style information. 
	/// </summary>
	/// <author>Manuel Fasse (ven)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 265 $, $Date: 2011-06-28 14:56:09 +0200 (Di, 28 Jun 2011) $</version>
	public class DefaultStyles
	{
		private Style defaultStyle;
		private Style defaultLabelStyle;
		private Style defaultButtonStyle;
		private Style defaultBoxStyle;
		private Style defaultEditBoxStyle;
		private Style defaultTextAreaStyle;
		private Style defaultVerticalSliderStyle;
		private Style defaultHorizontalSliderStyle;
		private Style defaultCheckBoxStyle;
		private Style defaultTooltipStyle;
		private Style defaultListBoxStyle;
		
		/// <summary>
		/// Create and load default styles. 
		/// </summary>
		public DefaultStyles ()
		{
			// Default Style
			this.defaultStyle = new Style();
			
			this.defaultStyle.Enabled = true;
			this.defaultStyle.Visible = true;
			
			this.defaultStyle.Left = 0;
			this.defaultStyle.Top = 0;
			this.defaultStyle.Width = 0;
			this.defaultStyle.Height = 0;
			
			this.defaultStyle.ContentOffsetLeft = 0;
			this.defaultStyle.ContentOffsetTop = 0;
			
			this.defaultStyle.FontAsset = new Font();
			this.defaultStyle.Clipping = TextClipping.Clip;
			this.defaultStyle.WordWrap = false;
			this.defaultStyle.Alignment = TextAnchor.UpperLeft;
			this.defaultStyle.TextColor = Color.black;
			this.defaultStyle.FontSize = 12;
			this.defaultStyle.FontStyling = FontStyle.Normal;
			this.defaultStyle.SetPadding(0);
			
			this.defaultStyle.Background = Types.EmptyTexture;
			this.defaultStyle.SetBorder(0);
			this.defaultStyle.SetOverflow(0);

			this.defaultStyle.CheckImagePosition = ImagePosition.TextOnly;
			this.defaultStyle.CheckImage = Types.EmptyTexture;
			this.defaultStyle.UncheckImage = Types.EmptyTexture;
			
			this.defaultStyle.ThumbWidth = 0;
			this.defaultStyle.ThumbHeight = 0;
			this.defaultStyle.ThumbBackground = Types.EmptyTexture;
			this.defaultStyle.SetThumbBorder(0);
			
			this.defaultStyle.ItemHeight = 0;
			this.defaultStyle.ItemBackground = Types.EmptyTexture;
			this.defaultStyle.SetItemPadding(0);
			this.defaultStyle.SelectedBackground = Types.EmptyTexture;
			this.defaultStyle.SelectedTextColor = Color.black;
			
			// Default Label Style
			this.defaultLabelStyle = this.defaultStyle.Copy();
			
			// Default Box Style
			this.defaultBoxStyle = this.defaultStyle.Copy();
			
			// Default Button Style
			this.defaultButtonStyle = this.defaultStyle.Copy();
			
			// Default EditBox Style
			this.defaultEditBoxStyle = this.defaultStyle.Copy();
			this.defaultEditBoxStyle.WordWrap = false;
			
			// Default TextArea Style
			this.defaultTextAreaStyle = this.defaultStyle.Copy();
			this.defaultTextAreaStyle.WordWrap = true;

			// Default CheckBox Style
			this.defaultCheckBoxStyle = this.defaultStyle.Copy();
			
			this.defaultCheckBoxStyle.CheckImagePosition = ImagePosition.ImageLeft;
			
			// Default Horizontal Slider Style
			this.defaultHorizontalSliderStyle = this.defaultStyle.Copy();
			
			this.defaultHorizontalSliderStyle.Height = 10f;
			this.defaultHorizontalSliderStyle.ThumbWidth = 10f;
			
			// Default Vertical Slider Style
			this.defaultVerticalSliderStyle = this.defaultStyle.Copy();
				
			this.defaultVerticalSliderStyle.Width = 10f;
			this.defaultVerticalSliderStyle.ThumbHeight = 10f;
			
			// Default Tooltip Style
			this.defaultTooltipStyle = this.defaultStyle.Copy();
			
			this.defaultTooltipStyle.WordWrap = true;
			this.defaultTooltipStyle.SetPadding(5);
			
			// Default ListBox Style
			this.defaultListBoxStyle = this.defaultStyle.Copy();
			this.defaultListBoxStyle.ItemHeight = 20;
			
			#region OVERRIDE TEXTURES
			Texture2D[] textures = new Texture2D[5];
			Color[] backgroundColors = new Color[5];
			backgroundColors[0] = new Color(0.75f, 0f, 0f, 0.75f);
			backgroundColors[1] = new Color(0f, 0f, 0.75f, 0.75f);
			backgroundColors[2] = new Color(1f, 0f, 0f, 0.75f);
			backgroundColors[3] = new Color(0f, 1f, 0f, 0.75f);
			backgroundColors[4] = new Color(1f, 1f, 0f, 0.75f);

			for (int t = 0; t < textures.Length; t++)
			{
				textures[t] = new Texture2D(20, 20);
				for (int x = 0; x < 20; x++)
				{
					for (int y = 0; y < 20; y++)
					{
						textures[t].SetPixel(x, y, backgroundColors[t]);
					}
				}
				textures[t].Apply();
			}

			this.defaultButtonStyle.Background = textures[0];
			this.defaultButtonStyle.Active.Background = textures[1];
			this.defaultButtonStyle.Hover.Background = textures[2];
			this.defaultButtonStyle.Focused.Background = textures[3];
			
			this.defaultEditBoxStyle.Background = textures[0];
			this.defaultEditBoxStyle.Active.Background = textures[1];
			this.defaultEditBoxStyle.Hover.Background = textures[2];
			this.defaultEditBoxStyle.Focused.Background = textures[3];
			
			this.defaultTextAreaStyle.Background = textures[0];
			this.defaultTextAreaStyle.Active.Background = textures[1];
			this.defaultTextAreaStyle.Hover.Background = textures[2];
			this.defaultTextAreaStyle.Focused.Background = textures[3];
			
			this.defaultListBoxStyle.Background = textures[0];
			this.defaultListBoxStyle.Active.Background = textures[1];
			this.defaultListBoxStyle.Hover.Background = textures[2];
			this.defaultListBoxStyle.Focused.Background = textures[3];
			
			Texture2D checkImage = new Texture2D(10, 10);
			Texture2D uncheckImage = new Texture2D(10, 10);
			for (int x = 0; x < 10; x++)
			{
				for (int y = 0; y < 10; y++)
				{
					checkImage.SetPixel(x, y, Color.black);
					uncheckImage.SetPixel(x, y, Color.gray);
				}
			}
			checkImage.Apply();
			uncheckImage.Apply();
			
			this.defaultCheckBoxStyle.CheckImage = checkImage;
			this.defaultCheckBoxStyle.UncheckImage = uncheckImage;
			
			this.defaultHorizontalSliderStyle.Background = textures[4];
			this.defaultHorizontalSliderStyle.ThumbBackground = textures[0];
			this.defaultHorizontalSliderStyle.Active.ThumbBackground = textures[1];
			this.defaultHorizontalSliderStyle.Hover.ThumbBackground = textures[2];
			
			this.defaultVerticalSliderStyle.Background = textures[4];
			this.defaultVerticalSliderStyle.ThumbBackground = textures[0];
			this.defaultVerticalSliderStyle.Active.ThumbBackground = textures[1];
			this.defaultVerticalSliderStyle.Hover.ThumbBackground = textures[2];
			
			this.defaultTooltipStyle.Background = new Texture2D(1,1);
			this.defaultTooltipStyle.Background.SetPixel(1,1, Color.yellow);
			this.defaultTooltipStyle.Background.Apply();
			#endregion
			
			StyleParser.Instance.UpdateStyle("DefaultStyle", this.defaultStyle);
			StyleParser.Instance.UpdateStyle("DefaultLabelStyle", this.defaultLabelStyle);
			StyleParser.Instance.UpdateStyle("DefaultBoxStyle", this.defaultBoxStyle);
			StyleParser.Instance.UpdateStyle("DefaultButtonStyle", this.defaultButtonStyle);
			StyleParser.Instance.UpdateStyle("DefaultEditBoxStyle", this.defaultEditBoxStyle);
			StyleParser.Instance.UpdateStyle("DefaultTextAreaStyle", this.defaultTextAreaStyle);
			StyleParser.Instance.UpdateStyle("DefaultHorizontalSliderStyle", this.defaultHorizontalSliderStyle);
			StyleParser.Instance.UpdateStyle("DefaultVerticalSliderStyle", this.defaultVerticalSliderStyle);
			StyleParser.Instance.UpdateStyle("DefaultCheckBoxStyle", this.defaultCheckBoxStyle);
			StyleParser.Instance.UpdateStyle("DefaultTooltipStyle", this.defaultTooltipStyle);
			StyleParser.Instance.UpdateStyle("DefaultListBoxStyle", this.defaultListBoxStyle);
		}
		
		/// <summary>
		/// Handle selection of specialized styles for various elements. 
		/// </summary>
		/// <param name="element">Element the style is needed for.</param>
		/// <returns>Specialized style for passed element or generic one.</returns>
		public Style GetDefaultStyle(object element)
		{
			if(element is Box)
			{
				return this.defaultBoxStyle;
			}
			if(element is Label)
			{
				return this.defaultLabelStyle;
			}
			if(element is Button)
			{
				return this.defaultButtonStyle;
			}
			if(element is EditBox)
			{
				return this.defaultEditBoxStyle;
			}
			if(element is TextArea)
			{
				return this.defaultTextAreaStyle;
			}
			if(element is CheckBox)
			{
				return this.defaultCheckBoxStyle;
			}
			if(element is Slider)
			{
				switch((element as Slider).Alignment)
				{
					default:
					case Types.Alignment.HORIZONTAL:
						return this.defaultHorizontalSliderStyle;
					case Types.Alignment.VERTICAL:
						return this.defaultVerticalSliderStyle;
				}
			}
			if(element is ToolTip)
			{
				return this.defaultTooltipStyle;
			}
			if(element is ListBox)
			{
				return this.defaultListBoxStyle;
			}
			// no specialized style found. return generic default style.
			return this.defaultStyle;
		}
	}
}

