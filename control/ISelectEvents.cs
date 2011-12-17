namespace GUILib
{
	/// <summary>
	/// Interface for elements that contain selectable items like ListBox.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 243 $, $Date: 2011-06-13 21:40:45 +0200 (Mo, 13 Jun 2011) $</version>
	public interface ISelectEvents
	{
		/// <summary>
		/// Event delegate for item selection.
		/// </summary>
		event OnSelectDelegate Select;

		/// <summary>
		/// This method fires the selected event.
		/// </summary>
		void OnSelect();
	}
}