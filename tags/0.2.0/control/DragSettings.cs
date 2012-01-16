using UnityEngine;


namespace GUILib
{
	/// <summary>
	/// Contains options for displaying the drag cursor.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 296 $, $Date: 2012-01-16 23:24:28 +0100 (Mon, 16 Jan 2012) $</version>
	public class DragSettings
	{
		/// <summary>
		/// <para>If false the LayerManager will make a clone of the element that is being dragged and will show it at the mouse position.</para>
		/// <para>You can set this to true to use a custom element which can be set with <see cref="DragElement">DragElement</see>.</para>
		/// </summary>
		public bool ShowOwnDragMarker;
		/// <summary>
		/// Hides the mouse cursor while dragging if set.
		/// </summary>
		public bool HideCursorWhileDragging;

		public Vector2 MouseOffset;

		/// <summary>
		/// Creates the default drag settings.
		/// </summary>
		public DragSettings() : this(null, null, null)
		{

		}
		
		/// <summary>
		/// Creates your customized drag settings. You may pass null as parameters to get the default values.
		/// </summary>
		/// <param name="showOwnDragMarker">Whether a custom element is used as the drag cursor.</param>
		/// <param name="hideCursorWhileDragging">Whether the cursor is hidden while dragging.</param>
		/// <param name="mouseOffset">The offset between the mouse cursor and the drag cursor.</param>
		public DragSettings(bool? showOwnDragMarker, bool? hideCursorWhileDragging, Vector2? mouseOffset)
		{
			if (showOwnDragMarker.HasValue)
			{
				this.ShowOwnDragMarker = showOwnDragMarker.Value;
			}
			else
			{
				// Set the default value.
				this.ShowOwnDragMarker = false;
			}

			if (hideCursorWhileDragging.HasValue)
			{
				this.HideCursorWhileDragging = hideCursorWhileDragging.Value;
			}
			else
			{
				// Set the default value.
				this.HideCursorWhileDragging = false;
			}

			if (mouseOffset.HasValue)
			{
				this.MouseOffset = mouseOffset.Value;
			}
			else
			{
				this.MouseOffset = new Vector2(10, 10);
			}
		}
	}
}