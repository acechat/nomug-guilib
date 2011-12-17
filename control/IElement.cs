using UnityEngine;
using System.Collections;

namespace GUILib {
	/// <summary>
	/// This interface is implemented by all GUILib elements.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 224 $, $Date: 2011-05-22 19:39:31 +0200 (So, 22 Mai 2011) $</version>
	public interface IElement {
		/// <summary>
		/// This method implements how an element should be drawn.
		/// </summary>
		void DrawGUI();

		/// <summary>
		/// This method should return the size dynamically based on its content.
		/// </summary>
		/// <returns>A vector with the needed with and height.</returns>
		Vector2 CalcSize();

		/// <summary>
		/// This method should return the size dynamically based on its content.
		/// </summary>
		/// <param name="content">The content that the element should use.</param>
		/// <returns>A vector with the needed with and height.</returns>
		Vector2 CalcSize(GUIContent content);

		/// <summary>
		/// This method should return the height dynamically based on its content.
		/// </summary>
		/// <param name="width">The width that the element should have.</param>
		/// <returns>The height needed to display the element.</returns>
		float CalcHeight(float width);

		/// <summary>
		/// This method should return the height dynamically based on its content.
		/// </summary>
		/// <param name="width">The width that the element should have.</param>
		/// <param name="content">The content that the element should use.</param>
		/// <returns>The height needed to display the element.</returns>
		float CalcHeight(GUIContent content, float width);


		/// <summary>
		/// Returns the parent of an element.
		/// </summary>
		IElementCollection Parent {
			get;
			set;
		}

		/// <summary>
		/// Returns whether the element and its parents are enabled.
		/// </summary>
		bool IsEnabled {
			get;
		}

		/// <summary>
		/// Returns whether the element is enabled.
		/// </summary>
		bool Enabled
		{
			get;
			set;
		}

		/// <summary>
		/// Returns whether the element is actually drawn.
		/// </summary>
		bool IsVisible {
			get;
		}

		/// <summary>
		/// Returns whether the element is visible.
		/// </summary>
		bool Visible
		{
			get;
			set;
		}
		
		/// <summary>
		/// Gets or sets a value indicating whether this instance is hovered.
		/// </summary>
		bool IsHovered {
			get;
		}


		/// <summary>
		/// Returns the left position of an element in relation to the screen.
		/// </summary>
		float AbsoluteLeft {
			get;
		}

		/// <summary>
		/// Returns the top position of an element in relation the the screen.
		/// </summary>
		float AbsoluteTop {
			get;
		}

		/// <summary>
		/// The left position of the element in relation to its parent.
		/// </summary>
		float Left {
			get;
			set;
		}

		/// <summary>
		/// The top position of the element in relation to its parent.
		/// </summary>
		float Top {
			get;
			set;
		}
		
		/// <summary>
		/// The width of an element.
		/// </summary>
		float Width {
			get;
			set;
		}

		/// <summary>
		/// The height of an element.
		/// </summary>
		float Height {
			get;
			set;
		}


		/// <summary>
		/// Returns the rectangle of an element.
		/// </summary>
		UnityEngine.Rect Rect {
			get;
		}

		/// <summary>
		/// A unique identifier (this is used as control name).
		/// </summary>
		string GUID {
			get;
		}

		/// <summary>
		/// Returns the name of an IElement.
		/// </summary>
		string Name {
			get;
			set;
		}

		/// <summary>
		/// The text that is displayed on this element.
		/// </summary>
		string Text {
			get;
			set;
		}
		
		Style DefaultStyle {
			get; 
			set;
		}
		
		Style ClassStyle {
			get;
			set;
		}
		
		Style ElementStyle {
			get;
			set;
		}

		/// <summary>
		/// The tooltip that this element uses.
		/// </summary>
		ToolTip ToolTip
		{
			get;
			set;
		}
	}
}