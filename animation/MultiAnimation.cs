using UnityEngine;
using System.Collections.Generic;

namespace GUILib
{
	/// <summary>
	/// Animation container to bundle multiple animations.
	/// </summary>
	/// <author>Manuel Fasse (ven)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 293 $, $Date: 2012-01-16 19:18:36 +0100 (Mon, 16 Jan 2012) $</version>
	public class MultiAnimation : AAnimation
	{
		private List<AAnimation> animations;
		
		/// <summary>
		/// Create a an animation bundle. 
		/// </summary>
		/// <param name="delay">Delay after which the animation should start in seconds.</param>
		public MultiAnimation(float delay) : base(0.0f, delay)
		{
			this.animations = new List<AAnimation>();
		}
		
		/// <summary>
		/// Whether the animation has finished.
		/// </summary>
		public override bool Done
		{
			get { return this.currentTime > 0.0f && this.animations.Count == 0; }
		}
		
		/// <summary>
		/// Whether the animation is currently running.
		/// </summary>
		public override bool Running
		{
			get { return this.currentTime > 0.0f && this.animations.Count > 0; }
		}
		
		protected override void step ()
		{
			int i = 0;
			while(i < animations.Count)
			{
				if(!animations[i].Done)
				{
					animations[i].Step();
					i++;
				}
				else
				{
					animations.RemoveAt(i);
				}
			}
		}
		
		/// <summary>
		/// Add an animation to the animation bundle. 
		/// </summary>
		/// <param name="animation">The animation to add.</param>
		public void Add(AAnimation animation)
		{
			if(animation == this)
			{
				LayerManager.Instance.LogError("[Animation] Can't add animation to itself.");
				return;
			}
			this.animations.Add(animation);
		}
	}
}