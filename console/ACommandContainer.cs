using System.Collections.Generic;

namespace GUILib
{
	/// <summary>
	/// Delegate Function which will execute the command.
	/// </summary>
	public delegate void ExecuteCommandProc(string parameters);
	
	/// <summary>
	/// Abstract class representing a set of topic related commands with proper delegate function to execute.
	/// </summary>
	/// <author>Matthias Stock (mstock)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 243 $, $Date: 2011-06-13 21:40:45 +0200 (Mo, 13 Jun 2011) $</version>
	public abstract class ACommandContainer
	{
		private Dictionary<string, ExecuteCommandProc> containerDictionary = new Dictionary<string, ExecuteCommandProc>();
		
		public Dictionary<string, ExecuteCommandProc> ContainerDictionary
		{
			get {return this.containerDictionary; }
			set { this.containerDictionary = value; }
		}

		/// <summary>
		/// Standard constructor.
		/// </summary>
		public ACommandContainer ()
		{
		}

		#region Public Methods

		/// <summary>
		/// Lookup for Conatiner if it contains given command.
		/// </summary>
		/// <param name="command">
		/// Command as string.
		/// </param>
		/// <returns>
		/// True if containerDictionary contains given command. False elsewhise.
		/// </returns>
		public bool HasCommand(string command)
		{
			return this.containerDictionary.ContainsKey(command);
		}

		/// <summary>
		/// Executes the delegate wich is related to command and gives it the parameters string.
		/// </summary>
		/// <param name="command">
		/// String representing key for selecting delegate.
		/// </param>
		/// <param name="parameters">
		/// String representing parameters for command. (will be given to the execute delegate as parameter).
		/// </param>
		/// <returns>
		/// True if delegate was executet properly. False otherwise.
		/// </returns>
		public bool Execute(string command, string parameters)
		{
			if (this.HasCommand(command))
			{
				ExecuteCommandProc commandExecution = this.containerDictionary[command];
				commandExecution(parameters);
				return true;
			}
			else return false;
		}
		
		/// <summary>
		/// Converts a string to a bool.
		/// Not case sensitive.
		/// </summary>
		/// <param name="s">The string that should contain a bool value.</param>
		/// <returns>True if the string is not "false", "off" or "0".</returns>
		public bool parseStringToBool(string s)
		{
			bool? result = this.tryParseStringToBool(s);
			return (result.GetValueOrDefault(true));
		}
		
		/// <summary>
		/// Converts a string to a bool if it is well formed.
		/// Not case sensitive.
		/// </summary>
		/// <param name="s">The string that contains a bool value.</param>
		/// <returns>
		/// <para>True if the string is "true", "on" or "1".</para>
		/// <para>False if the string is "false", "off" or "0".</para>
		/// <para>Null otherwise.</para>
		/// </returns>
		public bool? tryParseStringToBool(string s)
		{
			s = s.ToLower();
			if ("true".Equals(s) || "on".Equals(s) || "1".Equals(s))
			{
				return true;
			}
			if ("false".Equals(s) || "off".Equals(s) || "0".Equals(s))
			{
				return false;
			}
			return null;
		}
		
		#endregion
		
		/// <summary>
		/// Prints a message to the console.
		/// </summary>
		/// <param name="message">The message.</param>
		protected void print(string message)
		{
			if (OverlayManager.Instance.Console != null)
			{
				OverlayManager.Instance.Console.Print(message);
			}
		}
	}
}