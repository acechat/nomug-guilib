//--------------------//
// Settings
//--------------------//
// uncomment this if you don't want to use the console
#define LOGGER_USE_CONSOLE
// use unity's debug logging
#define LOGGER_USE_UNITY_DEBUG
// use log4net (will only work if you don't use WEBPLAYER build settings)
#define LOGGER_USE_LOG4NET

//--------------------//
// Debug Level
//--------------------//

// enables debug messages
#define LOGGER_DEBUG
// enables warnings
#define LOGGER_WARN
// enables errors
#define LOGGER_ERROR

using UnityEngine;
using System.Collections;
#if !(UNITY_WEBPLAYER)
#if LOGGER_USE_LOG4NET
using log4net;
using System.IO;
#endif
#endif

namespace GUILib
{
	/// <summary>
	/// Records logs and manages various ways of debugging. 
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 243 $, $Date: 2011-06-13 21:40:45 +0200 (Mon, 13 Jun 2011) $</version>
	public class Logger
	{
		private string logger;
		private bool enabled;
		private ConsoleOut consoleOut;
		
		/// <summary>
		/// Constructs a logger. You should use GetLogger of Console.
		/// </summary>
		/// <param name="logger">The logger name.</param>
		/// <param name="consoleOut">Consoles method to notify a message.</param>
		public Logger(string logger, ConsoleOut consoleOut)
		{
			this.logger = logger;
			this.enabled = true;
			this.consoleOut = consoleOut;
		}
		
		/// <summary>
		/// The loggers name. 
		/// </summary>
		public string Name
		{
			get { return this.logger; }
		}
		
		/// <summary>
		/// Status of logger. 
		/// </summary>
		public bool Enabled
		{
			get { return this.enabled; }
			set {
				if (!"Console".Equals(this.logger))
				{
					this.enabled = value;
				}
			}
		}
		
		/// <summary>
		/// Log something.
		/// </summary>
		/// <param name="msg">Message which should be logged.</param>
		public void Debug(string msg)
		{
#if LOGGER_DEBUG
			if (this.enabled)
			{
#if LOGGER_USE_CONSOLE
				this.consoleOut("[" + this.logger + "] " + msg, GUILib.Types.LoggerLevel.Debug);
#endif
			
#if LOGGER_USE_UNITY_DEBUG
				UnityEngine.Debug.Log("[" + this.logger + "] " + msg);
#endif
			
#if !(UNITY_WEBPLAYER)
#if LOGGER_USE_LOG4NET
				log4net.LogManager.GetLogger(this.logger).Debug(msg);
#endif
#endif
			}
#endif
		}
		
		/// <summary>
		/// Warn about something.
		/// </summary>
		/// <param name="msg">Warning message which should be logged.</param>
		public void Warn(string msg)
		{
#if LOGGER_WARN
			if (this.enabled)
			{
#if LOGGER_USE_CONSOLE
				this.consoleOut("[" + this.logger + "] " + msg, GUILib.Types.LoggerLevel.Warn);
#endif
			
#if LOGGER_USE_UNITY_DEBUG
				UnityEngine.Debug.LogWarning("[" + this.logger + "] " + msg);
#endif
			
#if !(UNITY_WEBPLAYER)
#if LOGGER_USE_LOG4NET
				log4net.LogManager.GetLogger(this.logger).Warn(msg);
#endif
#endif
			}
#endif
		}
		
		/// <summary>
		/// Logs an error.
		/// </summary>
		/// <param name="msg">Error message which should be logged.</param>
		public void Error(string msg)
		{
#if LOGGER_ERROR
			if (this.enabled)
			{
#if LOGGER_USE_CONSOLE
				this.consoleOut("[" + this.logger + "] " + msg, GUILib.Types.LoggerLevel.Error);
#endif
			
#if LOGGER_USE_UNITY_DEBUG
				UnityEngine.Debug.LogError("[" + this.logger + "] " + msg);
#endif
			
#if !(UNITY_WEBPLAYER)
#if LOGGER_USE_LOG4NET
				log4net.LogManager.GetLogger(this.logger).Error(msg);
#endif
#endif
			}
#endif
		}
		
		
		/// <summary>
		/// Initialises logging of log4net.
		/// Needs to be called once at startup of the application.
		/// </summary>
		public static void InitLogging()
		{
#if !(UNITY_WEBPLAYER)
#if LOGGER_USE_LOG4NET
			UnityEngine.Debug.Log("Initializing Logger");
			
			//Setting the Path to the Config
			string xmlFile = Application.dataPath + "/Resources/config/log4net.xml";
			
			//init
			FileInfo fInfo = new FileInfo(xmlFile);
			log4net.Config.XmlConfigurator.Configure(fInfo);
			log4net.ILog root = log4net.LogManager.GetLogger("root");
			root.Debug("-------------------");
			root.Debug("-------------------");
			root.Debug("Logging initialised");
#endif
#endif
		}
		
	}
}
