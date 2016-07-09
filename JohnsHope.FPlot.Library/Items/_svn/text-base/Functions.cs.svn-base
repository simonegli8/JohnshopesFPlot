using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;
using JohnsHope.Analysis;

namespace JohnsHope.FPlot.Library {

	/// <summary>
	/// The base class for function items.
	/// </summary>
	[Serializable]
	public class FunctionItem: Item, IParametricFunction {
		/// <summary>
		/// The number of function evaluations. Each function evaluation increments this value by one.
		/// </summary>
		[NonSerialized]
		public int NEval = 0;
		/// <summary>
		/// Parameters used by the function.
		/// </summary>
		public SmallData p = new SmallData();
		/// <summary>
		/// The derivatives df/dp of the function and the parameters.
		/// </summary>
		public new SmallData dfdp = new SmallData();
		/// <summary>
		/// Indicates if the function has been modified. This value is automatically set if <see cref="p">p</see> has changed or the
		/// function has been compiled.
		/// </summary>
		public override bool Modified {
			get{return base.Modified || p.Modified; }
			set{
				if (value == false) p.Modified = false;
				base.Modified = value;
			}
		}
		/// <summary>
		/// Ensures that either there is no derivative information in the function (no reference to dfdp)
		/// or that the lenghts of the array p and dfdp are the same. 
		/// </summary>
		protected void CheckdfdpLength() {
			if ((dfdp.Length > 0) && (p.Length != dfdp.Length)) {
				dfdp.Length = p.Length;
				for (int i = 0; i < dfdp.Length; i++) {
					dfdp[i] = double.NaN;
				}
			}
		}
		/// <summary>
		/// The default constructor.
		/// </summary>
		public FunctionItem() : base() { }
		/// <summary>
		/// Returns a user friendly name for the item.
		/// </summary>
		public override string TypeName() {
			return "Function";
		}
		/// <summary>
		/// Copies from another FunctionItem.
		/// </summary>
		/// <param name="src"></param>
		public override void CopyFrom(Item src) {
			base.CopyFrom(src);
			p = ((FunctionItem)src).p.Clone();
			dfdp = ((FunctionItem)src).dfdp.Clone();
		}
		/// <summary>
		/// Creates a copy of the FunctionItem.
		/// </summary>
		public override Item Clone() {
			FunctionItem f = new FunctionItem();
			f.CopyFrom(this);
			return f;
		}
		/// <summary>
		/// The following property is true, if the function can be fitted.
		/// </summary>
		public virtual bool Fitable {
			get {
				return p.Length > 0;
			}
		}
		/// <summary>
		/// An list of the functions parameters.
		/// </summary>
		public IList<double> Parameters {
			get { return p; }
			set {
				if (!(value is SmallData)) {
					p.Clear();
					foreach (double x in value) p.Add(x);
				} else {
					if (value is SmallData) p = ((SmallData)value).Clone();
					else p = new SmallData(value);
				}
			}
		}
		/// <summary>
		/// If set to false, exceptions will be cached upon calculation of the function and double.NaN returned instead
 		/// </summary>
		public virtual bool ThrowOnErrors {
			get { return false; }
			set { }
		}
		/// <summary>
		/// Returns all FunctionItems in a given list
		/// </summary>
		public static ItemList FunctionItems(ItemList list) {
			ItemList res = new ItemList();
			foreach (Item x in list) {
				if (x is FunctionItem) res.Add(x);
			}
			return res;
		}
		/// <summary>
		/// Returns the linenumber of the first line in the source. Always returns 1.
		/// </summary>
		public override int FirstSourceLine {
			get {	return 1; }
		}
	}
	/// <summary>
	/// This class represents a ordinary one dimensional function of the form
	/// <code>
	/// double f(double x) {
	///	  ...
	///	}
	/// </code>
	/// </summary>
	[Serializable]
	public class Function1DItem: FunctionItem, ICompilableClass, ILine, IParametricFunction, DefaultClass.IFunction1D {
		/// <summary>
		/// The base class of a compiled instance.
		/// </summary>
		public class Instance: Item {
			/// <summary>
			/// The parameter array p.
			/// </summary>
			public SmallData p;
			/// <summary>
			/// The derivatives df/dp.
			/// </summary>
			public new SmallData dfdp;
			/// <summary>
			/// The function to evaluate.
			/// </summary>
			public virtual double f(double x) { return double.NaN; }
		}
		private Color color = Color.Black;
		/// <summary>
		/// The color used to draw the function
		/// </summary>
		public Color Color {
			get { return color; }
			set { color = value; }
		}
		private DashStyle lineStyle = DashStyle.Solid;
		/// <summary>
		/// The line style used to draw the item.
		/// </summary>
		public DashStyle LineStyle {
			get { return lineStyle; }
			set { lineStyle = value; }
		}
		private float lineWidth = 1;
		/// <summary>
		/// The line width used to draw the item. 
		/// </summary>
		public float LineWidth {
			get { return lineWidth; }
			set { lineWidth = value; }
		}
		[NonSerialized]
		private Instance instance = new Instance();
		/// <summary>
		/// The source code of the function. The source represents the following function:
		/// <code>
		/// double[] p, dfdp;
		/// double f(double x) {
		///   ... Source ...
		/// }
		/// </code>
		/// </summary>
		public string Source = null;

