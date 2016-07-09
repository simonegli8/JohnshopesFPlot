using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// The style with which to display numbers.
	/// </summary>
	public enum NumberStyle {
		/// <summary>
		/// Uses either fixedpoint or scientific notation depending on the number.
		/// </summary>
		Normal,
		/// <summary>
		/// Use always fixedpoint notation.
		/// </summary>
		Fixedpoint,
		/// <summary>
		/// Use always scientific notation.
		/// </summary>
		Scientific
	}
	/// <summary>
	/// A class that describes the properties of the x-,y- and z-scale.
	/// </summary>
	[Serializable]
	public class Scale {
		const double ln10 = 2.3025850929940456840179914546844;
		/// <summary>
		/// The model the Scale belongs to. 
		/// </summary>
		public PlotModel model;
		/// <summary>
		/// The width of the rasterlines, or 0 if the rasterlines should be computed automatically.
		/// </summary>
		public double r;
		/// <summary>
		/// Indicates if the 0-axis should be drawn. Default is false.
		/// </summary>
		public bool axis = false;
		/// <summary>
		/// Indicates if the scale should be logarithmic. Default is false.
		/// </summary>
		public bool logarithmic = false;
		/// <summary>
		/// Indicates if the scale numbers are shown. Default is true.
		/// </summary>
		public bool scale = true;
		/// <summary>
		/// Indicates if the scale numbers should be on the inside or outside of the plotting area. Default value is true.
		/// </summary>
		public bool scaleOutside = true;
		/// <summary>
		/// Indicates if the scale raster lines should be drawn inside or outside of the plotting area. Default value is false.
		/// </summary>
		public bool rasterOutside = false;
		/// <summary>
		/// Indicates if raster lines should be drawn. Default is true.
		/// </summary>
		public bool raster = true;
		/// <summary>
		/// Indicates if a scale-line should be drawn. Default is true.
		/// </summary>
		public bool line = true;
		/// <summary>
		/// Indicates if grid lines should be drawn. Default is false.
		/// </summary>
		public bool grid = false;
		/// <summary>
		/// Indicates if the view range of this scale is fixed (no zooming in this direction allowed). Default is false.
		/// </summary>
		public bool fix = false;
		/// <summary>
		/// The text for the units of this scale.
		/// </summary>
		public string unit;
		/// <summary>
		/// The angle with which the units are drawn. Default is 0.
		/// </summary>
		public float unitAngle = 0;
		/// <summary>
		/// Indicates if the unit angle is relative to the scale or absolute. Default is true.
		/// </summary>
		public bool unitAngleRelative = true;
		/// <summary>
		/// If true, the scale-line and its opposite side scale will be drawn. Default is true.
		/// </summary>
		public bool oppositeLine = true;
		/// <summary>
		/// If true and oppositeScale is true, the opposite side scale will be drawn with a raster. Default is true.
		/// </summary>
		public bool oppositeRaster = true;
		/// <summary>
		/// The number of digits used for the scale numbers. Default is 3.
		/// </summary>
		public int digits = 3;
		/// <summary>
		/// The number style used for the scale numbers.
		/// </summary>
		public NumberStyle style;
		/// <summary>
		/// If true, no border is drawn
		/// </summary>
		public bool noBorder = false;
		/// <summary>
		/// If true, the DeviceCoordinates are inverted.
		/// </summary>
		public bool invert = false;
		/// <summary>
		/// The upper limit of the plotting area on that scale.
		/// </summary>
		public double upper {
			get {
				if (!invert) return x1;
				else return x0;
			}
			set {
				if (!invert) {
					x1 = value; logx1 = Math.Log10(Math.Abs(x1));
				} else {
					x0 = value; logx0 = Math.Log10(Math.Abs(x0));
				}
			}
		}
		/// <summary>
		/// The lower limit of the plotting area on that scale.
		/// </summary>
		public double lower {
			get {
				if (!invert) return x0;
				else return x1;
			}
			set {
				if (!invert) {
					x0 = value; logx0 = Math.Log10(Math.Abs(x0));
				} else {
					x1 = value; logx1 = Math.Log10(Math.Abs(x1));
				}
			}
		}
		private double x0, x1, logx0, logx1;

		/// <summary>
		/// The constructor of the Scale class.
		/// </summary>
		public Scale(PlotModel model) {
			this.model = model;
			r = 0;
			unit = null;
			style = NumberStyle.Normal;
			lower = -1;
			upper = 1;
		}
		/// <summary>
		/// Copies all scale data from the parameter source with a deep copy.
		/// </summary>
		public void CopyFrom(Scale source) {
			r = source.r;
			axis = source.axis;
			logarithmic = source.logarithmic;
			scale = source.scale;
			raster = source.raster;
			line = source.line;
			scaleOutside = source.scaleOutside;
			rasterOutside = source.rasterOutside;
			oppositeLine = source.oppositeLine;
			oppositeRaster = source.oppositeRaster;
			grid = source.grid;
			fix = source.fix;
			invert = source.invert;
			upper = source.upper;
			lower = source.lower;
			if (source.unit == null) unit = null;
			else unit = (string)source.unit.Clone();
			unitAngle = source.unitAngle;
			unitAngleRelative = source.unitAngleRelative;
			digits = source.digits;
			style = source.style;
		}
		/// <summary>
		/// Creates a deep copy.
		/// </summary>
		public Scale Clone(PlotModel model) {
			Scale copy = new Scale(model);
			copy.CopyFrom(this);
			return copy;
		}
		/// <summary>
		/// Returns a format string for use with the ToString method.
		/// </summary>
		/// <returns></returns>
		public string NumberFormat() {
			switch (style) {
			case NumberStyle.Normal:
				return "G"+digits.ToString();
			case NumberStyle.Fixedpoint:
				return "F"+digits.ToString();
			case NumberStyle.Scientific:
				return "E"+digits.ToString();
			default:
				return "G3";
			}
		}
		/// <summary>
		/// Returns the value x as a string, using the Scale's number format.
		/// </summary>
		public string UnitToString(double x) {
			return x.ToString(NumberFormat());
		}
		/// <summary>
		/// Returns the space between raster lines
		/// </summary>
		public double Raster(bool few) {
			double w = Math.Abs(x1-x0);
			double d = Math.Exp(Math.Floor(Math.Log(w)/ln10)*ln10);
			if (few) {
				if (w/d > 5) return 2*d;
				else if (w/d > 3) return d;
				else return d/2;
			} else {
				if (w/d > 5) return d;
				else if (w/d > 3) return d/2;
				else return d/5;
			}
		}
		/// <summary>
		/// Calculates a logarithmic value between 0 and 1 from a x between lower and upper. 
		/// </summary>
		public double Log(double x) {
			double d = logx1 - logx0;
			return (Math.Log10(Math.Abs(x)) - logx0)/d;
		}
		/// <summary>
		/// calculates a value between lower and upper from a log between 0 and 1.
		/// </summary>
		public double Exp(double log) {
			double d = logx1 - logx0;
			return Math.Exp(ln10*(d*log - logx0));
		}
		/// <summary>
		/// Gets the device coordinate of the point x.
		/// </summary>
		/// <param name="x">The point x in world coordinates</param>
		/// <param name="length">The length of the device plotting area</param>
		/// <returns>The device coordinate of x</returns>
		public float DeviceCoordinate(double x, int length) {
			const double BIG = 1e20F;
			double X;
			if (!logarithmic) X = (x - x0)/(x1 - x0)*(length-1);
			else {
				if (x == 0) X = double.NegativeInfinity;
				else X = Log(Math.Abs(x))*(length-1);
			}
			if (double.IsInfinity(X) || Math.Abs(X) > BIG) {
				if (X < -BIG && x1 > x0) X = -10;
				else X = length + 10;
			}
			return (float)X;
		}
		/// <summary>
		/// Gets the world coordinate of the point X.
		/// </summary>
		/// <param name="X">The point X in device coordinates</param>
		/// <param name="length">The length of the device plotting area</param>
		/// <returns>The world coordinate of X</returns>
		public double WorldCoordinate(int X, int length) {
			if (length <= 1) return x0;
			else if (!logarithmic) return x0 + X*(x1 - x0)/(length-1);
			else return Exp(((double)X)/((double)(length-1)));
		}
		/// <summary>
		/// Draws a scale. The point-array v is an array of 3 points that denote the orientation of the scale. The primary scale
		/// is drawn from v[0] -> v[1], the opposite scale is drawn from v[0] + v[2] -> v[1] + v[2]. 
		/// </summary>
		/// <param name="g">The Graphics object to Draw to</param>
		/// <param name="v">The orientation of the scale</param>
		/// <param name="draw">If false the Scale is not drawn but its extensions are calculated in norms</param>
		/// <param name="norms">The extensions of the scale</param>
		public void Draw(Graphics g, PointF[] v, bool draw, ref Norms norms) {
			Graphics2D.DrawScale(g, v, model, this, draw, ref norms);
		}

	}
}
