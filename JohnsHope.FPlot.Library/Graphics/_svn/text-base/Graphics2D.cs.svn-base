using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Diagnostics;

namespace JohnsHope.FPlot.Library {
	// TODO Scale alignment error
	// TODO Scale drawing endless loop
	
	/// <summary>
	/// Describes the norms average, min and max
	/// </summary>
	public enum Norm {
		/// <summary>
		/// The average norm
		/// </summary>
		Average,
		/// <summary>
		/// The minimum norm
		/// </summary>
		Min,
		/// <summary>
		/// The maximum norm
		/// </summary>
		Max }

	/// <summary>
	/// A class that computes a norm over a set of points. This class can be used to calculate the average, maximum and 
	/// minimum coordiantes of a set of points.
	/// </summary>
	public class Norms {
		/// <summary>
		/// The sum, min and max of the points in the norm (Further called the local norm.)
		/// </summary>
		public PointF sum, min, max;
		/// <summary>
		/// The sum, min, and max of the points in T-space of the norm. (Further called the T-space norm.)
		/// </summary>
		public PointF sumT, minT, maxT;
		/// <summary>
		/// The number of points in the norm.
		/// </summary>
		public int N;
		/// <summary>
		/// The transformation matrix used.
		/// </summary>
		public Matrix T;