		/// <summary>
		/// Copies from another Funtion1D.
		/// </summary>
		public override void CopyFrom(Item src) {
			base.CopyFrom(src);
			Function1DItem f = (Function1DItem)src;
			color = f.color;
			lineStyle = f.lineStyle;
			lineWidth = f.lineWidth;
			Source = f.Source;
			Compiler.Compile(this);
		}
		/// <summary>
		/// Creates a copy of the Function1DItem.
		/// </summary>
		public override Item Clone() {
			Function1DItem f = new Function1DItem();
			f.CopyFrom(this);
			return f;
		}
		/// <summary>
		/// The default constructor.
		/// </summary>
		public Function1DItem(): base() {}
		/// <summary>
		/// A constructor that set the Source of the function and compiles it.
		/// </summary>
		public Function1DItem(string source) : base() {
			Source = source;
			Compile();
		}

		/// <summary>
		/// The function that evaluates the 1D Function.
		/// </summary>
		public virtual double f(double x) {
			NEval++;
			instance.p = p; instance.dfdp = dfdp;
			return instance.f(x);
		}
		/// <summary>
		/// Returns the source of the function.
		/// </summary>
		public override string GetSource() {
			if (Source == null) return null;
			return "namespace JohnsHope.FPlot.Library.Code{public class Item" + TypeIndex +
				":JohnsHope.FPlot.Library.Function1DItem.Instance{public override double f(double x){\n" +	Source + "}}}";
		}
		/// <summary>
		/// Adapts the length of the <c>p</c> and <c>dfdp</c> arrays.
		/// </summary>
		public object ClassInstance {
			set {
				if (value != null) instance = (Instance)value;
				else instance = new Instance();
				//evaluate number of parameters p and derivatives dfdp
				p.AutoResize = dfdp.AutoResize = true;
				p.Length = dfdp.Length = 0;
				try {
					f(1);
				} catch { }
				p.AutoResize = dfdp.AutoResize = false;
				CheckdfdpLength();
			}
		}
		/// <summary>
		/// Gets the class name for the compiler.
		/// </summary>
		public string ClassName {
			get { return "JohnsHope.FPlot.Library.Code.Item" + TypeIndex; }
		}
		/// <summary>
		/// Returns if the function source calculates the derivatives <c>dfdp</c>.
		/// </summary>
		public bool HasDerivatives { get { return (p.Length > 0) && (dfdp.Length > 0) &&	(p.Length == dfdp.Length); } }
		/// <summary>
		/// Returns a painter for the function.
		/// </summary>
		public override Painter Painter(PlotModel model) {
			if (model.PlotType == Plot.Type.Plot2D) return new Function1DPainter2D(model, this);
			else return null;
		}
		/// <summary>
		/// Gets a user friendly name for the Item type.
		/// </summary>
		public override string TypeName() {
			return "1D Function";
		}
		/// <summary>
		/// Returns all Function1DItems in a given list
		/// </summary>
		public static ItemList Function1DItems(ItemList list) {
			ItemList res = new ItemList();
			foreach (Item x in list) {
				if (x is Function1DItem) res.Add(x);
			}
			return res;
		}
	}
	/// <summary>
	/// This class denotes a 2D <see cref="Painter">Painter</see> for a Function1D item.
	/// </summary>
	public class Function1DPainter2D: Painter {
		private Function1DItem F;
		private List<PointF[]> cache = new List<PointF[]>();
		/// <summary>
		/// A constructor that sets the Item and PlotModel of the painter.
		/// </summary>
		public Function1DPainter2D(PlotModel m, Item x) : base(m, x) { F = (Function1DItem)x; }

