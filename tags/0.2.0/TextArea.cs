using UnityEngine;
using System.Collections;
using System;

namespace GUILib 
{
	/// <summary>
	/// An EditBox for user input
	/// </summary>
	/// <author>Manuel Fasse (ven)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 245 $, $Date: 2011-06-13 21:53:17 +0200 (Mon, 13 Jun 2011) $</version>
	public class TextArea : AClickableElement, IChangeEvents, ICloneable
	{
		
		private string tempText;
		
		/// <summary>
		/// Initializes a new TextArea.
		/// </summary>
		public TextArea() : base() 
		{
			this.tempText = "";
			this.Changeable = true;
		}
		
		/// <summary>
		/// Initializes a new TextArea with the properties given by another TextArea.
		/// </summary>
		/// <param name='source'>Source from which to make the copy.</param>
		public TextArea(TextArea source) : base(source) 
		{
			this.tempText = source.tempText;
			this.changeable = source.changeable;
		}
		
		/// <summary>
		/// Draws the TextArea.
		/// </summary>
		public override void DrawGUI() 
		{
			base.DrawGUI();
			GUI.enabled = this.IsEnabled;
			GUI.SetNextControlName(this.GUID);
			// call unity textarea
			this.tempText = GUI.TextArea(this.Rect, this.Text, this.CurrentStyle);
			
			// check whether the text has changed
			if (IsChangeable && !this.Text.Equals(this.tempText))
			{
				this.Text = this.tempText;
				OnChange();
			}
		}
		
		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		public override string Text
		{
			get { return base.Text; }
			set
			{
				base.Text = value;
				this.tempText = value;
			}	
		}
		
		#region IChangeEvents Member
		
		private OnChangeDelegate changeStorage;
		private bool changeable;
		
		/// <summary>
        /// Will be raised when text is changed. Attach your method here.
        /// </summary>
		public event OnChangeDelegate Changed
		{
			add { changeStorage = value; }
			remove{ changeStorage = null; }
		}
		
		/// <summary>
		/// Raises the change event.
		/// </summary>
		public bool OnChange()
		{
			if (changeStorage != null) {
				changeStorage(this);
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// Gets or sets whether the text in this EditBox is changeable.
		/// </summary>
		public bool Changeable
		{
			get { return this.changeable; }
			set { this.changeable = value; }
		}
        
        public bool IsChangeable
        {
            get { return this.Changeable && this.IsEnabled; }
        }
		#endregion

		#region ICloneable Member

		/// <summary>
		/// Creates a duplicate of an existing TextArea.
		/// </summary>
		/// <returns>Returns the cloned TextArea.</returns>
		public object Clone()
		{
			return new TextArea(this);
		}

		#endregion
	}
}
