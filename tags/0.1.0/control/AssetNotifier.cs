using UnityEngine;
using System;
using System.Collections.Generic;

namespace GUILib
{
	/// <summary>
	/// AssetNotifier class to notify Styles waiting for AssetBundles to complete. 
	/// </summary>
	/// <author>Manuel Fasse (ven)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 216 $, $Date: 2011-05-16 18:33:44 +0200 (Mo, 16 Mai 2011) $</version>
	public class AssetNotifier
	{
		private Dictionary<string, List<WeakReference>> notificationList;
		private static AssetNotifier instance;
		
		/// <summary>
		/// Constructor. 
		/// </summary>
		public AssetNotifier ()
		{
			notificationList = new Dictionary<string, List<WeakReference>>();
		}
		
		// singleton
		public static AssetNotifier Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new AssetNotifier();
				}
				return instance;
			}
		}
		
		/// <summary>
		/// Add a Style that's waiting for an AssetBundle.
		/// </summary>
		/// <param name="name">
		/// Name of the AssetBundle the Style is waiting for.
		/// </param>
		/// <param name="style">
		/// Style that is waiting for the AssetBundle.
		/// </param>
		public void AddListener(string name, Style style)
		{
			if(!notificationList.ContainsKey(name))
			{	
				notificationList.Add(name, new List<WeakReference>());
			}
			
			bool found = false;
			for(int i=0; i<notificationList[name].Count; i++)
			{
				if(notificationList[name][i].Target == style)
				{
					found = true;
				}
			}
			if(!found)
			{
				notificationList[name].Add(new WeakReference(style));
			}
		}
		
		/// <summary>
		/// Remove Style that is waiting for an AssetBundle.
		/// </summary>
		/// <param name="name">
		/// Name of the AssetBundle the Style was waiting for.
		/// </param>
		/// <param name="style">
		/// Style that is to be removed.
		/// </param>
		public void RemoveListener(string name, Style style)
		{
			if(notificationList.ContainsKey(name))
			{			
				for(int i=0; i<notificationList[name].Count; i++)
				{
					if(notificationList[name][i].Target == style)
					{
						notificationList[name].RemoveAt(i);
					}
				}
				if(notificationList[name].Count == 0)
					notificationList.Remove(name);
			}
		}
		
		/// <summary>
		/// Call this method with the name of an AssetBundle when it
		/// finished loading.
		/// </summary>
		/// <param name="name">
		/// Name of the AssetBundle that was completed.
		/// </param>
		public void AssetBundleLoaded(string name)
		{
			if(notificationList.ContainsKey(name))
			{
				Style style;
				for(int i=0; i<notificationList[name].Count; i++)
				{
					style = (Style)notificationList[name][i].Target;
					if(notificationList[name][i].IsAlive)
					{
						style.CheckLoadAssets(name);
					}	
				}
			}
			notificationList.Remove(name);
		}
	}
}