		/// <summary>
		///  Resets the local part of the norm, sum, avg, min and max, but not the T-space norm sumT, avgT, minT and maxT.
		/// </summary>
		public void ResetLocal() {
			sum = new PointF(0, 0);
			min = new PointF(float.PositiveInfinity, float.PositiveInfinity);
			max = new PointF(float.NegativeInfinity, float.NegativeInfinity);
		}
		/// <summary>
		/// Resets the Norms
		/// </summary>
		public void Reset() {
			ResetLocal();
			sumT = new PointF(0, 0);
			minT = new PointF(float.PositiveInfinity, float.PositiveInfinity);
			maxT = new PointF(float.NegativeInfinity, float.NegativeInfinity);
			N = 0;
		}
		/// <summary>
		/// Creates a new norm with the transformation T
		/// </summary>
		public Norms(Matrix T) {
			Reset();
			this.T = T;
		}
		/// <summary>
		/// creates a new norm with the identity transformation
		/// </summary>
		public Norms() : this(new Matrix()) { }
		/// <summary>
		/// Returns the average of the norm points.
		/// </summary>
		public PointF avg { get { return new PointF(sum.X/N, sum.Y/N); } }
		/// <summary>
		/// Returns the average of the norm points in T-space.
		/// </summary>
		public PointF avgT { get { return new PointF(sumT.X/N, sumT.Y/N); } }
		/// <summary>
		/// Returns the Bounds of the Norms, i.e. the RectangleF(min.X, min.Y, max.X-min.X, max.Y-min.Y).
		/// </summary>
		public RectangleF Bounds {
			get {
				return new RectangleF(min.X, min.Y, max.X-min.X, max.Y-min.Y);
			}
		}
		/// <summary>
		/// Adds two norms together. The norms must have the same T transformation matrix.
		/// </summary>
		public static Norms operator+(Norms a, Norms b) {
			if (a.T != b.T) throw new ArgumentException("Transformations must be the same in a and b.");
			Norms res = new Norms(a.T);
			res.sum.X = a.sum.X + b.sum.X; res.sum.Y = a.sum.Y + b.sum.Y;
			res.min.X = Math.Min(a.min.X, b.min.X);
			res.min.Y = Math.Min(a.min.Y, b.min.Y);
			res.max.X = Math.Max(a.max.X, b.max.X);
			res.max.Y = Math.Max(a.max.Y, b.max.Y);
			res.sumT.X = a.sumT.X + b.sumT.X; res.sumT.Y = a.sumT.Y + b.sumT.Y;
			res.minT.X = Math.Min(a.minT.X, b.minT.X);
			res.minT.Y = Math.Min(a.minT.Y, b.minT.Y);
			res.maxT.X = Math.Max(a.maxT.X, b.maxT.X);
			res.maxT.Y = Math.Max(a.maxT.Y, b.maxT.Y);
			res.N = a.N + b.N;
			return res;
		}
		/// <summary>
		/// Adds points to the norms.
		/// </summary>
		public void Add(params PointF[] pts) {
			PointF[] ptsT = (PointF[])pts.Clone();
			T.TransformPoints(ptsT);
			for (int i = 0; i < pts.Length; i++) {
				sum.X += pts[i].X; sum.Y += pts[i].Y;
				min.X = Math.Min(min.X, pts[i].X);
				min.Y = Math.Min(min.Y, pts[i].Y);
				max.X = Math.Max(max.X, pts[i].X);
				max.Y = Math.Max(max.Y, pts[i].Y);
				sumT.X += ptsT[i].X; sumT.Y += ptsT[i].Y;
				minT.X = Math.Min(minT.X, ptsT[i].X);
				minT.Y = Math.Min(minT.Y, ptsT[i].Y);
				maxT.X = Math.Max(maxT.X, ptsT[i].X);
				maxT.Y = Math.Max(maxT.Y, ptsT[i].Y);
			}
			N += pts.Length;
		}
	}
	/// <summary>
	/// This class contains static routines to Draw specialized 2D objects
	/// </summary>
	public class Graphics2D {
		/// <summary>
		/// Calculates the difference between two points.
		/// </summary>
		public static PointF Diff(PointF a, PointF b) {
			return new PointF(a.X - b.X, a.Y - b.Y);
		}
		/// <summary>
		/// Calculates the angle of a vector from -180 to 180 degrees.
		/// </summary>
		public static float Angle(PointF v) {
			double l = Math.Sqrt(v.X*v.X + v.Y*v.Y);
			float a = (float)(Math.Asin(v.Y/l)*180/Math.PI);
			if (v.X >= 0) return a;
			else return 180 - a;
		}
		/// <summary>
		/// This routine moves the points in p so that the average/min/max Y component is y and the
		/// average/min/max X component is x.
		/// </summary>
		public static void MovePoints(ref PointF[] p, float x, Norm xmode, float y, Norm ymode) {

			float dx = 0, dy = 0;

			Norms norms = new Norms();
			norms.Add(p);

			switch (xmode) {
			case Norm.Average: dx = x - norms.avg.X; break;
			case Norm.Max: dx = x - norms.max.X; break;
			case Norm.Min: dx = x - norms.min.X; break;
			}
			switch (ymode) {
			case Norm.Average: dy = y - norms.avg.Y; break;
			case Norm.Max: dy = y - norms.max.Y; break;
			case Norm.Min: dy = y - norms.min.Y; break;
			}
			for (int i = 0; i < p.Length; i++) {
				p[i].X += dx; p[i].Y += dy;
			}
		}
		/// <summary>
		/// Draws text. The supplied X/Y coordinates denote the average/min/max value of the points of the rectangle around the text,
		/// according to xnorm/ynorm. You can specify a rotation matrix, to draw the text at a certain angle. Also you can specify
		/// a transformation matrix T and its inverse Tinv to influence the drawing position of the text. Note that the matrix T only
		/// influences the position where the text will be drawn and not it's angle or shape.
		/// </summary>
		/// <param name="g">The <see cref="Graphics"/> to draw to.</param>
		/// <param name="text">The text to draw</param>
		/// <param name="draw">If false, the text is not drawn but it's outline rectangle points are assigned to p and added to
		/// norms.</param>
		/// <param name="font">The font to use</param>
		/// <param name="brush">The brush used to draw the text</param>
		/// <param name="background">If background != null, the text background is filled with background</param>
		/// <param name="X">The avg/max/min x-coordinate of the text</param>
		/// <param name="xnorm">The norm to use for the x-coordinate (avg/min/max)</param>
		/// <param name="Y">The avg/max/min y-coordinate of the text</param>
		/// <param name="ynorm">The norm to use for the y-coordinate (avg/min/max)</param>
		/// <param name="Rotation">An additional rotation of the text</param>
		/// <param name="T">The default transformation used for drawing</param>
		/// <param name="Tinv">The inverse of T</param>
		/// <param name="p">Returns the edges of the text rectangle in T-space. You must pass a <see cref="PointF"/> array of size 4</param>
		/// <param name="norms">The <see cref="Norms"/>, the edges of the text will be added to.</param>
		public static void DrawText(Graphics g, string text, bool draw, Font font, Brush brush, Brush background, float X, Norm xnorm, float Y, Norm ynorm,
			//TODO Subscript & Superscript
			Matrix Rotation, Matrix T, Matrix Tinv, ref PointF[] p, ref Norms norms) {
			SizeF size = g.MeasureString(text, font);
			Norms norms1 = new Norms(T);
			// create a rectangle with size size in p.
			p[0] = new PointF(0, 0); p[1] = new PointF(size.Width, 0);
			p[2] = new PointF(size.Width, size.Height); p[3] = new PointF(0, size.Height);
			// rotate the rectangle with rotation
			Rotation.TransformPoints(p);
			// tranform the rectangle into T space
			Tinv.TransformPoints(p);
			// place the rectangle in the correct position
			MovePoints(ref p, X, xnorm, Y, ynorm);
			// calculate the extrema
			norms1.Add(p);
			if (draw) { // draw the text at Avg(p)
				Matrix T0 = g.Transform.Clone();
				g.TranslateTransform(norms1.avgT.X, norms1.avgT.Y);
				g.MultiplyTransform(Rotation);
				if (background != null) {
					p[0].X = -size.Width/2;	p[0].Y = -size.Height/2;
					p[2].X = size.Width - size.Width/2;	p[2].Y = size.Height - size.Height/2;
					p[1].X = p[2].X; p[1].Y = p[0].Y;
					p[3].X = p[0].X; p[3].Y = p[2].Y;
					g.FillPolygon(background, p);
				}
				g.DrawString(text, font, brush, -size.Width/2, -size.Height/2);
				g.Transform = T0;
			}
			norms += norms1;
		}
		/*
		/// <summary>
		/// Represents a raster line
		/// </summary>
		public class Line {
			/// <summary>
			/// The x-coordinate of the line
			/// </summary>
			public double x;
			/// <summary>
			/// Indicates if the line is a big or small raster line
			/// </summary>
			public bool big = false;
		}
		/// <summary>
		/// Represents all raster lines of a scale
		/// </summary>
		public class ScaleLines: List<Line> { }
		/// <summary>
		/// Draws the scale and raster lines specified in lines.
		/// </summary>
		*/ /*
		public void DrawScaleLines(Graphics g, PlotModel Model, Scale scale, bool draw,  PointF[] v, ref Norms norms, ,
			ScaleLines lines) {

			Norms normsText = new Norms();
			Norm yNorm;

			// init matrices
			Matrix id, T0, T, Tprod, Trot, Tinv;

			id = new Matrix();
			T0 = g.Transform.Clone();
			RectangleF Tframe = Bounds;
			T = new Matrix(Tframe, v);
			norms.T = normsText.T = T;
			Tinv = T.Clone();
			Tinv.Invert();
			Tprod = T0.Clone();
			Tprod.Multiply(T);
			Trot = new Matrix();
			if (scale.unitAngleRelative) Trot.Rotate(scale.unitAngle - Angle(Diff(v[1], v[0])));
			else Trot.Rotate(scale.unitAngle);
			g.Transform = Tprod;

			PointF[] p = new PointF[4];
			double x0 = scale.lower, x1 = scale.upper;
			if (x0 > x1) {
				double t = x0; x0 = x1; x1 = t;
			}
			// double d = (x1 - x0)/W;
			// double x = Math.Floor(x0/scale.r)*scale.r + scale.r;
			double sr = scale.r/5;
			float Y, X0 = scale.DeviceCoordinate(0, Bounds);
			SizeF size = g.MeasureString("0.5", Model.ScaleFont);
			float s, s0 = Math.Max(1, (int)(size.Height + 0.5F));
			Pen pen = new Pen(Model.ScaleColor, Model.ScaleLineWidth); // the pen used for drawing
			SolidBrush brush = new SolidBrush(Model.ScaleColor);
			SolidBrush background = new SolidBrush(Model.BackgroundColor);
			foreach (Line l in lines) { // interate over raster
				if (x0 <= l.x && l.x <= x1) {
					float X = scale.DeviceCoordinate(l.x, Bounds);
					bool isX0 = Math.Round(X0) == Math.Round(X);
					if (draw && (!isX0 || !scale.axis)) { // X is not axis
						if (scale.grid && l.big) { // draw grid
							pen.Color = Color.FromArgb(255, Model.ScaleColor);
							g.DrawLine(pen, X, 0, X, Bounds.Height);
							pen.Color = Model.ScaleColor;
						}
						if (scale.raster) { // draw raster
							if (l.big) s = s0;
							else s = s0/2;
							if (scale.rasterOutside) { // draw raster on outside
								if (scale.raster) g.DrawLine(pen, X, -s, X, 0);
								if (scale.oppositeRaster) g.DrawLine(pen, X, Bounds.Height + s, X, H);
							} else { // draw raster on inside
								if (scale.raster) g.DrawLine(pen, X, 0, X, s);
								if (scale.oppositeRaster) g.DrawLine(pen, X, H - s, X, H);
							}
						}
					} else if (draw && scale.axis) { // draw 0-axis.
						g.DrawLine(pen, X, 0, X, H);
					}

					if (scale.scale && l.big) { // draw scale text
						string label;
						if (isX0) label = "0";
						else label = scale.UnitToString(l.x);

						if (scale.scaleOutside != scale.rasterOutside) s = 0.5F*s0; // distance from line to scale 
						else s = 1.5F*s0;
						if (scale.scaleOutside) { // draw scale outside the box
							Y = -s;
							yNorm = Norm.Max;
						} else { // draw scale inside the box
							Y = s;
							yNorm = Norm.Min;
						}
						g.Transform = T0; // reset transformation
						DrawText(g, label, draw, Model.ScaleFont, brush, background, X, Norm.Average, Y, yNorm, id, T, Tinv, ref p, ref normsText);
						g.Transform = Tprod;
					}
				}
			}

			// draw units
			g.Transform = T0;

			size = g.MeasureString(scale.unit, Model.UnitsFont);
			s0 = size.Height/2;

			if (scale.scaleOutside) {
				Y = normsText.min.Y - s0; yNorm = Norm.Max; // draw the units below the scale
			} else {
				Y = normsText.max.Y + s0; yNorm = Norm.Min; // draw the units above the scale
			}
			DrawText(g, scale.unit, draw, Model.UnitsFont, brush, background, W/2, Norm.Average, Y, yNorm, Trot, T, Tinv, ref p, ref norms);
			norms += normsText;
		}
		*/

