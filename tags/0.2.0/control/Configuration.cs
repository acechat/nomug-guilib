namespace GUILib
{
	/// <summary>
	/// This class contains all constants of the library.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 296 $, $Date: 2012-01-16 23:24:28 +0100 (Mon, 16 Jan 2012) $</version>
	public class Configuration
	{
		/// <summary>
		/// <para>The maximum of the allowed z-index for layers. This value should be less than Int32.MaxValue.</para>
		/// <para>Note: Modal layers will be one higher than MaxZIndex and console layers can choose any ZIndex.</para>
		/// </summary>
		public const int MaxZIndex = 999999;

		/// <summary>
		/// The key which toggles the console layer.
		/// </summary>
		public const UnityEngine.KeyCode ConsoleKey = UnityEngine.KeyCode.F12;
		
		
		/// <summary>
		/// How many log messages should be saved.
		/// </summary>
		public const int LogMessageLimit = 500;
	}
}