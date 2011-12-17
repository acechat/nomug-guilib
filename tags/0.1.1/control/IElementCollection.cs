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
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 289 $, $Date: 2011-07-29 19:01:01 +0200 (Fr, 29 Jul 2011) $</version>
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
		/// Removes all children.
		/// </summary>
		void Clear();

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
