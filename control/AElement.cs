using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GUILib
{
	/// <summary>
	/// All GUI elements inherit from this class.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 295 $, $Date: 2012-01-16 21:10:18 +0100 (Mon, 16 Jan 2012) $</version>
	public abstract class AElement : IElement, IMouseEvents
	{
		// We need to save the reference to our update delegate so that it can not be collected by the GC. This seems to be the easier way to avoid gc on the delegate without loosing the weak reference in style which was needed to avoid gc on unneeded elements.
		private UpdateHandler updateHandler;
		
		/// <summary>
		/// Default constructor.
		/// </summary>
		public AElement()
		{
			this.isHovered = false;

			this.enabled = true;
			this.visible = true;
			this.left = this.top = this.width = this.height = 0f;

			this.guid = Guid.NewGuid().ToString();
			this.name = "";
			this.text = "";
			
			// initial values for style objects
			this.updateHandler = this.updateGuiProperty;
			this.defaultStyle = LayerManager.Instance.GetDefaultStyle(this);
			this.defaultStyle.CallUpdate += this.updateHandler;
			this.elementStyle = new Style();
			this.elementStyle.CallUpdate += this.updateHandler;
			
			this.parent = new WeakReference(null);
			this.toolTip = null;
			
			this.updateGuiStyles();
		}

		/// <summary>
		/// Creates a new AElement with the properties of a given source AElement object.
		/// </summary>
		/// <param name="source">The source from where the properties should be copied.</param>
		public AElement(AElement source)
		{
			this.isHovered = source.isHovered;

			this.enabled = source.enabled;
			this.visible = source.visible;
			this.left = source.left;
			this.top = source.top;
			this.height = source.height;
			this.width = source.width;

			this.guid = Guid.NewGuid().ToString();
			this.name = source.name;
			this.text = source.text;

			this.normal = new GUIStyle(source.normal);
			this.focused = new GUIStyle(source.focused);
			this.hover = new GUIStyle(source.hover);
			this.active = new GUIStyle(source.active);

			// initial values for style objects
			this.updateHandler = this.updateGuiProperty;
			this.defaultStyle = source.defaultStyle;
			this.defaultStyle.CallUpdate += this.updateHandler;
			if (source.classStyle != null)
			{
				this.classStyle = source.classStyle;
				this.classStyle.CallUpdate += this.updateHandler;
			}
			this.elementStyle = source.elementStyle.Copy();
			this.elementStyle.CallUpdate += this.updateHandler;
			
			this.parent = new WeakReference(null);
			this.toolTip = null;
		}

		/// <summary>
		/// Returns whether the object is the focus control or not.
		/// </summary>
		public bool IsFocused
		{
			//TODO discuss if this should be moved to IClickEvents and/or IChangeEvents
			get { return (this == LayerManager.Instance.FocusControl); }
		}

		/// <summary>
		/// Returns if the mouse is over the object.
		/// </summary>
		public bool IsHovered
		{
			get { return this.isHovered && (!LayerManager.Instance.Dragging || LayerManager.Instance.AcceptingElement == this); }
		}

		/// <summary>
		/// Returns whether the object is the active control or not.
		/// </summary>
		public bool IsActive
		{
			get { return (this == LayerManager.Instance.ActiveControl); }
		}

		#region IElement Member

		private bool enabled;
		private bool visible;
		
		// Reference to the parent (IElementCollection).
		private WeakReference parent;

		private float left;
		private float top;
		private float width;
		private float height;

		private string guid;
		private string name;
		private string text;

		private GUIStyle normal;
		private GUIStyle focused;
		private GUIStyle hover;
		private GUIStyle active;

		private Style defaultStyle;
		private Style classStyle;
		private Style elementStyle;

		private ToolTip toolTip;

		public virtual void Start()
		{

		}

		public virtual void Update()
		{

		}

		public virtual void DrawGUI()
		{

		}

		/// <summary>
		/// Returns the parent element of this object.
		/// </summary>
		public IElementCollection Parent
		{
			get {
				IElementCollection parent = this.parent.Target as IElementCollection;
				
				return parent;
			}
			set
			{
				if (value == null)
				{
					IElementCollection parent = this.parent.Target as IElementCollection;
					if (parent != null)
					{
						foreach (IElement elem in parent.Children)
						{
							if (elem == this)
							{
								// TODO: warning "Can not set parent to null of an element that has still a parent."
								return;
							}
						}
						this.parent.Target = null;
					}
				}
				else
				{
					// check if the given parent is the parent of this element (this will prevent corrupted trees)
					foreach (IElement elem in value.Children)
					{
						if (elem == this)
						{
							this.parent.Target = value;
							return;
						}
					}
					// TODO: warn "The parent isn't the parent of this element. Do you missed adding this element to a collection?"
				}
			}
		}

		/// <summary>
		/// Returns if the element and its parents are enabled.
		/// </summary>
		public bool IsEnabled
		{
			get { return (this.Parent == null || this.Parent.IsEnabled) && this.enabled; }
		}

		/// <summary>
		/// Returns whether the element is enabled.
		/// </summary>
		public bool Enabled
		{
			get { return this.enabled; }
			set { this.ElementStyle.Enabled = value; }
		}

		/// <summary>
		/// Returns whether the element is actually drawn.
		/// </summary>
		public bool IsVisible
		{
			get { return (this.Parent != null && this.Parent.IsVisible) && this.visible; }
		}

		/// <summary>
		/// Set or get the visibility value for this element.
		/// </summary>
		public bool Visible
		{
			get { return this.visible; }
			set { this.ElementStyle.Visible = value; }
		}

		/// <summary>
		/// The offset between the screen border and the left border of the element. Read-only.
		/// </summary>
		public float AbsoluteLeft
		{
			get
			{
				if (this.Parent != null && this.Parent is IElement)
				{
					// TODO: bugfix for scrollbars. this should be replaced!
					if (this.Parent is ScrollBox && (this.Parent as ScrollBox).ScrollChildren.Contains(this))
					{
						return (this.Parent as IElement).AbsoluteLeft + this.Left - (this.Parent as ScrollBox).HorizontalScrollPosition;
					}

					return (this.Parent as IElement).AbsoluteLeft + this.Left;
				}
				return this.Left;
			}
		}

		/// <summary>
		/// The offset between the screen border and the top border of the element. Read-only.
		/// </summary>
		public float AbsoluteTop
		{
			get
			{
				if (this.Parent != null && this.Parent is IElement)
				{
					// TODO: bugfix for scrollbars. this should be replaced!
					if (this.Parent is ScrollBox && (this.Parent as ScrollBox).ScrollChildren.Contains(this))
					{
						return (this.Parent as IElement).AbsoluteTop + this.Top - (this.Parent as ScrollBox).VerticalScrollPosition;
					}

					return (this.Parent as IElement).AbsoluteTop + this.Top;
				}
				return this.Top;
			}
		}

		/// <summary>
		/// The left offset in relation to the objects parent.
		/// </summary>
		public float Left
		{
			get { return this.left; }
			set { this.elementStyle.Left = value; }
		}

		/// <summary>
		/// The top offset in relation to the objects parent.
		/// </summary>
		public float Top
		{
			get { return this.top; }
			set { this.elementStyle.Top = value; }
		}

		/// <summary>
		/// The width of this element.
		/// </summary>
		public float Width
		{
			get { return this.width; }
			set { this.elementStyle.Width = value; }
		}

		/// <summary>
		/// The height of this element.
		/// </summary>
		public float Height
		{
			get { return this.height; }
			set { this.elementStyle.Height = value; }
		}

		/// <summary>
		/// Returns a rectangle where this element is shown.
		/// </summary>
		public UnityEngine.Rect Rect
		{
			get { return new Rect(this.Left, this.Top, this.Width, this.Height); }
		}

		/// <summary>
		/// Unique identifier. This is used for focusing elements.
		/// </summary>
		public string GUID
		{
			get { return this.guid; }
		}

		/// <summary>
		/// <para>Returns the name of an element (if not specified it returns the content).</para>
		/// <para>DO NOT USE THIS TO GET THE CONTENT OF AN ELEMENT.</para>
		/// </summary>
		public string Name
		{
			get
			{
				if ("".Equals(this.name))
				{
					return "[content: " + this.Text + "]";
				}
				return this.name;
			}
			set { this.elementStyle.Name = value; }
		}

		/// <summary>
		/// The text of an element.
		/// </summary>
		public virtual string Text
		{
			get { return this.text; }
			set { this.elementStyle.Text = value; }
		}

		/// <summary>
		/// GUIStyle which is used in the normal state.
		/// </summary>
		protected GUIStyle Normal
		{
			get { return this.normal; }
			set { this.normal = value; }
		}

		/// <summary>
		/// GUIStyle which is used while the element is focus.
		/// </summary>
		protected virtual GUIStyle Focused
		{
			get { return this.focused; }
			set { this.focused = value; }
		}

		/// <summary>
		/// GUIStyle which is used while the element is hovered.
		/// </summary>
		protected virtual GUIStyle Hover
		{
			get { return this.hover; }
			set { this.hover = value; }
		}

		/// <summary>
		/// GUIStyle which is used while the element is active.
		/// </summary>
		protected virtual GUIStyle Active
		{
			get { return this.active; }
			set { this.active = value; }
		}

		/// <summary>
		/// Gets the current GUIStyle needed for drawing.
		/// </summary>
		protected GUIStyle CurrentStyle
		{
			get
			{
				switch (this.CurrentState)
				{
					case Types.StyleStateType.Active:
						return this.Active;
					case Types.StyleStateType.Hover:
						return this.Hover;
					case Types.StyleStateType.Focused:
						return this.Focused;
					default: return this.Normal;
				}
			}
		}

		protected virtual Types.StyleStateType CurrentState
		{
			get
			{
				if (this.IsActive)
					return Types.StyleStateType.Active;
				if (this.IsHovered)
					return Types.StyleStateType.Hover;
				if (this.IsFocused)
					return Types.StyleStateType.Focused;
				return Types.StyleStateType.Normal;
			}
		}

		public Style DefaultStyle
		{
			get { return defaultStyle; }
			set
			{
				if (value != null && value != defaultStyle)
				{
					if (defaultStyle != null)
						defaultStyle.CallUpdate -= this.updateHandler;
					defaultStyle = value;
					value.CallUpdate += this.updateHandler;
					updateGuiStyles();
				}
			}
		}

		public Style ClassStyle
		{
			get {
				if (this.classStyle == null)
				{
					this.ClassStyle = new Style();	
				}
				return this.classStyle;
			}
			set
			{
				if (value != null && value != classStyle)
				{
					if (classStyle != null)
						classStyle.CallUpdate -= this.updateHandler;
					classStyle = value;
					value.CallUpdate += this.updateHandler;
					updateGuiStyles();
				}
			}
		}

		public Style ElementStyle
		{
			get { return this.elementStyle; }
			set
			{
				if (value != null && value != elementStyle)
				{
					if (elementStyle != null)
						elementStyle.CallUpdate -= this.updateHandler;
					elementStyle = value;
					value.CallUpdate += this.updateHandler;
					updateGuiStyles();
				}
			}
		}

		/// <summary>
		/// The tooltip that this element has.
		/// </summary>
		public ToolTip ToolTip
		{
			get { return this.toolTip; }
			set { this.toolTip = value; }
		}

		#endregion

		#region IMouseEvents Member

		/// <summary>
		/// Whether the mouse is in the rectangle of this element or not. Needed to determine if there is an Enter- or Move event.
		/// </summary>
		private bool isHovered;

		private OnMouseDelegate mouseEnterStorage;
		private OnMouseDelegate mouseExitStorage;
		private OnMouseDelegate mouseMoveStorage;


		/// <summary>
		/// Will be raised when the mouse enters this control. Attach your method here.
		/// </summary>
		public event OnMouseDelegate MouseEnter
		{
			add { mouseEnterStorage = value; }
			remove { mouseEnterStorage = null; }
		}

		/// <summary>
		/// Will be raised when the mouse leaves this control. Attach your method here.
		/// </summary>
		public event OnMouseDelegate MouseExit
		{
			add { mouseExitStorage = value; }
			remove { mouseExitStorage = null; }
		}

		/// <summary>
		/// Will be raised when the mouse moves inside this control. Attach your method here.
		/// </summary>
		public event OnMouseDelegate MouseMove
		{
			add { mouseMoveStorage = value; }
			remove { mouseMoveStorage = null; }
		}


		/// <summary>
		/// This method is called when the LayerManager determines that the element
		/// is under the mouse cursor. This method checks if it is an enter or move event
		/// and then calls the actual event method.
		/// </summary>
		/// <param name="mousePos">Position of the mouse.</param>
		/// <returns>Returns if an event handler was called in OnMouseEnter or -Move.</returns>
		public bool OnMouse(Vector2 mousePos)
		{
			if (this.isHovered)
			{
				return this.OnMouseMove(mousePos);
			}
			else
			{
				return this.OnMouseEnter(mousePos);
			}
		}

		/// <summary>
		/// Is called when a MouseEnter event has to be raised.
		/// </summary>
		/// <param name="mousePos">Position of the mouse.</param>
		/// <returns>Returns if an enter event was called.</returns>
		public bool OnMouseEnter(Vector2 mousePos)
		{
			this.isHovered = true;
			if (this.mouseEnterStorage != null)
			{
				this.mouseEnterStorage(this, mousePos);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Is called when a MouseExit event has to be raised.
		/// </summary>
		/// <param name="mousePos"></param>
		/// <returns>Returns if an exit event was called.</returns>
		public bool OnMouseExit(Vector2 mousePos)
		{
			this.isHovered = false;
			if (this.mouseExitStorage != null)
			{
				this.mouseExitStorage(this, mousePos);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Is called when a MouseMove event has to be raised.
		/// </summary>
		/// <param name="mousePos">Position of the mouse.</param>
		/// <returns></returns>
		public bool OnMouseMove(Vector2 mousePos)
		{
			if (this.mouseMoveStorage != null)
			{
				this.mouseMoveStorage(this, mousePos);
				return true;
			}
			return false;
		}

		#endregion

		/// <summary>
		/// <para>Calculate the size based on a given element.</para>
		/// <para>Note: This method can only be called during DrawGUI or OnRepaint.</para>
		/// </summary>
		/// <returns>A vector with the needed witdth and height.</returns>
		/// <exception cref="System.ArgumentException">Thrown if not called during DrawGUI.</exception>
		public virtual Vector2 CalcSize()
		{
			return this.CalcSize(new GUIContent(this.Text));
		}
		
		/// <summary>
		/// <para>Calculate the size based on a given element.</para>
		/// <para>Note: This method can only be called during DrawGUI or OnRepaint.</para>
		/// </summary>
		/// <param name="content">The content the element should use.</param>
		/// <returns>A vector with the needed witdth and height.</returns>
		/// <exception cref="System.ArgumentException">Thrown if not called during DrawGUI.</exception>
		public virtual Vector2 CalcSize(GUIContent content)
		{
			if (LayerManager.Instance.InsideDrawGUI)
			{
				// We need to set the word wrapping to true due to a strange behaviour by the unity calc size method.
				// If word wrapping is enabled you may get only one character per line.
				bool wordWrap = this.normal.wordWrap;
				this.normal.wordWrap = false;
				Vector2 size = this.normal.CalcSize(content);
				this.normal.wordWrap = wordWrap;
				return size;
			}
			else
			{
				throw new System.ArgumentException("CalcHeight can only be called inside DrawGUI.");	
			}
		}

		/// <summary>
		/// <para>Calculate the height based on its content.</para>
		/// <para>Note: This method can only be called during DrawGUI or OnRepaint.</para>
		/// </summary>
		/// <param name="width">The width that the element should have.</param>
		/// <returns>The height needed to display the element.</returns>
		/// <exception cref="System.ArgumentException">Thrown if not called during DrawGUI.</exception>
		public virtual float CalcHeight(float width)
		{
			return this.CalcHeight(new GUIContent(this.Text), width);
		}

		/// <summary>
		/// <para>Calculate the height based on a given content.</para>
		/// <para>Note: This method can only be called during DrawGUI or OnRepaint.</para>
		/// </summary>
		/// <param name="width">The width that the element should have.</param>
		/// <param name="content">The content that the element should use.</param>
		/// <returns>The height needed to display the element.</returns>
		/// <exception cref="System.ArgumentException">Thrown if not called during DrawGUI.</exception>
		public virtual float CalcHeight(GUIContent content, float width)
		{
			if (LayerManager.Instance.InsideDrawGUI)
			{
				return this.normal.CalcHeight(content, width);
			}
			else
			{
				throw new System.ArgumentException("CalcHeight can only be called inside DrawGUI.");	
			}
		}

		protected object getStyleProperty(Types.StylePropertyType stylePropertyType)
		{
			return getStyleProperty(stylePropertyType, Types.StyleStateType.Normal);
		}

		protected virtual object getStyleProperty(Types.StylePropertyType stylePropertyType, Types.StyleStateType styleStateType)
		{
			object propertyValue = null;

			if (styleStateType == Types.StyleStateType.Active)
			{
				if (propertyValue == null)
					propertyValue = elementStyle.GetProperty(stylePropertyType, Types.StyleStateType.Active);
				if (propertyValue == null && classStyle != null)
					propertyValue = classStyle.GetProperty(stylePropertyType, Types.StyleStateType.Active);
			}

			if (styleStateType == Types.StyleStateType.Hover || styleStateType == Types.StyleStateType.Active)
			{
				if (propertyValue == null)
					propertyValue = elementStyle.GetProperty(stylePropertyType, Types.StyleStateType.Hover);
				if (propertyValue == null && classStyle != null)
					propertyValue = classStyle.GetProperty(stylePropertyType, Types.StyleStateType.Hover);
			}

			if (styleStateType == Types.StyleStateType.Focused || styleStateType == Types.StyleStateType.Active)
			{
				if (propertyValue == null)
					propertyValue = elementStyle.GetProperty(stylePropertyType, Types.StyleStateType.Focused);
				if (propertyValue == null && classStyle != null)
					propertyValue = classStyle.GetProperty(stylePropertyType, Types.StyleStateType.Focused);
			}

			if (propertyValue == null)
				propertyValue = elementStyle.GetProperty(stylePropertyType, Types.StyleStateType.Normal);
			if (propertyValue == null && classStyle != null)
				propertyValue = classStyle.GetProperty(stylePropertyType, Types.StyleStateType.Normal);

			if (propertyValue == null)
				propertyValue = defaultStyle.GetProperty(stylePropertyType, styleStateType);
			if (styleStateType == Types.StyleStateType.Active)
			{
				if (propertyValue == null)
					propertyValue = defaultStyle.GetProperty(stylePropertyType, Types.StyleStateType.Hover);
				if (propertyValue == null)
					propertyValue = defaultStyle.GetProperty(stylePropertyType, Types.StyleStateType.Focused);
			}
			if (propertyValue == null)
				propertyValue = defaultStyle.GetProperty(stylePropertyType, Types.StyleStateType.Normal);
			return propertyValue;
		}

		protected void setAllStyleProperties()
		{
			foreach (Types.StylePropertyType key in Enum.GetValues(typeof(Types.StylePropertyType)))
			{
				setStyleProperty(key);
			}
		}

		protected virtual void setStyleProperty(Types.StylePropertyType stylePropertyType)
		{
			object[] value = new object[4];
			value[0] = getStyleProperty(stylePropertyType);
			value[1] = getStyleProperty(stylePropertyType, Types.StyleStateType.Focused);
			value[2] = getStyleProperty(stylePropertyType, Types.StyleStateType.Hover);
			value[3] = getStyleProperty(stylePropertyType, Types.StyleStateType.Active);

			if (value[0] == null) return;

			switch (stylePropertyType)
			{
				case Types.StylePropertyType.height:
					this.height = (float)value[0];
					break;
				case Types.StylePropertyType.width:
					this.width = (float)value[0];
					break;
				case Types.StylePropertyType.top:
					this.top = (float)value[0];
					break;
				case Types.StylePropertyType.left:
					this.left = (float)value[0];
					break;
				case Types.StylePropertyType.name:
					this.name = (string)value[0];
					break;
				case Types.StylePropertyType.text:
					this.text = (string)value[0];
					break;
				case Types.StylePropertyType.enabled:
					this.enabled = (bool)value[0];
					break;
				case Types.StylePropertyType.visible:
					this.visible = (bool)value[0];
					break;
				case Types.StylePropertyType.wordwrap:
					this.normal.wordWrap = (bool)value[0];
					this.focused.wordWrap = (bool)value[0];
					this.hover.wordWrap = (bool)value[0];
					this.active.wordWrap = (bool)value[0];
					break;
				case Types.StylePropertyType.clipping:
					this.normal.clipping = (TextClipping)value[0];
					this.focused.clipping = (TextClipping)value[0];
					this.hover.clipping = (TextClipping)value[0];
					this.active.clipping = (TextClipping)value[0];
					break;

				//padding
				case Types.StylePropertyType.paddingtop:
					this.normal.padding.top = (int)value[0];
					this.focused.padding.top = (int)value[1];
					this.hover.padding.top = (int)value[2];
					this.active.padding.top = (int)value[3];
					break;
				case Types.StylePropertyType.paddingleft:
					this.normal.padding.left = (int)value[0];
					this.focused.padding.left = (int)value[1];
					this.hover.padding.left = (int)value[2];
					this.active.padding.left = (int)value[3];
					break;
				case Types.StylePropertyType.paddingright:
					this.normal.padding.right = (int)value[0];
					this.focused.padding.right = (int)value[1];
					this.hover.padding.right = (int)value[2];
					this.active.padding.right = (int)value[3];
					break;
				case Types.StylePropertyType.paddingbottom:
					this.normal.padding.bottom = (int)value[0];
					this.focused.padding.bottom = (int)value[1];
					this.hover.padding.bottom = (int)value[2];
					this.active.padding.bottom = (int)value[3];
					break;
				//margin
				case Types.StylePropertyType.margintop:
					this.normal.margin.top = (int)value[0];
					this.focused.margin.top = (int)value[1];
					this.hover.margin.top = (int)value[2];
					this.active.margin.top = (int)value[3];
					break;
				case Types.StylePropertyType.marginleft:
					this.normal.margin.left = (int)value[0];
					this.focused.margin.left = (int)value[1];
					this.hover.margin.left = (int)value[2];
					this.active.margin.left = (int)value[3];
					break;
				case Types.StylePropertyType.marginright:
					this.normal.margin.right = (int)value[0];
					this.focused.margin.right = (int)value[1];
					this.hover.margin.right = (int)value[2];
					this.active.margin.right = (int)value[3];
					break;
				case Types.StylePropertyType.marginbottom:
					this.normal.margin.bottom = (int)value[0];
					this.focused.margin.bottom = (int)value[1];
					this.hover.margin.bottom = (int)value[2];
					this.active.margin.bottom = (int)value[3];
					break;
				//border
				case Types.StylePropertyType.bordertop:
					this.normal.border.top = (int)value[0];
					this.focused.border.top = (int)value[1];
					this.hover.border.top = (int)value[2];
					this.active.border.top = (int)value[3];
					break;
				case Types.StylePropertyType.borderleft:
					this.normal.border.left = (int)value[0];
					this.focused.border.left = (int)value[1];
					this.hover.border.left = (int)value[2];
					this.active.border.left = (int)value[3];
					break;
				case Types.StylePropertyType.borderright:
					this.normal.border.right = (int)value[0];
					this.focused.border.right = (int)value[1];
					this.hover.border.right = (int)value[2];
					this.active.border.right = (int)value[3];
					break;
				case Types.StylePropertyType.borderbottom:
					this.normal.border.bottom = (int)value[0];
					this.focused.border.bottom = (int)value[1];
					this.hover.border.bottom = (int)value[2];
					this.active.border.bottom = (int)value[3];
					break;
				//overflow
				case Types.StylePropertyType.overflowtop:
					this.normal.overflow.top = (int)value[0];
					this.focused.overflow.top = (int)value[1];
					this.hover.overflow.top = (int)value[2];
					this.active.overflow.top = (int)value[3];
					break;
				case Types.StylePropertyType.overflowleft:
					this.normal.overflow.left = (int)value[0];
					this.focused.overflow.left = (int)value[1];
					this.hover.overflow.left = (int)value[2];
					this.active.overflow.left = (int)value[3];
					break;
				case Types.StylePropertyType.overflowright:
					this.normal.overflow.right = (int)value[0];
					this.focused.overflow.right = (int)value[1];
					this.hover.overflow.right = (int)value[2];
					this.active.overflow.right = (int)value[3];
					break;
				case Types.StylePropertyType.overflowbottom:
					this.normal.overflow.bottom = (int)value[0];
					this.focused.overflow.bottom = (int)value[1];
					this.hover.overflow.bottom = (int)value[2];
					this.active.overflow.bottom = (int)value[3];
					break;
				case Types.StylePropertyType.alignment:
					this.normal.alignment = (TextAnchor)value[0];
					this.focused.alignment = (TextAnchor)value[1];
					this.hover.alignment = (TextAnchor)value[2];
					this.active.alignment = (TextAnchor)value[3];
					break;
				case Types.StylePropertyType.contentoffsetleft:
					this.normal.contentOffset = new Vector2((float)value[0], this.normal.contentOffset.y);
					this.focused.contentOffset = new Vector2((float)value[1], this.focused.contentOffset.y);
					this.hover.contentOffset = new Vector2((float)value[2], this.hover.contentOffset.y);
					this.active.contentOffset = new Vector2((float)value[3], this.active.contentOffset.y);
					break;
				case Types.StylePropertyType.contentoffsettop:
					this.normal.contentOffset = new Vector2(this.normal.contentOffset.x, (float)value[0]);
					this.focused.contentOffset = new Vector2(this.focused.contentOffset.x, (float)value[1]);
					this.hover.contentOffset = new Vector2(this.hover.contentOffset.x, (float)value[2]);
					this.active.contentOffset = new Vector2(this.active.contentOffset.x, (float)value[3]);
					break;
				case Types.StylePropertyType.fontasset:
					this.normal.font = (Font)value[0];
					this.focused.font = (Font)value[1];
					this.hover.font = (Font)value[2];
					this.active.font = (Font)value[3];
					break;
# if (!UNITY_2_6)
# if (!UNITY_2_6_1)
				case Types.StylePropertyType.fontstyling:
					this.normal.fontStyle = (FontStyle)value[0];
					this.focused.fontStyle = (FontStyle)value[1];
					this.hover.fontStyle = (FontStyle)value[2];
					this.active.fontStyle = (FontStyle)value[3];
					break;
				case Types.StylePropertyType.fontsize:
					this.normal.fontSize = (int)value[0];
					this.focused.fontSize = (int)value[1];
					this.hover.fontSize = (int)value[2];
					this.active.fontSize = (int)value[3];
					break;
# endif
# endif
				case Types.StylePropertyType.textcolor:
					this.normal.normal.textColor = (Color)value[0];
					this.normal.focused.textColor = (Color)value[0];
					this.normal.hover.textColor = (Color)value[0];
					this.normal.active.textColor = (Color)value[0];
					this.focused.normal.textColor = (Color)value[1];
					this.focused.focused.textColor = (Color)value[1];
					this.focused.hover.textColor = (Color)value[1];
					this.focused.active.textColor = (Color)value[1];
					this.hover.normal.textColor = (Color)value[2];
					this.hover.focused.textColor = (Color)value[2];
					this.hover.hover.textColor = (Color)value[2];
					this.hover.active.textColor = (Color)value[2];
					this.active.normal.textColor = (Color)value[3];
					this.active.focused.textColor = (Color)value[3];
					this.active.hover.textColor = (Color)value[3];
					this.active.active.textColor = (Color)value[3];
					break;
				case Types.StylePropertyType.background:
					this.normal.normal.background = (Texture2D)value[0];
					this.normal.focused.background = (Texture2D)value[0];
					this.normal.hover.background = (Texture2D)value[0];
					this.normal.active.background = (Texture2D)value[0];
					this.focused.normal.background = (Texture2D)value[1];
					this.focused.focused.background = (Texture2D)value[1];
					this.focused.hover.background = (Texture2D)value[1];
					this.focused.active.background = (Texture2D)value[1];
					this.hover.normal.background = (Texture2D)value[2];
					this.hover.focused.background = (Texture2D)value[2];
					this.hover.hover.background = (Texture2D)value[2];
					this.hover.active.background = (Texture2D)value[2];
					this.active.normal.background = (Texture2D)value[3];
					this.active.focused.background = (Texture2D)value[3];
					this.active.hover.background = (Texture2D)value[3];
					this.active.active.background = (Texture2D)value[3];
					break;
				default: break;
			}
		}
		
		protected void updateGuiProperty(Types.StylePropertyType type)
		{
			setStyleProperty(type);
		}
		
		protected virtual void updateGuiStyles()
		{
			this.normal = new GUIStyle();
			this.focused = new GUIStyle();
			this.hover = new GUIStyle();
			this.active = new GUIStyle();

			setAllStyleProperties();
		}
	}
}
