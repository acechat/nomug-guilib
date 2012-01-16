using UnityEngine;
using System.Collections;
using System;

namespace GUILib   
{
	/// <summary>
	/// This is a class for an one line EditBox which can be used for user input.
	/// </summary>
	/// <author>Jannis Drewello (drewello)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 245 $, $Date: 2011-06-13 21:53:17 +0200 (Mon, 13 Jun 2011) $</version>
	public class EditBox : AClickableElement, IChangeEvents, ICloneable
	{		
		private string tempText;
		private bool masked = false;
		
		/// <summary>
		/// Initializes a new instance of class EditBox.
		/// </summary>
		public EditBox() : base()
		{
			this.changeable = true;
			this.tempText = "";
		}
		
		/// <summary>
		/// Initializes a new instance of class EditBox and copies its values from an existing object.
		/// </summary>
		/// <param name='source'>
		/// Object which should be copied from.
		/// </param>
		public EditBox(EditBox source) : base(source)
		{
			this.changeable = source.changeable;
			this.tempText = source.tempText;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="GUILib.EditBox"/> is masked.
		/// </summary>
		/// <value>
		/// <c>true</c> if masked; otherwise, <c>false</c>.
		/// </value>
		public bool Masked
		{
			get { return this.masked; }
			set { this.masked = value; }
		}
		
		/// <summary>
		/// Draws the EditBox.
		/// </summary>
		public override void DrawGUI()
		{
			base.DrawGUI();
			
			GUI.enabled = this.IsEnabled;
			GUI.SetNextControlName(this.GUID);
			
			// draw EditBox
			if (this.masked)
			{
				this.tempText = GUI.PasswordField(this.Rect, this.Text, '*', this.CurrentStyle);
			}
			else
			{ 
				this.tempText = GUI.TextField(this.Rect, this.Text, this.CurrentStyle);
			}
			
			// compare actual text with last text only if EditBox is set to changeable
			if (this.changeable)
			{
				// take new value and call OnChange if text has changed
				if (!this.tempText.Equals(this.Text))
				{
					this.Text = this.tempText;
					OnChange();
				}
			}			
		}
		
		/// <summary>
		/// Gets or sets the text, displayed in the EditBox.
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
		
		private bool changeable;
		private OnChangeDelegate changeStorage;
		
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
			if (changeStorage != null) 
			{
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
			get { return changeable; }
			set { changeable = value; }
		}
        
        public bool IsChangeable
        {
            get { return this.Changeable && this.IsEnabled; }
        }
		#endregion

		#region ICloneable Member

		/// <summary>
		/// Creates a duplicate of an existing EditBox.
		/// </summary>
		/// <returns>Returns the cloned EditBox.</returns>
		public object Clone()
		{
			return new EditBox(this);
		}

		#endregion
	}
}