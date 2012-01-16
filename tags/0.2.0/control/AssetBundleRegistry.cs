using UnityEngine;
using System.Collections.Generic;

namespace GUILib
{
	/// <summary>
	/// This class manages your asset bundles. It is needed if you want to load textures in styles via the path property.
	/// The AssetBundleRegistry can also manage the download of asset bundles if you want.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 293 $, $Date: 2012-01-16 19:18:36 +0100 (Mon, 16 Jan 2012) $</version>
	public class AssetBundleRegistry
	{
		// Whether loading asset bundles is used or not.
		private bool usingLoaded;

		private float accumulatedProgress;
		private int finishedDownloads;
		private int runningDownloads;

		private Dictionary<string, AssetBundle> assetBundles;
		private Dictionary<string, AssetBundleInformation> loadingBundles;
		private List<string> finishedBundles;

		private Logger logger;

		// Used to lock loadingBundles.
		private readonly object locker;

		/// <summary>
		/// Constructor.
		/// </summary>
		public AssetBundleRegistry()
		{
			this.usingLoaded = false;

			this.accumulatedProgress = 0f;
			this.finishedDownloads = 0;
			this.runningDownloads = 0;

			this.assetBundles = new Dictionary<string, AssetBundle>();
			this.loadingBundles = new Dictionary<string, AssetBundleInformation>();
			this.finishedBundles = new List<string>();

			this.locker = new object();

			if (LayerManager.Instance.Console != null)
			{
				this.logger = LayerManager.Instance.Console.GetLogger("AssetBundleRegistry");
			}
		}

		/// <summary>
		/// Returns the process of all currently loading threads.
		/// </summary>
		/// <value><para>Percent as 0f to 1f.</para></value>
		public float Process
		{
			get
			{
				int downloads = this.runningDownloads + this.finishedDownloads;
				if (downloads == 0)
				{
					return 1f;
				}
				return (this.finishedDownloads + this.accumulatedProgress) / downloads;
			}
		}

		/// <summary>
		/// Get the reference to a completely downloaded asset bundle.
		/// </summary>
		/// <param name="name">The identifier of the asset bundle</param>
		/// <returns>The asset bundle.</returns>
		public AssetBundle Get(string name)
		{
			if (this.Contains(name))
			{
				return this.assetBundles[name];
			}
			else
			{
				if (this.logger != null)
				{
					this.logger.Warn("does not contain a bundle named \"" + name + "\"");
				}
				return null;
			}
		}

		/// <summary>
		/// Is an asset bundle loaded?
		/// </summary>
		/// <param name="name">The identifier of the asset bundle.</param>
		/// <returns>True if the asset bundle is loaded.</returns>
		public bool Loaded(string name)
		{
			return this.Contains(name);
		}

		/// <summary>
		/// Is an asset bundle loaded?
		/// </summary>
		/// <param name="name">The identifier of the asset bundle.</param>
		/// <returns>True if the asset bundle is loaded.</returns>
		public bool Contains(string name)
		{
			return this.assetBundles.ContainsKey(name);
		}