		/// <summary>
		/// Draws a <see cref="Scale">Scale</see>. The point-array v is an array of 3 points that denote the orientation
		/// of the scale. The primary scale is drawn from v[0] -> v[1], the opposite scale is drawn from 
		/// v[0] + v[2] -> v[1] + v[2]. 
		/// </summary>
		/// <param name="g">The <see cref="Graphics"/> object to draw to.</param>
		/// <param name="v">The orientation of the scale</param>
		/// <param name="Model">The <see cref="PlotModel"/> of the Plot to draw</param>
		/// <param name="scale">The scale to draw</param>
		/// <param name="draw">If false, the scale is not drawn but it's dimensions are added to norms.</param>
		/// <param name="norms">The <see cref="Norms"/> the extension-points of the scale will be added to.</param>
		public static void DrawScale(Graphics g, PointF[] v, PlotModel Model, Scale scale, bool draw, ref Norms norms) {
			//TODO Inverse Scales (where scale.lower > scale.upper)
			//TODO logscales

			const int GridAlpha = 64; // the alpha-color value of the grid lines.
			float X, X0, Y, W, H, s, s0;
			double x, sr, t, x0, x1;
			Matrix id, T0, T, Tprod, Trot, Tinv;
			int n, Width;
			Pen pen = new Pen(Model.ScaleColor, Model.ScaleLineWidth); // the pen used for drawing
			SolidBrush brush = new SolidBrush(Model.ScaleColor);
			SolidBrush background = new SolidBrush(Model.BackgroundColor);
			string label;
			RectangleF Tframe;
			PointF d1, d2, d3;
			bool isX0;
			Norm yNorm;
			PointF[] p = new PointF[4];
			SizeF size;
			Norms normsText = new Norms();

			x0 = scale.lower; x1 = scale.upper;
			if (x0 > x1) {	// lower bound is greater than upper bound. Exchange bounds.
				t = x0; x0 = x1; x1 = t;
			}
			d1 = Diff(v[1], v[0]);
			d2 = Diff(v[2], v[0]);
			d3 = Diff(v[1], v[2]);
			if (v[0] == v[1] || v[0] == v[2] || v[1] == v[2]) return;

			W = (float)Math.Sqrt(d1.X*d1.X + d1.Y*d1.Y);
			H = (float)Math.Sqrt(d2.X*d2.X + d2.Y*d2.Y);
			W = Math.Max(W, 1);
			H = Math.Max(H, 1);

			Width = (int)(W + 1.5);
			Tframe = new RectangleF(0, 0, W, H);

			size = g.MeasureString("0.5", Model.ScaleFont);
			s0 = Math.Max(1, size.Height);

			// init matrices
			id = new Matrix();
			T0 = g.Transform.Clone();
			T = new Matrix(Tframe, v);
			norms.T = normsText.T = T;
			Tinv = T.Clone();
			Tinv.Invert();
			Tprod = T0.Clone();
			Tprod.Multiply(T);
			Trot = new Matrix();
			if (scale.unitAngleRelative) Trot.Rotate(scale.unitAngle - Angle(Diff(v[1], v[0])));
			else Trot.Rotate(scale.unitAngle);
			g.Transform = Tprod;

			p[0] = new PointF(0, 0); p[1] = new PointF(W, 0); p[2] = new PointF(W, H); p[3] = new PointF(0, H);
			norms.Add(p);

			if (draw) {	// draw the solid lines of the scale and opposite scale
				if (scale.line) g.DrawLine(pen, p[0], p[1]);
				if (scale.oppositeLine) g.DrawLine(pen, p[3], p[2]);
			}

			// add the minimum rectangle to the normsText. (So that the units will be in the right position, even if there is no scale.)
			if (scale.scaleOutside != scale.rasterOutside) s = 0.5F*s0;
			else s = 1.5F*s0;
			if (scale.scaleOutside) s = -s;
			Y = s;
			p[0] = new PointF(0, Y); p[1] = new PointF(W, Y); p[2] = new PointF(W, Y); p[3] = new PointF(0, Y);
			normsText.Add(p);
			
			// draw scale
			if (!scale.logarithmic) {
				sr = scale.r;
				x = Math.Floor(x0/sr)*sr + sr;
				sr = sr/5;
				X0 = scale.DeviceCoordinate(0, (int)(W + 0.5F));
				n = -4;
				// interate over raster
				while ((sr > 0 && x + sr*n < x0) || (sr < 0 && x + sr*n > x0)) n++;
				while ((sr > 0 && x + sr*n < x1) || (sr < 0 && x + sr*n > x1)) {
					X = scale.DeviceCoordinate(x + sr*n, (int)(W + 0.5F));
					// X = (float)((x + sr*n - x0)/d);
					isX0 = Math.Round(X0) == Math.Round(X);
					if (draw && (!isX0 || !scale.axis)) { // X is not axis
						if (scale.grid && n%5 == 0) { // draw grid
							pen.Color = Color.FromArgb(GridAlpha, Model.ScaleColor);
							g.DrawLine(pen, X, 0, X, H);
							pen.Color = Model.ScaleColor;
						}
						if (scale.raster) { // draw raster
							if (n%5 == 0) s = s0;
							else s = s0/2;
							if (scale.rasterOutside) { // draw raster on outside
								if (scale.raster) g.DrawLine(pen, X, -s, X, 0);
								if (scale.oppositeRaster) g.DrawLine(pen, X, H + s, X, H);
							} else { // draw raster on inside
								if (scale.raster) g.DrawLine(pen, X, 0, X, s);
								if (scale.oppositeRaster) g.DrawLine(pen, X, H - s, X, H);
							}
						}
					} else if (scale.axis && draw) { // draw 0-axis.
						g.DrawLine(pen, X, 0, X, H);
					}

					if (scale.scale && (n%5 == 0)) { // draw scale
						if (isX0) label = "0";
						else label = scale.UnitToString(x + sr*n);

						if (scale.scaleOutside != scale.rasterOutside) s = 0.5F*s0; // distance from line to scale 
						else s = 1.5F*s0;
						if (scale.scaleOutside) { // draw scale outside the box
							Y = -s;
							yNorm = Norm.Max;
						} else { // draw scale inside the box
							Y = s;
							yNorm = Norm.Min;
						}
						g.Transform = T0; // reset transformation
						DrawText(g, label, draw, Model.ScaleFont, brush, null, X, Norm.Average, Y, yNorm, id, T, Tinv, ref p, ref normsText);
						g.Transform = Tprod;
					}
					n++;
				}
			} else { //scale is logarithmic
				double l0 = Math.Log10(Math.Abs(x0)), l1 = Math.Log10(Math.Abs(x1));
				double ld = l1 - l0;
			}

			// draw units
			g.Transform = T0;

			size = g.MeasureString(scale.unit, Model.UnitsFont);
			s0 = size.Height/2;

			if (scale.scaleOutside) {
				Y = normsText.min.Y - s0; yNorm = Norm.Max; // draw the units below the scale
			} else {
				Y = normsText.max.Y + s0; yNorm = Norm.Min; // draw the units above the scale
			}
			DrawText(g, scale.unit, draw, Model.UnitsFont, brush, null, W/2, Norm.Average, Y, yNorm, Trot, T, Tinv, ref p, ref norms);
			norms += normsText;
		}


