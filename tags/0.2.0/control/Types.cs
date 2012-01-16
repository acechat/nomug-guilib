using UnityEngine;
using System.Collections;
using System;

namespace GUILib
{
	/// <summary>
	/// This class provides a lot of types and other things which are needed in this framework such as delegate bodies.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 243 $, $Date: 2011-06-13 21:40:45 +0200 (Mon, 13 Jun 2011) $</version>
	public class Types
	{
		/// <summary>
		/// This enum is used for GUI elements that can be drawn in horizontal as well as in vertical alignment. 
		/// </summary>
		public enum Alignment
		{
			HORIZONTAL,
			VERTICAL
		}

		public enum StylePropertyType
		{
			height,
			width,
			top,
			left,
			text,
			name,
			enabled,
			visible,
			wordwrap,
			clipping,
			marginleft,
			margintop,
			marginright,
			marginbottom,
			paddingleft,
			paddingtop,
			paddingright,
			paddingbottom,
			borderleft,
			bordertop,
			borderright,
			borderbottom,
			overflowleft,
			overflowtop,
			overflowright,
			overflowbottom,
			alignment,
			contentoffsetleft,
			contentoffsettop,
			fontasset,
			fontsize,
			fontstyling,
			textcolor,
			background,
			checkimageposition,
			checkimage,
			uncheckimage,
			thumbbackground,
			thumbheight,
			thumbwidth,
			thumbborderleft,
			thumbborderright,
			thumbbordertop,
			thumbborderbottom,
			itembackground,
			selectedtextcolor,
			selectedbackground,
			itemheight,
			itempaddingleft,
			itempaddingright,
			itempaddingtop,
			itempaddingbottom
		}

		public enum StyleStateType
		{
			Normal,
			Active,
			Hover,
			Focused,
		}

		public enum ScrollBar
		{
			NONE,
			BOTH,
			VERTICAL,
			HORIZONTAL,
		}
		
		public enum LoggerLevel
		{
			Debug,
			Warn,
			Error,
		}

		/// <summary>
		/// Empty texture for disabling backgrounds. 
		/// </summary>
		public static Texture2D EmptyTexture
		{
			get
			{
				Texture2D empty = new Texture2D(1,1);
				empty.SetPixel(1,1, new Color(0,0,0,0));
				empty.Apply();
				return empty;
			}
		}
	}
}
