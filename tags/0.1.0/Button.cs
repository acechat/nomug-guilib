using UnityEngine;
using System.Collections;
using System;

namespace GUILib 
{
	/// <summary>
	/// A clickable button.
	/// </summary>
    /// <author>Henning Vreyborg (marshen)</author>
    /// <author>$LastChangedBy: marshen $</author>
    /// <version>$Rev: 254 $, $Date: 2011-06-17 21:08:29 +0200 (Fr, 17 Jun 2011) $</version>
	public class Button : AClickableElement, IDragEvents, ICloneable
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Button() : base() 
		{
			
		}

		/// <summary>
		/// Creates a new Button with the properties of a given Button object.
		/// </summary>
		/// <param name="source">The source from where the properties should be copied.</param>
		public Button(Button source) : base(source) 
		{
			this.draggable = source.draggable;
		}

		/// <summary>
		/// Draw the button.
		/// </summary>
		public override void DrawGUI()
		{
			base.DrawGUI();
			GUI.enabled = this.IsEnabled;
			GUI.Label(this.Rect, this.Text, this.CurrentStyle);
		}

		#region IDragable Member

		private bool draggable;
		private DragElement dragElement;

		private OnDragStartDelegate dragStart;
		private OnDragDelegate dragOver;
		private OnDragDelegate dragDrop;

		public bool IsDraggable
		{
			get { return this.IsVisible && this.IsEnabled && this.draggable; }
		}

		/// <summary>
		/// Is Dragging for this element enabled?
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
			get { return this.dragElement; }
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
		/// If this is null the dragging will start if the element is draggable.
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
		/// Creates a duplicate of an existing Button.
		/// </summary>
		/// <returns>Returns the cloned Button.</returns>
		public object Clone()
		{
			return new Button(this);
		}

		#endregion
	}
}