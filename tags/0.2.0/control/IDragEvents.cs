using System.Collections;

namespace GUILib {
	/// <summary>
	/// Interface for drag&drop events.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 296 $, $Date: 2012-01-16 23:24:28 +0100 (Mon, 16 Jan 2012) $</version>
	public interface IDragEvents {
		/// <summary>
		/// Returns whether you can drag this element. The element is draggable if draggable is set
		/// to true and if the element is visible and enabled.
		/// </summary>
		bool IsDraggable {
			get;
		}

		/// <summary>
		/// Enables dragging for this element.
		/// </summary>
		bool Draggable
		{
			get;
			set;
		}

		/// <summary>
		/// Visualisation for the element while being dragged. Leave this empty if you want the default behaviour.
		/// </summary>
		DragElement DragElement
		{
			get;
			set;
		}

		/// <summary>
		/// Event delegate for dragOver.
		/// </summary>
		event OnDragDelegate DragOver;

		/// <summary>
		/// Event delegate for dragDrop.
		/// </summary>
		event OnDragDelegate DragDrop;

		/// <summary>
		/// Event delegate for dragStart.
		/// </summary>
		event OnDragStartDelegate DragStart;

		/// <summary>
		/// This method fires the drag over events. It should only be called by the LayerManager.
		/// </summary>
		/// <param name="dragElement">The element which is dragged.</param>
		/// <returns>Returns true if an event has been fired.</returns>
		bool OnDragOver(IElement dragElement);
		/// <summary>
		/// This method fires the drag drop events if an element is dropped.
		/// </summary>
		/// <param name="dragElement">The element which was dragged.</param>
		/// <returns>Returns true if an event has been fired.</returns>
		bool OnDragDrop(IElement dragElement);
		/// <summary>
		/// This method fires the drag start event to determine wheter an element wants to be dragged.
		/// </summary>
		/// <returns>Returns true if an event has been fired.</returns>
		bool OnDragStart();
	}
}
