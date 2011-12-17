namespace GUILib
{
	/// <summary>
	/// Abstract class for displaying console.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 261 $, $Date: 2011-06-25 22:13:03 +0200 (Sa, 25 Jun 2011) $</version>
	public abstract class AConsoleOverlay : AOverlay
	{
		/// <summary>
		/// Construct a modal console overlay. 
		/// </summary>
		public AConsoleOverlay() : base(true)
		{
			
		}
		
		/// <summary>
		/// Will be called if the console receives an data changed notification. 
		/// </summary>
		abstract public void NotifyDataChanged();
		
		/// <summary>
		/// Will be called if the console overlay is shown. 
		/// </summary>
		abstract public void Show();
	}
}