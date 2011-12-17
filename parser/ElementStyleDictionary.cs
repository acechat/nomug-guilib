using System.Collections.Generic;

namespace GUILib
{
	/// <summary>
	/// Pre-cache class for a single element Style.
	/// </summary>
	/// <author>Matthias Stock (mstock)</author>
	/// <author>$LastChangedBy: mstock $</author>
	/// <version>$Rev: 270 $, $Date: 2011-07-01 14:36:45 +0200 (Fr, 01 Jul 2011) $</version>
	public class ElementStyleDictionary
	{
		public Dictionary<string, string> Normal;
		public Dictionary<string, string> Active;
		public Dictionary<string, string> Hover;
		public Dictionary<string, string> Focused;
		
		public string FileName;
	
		/// <summary>
		/// Standard constructor creates all sub classes.
		/// </summary>
		public ElementStyleDictionary()
		{
			this.Normal = new Dictionary<string, string>();
			this.Active = new Dictionary<string, string>();
			this.Hover = new Dictionary<string, string>();
			this.Focused = new Dictionary<string, string>();
			
			this.FileName = "";
		}
	}
}