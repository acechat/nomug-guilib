using UnityEngine;
using System;
using System.Collections.Generic;

namespace GUILib
{
	/// <summary>
	/// A simple box which can be checked via click.
	/// </summary>
	/// <author>Manuel Fasse (ven)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 260 $, $Date: 2011-06-25 17:13:20 +0200 (Sa, 25 Jun 2011) $</version>
	public class CheckBox : AClickableElement, ISelectEvents, ICloneable
	{
		private bool selected;
		private Dictionary<Types.StyleStateType, Texture2D> checkImage = new Dictionary<Types.StyleStateType, Texture2D>(4);
		private Dictionary<Types.StyleStateType, Texture2D> uncheckImage = new Dictionary<Types.StyleStateType, Texture2D>(4);
		
		/// <summary>
		/// Default constructor.
		/// </summary>
		public CheckBox() : base()
		{
			this.selected = false;
		}

		/// <summary>
		/// Creates a CheckBox with the properties of a given CheckBox.
		/// </summary>
		/// <param name="source">The source from where the properties should be copied.</param>
		public CheckBox(CheckBox source) : base(source)
		{
			this.selected = source.selected;
			this.checkImage = source.checkImage;
			this.uncheckImage = source.uncheckImage;
		}

		/// <summary>
		/// Draws the CheckBox.
		/// </summary>
		public override void DrawGUI()
		{
			base.DrawGUI();
			GUI.Button(this.Rect, this.content, this.CurrentStyle);
		}
		
		private GUIContent content
		{
			get
			{
				Texture2D texture = null;
				if (this.selected)
				{
					if (this.checkImage.ContainsKey(this.CurrentState))
					{
						texture = this.checkImage[this.CurrentState];
					}
				}
				else
				{
					if (this.uncheckImage.ContainsKey(this.CurrentState))
					{
						texture = this.uncheckImage[this.CurrentState];
					}
				}
				if (texture != null)
				{
					return new GUIContent(this.Text, texture);
				}
				return new GUIContent(this.Text);
			}
		}
		
		
		/// <summary>
		/// Whether the checkbox is checked.
		/// </summary>
		public bool Selected
		{
			get { return this.selected; }
			set { this.selected = value; }
		}
		
		#region IClickEvents Member
		
		public override bool OnClick()
		{
			this.Selected = !Selected;
			base.OnClick();
            this.OnSelect();
			return true;
		}
		
		#endregion
		
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

		#region ICloneable Member

		/// <summary>
		/// Creates a duplicate of an existing CheckBox.
		/// </summary>
		/// <returns>Returns the cloned CheckBox.</returns>
		public object Clone()
		{
			return new CheckBox(this);
		}

		#endregion
		
		protected override void setStyleProperty(Types.StylePropertyType stylePropertyType)
		{
			object[] value = new object[4];
			value[0] = getStyleProperty(stylePropertyType);
			value[1] = getStyleProperty(stylePropertyType, Types.StyleStateType.Focused);
			value[2] = getStyleProperty(stylePropertyType, Types.StyleStateType.Hover);
			value[3] = getStyleProperty(stylePropertyType, Types.StyleStateType.Active);
			
			if(value[0] == null) return;
			
			switch(stylePropertyType)
			{
				case Types.StylePropertyType.checkimageposition:
					this.Normal.imagePosition = (ImagePosition)value[0];
					this.Focused.imagePosition = (ImagePosition)value[0];
					this.Hover.imagePosition = (ImagePosition)value[0];
					this.Active.imagePosition = (ImagePosition)value[0];
					break;
				case Types.StylePropertyType.checkimage:
					this.checkImage[Types.StyleStateType.Normal] = (Texture2D)value[0];
					this.checkImage[Types.StyleStateType.Focused] = (Texture2D)value[1];
					this.checkImage[Types.StyleStateType.Hover] = (Texture2D)value[2];
					this.checkImage[Types.StyleStateType.Active] = (Texture2D)value[3];
					break;
				case Types.StylePropertyType.uncheckimage:
					this.uncheckImage[Types.StyleStateType.Normal] = (Texture2D)value[0];
					this.uncheckImage[Types.StyleStateType.Focused] = (Texture2D)value[1];
					this.uncheckImage[Types.StyleStateType.Hover] = (Texture2D)value[2];
					this.uncheckImage[Types.StyleStateType.Active] = (Texture2D)value[3];
					break;
				default:
					base.setStyleProperty(stylePropertyType);
					break;
			}
		}
	}
}