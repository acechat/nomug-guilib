using UnityEngine;
using System;
using System.Collections;

namespace GUILib 
{
	/// <summary>
	/// All clickable gui elements inherit from this class
	/// </summary>
	/// <author>Peer Adelt (adelt)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 292 $ ,$Date: 2011-08-01 18:52:27 +0200 (Mo, 01 Aug 2011) $</version>
	public abstract class AClickableElement : AElement, IClickEvents, IKeyEvents
	{
		/// <summary>
		/// Default constructor (runs AElement constructor).
		/// </summary>
		public AClickableElement() : base() 
		{
		}
		
		/// <summary>
		/// Creates a new AClickableElement with the properties of a given source AClickableElement object.
		/// </summary>
		/// <param name='src'>Pass through of AClickableElement src.</param>
		public AClickableElement(AClickableElement src): base(src)
		{
		}
		
		#region IClickEvents Member

	    private OnClickDelegate clickStorage;
		private OnClickDelegate mouseUpStorage;
		private OnClickDelegate mouseDownStorage;

        /// <summary>
        /// Will be raised when this control is clicked. Attach your method here.
        /// </summary>
		public event OnClickDelegate Click
		{
		    add { clickStorage = value; }
            remove { clickStorage = null; }
		}

		/// <summary>
		/// Will be raised when a mouse button is released at the position of the current control. Attach your method here.
		/// </summary>
		public event OnClickDelegate MouseUp
		{
			add { mouseUpStorage = value; }
			remove { mouseUpStorage = null; }
		}

		/// <summary>
		/// Will be raised when a mouse button is pressed at the position of the current control. Attach your method here.
		/// </summary>
		public event OnClickDelegate MouseDown
		{
			add { mouseDownStorage = value; }
			remove { mouseDownStorage = null; }
		}

		/// <summary>
		/// <para>Is called when a Click event has to be raised.</para>
		/// <para>(Clicks happen if the mouse was pressed and released on the same element.)</para>
		/// </summary>
		/// <returns>Returns true if an event has been raised.</returns>
		public virtual bool OnClick() {
			if (clickStorage != null) {
				clickStorage(this);
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// Is called when a MouseUp event has to be raised.
		/// </summary>
		/// <returns>True if an event has been raised.</returns>
		public virtual bool OnMouseUp()
		{
			if (this.mouseUpStorage != null) 
			{
				this.mouseUpStorage(this);
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// Is called when a MouseDown event has to be raised.
		/// </summary>
		/// <returns>True if an event has been raised.</returns>
		public virtual bool OnMouseDown()
		{
			if (this.mouseDownStorage != null)
			{
				this.mouseDownStorage(this);
				return true;
			}
			return false;
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
			return this.IsEnabled && this.keyDownStorage != null && keyDownStorage(this, key);
		}

		public virtual bool OnKeyUp (KeyCode key)
		{
			return this.IsEnabled && this.keyUpStorage != null && keyUpStorage(this, key);
		}
		
		#endregion
	}
}

