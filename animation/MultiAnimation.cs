using UnityEngine;
using System.Collections.Generic;

namespace GUILib
{
	/// <summary>
	/// Animation container to bundle multiple animations.
	/// </summary>
	/// <author>Manuel Fasse (ven)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 267 $, $Date: 2011-06-28 21:00:01 +0200 (Di, 28 Jun 2011) $</version>
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
				OverlayManager.Instance.LogError("[Animation] Can't add animation to itself.");
				return;
			}
			this.animations.Add(animation);
		}
	}
}