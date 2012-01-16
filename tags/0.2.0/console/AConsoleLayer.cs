namespace GUILib
{
	/// <summary>
	/// Abstract class for displaying console.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 296 $, $Date: 2012-01-16 23:24:28 +0100 (Mon, 16 Jan 2012) $</version>
	public abstract class AConsoleLayer : ALayer
	{
		/// <summary>
		/// Construct a modal console layer. 
		/// </summary>
		public AConsoleLayer() : base(true)
		{
			
		}
		
		/// <summary>
		/// Will be called if the console receives an data changed notification. 
		/// </summary>
		abstract public void NotifyDataChanged();
		
		/// <summary>
		/// Will be called if the console layer is shown. 
		/// </summary>
		abstract public void Show();
	}
}