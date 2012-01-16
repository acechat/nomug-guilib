using UnityEngine;

namespace GUILib
{
	/// <summary>
	/// Abstract class for all animations.
	/// </summary>
	/// <author>Manuel Fasse (ven)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 267 $, $Date: 2011-06-28 21:00:01 +0200 (Tue, 28 Jun 2011) $</version>
	public abstract class AAnimation
	{
		protected float currentTime;
		protected float startTime;
		protected float runtime;
		protected float timeStamp;
		protected float deltaTime;
		
		private bool firstStep;
		
		/// <summary>
		/// Base constructor.
		/// </summary>
		/// <param name="runtime">Runtime of the animation in seconds.</param>
		/// <param name="delay">Delay after which the animation should start in seconds.</param>
		public AAnimation(float runtime, float delay)
		{
			this.currentTime = 0.0f - delay;
			this.runtime = runtime;
			firstStep = true;
		}
		
		/// <summary>
		/// Animation progress represented as a value between 0 and 1.
		/// </summary>
		public float Progress
		{
			get
			{		
				float progress;
				if(this.currentTime >= this.runtime)
				{
					progress = 1.0f;
				}
				else if(this.currentTime <= 0.0f)
				{
					progress = 0.0f;
				}
				else
				{
					progress = this.currentTime / this.runtime;
				}
				return progress;
			}
		}
		
		/// <summary>
		/// Whether the animation has finished.
		/// </summary>
		public virtual bool Done
		{
			get { return this.currentTime >= this.runtime; }
		}
		
		/// <summary>
		/// Whether the animation is currently running.
		/// </summary>
		public virtual bool Running
		{
			get { return 0.0f < this.currentTime && this.currentTime < this.runtime; }
		}
		
		/// <summary>
		/// Progress the animation by one step.
		/// </summary>
		public void Step()
		{
			if(firstStep)
			{
				this.timeStamp = Time.time;
				this.deltaTime = 0.0f;
				firstStep = false;
			}
			else
			{
				float newTimeStamp = Time.time;
				this.deltaTime = newTimeStamp - this.timeStamp;
				this.timeStamp = newTimeStamp;
				this.currentTime += this.deltaTime;
			}
			if(this.Running)
			{
				this.step();
			}
		}
		
		/// <summary>
		/// Override step method with your own calculations.
		/// Will be called each frame.
		/// </summary>
		protected abstract void step();
	}
}