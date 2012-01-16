using System.Collections.Generic;

namespace GUILib
{
	/// <summary>
	/// Pre-cache class for all parsed stylesheet informations.
	/// </summary>
	/// <author>Matthias Stock (mstock)</author>
	/// <author>$LastChangedBy: mstock $</author>
	/// <version>$Rev: 196 $, $Date: 2011-05-01 13:04:25 +0200 (Sun, 01 May 2011) $</version>
	public class StyleDictionary
	{
		public Dictionary<string, ElementStyleDictionary> Items;
	
		/// <summary>
		/// Standard constructor.
		/// </summary>
		public StyleDictionary()
		{
			this.Items = new Dictionary<string, ElementStyleDictionary>();
		}
	
		/// <summary>
		/// Adds a key to the dictionary safely (so there is no error when the key is already added).
		/// </summary>
		/// <param name="key">The given key as string.</param>
		/// <param name="elementDictionary">The given ElementStyleDict.</param>
		public void AddUnique(string key, ElementStyleDictionary elementDictionary)
		{
			if (!this.Items.ContainsKey(key))
			{
				this.Items.Add(key,elementDictionary);
			}
		}
	}
}