using UnityEngine;
using System.Collections;
using System;

namespace GUILib
{
	/// <summary>
	/// This is a class for a horizontal or vertical slider allowing the user to change a numerical value.
	/// </summary>
	/// <author>Jannis Drewello (drewello)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 293 $, $Date: 2012-01-16 19:18:36 +0100 (Mon, 16 Jan 2012) $</version>
	public class Slider : AElement, IChangeEvents, IClickEvents, ICloneable
	{
		private float minimum;
		private float maximum;
		private float sliderValue;
		private float tempValue;
		private float defaultValue;
		private float step;
		private bool sliding;
		private float thumbLeft;
		private float thumbTop;
		private float thumbWidth;
		private float thumbHeight;
		private float leftOffset;
		private float topOffset;
		private GUIStyle thumbStyle;

		private Types.Alignment alignment;
		private bool resetOnRelease;
		// temporary variable to save the event for later usage.
		private EventType currentEventType;

		/// <summary>
		/// Initializes a new instance of the class Slider with default alignment horizontal.
		/// </summary>
		public Slider()
			: base()
		{
			this.minimum = 0f;
			this.maximum = 0f;
			this.sliderValue = 0f;
			this.tempValue = 0f;
			this.defaultValue = 0f;
			this.step = 0f;
			this.resetOnRelease = false;
			this.changeable = true;
			this.sliding = false;
			this.leftOffset = 0.0f;
			this.topOffset = 0.0f;
			this.Alignment = Types.Alignment.HORIZONTAL;
		}

		/// <summary>
		/// Initializes a new instance of the class Slider and copies its values from an existing object.
		/// </summary>
		public Slider(Slider source)
			: base(source)
		{
			this.minimum = source.Minimum;
			this.maximum = source.Maximum;
			this.sliderValue = source.Value;
			this.tempValue = 0f;
			this.defaultValue = source.DefaultValue;
			this.resetOnRelease = source.ResetOnRelease;
			this.Alignment = source.Alignment;
			this.changeable = source.Changeable;
			this.Step = source.Step;
			this.sliding = false;
			this.leftOffset = 0.0f;
			this.topOffset = 0.0f;
			this.thumbStyle = new GUIStyle(source.thumbStyle);
		}

		/// <summary>
		/// Initializes a new instance of the Slider class with given alignment.
		/// </summary>
		/// <param name='align'>
		/// Defines the alignment of the Slider. 
		/// </param>
		public Slider(Types.Alignment align)
			: base()
		{
			this.minimum = 0f;
			this.maximum = 0f;
			this.sliderValue = 0f;
			this.tempValue = 0f;
			this.defaultValue = 0f;
			this.step = 0f;
			this.resetOnRelease = false;
			this.changeable = true;
			this.Alignment = align;
			this.sliding = false;
			this.leftOffset = 0.0f;
			this.topOffset = 0.0f;
		}

		/// <summary>
		/// Initializes a new instance of the class Slider with a minimum, a maximum and an initial float value.
		/// </summary>
		/// <param name='min'>
		/// Minimum value of the Slider.
		/// </param>
		/// <param name='max'>
		/// Maximum value of the Slider.
		/// </param>
		/// <param name='val'>
		/// Value of the Slider.
		/// </param>
		/// <param name='align'>
		/// Draw alignment of the Slider.
		/// </param>
		public Slider(float min, float max, float val, Types.Alignment align)
			: base()
		{
			this.resetOnRelease = false;
			this.changeable = true;
			this.minimum = min;
			this.maximum = max;
			this.sliderValue = val;
			this.tempValue = val;
			this.defaultValue = val;
			this.Alignment = align;
			this.step = 0f;
			this.sliding = false;
			this.leftOffset = 0.0f;
			this.topOffset = 0.0f;
		}

		/// <summary>
		/// Gets or sets the minimum value of the Slider.
		/// </summary>
		public float Minimum
		{
			get { return minimum; }
			set {
				minimum = value;
				this.sliderValue = Mathf.Max(value, this.sliderValue);
			}
		}

		/// <summary>
		/// Gets or sets the maximum value of the Slider.
		/// </summary>
		public float Maximum
		{
			get { return maximum; }
			set {
				this.maximum = value;
				this.sliderValue = Mathf.Min(value, this.sliderValue);
			}
		}

