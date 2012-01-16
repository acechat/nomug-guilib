using UnityEngine;
using System.Collections;

namespace GUILib   
{
	/// <summary>
	/// This is an interface for text change handling.
	/// It is implemented by classes which allow text input by the user, e.g. EditBox.
	/// </summary>
	/// <author>Jannis Drewello (drewello)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 243 $, $Date: 2011-06-13 21:40:45 +0200 (Mon, 13 Jun 2011) $</version>
	public interface IChangeEvents
	{	
		event OnChangeDelegate Changed;
		
		/// <summary>
		/// Gets whether the element allows user interaction and the
        /// content is changeable.
		/// </summary>
		bool IsChangeable
		{
			get;
		}
        
        /// <summary>
        /// Gets or sets whether the text should be changeable. 
        /// </summary>
        bool Changeable
        {
            get;
            set;
        }
		
		/// <summary>
		/// Raises the change event.
		/// </summary>
		bool OnChange();
	}
}

