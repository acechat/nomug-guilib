using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GUILib
{
	/// <summary>
	/// A box can contain various gui elements.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 243 $, $Date: 2011-06-13 21:40:45 +0200 (Mo, 13 Jun 2011) $</version>
	public class Box : AElement, IElementCollection, IDragEvents, ICloneable
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Box()
			: base()
		{
			this.children = new List<IElement>();
		}

		/// <summary>
		/// Creates a new Box with the properties of a given Box object.
		/// </summary>
		/// <param name="source">The source from where the properties should be copied.</param>
		public Box(Box source)
			: base(source)
		{
			this.children = new List<IElement>();
			// Set capacity so that the List doesn't need to allocate more ram each loop step.
			this.children.Capacity = source.children.Count;

			// Clone the children if they implement IClonable.
			foreach (IElement elem in source.children)
			{
				if (elem is ICloneable)
				{
					IElement clone = (IElement)((ICloneable)elem).Clone();
					this.children.Add(clone);
					clone.Parent = this;
				}
				else
				{
					OverlayManager.Instance.LogError("The element '" + elem.GetType().ToString() + "' doesn't implement ICloneable. '" + source.Name + "' couldn't be cloned completely.");
				}
			}
		}

		/// <summary>
		/// Draw the box.
		/// </summary>
		public override void DrawGUI()
		{
			base.DrawGUI();
			
			GUI.enabled = this.IsEnabled;

			// TODO: discuss if a group should be drawn with the normal guistyle which would result in a background.
			GUI.BeginGroup(this.Rect, this.CurrentStyle);
			
			foreach (IElement element in this.Children)
			{
				if (element.Visible)
				{
					element.DrawGUI();
				}
			}

			GUI.EndGroup();
		}


		/// <summary>
		/// Calculates the size of the content of the box.
		/// </summary>
		/// <returns>The size of the content.</returns>
		public override Vector2 CalcSize()
		{
			if (OverlayManager.Instance.InsideDrawGUI)
			{
				float maxHeight = 0;
				float maxWidth = 0;

				foreach (IElement elem in this.children)
				{
					maxHeight = Math.Max(elem.Top + elem.Height, maxHeight);
					maxWidth = Math.Max(elem.Left + elem.Width, maxWidth);
				}

				return new Vector2(maxWidth, maxHeight);
			}
			else
			{
				throw new System.ArgumentException("CalcHeight can only be called inside DrawGUI.");
			}
		}

		/// <summary>
		/// Calculates the size of the content of the box.
		/// </summary>
		/// <param name="content">Obsolete.</param>
		/// <returns>The size of the content.</returns>
		public override Vector2 CalcSize(GUIContent content)
		{
			return this.CalcSize();
		}

		/// <summary>
		/// Calculates the size of the content of the box.
		/// </summary>
		/// <param name="width">No influence.</param>
		/// <returns>The size of the content.</returns>
		public override float CalcHeight(float width)
		{
			if (OverlayManager.Instance.InsideDrawGUI)
			{
				return this.CalcSize().y;
			}
			else
			{
				throw new System.ArgumentException("CalcHeight can only be called inside DrawGUI.");
			}
		}

		/// <summary>
		/// Calculates the size of the content of the box.
		/// </summary>
		/// <param name="content">Obsolete.</param>
		/// <param name="width">No influence.</param>
		/// <returns>The size of the content.</returns>
		public override float CalcHeight(GUIContent content, float width)
		{
			return this.CalcHeight(0);
		}

		#region IElementCollection Member

		protected List<IElement> children;

		/// <summary>
		/// Adds an element to the box.
		/// </summary>
		/// <param name="element">The element to add.</param>
		/// <returns>True if element was successfully added.</returns>
		public bool AddElement(IElement element)
		{
			return this.InsertElement(this.children.Count, element);
		}

		/// <summary>
		/// Insert a new element at a given position.
		/// </summary>
		/// <param name="index">The position where the item is inserted. Valid values are 0 to Count.</param>
		/// <param name="element">The element to insert.</param>
		/// <returns>True if the element was successfully added.</returns>
		public bool InsertElement(int index, IElement element)
		{
			if (element == null)
			{
				OverlayManager.Instance.LogError("Insertion failed. Given element is null.");
				return false;
			}
			
			if (element.Parent != null)
			{
				OverlayManager.Instance.LogWarning("Insertion failed. " + element.Name + " is already present in a collection. Try cloning it.");
				return false;
			}

			if (index < 0 || index > this.children.Count)
			{
				OverlayManager.Instance.LogWarning("Insertion of '" + element.Name + "' failed. Index is out of range.");
				return false;
			}

			// check for circles
			IElement toCheck = this;
			while (toCheck != null)
			{
				if (toCheck == element)
				{
					// circle found.
					OverlayManager.Instance.LogError("Inserting " + element.Name + " to " + this.Name + " would create a circle. Aborting.");
					return false;
				}
				if (toCheck.Parent is IElement)
				{
					toCheck = toCheck.Parent as IElement;
				}
				else
				{
					break;
				}
			}
			// no circles found. insert element.
			this.children.Insert(index, element);
			element.Parent = this;
			return true;
		}

		/// <summary>
		/// Removes an element from the box.
		/// </summary>
		/// <param name="element">The element which should be removed.</param>
		/// <returns>True if the element was removed.</returns>
		public bool RemoveElement(IElement element)
		{
			// if element was removed (which implies that it was a child) clear its parent
			if (this.children.Remove(element))
			{
				element.Parent = null;
				return true;
			}
			else
			{
				OverlayManager.Instance.LogWarning(this.Name + " does not contain " + element.Name + " so it can not be removed.");
				return false;
			}
		}

		/// <summary>
		/// Returns a collection of the box' children.
		/// </summary>
		public virtual System.Collections.ObjectModel.ReadOnlyCollection<IElement> Children
		{
			get { return this.children.AsReadOnly(); }
		}

		/// <summary>
		/// Returns the amount of child elements.
		/// </summary>
		public int Count
		{
			get { return this.children.Count; }
		}

		#endregion

		#region IDragEvents Member

		private bool draggable;
		private DragElement dragElement;

		private OnDragDelegate dragOver;
		private OnDragDelegate dragDrop;
		private OnDragStartDelegate dragStart;

		/// <summary>
		/// Returns whether you can drag this element. The element is draggable if draggable is set to true and if the element is visible and enabled.
		/// </summary>
		public bool IsDraggable
		{
			get { return this.IsVisible && this.IsEnabled && this.draggable; }
		}

		/// <summary>
		/// Set or get whether the element is draggable.
		/// </summary>
		public bool Draggable
		{
			get { return this.draggable; }
			set { this.draggable = value; }
		}

		/// <summary>
		/// Visualisation for the element while being dragged. Leave this empty if you want the default behaviour.
		/// </summary>
		public DragElement DragElement
		{
			get	{ return this.dragElement; }
			set { this.dragElement = value; }
		}

		/// <summary>
		/// Will be raised when an element is dragged over this element. Attach your method here.
		/// </summary>
		public event OnDragDelegate DragOver
		{
			add { this.dragOver = value; }
			remove { this.dragOver = null; }
		}

		/// <summary>
		/// Will be raised when an element is dropped on this element. Attach your method here.
		/// </summary>
		public event OnDragDelegate DragDrop
		{
			add { this.dragDrop = value; }
			remove { this.dragDrop = null; }
		}

		/// <summary>
		/// Will be raised when the user starts Dragging on this element. Attach your method here.
		/// If this is left null the dragging will start if the element is draggable.
		/// </summary>
		public event OnDragStartDelegate DragStart
		{
			add { this.dragStart = value; }
			remove { this.dragStart = null; }
		}

		/// <summary>
		/// Called if an element is dragged over another. This event checks if the dragged item can be dropped here.
		/// </summary>
		/// <param name="dragElement">The element which is dragged.</param>
		/// <returns>True if the dragOver event was successfully called and if it returned true.</returns>
		public bool OnDragOver(IElement dragElement)
		{
			return this.IsVisible && this.IsEnabled && this.dragOver != null && this.dragOver(this, dragElement);
		}

		/// <summary>
		/// Called if an element is dropped on a button.
		/// </summary>
		/// <param name="dragElement">The element wich was dropped.</param>
		/// <returns>True if the dragDrop event was successfully called and if it returned true.</returns>
		public bool OnDragDrop(IElement dragElement)
		{
			return this.IsVisible && this.IsEnabled && this.dragDrop != null && this.dragDrop(this, dragElement);
		}

		/// <summary>
		/// Called if Dragging is about to start.
		/// </summary>
		/// <returns>Returns true if this element wants to be dragged.</returns>
		public bool OnDragStart()
		{
			return this.IsDraggable && (this.dragStart == null || this.dragStart(this));
		}

		#endregion

		#region ICloneable Member

		/// <summary>
		/// Creates a duplicate of an existing Box including its children.
		/// </summary>
		/// <returns>Returns the cloned Box.</returns>
		public object Clone()
		{
			return new Box(this);
		}

		#endregion
	}
}