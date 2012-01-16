using UnityEngine;

namespace GUILib
{
	/// <summary>
	/// Basic move animation.
	/// </summary>
	/// <author>Manuel Fasse (ven)</author>
	/// <author>$LastChangedBy: ven $</author>
	/// <version>$Rev: 267 $, $Date: 2011-06-28 21:00:01 +0200 (Tue, 28 Jun 2011) $</version>
	public class MoveAnimation : AAnimation
	{
		private IElement element;
		private float moveLeft;
		private float moveTop;
		
		/// <summary>
		/// Create a move animation.
		/// </summary>
		/// <param name="element">The element to move.</param>
		/// <param name="moveLeft">Amount by which the left position should be changed.</param>
		/// <param name="moveTop">Amount by which the top position should be changed.</param>
		/// <param name="runtime">Runtime of the animation in seconds.</param>
		/// <param name="delay">Delay after which the animation should start in seconds.</param>
		public MoveAnimation(IElement element, float moveLeft, float moveTop, float runtime, float delay) : base(runtime, delay)
		{
			this.element = element;
			this.moveLeft = moveLeft;
			this.moveTop = moveTop;
		}
		
		private float partValue(float length)
		{
			float partTime = deltaTime / this.runtime;
			return length * partTime;
		}
		
		protected override void step ()
		{
			this.element.Left += this.partValue(this.moveLeft);
			this.element.Top += this.partValue(this.moveTop);
		}
	}
}