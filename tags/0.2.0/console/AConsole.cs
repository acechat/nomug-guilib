using UnityEngine;
using System.Collections;

namespace GUILib
{
	/// <summary>
	/// Abstract class for console implementations.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 296 $, $Date: 2012-01-16 23:24:28 +0100 (Mon, 16 Jan 2012) $</version>
	abstract public class AConsole
	{
		protected AConsoleLayer consoleLayer;
		protected ConsoleData consoleData;
		
		public AConsole()
		{
			this.consoleData = new ConsoleData(this.notifyDataChanged);
		}
		
		/// <summary>
		/// Returns the reference to an layer that is used as console. 
		/// </summary>
		public AConsoleLayer ConsoleLayer {
			get { return this.consoleLayer; }
		}
		
		/// <summary>
		/// Returns a logger to a given identifier.
		/// </summary>
		/// <param name="logger">The identifier of the logger.</param>
		/// <returns>The logger.</returns>
		abstract public Logger GetLogger(string logger);
		
		/// <summary>
		/// Get all loggers.
		/// </summary>
		/// <returns>An array with all logger names.</returns>
		abstract public string[] Loggers();
		
		/// <summary>
		/// Will be called after the constructor is executed.
		/// </summary>
		abstract protected void initialize();
		
		/// <summary>
		/// Prints a console message to the console. 
		/// </summary>
		/// <param name="message">Message to be printed.</param>
		public void Print(string message)
		{
			this.Out("[Console] " + message, GUILib.Types.LoggerLevel.Debug);
		}
		
		/// <summary>
		/// Display a message to the console.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="level">The log level of the message.</param>
		protected void Out(string message, GUILib.Types.LoggerLevel level)
		{
			this.consoleData.AddMessage(message, level);
		}
		
		/// <summary>
		/// Will be called if ConsoleData is changed.
		/// </summary>
		private void notifyDataChanged()
		{
			// Forward the notification.
			this.consoleLayer.NotifyDataChanged();
		}
	}
}