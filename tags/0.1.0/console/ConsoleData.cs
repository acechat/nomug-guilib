using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GUILib
{
	/// <summary>
	/// This class holds the log messages.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 243 $, $Date: 2011-06-13 21:40:45 +0200 (Mo, 13 Jun 2011) $</version>
	public class ConsoleData
	{
		private List<KeyValuePair<GUILib.Types.LoggerLevel, string>> messages = new List<KeyValuePair<GUILib.Types.LoggerLevel, string>>(Configuration.LogMessageLimit);
		
		private ConsoleDataChanged dataChanged;
		
		/// <summary>
		/// Constructs a ConsoleData class.
		/// </summary>
		/// <param name="dataChanged">Delegate for notify the console that data changed.</param>
		public ConsoleData(ConsoleDataChanged dataChanged)
		{
			this.dataChanged = dataChanged;
		}
		
		/// <summary>
		/// Get all messages including their log level.
		/// </summary>
		public ReadOnlyCollection<KeyValuePair<GUILib.Types.LoggerLevel, string>> Messages
		{
			get { return this.messages.AsReadOnly(); }
		}
		
		/// <summary>
		/// Add a new message.
		/// </summary>
		/// <param name="message">Message to be added.</param>
		/// <param name="level">The log level of the message.</param>
		public void AddMessage(string message, GUILib.Types.LoggerLevel level)
		{
			this.messages.Add(new KeyValuePair<GUILib.Types.LoggerLevel, string>(level, message));
			if (this.messages.Count > Configuration.LogMessageLimit)
			{
				this.messages.RemoveAt(0);
			}
			this.dataChanged();
		}
	}
}
