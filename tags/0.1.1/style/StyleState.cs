using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GUILib
{
	/// <summary>
	/// Class containing all state-dependent properties of a style.
	/// </summary>
	/// <author>Peer Adelt (adelt)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 286 $, $Date: 2011-07-28 16:47:17 +0200 (Do, 28 Jul 2011) $</version>
	public class StyleState
	{
		private int? paddingLeft;
		private int? paddingRight;
		private int? paddingTop;
		private int? paddingBottom;
		private int? marginLeft;
		private int? marginRight;
		private int? marginTop;
		private int? marginBottom;
		private int? borderLeft;
		private int? borderRight;
		private int? borderTop;
		private int? borderBottom;
		private int? overflowLeft;
		private int? overflowRight;
		private int? overflowTop;
		private int? overflowBottom;
		private TextAnchor? alignment;
		private float? contentOffsetLeft;
		private float? contentOffsetTop;
		private Font fontAsset;
		private string fontPath;
		private Color? textColor;
		private Texture2D background;
		private string backgroundPath;
		private Color? backgroundColor;
# if (!UNITY_2_6)
# if (!UNITY_2_6_1)
		private FontStyle? fontStyling;
		private int? fontSize;
# endif
# endif
		
		private Texture2D checkImage;
		private string checkImagePath;
		private Texture2D uncheckImage;
		private string uncheckImagePath;
		
		private Texture2D thumbBackground;
		private string thumbBackgroundPath;
		private int? thumbBorderLeft;
		private int? thumbBorderRight;
		private int? thumbBorderTop;
		private int? thumbBorderBottom;
		
		private int? itemPaddingLeft;
		private int? itemPaddingRight;
		private int? itemPaddingTop;
		private int? itemPaddingBottom;
		private Texture2D itemBackground;
		private string itemBackgroundPath;
		private Color? selectedTextColor;
		private Texture2D selectedBackground;
		private string selectedBackgroundPath;

		private Style parent;
		private Types.StyleStateType type;
		
		/// <summary>
		/// Create a new StyleState. 
		/// </summary>
		/// <param name="parent">
		/// Parent Style.
		/// </param>
		/// <param name="type">
		/// Type of the StyleState.
		/// </param>
		public StyleState(Style parent, Types.StyleStateType type)
		{
			// set parent and type
			this.parent = parent;
			this.type = type;
		}
		
		/// <summary>
		/// Copy a StyleState. 
		/// </summary>
		/// <param name="parent">
		/// Parent Style.
		/// </param>
		/// <returns>
		/// The copied StyleState.
		/// </returns>
		public StyleState Copy(Style parent)
		{
			StyleState copy = (StyleState)this.MemberwiseClone();
			copy.parent = parent;
			return copy;
		}
		
		/// <summary>
		/// Left space from the element to its content. 
		/// </summary>
		public int? PaddingLeft
		{
			get { return this.paddingLeft; }
			set
			{
				this.paddingLeft = value;
				parent.UpdateProperty(Types.StylePropertyType.paddingleft);
			}
		}
		
		/// <summary>
		/// Right space from the element to its content. 
		/// </summary>
		public int? PaddingRight
		{
			get { return this.paddingRight; }
			set
			{
				this.paddingRight = value;
				parent.UpdateProperty(Types.StylePropertyType.paddingright);
			}
		}
		
		/// <summary>
		/// Top space from the element to its content. 
		/// </summary>
		public int? PaddingTop
		{
			get { return this.paddingTop; }
			set
			{
				this.paddingTop = value;
				parent.UpdateProperty(Types.StylePropertyType.paddingtop);
			}
		}

		/// <summary>
		/// Bottom space from the element to its content. 
		/// </summary>
		public int? PaddingBottom
		{
			get { return this.paddingBottom; }
			set
			{
				this.paddingBottom = value;
				parent.UpdateProperty(Types.StylePropertyType.paddingbottom);
			}
		}

		public int? MarginBottom
		{
			get { return this.marginBottom; }
			set
			{
				this.marginBottom = value;
				parent.UpdateProperty(Types.StylePropertyType.marginbottom);
			}
		}

		public int? MarginLeft
		{
			get { return this.marginLeft; }
			set
			{
				this.marginLeft = value;
				parent.UpdateProperty(Types.StylePropertyType.marginleft);
			}
		}

		public int? MarginRight
		{
			get { return this.marginRight; }
			set
			{
				this.marginRight = value;
				parent.UpdateProperty(Types.StylePropertyType.marginright);
			}
		}

		public int? MarginTop
		{
			get { return this.marginTop; }
			set
			{
				this.marginTop = value;
				parent.UpdateProperty(Types.StylePropertyType.margintop);
			}
		}
		
		/// <summary>
		/// Space on the left side of the background that should be considered border.
		/// </summary>
		public int? BorderLeft
		{
			get { return this.borderLeft; }
			set
			{
				this.borderLeft = value;
				parent.UpdateProperty(Types.StylePropertyType.borderleft);
			}
		}
		
		/// <summary>
		/// Space on the right side of the background that should be considered border.
		/// </summary>
		public int? BorderRight
		{
			get { return this.borderRight; }
			set
			{
				this.borderRight = value;
				parent.UpdateProperty(Types.StylePropertyType.borderright);
			}
		}
		
		/// <summary>
		/// Space on the top side of the background that should be considered border.
		/// </summary>
		public int? BorderTop
		{
			get { return this.borderTop; }
			set
			{
				this.borderTop = value;
				parent.UpdateProperty(Types.StylePropertyType.bordertop);
			}
		}
		
		/// <summary>
		/// Space on the bottom side of the background that should be considered border.
		/// </summary>
		public int? BorderBottom
		{
			get { return this.borderBottom; }
			set
			{
				this.borderBottom = value;
				parent.UpdateProperty(Types.StylePropertyType.borderbottom);
			}
		}

		/// <summary>
		/// Extra space to be added to the left side of the background image. 
		/// </summary>
		public int? OverflowLeft
		{
			get { return this.overflowLeft; }
			set
			{
				this.overflowLeft = value;
				parent.UpdateProperty(Types.StylePropertyType.overflowleft);
			}
		}

		/// <summary>
		/// Extra space to be added to the right side of the background image. 
		/// </summary>
		public int? OverflowRight
		{
			get { return this.overflowRight; }
			set
			{
				this.overflowRight = value;
				parent.UpdateProperty(Types.StylePropertyType.overflowright);
			}
		}

		/// <summary>
		/// Extra space to be added to the top side of the background image. 
		/// </summary>
		public int? OverflowTop
		{
			get { return this.overflowTop; }
			set
			{
				this.overflowTop = value;
				parent.UpdateProperty(Types.StylePropertyType.overflowtop);
			}
		}
		
		/// <summary>
		/// Extra space to be added to the bottom side of the background image. 
		/// </summary>
		public int? OverflowBottom
		{
			get { return this.overflowBottom; }
			set
			{
				this.overflowBottom = value;
				parent.UpdateProperty(Types.StylePropertyType.overflowbottom);
			}
		}

		/// <summary>
		/// Text alignment. 
		/// </summary>
		public TextAnchor? Alignment
		{
			get { return this.alignment; }
			set
			{
				this.alignment = value;
				parent.UpdateProperty(Types.StylePropertyType.alignment);
			}
		}
		
		/// <summary>
		/// Content offset to the left.
		/// </summary>
		public float? ContentOffsetLeft
		{
			get { return this.contentOffsetLeft; }
			set
			{
				this.contentOffsetLeft = value;
				parent.UpdateProperty(Types.StylePropertyType.contentoffsetleft);
			}
		}

		/// <summary>
		/// Content offset to the top.
		/// </summary>
		public float? ContentOffsetTop
		{
			get { return this.contentOffsetTop; }
			set
			{
				this.contentOffsetTop = value;
				parent.UpdateProperty(Types.StylePropertyType.contentoffsettop);
			}
		}
		
		/// <summary>
		/// Font used for the text. 
		/// </summary>
		public Font FontAsset
		{
			get { return this.fontAsset; }
			set
			{
				this.fontAsset = value;
				this.fontPath = null;
				parent.UpdateProperty(Types.StylePropertyType.fontasset);
			}
		}
		
		/// <summary>
		/// Font used for the text.
		/// </summary>
		public string FontPath
		{
			get { return this.fontPath; }
			set
			{
				this.fontPath = value;
				loadFontFromPath();
			}
		}
		
		/// <summary>
		/// Text color.
		/// </summary>
		public Color? TextColor
		{
			get { return this.textColor; }
			set
			{
				this.textColor = value;
				parent.UpdateProperty(Types.StylePropertyType.textcolor);
			}
		}

		/// <summary>
		/// Background image. 
		/// </summary>
		public Texture2D Background
		{
			get { return this.background; }
			set
			{
				this.background = value;
				this.backgroundPath = null;
				this.backgroundColor = null;
				parent.UpdateProperty(Types.StylePropertyType.background);
			}
		}
		
		/// <summary>
		/// Background image. 
		/// </summary>
		public string BackgroundPath
		{
			get { return this.backgroundPath; }
			set
			{
				if (value != null)
				{
					this.backgroundColor = null;
					this.backgroundPath = value;
					loadTextureFromPath(backgroundPath, ref this.background, Types.StylePropertyType.background);
				}
				else
				{
					this.Background = null;
				}
			}
		}
		
		/// <summary>
		/// Background color. 
		/// </summary>
		public Color? BackgroundColor
		{
			get {return this.backgroundColor; }
			set 
			{
				if (value.HasValue)
				{
					Texture2D backgroundTexture = new Texture2D(1, 1);
					backgroundTexture.SetPixel(0, 0, value.Value);
					backgroundTexture.Apply();
					
					this.Background = backgroundTexture; // Let Background property do the rest.
					
					this.backgroundColor = value;
				}
				else
				{
					this.Background = null;
				}
			}
		}

# if (!UNITY_2_6)
# if (!UNITY_2_6_1)
		/// <summary>
		/// Font style used for the text. 
		/// </summary>
		public FontStyle? FontStyling
		{
			get { return this.fontStyling; }
			set
			{
				this.fontStyling = value;
				parent.UpdateProperty(Types.StylePropertyType.fontstyling);
			}
		}

		/// <summary>
		/// Font size used for the text. 
		/// </summary>
		public int? FontSize
		{
			get { return this.fontSize; }
			set
			{
				this.fontSize = value;
				parent.UpdateProperty(Types.StylePropertyType.fontsize);
			}
		}
# endif
# endif
		/// <summary>
		/// Texture when a checkbox is checked. 
		/// </summary>
		public Texture2D CheckImage
		{
			get { return this.checkImage; }
			set
			{
				this.checkImage = value;
				this.checkImagePath = null;
				parent.UpdateProperty(Types.StylePropertyType.checkimage);
			}
		}
		
		/// <summary>
		/// Texture when a checkbox is checked.
		/// </summary>
		public string CheckImagePath
		{
			get { return this.checkImagePath; }
			set
			{
				this.checkImagePath = value; 
				loadTextureFromPath(this.checkImagePath, ref this.checkImage, Types.StylePropertyType.checkimage);
			}
		}
		
		/// <summary>
		/// Texture when a checkbox is unchecked. 
		/// </summary>
		public Texture2D UncheckImage
		{
			get { return this.uncheckImage; }
			set
			{
				this.uncheckImage = value;
				this.uncheckImagePath = null;
				parent.UpdateProperty(Types.StylePropertyType.uncheckimage);
			}
		}
		
		/// <summary>
		/// Texture when a checkbox is unchecked. 
		/// </summary>
		public string UncheckImagePath
		{
			get { return this.uncheckImagePath; }
			set 
			{ 
				this.uncheckImagePath = value;
				loadTextureFromPath(this.uncheckImagePath, ref this.uncheckImage, Types.StylePropertyType.uncheckimage);
			}
		}
		
		/// <summary>
		/// Background of the thumb in sliders and scrollbars. 
		/// </summary>
		public Texture2D ThumbBackground
		{
			get { return this.thumbBackground; }
			set
			{
				this.thumbBackground = value;
				this.thumbBackgroundPath = null;
				parent.UpdateProperty(Types.StylePropertyType.thumbbackground);
			}
		}
		
		/// <summary>
		/// Background of the thumb in sliders and scrollbars. 
		/// </summary>
		public string ThumbBackgroundPath
		{
			get { return this.thumbBackgroundPath; }
			set
			{
				this.thumbBackgroundPath = value;
				loadTextureFromPath(this.thumbBackgroundPath, ref this.thumbBackground, Types.StylePropertyType.thumbbackground);
			}
		}
		
		/// <summary>
		/// Space on the left side of the thumb background that should be considered border.
		/// </summary>
		public int? ThumbBorderLeft
		{
			get { return this.thumbBorderLeft; }
			set
			{
				this.thumbBorderLeft = value;
				parent.UpdateProperty(Types.StylePropertyType.thumbborderleft);
			}
		}
		
		/// <summary>
		/// Space on the right side of the thumb background that should be considered border.
		/// </summary>
		public int? ThumbBorderRight
		{
			get { return this.thumbBorderRight; }
			set
			{
				this.thumbBorderRight = value;
				parent.UpdateProperty(Types.StylePropertyType.thumbborderright);
			}
		}
		
		/// <summary>
		/// Space on the top side of the thumb background that should be considered border.
		/// </summary>
		public int? ThumbBorderTop
		{
			get { return this.thumbBorderTop; }
			set
			{
				this.thumbBorderTop = value;
				parent.UpdateProperty(Types.StylePropertyType.thumbbordertop);
			}
		}
		
		/// <summary>
		/// Space on the bottom side of the thumb background that should be considered border.
		/// </summary>
		public int? ThumbBorderBottom
		{
			get { return this.thumbBorderBottom; }
			set
			{
				this.thumbBorderBottom = value;
				parent.UpdateProperty(Types.StylePropertyType.thumbborderbottom);
			}
		}
		
		/// <summary>
		/// Left space from the individual elements in a list to its content.
		/// </summary>
		public int? ItemPaddingLeft
		{
			get { return this.itemPaddingLeft; }
			set
			{
				this.itemPaddingLeft = value;
				parent.UpdateProperty(Types.StylePropertyType.itempaddingleft);
			}
		}
		
		/// <summary>
		/// Right space from the individual elements in a list to its content.
		/// </summary>
		public int? ItemPaddingRight
		{
			get { return this.itemPaddingRight; }
			set
			{
				this.itemPaddingRight = value;
				parent.UpdateProperty(Types.StylePropertyType.itempaddingright);
			}
		}
		
		/// <summary>
		/// Top space from the individual elements in a list to its content.
		/// </summary>
		public int? ItemPaddingTop
		{
			get { return this.itemPaddingTop; }
			set
			{
				this.itemPaddingTop = value;
				parent.UpdateProperty(Types.StylePropertyType.itempaddingtop);
			}
		}
		
		/// <summary>
		/// Bottom space from the individual elements in a list to its content.
		/// </summary>
		public int? ItemPaddingBottom
		{
			get { return this.itemPaddingBottom; }
			set
			{
				this.itemPaddingBottom = value;
				parent.UpdateProperty(Types.StylePropertyType.itempaddingbottom);
			}
		}
		
		/// <summary>
		/// Background of the individual items in a list. 
		/// </summary>
		public Texture2D ItemBackground
		{
			get { return this.itemBackground; }
			set
			{
				this.itemBackground = value;
				this.itemBackgroundPath = null;
				parent.UpdateProperty(Types.StylePropertyType.itembackground);
			}
		}
		
		/// <summary>
		/// Background of the individual items in a list. 
		/// </summary>
		public string ItemBackgroundPath
		{
			get { return this.thumbBackgroundPath; }
			set
			{
				this.itemBackgroundPath = value;
				loadTextureFromPath(this.itemBackgroundPath, ref this.itemBackground, Types.StylePropertyType.itembackground);
			}
		}
		
		/// <summary>
		/// Text color of the selected item in a list. 
		/// </summary>
		public Color? SelectedTextColor
		{
			get { return this.selectedTextColor; }
			set
			{
				this.selectedTextColor = value;
				parent.UpdateProperty(Types.StylePropertyType.selectedtextcolor);
			}
		}
		
		/// <summary>
		/// Background of the selected item in a list. 
		/// </summary>
		public Texture2D SelectedBackground
		{
			get { return this.selectedBackground; }
			set
			{
				this.selectedBackground = value;
				this.selectedBackgroundPath = null;
				parent.UpdateProperty(Types.StylePropertyType.selectedbackground);
			}
		}
		
		/// <summary>
		/// Background of the selected item in a list. 
		/// </summary>
		public string SelectedBackgroundPath
		{
			get { return this.selectedBackgroundPath; }
			set
			{
				this.selectedBackgroundPath = value;
				loadTextureFromPath(this.selectedBackgroundPath, ref this.selectedBackground, Types.StylePropertyType.selectedbackground);
			}
		}
		
		/// <summary>
		/// Type of this StyleState. 
		/// </summary>
		public Types.StyleStateType Type
		{
			get
			{
				return this.type;
			}
		}
		
		/// <summary>
		/// Set padding for all sides. 
		/// </summary>
		/// <param name="left">
		/// Left padding.
		/// </param>
		/// <param name="right">
		/// Right padding.
		/// </param>
		/// <param name="top">
		/// Top padding.
		/// </param>
		/// <param name="bottom">
		/// Bottom padding.
		/// </param>
		public void SetPadding(int left, int right, int top, int bottom)
		{
			this.PaddingLeft = left;
			this.PaddingRight = right;
			this.PaddingTop = top;
			this.PaddingBottom = bottom;
		}
		
		/// <summary>
		/// Set padding for all sides. 
		/// </summary>
		/// <param name="padding">
		/// Overall padding.
		/// </param>
		public void SetPadding(int padding)
		{
			SetPadding(padding, padding, padding, padding);
		}
		
		/// <summary>
		/// Set item padding for all sides. 
		/// </summary>
		/// <param name="left">
		/// Left item padding.
		/// </param>
		/// <param name="right">
		/// Right item padding.
		/// </param>
		/// <param name="top">
		/// Top item padding.
		/// </param>
		/// <param name="bottom">
		/// Bottom item padding.
		/// </param>
		public void SetItemPadding(int left, int right, int top, int bottom)
		{
			this.ItemPaddingLeft = left;
			this.ItemPaddingRight = right;
			this.ItemPaddingTop = top;
			this.ItemPaddingBottom = bottom;
		}
		
		/// <summary>
		/// Set item padding for all sides. 
		/// </summary>
		/// <param name="padding">
		/// Overall item padding.
		/// </param>
		public void SetItemPadding(int padding)
		{
			SetItemPadding(padding, padding, padding, padding);
		}
		
		/// <summary>
		/// Set border for all sides. 
		/// </summary>
		/// <param name="left">
		/// Left border.
		/// </param>
		/// <param name="right">
		/// Right border.
		/// </param>
		/// <param name="top">
		/// Top border.
		/// </param>
		/// <param name="bottom">
		/// Bottom border.
		/// </param>
		public void SetBorder(int left, int right, int top, int bottom)
		{
			this.BorderLeft = left;
			this.BorderRight = right;
			this.BorderTop = top;
			this.BorderBottom = bottom;
		}
		
		/// <summary>
		/// Set border for all sides. 
		/// </summary>
		/// <param name="border">
		/// Overall border.
		/// </param>
		public void SetBorder(int border)
		{
			SetBorder(border, border, border, border);
		}
		
		/// <summary>
		/// Set overflow for all sides. 
		/// </summary>
		/// <param name="left">
		/// Left overflow.
		/// </param>
		/// <param name="right">
		/// Right overflow.
		/// </param>
		/// <param name="top">
		/// Top overflow.
		/// </param>
		/// <param name="bottom">
		/// Bottom overflow.
		/// </param>
		public void SetOverflow(int left, int right, int top, int bottom)
		{
			this.OverflowLeft = left;
			this.OverflowRight = right;
			this.OverflowTop = top;
			this.OverflowBottom = bottom;
		}
		
		/// <summary>
		/// Set overflow for all sides. 
		/// </summary>
		/// <param name="overflow">
		/// Overall overflow.
		/// </param>
		public void SetOverflow(int overflow)
		{
			SetOverflow(overflow, overflow, overflow, overflow);
		}
		
		/// <summary>
		/// Set thumb border for all sides. 
		/// </summary>
		/// <param name="left">
		/// Left thumb border.
		/// </param>
		/// <param name="right">
		/// Right thumb border.
		/// </param>
		/// <param name="top">
		/// Top thumb border.
		/// </param>
		/// <param name="bottom">
		/// Bottom thumb border.
		/// </param>
		public void SetThumbBorder(int left, int right, int top, int bottom)
		{
			this.ThumbBorderLeft = left;
			this.ThumbBorderRight = right;
			this.ThumbBorderTop = top;
			this.ThumbBorderBottom = bottom;
		}
		
		/// <summary>
		/// Set thumb border for all sides. 
		/// </summary>
		/// <param name="thumbBorder">
		/// Overall thumb border.
		/// </param>
		public void SetThumbBorder(int thumbBorder)
		{
			SetThumbBorder(thumbBorder, thumbBorder, thumbBorder, thumbBorder);
		}
		
		/// <summary>
		/// Load a texture from a path. 
		/// </summary>
		/// <param name="path">
		/// The path to load from.
		/// </param>
		/// <param name="target">
		/// The texture to save to.
		/// </param>
		/// <param name="property">
		/// The type of the property that requested the load.
		/// </param>
		private void loadTextureFromPath(string path, ref Texture2D target, Types.StylePropertyType property)
		{
			if (path != null)
			{
				Texture2D newTexture;
				if(path.ToLower().Equals("empty"))
					newTexture = Types.EmptyTexture;
				else
					newTexture = (Texture2D)parent.LoadAssetFromPath(path);

				target = newTexture;
				parent.UpdateProperty(property);
			}
		}
		
		/// <summary>
		/// Load a font from a path. 
		/// </summary>
		private void loadFontFromPath()
		{
			if (this.fontPath != null)
			{
				Font newFont = (Font)parent.LoadAssetFromPath(this.fontPath);
				if (newFont != null)
				{
					this.fontAsset = newFont;
					parent.UpdateProperty(Types.StylePropertyType.fontasset);
				}
			}
		}
		
		/// <summary>
		/// Check an AssetBundle for Assets that need to be loaded. 
		/// </summary>
		/// <param name="bundleName">
		/// The AssetBundle to load from.
		/// </param>
		public void CheckLoadAssets(string bundleName)
		{
			if(bundleName != null)
			{
				if (this.BackgroundPath != null &&
				   this.BackgroundPath.IndexOf("|") > -1 &&
				   this.BackgroundPath.Substring(0, this.BackgroundPath.IndexOf("|")).Equals(bundleName))
				{
					loadTextureFromPath(backgroundPath, ref this.background, Types.StylePropertyType.background);
				}
				if (this.CheckImagePath != null &&
				   this.CheckImagePath.IndexOf("|") > -1 &&
				   this.CheckImagePath.Substring(0, this.BackgroundPath.IndexOf("|")).Equals(bundleName))
				{
					loadTextureFromPath(this.checkImagePath, ref this.checkImage, Types.StylePropertyType.checkimage);
				}
				if (this.UncheckImagePath != null &&
				   this.UncheckImagePath.IndexOf("|") > -1 &&
				   this.UncheckImagePath.Substring(0, this.UncheckImagePath.IndexOf("|")).Equals(bundleName))
				{
					loadTextureFromPath(this.uncheckImagePath, ref this.uncheckImage, Types.StylePropertyType.uncheckimage);
				}
				if (this.FontPath != null &&
				   this.FontPath.IndexOf("|") > -1 &&
				   this.FontPath.Substring(0, this.FontPath.IndexOf("|")).Equals(bundleName))
				{
					loadFontFromPath();
				}
			}
		}
		
		/// <summary>
		/// Check AssetBundles for Assets that need to be loaded.
		/// </summary>
		public void CheckLoadAssets()
		{
			loadTextureFromPath(this.backgroundPath, ref this.background, Types.StylePropertyType.background);
			loadTextureFromPath(this.checkImagePath, ref this.checkImage, Types.StylePropertyType.checkimage);
			loadTextureFromPath(this.uncheckImagePath, ref this.uncheckImage, Types.StylePropertyType.uncheckimage);
			loadFontFromPath();	
		}
	}
}