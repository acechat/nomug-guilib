using System;
using UnityEngine;
using System.Collections.ObjectModel;

namespace GUILib
{
	/// <summary>
	/// This container implements useful debugging commands for developing user interfaces with the GUILib.
	/// </summary>
	/// <author>Matthias Stock (mstock)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 296 $, $Date: 2012-01-16 23:24:28 +0100 (Mon, 16 Jan 2012) $</version>
	public class UICommandContainer : GUILib.ACommandContainer
	{
		public UICommandContainer ()
		{
			this.ContainerDictionary.Add("ping", this.pong);
			this.ContainerDictionary.Add("ui_layer_info", this.logLayers);
			this.ContainerDictionary.Add("ui_remove_layer", this.removeLayer);
			this.ContainerDictionary.Add("ui_show", this.showUI);
			this.ContainerDictionary.Add("ui_show_mask", this.showMask);
		}
	
		/// <summary>
		/// Simple sample request -> answer method.
		/// </summary>
		/// <param name="parameters">Parameters as string given from ConsoleRegistry.
		/// </param>
		private void pong(string parameters)
		{
			this.print("PONG!");
		}
		
		private void logLayers(string s)
		{
			ReadOnlyCollection<ALayer> layers = LayerManager.Instance.Layers;
			int parsedInt;
			int count = (Int32.TryParse(s, out parsedInt) ? Math.Min(parsedInt, layers.Count) : layers.Count);
			string modal;
			
			for (int i = 1; i <= count; i++)
			{
				modal = (LayerManager.Instance.ModalLayer == layers[layers.Count - i] ? "Modal: True, " : "");
				this.print((i) + " " + layers[layers.Count - i].Name + " (" + modal + "Visible: " + layers[layers.Count - i].Visible + ", Enabled: " + layers[layers.Count - i].Enabled + ", ZIndex: " + layers[layers.Count - i].ZIndex + ")");
			}
		}
		
		private void removeLayer(string s)
		{
			int count = 0;
			foreach (ALayer ov in LayerManager.Instance.Layers)
			{
				if (ov.Name.Equals(s))
				{
					if (LayerManager.Instance.RemoveLayer(ov))
						count++;
				}
			}
			this.print(count + " layers have been removed.");
		}
		
		private void showUI(string s)
		{
			bool visible = this.parseStringToBool(s);
			foreach (ALayer ov in LayerManager.Instance.Layers)
			{
				if (LayerManager.Instance.Console != null && LayerManager.Instance.Console.ConsoleLayer != null && LayerManager.Instance.Console.ConsoleLayer != ov)
					ov.Visible = visible;
			}
		}
		
		private void showMask(string s)
		{
			ElementMaskLayer maskLayer = null;
			int interval = 0;
			#region read params
			if ("".Equals(s))
			{
				maskLayer = new ElementMaskLayer(100);
			}
			else
			{
				string[] arr = s.Split(' ');
				if (!"-h".Equals(arr[0]))
				{
					bool? visibility = this.tryParseStringToBool(arr[0]);
					if (visibility.HasValue)
					{
						if (visibility.Value)
						{
							maskLayer = new ElementMaskLayer(100);
						}
						else
						{
							this.removeThis();
							return;
						}
					}
					else
					{
						if (Int32.TryParse(arr[0], out interval))
						{
							if (interval > 0)
							{
								maskLayer = new ElementMaskLayer(interval);
							}
						}
						else
						{
							if (arr.Length > 1)
							{
								if (Int32.TryParse(arr[1], out interval) && interval > 0)
								{
									maskLayer = new ElementMaskLayer(interval, arr[0]);
								}
							}
							else
							{
								maskLayer = new ElementMaskLayer(100, arr[0]);
							}
						}
					}
				}
			}
			#endregion
			
			if (maskLayer == null)
			{
				if (interval >= 0)
				{
					this.print("Error: updateInterval parameter has to be positive.");
				}
				else
				{
					this.print("Usage: \"ui_show_mask [visible|updateInterval|layerName [updateInterval]]\".");
				}
				return;
			}
			
			this.removeThis();
			this.print("Showing user interface mask.");
			LayerManager.Instance.AddLayer(maskLayer);
		}
		
		private void removeThis()
		{
			foreach (ALayer ov in LayerManager.Instance.Layers)
			{
				if (ov is ElementMaskLayer)
				{
					LayerManager.Instance.RemoveLayer(ov);
					break;
				}
			}
		}
	}
}