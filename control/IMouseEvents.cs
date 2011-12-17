using UnityEngine;
using System.Collections;

namespace GUILib 
{
	/// <summary>
	/// IMouseEvents.
	/// Interface for MouseEvents.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 243 $, $Date: 2011-06-13 21:40:45 +0200 (Mo, 13 Jun 2011) $</version>
	public interface IMouseEvents 
	{
		/// <summary>
		/// Event delegate for mouseEnter.
		/// </summary>
		event OnMouseDelegate MouseEnter;
		/// <summary>
		/// Event delegate for mouseExit.
		/// </summary>
		event OnMouseDelegate MouseExit;
		/// <summary>
		/// Event delegate for mouseMove.
		/// </summary>
		event OnMouseDelegate MouseMove;


		/// <summary>
		/// This method determines which kind of event needs to be fired (MouseEnter or MouseMove). Only this method will be called by the OverlayManager.
		/// The method is as well responsible for the hover state.
		/// </summary>
		/// <param name="mousePos">Position of the mouse.</param>
		/// <returns>Returns true if an event has been fired.</returns>
		bool OnMouse(Vector2 mousePos);

		/// <summary>
		/// This method fires the mouseEnter events.
		/// </summary>
		/// <param name="mousePos">Position of the mouse.</param>
		/// <returns>Returns true if an event was fired.</returns>
		bool OnMouseEnter(Vector2 mousePos);
		/// <summary>
		/// This method fires the mouseExit events.
		/// </summary>
		/// <param name="src">The reference of the element which was exited.</param>
		/// <param name="mousePos">Last position of the mouse which was INSIDE this element.</param>
		/// <returns>Returns true if an event was fired.</returns>
		bool OnMouseExit(Vector2 mousePos);
		/// <summary>
		/// This method fires the mouseMove events.
		/// </summary>
		/// <param name="mousePos">Position of the mouse.</param>
		/// <returns>Returns true if an event was fired.</returns>
		bool OnMouseMove(Vector2 mousePos);
	}
}
