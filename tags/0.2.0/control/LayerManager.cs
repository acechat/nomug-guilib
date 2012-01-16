using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace GUILib
{
	/// <summary>
	/// The layer manager is the base class of the GUI.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 298 $, $Date: 2012-01-16 23:34:33 +0100 (Mon, 16 Jan 2012) $</version>
	public class LayerManager
	{
		// You may set a console in "initialize()".
		public AConsole Console;

		/// <summary>
		/// Returns the reference of the focused control.
		/// </summary>
		public IElement FocusControl
		{
			get { return this.focusControl; }
		}

		/// <summary>
		/// Returns the reference of the active control.
		/// </summary>
		public IElement ActiveControl
		{
			get
			{
				if (this.activeControl == null || !this.activeControl.IsHovered)
				{
					return null;
				}
				else return this.activeControl;
			}
		}

		public EventType CurrentEventType
		{
			get { return this.currentEventType; }
		}

		private static LayerManager instance; // singleton reference

		private List<ALayer> layers; // list of all layers
		private List<ALayer> modalLayers; // list of modal layers (fifo)
		private Vector2 mousePos;
		private IElement focusControl;
		private IElement activeControl;
		private EventType currentEventType;
		private List<IElement> lastElementsAtMousePosition;
		private List<IElement> lastElementsAtMouseDown;
		private string loggerName; // leave unassigned to disable logging for GUILib framework
		private bool clearFocus;
		private bool resetFocus;
		private Vector2 dragStartMousePosition;
		private float dragThreshold;
		private bool insideDrawGUI;
		private DefaultStyles defaultStyles;
		private ToolTip toolTip;
		private MultiAnimation animationManager;

		public AssetBundleRegistry AssetBundleRegistry;

		private bool dragStarted;
		/// <summary>
		/// True if an element is currently being dragged.
		/// </summary>
		public bool Dragging
		{
			get { return this.dragStarted; }
		}
		/// <summary>
		/// The element which is currently dragged.
		/// </summary>
		private IElement draggingElement;
		/// <summary>
		/// The representation of the dragging element which is used for painting it under the mouse cursor.
		/// </summary>
		private DragElement draggingElementRepresentation;
		private IElement acceptingElement;

		/// <summary>
		/// The element that has accepted the drag over event.
		/// </summary>
		public IElement AcceptingElement
		{
			get { return this.acceptingElement; }
		}

		public float DragThreshold
		{
			get { return this.dragThreshold; }
			set
			{
				if (value >= 1)
				{
					this.dragThreshold = value;
				}
				else
				{
					this.LogWarning("DragThreshold must be greater or equal 1");
				}
			}
		}
		
		public bool InsideDrawGUI
		{
			get { return this.insideDrawGUI; }
		}
		
		public ALayer ModalLayer
		{
			get {
				if (this.modalLayers.Count > 0)
				{
					return this.modalLayers[modalLayers.Count - 1];
				}
				return null;
			}
		}
		
		public MultiAnimation AnimationManager
		{
			get
			{
				if(this.animationManager == null)
				{
					this.animationManager = new MultiAnimation(0.0f);
				}
				return this.animationManager;
			}
		}

		/// <summary>
		/// Constructor of LayerManager.
		/// </summary>
		private LayerManager()
		{
			this.layers = new List<ALayer>();
			this.modalLayers = new List<ALayer>();
			this.lastElementsAtMouseDown = new List<IElement>();
			this.lastElementsAtMousePosition = new List<IElement>();

			this.loggerName = "GUILib";
			this.DragThreshold = 5f;
			
			this.toolTip = null;
		}

		/// <summary>
		/// Singleton.
		/// </summary>
		public static LayerManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new LayerManager();
					instance.initialize();
				}
				return instance;
			}
		}

		public ReadOnlyCollection<ALayer> Layers
		{
			get { return this.layers.AsReadOnly(); }
		}

		// Use this for initialization
		public void Start()
		{
			foreach (ALayer layer in this.layers)
			{
				layer.Start();
			}
		}

		// Update is called once per frame
		public void Update()
		{
			this.AssetBundleRegistry.Update();

			foreach (ALayer layer in this.layers)
			{
				layer.Update();
			}
		}

		public Style GetDefaultStyle(object element)
		{
			if(this.defaultStyles == null)
			{
				this.defaultStyles = new DefaultStyles();
			}
			return this.defaultStyles.GetDefaultStyle(element);
		}

		/// <summary>
		/// Add a layer.
		/// </summary>
		/// <param name="layer">Layer</param>
		public void AddLayer(ALayer layer)
		{
			if (layer != null)
			{
				bool added = false;
				List<ALayer> tempList = new List<ALayer>(this.layers.Count + 1);
				foreach (ALayer currentLayer in this.layers)
				{
					// if not inserted check if the ZIndex is lower or if the currentLayer is modal (insert before the first modal layer).
					if (!added && currentLayer.ZIndex > layer.ZIndex)
					{
						// add the layer at the right spot
						tempList.Add(layer);
						added = true;
					}
					tempList.Add(currentLayer);
				}
				if (!added)
					tempList.Add(layer);
	
				this.layers = tempList;
				
				if (layer.IsModal)
				{
					// if the layer is modal add it to the modal list which represents the order...
					// insert the modal layer if the console layer is shown
					if (this.Console != null && modalLayers.Count > 0 && modalLayers[modalLayers.Count - 1] == this.Console.ConsoleLayer)
					{
						this.modalLayers.Insert(modalLayers.Count - 1, layer);
					}
					else
					{
						this.modalLayers.Add(layer);
					}
				}
			}
		}

		/// <summary>
		/// Removes a layer if it was added.
		/// </summary>
		/// <param name="layer">Reference to the layer that should be removed.</param>
		/// <returns>True if a layer was removed.</returns>
		public bool RemoveLayer(ALayer layer)
		{
			bool found = false;
			List<ALayer> tempList = new List<ALayer>(Math.Max(0, this.layers.Count - 1));
			foreach (ALayer currentLayer in this.layers)
			{
				if (currentLayer != layer)
				{
					tempList.Add(currentLayer);
				}
				else
				{
					found = true;
					if (layer.IsModal)
					{
						this.modalLayers.Remove(layer);
					}
				}
			}

			this.layers = tempList;
			
			return found;
		}

		/// <summary>
		/// <para>This method should be called once in your OnGUI in your game loop.</para>
		/// <para>It draws the GUI and manages the events.</para>
		/// </summary>
		public void DrawGUI()
		{
			this.insideDrawGUI = true;
			this.currentEventType = Event.current.type;
			
			//if (Event.current.type != EventType.repaint && Event.current.type != EventType.layout)
			//    Debug.Log(Event.current.type);

			// check for mouseMove
			if (!this.mousePos.Equals(Event.current.mousePosition))
			{
				this.toolTip = null;
				// mouseMove event
				if (new Rect(0, 0, Screen.width, Screen.height).Contains(Event.current.mousePosition))
				{
					// this mouse move event is for us
					List<IElement> elementsAtMousePosition = this.findElementsAtMousePosition(Event.current.mousePosition, true, false);

					foreach (IElement elem in elementsAtMousePosition)
					{
						// fire mouse event for each element at the mouse position
						if (elem is IMouseEvents)
						{
							((IMouseEvents)elem).OnMouse(Event.current.mousePosition);
							

						}
						// find tooltip to draw
						if(this.toolTip == null)
						{
							this.toolTip = elem.ToolTip;
						}
					}
					
					
					if (this.lastElementsAtMousePosition != null)
					{
						// OnMouseExit calculation
						// find the difference between the current elements and the elements of the last event to find the exited ones
						foreach (IElement elem in this.lastElementsAtMousePosition)
						{
							if (elem is IMouseEvents && elementsAtMousePosition.IndexOf(elem) == -1)
							{
								((IMouseEvents)elem).OnMouseExit(this.mousePos);
							}
						}
					}

					this.lastElementsAtMousePosition = elementsAtMousePosition;
				}
				this.mousePos = Event.current.mousePosition;
			}

			// handle unity events
			List<IElement> elementsAtMousePos;

			switch (Event.current.type)
			{
				case EventType.Repaint:
					
					if (this.toolTip != null)
					{
						this.toolTip.OnRepaint();
					}
					foreach (ALayer layer in this.layers)
					{
						layer.OnRepaint();
					}
					if(this.animationManager != null)
					{
						this.animationManager.Step();
					}
					break;
				case EventType.MouseUp: // click and mouseup event
					bool clickFired = false;
					bool mouseUpFired = false;
					// What elements are present at current mouse position?
					elementsAtMousePos = findElementsAtMousePosition(Event.current.mousePosition, true, true);

					// Fire mouse events if nothing was dragged until now.
					if (!this.Dragging)
					{
						foreach (IElement elem in elementsAtMousePos)
						{
							if (elem is IClickEvents && this.lastElementsAtMouseDown.Contains(elem))
							{
								if (!clickFired && ((IClickEvents)elem).OnClick())
								{
									// Ff an element uses this event than stop. Underlying clicks shouldn't be called!
									// Only fire event on topmost control.
									clickFired = true;
								}
							}
							if (elem is IClickEvents)
							{
								if (!mouseUpFired && ((IClickEvents)elem).OnMouseUp())
								{
									// Only fire event on topmost control.
									mouseUpFired = true;
								}
							}
						}
					}
					else
					{
						// handle drag drop
						foreach (IElement elem in elementsAtMousePos)
						{
							if (elem is IDragEvents && ((IDragEvents)elem).OnDragDrop(this.draggingElement))
							{
								break;
							}
						}

						if (this.draggingElementRepresentation.Settings.HideCursorWhileDragging)
						{
							Screen.showCursor = true;
						}

						this.draggingElement = null;
						this.draggingElementRepresentation = null;
						this.dragStarted = false;
					}
				
				
					this.resetFocus = true;
					this.activeControl = null;
					break;
				case EventType.MouseDown:
					bool controlFocused = false;
					bool mouseDownFired = false;
					// set active control
					elementsAtMousePos = this.findElementsAtMousePosition(Event.current.mousePosition, true, true);
					this.lastElementsAtMouseDown = elementsAtMousePos;
					this.dragStartMousePosition = Event.current.mousePosition;
					foreach (IElement elem in elementsAtMousePos)
					{
						if (!controlFocused && (elem is IClickEvents || elem is IChangeEvents))
						{
							// only activate topmost control
							this.SetFocus(elem);
							this.activeControl = elem;
							controlFocused = true;
						}
						if (elem is IClickEvents)
						{
							if (!mouseDownFired && ((IClickEvents)elem).OnMouseDown())
							{
								// only fire event on topmost control
								mouseDownFired = true;
							}
						}
					}
					// clear focus if no element has been found
					if(!controlFocused)
					{
						this.ClearFocus();
					}
					break;
				case EventType.ScrollWheel:
					elementsAtMousePos = findElementsAtMousePosition(Event.current.mousePosition, true, true);
					
					foreach (IElement elem in elementsAtMousePos)
					{
						if (elem is ScrollBox)
						{
							ScrollBox scrollBox = ((ScrollBox)elem);
							scrollBox.VerticalScrollPosition = Math.Min(Math.Max(scrollBox.VerticalScrollPosition + Event.current.delta.y * 5, 0), scrollBox.VerticalScrollMaximum);
							break;
						}
						if (elem is ListBox)
						{
							ListBox listBox = ((ListBox)elem);
							listBox.ScrollPosition = Math.Min(Math.Max(listBox.ScrollPosition + Event.current.delta.y * 5, 0), listBox.ScrollMaximum);
							break;
						}
					}
					
					break;
				case EventType.MouseDrag:
					// mouse drag handling
					if (!dragStarted)
					{
						// check if the mouse was moved beyond the threshold.
						if (Math.Abs(this.dragStartMousePosition.x - Event.current.mousePosition.x) > this.dragThreshold
							|| Math.Abs(this.dragStartMousePosition.y - Event.current.mousePosition.y) > this.dragThreshold)
						{
							foreach (IElement elem in this.lastElementsAtMouseDown)
							{
								if (elem is Slider)
								{
									break;
								}
								if (elem is IDragEvents && ((IDragEvents)elem).OnDragStart())
								{
									this.draggingElement = elem;
									this.dragStarted = true;

									// clear the accepted element
									this.acceptingElement = null;

									// initialize the representation of the dragged element
									if (((IDragEvents)elem).DragElement != null)
									{
										this.draggingElementRepresentation = ((IDragEvents)elem).DragElement;
									}
									else
									{
										// no drag representation set. using default instead.
										if (elem is ICloneable)
										{
											// create an drag element with a clone of the element which is being dragged.
											this.draggingElementRepresentation = new DragElement(
												(IElement)((ICloneable)elem).Clone()
											);
										}
										else
										{
											this.LogError("Couldn't create a clone of " + elem.GetType().ToString() + " '" + elem.Name + "'. No representation of element will follow the mouse position.");
										}
									}

									// hide the cursor if requested
									if (this.draggingElementRepresentation.Settings.HideCursorWhileDragging)
									{
										Screen.showCursor = false;
									}

									this.activeControl = null;
									this.ClearFocus();

									break;
								}
							}
						}
					}
					else
					{
						// drag over event handling
						elementsAtMousePos = this.findElementsAtMousePosition(Event.current.mousePosition, true, true);

						// clear the accepted element so that it reflects the element that has accepted the last dragOver
						// while dragging but than remains until a new drag starts
						this.acceptingElement = null;

						foreach (IElement elem in elementsAtMousePos)
						{
							if (elem is IDragEvents && ((IDragEvents)elem).OnDragOver(this.draggingElement))
							{
								// TODO: actually do something to show the user that you can drop the element here.
								this.acceptingElement = elem;
							}
						}
					}
					break;
				case EventType.KeyUp:
					if (Event.current.keyCode == Configuration.ConsoleKey && this.Console != null)
					{
						this.Console.ConsoleLayer.Visible = !this.Console.ConsoleLayer.Visible;
						if (this.Console.ConsoleLayer.Visible)
						{
							this.AddLayer(this.Console.ConsoleLayer);
							this.Console.ConsoleLayer.Show();
						}
						else
						{
							this.RemoveLayer(this.Console.ConsoleLayer);
						}
					}
				
					bool keyUpFired = false;
					if(this.FocusControl != null)
					{
						if(this.FocusControl is IKeyEvents)
						{
							keyUpFired = (this.focusControl as IKeyEvents).OnKeyUp(Event.current.keyCode);
						}
					
						if(!keyUpFired)
						{
							IElementCollection parentElement = this.focusControl.Parent;
							while(parentElement is IElement)
							{
								parentElement = (parentElement as IElement).Parent;
							}
							if(parentElement is IKeyEvents)
							{
								keyUpFired = (parentElement as IKeyEvents).OnKeyUp(Event.current.keyCode);
							}
						}
						if(!keyUpFired)
						{
							foreach(ALayer layer in this.layers)
							{
								if(layer is IKeyEvents && !keyUpFired)
								{
									keyUpFired = (layer as IKeyEvents).OnKeyUp(Event.current.keyCode);
								}
							}
						}
					}
					break;
				case EventType.KeyDown:
					bool keyDownFired = false;
					if(this.FocusControl != null)
					{
						if(this.FocusControl is IKeyEvents)
						{
							keyDownFired = (this.focusControl as IKeyEvents).OnKeyDown(Event.current.keyCode);
						}
					
						if(!keyDownFired)
						{
							IElementCollection parentElement = this.focusControl.Parent;
							while(parentElement is IElement)
							{
								parentElement = (parentElement as IElement).Parent;
							}
							if(parentElement is IKeyEvents)
							{
								keyDownFired = (parentElement as IKeyEvents).OnKeyDown(Event.current.keyCode);
							}
						}
						if(!keyDownFired)
						{
							foreach(ALayer layer in this.layers)
							{
								if(layer is IKeyEvents && !keyDownFired)
								{
									keyDownFired = (layer as IKeyEvents).OnKeyDown(Event.current.keyCode);
								}
							}
						}
					}
				
					break;
			}

			// forward OnGUI to all elements
			foreach (ALayer layer in this.layers)
			{
				if (layer.Visible)
				{
					layer.DrawGUI();
				}
			}

			// remove keyboard focus if clearFocus is set
			if (this.clearFocus)
			{
				this.clearFocus = false;
				GUIUtility.keyboardControl = 0;
			}

			// reset focus to named control
			if (this.resetFocus)
			{
				this.resetFocus = false;
				if (this.focusControl != null && (this.focusControl is EditBox || this.focusControl is TextArea))
				{
					GUI.FocusControl(this.focusControl.GUID);
				}
				else
				{
					GUIUtility.keyboardControl = 0;
				}
			}

			// paint drag cursor
			if (this.draggingElementRepresentation != null)
			{
				this.draggingElementRepresentation.DrawGUI();
			}
			
			// paint tooltip
			if(this.toolTip != null)
			{
				this.toolTip.DrawGUI();
			}
			
			this.insideDrawGUI = false;
		}

		/// <summary>
		/// Get a list of ALL elements that are present at the given mouse position.
		/// </summary>
		/// <seealso cref="findChildrenElementsAtMousePosition"/>
		/// <param name="mousePosition">The mouse position where the elements should be searched.</param>
		/// <param name="onlyVisible">Whether the method should return only visible elements or not.</param>
		/// <param name="onlyEnabled">Whether the method should return only elements which are enabled or not.</param>
		/// <returns>A list with all elements which are at the given position.</returns>
		public List<IElement> findElementsAtMousePosition(Vector2 mousePosition, bool onlyVisible, bool onlyEnabled)
		{
			return this.findElementsAtMousePosition(mousePosition, onlyVisible, onlyEnabled, true);
		}
		
		/// <summary>
		/// Get a list of ALL elements that are present at the given mouse position.
		/// </summary>
		/// <seealso cref="findChildrenElementsAtMousePosition"/>
		/// <param name="mousePosition">The mouse position where the elements should be searched.</param>
		/// <param name="onlyVisible">Whether the method should return only visible elements or not.</param>
		/// <param name="onlyEnabled">Whether the method should return only elements which are enabled or not.</param>
		/// <param name="onlyModal">Whether the method should return only elements from the modal layer.</param>
		/// <returns>A list with all elements which are at the given position.</returns>
		public List<IElement> findElementsAtMousePosition(Vector2 mousePosition, bool onlyVisible, bool onlyEnabled, bool onlyModal)
		{
			// sorted implementation (highest first)
			List<IElement> foundElements = new List<IElement>();
			
			List<ALayer> layers;
			if (onlyModal && this.modalLayers.Count > 0)
			{
				layers = new List<ALayer>();
				layers.Add(this.modalLayers[this.modalLayers.Count - 1]);
			}
			else
			{
				layers = this.layers;
			}
			// go through all layers
			for (int i = layers.Count - 1; i >= 0; i--)
			{
				if ((!onlyVisible || layers[i].IsVisible) && (!onlyEnabled || layers[i].IsEnabled))
				{
					foundElements.AddRange(this.findChildrenElementsAtMousePosition(mousePosition, layers[i], onlyVisible, onlyEnabled));
				}
			}
			return foundElements;
		}

		/// <summary>
		/// Get a list of children elements that are present at the given mouse position.
		/// </summary>
		/// <param name="mousePosition">The mouse position where the elements should be searched.</param>
		/// <param name="parent">The reference to an object that has children.</param>
		/// <param name="onlyVisible">Whether the method should return only visible elements or not.</param>
		/// <param name="onlyEnabled">Wheter the method should return only elements which are enabled or not.</param>
		/// <returns>A list with all elements which are at the given position.</returns>
		public List<IElement> findChildrenElementsAtMousePosition(Vector2 mousePosition, IElementCollection parent, bool onlyVisible, bool onlyEnabled)
		{
			List<IElement> foundElements = new List<IElement>();
			ReadOnlyCollection<IElement> children = parent.Children;
			// go through elements (highest first)
			for (int i = parent.Count - 1; i >= 0; i--)
			{
				// calculate absolute rectangle of child element
				IElement child = children[i];
				Rect childRect = new Rect(child.AbsoluteLeft, child.AbsoluteTop, child.Width, child.Height);

				// check if element is under the mouse position (and take only the ones which are visible and enabled if the parameters are set)
				if ((!onlyVisible || child.IsVisible) && (!onlyEnabled || child.IsEnabled) && childRect.Contains(mousePosition))
				{
					if (child is IElementCollection)
					{
						foundElements.AddRange(this.findChildrenElementsAtMousePosition(mousePosition, (IElementCollection)child, onlyVisible, onlyEnabled));
					}
					foundElements.Add(child);
				}
			}
			return foundElements;
		}
		
		/// <summary>
		/// Sort layers by z-index.
		/// </summary>
		public void SortLayers()
		{
			this.layers.Sort(
				delegate(ALayer layer1, ALayer layer2)
				{
					// if both are modal sort by the order in the modal layer list.
					if (layer1.IsModal && layer2.IsModal)
					{
						return this.modalLayers.IndexOf(layer1).CompareTo(this.modalLayers.IndexOf(layer2));
					}
					return layer1.ZIndex.CompareTo(layer2.ZIndex);
				}
			);
		}

		/// <summary>
		/// Remove the focus from any focused element.
		/// </summary>
		public void ClearFocus()
		{
			this.focusControl = null;
			this.clearFocus = true;
		}
		
		/// <summary>
		/// Set the focus to the given element. 
		/// </summary>
		/// <param name="element">
		/// Element to be focused.
		/// </param>
		public void SetFocus(IElement element)
		{
			if(element != null)
			{
				this.focusControl = element;
				this.resetFocus = true;
			}
		}

		/// <summary>
		/// <para>Logs a message if a console has been added to the LayerManager.</para>
		/// <para>Only use this inside the GUILib.</para>
		/// </summary>
		/// <param name="msg">Message which should be logged.</param>
		public void Log(string msg)
		{
			if (this.Console != null && this.loggerName != null)
				this.Console.GetLogger(this.loggerName).Debug(msg);
		}

		/// <summary>
		/// <para>Logs a warning message if a console has been added to the LayerManager.</para>
		/// <para>Only use this inside the GUILib.</para>
		/// </summary>
		/// <param name="msg">Warning which should be logged.</param>
		public void LogWarning(string msg)
		{
			if (this.Console != null && this.loggerName != null)
				this.Console.GetLogger(this.loggerName).Warn(msg);
		}

		/// <summary>
		/// <para>Logs an error message if a console has been added to the LayerManager.</para>
		/// <para>Only use this inside the GUILib.</para>
		/// </summary>
		/// <param name="msg">Error message which should be logged.</param>
		public void LogError(string msg)
		{
			if (this.Console != null && this.loggerName != null)
				this.Console.GetLogger(this.loggerName).Error(msg);
		}

		/// <summary>
		/// This method will be called directly after the constructor. It initializes code that uses the LayerManager instance.
		/// </summary>
		private void initialize()
		{
			// Remove the following lines if you don't want to use the frameworks console
			// or create your own AConsole implementation.
			// (You may want to change logging settings in Logger.cs.)
			this.Console = global::Console.Instance;
			
			// create a new AssetBundleRegistry (needs console to be created earlier for logging).
			this.AssetBundleRegistry = new AssetBundleRegistry();
		}
	}
}