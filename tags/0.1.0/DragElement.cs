using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GUILib
{
	/// <summary>
	/// With this class you can customize the visualisation of the dragged elements.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 221 $, $Date: 2011-05-21 21:42:01 +0200 (Sa, 21 Mai 2011) $</version>
	public class DragElement : ACollectionDummy
	{
		private DragSettings dragSettings;

		/// <summary>
		/// Creates a drag visualisation element.
		/// </summary>
		/// <param name="dragElement">
		/// <para>The representation of the element which is dragged.</para>
		/// <para>Note: You shouldn't pass an element which is added in any overlay. Use an clone instead.</para>
		/// </param>
		public DragElement(IElement dragElement)
		{
			this.Element = dragElement;
			this.dragSettings = new DragSettings(null, null, null);
			if (this.Element != null)
			{
				this.dragSettings.ShowOwnDragMarker = true;
			}
			this.visible = true;
		}
		
		/// <summary>
		/// Creates a drag visualisation element.
		/// </summary>
		/// <param name="dragElement">
		/// <para>The representation of the element which is dragged.</para>
		/// <para>Note: You shouldn't pass an element which is added in any overlay. Use an clone instead.</para>
		/// </param>
		/// <param name="dragSettings"></param>
		public DragElement(IElement dragElement, DragSettings dragSettings)
		{
			this.Element = dragElement;
			this.dragSettings = dragSettings;
			this.visible = true;
		}

		public void DrawGUI()
		{
			if (this.IsVisible && this.Element != null)
			{
				if (Event.current.type == EventType.Repaint)
				{
					this.Element.Left = Event.current.mousePosition.x + this.dragSettings.MouseOffset.x;
					this.Element.Top = Event.current.mousePosition.y + this.dragSettings.MouseOffset.y;
				}
				this.Element.DrawGUI();
			}
		}

		/// <summary>
		/// Contains options for displaying the drag cursor.
		/// </summary>
		public DragSettings Settings
		{
			get { return this.dragSettings; }
			set { this.dragSettings = value; }
		}
	}
}
