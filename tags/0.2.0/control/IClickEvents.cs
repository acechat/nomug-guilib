using UnityEngine;
using System.Collections;

namespace GUILib 
{
	/// <summary>
	/// Interface for classes that need to inform user when they are clicked.
	/// </summary>
	/// <author>Peer Adelt (adelt)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 243 $, $Date: 2011-06-13 21:40:45 +0200 (Mon, 13 Jun 2011) $</version>
	public interface IClickEvents
	{
		/// <summary>
		/// Public event to which the user can register an event handler.
		/// </summary>
		event OnClickDelegate Click;
		/// <summary>
		/// Is raised when a mouse button is released over an IElement;
		/// </summary>
		event OnClickDelegate MouseUp;
		/// <summary>
		/// Is raised when a mouse button is pressed over an IElement;
		/// </summary>
		event OnClickDelegate MouseDown;
		
		/// <summary>
		/// Public method that raises the Click event.
		/// This event will only be fired if the element is the active control while the mouse up happens.
		/// </summary>
		bool OnClick();
		/// <summary>
		/// This method fires the mouseUp event.
		/// </summary>
		bool OnMouseUp();
		/// <summary>
		/// This method fires the mouseDown event.
		/// </summary>
		/// </param>
		bool OnMouseDown();
	}
}

