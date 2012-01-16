using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GUILib
{
	/// <summary>
	/// IElementCollection dummy to store single elements out of layer context.
	/// </summary>
	/// <author>Manuel Fasse (ven)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 296 $, $Date: 2012-01-16 23:24:28 +0100 (Mon, 16 Jan 2012) $</version>
	public abstract class ACollectionDummy : IElementCollection
	{
		private IElement element;
		
		/// <summary>
		/// The stored element.
		/// </summary>
		public IElement Element
		{
			get { return this.element; }
			set
			{
				this.RemoveElement(this.element);
				this.AddElement(value);
			}
		}

		#region IElementCollection Member
		protected string name;
		protected bool visible;

		public void Start()
		{
		}

		public void Update()
		{
		}
		
		/// <summary>
		/// <para>Add the supplied element.</para>
		/// <para>Note: Can only have one.</para>
		/// </summary>
		/// <param name="element">
		/// Element to add.
		/// </param>
		/// <returns>
		/// Whether the operation was successful.
		/// </returns>
		public bool AddElement(IElement element)
		{
			if (this.element == null && element != null)
			{
				this.element = element;
				this.element.Parent = this;
				return true;
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>
		/// <para>Add the supplied element.</para>
		/// <para>Note: Can only have one.</para>
		/// </summary>
		/// <param name="index">
		/// Obsolete.
		/// </param>
		/// <param name="element">
		/// Element to add.
		/// </param>
		/// <returns>
		/// Whether the operation was successful.
		/// </returns>
		public bool InsertElement(int index, IElement element)
		{
			return this.AddElement(element);
		}
		
		/// <summary>
		/// Removes the given element.
		/// </summary>
		/// <param name="element">
		/// The element to be removed.
		/// </param>
		/// <returns>
		/// Whether the operation was successful.
		/// </returns>
		public bool RemoveElement(IElement element)
		{
			if (element != null && this.element == element)
			{
				this.element = null;
				element.Parent = null;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Removes any element.
		/// </summary>
		public void Clear()
		{
			this.RemoveElement(this.element);
		}

		/// <summary>
		/// Name of the containing element.
		/// </summary>
		public string Name
		{
			get
			{
				if (this.element != null)
				{
					return this.ToString() + ": \"" + this.element.Name + "\"";
				}
				return this.ToString();
			}
			set { }
		}
		
		/// <summary>
		/// <para>Returns the children of this collection.</para>
		/// <para>Note: Read-only.</para>
		/// </summary>
		public System.Collections.ObjectModel.ReadOnlyCollection<IElement> Children
		{
			get
			{
				List<IElement> list = new List<IElement>();
				if (this.element != null)
				{
					list.Add(this.element);
				}
				return list.AsReadOnly();
			}
		}
		
		/// <summary>
		/// Returns the amount of children in the collection.
		/// </summary>
		public int Count
		{
			get
			{
				if (this.element != null)
				{
					return 1;
				}
				return 0;
			}
		}
		
		/// <summary>
		/// <para>Returns whether the collection is enabled.</para>
		/// <para>Note: has no effect.</para>
		/// </summary>
		public bool IsEnabled
		{
			get { return this.Enabled; }
		}
		
		/// <summary>
		/// <para>Returns whether the collection is enabled.</para>
		/// <para>Note: has no effect.</para>
		/// </summary>
		public bool Enabled
		{
			get { return true; }
			set { LayerManager.Instance.LogWarning(this.ToString() + " is a wrapper. Enabled property has no effect."); }
		}
		
		/// <summary>
		/// Returns whether the Dummy will draw its element.
		/// </summary>
		public bool IsVisible
		{
			get { return this.Visible; }
		}
		
		/// <summary>
		/// Gets and sets whether the Dummy will draw its element.
		/// </summary>
		public bool Visible
		{
			get { return this.visible; }
			set { this.visible = value; }
		}

		#endregion	
	}
}