		/// <summary>
		/// Adds an asset bundle.
		/// </summary>
		/// <param name="name">The identifier of the asset bundle.</param>
		/// <param name="bundle">The asset bundle.</param>
		/// <returns>True if the bundle has been added.</returns>
		public bool Register(string name, AssetBundle bundle)
		{
			if (bundle != null && !this.Contains(name))
			{
				Dictionary<string, AssetBundle> tempBundles = new Dictionary<string, AssetBundle>(this.assetBundles.Count + 1);
				foreach (KeyValuePair<string, AssetBundle> item in this.assetBundles)
				{
					tempBundles.Add(item.Key, item.Value);
				}
				tempBundles.Add(name, bundle);

				this.assetBundles = tempBundles;
				
				AssetNotifier.Instance.AssetBundleLoaded(name);
				
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// Unregisters and unloads the bundle.
		/// </summary>
		/// <param name="name">The bundles identifier.</param>
		/// <returns>Whether the unregistration has been successful.</returns>
		public bool Unregister(string name)
		{
			if (this.Contains(name))
			{
				this.assetBundles[name].Unload(false);
				this.assetBundles.Remove(name);
				
				return true;
			}
			return false;
		}

		/// <summary>
		/// Load an asset bundle asynchronously.
		/// </summary>
		/// <param name="name"><para>The identifier of the asset bundle.</para><para>Needs to be unique!</para></param>
		/// <param name="url">The url to the asset bundle.</param>
		public void Load(string name, string url)
		{
			this.Load(name, url, null);
		}

		/// <summary>
		/// <para>Load an asset bundle asynchronously.</para>
		/// <para>If the identifier is already present the delegate will be caled at once.</para>
		/// </summary>
		/// <param name="name"><para>The identifier of the asset bundle.</para><para>Needs to be unique!</para></param>
		/// <param name="url">The url to the asset bundle.</param>
		/// <param name="loadCompletedDelegate">This method will be called when the asset bundle has been completed.</param>
		public void Load(string name, string url, AssetBundleLoadCompleted loadCompletedDelegate)
		{
			if (!this.Loading(name))
			{
				if (!this.Contains(name))
				{
					WWW www = new WWW(url);
					lock (this.locker)
					{
						AssetBundleInformation info;
						info.Name = name;
						info.WWW = www;
						info.OnLoadCompleted = loadCompletedDelegate;
						this.loadingBundles.Add(name, info);

						this.runningDownloads++;

						this.usingLoaded = true;
					}
				}
				else
				{
					if (loadCompletedDelegate != null)
					{
						try
						{
							loadCompletedDelegate(name);
						}
						catch (System.Exception e)
						{
							if (this.logger != null)
							{
								this.logger.Error("OnLoadCompleted delegate for \"" + name + "\" crashed.\n" + e.ToString());
							}
						}
					}
				}
			}
			else
			{
				if (this.logger != null)
				{
					this.logger.Warn("Bundle \"" + name + "\" is already loading...");
				}
			}
		}

		/// <summary>
		/// Returns whether a bundle is currently downloading.
		/// </summary>
		/// <param name="name">The identifier of the asset bundle.</param>
		/// <returns>True if the bundle is currently downloading.</returns>
		public bool Loading(string name)
		{
			lock (this.locker)
			{
				return this.loadingBundles.ContainsKey(name);
			}
		}

		/// <summary>
		/// Returns a collection with all loaded bundles.
		/// </summary>
		/// <returns>Returns a collection with all loaded bundles.</returns>
		public ICollection<AssetBundle> GetBundles()
		{
			return this.assetBundles.Values;
		}
		
		/// <summary>
		/// Unregisters and unloads all AssetBundles.
		/// </summary>
		public void UnLoadAll()
		{
			foreach (string key in this.assetBundles.Keys)
			{
				this.Unregister(key);
			}
		}

		/// <summary>
		/// Called every frame to check the status of the asset bundle download.
		/// </summary>
		public void Update()
		{
			if (this.usingLoaded)
			{
				lock (this.locker)
				{
					this.accumulatedProgress = 0f;

					// Check for each thread if downloading has finished.
					foreach (AssetBundleInformation item in this.loadingBundles.Values)
					{
						if (item.WWW.isDone)
						{
							// Downloading has finished.
							this.finishedBundles.Add(item.Name);

							this.finishedDownloads++;
							this.runningDownloads--;

							// Register the bundle.
							if (item.WWW.assetBundle != null)
							{
								this.Register(item.Name, item.WWW.assetBundle);
							}
							else
							{
								if (this.logger != null)
								{
									this.logger.Error("\"" + item.Name + "\" does not contain an asset bundle.");
								}
							}

							// Call a load completed handler if set.
							if (item.OnLoadCompleted != null)
							{
								try
								{
									item.OnLoadCompleted(item.Name);
								}
								catch (System.Exception e)
								{
									if (this.logger != null)
									{
										this.logger.Error("OnLoadCompleted delegate for \"" + item.Name + "\" crashed.\n" + e.ToString());
									}
								}
							}
						}
						else
						{
							this.accumulatedProgress += item.WWW.progress;
						}
					}

					// remove the finished bundles
					foreach (string key in this.finishedBundles)
					{
						this.loadingBundles.Remove(key);
					}
					this.finishedBundles.Clear();

					// reset values if no bundle is loading.
					if (this.loadingBundles.Count == 0)
					{
						this.finishedDownloads = 0;
						this.runningDownloads = 0;
						this.accumulatedProgress = 0f;
						this.usingLoaded = false;
					}
				}
			}
		}

		/// <summary>
		/// Used as storage of information about asset bundle downloads.
		/// </summary>
		private struct AssetBundleInformation
		{
			public string Name;
			public WWW WWW;
			public AssetBundleLoadCompleted OnLoadCompleted;
		}
	}
}