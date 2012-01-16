using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GUILib;

/// <summary>
/// Shows where the boundaries of elements are.
/// </summary>
/// <author>Henning Vreyborg (marshen)</author>
/// <author>$LastChangedBy: ven $</author>
/// <version>$Rev: 296 $, $Date: 2012-01-16 23:24:28 +0100 (Mon, 16 Jan 2012) $</version>
public class ElementMaskLayer : ALayer {
	// list of buttons which will be used again to minimize garbage collection
	private List<Button> buttonList = new List<Button>();
	
	// if set only elements from the type of this layer will be shown
	private string layerName;
	
	private float updateInterval;
	
	private float lastUpdate = 0f;
	
	private Texture2D background;
	
	private int lastElementCount = 0;
	
	/// <summary>
	/// Creates an ElementPositionLayer which will show layers for all elements.
	/// </summary>
	/// <param name="updateInterval">The interval for updating the positions in milliseconds.</param>
	public ElementMaskLayer(int updateInterval) : this(updateInterval, "")
	{
		
	}
	
	/// <summary>
	/// Creates an ElementPositionLayer which will show layers for all elements of an layer.
	/// </summary>
	/// <param name="updateInterval">The interval for updating the positions in milliseconds.</param>
	/// <param name="layerName">The name of the layer you want to track (if you use an layer multiple times all elements of them will be tracked).</param>
	public ElementMaskLayer(int updateInterval, string layerName)
	{
		this.ZIndex = Configuration.MaxZIndex;
		this.layerName = layerName;
		this.lastUpdate = Time.realtimeSinceStartup;
		this.updateInterval = updateInterval / 1000f;
		
		this.background = new Texture2D(5, 5);
		for (int x = 0; x < this.background.width; x++)
		{
			for (int y = 0; y < this.background.height; y++)
			{
				if (x == 0 || x == this.background.width - 1 || y == 0 || y == this.background.height- 1)
				{
					this.background.SetPixel(x, y, new Color(0f, 1f, 0f, 0.75f));
				}
				else
				{
					if (x == 1 || x == this.background.width - 2 || y == 1 || y == this.background.height - 2)
					{
						this.background.SetPixel(x, y, new Color(0f, 1f, 0f, 0.25f));
					}
					else
					{
						this.background.SetPixel(x, y, new Color(0f, 0f, 0f, 0.1f));
					}
				}
			}
		}
		this.background.Apply();
	}
	
	public override void Update ()
	{
		if (this.lastUpdate + this.updateInterval < Time.realtimeSinceStartup)
		{
			this.lastUpdate = Time.realtimeSinceStartup;
			this.updateElements();
		}
	}
	
	/// <summary>
	/// Update position and size of all elements.
	/// </summary>
	private void updateElements()
	{
		List<IElement> elements = this.findElements(this.layerName);
		Button b;
		
		while (this.Count > this.lastElementCount)
		{
			this.RemoveElement(this.Children[this.lastElementCount]);
		}
		
		for (int i = 0; i < elements.Count; i++)
		{
			b = this.getButton(i);
			b.Text = elements[i].Name;
			b.Left = elements[i].AbsoluteLeft;
			b.Top = elements[i].AbsoluteTop;
			b.Width = elements[i].Width;
			b.Height = elements[i].Height;
		}
		
		this.lastElementCount = elements.Count;
	}
	
	/// <summary>
	/// Recursively returns all elements of an layer or all layers.
	/// </summary>
	/// <param name="layerName">The name of the layer you want to track. May be the empty string to track all layers.</param>
	/// <returns>Returns list of all elements in layers.</returns>
	private List<IElement> findElements(string layerName)
	{
		List<IElement> elements = new List<IElement>();
		bool allLayers = ("".Equals(layerName));
		foreach (ALayer ov in LayerManager.Instance.Layers)
		{
			if (ov != this && (allLayers || layerName.Equals(ov.Name)))
			{
				elements.AddRange(this.findElements(ov));
			}
		}
		
		return elements;
	}
	
	/// <summary>
	/// Recursively returns all elements of a collection.
	/// </summary>
	/// <param name="collection">The collection from which you need all elements.</param>
	/// <returns>Returns list of all elements in a collection.</returns>
	private List<IElement> findElements(IElementCollection collection)
	{
		List<IElement> elements = new List<IElement>();
		
		foreach (IElement elem in collection.Children)
		{
			if (elem is IElementCollection)
			{
				elements.AddRange(this.findElements(elem as IElementCollection));
			}
			elements.Add(elem);
		}
		
		return elements;
	}
	
	/// <summary>
	/// Returns a button.
	/// This method creates a new button if there are to few and adds buttons that are no layer children at the moment.
	/// </summary>
	/// <param name="number">The number of the next button needed.</param>
	/// <returns>Button.</returns>
	private Button getButton(int number)
	{
		if (number >= this.buttonList.Count)
		{
			Button b = new Button();
			b.ElementStyle.SetBorder(2);
			b.ElementStyle.Background = this.background;
			b.ElementStyle.Alignment = TextAnchor.MiddleCenter;
			b.ElementStyle.TextColor = new Color(0f, 0f, 0f, 0.4f);
# if (!UNITY_2_6)
# if (!UNITY_2_6_1)
			b.ElementStyle.FontStyling = FontStyle.Bold;
			b.ElementStyle.FontSize = 12;
# endif
# endif
			this.buttonList.Add(b);
		}
		if (number >= this.lastElementCount)
		{
			this.AddElement(this.buttonList[number]);
		}
		return this.buttonList[number];
	}
}
