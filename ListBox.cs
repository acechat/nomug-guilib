using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace GUILib
{
	/// <summary>
	/// A box that shows a list of items which can be selected via clicking.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 291 $, $Date: 2011-07-30 15:00:20 +0200 (Sa, 30 Jul 2011) $</version>
	public class ListBox : AListElement, ICloneable, IElementCollection
	{
		private GUIStyle itemStyle;
		private GUIStyle selectedStyle;
		private RectOffset padding = new RectOffset();
		private Slider slider;
		private float scrollPosition;
		
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ListBox() : base()
		{
			this.slider = new Slider(Types.Alignment.VERTICAL);
			this.slider.Parent = this;
			this.slider.Minimum = 0;
		}

		/// <summary>
		/// Creates a ListBox with the properties of a given ListBox.
		/// </summary>
		/// <param name="source">The source from where the properties should be copied.</param>
		public ListBox(ListBox source) : base(source)
		{
			this.itemStyle = new GUIStyle(source.itemStyle);
			this.selectedStyle = new GUIStyle(source.selectedStyle);
			this.slider = new Slider(Types.Alignment.VERTICAL);
			this.slider.Parent = this;
			this.slider.Minimum = 0;
			this.padding = source.padding;
		}
		
		/// <summary>
		/// Height of the individual items in the list box. 
		/// </summary>
		public float ItemHeight
		{
			get { return this.itemHeight; }
			set { this.ElementStyle.ItemHeight = value; }
		}
		
		/// <summary>
		/// ClassStyle of the embedded scrollbar.
		/// </summary>
		public Style ScrollBarStyle
		{
			get { return this.slider.ClassStyle; }
			set { this.slider.ClassStyle = value; }
		}
		
		/// <summary>
		/// Current positioning offset. 
		/// </summary>
		public float ScrollPosition
		{
			get
			{
				if (this.slider.Visible)
				{
					return this.slider.Value;
				}
				return 0f;
			}
			set
			{
				if (value >= 0f && value <= this.ScrollMaximum)
				{
					this.slider.Value = value;
				}
			}
		}
		
		/// <summary>
		/// Maximum the scroll position can reach. 
		/// </summary>
		public float ScrollMaximum
		{
			get
			{
				if (this.slider.Visible)
				{
					return this.slider.Maximum;
				}
				return 0f;
			}
		}

		/// <summary>
		/// Draws the ListBox.
		/// </summary>
		public override void DrawGUI()
		{
			base.DrawGUI();
			GUI.enabled = this.IsEnabled;
			GUI.BeginGroup(this.Rect, this.CurrentStyle);

			Rect innerRect = new Rect(0 + this.padding.left, 0 + this.padding.top, this.Width - this.padding.horizontal, this.Height - this.padding.vertical);
			GUI.BeginGroup(innerRect);
			
			float drawWidth;
			if(slider.Visible)
			{
				drawWidth = innerRect.width - slider.Width;
			}
			else
			{
				drawWidth = innerRect.width;
			}
			
			float drawTop;
			GUIStyle drawStyle;
			for (int i = 0; i < this.items.Count; i++)
			{
				drawTop = i * this.itemHeight - this.slider.Value;
				
				if(this.selected == i)
				{
					drawStyle = this.selectedStyle;
				}
				else
				{
					drawStyle = this.itemStyle;
				}
				
				GUI.Button(new Rect(0, drawTop, drawWidth, this.itemHeight), this.items[i], drawStyle);
			}		

			GUI.EndGroup();

			this.slider.Visible = (this.items.Count * this.itemHeight > innerRect.height);
			if (this.slider.Visible)
			{
				this.slider.Left = innerRect.x + innerRect.width - this.slider.Width;
				this.slider.Top = innerRect.y;
				this.slider.Height = innerRect.height;
				this.slider.Maximum = this.items.Count * this.itemHeight - innerRect.height;
				this.slider.ElementStyle.ThumbHeight = innerRect.height / (this.items.Count * this.itemHeight) * innerRect.height;
				this.slider.DrawGUI();
			}	

			GUI.EndGroup();
			
		}
		
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
				case Types.StylePropertyType.alignment:
					this.itemStyle.alignment = (TextAnchor)value[0];
					this.selectedStyle.alignment = (TextAnchor)value[0];
					break;
				case Types.StylePropertyType.fontasset:
					this.itemStyle.font = value[0] as Font;
					this.selectedStyle.font = value[0] as Font;
					break;
# if (!UNITY_2_6)
# if (!UNITY_2_6_1)
				case Types.StylePropertyType.fontsize:
					this.itemStyle.fontSize = (int)value[0];
					this.selectedStyle.fontSize = (int)value[0];
					break;
				case Types.StylePropertyType.fontstyling:
					this.itemStyle.fontStyle = (FontStyle)value[0];
					this.selectedStyle.fontStyle = (FontStyle)value[1];
					break;
# endif
# endif
				case Types.StylePropertyType.paddingleft:
					this.padding.left = (int)value[0];
					break;
				case Types.StylePropertyType.paddingright:
					this.padding.right = (int)value[0];
					break;
				case Types.StylePropertyType.paddingtop:
					this.padding.top = (int)value[0];
					break;
				case Types.StylePropertyType.paddingbottom:
					this.padding.bottom = (int)value[0];
					break;
				case Types.StylePropertyType.itempaddingleft:
					this.itemStyle.padding.left = (int)value[0];
					this.selectedStyle.padding.left = (int)value[0];
					break;
				case Types.StylePropertyType.itempaddingright:
					this.itemStyle.padding.right = (int)value[0];
					this.selectedStyle.padding.right = (int)value[0];
					break;
				case Types.StylePropertyType.itempaddingtop:
					this.itemStyle.padding.top = (int)value[0];
					this.selectedStyle.padding.top = (int)value[0];
					break;
				case Types.StylePropertyType.itempaddingbottom:
					this.itemStyle.padding.bottom = (int)value[0];
					this.selectedStyle.padding.bottom = (int)value[0];
					break;
				case Types.StylePropertyType.clipping:
					this.itemStyle.clipping = (TextClipping)value[0];
					this.selectedStyle.clipping = (TextClipping)value[0];
					break;
				case Types.StylePropertyType.wordwrap:
					this.itemStyle.wordWrap = (bool)value[0];
					this.selectedStyle.wordWrap = (bool)value[0];
					break;
				case Types.StylePropertyType.textcolor:
					this.itemStyle.normal.textColor = (Color)value[0];
					this.itemStyle.focused.textColor = (Color)value[1];
					this.itemStyle.hover.textColor = (Color)value[2];
					this.itemStyle.active.textColor = (Color)value[3];
					break;
				case Types.StylePropertyType.itembackground:
					this.itemStyle.normal.background = value[0] as Texture2D;
					this.itemStyle.focused.background = value[1] as Texture2D;
					this.itemStyle.hover.background = value[2] as Texture2D;
					this.itemStyle.active.background = value[3] as Texture2D;
					break;
				case Types.StylePropertyType.selectedtextcolor:
					this.selectedStyle.normal.textColor = (Color)value[0];
					this.selectedStyle.focused.textColor = (Color)value[1];
					this.selectedStyle.hover.textColor = (Color)value[2];
					this.selectedStyle.active.textColor = (Color)value[3];
					break;
				case Types.StylePropertyType.selectedbackground:
					this.selectedStyle.normal.background = value[0] as Texture2D;
					this.selectedStyle.focused.background = value[1] as Texture2D;
					this.selectedStyle.hover.background = value[2] as Texture2D;
					this.selectedStyle.active.background = value[3] as Texture2D;
					break;
				case Types.StylePropertyType.itemheight:
					this.itemHeight = (float)value[0];
					break;
				default:
					base.setStyleProperty(stylePropertyType);
					break;
			}
		}
		
		protected override void updateGuiStyles()
		{
			this.itemStyle = new GUIStyle();
			this.selectedStyle = new GUIStyle();
			base.updateGuiStyles();
		}
		
		#region IClickEvents Member

		/// <summary>
		/// Is called when a MouseDown event has to be raised.
		/// </summary>
		/// <returns>Returns true because ListBox selection is a mouse down event.</returns>
		public override bool OnMouseDown()
		{
			// Calculate the index beneath the mouse.
			int selected = (int)((Event.current.mousePosition.y - this.AbsoluteTop + this.slider.Value - this.padding.top) / this.itemHeight);
			// Only change the selected index if an item was clicked.
			if (selected < this.items.Count &&
			    (!this.slider.Visible ||
			     (Event.current.mousePosition.x < (this.AbsoluteLeft + this.Width - this.slider.Width - this.padding.right)) &&
			     (Event.current.mousePosition.x > (this.AbsoluteLeft + this.padding.left))
			    ) &&
			    (Event.current.mousePosition.y < (this.AbsoluteTop + this.Height - this.padding.bottom))
			   )
			{
				this.selected = selected;
				// Fire events.
				this.OnSelect();
			}

			//if(new Rect(this.slider.AbsoluteLeft, this.slider.AbsoluteTop, this.slider.Width, this.slider.Height).Contains(Event.current.mousePosition))
			//{
			//    this.slider.OnMouseDown();
			//}

			// Call a mouseDown event if set.
			base.OnMouseDown();
			// Return true because this event was used.
			return true;
		}

		/// <summary>
		/// <para>Is called when a Click event has to be raised.</para>
		/// <para>(Clicks happen if the mouse was pressed and released on the same element.)</para>
		/// </summary>
		/// <returns>Returns true because ListBox selection is a click even if no click handler has been attached.</returns>
		public override bool OnClick()
		{
			base.OnClick();
			return true;
		}

		#endregion

		#region ICloneable Member

		/// <summary>
		/// Creates a duplicate of an existing ListBox.
		/// </summary>
		/// <returns>Returns the cloned ListBox.</returns>
		public object Clone()
		{
			return new ListBox(this);
		}

		#endregion

		#region IElementCollection Member

		/// <summary>
		/// Does nothing.
		/// </summary>
		public bool AddElement(IElement element)
		{
			//do nothing
			return false;
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		public bool InsertElement(int index, IElement element)
		{
			//do nothing
			return false;
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		public bool RemoveElement(IElement element)
		{
			//do nothing
			return false;
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		public void Clear()
		{
			//do nothing
		}

		/// <summary>
		/// Returns the the scrollbar as child element.
		/// </summary>
		public System.Collections.ObjectModel.ReadOnlyCollection<IElement> Children
		{
			get
			{
				List<IElement> tempList = new List<IElement>();
				tempList.Add(this.slider);
				return tempList.AsReadOnly();
			}
		}

		/// <summary>
		/// Returns the amount of children.
		/// </summary>
		public int Count
		{
			get { return 1; }
		}

		#endregion
	}
}