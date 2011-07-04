using UnityEngine;

namespace GUILib
{
	/// <summary>
	/// A box with scrollbars.
	/// </summary>
	/// <author>Henning Vreyborg (marshen)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 266 $, $Date: 2011-06-28 17:29:50 +0200 (Di, 28 Jun 2011) $</version>
	public class ScrollBox : Box
	{
		private Types.ScrollBar scrollBar;
		private Slider horizontalSlider;
		private Slider verticalSlider;
		private Rect boxRectangle;
		private Rect groupRectangle;

		public ScrollBox()
			: base()
		{
			this.scrollBar = Types.ScrollBar.BOTH;
			this.horizontalSlider = new Slider(Types.Alignment.HORIZONTAL);
			this.verticalSlider = new Slider(Types.Alignment.VERTICAL);
		}

		public ScrollBox(ScrollBox source)
			: base(source)
		{
			this.scrollBar = source.scrollBar;
			this.horizontalSlider = new Slider(source.horizontalSlider);
			this.verticalSlider = new Slider(source.verticalSlider);
		}

		public override void DrawGUI()
		{
			GUI.enabled = this.IsEnabled;

			this.refreshScrollBars();

			// TODO: this should happen on property change
			this.verticalSlider.Left = this.Width - this.verticalSlider.Width + this.Left;
			this.horizontalSlider.Top = this.Height - this.horizontalSlider.Height + this.Top;


			GUI.BeginGroup(this.groupRectangle, this.CurrentStyle);
			{
				GUI.BeginGroup(this.boxRectangle);
				{
					foreach (IElement elem in this.children)
					{
						elem.DrawGUI();
					}
				}
				GUI.EndGroup();
			}
			GUI.EndGroup();

			if (this.verticalSlider.Visible)
			{
				this.verticalSlider.DrawGUI();
			}
			if (this.horizontalSlider.Visible)
			{
				this.horizontalSlider.DrawGUI();
			}
		}

		public override System.Collections.ObjectModel.ReadOnlyCollection<IElement> Children
		{
			get
			{
				this.children.Add(this.verticalSlider);
				this.children.Add(this.horizontalSlider);
				System.Collections.ObjectModel.ReadOnlyCollection<IElement> chidren = this.children.AsReadOnly();
				this.children.Remove(this.verticalSlider);
				this.children.Remove(this.horizontalSlider);
				return chidren;
			}
		}

		public Types.ScrollBar ScrollBar
		{
			get { return this.scrollBar; }
			set { this.scrollBar = value; }
		}
		
		/// <summary>
		/// ClassStyle of the embedded horizontal scrollbar.
		/// </summary>
		public Style HorizontalScrollBarStyle
		{
			get { return this.horizontalSlider.ClassStyle; }
			set { this.horizontalSlider.ClassStyle = value; }
		}
		
		/// <summary>
		/// ClassStyle of the embedded vertical scrollbar.
		/// </summary>
		public Style VerticalScrollBarStyle
		{
			get { return this.verticalSlider.ClassStyle; }
			set { this.verticalSlider.ClassStyle = value; }
		}

		public float VerticalScrollPosition
		{
			get
			{
				if (this.verticalSlider.Visible)
				{
					return this.verticalSlider.Value;
				}
				return 0f;
			}
			set
			{
				if (value >= 0f && value <= this.VerticalScrollMaximum)
				{
					this.verticalSlider.Value = value;
				}
			}
		}
		
		public float VerticalScrollMaximum
		{
			get
			{
				if (this.verticalSlider.Visible)
				{
					return this.verticalSlider.Maximum;
				}
				return 0f;
			}
		}
		
		public float HorizontalScrollPosition
		{
			get
			{
				if (this.horizontalSlider.Visible)
				{
					return this.horizontalSlider.Value;
				}
				return 0f;
			}
			set
			{
				if (value >= 0f && value <= this.HorizontalScrollMaximum)
				{
					this.horizontalSlider.Value = value;
				}
			}
		}
		
		public float HorizontalScrollMaximum
		{
			get {
				if (this.horizontalSlider.Visible)
				{
					return this.horizontalSlider.Maximum;
				}
				return 0f;
			}
		}
		
		private void refreshScrollBars()
		{
			Vector2 size = this.CalcSize();

			this.boxRectangle = new Rect(0, 0, Mathf.Max(size.x, this.Width), Mathf.Max(size.y, this.Height));
			this.groupRectangle = this.Rect;

			//Debug.Log(size + " - " + this.groupRectangle);

			if ((this.scrollBar == Types.ScrollBar.BOTH || this.scrollBar == Types.ScrollBar.VERTICAL) && size.y > this.groupRectangle.height)
			{
				// vertical scrollbar needed
				this.groupRectangle.width -= this.verticalSlider.Width;
				this.verticalSlider.Visible = true;
			}
			else
			{
				this.verticalSlider.Visible = false;
				this.verticalSlider.Value = 0f;
			}

			if ((this.scrollBar == Types.ScrollBar.BOTH || this.scrollBar == Types.ScrollBar.HORIZONTAL) && size.x > this.groupRectangle.width)
			{
				// horizontal scrollbar needed
				this.groupRectangle.height -= this.horizontalSlider.Height;
				this.horizontalSlider.Visible = true;

				// after displaying the horizontal slider a vertical scrollbar may be needed...
				if (!this.verticalSlider.Visible && (this.scrollBar == Types.ScrollBar.BOTH || this.scrollBar == Types.ScrollBar.VERTICAL) && size.y > this.groupRectangle.height)
				{
					// vertical scrollbar needed
					this.groupRectangle.width -= this.verticalSlider.Width;
					this.verticalSlider.Visible = true;
				}
			}
			else
			{
				this.horizontalSlider.Visible = false;
				this.horizontalSlider.Value = 0f;
			}
			
			// calculate width/height of the slider
			if (this.verticalSlider.Visible)
			{
				if (this.horizontalSlider.Visible)
				{
					// both sliders shown
					this.verticalSlider.Height = this.Height - this.horizontalSlider.Height;
					this.horizontalSlider.Width = this.Width - this.verticalSlider.Width;

					this.verticalSlider.Top = this.Top;
					this.horizontalSlider.Left = this.Left;
				}
				else
				{
					// only verticalSlider shown
					this.verticalSlider.Height = this.Height;

					this.verticalSlider.Top = this.Top;
				}
			}
			else
			{
				if (this.horizontalSlider.Visible)
				{
					// only horizontalSlider shown
					this.horizontalSlider.Width = this.Width;

					this.horizontalSlider.Left = this.Left;
				}
			}

			this.refreshSlider();
		}

		private void refreshSlider()
		{
			float v;
			if (this.horizontalSlider.Visible)
			{
				v = this.boxRectangle.width - this.groupRectangle.width;
				if (this.horizontalSlider.Maximum != v)
				{
					this.horizontalSlider.Maximum = v;
				}
				this.boxRectangle = new Rect(-this.horizontalSlider.Value, 0, this.boxRectangle.width, this.boxRectangle.height);
			}
			if (this.verticalSlider.Visible)
			{
				v = this.boxRectangle.height - this.groupRectangle.height;
				if (this.verticalSlider.Maximum != v)
				{
					this.verticalSlider.Maximum = v;
				}
				this.boxRectangle = new Rect(this.boxRectangle.xMin, -this.verticalSlider.Value, this.boxRectangle.width, this.boxRectangle.height);
			}
		}
	}
}