using System;
using System.IO;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO.Compression;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// The Model of a <see cref="PlotControl">PlotControl</see> and of a <see cref="Plot">Plot</see>.
	/// The Model contains all data such as functions, plotting area, scale-style, etc.
	/// </summary>
	[Serializable]
	public class PlotModel: ItemList {
		/// <summary>
		/// An Event handler that handles events from the ItemsModel
		/// </summary>
		public class ItemEventHandler: IItemEventHandler {
			/// <summary>
			/// The PlotModel of the ItemEventHandler
			/// </summary>
			public PlotModel Model = null;
			/// <summary>
			/// Creates an ItemEventHandler that delegates events from the ItemsModel to the PlotModel
			/// </summary>
			public ItemEventHandler(ItemsModel Items, PlotModel Model) {
				this.Model = Model;
				Items.Handlers += this;
			}
			/// <summary>
			/// Handles Update events
			/// </summary>
			public void HandleUpdate(Item x) {
				if (Model.Contains(x)) Model.BroadcastUpdate(x);
			}
			/// <summary>
			/// Handles Remove events
			/// </summary>
			public void HandleRemove(Item x) {
				if (Model.Contains(x)) Model.Remove(x);
			}
			/// <summary>
			/// Handles Add events
			/// </summary>
			public void HandleAdd(Item x) {	}
			/// <summary>
			/// Handles Replace events
			/// </summary>
			public void HandleReplace(Item oldItem, Item newItem) {
				if (Model.Contains(oldItem)) {
					Model.Replace(oldItem, newItem);
				}
			}
			/// <summary>
			/// Handles Reorder events
			/// </summary>
			public void HandleReorder(ItemList order) { Model.Reorder(order); }
			/// <summary>
			/// Handles Invalidate events
			/// </summary>
			public void HandleInvalidate() { Model.BroadcastInvalidate(); }
		}

		private const double ln10 = 2.3025850929940456840179914546844;
		
		/// <summary>
		/// The Painters of all Items in the Plot.
		/// </summary>
		[NonSerialized]
		public Dictionary<Item, Painter> Painters;
		/// <summary>
		/// The type of the plot.
		/// </summary>
		public Plot.Type PlotType = Plot.Type.Plot2D;
		private double X0, Y0, X1, Y1, Z0, Z1;
		/// <summary>
		/// All properties of the x-scale.
		/// </summary>
		public Scale x;
		/// <summary>
		/// All properties of the y-scale.
		/// </summary>
		public Scale y;
		/// <summary>
		/// All properties of the z-scale.
		/// </summary>
		public Scale z;
		/// <summary>
		/// Indicates wether the scales should be drawn on both sides.
		/// </summary>
		public bool Twoside;
		/// <summary>
		/// Indicates wether the plotting area should be drawn inside a box.
		/// </summary>
		public bool Border;
		/// <summary>
		/// Indicates if the x-scale should be fixed to the y-scale.
		/// </summary>
		public bool FixXtoY;
		/// <summary>
		/// Indicates if a legend should be drawn.
		/// </summary>
		public bool Legend;
		/// <summary>
		/// Indicates if a border should be drawn around the legend.
		/// </summary>
		public bool LegendBorder;
		/// <summary>
		/// The font to use for the scales.
		/// </summary>
		public Font ScaleFont;
		/// <summary>
		/// The font to use for the legend.
		/// </summary>
		public Font LegendFont;
		/// <summary>
		/// The font to use in the units.
		/// </summary>
		public Font UnitsFont;
		/// <summary>
		/// The background color.
		/// </summary>
		public Color BackgroundColor;
		/// <summary>
		/// The color to use for all scales.
		/// </summary>
		public Color ScaleColor;
		/// <summary>
		/// The line-witdth to draw the scale with.
		/// </summary>
		public float ScaleLineWidth = 1;
		/// <summary>
		/// The filename that is used by Save. Load updates Filename.
		/// </summary>
		[NonSerialized]
		public string Filename;
		/// <summary>
		/// The View parameters of a 3D Plot
		/// </summary>
		[NonSerialized]
		private Graphics3D.View view = new Graphics3D.View();
		[NonSerialized]
		private ItemsModel items = null;
		[NonSerialized]
		private ItemEventHandler handler;
		/// <summary>
		/// The default constructor.
		/// </summary>
		public PlotModel(): base() {
			Painters = new Dictionary<Item, Painter>();
			X0 = Y0 = Z0 = -1; X1 = Y1 = Z1 = 1;
			x = new Scale(this); y = new Scale(this); z = new Scale(this);
			y.invert = true;
			Border = LegendBorder = Twoside = true;
			Legend = FixXtoY = false; 
			ScaleFont = new Font("Arial", 8);
			LegendFont = new Font("Arial", 12);
			UnitsFont = new Font("Arial", 12);
			BackgroundColor = Color.White;
			ScaleColor = Color.Black;
			ScaleLineWidth = 1;
			view.Model = this;
			ResetRaster();
			Handlers += this;
		}
		/// <summary>
		/// Initializes a PlotModel with the given Items Model
		/// </summary>
		public PlotModel(ItemsModel Items): this() {
			ItemsModel = Items;
		}
		/// <summary>
		/// Initializes a PlotModel with the given Items Model
		/// </summary>
		public PlotModel(ItemsModel Items, Plot.Type Type): this(Items) {
			PlotType = Type;
		}
		
		[OnDeserialized]
		private void PlotModelInit(StreamingContext sc) {
			Painters = new Dictionary<Item, Painter>();
			AddPainters();
			Modified = true;
		}
		/// <summary>
		/// Represents the global ItemModel of all global Items
		/// </summary>
		public ItemsModel ItemsModel {
			get { return items; }
			set {
				if (items != value) {
					if (items != null) {
						foreach (IItemEventHandler h in items.Handlers) {
							if (h is ItemEventHandler) {
								ItemEventHandler ih = (ItemEventHandler)h;
								if (ih.Model == this)	items.Handlers -= h;
							}
						}
					}
					if (value != null) {
						handler = new ItemEventHandler(value, this);
					}
					items = value;
				}
			}
		}
		/// <summary>
		/// Sets or gets the 3D view parameters of the PlotModel
		/// </summary>
		public Graphics3D.View View {
			get { return view; }
			set {
				view = value;
				if (view != null) view.Model = this;
			}
		}
		/// <summary>
		/// Copies from another model with a deep copy.
		/// </summary>
		/// <param name="m">The model to copy from.</param>
		public void CopyFrom(PlotModel m)
		{
			X0 = m.X0;	X1 = m.X1; Y0 = m.Y0; Y1 = m.Y1; Z0 = m.Z0; Z1 = m.Z1;

			x.CopyFrom(m.x); y.CopyFrom(m.y); z.CopyFrom(m.z);

			Border = m.Border; FixXtoY = m.FixXtoY; Legend = m.Legend; LegendBorder = m.LegendBorder; Twoside = m.Twoside;

			ScaleFont = (Font)m.ScaleFont.Clone();
			LegendFont = (Font)m.LegendFont.Clone();
			UnitsFont = (Font)m.UnitsFont.Clone();
			ScaleColor = m.ScaleColor;
			ScaleLineWidth = m.ScaleLineWidth;
			View = m.View;

			if (m.Filename != null) Filename = (string)m.Filename.Clone();
			else Filename = null;
			base.CopyFrom(m);
			
			Modified = true;
		}
		/// <summary>
		/// Creates a deep copy.
		/// </summary>
		public new PlotModel Clone() {
			PlotModel m = new PlotModel();
			m.CopyFrom(this);
			return m;
		}
		// mutation of Painters
		private void AddPainter(Item x) {
			lock (Painters) {
				if (x != null && !Painters.ContainsKey(x)) Painters[x] = x.Painter(this);
			}
		}

		private void RemovePainter(Item x) {
			lock (Painters) {
				Painters.Remove(x);
			}
		}

		private void RemovePainters() {
			lock (Painters) {
				foreach (Item x in Painters.Keys) {
					if (!this.Contains(x)) Painters.Remove(x);
				}
			}
		}

		private void AddPainters() {
			lock (Painters) {
				foreach (Item x in this) {
					if (!Painters.ContainsKey(x)) Painters[x] = x.Painter(this);
				}
			}
		}

		private void UpdatePainters() {
			RemovePainters(); AddPainters();
		}

		/// <summary>
		/// Handles Update events
		/// </summary>
		public override void HandleUpdate(Item x) {
			if (Painters.ContainsKey(x) && Painters[x] != null) Painters[x].Modified = true;
		}
		/// <summary>
		/// Handles Remove events
		/// </summary>
		public override void HandleRemove(Item x) {
			RemovePainter(x);
		}
		/// <summary>
		/// Handles Add events
		/// </summary>
		public override void HandleAdd(Item x) {
			AddPainter(x);
		}
		/// <summary>
		/// Handles Replace events
		/// </summary>
		public override void HandleReplace(Item oldItem, Item newItem) {
			if (!this.Contains(oldItem)) Painters.Remove(oldItem);
			Painters.Add(newItem, newItem.Painter(this));
		}
		/// <summary>
		/// Handles Reorder events
		/// </summary>
		public override void HandleReorder(ItemList order) {
		}
		/// <summary>
		/// Handles Invalidate events
		/// </summary>
		public override void HandleInvalidate() {
			this.Modified = true;
		}
		
		/// <summary>
		/// Get or sets the left border of the plotting area.
		/// </summary>
		public double x0 {
			get{lock(this) return X0;}
			set{
				lock(this) {
					double old = X0;
					X0 = value;
					x.lower = X0;
					if (old != X0) Modified = true;
				}
			}
		}

		/// <summary>
		/// Get or sets the right border of the plotting area.
		/// </summary>
		public double x1 {
			get{lock(this) return X1;}
			set{
				lock(this) {
					double old = X1;
					X1 = value;
					x.upper = X1;
					if (old != X1) Modified = true;
				}
			}
		}

		/// <summary>
		/// Get or sets the lower border of the plotting area.
		/// </summary>
		public double y0 {
			get{lock(this) return Y0;}
			set{
				lock(this) {
					double old = Y0;
					Y0 = value;
					y.lower = Y0;
					if (old != Y0) Modified = true;
				}
			}
		}

		/// <summary>
		/// Get or sets the upper border of the plotting area.
		/// </summary>
		public double y1 {
			get{lock(this) return Y1;}
			set{
				lock(this) {
					double old = Y1;
					Y1 = value;
					y.upper = Y1;
					if (old != Y1) Modified = true;
				}
			}
		}

		/// <summary>
		/// Get or sets the lower z-border of the plotting area.
		/// </summary>
		public double z0 {
			get{lock(this) return Z0;}
			set{
				lock(this) {
					double old = Z0;
					Z0 = value;
					z.lower = Z0;
					if (old != Z0) Modified = true;
				}
			}
		}

		/// <summary>
		/// Get or sets the upper z-border of the plotting area.
		/// </summary>
		public double z1 {
			get{lock(this) return Z1;}
			set {
				lock (this) {
					double old = Z1;
					Z1 = value;
					z.upper = Z1;
					if (old != Z1) Modified = true;
				}
			}
		}

		/// <summary>
		/// Indicates wether the x-scale is logarithmic.
		/// </summary>
		public bool xLog {
			get { lock (this) return x.logarithmic; }
			set {
				lock (this) {
					if (!value || (X0 > 0 && X1 > 0) || (X0 < 0 && X1 < 0)) x.logarithmic = value;
					else throw new System.ArgumentException("x-range must not contain zero for logarithmic scale.");
					Modified = true;
				}
			}
		}
		/// <summary>
		/// Indicates wether the y-scale is logarithmic.
		/// </summary>
		public bool yLog {
			get { lock (this) return y.logarithmic; }
			set {
				lock (this) {
					if (!value || (Y0 > 0 && Y1 > 0) || (Y0 < 0 && Y1 < 0)) y.logarithmic = value;
					else throw new System.ArgumentException("y-range must not contain zero for logarithmic scale.");
					Modified = true;
				}
			}
		}
		/// <summary>
		/// Indicates wether the z-scale is logarithmic.
		/// </summary>
		public bool zLog {
			get { lock (this) return z.logarithmic; }
			set {
				lock (this) {
					if (!value || (Z0 > 0 && Z1 > 0) || (Z0 < 0 && Z1 < 0)) z.logarithmic = value;
					else throw new System.ArgumentException("z-range must not conatin zero for logarithmic scale.");
					Modified = true;
				}
			}
		}
		
		/// <summary>
		/// Indicates if the model was modified.
		/// </summary>
		public bool Modified {
			get{
				bool b = false;	
				lock(this) {
					foreach (Item x in this) b = b || (Painters.ContainsKey(x) && Painters[x] != null && Painters[x].Modified);
				}
				return b;
			}
			set {
				lock(this) {
					foreach (Item x in this) {
						if (Painters.ContainsKey(x) && Painters[x] != null) Painters[x].Modified = value;
					}
				}
			}
		}
		/// <summary>
		/// Returns the first Function2DItem in the PlotModel or null.
		/// </summary>
		public Item GetPaintableItem() {
			foreach (Item item in this) {
				if (item.CanPaintTo(this)) return item;
			}
			return null;
		}
		/// <summary>
		/// Resets the raster width to default values.
		/// </summary>
		public void ResetRaster() {
			x.r = x.Raster(false);
			y.r = y.Raster(false);
			z.r = z.Raster(false);
		}
		/// <summary>
		/// Calculates the values for x0 and x1 if FixXtoY is set to true. If it is set to false x0 and x1 remain unchanged.
		/// </summary>
		public void CalcRange(double x0, double x1, double y0, double y1, Rectangle bounds, out double X0, out double X1) {
			if (this.FixXtoY && PlotType == Plot.Type.Plot2D && bounds.Width > 1 && bounds.Height > 1) {
				double dy = (y1-y0)/(bounds.Height-1);
				double w = dy*(bounds.Width-1);
				if ((x0 < x1) == (w < 0)) w = -w;
				X0 = (x1+x0-w)/2;
				X1 = (x1+x0+w)/2;
			} else {
				X0 = x0;
				X1 = x1;
			}
		}
		/// <summary>
		/// Sets the range of the plotting area. Throws an <c>System.ArgumentException</c> if the parameters
		/// are invalid.
		/// </summary>
		/// <param name="x0">The left border of the plotting area.</param>
		/// <param name="x1">The right border of the plotting area.</param>
		/// <param name="y0">The lower border of the plotting area.</param>
		/// <param name="y1">The upper border of the plotting area.</param>
		/// <param name="z0">The lower z-border of the plotting area.</param>
		/// <param name="z1">The upper z-border of the plotting area.</param>
		/// <param name="bounds">The bounds of the plotting area in pixels.</param>
		/// <param name="invalidate">If true the model's Invalidated event is fired.</param>
		/// <param name="calc">If true, the x0 and x1 bounds are set according to FixXtoY</param>
		public void SetRange(double x0, double x1, double y0, double y1, double z0, double z1, Rectangle bounds, bool invalidate, bool calc) {
			if (x0 == x1 || y0 == y1 || z0 == z1) throw new System.ArgumentException("unable to zoom any further");
			if (x.logarithmic && ((x0 <= 0 && x1 >= 0) || (x0 >= 0 && x1 <= 0)))
				throw new System.ArgumentException("x-range must not conatin zero for logarithmic scale.");
			if (y.logarithmic && ((y0 <= 0 && y1 >= 0) || (y0 >= 0 && y1 <= 0)))
				throw new System.ArgumentException("y-range must not conatin zero for logarithmic scale.");
			if (z.logarithmic && ((z0 <= 0 && z1 >= 0) || (z0 >= 0 && z1 <= 0)))
				throw new System.ArgumentException("z-range must not conatin zero for logarithmic scale.");

			if (calc && FixXtoY) CalcRange(x0, x1, y0, y1, bounds, out x0, out x1);
			this.x0 = x0;
			this.x1 = x1;
			this.y0 = y0;
			this.y1 = y1;
			this.z0 = z0;
			this.z1 = z1;
			ResetRaster();
			if (invalidate) Invalidate();
		}
		/// <summary>
		/// Sets the range of the plotting area. Throws an <c>System.ArgumentException</c> if the parameters
		/// are invalid.
		/// </summary>
		/// <param name="x0">The left border of the plotting area.</param>
		/// <param name="x1">The right border of the plotting area.</param>
		/// <param name="y0">The lower border of the plotting area.</param>
		/// <param name="y1">The upper border of the plotting area.</param>
		/// <param name="z0">The lower z-border of the plotting area.</param>
		/// <param name="z1">The upper z-border of the plotting area.</param>
		/// <param name="bounds">The bounds of the plotting area in pixels.</param>
		public void SetRange(double x0, double x1, double y0, double y1, double z0, double z1, Rectangle bounds) {
			SetRange(x0, x1, y0, y1, z0, z1, bounds, true, true);
		}
		/// <summary>
		/// Sets the range of the plotting area.
		/// </summary>
		/// <param name="x0">The left border of the plotting area.</param>
		/// <param name="x1">The right border of the plotting area.</param>
		/// <param name="y0">The lower border of the plotting area.</param>
		/// <param name="y1">The upper border of the plotting area.</param>
		/// <param name="bounds">The bounds of the plotting area in pixels.</param>
		public void SetRange(double x0, double x1, double y0, double y1, Rectangle bounds) {
			SetRange(x0, x1, y0, y1, z0, z1, bounds);
		}
		/// <summary>
		/// Sets the range of the plotting area.
		/// </summary>
		/// <param name="x0">The left border of the plotting area.</param>
		/// <param name="x1">The right border of the plotting area.</param>
		/// <param name="y0">The lower border of the plotting area.</param>
		/// <param name="y1">The upper border of the plotting area.</param>
		/// <param name="z0">The lower z-border of the plotting area.</param>
		/// <param name="z1">The upper z-border of the plotting area.</param>
		public void SetRange(double x0, double x1, double y0, double y1, double z0, double z1) {
			if (FixXtoY && PlotType == Plot.Type.Plot2D) throw new ArgumentException("This overload of SetRange is not supported with FixXtoY=true"); 
			SetRange(x0, x1, y0, y1, z0, z1, new Rectangle(0, 0, 0, 0));
		}
		/// <summary>
		/// Sets the range of the plotting area.
		/// </summary>
		/// <param name="x0">The left border of the plotting area.</param>
		/// <param name="x1">The right border of the plotting area.</param>
		/// <param name="y0">The lower border of the plotting area.</param>
		/// <param name="y1">The upper border of the plotting area.</param>
		public void SetRange(double x0, double x1, double y0, double y1) {
			SetRange(x0, x1, y0, y1, z0, z1);
		}


	}
}
