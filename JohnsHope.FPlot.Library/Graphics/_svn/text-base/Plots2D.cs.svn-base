using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Reflection;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// Plots 2D plots
	/// </summary>
	//TODO better mouse button handling
	[Serializable]
	public class Plot2D: Plot  {
		[NonSerialized]
		int sx, sy, sw, sh, fd, fld;
		[NonSerialized]
		bool zoomIn;

		/// <summary>
		/// Constructs a new Plot2D.
		/// </summary>
		/// <param name="Model">The PlotModel of the Plot2D</param>
		public Plot2D(PlotModel Model): base(Model) {
			sx = sy = -1; sw = sh = 0;
			fd = fld = 1;
			zoomIn = false;
		}
		/// <summary>
		/// Copies from another Plot2D object
		/// </summary>
		public void CopyFrom(Plot2D p) {
			base.CopyFrom(p);
			fd = p.fd; fld = p.fld;
			Model = p.Model;
		}
		/// <summary>
		/// Clones the Plot2D object
		/// </summary>
		public override Plot Clone() {
			Plot2D p = new Plot2D(Model);
			p.CopyFrom(this);
			return p;
		}
		/// <summary>
		/// Returns the x device coordinate of a x world coordinate 
		/// </summary>
		public float DeviceCoordinateX(double x) {
			return Model.x.DeviceCoordinate(x, Bounds.Width) + Bounds.X;
		}
		/// <summary>
		/// Returns the y device coordinate of a y world coordinate.
		/// </summary>
		public float DeviceCoordinateY(double y) {
			return Model.y.DeviceCoordinate(y, Bounds.Height) + Bounds.Y;
		}
		/// <summary>
		/// Returns a x world coordinate of a x device coordinate.
		/// </summary>
		public double WorldCoordinateX(int X) {
			return Model.x.WorldCoordinate(X - Bounds.X, Bounds.Width);
		}
		/// <summary>
		/// Returns a y world coordinate of a y device coordinate.
		/// </summary>
		public double WorldCoordinateY(int Y) {
			return Model.y.WorldCoordinate(Y - Bounds.Y, Bounds.Height);
		}
		/// <summary>
		/// Returns the device coordinates of a world cooridnates Point.
		/// </summary>
		public PointF DeviceCoordinate(GraphicsBase.Point world) {
			return new PointF(Model.x.DeviceCoordinate(world.x, Bounds.Width) + Bounds.X,
				Model.y.DeviceCoordinate(world.y, Bounds.Height) + Bounds.Y);
		}
		/// <summary>
		/// Returns the world coordinates of a device coordinates PointF.
		/// </summary>
		public GraphicsBase.Point WorldCoordinate(PointF device) {
			return new GraphicsBase.Point(Model.x.WorldCoordinate((int)(device.X - Bounds.X + 0.5F), Bounds.Width),
				Model.y.WorldCoordinate((int)(device.Y - Bounds.Y + 0.5F), Bounds.Height));
		}
		/// <summary>
		/// Transforms the points in <em>world</em> to device coordinates in the array <em>device</em>. <em>device</em> must be of the
		/// same size than <em>world</em>.
		/// </summary>
		public void DeviceCoordinates(GraphicsBase.Point[] world, PointF[] device) {
			if (world.Length != device.Length) throw new ArgumentException("Plot2D.DeviceCoordinates: world and device point arrays " + 
				"must be of the same size.");
			for (int i = 0; i < world.Length; i++) {
				device[i] = DeviceCoordinate(world[i]);
			}
		}
		/// <summary>
		/// Transforms the points in <em>device</em> to world coordinates in the array <em>world</em>. <em>world</em> must be of the
		/// same size than <em>device</em>.
		/// </summary>
		public void WorldCoordinates(PointF[] device, GraphicsBase.Point[] world) {
			if (world.Length != device.Length) throw new ArgumentException("Plot2D.DeviceCoordinates: world and device point arrays " + 
				"must be of the same size.");
			for (int i = 0; i < world.Length; i++) {
				world[i] = WorldCoordinate(device[i]);
			}
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

		private bool Crop(Norms norms, Rectangle bounds, out double x0, out double x1) {
			bool res = base.Crop(norms, bounds);
			if (Model.FixXtoY && Bounds.Width > 1 && Bounds.Height > 1) {
				Model.CalcRange(Model.x0, Model.x1, Model.y0, Model.y1, Bounds, out x0, out x1);
			} else {
				x0 = Model.x0; x1 = Model.x1;
			}
			return res;
		}
	
		private void Measure(Graphics g, Rectangle bounds) {
			SizeF size;
			Size oldSize;
			PointF[] v = new PointF[3];
			Norms norms;
			double x0, x1, y0, y1;

			size = g.MeasureString("0.5", Model.ScaleFont);
			fd = Math.Max(1, (int)(size.Height + 0.5F));
			size = g.MeasureString("0.5", Model.LegendFont);
			fld = Math.Max(1, (int)(size.Height + 0.5F));

			oldSize = Bounds.Size;
			Bounds = bounds;
			x0 = Model.x0; x1 = Model.x1; y0 = Model.y0; y1 = Model.y1;
			Model.x.scaleOutside = Model.y.scaleOutside = Model.Border;
			Model.x.rasterOutside = Model.y.rasterOutside = false;
			Model.z.scaleOutside = Model.z.rasterOutside = true;
			Model.y.r = Model.y.Raster(Bounds.Height < 100); Model.z.r = Model.z.Raster(Bounds.Height < 100);
			if (Model.Border && Bounds.Width > 1 && Bounds.Height > 1) {
				do {
					// Set the x0 and x1 bounds. Do not use Model.SetRange, because it would set Model.Modified to true.
					Model.x.lower = x0; Model.x.upper = x1; Model.x.r = Model.x.Raster(Bounds.Width < 100);
					norms = new Norms(new Matrix());
					if (Model.x.scale || Model.x.unit != null) {
						v[0] = new PointF(Bounds.X, Bounds.Y + Bounds.Height);
						v[1] = new PointF(Bounds.X + Bounds.Width, Bounds.Y + Bounds.Height);
						v[2] = new PointF(Bounds.X, Bounds.Y);
						Model.x.Draw(g, v, false, ref norms);
					}

					if (Model.y.scale || Model.y.unit != null) {
						v[0] = new PointF(Bounds.X, Bounds.Y);
						v[1] = new PointF(Bounds.X, Bounds.Y + Bounds.Height);
						v[2] = new PointF(Bounds.X + Bounds.Width, Bounds.Y);
						Model.y.Draw(g, v, false, ref norms);
					}

					Graphics2D.DrawZScale(g, Model, ref v, false, new Rectangle(Bounds.X + Bounds.Width + fd, Bounds.Y, fd, Bounds.Height), ref norms);

				} while (Crop(norms, bounds, out x0, out x1));
			}
			Bounds.Width = Math.Max(Bounds.Width, 1); Bounds.Height = Math.Max(Bounds.Height, 1);

			if (oldSize != Bounds.Size) {
				Model.SetRange(x0, x1, Model.y0, Model.y1, Model.z0, Model.z1, Bounds, false, false);
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

			g.Clear(Model.BackgroundColor);
			SmoothingMode smoothingMode = g.SmoothingMode;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Measure(g, bounds);

			if (Bounds.Width >= 1 && Bounds.Height >= 1) {

				//calculate functions
				PaintThread.DrawStart(this);

				//draw items
				PaintThread.DrawItems(g);

				// draw selection
				if (sw != 0 && sh != 0) {
					Pen Pen = new Pen(new SolidBrush(Color.White));
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

				// draw x-scale
				v[0] = new PointF(Bounds.X, Bounds.Y + Bounds.Height);
				v[1] = new PointF(Bounds.X + Bounds.Width, Bounds.Y + Bounds.Height);
				v[2] = new PointF(Bounds.X, Bounds.Y);
				Model.x.Draw(g, v, true, ref norms);

				// draw y-scale
				v[0] = new PointF(Bounds.X, Bounds.Y);
				v[1] = new PointF(Bounds.X, Bounds.Y + Bounds.Height);
				v[2] = new PointF(Bounds.X + Bounds.Width, Bounds.Y);
				Model.y.Draw(g, v, true, ref norms);

				//draw z-scale
				Graphics2D.DrawZScale(g, Model, ref v, true, new Rectangle(Bounds.X + Bounds.Width + fd, Bounds.Y, fd, Bounds.Height), ref norms);

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

				g.SmoothingMode = smoothingMode;
			}
		}

		/// <summary>
		/// Resizes the control.
		/// </summary>
		public override void OnResize(EventArgs e) {
			if (Model.FixXtoY) Model.SetRange(Model.x0, Model.x1, Model.y0, Model.y1, Bounds);
			else Model.Invalidate();
		}
		[NonSerialized]
		private MouseButtons button = MouseButtons.None;
		[NonSerialized]
		Brush white = new SolidBrush(Color.White);


		void ZoomCursor(Plot.MouseEventArgs e) {
			if (Parent != null && e.Button == MouseButtons.Left) {
				zoomIn = ((e.ModifierKeys & (Keys.Shift | Keys.Control)) == Keys.None);
				if (zoomIn) Parent.Cursor = SpecialCursors.EnlargeCursor;
				else Parent.Cursor = SpecialCursors.ShrinkCursor;
			}
		}

		/// <summary>
		/// Is called when a mouse button is pressed.
		/// </summary>
		/// <param name="e"></param>
		public override void OnMouseDown(Plot.MouseEventArgs e) {
			button = e.Button;
			ZoomCursor(e);
		}
		/// <summary>
		/// Is called when the mouse moves over the control.
		/// </summary>
		/// <param name="e"></param>
		public override void OnMouseMove(Plot.MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				if (Parent != null) Parent.Cursor = Cursors.SizeAll;
				if (sx != -1) MovePlot(e.X-sx, e.Y-sy);
				sx = e.X;
				sy = e.Y;
			} else if (Parent != null && (e.Button == MouseButtons.Left) && !(Model.x.fix && Model.y.fix)) {
				ZoomCursor(e);

				Graphics g = Parent.CreateGraphics();

				Region clip;

				if (sx == -1 || sy == -1) {
					sx = e.X; sy = e.Y; sw = sh = 0;
					if (Model.x.fix) {
						sx = Bounds.X; sw = Bounds.Width;
					}
					if (Model.y.fix) {
						sy = Bounds.Y; sh = Bounds.Height;
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
				Draw(g, new Rectangle(0, 0, Parent.Bounds.Width, Parent.Bounds.Height));
			}

			ZoomCursor(e);
			double mx = WorldCoordinateX(e.X);
			double my = WorldCoordinateY(e.Y);
			double mz = 0;
			if (Model != null) {
				int i = 0;
				while (i < Model.Count && !(Model[i] is Function2DItem)) i++;
				if (i < Model.Count) {
					try {
						mz = ((Function2DItem)Model[i]).f(mx, my);
					} catch { }
				}
			}
			FireNotifyCursor(mx, my, mz);

		}
		/// <summary>
		/// Zooms into or out of a specified rectangle in device coordinates.
		/// </summary>
		/// <param name="zoomIn">Zoom in or out</param>
		/// <param name="area">The rectangle in device coordinates.</param>
		public void Zoom(bool zoomIn, Rectangle area) {
			int sx = area.X, sy = area.Y, sw = area.Width, sh = area.Height;

			if (sx != -1 && sy != -1 &&  Math.Abs(sw) > 10 && Math.Abs(sh) > 10) {
				if (zoomIn) {
					if (Model.x.fix) Model.SetRange(Model.x0, Model.x1, WorldCoordinateY(Math.Max(sy, sy+sh)),
						WorldCoordinateY(Math.Min(sy, sy+sh)), Bounds);
					else if (Model.y.fix) Model.SetRange(WorldCoordinateX(Math.Min(sx, sx + sw)), WorldCoordinateX(Math.Max(sx, sx + sw)),
						Model.y0, Model.y1, Bounds);
					else Model.SetRange(WorldCoordinateX(Math.Min(sx, sx+sw)), WorldCoordinateX(Math.Max(sx, sx+sw)),
						WorldCoordinateY(Math.Max(sy, sy+sh)), WorldCoordinateY(Math.Min(sy, sy+sh)), Bounds);
				} else {
					double dx = (Model.x1 - Model.x0)/Math.Abs(sw);
					double dy = (Model.y1 - Model.y0)/Math.Abs(sh);
					double y = Model.y0 - (Bounds.Y + Bounds.Height - Math.Max(sy, sy+sh))*dy;
					if (Model.x.fix) Model.SetRange(Model.x0, Model.x1, y, Bounds.Height * dy + y, Bounds);
					else if (Model.y.fix) Model.SetRange(Model.x0 - (Math.Min(sx, sx+sw) - Bounds.X)*dx,
						Model.x0 + (Bounds.X + Bounds.Width - Math.Min(sx, sx+sw))*dx, Model.y0, Model.y1, Bounds);
					else Model.SetRange(Model.x0 - (Math.Min(sx, sx+sw) - Bounds.X)*dx,
						Model.x0 + (Bounds.X + Bounds.Width - Math.Min(sx, sx+sw))*dx, y, Bounds.Height*dy + y, Bounds);
				}
			} else if (sx != -1 && sy != -1 && sw != 0 && sh != 0 && Parent != null) Parent.Invalidate();
			 
		}

		/// <summary>
		/// Is called when a mouse-button is released.
		/// </summary>
		public override void OnMouseUp(Plot.MouseEventArgs e) {
			if (sx != -1 && sy != -1 && sw != 0 && sh != 0)	Zoom(zoomIn, new Rectangle(sx, sy, sw, sh));
			sx = sy = -1; sw = sh = 0;
			if (Parent != null) {
				if (PaintThread.DrawDone) Parent.Cursor = Cursors.Cross;
				else Parent.Cursor = Cursors.WaitCursor;
			}
		}
		/// <summary>
		/// Is called when the mouse-wheel is moved.
		/// </summary>
		public override void OnMouseWheel(Plot.MouseEventArgs e) {
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
		/// <summary>
		/// Sets the range of a 2D Plot
		/// </summary>
		/// <param name="x0"></param>
		/// <param name="x1"></param>
		/// <param name="y0"></param>
		/// <param name="y1"></param>
		/// <param name="z0"></param>
		/// <param name="z1"></param>
		public override void SetRange(double x0, double x1, double y0, double y1, double z0, double z1) {
			Model.SetRange(x0, x1, y0, y1, z0, z1, Bounds);
		}
	}
}
