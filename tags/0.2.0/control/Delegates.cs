// <summary>
// Delegate declarations.
// </summary>
// <author>Henning Vreyborg (marshen)</author>
// <author>$LastChangedBy: marshen $</author>
// <version>$Rev: 254 $, $Date: 2011-06-17 21:08:29 +0200 (Fri, 17 Jun 2011) $</version>
using UnityEngine;

namespace GUILib
{
	/// <summary>
	/// Default click delegate definiton.
	/// </summary>
	/// <param name="src">The clicked element.</param>
	public delegate void OnClickDelegate(IElement src);

	/// <summary>
	/// Delegate body for common mouseEvents.
	/// </summary>
	/// <param name="src">The element which called the delegate.</param>
	/// <param name="mousePos">Position of the mouse.</param>
	public delegate void OnMouseDelegate(IElement src, UnityEngine.Vector2 mousePos);

	/// <summary>
	/// Delegate body of drag start events.
	/// </summary>
	/// <param name="src">The source element from which the event is called.</param>
	/// <returns>.</returns>
	public delegate bool OnDragStartDelegate(IElement src);

	/// <summary>
	/// Common delegate body for drag events.
	/// </summary>
	/// <param name="src">The source element from which the event is called.</param>
	/// <param name="dragElement">The element which is dragged.</param>
	/// <returns></returns>
	public delegate bool OnDragDelegate(IElement src, IElement dragElement);

	/// <summary>
	/// Delegate body for keyboard input change events. 
	/// </summary>
	/// <param name="src">The changed element.</param>
	public delegate void OnChangeDelegate(IElement src);

	/// <summary>
	/// Delegate body for select events.
	/// </summary>
	/// <param name="src"></param>
	public delegate void OnSelectDelegate(IElement src);
	
	/// <summary>
	/// Delegate body for Key events. 
	/// </summary>
	/// <param name="src"></para>
	/// <param name="key"></param>
	/// <param name="modifiers"></para>
	public delegate bool OnKeyDelegate(IElement src, KeyCode key);
	
	public delegate void OnReleaseDelegate(IElement src);

	/// <summary>
	/// Delegate body for asset bundle download completion.
	/// </summary>
	/// <param name="name">The identifier of the asset bundle which has been completed.</param>
	public delegate void AssetBundleLoadCompleted(string name);
	
	/// <summary>
	/// Delegate for internal use. Delegate to notify that property changed.
	/// </summary>
	public delegate void UpdateHandler(Types.StylePropertyType type);
	
	/// <summary>
	/// Delegate to notify if data changed.
	/// </summary>
	public delegate void ConsoleDataChanged();
	
	/// <summary>
	/// Delegate for console messages. 
	/// </summary>
	public delegate void ConsoleOut(string message, Types.LoggerLevel level);
}