		private void BinSearchNaN(double x0, double x1, out double Y, out double X) {
			double y, y0, y1;

			try {
				y0 = F.f(x0);
			} catch (ThreadAbortException ex) {
				throw ex;
			} catch { y0 = double.NaN; }
			try {
				y1 = F.f(x1);
			} catch (ThreadAbortException ex) {
				throw ex;
			} catch { y1 = double.NaN; }

			if (!double.IsNaN(y0)) {
				if (double.IsNaN(y1)) Y = y0;
				else throw new Exception("No NaN parameter");
			} else if (double.IsNaN(y1)) throw new Exception("Only NaN parameters.");
			else Y = y1;

			X = (x0 + x1) / 2;
			while (x0 != X && x1 != X) {
				try {
					y = F.f(X);
				} catch (ThreadAbortException ex) {
					throw ex;
				} catch {
					y = double.NaN;
				}
				if (double.IsNaN(y) == double.IsNaN(y1)) { x1 = X; y1 = y; } else { x0 = X; y0 = y; }
				if (!double.IsNaN(y)) Y = y;
				X = (x0 + x1) / 2;
			}
			if (double.IsNaN(Y)) throw new Exception("Y is NaN");
		}
		/// <summary>
		/// Starts the Recalculation of the Painter.
		/// </summary>
		/// <param name="plot">The Plot to paint from</param>
		public override void Start(Plot plot) {
			base.Start(plot);
			if (Recalc) {
				lock (this) this.cache = new List<PointF[]>();
				plot.MaxProgress += plot.Bounds.Width;
			}
		}
		/// <summary>
		/// Recalculates the Painter.
		/// </summary>
		/// <param name="plot">The Plot to paint from</param>
		public override void Calc(Plot plot) {
			Plot2D plot2D = (Plot2D)plot;
			int X, Xp;
			double x, y, x0, x1, xn;
			float Y;
			PointF[] points = new PointF[plot2D.Bounds.Width];
			List<PointF[]> plist = cache;
			if (Recalc) {
				x0 = x1 = Model.x0;
				for (X = plot2D.Bounds.X, Xp = 0; X < plot2D.Bounds.X + plot2D.Bounds.Width; X++) {
					if (plot2D.DrawStop) break;
					x = plot2D.WorldCoordinateX(X);
					try {
						y = F.f(x);
					} catch (ThreadAbortException ex) {
						throw ex;
					} catch (Exception e) {
						if (F.ThrowOnErrors) throw e;
						else y = double.NaN;
					}
					Y = plot2D.DeviceCoordinateY(y);
					if (float.IsNaN(Y)) {
						if (Xp > 0) {
							// search the y value where the function turns into NaN 
							BinSearchNaN(x1, x, out y, out xn);
							points[Xp].X = plot2D.DeviceCoordinateX(xn);
							points[Xp++].Y = plot2D.DeviceCoordinateY(y);
							// insert the points into the cache
							PointF[] copy = new PointF[Xp];
							Array.Copy(points, copy, Xp);
							lock (this) plist.Add(copy);
						}
						Xp = -1;
					} else {
						if (Xp == -1) { // the last point was a NaN
							// search the y value where the function turns into NaN 
							BinSearchNaN(x1, x, out y, out xn);
							points[0].X = plot2D.DeviceCoordinateX(xn);
							points[0].Y = plot2D.DeviceCoordinateY(y);
							Xp = 1;
						}
						points[Xp].X = X;
						points[Xp++].Y = Y;
					}
					x0 = x1;
					x1 = x;
					plot2D.Progress++;				
				}
				if (Xp == plot2D.Bounds.Width) lock (this) plist.Add(points);
				else if (Xp > 0) {
					PointF[] copy = new PointF[Xp];
					Array.Copy(points, copy, Xp);
					lock (this) plist.Add(copy);
				}
				Recalc = plot2D.DrawStop;
			}
		}
		/// <summary>
		/// Paints the previosuly calculated Painter to g.
		/// </summary>
		/// <param name="g">The Graphics to paint to</param>
		/// <param name="plot">The Plot to paint from</param>
		public override void Paint(Graphics g, Plot plot) {
			Pen Pen = new Pen(new SolidBrush(F.Color));
			Pen.DashStyle = F.LineStyle;
			Pen.Width = F.LineWidth;
			for (int i = 0; i < cache.Count; i++) {
				if (Monitor.TryEnter(this)) {
					try {
						g.DrawLines(Pen, cache[i]);
					} catch (Exception ex) { Console.WriteLine(ex.Message); }
					Monitor.Exit(this);
				}
			}
		}
	}
	/// <summary>
	/// This class represents two dimensional functions of the form
	/// <code>
	/// double f(double x, double y) {
	///   ...
	/// }
	/// </code>
	/// </summary>
	[Serializable]
	public class Function2DItem: FunctionItem, ICompilableClass, IParametricFunction2D, DefaultClass.IFunction2D {
		/// <summary>
		/// The base class of a compiled instance.
		/// </summary>
		public class Instance: Item {
			/// <summary>
			/// The parameter array p.
			/// </summary>
			public SmallData p;
			/// <summary>
			/// The derivatives df/dp.
			/// </summary>
			public new SmallData dfdp;
			/// <summary>
			/// The function to evaluate.
			/// </summary>
			public virtual double f(double x, double y) { return double.NaN; }
		}
		/// <summary>
		/// The Gradient used to draw the function.
		/// </summary>
		public IGradient Gradient = new RainbowGradient();
		// new LinearGradient(Color.Transparent, Color.Black);
		/// <summary>
		/// The source code of the function. The source represents the following function:
		/// <code>
		/// double[] p, dfdp;
		/// double f(double x, double y) {
		///   ... Source ...
		/// }
		/// </code>
		/// </summary>
		public string Source = null;
		[NonSerialized]
		private Instance instance = new Instance();
		/// <summary>
		/// Copies from another Function2D.
		/// </summary>
		public override void CopyFrom(Item src) {
			base.CopyFrom(src);
			Function2DItem f = (Function2DItem)src;
			Gradient = f.Gradient.Clone();
			Source = f.Source;
			Compiler.Compile(this);
		}
		/// <summary>
		/// Creates a copy of the Function2DItem.
		/// </summary>
		public override Item Clone() {
			Function2DItem f = new Function2DItem();
			f.CopyFrom(this);
			return f;
		}
		/// <summary>
		/// The default constructor.
		/// </summary>
		public Function2DItem() : base() { }
		/// <summary>
		/// A constructor that set the Source of the function and compiles it.
		/// </summary>
		public Function2DItem(string source) : base() {
			Source = source;
			Compile();
		}
		/// <summary>
		///  The evaluating function.
		/// </summary>
		public virtual double f(double x, double y) {
			NEval++;
			instance.p = p; instance.dfdp = dfdp;
			return instance.f(x, y);
		}
		private Color color;
		/// <summary>
		/// The color used to draw the function.
		/// </summary>
		public Color Color {
			get{ return color; }
			set{
				color = value;
				if (Gradient is LinearGradient) ((LinearGradient)Gradient).UpperColor = color;
				Modified = true;
			}
		}
		/// <summary>
		/// Returns the C# source of the function.
		/// </summary>
		public override string GetSource() {
			if (Source == null) return null;
			return "namespace JohnsHope.FPlot.Library.Code{public class Item" + TypeIndex +
				":JohnsHope.FPlot.Library.Function2DItem.Instance{public override double f(double x, double y){\n" +	Source + "}}}";
		}
		/// <summary>
		/// Adapts the length of the <c>p</c> and <c>dfdp</c> arrays.
		/// </summary>
		public object ClassInstance {
			set {
				if (value != null) instance = (Instance)value;
				else instance = new Instance();
				//evaluate number of parameters p and derivatives dfdp
				p.AutoResize = dfdp.AutoResize = true;
				p.Length = dfdp.Length = 0;
				try {
					f(1, 1);
				} catch { }
				p.AutoResize = dfdp.AutoResize = false;
				CheckdfdpLength();
			}
		}
		/// <summary>
		/// Gets the class name for the compiler.
		/// </summary>
		public string ClassName {
			get { return "JohnsHope.FPlot.Library.Code.Item" + TypeIndex; }
		}
		/// <summary>
		/// Returns a painter for the function.
		/// </summary>
		public override Painter Painter(PlotModel model) {
			switch (model.PlotType) {
			case Plot.Type.Plot2D: return new Function2DPainter2D(model, this);
			default: return null;
			}
		}
		/// <summary>
		/// Gets a user friendly name for the item type.
		/// </summary>
		/// <returns></returns>
		public override string TypeName() {
			return "2D Function";
		}
		/// <summary>
		/// Returns all Function1DItems in a given list
		/// </summary>
		public static ItemList Function2DItems(ItemList list) {
			ItemList res = new ItemList();
			foreach (Item x in list) {
				if (x is Function2DItem) res.Add(x);
			}
			return res;
		}
	}
	/// <summary>
	/// This class represents a 2D <see cref="Painter">Painter</see> for a 2D function.
	/// </summary>
	public class Function2DPainter2D: Painter {
		Function2DItem F;
		/// <summary>
		/// The calculated bitmap of the function.
		/// </summary>
		private BitmapBuilder cache;
		/// <summary>
		/// A constructor setting the model and item of the painter.
		/// </summary>
		public Function2DPainter2D(PlotModel m, Item x) : base(m, x) { F = (Function2DItem)x; }
		/// <summary>
		/// Starts recalculation of the painter.
		/// </summary>
		public override void Start(Plot plot) {
			base.Start(plot);
			if (Recalc) {
				cache = new BitmapBuilder(plot.Bounds);
				cache.Graphics.Clear(Color.Transparent);
				plot.MaxProgress += plot.Bounds.Width * plot.Bounds.Height;
			}
		}
		/// <summary>
		/// Recalculates the Painter
		/// </summary>
		public override void Calc(Plot plot) {
			int X, Y;
			double z, dzinv;
			Plot2D plot2D = (Plot2D)plot;
			Rectangle r = plot2D.Bounds;
			BitmapBuilder bmp;
			//Color c;
			if (Recalc) {
				dzinv = 1 / (Model.z1 - Model.z0);
				unsafe {
					bmp = cache;
					bmp.Lock();
					int* pixel = bmp.Pixel(r.X, r.Y);
					for (Y = r.Y; Y < r.Y + r.Height; Y++) {
						if (plot2D.DrawStop) break;
						for (X = r.X; X < r.X + r.Width; X++) {
							if (plot2D.DrawStop) break;
							try {
								z = (F.f(plot2D.WorldCoordinateX(X), plot2D.WorldCoordinateY(Y)) - Model.z0) * dzinv; 
							} catch (ThreadAbortException ex) {
								throw ex;
							} catch (Exception e) {
								if (F.ThrowOnErrors) throw e;
								else z = double.NaN;
							}
							if (double.IsInfinity(z) || double.IsNaN(z)) {
								*pixel = Color.Transparent.ToArgb();
							} else {
								try {
									*pixel = F.Gradient.Color(z).ToArgb();
								} catch (ThreadAbortException ex) {
									throw ex;
								} catch {
									*pixel = Color.Transparent.ToArgb();
								}
							}
							pixel++;
						}
						plot2D.Progress += r.Width;
					}
					bmp.Unlock();
				}
				Recalc = plot2D.DrawStop;
			}
		}

