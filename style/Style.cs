using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GUILib
{
	/// <summary>
	/// Class containing all non state-dependent properties of a style.
	/// </summary>
	/// <author>Peer Adelt (adelt)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 286 $, $Date: 2011-07-28 16:47:17 +0200 (Do, 28 Jul 2011) $</version>
	public class Style
	{
		private string name;
		private string text;
		private float? top;
		private float? left;
		private float? width;
		private float? height;
		private bool? enabled;
		private bool? visible;
		private bool? wordWrap;
		private TextClipping? clipping;
		private ImagePosition? checkImagePosition;
		private float? thumbHeight;
		private float? thumbWidth;
		private float? itemHeight;
		
		private StyleState normal;
		private StyleState active;
		private StyleState hover;
		private StyleState focused;
		
		private List<WeakReference> updateHandlerList;
		
		public event UpdateHandler CallUpdate
		{
			add
			{
				if (value != null)
				{
					this.updateHandlerList.Add(new WeakReference(value, false));
				}
			}
			remove
			{
				this.unregisterHandler(value);
			}
		}
		
		/// <summary>
		/// Creates a new Style. 
		/// </summary>
		public Style()
		{
			this.updateHandlerList = new List<WeakReference>();
			
			// initialise stylestates
			normal = new StyleState(this, Types.StyleStateType.Normal);
			active = new StyleState(this, Types.StyleStateType.Active);
			hover = new StyleState(this, Types.StyleStateType.Hover);
			focused = new StyleState(this, Types.StyleStateType.Focused);
		}
		
		/// <summary>
		/// Copies a Style. 
		/// </summary>
		/// <returns>
		/// The copied Style.
		/// </returns>
		public Style Copy()
		{
			Style copy = (Style)this.MemberwiseClone();
			
			copy.updateHandlerList = new List<WeakReference>();
			copy.normal = this.normal.Copy(copy);
			copy.hover = this.hover.Copy(copy);
			copy.focused = this.focused.Copy(copy);
			copy.active = this.active.Copy(copy);
			copy.CheckLoadAssets();
			
			return copy;
		}
		
		/// <summary>
		/// Loads an Asset from an AssetBundle or Resource path. 
		/// </summary>
		/// <param name="path">
		/// The path to load from.
		/// </param>
		/// <returns>
		/// The Asset loaded.
		/// </returns>
		internal object LoadAssetFromPath(string path)
		{
			object obj = null;
			if (path.IndexOf("|") > -1)
			{
				string bundleName = path.Substring(0, path.IndexOf("|"));
				string assetName = path.Substring(path.IndexOf("|") + 1);
				obj = LoadAssetFromPath(bundleName, assetName);
			}
			else
				obj = Resources.Load(path);
			return obj;
		}
		
		/// <summary>
		/// Loads an Asset from an AssetBundle.
		/// </summary>
		/// <param name="bundleName">
		/// The name of the AssetBundle to load from.
		/// </param>
		/// <param name="assetName">
		/// The name of the Asset to load.
		/// </param>
		/// <returns>
		/// The Asset loaded.
		/// </returns>
		internal object LoadAssetFromPath(string bundleName, string assetName)
		{
			AssetBundleRegistry AR = OverlayManager.Instance.AssetBundleRegistry;
			object obj = null;
			if (AR.Contains(bundleName))
			{
				AssetBundle bundle = AR.Get(bundleName);
				obj = bundle.Load(assetName);
			}
			else
				AssetNotifier.Instance.AddListener(bundleName, this);
			return obj;
		}
		
		/// <summary>
		/// Retrieves a style property based on its PropertyType. 
		/// </summary>
		/// <param name="property">
		/// The type of the property.
		/// </param>
		/// <param name="state">
		/// The state to load from.
		/// </param>
		/// <returns>
		/// The property requested.
		/// </returns>
		public object GetProperty(Types.StylePropertyType property, Types.StyleStateType state)
		{
			StyleState styleState;
			switch (state)
			{
				default:
				case Types.StyleStateType.Normal:
					styleState = this.Normal;
					break;
				case Types.StyleStateType.Hover:
					styleState = this.Hover;
					break;
				case Types.StyleStateType.Focused:
					styleState = this.Focused;
					break;
				case Types.StyleStateType.Active:
					styleState = this.Active;
					break;
			}

			object value = null;
			switch (property)
			{
				case Types.StylePropertyType.height:
					value = this.Height;
					break;
				case Types.StylePropertyType.width:
					value = this.Width;
					break;
				case Types.StylePropertyType.top:
					value = this.Top;
					break;
				case Types.StylePropertyType.left:
					value = this.Left;
					break;
				case Types.StylePropertyType.text:
					value = this.Text;
					break;
				case Types.StylePropertyType.name:
					value = this.Name;
					break;
				case Types.StylePropertyType.enabled:
					value = this.Enabled;
					break;
				case Types.StylePropertyType.visible:
					value = this.Visible;
					break;
				case Types.StylePropertyType.wordwrap:
					value = this.WordWrap;
					break;
				case Types.StylePropertyType.clipping:
					value = this.Clipping;
					break;
				case Types.StylePropertyType.checkimageposition:
					value = this.CheckImagePosition;
					break;
				case Types.StylePropertyType.thumbheight:
					value = this.ThumbHeight;
					break;
				case Types.StylePropertyType.thumbwidth:
					value = this.ThumbWidth;
					break;
				case Types.StylePropertyType.thumbbordertop:
					value = this.ThumbBorderTop;
					break;
				case Types.StylePropertyType.thumbborderbottom:
					value = this.ThumbBorderBottom;
					break;
				case Types.StylePropertyType.thumbborderleft:
					value = this.ThumbBorderLeft;
					break;
				case Types.StylePropertyType.thumbborderright:
					value = this.ThumbBorderRight;
					break;
				case Types.StylePropertyType.itemheight:
					value = this.ItemHeight;
					break;
				case Types.StylePropertyType.itempaddingtop:
					value = styleState.ItemPaddingTop;
					break;
				case Types.StylePropertyType.itempaddingleft:
					value = styleState.ItemPaddingLeft;
					break;
				case Types.StylePropertyType.itempaddingright:
					value = styleState.ItemPaddingRight;
					break;
				case Types.StylePropertyType.itempaddingbottom:
					value = styleState.ItemPaddingBottom;
					break;
				case Types.StylePropertyType.itembackground:
					value = styleState.ItemBackground;
					break;
				case Types.StylePropertyType.selectedtextcolor:
					value = styleState.SelectedTextColor;
					break;
				case Types.StylePropertyType.selectedbackground:
					value = styleState.SelectedBackground;
					break;
# if (!UNITY_2_6)
# if (!UNITY_2_6_1)
				case Types.StylePropertyType.fontstyling:
					value = styleState.FontStyling;
					break;
				case Types.StylePropertyType.fontsize:
					value = styleState.FontSize;
					break;
# endif
# endif
				case Types.StylePropertyType.paddingtop:
					value = styleState.PaddingTop;
					break;
				case Types.StylePropertyType.paddingleft:
					value = styleState.PaddingLeft;
					break;
				case Types.StylePropertyType.paddingright:
					value = styleState.PaddingRight;
					break;
				case Types.StylePropertyType.paddingbottom:
					value = styleState.PaddingBottom;
					break;
					
				case Types.StylePropertyType.margintop:
					value = styleState.MarginTop;
					break;
				case Types.StylePropertyType.marginleft:
					value = styleState.MarginLeft;
					break;
				case Types.StylePropertyType.marginright:
					value = styleState.MarginRight;
					break;
				case Types.StylePropertyType.marginbottom:
					value = styleState.MarginBottom;
					break;
				
				case Types.StylePropertyType.bordertop:
					value = styleState.BorderTop;
					break;
				case Types.StylePropertyType.borderleft:
					value = styleState.BorderLeft;
					break;
				case Types.StylePropertyType.borderright:
					value = styleState.BorderRight;
					break;
				case Types.StylePropertyType.borderbottom:
					value = styleState.BorderBottom;
					break;
					
				case Types.StylePropertyType.overflowtop:
					value = styleState.OverflowTop;
					break;
				case Types.StylePropertyType.overflowleft:
					value = styleState.OverflowLeft;
					break;
				case Types.StylePropertyType.overflowright:
					value = styleState.OverflowRight;
					break;
				case Types.StylePropertyType.overflowbottom:
					value = styleState.OverflowBottom;
					break;

				case Types.StylePropertyType.alignment:
					value = styleState.Alignment;
					break;
				case Types.StylePropertyType.contentoffsetleft:
					value = styleState.ContentOffsetLeft;
					break;
				case Types.StylePropertyType.contentoffsettop:
					value = styleState.ContentOffsetTop;
					break;
				case Types.StylePropertyType.fontasset:
					value = styleState.FontAsset;
					break;
				case Types.StylePropertyType.textcolor:
					value = styleState.TextColor;
					break;
				case Types.StylePropertyType.background:
					value = styleState.Background;
					break;
				case Types.StylePropertyType.checkimage:
					value = styleState.CheckImage;
					break;
				case Types.StylePropertyType.uncheckimage:
					value = styleState.UncheckImage;
					break;
				case Types.StylePropertyType.thumbbackground:
					value = styleState.ThumbBackground;
					break;
				default: break;
			}
			return value;
		}
		
		/// <summary>
		/// Top positioning of an element using this style.
		/// </summary>
		public float? Top
		{
			get
			{
				return this.top;
			}
			set
			{
				this.top = value;
				UpdateProperty(Types.StylePropertyType.top);
			}
		}
		
		/// <summary>
		/// Left positioning of an element using this style. 
		/// </summary>
		public float? Left
		{
			get
			{
				return this.left;
			}
			set
			{
				this.left = value;
				UpdateProperty(Types.StylePropertyType.left);
			}
		}
		
		/// <summary>
		/// Width of an element using this style. 
		/// </summary>
		public float? Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
				UpdateProperty(Types.StylePropertyType.width);
			}
		}
		
		/// <summary>
		/// Height of an element using this style. 
		/// </summary>
		public float? Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
				UpdateProperty(Types.StylePropertyType.height);
			}
		}

		/// <summary>
		/// Name of an element using this style. 
		/// </summary>
		public string Name
		{
			get { return this.name; }
			set
			{
				this.name = value;
				UpdateProperty(Types.StylePropertyType.name);
			}
		}
		
		/// <summary>
		/// Text content of an element using this style. 
		/// </summary>
		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				this.text = value;
				UpdateProperty(Types.StylePropertyType.text);
			}
		}
		
		/// <summary>
		/// Enabled status of an element using this style. 
		/// </summary>
		public bool? Enabled
		{
			get { return this.enabled; }
			set {
				this.enabled = value;
				this.UpdateProperty(Types.StylePropertyType.enabled);
			}
		}
		
		/// <summary>
		/// Visibility of an element using this style. 
		/// </summary>
		public bool? Visible
		{
			get { return this.visible; }
			set {
				this.visible = value;
				this.UpdateProperty(Types.StylePropertyType.visible);
			}
		}
		
		/// <summary>
		/// Word wrapping of the text. 
		/// </summary>
		public bool? WordWrap
		{
			get { return this.wordWrap; }
			set
			{
				this.wordWrap = value;
				UpdateProperty(Types.StylePropertyType.wordwrap);
			}
		}
		
		/// <summary>
		/// Whether the content should overflow the element constraints.
		/// </summary>
		public TextClipping? Clipping
		{
			get { return this.clipping; }
			set
			{
				this.clipping = value;
				UpdateProperty(Types.StylePropertyType.clipping);
			}
		}
		
		/// <summary>
		/// Which side the check image should be positioned. 
		/// </summary>
		public ImagePosition? CheckImagePosition
		{
			get { return this.checkImagePosition; }
			set
			{
				this.checkImagePosition = value;
				UpdateProperty(Types.StylePropertyType.checkimageposition);
			}
		}
		
		/// <summary>
		/// Height of the draggable thumb in sliders and scrollbars.
		/// </summary>
		public float? ThumbHeight
		{
			get { return this.thumbHeight; }
			set
			{
				this.thumbHeight = value;
				UpdateProperty(Types.StylePropertyType.thumbheight);
			}
		}
		
		/// <summary>
		/// Width of the draggable thumb in sliders and scrollbars.
		/// </summary>
		public float? ThumbWidth
		{
			get { return this.thumbWidth; }
			set
			{
				this.thumbWidth = value;
				UpdateProperty(Types.StylePropertyType.thumbwidth);
			}
		}
		
		/// <summary>
		/// Height of the individual items in a list.
		/// </summary>
		public float? ItemHeight
		{
			get { return this.itemHeight; }
			set
			{
				this.itemHeight = value;
				UpdateProperty(Types.StylePropertyType.itemheight);
			}
		}
		
		/// <summary>
		/// Left space from the element to its content. 
		/// </summary>
		public int? PaddingLeft
		{
			get { return Normal.PaddingLeft; }
			set { Normal.PaddingLeft = value; }
		}

		/// <summary>
		/// Right space from the element to its content. 
		/// </summary>
		public int? PaddingRight
		{
			get { return Normal.PaddingRight; }
			set { Normal.PaddingRight = value; }
		}
		
		/// <summary>
		/// Top space from the element to its content. 
		/// </summary>
		public int? PaddingTop
		{
			get { return Normal.PaddingTop; }
			set { Normal.PaddingTop = value; }
		}
		
		/// <summary>
		/// Bottom space from the element to its content. 
		/// </summary>
		public int? PaddingBottom
		{
			get { return Normal.PaddingBottom; }
			set { Normal.PaddingBottom = value; }
		}
		
		public int? MarginLeft
		{
			get { return Normal.MarginLeft; }
			set { Normal.MarginLeft = value; }
		}

		public int? MarginRight
		{
			get { return Normal.MarginRight; }
			set { Normal.MarginRight = value; }
		}

		public int? MarginTop
		{
			get { return Normal.MarginTop; }
			set { Normal.MarginTop = value; }
		}

		public int? MarginBottom
		{
			get { return Normal.MarginBottom; }
			set { Normal.MarginBottom = value; }
		}
	
		/// <summary>
		/// Space on the left side of the background that should be considered border.
		/// </summary>
		public int? BorderLeft
		{
			get { return Normal.BorderLeft; }
			set { Normal.BorderLeft = value; }
		}
		
		/// <summary>
		/// Space on the right side of the background that should be considered border.
		/// </summary>
		public int? BorderRight
		{
			get { return Normal.BorderRight; }
			set { Normal.BorderRight = value; }
		}
		
		/// <summary>
		/// Space on the top side of the background that should be considered border.
		/// </summary>
		public int? BorderTop
		{
			get { return Normal.BorderTop; }
			set { Normal.BorderTop = value; }
		}

		/// <summary>
		/// Space on the bottom side of the background that should be considered border.
		/// </summary>
		public int? BorderBottom
		{
			get { return Normal.BorderBottom; }
			set { Normal.BorderBottom = value; }
		}
		
		/// <summary>
		/// Extra space to be added to the left side of the background image. 
		/// </summary>
		public int? OverflowLeft
		{
			get { return Normal.OverflowLeft; }
			set { Normal.OverflowLeft = value; }
		}

		/// <summary>
		/// Extra space to be added to the right side of the background image. 
		/// </summary>
		public int? OverflowRight
		{
			get { return Normal.OverflowRight; }
			set { Normal.OverflowRight = value; }
		}

		/// <summary>
		/// Extra space to be added to the top side of the background image. 
		/// </summary>
		public int? OverflowTop
		{
			get { return Normal.OverflowTop; }
			set { Normal.OverflowTop = value; }
		}

		/// <summary>
		/// Extra space to be added to the bottom side of the background image. 
		/// </summary>
		public int? OverflowBottom
		{
			get { return Normal.OverflowBottom; }
			set { Normal.OverflowBottom = value; }
		}
		
		/// <summary>
		/// Text alignment. 
		/// </summary>
		public TextAnchor? Alignment
		{
			get { return Normal.Alignment; }
			set { Normal.Alignment = value; }
		}
		
		/// <summary>
		/// Content offset to the left.
		/// </summary>
		public float? ContentOffsetLeft
		{
			get { return Normal.ContentOffsetLeft; }
			set { Normal.ContentOffsetLeft = value; }
		}

		/// <summary>
		/// Content offset to the top.
		/// </summary>
		public float? ContentOffsetTop
		{
			get { return Normal.ContentOffsetTop; }
			set { Normal.ContentOffsetTop = value; }
		}
		
		/// <summary>
		/// Font used for the text. 
		/// </summary>
		public Font FontAsset
		{
			get { return Normal.FontAsset; }
			set { Normal.FontAsset = value; }
		}
		
		/// <summary>
		/// Font used for the text.
		/// </summary>
		public string FontPath
		{
			get { return Normal.FontPath; }
			set { Normal.FontPath = value; }
		}
		
		/// <summary>
		/// Text color.
		/// </summary>
		public Color? TextColor
		{
			get { return Normal.TextColor; }
			set { Normal.TextColor = value; }
		}
		
		/// <summary>
		/// Background image. 
		/// </summary>
		public Texture2D Background
		{
			get { return Normal.Background; }
			set { Normal.Background = value; }
		}
		
		/// <summary>
		/// Background image. 
		/// </summary>
		public string BackgroundPath
		{
			get { return Normal.BackgroundPath; }
			set { Normal.BackgroundPath = value; }
		}
		
		/// <summary>
		/// Set a background color as background texture.
		/// </summary>
		public Color? BackgroundColor
		{
			get { return Normal.BackgroundColor; }
			set { Normal.BackgroundColor = value; }
		}

