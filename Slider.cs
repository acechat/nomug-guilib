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
	/// <version>$Rev: 257 $, $Date: 2011-06-23 18:46:25 +0200 (Do, 23 Jun 2011) $</version>
	public class Slider : AClickableElement, IChangeEvents, ICloneable
	{
		private float minimum;
		private float maximum;
		private float sliderValue;
		private float tempValue;
		private float defaultValue;
		private float step;
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
			this.defaultValue = 0f;
			this.step = 0f;
			this.resetOnRelease = false;
			this.changeable = true;
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
			this.defaultValue = source.DefaultValue;
			this.resetOnRelease = source.ResetOnRelease;
			this.Alignment = source.Alignment;
			this.changeable = source.Changeable;
			this.Step = source.Step;
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
			this.defaultValue = 0f;
			this.step = 0f;
			this.resetOnRelease = false;
			this.changeable = true;
			this.Alignment = align;
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
			this.defaultValue = val;
			this.Alignment = align;
			this.step = 0f;
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
				this.DefaultStyle = OverlayManager.Instance.GetDefaultStyle(this);
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
			
			// Save current event type because we need it later and the following lines may use it.
			currentEventType = Event.current.type;

			// Draw horizontal or vertical slider.
			if (this.alignment == Types.Alignment.HORIZONTAL)
			{
				this.tempValue = GUI.HorizontalSlider(this.Rect, sliderValue, minimum, maximum, this.CurrentStyle, this.thumbStyle);
			}
			else
			{
				this.tempValue = GUI.VerticalSlider(this.Rect, sliderValue, minimum, maximum, this.CurrentStyle, this.thumbStyle);
			}

			// Assign the new value if slider is changable.
			if (this.changeable)
			{
				if (this.IsFocused && currentEventType == EventType.MouseUp)
				{
					this.OnRelease();
					// Reset after MouseUp if resetOnRelease is set.
					if(this.resetOnRelease)
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
	}
}
