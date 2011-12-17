using UnityEngine;
using System.Collections;
using System;

namespace GUILib
{
	/// <summary>
	/// A texture
	/// </summary>
	/// <author>Andreas Erhardt (erhardt)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 226 $, $Date: 2011-05-26 14:27:34 +0200 (Do, 26 Mai 2011) $</version>
	public class Image : AElement, ICloneable
	{
		private ScaleMode scaleMode;
		private bool alphaBlend;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Image()
			: base()
		{
			this.scaleMode = ScaleMode.StretchToFill;
			this.alphaBlend = true;
		}

		/// <summary>
		/// Creates a new Texture with the properties of a given Texture object.
		/// </summary>
		/// <param name="source">The source from where the properties should be copied.</param>
		public Image(Image source)
			: base(source)
		{
			this.scaleMode = source.scaleMode;
			this.alphaBlend = source.alphaBlend;
		}

		/// <summary>
		/// Loading the Texture
		/// </summary>
		/// <param name="path">Path to the texture</param>
		private void LoadTexture(string path)
		{
			if (path.Length < 255)
			{
				this.ElementStyle.BackgroundPath = path;
			}
			else
			{
				OverlayManager.Instance.Log("The string is to long (max. 255 chars!)");
			}
		}

		/// <summary>
		/// Draws the Image.
		/// </summary>
		public override void DrawGUI()
		{
			base.DrawGUI();
			if (this.ElementStyle.Normal.Background != null)
			{
				GUI.enabled = this.IsEnabled;
				GUI.DrawTexture(this.Rect, this.ElementStyle.Normal.Background, this.scaleMode, this.alphaBlend, 0.0F);
			}
		}

		/// <summary>
		/// <para>Gets or sets the path to the image.</para>
		/// <para>Note: Short version for <see cref="Style.BackgroundPath"></para>
		/// </summary>
		public string ImagePath
		{
			get { return this.ElementStyle.BackgroundPath; }
			set { this.ElementStyle.BackgroundPath = value; }
		}

		/// <summary>
		/// Gets or sets the scale mode.
		/// </summary>
		public ScaleMode ScaleMode
		{
			get { return this.scaleMode; }
			set { this.scaleMode = value; }
		}

		/// <summary>
		/// Gets or sets alpha blending value.
		/// </summary>
		public bool AlphaBlend
		{
			get { return this.alphaBlend; }
			set { this.alphaBlend = value; }
		}

		/// <summary>
		/// <para>Gets or sets the texture.</para>
		/// <para>Note: Short version for <see cref="Style.Background"></para>
		/// </summary>
		public Texture2D Texture
		{
			get { return this.ElementStyle.Background; }
			set { this.ElementStyle.Background = value; }
		}


		#region ICloneable Member

		/// <summary>
		/// Creates a duplicate of an existing Image.
		/// </summary>
		/// <returns>Returns the cloned Image.</returns>
		public object Clone()
		{
			return new Image(this);
		}

		#endregion
	}
}