		/// <summary>
		/// Gets or sets the value determined by the actual slider position.
		/// </summary>
		public float Value
		{
			get { return this.sliderValue; }
			set { this.sliderValue = this.calculateValue(value); }
		}

		/// <summary>
		/// Gets or sets the alignment for drawing the Slider.
		/// </summary>
		public Types.Alignment Alignment
		{
			get { return this.alignment; }
			set
			{
				this.alignment = value;
				this.DefaultStyle = LayerManager.Instance.GetDefaultStyle(this);
			}
		}

		/// <summary>
		/// Gets or sets the initial value of the Slider.
		/// </summary>
		public float DefaultValue
		{
			get { return this.defaultValue; }
			set { this.defaultValue = value; }
		}

		/// <summary>
		/// The step between two values.
		/// </summary>
		/// <value>Set to zero or less to disable steps.</value>
		public float Step
		{
			get { return this.step; }
			set
			{
				if (value > 0f)
				{
					this.step = value;
				}
				else
				{
					this.step = 0f;
				}
			}
		}

		/// <summary>
		/// Gets or sets whether the Slider flips back to its initial position on mouse release.
		/// </summary>
		public bool ResetOnRelease
		{
			get { return this.resetOnRelease; }
			set { this.resetOnRelease = value; }
		}

		/// <summary>
		/// Draws the Slider.
		/// </summary>
		public override void DrawGUI()
		{
			base.DrawGUI();
			GUI.enabled = this.IsEnabled;

			GUI.BeginGroup(this.Rect, this.CurrentStyle);
			this.tempValue = this.sliderValue;

			// Draw horizontal or vertical slider.
			if (this.alignment == Types.Alignment.HORIZONTAL)
			{
				if (sliding)
				{
					// prevent divide by zero
					if (this.Width - this.thumbStyle.fixedWidth != 0.0f)
					{
						// get current mouse position on the slider area adjusted to thumb size
						float sliderPos = Mathf.Clamp(Event.current.mousePosition.x - this.leftOffset, 0.0f, this.Width - this.thumbStyle.fixedWidth) / (this.Width - this.thumbStyle.fixedWidth);
						this.tempValue = (this.maximum - this.minimum) * sliderPos;
					}
					else
					{
						this.tempValue = this.minimum;
					}
				}
				// draw thumb according to its size
				this.thumbLeft = (this.tempValue / (this.maximum - this.minimum != 0 ? this.maximum - this.minimum : 0) * (this.Width - this.thumbStyle.fixedWidth));
				this.thumbTop = 0.0f;
				this.thumbWidth = this.thumbStyle.fixedWidth;
				this.thumbHeight = this.Height;
			}
			else
			{
				if (sliding)
				{
					//prevent divide by zero
					if (this.Height - this.thumbStyle.fixedHeight != 0.0f)
					{
						// get current mouse position on the slider area adjusted to thumb size
						float sliderPos = Mathf.Clamp(Event.current.mousePosition.y - this.topOffset, 0.0f, this.Height - this.thumbStyle.fixedHeight) / (this.Height - this.thumbStyle.fixedHeight);
						this.tempValue = (this.maximum - this.minimum) * sliderPos;
					}
					else
					{
						this.tempValue = this.minimum;
					}
				}
				// draw thumb according to its size
				this.thumbLeft = 0.0f;
				this.thumbTop = (this.tempValue / (this.maximum - this.minimum != 0 ? this.maximum - this.minimum : 0) * (this.Height - this.thumbStyle.fixedHeight));
				this.thumbWidth = this.Width;
				this.thumbHeight = this.thumbStyle.fixedHeight;
			}
			GUI.Button(new Rect(this.thumbLeft, this.thumbTop, this.thumbWidth, this.thumbHeight), "", this.thumbStyle);

			GUI.EndGroup();

			// Assign the new value if slider is changable.
			if (this.changeable)
			{
				if (this.sliding && LayerManager.Instance.CurrentEventType == EventType.MouseUp)
				{
					this.OnRelease();
					// Reset after MouseUp if resetOnRelease is set.
					if (this.resetOnRelease)
					{
						this.tempValue = this.defaultValue;
					}
				}
				// Handle value change if Slider is set to changeable.
				if (this.sliderValue != this.calculateValue(this.tempValue))
				{
					this.sliderValue = this.calculateValue(this.tempValue);
					this.OnChange();
				}
			}
		}

