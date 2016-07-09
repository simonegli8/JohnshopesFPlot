using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// Plots 3D plots
	/// </summary>
	[Serializable]
	public class Plot3D: Plot  {
		[NonSerialized]
		int sx, sy, sw, sh, fd, fld;
		[NonSerialized]
		bool zoomIn;
		/// <summary>
		/// Constructs a new Plot2D.
		/// </summary>
		/// <param name="Model">The PlotModel of the Plot2D</param>
		public Plot3D(PlotModel Model): base(Model) {
			sx = sy = -1; sw = sh = 0;
			fd = fld = 1;
			zoomIn = false;
		}
		/// <summary>
		/// Copies from another Plot2D object
		/// </summary>
		public void CopyFrom(Plot3D p) {
			base.CopyFrom(p);
			fd = p.fd; fld = p.fld;
			Model = p.Model;
		}
		/// <summary>
		/// Clones the Plot2D object
		/// </summary>
		public override Plot Clone() {
			Plot3D p = new Plot3D(Model);
			p.CopyFrom(this);
			return p;
		}

		/// <summary>
		/// Moves the displayed range in the plotting area by the specified amount of pixels.
		/// </summary>
		public void MovePlot(int dx, int dy) {
			double pw = (Model.x1 - Model.x0)/(Bounds.Width-1);
			double ph = (Model.y1 - Model.y0)/(Bounds.Height-1);
			if (Model.x.fix) dx = 0;
			if (Model.y.fix) dy = 0;
			Model.SetRange(Model.x0 - pw*dx, Model.x1 - pw*dx, Model.y0 + ph*dy, Model.y1 + ph*dy, Bounds);
		}
		/// <summary>
		/// Crops the Ploting area according to the extensions of the plot described by norms
		/// </summary>
		/// <param name="norms">The extensions of the plot</param>
		/// <param name="bounds">The bounds of the painting area</param>
		/// <returns>Returns true, if the plotting area was cropped</returns>
		public override bool Crop(Norms norms, Rectangle bounds) {
			bool res = base.Crop(norms, bounds);
			Model.View.SetBounds(Model, Bounds);
			return res;
		}
		/// <summary>
		/// Increases the size of the plot area until its bounds fit into bounds.
		/// </summary>
		public void Grow(Norms norms, Rectangle bounds) { }

		/// <summary>
		/// Calculates the bounds of the plotting area inside the control.
		/// </summary>
		/// <param name="g">A Graphics object.</param>
		/// <param name="bounds">The bounds of the entire control.</param>
		void Measure(Graphics g, Rectangle bounds) {
			SizeF size;
			PointF[] v = new PointF[3];
			Norms norms;
			double x0, x1, y0, y1, z0, z1;

			size = g.MeasureString("0.5", Model.ScaleFont);
			fd = Math.Max(1, (int)(size.Height + 0.5F));
			size = g.MeasureString("0.5", Model.LegendFont);
			fld = Math.Max(1, (int)(size.Height + 0.5F));

			Bounds = bounds;
			x0 = Model.x0; x1 = Model.x1; y0 = Model.y0; y1 = Model.y1; z0 = Model.z0; z1 = Model.z1;
			Model.x.scaleOutside = Model.y.scaleOutside =	Model.z.scaleOutside = true;
			Model.x.rasterOutside = Model.y.rasterOutside = Model.z.rasterOutside = false;
			Model.x.r = Model.x.Raster(Bounds.Width < 200);
			Model.y.r = Model.y.Raster(Bounds.Width < 200);
			Model.z.r = Model.z.Raster(Bounds.Height < 200);
			if (Model.Border && Bounds.Width > 1 && Bounds.Height > 1) {
				norms = new Norms(new Matrix());

				Model.View.ResetBounds();
				GraphicsBase.Point[] p = Model.View.BoundsCube();
				PointF[] dp = new PointF[p.Length];
				Model.View.DeviceCoordinates(p, dp);

				norms.Add(dp);

				Crop(norms, bounds);
			}
		}

		/// <summary>
		/// Paints the control.
		/// </summary>
		/// <param name="g">The Graphics object to paint to.</param>
		/// <param name="bounds">The bounds of the plotting area.</param>
		public override void Draw(Graphics g, Rectangle bounds) {
			PointF[] v = new PointF[3];
			Norms norms = new Norms();
			Pen Pen = new Pen(new SolidBrush(Color.White));
			
			g.Clear(Model.BackgroundColor);

			Measure(g, bounds);

			if (Bounds.Width >= 1 && Bounds.Height >= 1) {

				//calculate functions
				PaintThread.DrawStart(this);

				//draw items
				PaintThread.DrawItems(g);

				// draw selection
				if (sw != 0 && sh != 0) {
					Pen.Color = Color.White;
					g.DrawRectangle(Pen, Math.Min(sx, sx + sw), Math.Min(sy, sy + sh),
						Math.Abs(sw), Math.Abs(sh));
					Pen.DashPattern = new float[2] { 3, 3 };
					Pen.DashStyle = DashStyle.Custom;
					Pen.Color = Color.Black;
					g.DrawRectangle(Pen, Math.Min(sx, sx + sw), Math.Min(sy, sy + sh),
						Math.Abs(sw), Math.Abs(sh));
					Pen.DashStyle = DashStyle.Solid;
				}

				Pen.Color = Model.ScaleColor;
				Pen.Width = Model.ScaleLineWidth;

				GraphicsBase.Point[] p = Model.View.BoundsCube();
				PointF[] dp = new PointF[p.Length];
				Model.View.DeviceCoordinates(p, dp);
				g.DrawLines(Pen, dp);

				//draw legend
				GraphicsBase.DrawLegend(g, this);

				//draw status message
				if (!PaintThread.DrawDone) {
					if (Parent != null) Parent.Cursor = Cursors.WaitCursor;
				} else {
					if (Parent != null) Parent.Cursor = Cursors.Cross;
					if (ProgressBar != null) lock (ProgressBar) {
							ProgressBar.Value = 0;
							ProgressBar.Visible = false;
						}
				}

			}
		}

		/// <summary>
		/// Resizes the control.
		/// </summary>
		public override void OnResize(EventArgs e) {
			if (Model.FixXtoY) Model.SetRange(Model.x0, Model.x1, Model.y0, Model.y1, Bounds);
			else Model.Invalidate();
		}

		private MouseButtons button = MouseButtons.None;
		Brush white = new SolidBrush(Color.White);

		/// <summary>
		/// Is called when a mouse button is pressed.
		/// </summary>
		/// <param name="e"></param>
		public override void OnMouseDown(MouseEventArgs e) {
			button = e.Button;
			zoomIn = (button == MouseButtons.Left);
			if (Parent != null) Parent.Cursor = Cursors.Cross;
		}
		/// <summary>
		/// Is called when the mouse moves over the control.
		/// </summary>
		/// <param name="e"></param>
		public override void OnMouseMove(MouseEventArgs e) {
			if (e.Button == MouseButtons.Middle) {
				if (Parent != null) Parent.Cursor = Cursors.SizeAll;
				if (sx != -1) MovePlot(e.X-sx, e.Y-sy);
				sx = e.X;
				sy = e.Y;
			} else if (Parent != null && (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) && !(Model.x.fix && Model.y.fix)) {
				Graphics g = Parent.CreateGraphics();

				Region clip;

				if (sx == -1 || sy == -1) {
					sx = e.X; sy = e.Y; sw = sh = 0;
					if (Model.x.fix) {
						sx = 0; sw = Bounds.Width;
					}
					if (Model.y.fix) {
						sy = 0; sh = Bounds.Height;
					}
				}

				int dw, dh;

				dw = e.X-sx-sw;
				dh = e.Y-sy-sh;

				if (Model.x.fix) { dw = 0; }
				if (Model.y.fix) { dh = 0; }

				int x = Math.Min(sx, sx + sw); int y = Math.Min(sy, sy + sh);
				int w = Math.Abs(sw); int h = Math.Abs(sh);
				clip = new Region(new Rectangle(x, y, w+1, 1));
				clip.Union(new Rectangle(x, y, 1, h+1));
				clip.Union(new Rectangle(x+w, y, 1, h+1));
				clip.Union(new Rectangle(x, y+h, w+1, 1));

				sw += dw;
				sh += dh;

				x = Math.Min(sx, sx + sw); y = Math.Min(sy, sy + sh);
				w = Math.Abs(sw); h = Math.Abs(sh);
				clip.Union(new Rectangle(x, y, w+1, 1));
				clip.Union(new Rectangle(x, y, 1, h+1));
				clip.Union(new Rectangle(x+w, y, 1, h+1));
				clip.Union(new Rectangle(x, y+h, w+1, 1));

				g.SetClip(clip, CombineMode.Intersect);
				g.FillRectangle(white, sx, sy, w+1, h+1);
				g.SmoothingMode = SmoothingMode.AntiAlias;
				Draw(g, Parent.Bounds);
			}

			double mx = GetX(e.X);
			double my = GetY(e.Y);
			double mz = 0;
			if (Model != null) {
				int i = 0;
				while (i < Model.Count && !(Model[i] is Function2DItem)) i++;
				try {
					mz = ((Function2DItem)Model[i]).f(mx, my);
				} catch { }
			}
			FireNotifyCursor(mx, my, mz);

		}

		/// <summary>
		/// Gets the x-coordinate of the speified x-value of a pixel in the plotting area.
		/// </summary>
		public double GetX(int x) {
			return Model.x.WorldCoordinate(x, Bounds.Width);
			//Model.x0 + (x - fx)*(Model.x1 - Model.x0)/fw;
		}

		/// <summary>
		/// Gets the y-coordinate of the speified y-value of a pixel in the plotting area.
		/// </summary>		
		public double GetY(int y) {
			return Model.y.WorldCoordinate(y, Bounds.Height);
			//Model.y0 + (fy + fh - 1 - y)*(Model.y1 - Model.y0)/fh; 
		}

		/// <summary>
		/// Is called when a mouse-button is released.
		/// </summary>
		public override void OnMouseUp(MouseEventArgs e) {
			if (sx != -1 && sy != -1 && sw != 0 && sh != 0 &&
				Math.Abs(sw) > 10 && Math.Abs(sh) > 10) {
				if (zoomIn) {
				} else {
				}
			} else if (sx != -1 && sy != -1 && sw != 0 && sh != 0) {
				sx = sy = -1; sw = sh = 0;
				if (Parent != null) Parent.Invalidate();
			}
			sx = sy = -1; sw = sh = 0;
			if (Parent != null) {
				if (PaintThread.DrawDone) Parent.Cursor = Cursors.Cross;
				else Parent.Cursor = Cursors.WaitCursor;
			}
		}
		/// <summary>
		/// Is called when the mouse-wheel is moved.
		/// </summary>
		public override void OnMouseWheel(MouseEventArgs e) {
			double factor = Math.Pow(1.2, Math.Sign(e.Delta));
			if (factor != 1) {
				double x = Model.x0 + (1 - factor)*e.X*(Model.x1 - Model.x0)/Bounds.Width;
				double y = Model.y0 + (1 - factor)*(Bounds.Height - e.Y)*(Model.y1 - Model.y0)/Bounds.Height;
				if (!(Model.x.fix && Model.y.fix)) {
					if (Model.x.fix) Model.SetRange(Model.x0, Model.x1, y, y + factor * (Model.y1 - Model.y0), Bounds);
					else if (Model.y.fix) Model.SetRange(x, x + factor * (Model.x1 - Model.x0), Model.y0, Model.y1, Bounds);
					else Model.SetRange(x, x + factor * (Model.x1 - Model.x0), y, y + factor * (Model.y1 - Model.y0), Bounds);
				}
			}
		}
	}
}