# if (!UNITY_2_6)
# if (!UNITY_2_6_1)
		/// <summary>
		/// Font style used for the text. 
		/// </summary>
		public FontStyle? FontStyling
		{
			get { return Normal.FontStyling; }
			set { Normal.FontStyling = value; }
		}

		/// <summary>
		/// Font size used for the text. 
		/// </summary>
		public int? FontSize
		{
			get { return Normal.FontSize; }
			set { Normal.FontSize = value; }
		}
# endif
# endif
		/// <summary>
		/// Texture when a checkbox is checked. 
		/// </summary>
		public Texture2D CheckImage
		{
			get { return Normal.CheckImage; }
			set { Normal.CheckImage = value; }
		}
		
		/// <summary>
		/// Texture when a checkbox is checked.
		/// </summary>
		public string CheckImagePath
		{
			get { return Normal.CheckImagePath; }
			set { Normal.CheckImagePath = value; }
		}
		
		/// <summary>
		/// Texture when a checkbox is unchecked. 
		/// </summary>
		public Texture2D UncheckImage
		{
			get { return Normal.UncheckImage; }
			set { Normal.UncheckImage = value; }
		}
		
		/// <summary>
		/// Texture when a checkbox is unchecked. 
		/// </summary>
		public string UncheckImagePath
		{
			get { return Normal.UncheckImagePath; }
			set { Normal.UncheckImagePath = value; }
		}
		
		/// <summary>
		/// Background of the thumb in sliders and scrollbars. 
		/// </summary>
		public Texture2D ThumbBackground
		{
			get { return Normal.ThumbBackground; }
			set { Normal.ThumbBackground = value; }
		}
		
		/// <summary>
		/// Background of the thumb in sliders and scrollbars. 
		/// </summary>
		public string ThumbBackgroundPath
		{
			get { return Normal.ThumbBackgroundPath; }
			set { Normal.ThumbBackgroundPath = value; }
		}
		
		/// <summary>
		/// Space on the left side of the thumb background that should be considered border.
		/// </summary>
		public int? ThumbBorderLeft
		{
			get { return Normal.ThumbBorderLeft; }
			set { Normal.ThumbBorderLeft = value; }
		}
		
		/// <summary>
		/// Space on the right side of the thumb background that should be considered border.
		/// </summary>
		public int? ThumbBorderRight
		{
			get { return Normal.ThumbBorderRight; }
			set { Normal.ThumbBorderRight = value; }
		}
		
		/// <summary>
		/// Space on the top side of the thumb background that should be considered border.
		/// </summary>
		public int? ThumbBorderTop
		{
			get { return Normal.ThumbBorderTop; }
			set { Normal.ThumbBorderTop = value; }
		}
		
		/// <summary>
		/// Space on the bottom side of the thumb background that should be considered border.
		/// </summary>
		public int? ThumbBorderBottom
		{
			get { return Normal.ThumbBorderBottom; }
			set { Normal.ThumbBorderBottom = value; }
		}
		
		/// <summary>
		/// Left space from the individual elements in a list to its content.
		/// </summary>
		public int? ItemPaddingLeft
		{
			get { return Normal.ItemPaddingLeft; }
			set
			{
				Normal.ItemPaddingLeft = value;
			}
		}
		
		/// <summary>
		/// Right space from the individual elements in a list to its content.
		/// </summary>
		public int? ItemPaddingRight
		{
			get { return Normal.ItemPaddingRight; }
			set
			{
				Normal.ItemPaddingRight = value;
			}
		}
		
		/// <summary>
		/// Top space from the individual elements in a list to its content.
		/// </summary>
		public int? ItemPaddingTop
		{
			get { return Normal.ItemPaddingTop; }
			set
			{
				Normal.ItemPaddingTop = value;
			}
		}
		
		/// <summary>
		/// Bottom space from the individual elements in a list to its content.
		/// </summary>
		public int? ItemPaddingBottom
		{
			get { return Normal.ItemPaddingBottom; }
			set
			{
				Normal.ItemPaddingBottom = value;
			}
		}
		
		/// <summary>
		/// Background of the individual items in a list. 
		/// </summary>
		public Texture2D ItemBackground
		{
			get { return Normal.ItemBackground; }
			set { Normal.ItemBackground = value; }
		}
		
		/// <summary>
		/// Background of the individual items in a list. 
		/// </summary>
		public string ItemBackgroundPath
		{
			get { return Normal.ItemBackgroundPath; }
			set { Normal.ItemBackgroundPath = value; }
		}
		
		/// <summary>
		/// Text color of the selected item in a list. 
		/// </summary>
		public Color? SelectedTextColor
		{
			get { return Normal.SelectedTextColor; }
			set { Normal.SelectedTextColor = value; }
		}
		
		/// <summary>
		/// Background of the selected item in a list. 
		/// </summary>
		public Texture2D SelectedBackground
		{
			get { return Normal.SelectedBackground; }
			set { Normal.SelectedBackground = value; }
		}
		
		/// <summary>
		/// Background of the selected item in a list. 
		/// </summary>
		public string SelectedBackgroundPath
		{
			get { return Normal.SelectedBackgroundPath; }
			set { Normal.SelectedBackgroundPath = value; }
		}
		
		/// <summary>
		/// StyleState Normal. 
		/// </summary>
		public StyleState Normal
		{
			get { return this.normal; }
		}
		
		/// <summary>
		/// StyleState Active. 
		/// </summary>
		public StyleState Active
		{
			get { return this.active; }
		}
		
		/// <summary>
		/// StyleState Hover. 
		/// </summary>
		public StyleState Hover
		{
			get { return this.hover; }
		}
		
		/// <summary>
		/// StyleState Focused. 
		/// </summary>
		public StyleState Focused
		{
			get { return this.focused; }
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
			Normal.SetPadding(left, right, top, bottom);
		}
		
		/// <summary>
		/// Set padding for all sides. 
		/// </summary>
		/// <param name="padding">
		/// Overall padding.
		/// </param>
		public void SetPadding(int padding)
		{
			Normal.SetPadding(padding);
		}
		
		/// <summary>
		/// Set item padding for all sides. 
		/// </summary>
		/// <param name="left">
		/// <summary>
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
			Normal.SetItemPadding(left, right, top, bottom);
		}
		
		/// <summary>
		/// Set item padding for all sides. 
		/// </summary>
		/// <param name="padding">
		/// Overall item padding.
		/// </param>
		public void SetItemPadding(int padding)
		{
			Normal.SetItemPadding(padding);
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
			Normal.SetBorder(left, right, top, bottom);
		}
		
		/// <summary>
		/// Set border for all sides. 
		/// </summary>
		/// <param name="border">
		/// Overall border.
		/// </param>
		public void SetBorder(int border)
		{
			Normal.SetBorder(border);
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
			Normal.SetOverflow(left, right, top, bottom);
		}
		
		/// <summary>
		/// Set overflow for all sides. 
		/// </summary>
		/// <param name="overflow">
		/// Overall overflow.
		/// </param>
		public void SetOverflow(int overflow)
		{
			Normal.SetOverflow(overflow);
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
			Normal.SetThumbBorder(left, right, top, bottom);
		}
		
		/// <summary>
		/// Set thumb border for all sides. 
		/// </summary>
		/// <param name="thumbBorder">
		/// Overall thumb border.
		/// </param>
		public void SetThumbBorder(int thumbBorder)
		{
			Normal.SetThumbBorder(thumbBorder);
		}
		
		/// <summary>
		/// Check AssetBundles for Assets that need to be loaded.
		/// </summary>
		public void CheckLoadAssets()
		{
			this.Normal.CheckLoadAssets();
			this.Focused.CheckLoadAssets();
			this.Hover.CheckLoadAssets();
			this.Active.CheckLoadAssets();
		}

		/// <summary>
		/// Check an AssetBundle for Assets that need to be loaded. 
		/// </summary>
		/// <param name="bundleName">
		/// The AssetBundle to load from.
		/// </param>
		public void CheckLoadAssets(string bundleName)
		{
			this.Normal.CheckLoadAssets(bundleName);
			this.Focused.CheckLoadAssets(bundleName);
			this.Hover.CheckLoadAssets(bundleName);
			this.Active.CheckLoadAssets(bundleName);
		}
		
		/// <summary>
		/// Call event when a property changed.
		/// Also removes all entries which are not present anymore due to garbage collection.
		/// </summary>
		/// <param name="type">
		/// The property type that changed.
		/// </param>
		internal void UpdateProperty(Types.StylePropertyType type)
		{
			int i = 0;
			while (i < this.updateHandlerList.Count)
			{
				UpdateHandler handler = this.updateHandlerList[i].Target as UpdateHandler;
				if (handler != null)
				{
					// Handler is present. Execute.
					handler(type);
					i++;
				}
				else
				{
					// Handler expired. Remove.
					this.updateHandlerList.RemoveAt(i);
				}
			}
		}
		
		/// <summary>
		/// Unregisters an update handler.
		/// Also removes all entries which are not present anymore due to garbage collection.
		/// </summary>
		/// <param name="handler">The update handler.</param>
		private void unregisterHandler(UpdateHandler handler)
		{
			int i = 0;
			while (i < this.updateHandlerList.Count)
			{
				UpdateHandler current = this.updateHandlerList[i].Target as UpdateHandler;
				if (current == null || current == handler)
				{
					this.updateHandlerList.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}
	}
}