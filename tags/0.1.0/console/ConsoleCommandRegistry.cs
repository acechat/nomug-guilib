using System.Collections.Generic;

namespace GUILib
{
	/// <summary>
	/// Manages all CommandContainers and handles all execute requests for a specific console command.
	/// </summary>
	/// <author>Matthias Stock (mstock)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 261 $, $Date: 2011-06-25 22:13:03 +0200 (Sa, 25 Jun 2011) $</version>
	public class ConsoleCommandRegistry
	{
		private static ConsoleCommandRegistry instance = null;
		
		public static ConsoleCommandRegistry Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ConsoleCommandRegistry();
				}
				return instance;
			}
		}

		private List<ACommandContainer> containerList;
		
		/// <summary>
		/// Standard Constructor.
		/// </summary>
		private ConsoleCommandRegistry()
		{
			containerList = new List<ACommandContainer>();
		}

		/// <summary>
		/// Registers given command container and adds it to the internal list.
		/// </summary>
		/// <param name="container">
		/// An implementation of AContainer.
		/// </param>
		public void Register(ACommandContainer container)
		{
			containerList.Add(container);
		}

		/// <summary>
		/// Unregisters given command container and removes it from the internal list if it is present in list.
		/// </summary>
		/// <param name="container">
		/// An implementation of AContainer which is present in list.
		/// </param>
		public void UnRegister(ACommandContainer container)
		{
			if (this.containerList.Contains(container))
			{
				this.containerList.Remove(container);
			}
		}

		/// <summary>
		/// Iterates trough all registered CommandContainers, searches for the given command and executes it when it is found.
		/// </summary>
		/// <param name="input">
		/// String representation of the command itself.
		/// </param>
		/// <returns>
		/// True if Command was found and executed. False else.
		/// </returns>
		public bool ExecuteCommand(string input)
		{
			string command;
			string parameters;
			int firstSpace;
			
			firstSpace = input.IndexOf(' ');
			
			if (firstSpace != -1)
			{
				command = input.Substring(0, firstSpace);
				parameters = input.Substring(firstSpace + 1);
			}
			else 
			{
				command = input;
				parameters = "";
			}
			
			foreach (ACommandContainer commandContainer in this.containerList)
			{
				if (commandContainer.HasCommand(command))
				{
					return commandContainer.Execute(command, parameters);
				}
			}
			return false;
		}
		
		/// <summary>
		/// Get possible commands with a given prefix.
		/// </summary>
		/// <param name="prefix">The first characters of </param>
		/// <returns>An array with possible commands.</returns>
		public string[] GetCommands(string prefix)
		{
			List<string> commands = new List<string>();
			
			foreach(ACommandContainer container in this.containerList)
			{
				foreach(string command in container.ContainerDictionary.Keys)
				{
					if (command.StartsWith(prefix))
					{
						commands.Add(command);
					}
				}
			}
			
			commands.Sort();
			
			return commands.ToArray();
		}
	}
}