		/// <summary>
		/// Calculate the value of a slider based on its settings e.g. Steps.
		/// </summary>
		/// <param name="value">The base value.</param>
		/// <returns>The value which the slider accepts based on its settings.</returns>
		private float calculateValue(float value)
		{
			float result = value;
			if (this.step > 0f)
			{
				result = Mathf.Round(value / this.step) * this.step;
				result = Mathf.Min(this.maximum, Mathf.Max(this.minimum, result));
			}
			return result;
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
				case Types.StylePropertyType.thumbheight:
					this.thumbStyle.fixedHeight = (float)value[0];
					if(this.thumbStyle.fixedHeight == 0)
					{
						this.thumbStyle.stretchHeight = true;
					}
					else
					{
						this.thumbStyle.stretchHeight = false;
					}
					break;
				case Types.StylePropertyType.thumbwidth:
					this.thumbStyle.fixedWidth = (float)value[0];
					if(this.thumbStyle.fixedWidth == 0)
					{
						this.thumbStyle.stretchWidth = true;
					}
					else
					{
						this.thumbStyle.stretchWidth = false;
					}
					break;
				case Types.StylePropertyType.thumbbackground:
					this.thumbStyle.normal.background = value[0] as Texture2D;
					this.thumbStyle.focused.background = value[1] as Texture2D;
					this.thumbStyle.hover.background = value[2] as Texture2D;
					this.thumbStyle.active.background = value[3] as Texture2D;
					break;
				case Types.StylePropertyType.thumbborderleft:
					this.thumbStyle.border.left = (int)value[0];
					break;
				case Types.StylePropertyType.thumbborderright:
					this.thumbStyle.border.right = (int)value[0];
					break;
				case Types.StylePropertyType.thumbbordertop:
					this.thumbStyle.border.top = (int)value[0];
					break;
				case Types.StylePropertyType.thumbborderbottom:
					this.thumbStyle.border.bottom = (int)value[0];
					break;
				default:
					base.setStyleProperty(stylePropertyType);
					break;
			}
		}
		
		protected override void updateGuiStyles()
		{
			thumbStyle = new GUIStyle();
			base.updateGuiStyles();
		}
		
		private OnReleaseDelegate releaseStorage;
		public void OnRelease()
		{
			this.sliding = false;
			if(this.releaseStorage != null)
			{
				this.releaseStorage(this);
			}
		}

		public event OnReleaseDelegate Released
		{
			add { releaseStorage = value; }
			remove { releaseStorage = null; }
		}

		#region IChangeEvents Member

		private bool changeable;
		private OnChangeDelegate changeStorage;

		/// <summary>
		/// Gets or sets whether the Slider can be manipulated by the user.
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
		
		/// <summary>
		/// Will be raised when slider is manipulated by user. Attach your method here.
		/// </summary>
		public event OnChangeDelegate Changed
		{
			add { changeStorage = value; }
			remove { changeStorage = null; }
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

		#endregion

		#region ICloneable Member

		/// <summary>
		/// Creates a duplicate of an existing Slider.
		/// </summary>
		/// <returns>Returns the cloned Slider.</returns>
		public object Clone()
		{
			return new Slider(this);
		}

		#endregion

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
		public virtual bool OnClick()
		{
			if (clickStorage != null)
			{
				clickStorage(this);
			}
			// always grab event
			return true;
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
			return true;
		}

		/// <summary>
		/// Is called when a MouseDown event has to be raised.
		/// </summary>
		/// <returns>True if an event has been raised.</returns>
		public virtual bool OnMouseDown()
		{
			if (new Rect(this.AbsoluteLeft + this.thumbLeft, this.AbsoluteTop + this.thumbTop, this.thumbWidth, this.thumbHeight).Contains(Event.current.mousePosition))
			{
				this.leftOffset = Event.current.mousePosition.x - (this.AbsoluteLeft + this.thumbLeft);
				this.topOffset = Event.current.mousePosition.y - (this.AbsoluteTop + this.thumbTop);
			}
			else
			{
				this.leftOffset = this.thumbStyle.fixedWidth / 2;
				this.topOffset = this.thumbStyle.fixedHeight / 2;
			}
			this.sliding = true;
			if (this.mouseDownStorage != null)
			{
				this.mouseDownStorage(this);
			}
			// always grab event
			return true;
		}

		#endregion
	}
}
