using UnityEngine;

namespace GUILib
{
	/// <summary>
	/// Basic resize animation.
	/// </summary>
	/// <author>Manuel Fasse (ven)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 267 $, $Date: 2011-06-28 21:00:01 +0200 (Tue, 28 Jun 2011) $</version>
	public class ResizeAnimation : AAnimation
	{
		private IElement element;
		private float resizeWidth;
		private float resizeHeight;
		
		/// <summary>
		/// Create a resize animation.
		/// </summary>
		/// <param name="element">The element to resize.</param>
		/// <param name="resizeWidth">Amount by which the width should be changed.</param>
		/// <param name="resizeHeight">Amount by which the height should be changed.</param>
		/// <param name="runtime">Runtime of the animation in seconds.</param>
		/// <param name="delay">Delay after which the animation should start in seconds.</param>
		public ResizeAnimation(IElement element, float resizeWidth, float resizeHeight, float runtime, float delay) : base(runtime, delay)
		{
			this.element = element;
			this.resizeWidth = resizeWidth;
			this.resizeHeight = resizeHeight;
		}
		
		private float partValue(float length)
		{
			float partTime = deltaTime / this.runtime;
			return length * partTime;
		}
		
		protected override void step ()
		{
			this.element.Width += this.partValue(this.resizeWidth);
			this.element.Height += this.partValue(this.resizeHeight);
		}
	}
}