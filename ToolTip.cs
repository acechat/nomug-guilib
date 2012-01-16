using UnityEngine;
using System;

namespace GUILib
{
	/// <summary>
	/// ToolTip wrapper to store an IElement which will be shown when mousing over the element it is attached to.
	/// </summary>
	/// <author>Manuel Fasse (ven)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 293 $, $Date: 2012-01-16 19:18:36 +0100 (Mon, 16 Jan 2012) $</version>
	public class ToolTip : ACollectionDummy
	{
		private TooltipSettings settings;
		private bool resize;
		
		/// <summary>
		/// Creates a ToolTip and stores the supplied IElement.
		/// </summary>
		/// <param name="element">The Element that will be shown when the ToolTip is called.</param>
		public ToolTip(IElement element)
		{
			this.settings = new TooltipSettings();
			this.Element = element;
		}
		
		/// <summary>
		/// Creates a ToolTip that displays the given text.
		/// </summary>
		/// <param name="text">Text to display on the ToolTip.</param>
		public ToolTip(string text)
		{
			this.settings = new TooltipSettings();
			
			this.Element = new Label();
			this.Element.Text = text;
			this.Element.DefaultStyle = LayerManager.Instance.GetDefaultStyle(this);
			this.resize = true;
		}
		
		/// <summary>
		/// Updates tooltip position to the one given.
		/// </summary>
		/// <param name="mousePos">Position to move the tooltip to.</param>
		private void updatePosition(Vector2 mousePos)
		{
			if(settings.FollowMouse && this.Element != null)
			{
				// flip mouse anchor to top of the cursor if the tooltip breaks the screen constraints.
				if(mousePos.y + this.Element.Height + this.settings.MouseOffset.y < Screen.height)
				{
					this.Element.Top = Math.Max(mousePos.y + this.settings.MouseOffset.y, 0);
				}
				else
				{
					this.Element.Top = Math.Max(mousePos.y - this.Element.Height - this.settings.MouseOffset.y, 0);
				}
				
				// limit the vertical tooltip position to not break screen constraints.
				this.Element.Left = Math.Max(Math.Min(mousePos.x + this.settings.MouseOffset.x, Screen.width - this.Element.Width), 0);
			}
		}
		
		/// <summary>
		/// Settings to control the display of tooltips.
		/// </summary>
		public TooltipSettings Settings
		{
			get { return this.settings; }
			set {
				if (value != null)
				{
					this.settings = value;
				}
			}
		}
		
		public void DrawGUI()
		{
			if(this.Element != null && this.Element.Visible)
			{
				if(Event.current.type == EventType.Repaint)
					updatePosition(Event.current.mousePosition);
				this.Element.DrawGUI();
			}
		}
		
		/// <summary>
		/// Called each repaint event. Used to resize default tooltips.
		/// </summary>
		public void OnRepaint()
		{
			if (this.resize && this.Element != null)
			{
				Vector2 elementSize = this.Element.CalcSize();
				
				if (elementSize.x > this.settings.MaxWidth)
				{
					this.Element.Width = this.settings.MaxWidth;
					this.Element.Height = this.Element.CalcHeight(this.settings.MaxWidth);
				}
				else
				{
					this.Element.Width = elementSize.x;
					this.Element.Height = elementSize.y;
				}
			}
		}
	}
}