		/// <summary>
		/// Paints the Painter.
		/// </summary>
		public override void Paint(Graphics g, Plot plot) {
			cache.TryPaint(g);
		}
	}

	/// <summary>
	/// This class represents two dimensional functions that return a Color of the form
	/// <code>
	/// System.Drawing.Color f(double x, double y) {
	///   ...
	/// }
	/// </code>
	/// </summary>
	[Serializable]
	public class FunctionColorItem: FunctionItem, ICompilableClass {
		/// <summary>
		/// The base class of a compiled instance.
		/// </summary>
		public class Instance: Item {
			/// <summary>
			/// The parameter array p.
			/// </summary>
			public SmallData p;
			/// <summary>
			/// The derivatives df/dp.
			/// </summary>
			public new SmallData dfdp;
			/// <summary>
			/// The function to evaluate.
			/// </summary>
			public virtual Color f(double x, double y) { return Color.Transparent; }
		}
		/// <summary>
		/// The source code of the function. The source represents the following function:
		/// <code>
		/// double[] p, dfdp;
		/// System.Drawing.Color f(double x, double y) {
		///   ... Source ...
		/// }
		/// </code>
		/// </summary>
		public string Source = null;
		[NonSerialized]
		private Instance instance = new Instance();
		/// <summary>
		/// Copies from another FunctionColor.
		/// </summary>
		public override void CopyFrom(Item src) {
			base.CopyFrom(src);
			FunctionColorItem f = (FunctionColorItem)src;
			Source = f.Source;
			Compiler.Compile(this);
		}
		/// <summary>
		/// The default constructor.
		/// </summary>
		public FunctionColorItem(): base() { }
		/// <summary>
		/// A constructor that set the Source of the function and compiles it.
		/// </summary>
		public FunctionColorItem(string source) : base() {
			Source = source;
			Compile();
		}
		/// <exclude/>
		public virtual Color f(double x, double y) {
			NEval++;
			instance.p = p; instance.dfdp = dfdp;
			return instance.f(x, y);
		}
		/// <summary>
		/// Returns the C# code for the function.
		/// </summary>
		public override string GetSource() {
			if (Source == null) return null;
			return "namespace JohnsHope.FPlot.Library.Code{public class Item" + TypeIndex +
				":JohnsHope.FPlot.Library.FunctionColorItem.Instance{public override System.Drawing.Color f(double x, double y){\n" + Source +
				"}}}";
		}
		/// <summary>
		/// Adapts the length of the <c>p</c> and <c>dfdp</c> arrays.
		/// </summary>
		public object ClassInstance {
			set {
				if (value != null) instance = (Instance)value;
				else instance = new Instance();
				//evaluate number of parameters p and derivatives dfdp
				p.AutoResize = dfdp.AutoResize = true;
				p.Length = dfdp.Length = 0;
				try {
					f(1, 1);
				} catch { }
				p.AutoResize = dfdp.AutoResize = false;
				CheckdfdpLength();
			}
		}
		/// <summary>
		/// Gets the class name for the compiler.
		/// </summary>
		public string ClassName {
			get { return "JohnsHope.FPlot.Library.Code.Item" + TypeIndex; }
		}
		/// <summary>
		/// Returns a painter for the function.
		/// </summary>
		public override Painter Painter(PlotModel model) {
			if (model.PlotType == Plot.Type.Plot2D) return new FunctionColorPainter2D(model, this);
			else return null;
		}
		/// <summary>
		/// Gets a user friendly name for the Item type.
		/// </summary>
		public override string TypeName() {
			return "Color Function";
		}
		/// <summary>
		/// Returns all Function1DItems in a given list
		/// </summary>
		public static ItemList FunctionColorItems(ItemList list) {
			ItemList res = new ItemList();
			foreach (Item x in list) {
				if (x is FunctionColorItem) res.Add(x);
			}
			return res;
		}
	}
	/// <summary>
	/// This class is a 2D <see cref="Painter">Painter</see> for a color function.
	/// </summary>
	public class FunctionColorPainter2D: Painter {
		private FunctionColorItem F;
		private BitmapBuilder cache;
		/// <summary>
		/// The default constructor.
		/// </summary>
		public FunctionColorPainter2D(PlotModel m, Item x) : base(m, x) { F = (FunctionColorItem)x; }
		/// <summary>
		/// Starts calculation of the Painters image data
		/// </summary>
		/// <param name="plot">The Plot to paint from</param>
		public override void  Start(Plot plot) {
 			base.Start(plot);
			if (Recalc) {
				cache = new BitmapBuilder(plot.Bounds);
				cache.Graphics.Clear(Color.Transparent);
				plot.MaxProgress += plot.Bounds.Width * plot.Bounds.Height;
			}
		}
		/// <summary>
		/// Recalculates the Painter.
		/// </summary>
		public override void Calc(Plot plot) {
			int X, Y;
			Plot2D plot2D = (Plot2D)plot;
			BitmapBuilder bmp = cache;
			Rectangle r = plot2D.Bounds;
			if (Recalc) {
				unsafe {
					lock (this) {
						int* pixel;
						Color c;
						bmp.Lock();
						pixel = bmp.Pixel(r.X, r.Y);
						for (Y = r.Y; Y < r.Y + r.Height; Y++) {
							if (plot2D.DrawStop) break;
							for (X = r.X; X < r.X + r.Width; X++) {
								if (plot2D.DrawStop) break;
								try {
									c = F.f(plot2D.WorldCoordinateX(X), plot2D.WorldCoordinateY(Y));
								} catch (ThreadAbortException ex) {
									throw ex;
								} catch (Exception e) {
									if (F.ThrowOnErrors) throw e;
									else c = Color.Transparent;
								}
								*pixel = c.ToArgb();
								pixel++;
							}
							plot2D.Progress += r.Width;
						}
						bmp.Unlock();
					}
				}
				Recalc = plot.DrawStop;
			}
		}

		/// <summary>
		/// Paints the cached bitmap that was previously calculated.
		/// </summary>
		public override void Paint(Graphics g, Plot plot) {
			cache.TryPaint(g);
		}
	}

}
