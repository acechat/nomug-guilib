using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GUILib 
{
	/// <summary>
	/// This is the abstract class from which all overlays need to inherit.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 292 $, $Date: 2011-08-01 18:52:27 +0200 (Mo, 01 Aug 2011) $</version>
	public abstract class AOverlay : IElementCollection, IKeyEvents
	{
		private int zIndex;
		private bool isModal;

		/// <summary>
		/// Constructor.
		/// </summary>
		public AOverlay() 
		{
			this.zIndex = 0;
			this.children = new List<IElement>();
			this.Enabled = true;
			this.Visible = true;
		}
		
		/// <summary>
		/// Modal overlay constructor.
		/// </summary>
		/// <param name="modal">Whether the overlay shall be modal.</param>
		public AOverlay(bool modal) : this()
		{
			if (modal)
			{
				this.zIndex = Configuration.MaxZIndex + 1;
				this.isModal = modal;
			}
		}

		/// <summary>
		/// Whether the overlay is modal. Modal overlays will block interaction with other overlays.
		/// </summary>
		public bool IsModal 
		{
			get { return this.isModal; }
		}

		/// <summary>
		/// The index of the overlay on the pseudo z-axis.
		/// </summary>
		public int ZIndex 
		{
			get { return this.zIndex; }
			set
			{
				bool specialOverride = (this is AConsoleOverlay);
				if (!this.IsModal || specialOverride)
				{
					if (value <= Configuration.MaxZIndex || specialOverride)
					{
						this.zIndex = value;
						OverlayManager.Instance.SortLayers();
					}
					else
					{
						OverlayManager.Instance.LogWarning("ZIndex of \"" + this.Name + "\" exceeds the maximum value of " + Configuration.MaxZIndex + ".");
					}
				}
				else
				{
					OverlayManager.Instance.LogError("Can't set ZIndex of modal overlay \"" + this.Name + "\".");
				}
			}
		}

		public virtual void Start() 
		{

		}

		public virtual void Update() 
		{

		}
		
		/// <summary>
		/// Will be called on repaint. You may need to override this method if you want to use CalcSize or CalcHeight. 
		/// </summary>
		public virtual void OnRepaint()
		{
			
		}

		/// <summary>
		/// Draw the children.
		/// </summary>
		public void DrawGUI() 
		{
			foreach (IElement element in this.Children) 
			{
				if (element.Visible)
				{
					element.DrawGUI();
				}
			}
		}

		/// <summary>
		/// Overlays don't have a parent. Always returns null.
		/// </summary>
		public IElement Parent 
		{
			get { return null; }
			set { throw new Exception("Can't set parent of an overlay"); }
		}

		/// <summary>
		/// Is the overlay enabled.
		/// </summary>
		public bool IsEnabled
		{
			get { return this.enabled && !(OverlayManager.Instance.ModalOverlay != null && OverlayManager.Instance.ModalOverlay != this); }
		}

		/// <summary>
		/// Is the overlay enabled.
		/// </summary>
		public bool Enabled 
		{
			get { return this.enabled; }
			set { this.enabled = value; }
		}

		/// <summary>
		/// Is the overlay visible.
		/// </summary>
		public bool IsVisible 
		{
			get {
				if (!this.visible)
					return false;
				foreach (AOverlay ov in OverlayManager.Instance.Overlays)
				{
					if (ov == this)
						return true;
				}
				return false;
			}
		}

		/// <summary>
		/// The overlay visibility.
		/// </summary>
		public bool Visible
		{
			get { return this.visible; }
			set { this.visible = value; }
		}

		#region IElementCollection Member

		private bool enabled;
		private bool visible;
		private List<IElement> children;

		/// <summary>
		/// Add a new element.
		/// </summary>
		/// <param name="element">The element to add.</param>
		/// <returns>True if the element was successfully added.</returns>
		public bool AddElement(IElement element) 
		{
			if (element is AOverlay) {
				OverlayManager.Instance.LogError("Adding failed. Overlay can't be a child.");
				return false;
			}
			if (element.Parent != null) {
				OverlayManager.Instance.LogWarning("Adding failed. " + element.Name + " is already present in a collection. Try cloning it.");
				return false;
			}

			this.children.Add(element);
			element.Parent = this;
			return true;
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
			
			if (element is AOverlay)
			{
				OverlayManager.Instance.LogError("Insertion failed. Overlay can't be a child.");
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

			this.children.Insert(index, element);
			element.Parent = this;
			return true;
		}

		/// <summary>
		/// Remove an element.
		/// </summary>
		/// <param name="element">The element that should be removed.</param>
		/// <returns>True if the element was successfully removed.</returns>
		public bool RemoveElement(IElement element) 
		{
			if (this.children.Remove(element)) {
				element.Parent = null;
				return true;
			} 
			else 
			{
				OverlayManager.Instance.LogWarning("Removal failed. The element " + element.Name + " is not a child of " + this.Name + ".");
			}
			return false;
		}

		/// <summary>
		/// Removes all children.
		/// </summary>
		public void Clear()
		{
			List<IElement> oldChildren = this.children;
			this.children = new List<IElement>();
			foreach (IElement child in oldChildren)
			{
				child.Parent = null;
			}
		}

		public string Name
		{
			get { return this.ToString(); }
			set { }
		}

		/// <summary>
		/// Returns a collection of the children.
		/// </summary>
		public System.Collections.ObjectModel.ReadOnlyCollection<IElement> Children 
		{
			get { return this.children.AsReadOnly(); }
		}

		/// <summary>
		/// Returns the number of the attached children.
		/// </summary>
		public int Count 
		{
			get { return this.children.Count; }
		}

		#endregion
		
		#region IKeyEvents implementation
		
		private OnKeyDelegate keyDownStorage;
		private OnKeyDelegate keyUpStorage;
		
		public event OnKeyDelegate KeyDown
		{
			add { keyDownStorage = value; }
			remove { keyDownStorage = null; }
		}

		public event OnKeyDelegate KeyUp
		{
			add { keyUpStorage = value; }
			remove { keyUpStorage = null; }
		}
		
		public virtual bool OnKeyDown (KeyCode key)
		{
			return this.IsEnabled && this.keyDownStorage != null && keyDownStorage(null, key);
		}

		public virtual bool OnKeyUp (KeyCode key)
		{
			return this.IsEnabled && this.keyUpStorage != null && keyUpStorage(null, key);
		}
		
		#endregion
	}
}