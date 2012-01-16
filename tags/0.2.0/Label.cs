using UnityEngine;
using System.Collections;
using System;

namespace GUILib
{
	/// <summary>
	/// Draws text on the screen. Labels don't catch mouse clicks, but catch mouse events and are always rendered in normal style.
	/// </summary>
	/// <author>All</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 214 $, $Date: 2011-05-16 00:46:11 +0200 (Mon, 16 May 2011) $</version>
	public class Label : AElement, ICloneable
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Label() : base()
		{
		}

		/// <summary>
		/// Creates a new Label with the properties of a given label object.
		/// </summary>
		/// <param name="source">The source from where the properties should be copied.</param>
		public Label(Label source) : base(source)
		{

		}

		/// <summary>
		/// Draw a label.
		/// </summary>
		public override void DrawGUI()
		{
			base.DrawGUI();
			GUI.enabled = this.IsEnabled;
			GUI.Label(this.Rect, this.Text, this.CurrentStyle);
		}

		/// <summary>
		/// Label has no states. Returns Normal Style. 
		/// </summary>
		protected override Types.StyleStateType CurrentState
		{
			get { return Types.StyleStateType.Normal; }
		}

		#region ICloneable Member

		/// <summary>
		/// Creates a duplicate of an existing Label.
		/// </summary>
		/// <returns>Returns the cloned Label.</returns>
		public object Clone()
		{
			return new Label(this);
		}

		#endregion
	}
}