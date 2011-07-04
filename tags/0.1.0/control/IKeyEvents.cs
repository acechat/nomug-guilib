using UnityEngine;

namespace GUILib
{
	/// <summary>
	/// Interface for elements that need to react to key events.
	/// </summary>
	/// <author>Manuel Fasse (ven)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 243 $, $Date: 2011-06-13 21:40:45 +0200 (Mo, 13 Jun 2011) $</version>
	public interface IKeyEvents
	{
		/// <summary>
		/// Delegate for key down events. 
		/// </summary>
		event OnKeyDelegate KeyDown;
		
		/// <summary>
		/// Delegate for key up events. 
		/// </summary>
		event OnKeyDelegate KeyUp;
		
		/// <summary>
		/// Fires key down events. 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="modifiers"></param>
		bool OnKeyDown(KeyCode key);
		
		/// <summary>
		/// Fires key up events. 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="modifiers"></param>
		bool OnKeyUp(KeyCode key);
	}
}