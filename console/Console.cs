using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This is an example implementation of a console. It shows all messages that the console receives.
/// </summary>
/// <author>Henning Vreyborg (marshen)</author>
/// <author>$LastChangedBy: ven $</author>
/// <version>$Rev: 296 $, $Date: 2012-01-16 23:24:28 +0100 (Mon, 16 Jan 2012) $</version>
public class Console : GUILib.AConsole
{

	private static Console instance;

	private Dictionary<string, GUILib.Logger> loggers;

	private Console() : base()
	{
		this.loggers = new Dictionary<string, GUILib.Logger>();
		
		GUILib.ConsoleCommandRegistry.Instance.Register(new GUILib.UICommandContainer());
	}

	/// <summary>
	/// Console instance. 
	/// </summary>
	public static Console Instance {
		get {
			if (instance == null)
			{
				instance = new Console();
				instance.initialize();
				
				try
				{
					GUILib.Logger.InitLogging();
				}
				catch (System.Exception e)
				{
					instance.GetLogger("Console").Warn("log4net initialization failed. You may want to disable it in 'Logger.cs'.\n" + e.ToString());
				}
			}
			return instance;
		}
	}

	#region AConsole;

	/// <summary>
	/// Returns a logger to a given identifier.
	/// </summary>
	/// <param name="logger">The identifier of the logger.</param>
	/// <returns>The logger.</returns>
	public override GUILib.Logger GetLogger(string logger)
	{
		if (!this.loggers.ContainsKey(logger))
		{
			// Create a new instance if the logger doesn't exist. Pass the console output method.
			this.loggers.Add(logger, new GUILib.Logger(logger, this.Out));
		}
		
		return this.loggers[logger];
	}

	/// <summary>
	/// Get loggers.
	/// </summary>
	/// <returns>An array with all logger names.</returns>
	public override string[] Loggers()
	{
		string[] arr = new string[this.loggers.Count];
		int i = 0;
		foreach (string key in this.loggers.Keys)
		{
			arr[i] = key;
			i++;
		}
		return arr;
	}

	/// <summary>
	/// Initializing all stuff that doesn't work in the constructor like setting layers ZIndex because this needs a assigned Console instance.
	/// </summary>
	protected override void initialize()
	{
		this.consoleLayer = new ConsoleLayer(this.consoleData);
		// setting the ZIndex of the layer to MaxZIndex+2 because modal layers can be MaxZIndex+1.
		this.consoleLayer.ZIndex = GUILib.Configuration.MaxZIndex + 2;
	}
	#endregion
}
