using System;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

namespace GUILib
{
	/// <summary>
	/// Creates a Style object from the parser's already parsed StyleDictionary.
	/// </summary>
	/// <author>Matthias Stock (mstock)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 293 $, $Date: 2012-01-16 19:18:36 +0100 (Mon, 16 Jan 2012) $</version>
	public class StyleFactory
	{
		#region Private Variables
		private Dictionary<string, Style> styleCache;
		private List<string> majorProperties;
		
		private string fileName;
		private string curStyle;
		#endregion
		
		#region Constructors
		/// <summary>
		/// Standard constructor. 
		/// </summary>
		public StyleFactory()
		{
			this.styleCache = new Dictionary<string, Style>();
			this.majorProperties = new List<string>();
			this.majorProperties.Add("name");
			this.majorProperties.Add("text");
			this.majorProperties.Add("wordwrap");
			this.majorProperties.Add("enabled");
			this.majorProperties.Add("visible");
			this.majorProperties.Add("top");
			this.majorProperties.Add("left");
			this.majorProperties.Add("height");
			this.majorProperties.Add("width");
			this.majorProperties.Add("textclipping");
			this.majorProperties.Add("checkimageposition");
			this.majorProperties.Add("thumbheight");
			this.majorProperties.Add("thumbwidth");
			this.majorProperties.Add("itemheight");
			
			this.fileName = "";
		}
		#endregion
	
		#region Public Methods
		/// <summary>
		/// Returns a Style object for the given identifier.
		/// </summary>
		/// <param name="identifier">
		/// String for the unique identifier to get the proper style.
		/// </param>
		/// <param name="dictionary">
		/// The StyleDictionary containing the parsed information from the stylesheets.
		/// </param>
		/// <returns>
		/// The proper Style for the object with the given identifier.
		/// </returns>
		public Style GetStyle(string identifier, StyleDictionary dictionary)
		{
			if(styleCache.ContainsKey(identifier))
			{
				return styleCache[identifier];
			}
			else
			{
				this.fileName = dictionary.Items[identifier].FileName;
				this.curStyle = identifier;
				
				if (dictionary.Items.ContainsKey(identifier))
				{
					ElementStyleDictionary elementDictionary = dictionary.Items[identifier];
					styleCache.Add(identifier, this.makeStyle(elementDictionary));
					return styleCache[identifier];
				}
				else
				{
					return null;
				}
			}
		}
		
		/// <summary>
		/// Modifies a Style object for the given identifier. 
		/// </summary>
		/// <param name="identifier">
		/// Name of the Style properties to load.
		/// </param>
		/// <param name="targetStyle">
		/// Style to modify.
		/// </param>
		/// <param name="dictionary">
		/// StyleDictionary containing the Style properties.
		/// </param>
		public void UpdateStyle(string identifier, Style targetStyle, StyleDictionary dictionary)
		{
			if (targetStyle == null)
			{
				this.warning("UpdateStyle for \"" + identifier + "\" failed because the targetStyle is empty.");
				return;
			}
			if (dictionary.Items.ContainsKey(identifier))
			{
				ElementStyleDictionary elementDictionary = dictionary.Items[identifier];
				this.makeStyle(elementDictionary, targetStyle);
			}
		}
		#endregion
	
		#region Private Methods
		/// <summary>
		/// Generates a Style object for the given identifier.
		/// </summary>
		/// <param name="dictionary">
		/// The dictionary containing all key-value pairs for one object.
		/// </param>
		/// <returns>
		/// The proper Style for the object with the given identifier.
		/// </returns>
		private Style makeStyle(ElementStyleDictionary dictionary)
		{
			return makeStyle(dictionary, new Style());
		}
		
		/// <summary>
		/// Modifies a Style object for the given identifier.
		/// </summary>
		/// <param name="dictionary">
		/// The dictionary containing all key-value pairs for one object.
		/// </param>
		/// <param name="targetStyle">
		/// The Style to modify.
		/// </param>
		/// <returns>
		/// The proper Style for the object with the given identifier.
		/// </returns>
		private Style makeStyle(ElementStyleDictionary dictionary, Style targetStyle)
		{	
			this.getMajorProperties(targetStyle, dictionary.Normal);
			this.getStateProperties(targetStyle.Normal, dictionary.Normal);
			this.getStateProperties(targetStyle.Active, dictionary.Active);
			this.getStateProperties(targetStyle.Hover, dictionary.Hover);
			this.getStateProperties(targetStyle.Focused, dictionary.Focused);
	
			return targetStyle;
		}
	
