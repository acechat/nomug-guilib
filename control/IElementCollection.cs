using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GUILib
{
	/// <summary>
	/// Basic interface for collections of IElements.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 209 $, $Date: 2011-05-07 19:37:03 +0200 (Sa, 07 Mai 2011) $</version>
	public interface IElementCollection
	{
		/// <summary>
		/// This method will be called when the scene starts.
		/// </summary>
		void Start();

		/// <summary>
		/// This method will be called once per frame.
		/// </summary>
		void Update();

		bool AddElement(IElement element);
		bool InsertElement(int index, IElement element);
		bool RemoveElement(IElement element);

		/// <summary>
		/// The name of the collection.
		/// </summary>
		string Name
		{
			get;
			set;
		}

		ReadOnlyCollection<IElement> Children
		{
			get;
		}

		int Count
		{
			get;
		}

		/// <summary>
		/// Returns whether the element and its parents are enabled.
		/// </summary>
		bool IsEnabled
		{
			get;
		}

		/// <summary>
		/// Returns whether the element is enabled.
		/// </summary>
		bool Enabled
		{
			get;
			set;
		}

		/// <summary>
		/// Returns whether the element is actually drawn.
		/// </summary>
		bool IsVisible
		{
			get;
		}

		/// <summary>
		/// Returns whether the element is visible.
		/// </summary>
		bool Visible
		{
			get;
			set;
		}
	}
}
