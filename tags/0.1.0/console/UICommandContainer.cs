using System;
using UnityEngine;
using System.Collections.ObjectModel;

namespace GUILib
{
	/// <summary>
	/// This container implements useful debugging commands for developing user interfaces with the GUILib.
	/// </summary>
	/// <author>Matthias Stock (mstock)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 256 $, $Date: 2011-06-23 18:41:47 +0200 (Do, 23 Jun 2011) $</version>
	public class UICommandContainer : GUILib.ACommandContainer
	{
		public UICommandContainer ()
		{
			this.ContainerDictionary.Add("ping", this.pong);
			this.ContainerDictionary.Add("ui_layer_info", this.logOverlays);
			this.ContainerDictionary.Add("ui_remove_layer", this.removeOverlay);
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
		
		private void logOverlays(string s)
		{
			ReadOnlyCollection<AOverlay> overlays = OverlayManager.Instance.Overlays;
			int parsedInt;
			int count = (Int32.TryParse(s, out parsedInt) ? Math.Min(parsedInt, overlays.Count) : overlays.Count);
			string modal;
			
			for (int i = 1; i <= count; i++)
			{
				modal = (OverlayManager.Instance.ModalOverlay == overlays[overlays.Count - i] ? "Modal: True, " : "");
				this.print((i) + " " + overlays[overlays.Count - i].Name + " (" + modal + "Visible: " + overlays[overlays.Count - i].Visible + ", Enabled: " + overlays[overlays.Count - i].Enabled + ", ZIndex: " + overlays[overlays.Count - i].ZIndex + ")");
			}
		}
		
		private void removeOverlay(string s)
		{
			int count = 0;
			foreach (AOverlay ov in OverlayManager.Instance.Overlays)
			{
				if (ov.Name.Equals(s))
				{
					if (OverlayManager.Instance.RemoveOverlay(ov))
						count++;
				}
			}
			this.print(count + " overlays have been removed.");
		}
		
		private void showUI(string s)
		{
			bool visible = this.parseStringToBool(s);
			foreach (AOverlay ov in OverlayManager.Instance.Overlays)
			{
				if (OverlayManager.Instance.Console != null && OverlayManager.Instance.Console.ConsoleOverlay != null && OverlayManager.Instance.Console.ConsoleOverlay != ov)
					ov.Visible = visible;
			}
		}
		
		private void showMask(string s)
		{
			ElementMaskOverlay maskOverlay = null;
			int interval = 0;
			#region read params
			if ("".Equals(s))
			{
				maskOverlay = new ElementMaskOverlay(100);
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
							maskOverlay = new ElementMaskOverlay(100);
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
								maskOverlay = new ElementMaskOverlay(interval);
							}
						}
						else
						{
							if (arr.Length > 1)
							{
								if (Int32.TryParse(arr[1], out interval) && interval > 0)
								{
									maskOverlay = new ElementMaskOverlay(interval, arr[0]);
								}
							}
							else
							{
								maskOverlay = new ElementMaskOverlay(100, arr[0]);
							}
						}
					}
				}
			}
			#endregion
			
			if (maskOverlay == null)
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
			OverlayManager.Instance.AddOverlay(maskOverlay);
		}
		
		private void removeThis()
		{
			foreach (AOverlay ov in OverlayManager.Instance.Overlays)
			{
				if (ov is ElementMaskOverlay)
				{
					OverlayManager.Instance.RemoveOverlay(ov);
					break;
				}
			}
		}
	}
}