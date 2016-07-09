using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// A class that supports generic behavior of a Plot.
	/// </summary>
	[Serializable]
	public partial class Plot: PaintThread {
		/// <summary>
		/// The possible types of plots.
		/// </summary>
		public enum Type {
			/// <summary>
			/// Denotes a two dimensional plot.
			/// </summary>
			Plot2D,
			/// <summary>
			/// Denotes a three dimesional plot with grid lines.
			/// </summary>
			Plot3DGrid,
			/// <summary>
			/// Denotes a three dimensional plot with surface shading.
			/// </summary>
			Plot3DSurface
		}

		/// <summary>
		/// A class that derives from System.Windows.Forms.MouseEventArgs and also contains the state of the modifier keys.
		/// </summary>
		public class MouseEventArgs: System.Windows.Forms.MouseEventArgs {

			/// <summary>
			/// The modifier keys during the mouse event.
			/// </summary>
			public Keys ModifierKeys;

			/// <summary>
			/// A constructor that takes a System.Windows.Forms.MouseEventArgs to initialize the class.
			/// </summary>
			public MouseEventArgs(System.Windows.Forms.MouseEventArgs e)
				: base(e.Button, e.Clicks, e.X, e.Y, e.Delta) {
				ModifierKeys = Keys.None;
			}

			/// <summary>
			/// Initializes the class with the given MouseEventArgs and modifier keys.
			/// </summary>
			public MouseEventArgs(System.Windows.Forms.MouseEventArgs e, Keys modifiers)
				: base(e.Button, e.Clicks, e.X, e.Y, e.Delta) {
				ModifierKeys = modifiers;
			}

		}

		/// <summary>
		/// The contructor for a Plot object. Do not use this constructor, use the static method Plot.New instead.
		/// Plot.New returns the right Plot type according to the Model.type property.
		/// </summary>
		public Plot(PlotModel Model): base(Model) {
			Bounds.X = 0; Bounds.Y = 0; Bounds.Width = -1; Bounds.Height = -1;
		}
		[NonSerialized]
		private Plot paintThread = null;
		/// <summary>
		/// The thread that paints the plot's items.
		/// </summary>
		public Plot PaintThread {
			get {
				if (paintThread == null) paintThread = this.Clone();
				return paintThread;
			}
		}
		/// <summary>
		/// Stops the Painter thread.
		/// </summary>
		public void Dispose() {
			DrawStop = true;
		}
		/// <summary>
		/// Returns a new Plot object for the supplied PlotModel, according to Model.type.
		/// </summary>
		/// <param name="Model">The PlotModel</param>
		public static Plot New(PlotModel Model) {
			switch (Model.PlotType) {
				case Plot.Type.Plot2D: return new Plot2D(Model);
				case Plot.Type.Plot3DSurface: return new Plot3D(Model);
				default: return new Plot(Model);
			}
		}
		/// <summary>
		/// Copies from another Plot
		/// </summary>
		public void CopyFrom(Plot p) {
			base.CopyFrom(p);
			Bounds = p.Bounds;
			Model = p.Model; 
		}
		/// <summary>
		/// Returns a clone of the Plot.
		/// </summary>
		public new virtual Plot Clone() {
			Plot p = new Plot(Model);
			p.CopyFrom(this);
			return p;
		}
		/// <summary>
		/// Returns the <see cref="PlotType"/> of the Plot
		/// </summary>
		public virtual Plot.Type PlotType {
			get { return Plot.Type.Plot2D; }
		}
		/// <summary>
		/// Crops the display area of the Plot according to norms and bounds. If the max/min value of the norms lies outside the
		/// bounds, the bounds are adjusted accordingly.
		/// </summary>
		/// <returns>Returns true if the Plot was cropped, i.e. the max/min value of the norms was outside the
		/// Plot.Bounds.</returns>
		public virtual bool Crop(Norms norms, Rectangle bounds) {
			const int B = GraphicsBase.Border; // The width of the white-space border around the plotting area
			/* 
			int dx, dy, dw, dh;
			
			dx = (int)(Math.Max(0, Graphics2D.D - norms.minT.X) + 0.5F);
			dy = (int)(Math.Max(0, Graphics2D.D - norms.minT.Y) + 0.5F);
			dw = (int)(Math.Min(0, bounds.Width - Graphics2D.D - norms.maxT.X - dx) + 0.5F);
			dh = (int)(Math.Min(0, bounds.Height - Graphics2D.D - norms.maxT.Y - dy) + 0.5F);

			X += dx; Y += dy; Width += dw; Height += dh;
			Width = Math.Max(Width, 1); Height = Math.Max(Height, 1);

			return (dx > 0 || dy > 0 || dw > 0 || dh > 0) && Width > 1 && Height > 1;
			*/
			Rectangle innerBounds, d = new Rectangle();
			int dw, dh;

			// set innerBounds to the rectangle that is by Graphics2D.Border smaller than bounds.
			if (Model.Border) {
				innerBounds = new Rectangle(bounds.X + B, bounds.Y + B, bounds.Width - 2*B, bounds.Height - 2*B);
			} else innerBounds = bounds;

			d.X = (int)(innerBounds.X - norms.minT.X + 0.5F);
			d.Y = (int)(innerBounds.Y - norms.minT.Y + 0.5F);
			d.X = Math.Max(0, d.X); d.Y = Math.Max(0, d.Y);
			d.Width = (int)(innerBounds.X + innerBounds.Width - norms.maxT.X  + 0.5F) - d.X;
			d.Height = (int)(innerBounds.Y + innerBounds.Height - norms.maxT.Y + 0.5F) - d.Y;
			d.Width = Math.Min(0, d.Width); d.Height = Math.Min(0, d.Height);
 
			Bounds.X += d.X; Bounds.Y += d.Y; Bounds.Width += d.Width; Bounds.Height += d.Height;

			if (Model.Border) { // check if Bounds lie outside innerBounds
				// crop Bounds to innerBounds
				if (Bounds.X < innerBounds.X) Bounds.X = innerBounds.X;
				if (Bounds.Y < innerBounds.Y) Bounds.Y = innerBounds.Y;
				dw = Bounds.X + Bounds.Width - innerBounds.X - innerBounds.Width;
				dh = Bounds.Y + Bounds.Height - innerBounds.Y - innerBounds.Height;
				if (dw > 0) Bounds.Width -= dw;
				if (dh > 0) Bounds.Height += dh;
			}
			Bounds.Width = Math.Max(1, Bounds.Width); Bounds.Height = Math.Max(1, Bounds.Height);
			
			return (d.X != 0 || d.Y != 0 || d.Width != 0 || d.Height != 0) && Bounds.Width > 1 && Bounds.Height > 1;  
		}
		/// <summary>
		/// Draws the Plot
		/// </summary>
		/// <param name="g">The Graphics object to paint to.</param>
		/// <param name="bounds">The bounds of the Plot</param>
		public virtual void Draw(Graphics g, Rectangle bounds) { }
		/// <summary>
		/// Sets the plot range
		/// </summary>
		public virtual void SetRange(double x0, double x1, double y0, double y1, double z0, double z1) {
			Model.SetRange(x0, x1, y0, y1, z0, z1);
		}

	}
}