		/// <summary>
		/// Parses the major properties for all styleStates. 
		/// </summary>
		/// <param name="target">
		/// Target style.
		/// </param>
		/// <param name="container">
		/// State container holding the properties (currently only normal container).
		/// </param>
		private void getMajorProperties(Style target, Dictionary<string, string> container)
		{
			foreach (KeyValuePair<string, string> entry in container)
			{
				switch (entry.Key.ToLower())
				{
					case "name":
						target.Name = entry.Value;
						break;
					case "text":
						target.Text = entry.Value;
						break;
					case "enabled":
						target.Enabled = this.parseBool(entry.Value);
						break;
					case "visible":
						target.Visible = this.parseBool(entry.Value);
						break;
					case "wordwrap":
						target.WordWrap = this.parseBool(entry.Value);
						break;
					case "top":
						target.Top = this.parseFloat(entry.Value);
						break;
					case "left":
						target.Left = this.parseFloat(entry.Value);
						break;
					case "width":
						target.Width = this.parseFloat(entry.Value);
						break;
					case "height":
						target.Height = this.parseFloat(entry.Value);
						break;
					case "textclipping":
						target.Clipping = this.parseTextClipping(entry.Value);
						break;
					case "checkimageposition":
						target.CheckImagePosition = this.parseImagePosition(entry.Value);
						break;
					case "thumbheight":
						target.ThumbHeight = this.parseFloat(entry.Value);
						break;
					case "thumbwidth":
						target.ThumbWidth = this.parseFloat(entry.Value);
						break;
					case "itemheight":
						target.ItemHeight = this.parseFloat(entry.Value);
						break;
					
					default:
						break;
				}
			}
		}
	
		/// <summary>
		/// Parses all properties for the StyleStates.
		/// </summary>
		/// <param name="target">
		/// Target Stylestate.
		/// </param>
		/// <param name="container">
		/// The Container dictionary holding all key-value pairs.
		/// </param>
		private void getStateProperties(StyleState target, Dictionary<string,string> container)
		{
			foreach(KeyValuePair<string,string> entry in container)
			{
				string key = entry.Key.ToLower();
				switch (key)
				{
					case "paddingleft":
						target.PaddingLeft = this.parseInt(entry.Value);
						break;
					case "paddingright":
						target.PaddingRight = this.parseInt(entry.Value);
						break;
					case "paddingtop":
						target.PaddingTop = this.parseInt(entry.Value);
						break;
					case "paddingbottom":
						target.PaddingBottom = this.parseInt(entry.Value);
						break;
					case "marginleft":
						target.MarginLeft = this.parseInt(entry.Value);
						break;
					case "marginright":
						target.MarginRight = this.parseInt(entry.Value);
						break;
					case "margintop":
						target.MarginTop = this.parseInt(entry.Value);
						break;
					case "marginbottom":
						target.MarginBottom = this.parseInt(entry.Value);
						break;
					case "borderleft":
						target.BorderLeft = this.parseInt(entry.Value);
						break;
					case "borderright":
						target.BorderRight = this.parseInt(entry.Value);
						break;
					case "bordertop":
						target.BorderTop = this.parseInt(entry.Value);
						break;
					case "borderbottom":
						target.BorderBottom = this.parseInt(entry.Value);
						break;
					case "overflowleft":
						target.OverflowLeft = this.parseInt(entry.Value);
						break;
					case "overflowright":
						target.OverflowRight = this.parseInt(entry.Value);
						break;
					case "overflowtop":
						target.OverflowTop = this.parseInt(entry.Value);
						break;
					case "overflowbottom":
						target.OverflowBottom = this.parseInt(entry.Value);
						break;
					case "contentoffsetleft":
						target.ContentOffsetLeft = this.parseFloat(entry.Value);
						break;
					case "contentoffsettop":
						target.ContentOffsetTop = this.parseFloat(entry.Value);
						break;
					case "textcolor":
						target.TextColor = this.hexToColor(entry.Value);
						break;
					case "background":
					case "backgroundpath":
						target.BackgroundPath = entry.Value;
						break;
					case "backgroundcolor":
						target.BackgroundColor = this.hexToColor(entry.Value);
						break;
					case "alignment":
						target.Alignment = this.getTextAnchor(entry.Value);
						break;
					case "font":
					case "fontpath":
						target.FontPath = entry.Value;
						break;
					case "checkimage":
					case "checkimagepath":
						target.CheckImagePath = entry.Value;
						break;
					case "uncheckimage":
					case "uncheckimagepath":
						target.UncheckImagePath = entry.Value;
						break;
					case "thumbbackground":
					case "thumbbackgroundpath":
						target.ThumbBackgroundPath = entry.Value;
						break;
					case "thumbborderleft":
						target.ThumbBorderLeft = this.parseInt(entry.Value);
						break;
					case "thumbborderright":
						target.ThumbBorderRight = this.parseInt(entry.Value);
						break;
					case "thumbbordertop":
						target.ThumbBorderTop = this.parseInt(entry.Value);
						break;
					case "thumbborderbottom":
						target.ThumbBorderBottom = this.parseInt(entry.Value);
						break;
					case "itembackground":
					case "itembackgroundpath":
						target.ItemBackgroundPath = entry.Value;
						break;
					case "itempaddingleft":
						target.ItemPaddingLeft = this.parseInt(entry.Value);
						break;
					case "itempaddingright":
						target.ItemPaddingRight = this.parseInt(entry.Value);
						break;
					case "itempaddingtop":
						target.ItemPaddingTop = this.parseInt(entry.Value);
						break;
					case "itempaddingbottom":
						target.ItemPaddingBottom = this.parseInt(entry.Value);
						break;
					case "selectedbackground":
					case "selectedbackgroundpath":
						target.SelectedBackgroundPath = entry.Value;
						break;
					case "selectedtextcolor":
						target.SelectedTextColor = this.hexToColor(entry.Value);
						break;
#if (!UNITY_2_6)
#if (!UNITY_2_6_1)
					case "fontsize":
						target.FontSize = this.parseInt(entry.Value);
						break;
					case "fontstyle":
						target.FontStyling = this.parseFontStyle(entry.Value);
						break;
#endif
#endif
					default:
						if (!this.majorProperties.Contains(key))
						{
							this.warning("\"" + entry.Key + "\" is no valid property! Skipping...");
						}
						break;
				}
			}
		}
	
