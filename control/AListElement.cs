using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace GUILib
{
	/// <summary>
	/// Base class for elements which contain a list of items (e.g. ListBox).
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 243 $, $Date: 2011-06-13 21:40:45 +0200 (Mo, 13 Jun 2011) $</version>
	public class AListElement : AClickableElement, ISelectEvents
	{
		protected List<string> items;
		protected int selected;
		protected float itemHeight;

		/// <summary>
		/// Default constructor
		/// </summary>
		public AListElement()
			: base()
		{
			this.items = new List<string>();
			this.selected = -1;
			this.itemHeight = 20f;
		}

		/// <summary>
		/// Creates a AListElement object with the properties of a given AListElement.
		/// </summary>
		/// <param name="source">The source from which the properties will be copied.</param>
		public AListElement(AListElement source)
			: base(source)
		{
			this.items = new List<string>(source.items);
			this.selected = source.selected;
			this.itemHeight = source.itemHeight;
		}

		/// <summary>
		/// Returns a collection of all items.
		/// </summary>
		public ReadOnlyCollection<string> Items
		{
			get { return this.items.AsReadOnly(); }
		}

		/// <summary>
		/// The count of added items.
		/// </summary>
		public int Count
		{
			get { return this.items.Count; }
		}


		/// <summary>
		/// The index of the selected item.
		/// </summary>
		/// <value>-1 for unselected up to Count-1</value>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public int SelectedItem
		{
			get { return this.selected; }
			set
			{
				if (value >= -1 && value < this.items.Count)
				{
					this.selected = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("SelectedItem");
				}
			}
		}

		/// <summary>
		/// Add an item to the ListBox.
		/// </summary>
		/// <param name="item">Item to add.</param>
		public void AddItem(string item)
		{
			this.items.Add(item);
		}

		/// <summary>
		/// Inserts an item at a given position.
		/// </summary>
		/// <param name="index">The position where the item is inserted. Valid values are 0 to Count.</param>
		/// <param name="item">Item to insert.</param>
		public void InsertAt(int index, string item)
		{
			if (index >= 0 && index <= this.items.Count)
			{
				this.items.Insert(index, item);
			}
			else
			{
				throw new ArgumentOutOfRangeException("index");
			}
		}

		/// <summary>
		/// Remove an item by its name.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		/// <returns>Returns true if successful.</returns>
		public bool RemoveItem(string item)
		{
			return this.items.Remove(item);
		}

		/// <summary>
		/// Removes an element at a given position.
		/// </summary>
		/// <param name="index">Position of the item you want to remove.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if index less than 0 or greater or equal than Count</exception>
		public void RemoveAt(int index)
		{
			this.items.RemoveAt(index);
		}

		#region ISelectEvents Member

		private OnSelectDelegate selectStorage;

		/// <summary>
		/// Event that will be fired if an item was selected.
		/// </summary>
		public event OnSelectDelegate Select
		{
			add { selectStorage = value; }
			remove { selectStorage = null; }
		}

		/// <summary>
		/// Is called when an item select event has to be raised.
		/// </summary>
		public void OnSelect()
		{
			if (this.selectStorage != null)
			{
				this.selectStorage(this);
			}
		}

		#endregion

	}
}