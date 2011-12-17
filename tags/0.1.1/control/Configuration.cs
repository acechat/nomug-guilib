namespace GUILib
{
	/// <summary>
	/// This class contains all constants of the library.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 243 $, $Date: 2011-06-13 21:40:45 +0200 (Mo, 13 Jun 2011) $</version>
	public class Configuration
	{
		/// <summary>
		/// <para>The maximum of the allowed z-index for overlays. This value should be less than Int32.MaxValue.</para>
		/// <para>Note: Modal overlays will be one higher than MaxZIndex and console overlays can choose any ZIndex.</para>
		/// </summary>
		public const int MaxZIndex = 999999;

		/// <summary>
		/// The key which toggles the console overlay.
		/// </summary>
		public const UnityEngine.KeyCode ConsoleKey = UnityEngine.KeyCode.F12;
		
		
		/// <summary>
		/// How many log messages should be saved.
		/// </summary>
		public const int LogMessageLimit = 500;
	}
}