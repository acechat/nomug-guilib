using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

namespace GUILib
{
	/// <summary>
	/// Parser for reading external stylesheets to format GUI-Elements by ID.
	/// </summary>
	/// <author>Matthias Stock (mstock)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 273 $, $Date: 2011-07-02 19:01:52 +0200 (Sa, 02 Jul 2011) $</version>
	class StyleParser
	{
	
		#region Constructors
		/// <summary>
		/// Standard contructor.
		/// </summary>
		private StyleParser()
		{
			styleDict = new StyleDictionary();
			
			this.isInitialized = false;
		}
		#endregion
	
		#region Singleton
		private static StyleParser instance = null;
	
		/// <summary>
		/// Singleton instance getter.
		/// </summary>
		public static StyleParser Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new StyleParser();
				}
				return instance;
			}
		}
		#endregion
	
		#region Private Variables
	
		private StyleDictionary styleDict;
		private StyleFactory styleFactory = null;
		
		private bool isInitialized;
			
		#endregion
	
		#region Public Methods
		/// <summary>
		/// Initialises the parser and loads all stylesheet files at the given location in the resources folder. 
		/// </summary>
		/// <param name="path">Path to the folder containing the GUI-Stylesheets. Use a Unity Path (e.g. "GUI/Styles/" for <ProjectDir>/Resources/GUI/Styles).</param>
		public void Init(string path)
		{
			if (path.Length < 255)
			{
	
				this.styleDict.Items.Clear();
				UnityEngine.Object[] metafiles = Resources.LoadAll(path, typeof(TextAsset));
	
	
				foreach (TextAsset t in metafiles)
				{
					//Debug.Log(t.name);
					this.parseTextAsset(t);
				}
				
				this.isInitialized = true;
			}
			else throw new IOException("The filepath is to long (max. 255 chars)!");
		}
		
		/// <summary>
		/// Initialises the parser and loads all stylesheet files in the given AssetBundle.
		/// </summary>
		/// <param name="bundle">
		/// Unity AssetBundle containing Stylesheets as txt files.
		/// </param>
		public void Init(AssetBundle bundle)
		{
			this.styleDict.Items.Clear();
			UnityEngine.Object[] metafiles = bundle.LoadAll(typeof(TextAsset));
				
			foreach (TextAsset t in metafiles)
			{
				this.parseTextAsset(t);
			}
			
			this.isInitialized = true;
		}
		
		/// <summary>
		/// Initialises the parser and takes a StringList and handles every containing string as one stylesheet. 
		/// </summary>
		/// <param name="styles">
		/// A Stringlist containing stylerules in every string as if it was a single stylesheet.
		/// </param>
		public void Init(List<String> styles)
		{
				this.styleDict.Items.Clear();
				foreach (String s in styles)
				{
					this.fillDict(s, "Stylesheet given as string");
				}
				
				this.isInitialized = true;
		}
			
		/// <summary>
		/// Gets the ExtendedGUIStyle for the given identifier.
		/// </summary>
		/// <param name="identifier">the identifier for the GUI element as string.</param>
		/// <returns>The proper Style object.</returns>
		public Style GetStyle(string identifier)
		{
			if (!this.isInitialized) OverlayManager.Instance.LogWarning("[StyleParser] Call of GetStyle() before parser was initialized.");
			
			if (this.styleDict.Items.ContainsKey(identifier))
			{
				if (this.styleFactory == null)
				{
					this.styleFactory = new StyleFactory();	
				}
				return this.styleFactory.GetStyle(identifier, styleDict);
			}
			else
			{
				OverlayManager.Instance.LogError("[StyleParser] Can not find style for identifier '" + identifier + "' returning null instead!");
				return null;
			}
		}
		
		/// <summary>
		/// Modifies the given Style. 
		/// </summary>
		/// <param name="identifier">
		/// Name of the Style properties to load.
		/// </param>
		/// The Style to modify.
		/// </param>
		public void UpdateStyle(string identifier, Style targetStyle)
		{
			if (!this.isInitialized) OverlayManager.Instance.LogWarning("[StyleParser] Call of UpdateStyle() before parser was initialized.");
			
			if (this.styleDict.Items.ContainsKey(identifier))
			{
				if (this.styleFactory == null)
				{
					this.styleFactory = new StyleFactory();	
				}
				styleFactory.UpdateStyle(identifier, targetStyle, styleDict);
			}
			else
			{
				OverlayManager.Instance.LogError("[StyleParser] Can not find style for identifier '" + identifier + "'. Doing nothing!");
			}
		}
	
		#endregion
	
		#region Private Methods
	
		/// <summary>
		/// Parses a single GUI stylesheet file located at the given path.
		/// </summary>
		/// <param name="stylesheet">A TextAsset stylesheet resource.</param>
		private void parseTextAsset(TextAsset stylesheet)
		{
			try
			{
				this.fillDict(stylesheet.text, stylesheet.name + ".txt");
			}
			catch (Exception readException)
			{
				OverlayManager.Instance.LogError("[StyleParser] Error while opening " + stylesheet.name + " with Exception: " + readException.ToString());
			}
	
		}
		/// <summary>
		/// Uses a KeyValueParser to get all key-value pairs out of the file.
		/// </summary>
		/// <param name="content">String representation of the file-content.</param>
		/// <exception cref="System.Exception">Thrown if a parsing error occurs.</exception>
		private void fillDict(string content, string filename)
		{
			KeyValueParser parser = new KeyValueParser(content, styleDict, filename);
			parser.Parse();
		}
		#endregion
	
	}
}