		/// <summary>
		/// Draws a z-<see cref="Scale">Scale</see> for a <see cref="Plot2D">2D Plot</see>.
		/// </summary>
		/// <param name="g">The <see cref="Graphics"/> to paint to</param>
		/// <param name="Model">The <see cref="PlotModel"/> of the Plot</param>
		/// <param name="v">The orientation of the scale</param>
		/// <param name="draw">If false, the scale is not drawn only is extensions are measured</param>
		/// <param name="r">The rectangle where the z-gradient is drawn inside</param>
		/// <param name="norms">The extensions of the painting</param>
		public static void DrawZScale(Graphics g, PlotModel Model, ref PointF[] v, bool draw, Rectangle r, ref Norms norms) {
			Function2DItem f = Model.GetPaintableItem() as Function2DItem;
			if (Model.z.scale && f != null) {
				Model.z.oppositeRaster = Model.z.oppositeLine = false;
				Model.z.scaleOutside = Model.z.rasterOutside = false;
				if (draw) {
					GradientPainter.FillRectangle(g, f.Gradient, r, GradientPainter.Direction.Up);
					Pen pen = new Pen(Model.ScaleColor, Model.ScaleLineWidth);
					g.DrawRectangle(pen, r);
				}
				SizeF size = g.MeasureString("0.5", Model.ScaleFont);
				float s0 = Math.Max(1, size.Height);
				v[0] = new PointF(r.X + r.Width, r.Y + r.Height);
				v[1] = new PointF(r.X + r.Width, r.Y);
				v[2] = new PointF(r.X + r.Width + s0, r.Y + r.Height);

				DrawScale(g, v, Model, Model.z, draw, ref norms);
			}
		}

	}
}