		/// <summary>
		/// Generates a warning message and prints it to the log.
		/// </summary>
		/// <param name="msg">
		/// Message to be displayed.
		/// </param>
		private void warning(string msg)
		{
			LayerManager.Instance.LogWarning("[StyleParser] " + msg + "\nOccured near '" + this.curStyle + "' in '" + this.fileName + "'.");
		}
	
		/// <summary>
		/// Parses a bool value out of a string.
		/// </summary>
		/// <param name="val">
		/// String Value.
		/// </param>
		/// <returns>
		/// Proper bool value.
		/// </returns>
		private bool parseBool(string val)
		{
			if ("true".Equals(val.ToLower()))
			{
				return true;
			}
			else if ("false".Equals(val.ToLower()))
			{
				return false;
			}
			else
			{
				this.warning(val + " is no valid bool value! Returning false instead!");
				return false;
			}
		}
	
		/// <summary>
		/// Parses a float value out of a string.
		/// </summary>
		/// <param name="val">
		/// String value.
		/// </param>
		/// <returns>
		/// Proper float value.
		/// </returns>
		private float parseFloat(string val)
		{
			try
			{
				return float.Parse(val,System.Globalization.CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
				this.warning(val + " is no valid float value! Returning 0.0 instead!");
				return 0.0f;
			}
		}
	
		/// <summary>
		/// Parses a TextClipping object out of a string.
		/// </summary>
		/// <param name="val">
		/// String value.
		/// </param>
		/// <returns>
		/// A proper TextClipping object. 
		/// </returns>
		private TextClipping parseTextClipping(string val)
		{
			if ("overflow".Equals(val.ToLower())) return TextClipping.Overflow;
			else if ("clip".Equals(val.ToLower())) return TextClipping.Clip;
			else
			{
				this.warning(val + " is no valid TextClipping value! Returning Overflow instead!");
				return TextClipping.Overflow;
			}
		}
	
		/// <summary>
		/// Parses save an int value out of a string. 
		/// </summary>
		/// <param name="val">
		/// String value.
		/// </param>
		/// <returns>
		/// Proper int value.
		/// </returns>
		private int parseInt(string val)
		{
			try
			{
				return Int32.Parse(val);
			}
			catch (Exception)
			{
				this.warning(val + " is no valid Integer value! Returning -1 instead!");
				return -1;
			}
		}
	
		/// <summary>
		/// Converts a RGBA or RGB hex-color string (e.g. #FF00AAFF or #FF00AA) to a Unity Color Object.
		/// </summary>
		/// <param name="hexvalue">Hex value of color as a string.</param>
		/// <returns>Unity Color object.</returns>
		private Color hexToColor(string hexvalue)
		{
			if ((hexvalue.Length != 9 && hexvalue.Length != 7) || !hexvalue.StartsWith("#"))
			{
				//That's no html color string!
				this.warning(hexvalue + " is not a valid 6 or 8 digit Color-String, using #FF0000FF instead!");
				return new Color(1f, 0f, 0f, 1f);
			}
	
			try
			{
				float r = Int32.Parse(hexvalue.Substring(1, 2), NumberStyles.AllowHexSpecifier) / 255f;
				float g = Int32.Parse(hexvalue.Substring(3, 2), NumberStyles.AllowHexSpecifier) / 255f;
				float b = Int32.Parse(hexvalue.Substring(5, 2), NumberStyles.AllowHexSpecifier) / 255f;
	
				if (hexvalue.Length == 9)
				{
					float a = Int32.Parse(hexvalue.Substring(7, 2), NumberStyles.AllowHexSpecifier) / 255f;
					return new Color(r, g, b, a);
				}
				else return new Color(r, g, b, 1f);
			}
			catch (FormatException)
			{
				this.warning(hexvalue + " is not a valid 6 or 8 digit Color-String, using #FF0000FF instead!");
				return new Color(1f, 0f, 0f, 1f);
			}
		}
	
		/// <summary>
		/// Gets a proper TextAnchor object from a given string.
		/// </summary>
		/// <param name="val">Text-Anchor value as string.</param>
		/// <returns>The appropriate TextAnchor object.</returns>
		private TextAnchor getTextAnchor(string val)
		{
			TextAnchor anchor;
	
			switch (val.ToLower())
			{
				case "lowercenter":
					anchor = TextAnchor.LowerCenter;
					break;
				case "lowerleft":
					anchor = TextAnchor.LowerLeft;
					break;
				case "lowerright":
					anchor = TextAnchor.LowerRight;
					break;
				case "middlecenter":
					anchor = TextAnchor.MiddleCenter;
					break;
				case "middleleft":
					anchor = TextAnchor.MiddleLeft;
					break;
				case "middleright":
					anchor = TextAnchor.MiddleRight;
					break;
				case "uppercenter":
					anchor = TextAnchor.UpperCenter;
					break;
				case "upperleft":
					anchor = TextAnchor.UpperLeft;
					break;
				case "upperright":
					anchor = TextAnchor.UpperRight;
					break;
				case "top":
					anchor = TextAnchor.UpperCenter;
					break;
				case "bottom":
					anchor = TextAnchor.LowerCenter;
					break;
				case "center":
					anchor = TextAnchor.MiddleCenter;
					break;
				case "left":
					anchor = TextAnchor.UpperLeft;
					break;
				case "right":
					anchor = TextAnchor.UpperRight;
					break;
				default:
					this.warning(val + " is not a valid Text-Anchor, using UpperLeft instead!");
					anchor = TextAnchor.UpperLeft;
					break;
			}
	
			return anchor;
		}
		
#if (!UNITY_2_6)
#if (!UNITY_2_6_1)
		/// <summary>
		/// Gets a proper FontStyle object from a given string.
		/// </summary>
		/// <param name="val">
		/// FontStyle value as string.
		/// </param>
		private FontStyle parseFontStyle(string val)
		{
			switch (val.ToLower())
			{
				case "normal":
					return FontStyle.Normal;
				case "bold":
					return FontStyle.Bold;
				case "italic":
					return FontStyle.Italic;
				case "boldanditalic":
					return FontStyle.BoldAndItalic;
				default:
					this.warning(val + " is not a valid FontStyle, using Normal instead!");
					return FontStyle.Normal;
			}
		}
#endif
#endif
		
		private ImagePosition parseImagePosition(string val)
		{
			switch (val.ToLower())
			{
				case "above":
					return ImagePosition.ImageAbove;
				case "left":
					return ImagePosition.ImageLeft;
				case "imageonly":
					return ImagePosition.ImageOnly;
				case "textonly":
					return ImagePosition.TextOnly;
				default:
					this.warning(val + " is not a valid ImagePosition, using Left instead!");
					return ImagePosition.ImageLeft;
			}
		}
	
	#endregion
	}
	
}