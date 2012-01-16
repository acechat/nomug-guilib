using UnityEngine;


namespace GUILib
{
	/// <summary>
	/// Contains Settings to control the display of tooltips.
	/// </summary>
	/// <author>Manuel Fasse (ven)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 221 $, $Date: 2011-05-21 21:42:01 +0200 (Sat, 21 May 2011) $</version>
	public class TooltipSettings
	{
		/// <summary>
		/// Whether the tooltip should follow the mouse. 
		/// </summary>
		public bool FollowMouse;
		
		/// <summary>
		/// The tooltip's offset to the mouse cursor. 
		/// </summary>
		public Vector2 MouseOffset;
		
		/// <summary>
		/// The maximum width a tooltip may have. 
		/// </summary>
		public float MaxWidth;
		
		/// <summary>
		/// Tooltip settings. 
		/// </summary>
		public TooltipSettings() : this(null, null, null)
		{
		}
		
		/// <summary>
		/// Creates Tooltip settings.
		/// </summary>
		/// <param name="followMouse">
		/// Whether the tooltip should follow the mouse.
		/// </param>
		/// <param name="mouseOffset">
		/// The tooltip's offset to the mouse cursor.
		/// </param>
		public TooltipSettings(bool? followMouse, Vector2? mouseOffset, float? maxWidth)
		{
			// followMouse
			if(followMouse.HasValue)
			{
				this.FollowMouse = followMouse.Value;
			}
			else
			{
				this.FollowMouse = true;
			}
			
			// mouseOffset
			if(mouseOffset.HasValue)
			{
				this.MouseOffset = mouseOffset.Value;
			}
			else
			{
				this.MouseOffset = new Vector2(15, 15);
			}
			
			// maxWidth
			if (maxWidth.HasValue)
			{
				this.MaxWidth = maxWidth.Value;
			}
			else
			{
				this.MaxWidth = 150f;	
			}
		}
	}
}