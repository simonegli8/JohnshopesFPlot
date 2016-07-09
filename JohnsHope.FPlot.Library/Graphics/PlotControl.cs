using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// PlotControl is a Windows.Forms Control that displays mathematical funcions and data. Here is some example code, that displays a
	/// x^2 function.
	/// <code>
	/// using JohsnHope.FPlot.Library;
	/// using System.Windows.Forms;
	/// using System.Drawing;
	/// 
	/// namespace FPlotTest {
	/// 
	///		public class MainForm: Form {
	///		
	///			MainForm() {
	///				PlotControl plot = new PlotControl(Plot.Type.Plot2D);
	///				plot.Bounds = new Rectangle(5, 5, 100, 100);
	///				Function1DItem f = new Function1DItem();
	///				f.source = &quot;return x*x&quot;;
	///				Compiler.Compile(f);
	///				plot.Model.Add(f);
	///				Add(plot);
	///			}
	///		}
	///		
	///		public static Main(string[] args) {
	///			MainForm f = new MainForm();
	///			f.Bounds = new Rectangle(0, 0, 110, 110);
	///			Application.Run(f);
	///		}
	/// 
	///}
	/// </code>
	/// </summary>
	[ToolboxBitmap(typeof(resfinder), "JohnsHope.FPlot.Library.Resources.Mandelbrot.ico")]
	public class PlotControl: System.Windows.Forms.Control, IItemEventHandler {

		private PlotModel model;
		/// <summary>
		/// The Plot that draws the content of the Control
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Plot Plot;
		private SolidBrush brush = new SolidBrush(Color.Black);
		private Pen pen = new Pen(Color.Black, 1);

		private ProgressBar progressBar;
		/// <summary>
		/// A constant representing the scale transform used when drawing the control.
		/// </summary>
		public float scale = 1;

		/// <summary>
		/// if SynchDraw is set to false, Draw returns before the complete plot is drawn.
		/// </summary>
		public bool SynchDraw = false;
		/// <summary>
		/// The PlotModel for the PlotControl.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PlotModel Model {
			get { return model; }
			set {
				if (model != null) model.Handlers -= this;
				model = value;
				if (model != null) {
					model.Handlers += this;
					Plot = Plot.New(model);
					Plot.Parent = this;
					Plot.ProgressBar = progressBar;
				}
				Invalidate();
			}
		}
		/// <summary>
		/// The constructor of the control.
		/// </summary>
		public PlotControl(PlotModel Model) {
			this.Model = Model;
			Cursor = Cursors.Cross;
			BackColor = Color.White;
		}
		/// <summary>
		/// Constructor for a new PlotControl of the specified type
		/// </summary>
		public PlotControl(Plot.Type type) {
			PlotModel m = new PlotModel();
			m.PlotType = type;
			Model = m;
			Cursor = Cursors.Cross;
			BackColor = Color.White;
		}
		/// <summary>
		/// The default constructor. Creates a PlotControl of type Plot.Type.Plot2D.
		/// </summary>
		public PlotControl(): this(new PlotModel()) { }
		/// <summary>
		/// <c>ProgressBar</c> denotes a System.Windows.Form ProgressBar, that shows the progress of the calculating Painter thread.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ProgressBar ProgressBar {
			get { return progressBar; }
			set {
				if (value != null) value.MarqueeAnimationSpeed = 0;
				progressBar = value;
				Plot.ProgressBar = progressBar;
			}
		}

		/// <summary>
		/// Dispose can be called to abort all threads.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if (disposing) {
				try {
					Plot.Dispose();
				} catch {}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Copies from another PlotControl.
		/// </summary>
		/// <param name="plot">The control to copy from.</param>
		public void CopyFrom(PlotControl plot) {
			Model = (PlotModel)model.Clone();
			Plot = plot.Plot.Clone();
			Plot.Parent = this;
		}

		/// <summary>
		/// Creates a deep copy of the control. 
		/// </summary>
		public PlotControl Clone() {
			PlotControl dest = new PlotControl();
			dest.CopyFrom(this);
			return dest;
		}

		#region Properties

		/// <summary>
		/// Represents the left border of the displayed plotting area.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(0)]
		public double x0 {
			get {
				if (Model != null) return Model.x0;
				else return -1;
			}
			set {
				if (Model != null) Model.x0 = value;
				ResetRange();
			}
		}

		/// <summary>
		/// Represents the right border of the displayed plotting area.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(1)]
		public double x1 {
			get {
				if (Model != null) return Model.x1;
				else return -1;
			}
			set {
				if (Model != null) Model.x1 = value;
				ResetRange();
			}
		}

		/// <summary>
		/// Represents the lower border of the displayed plotting area.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(0)]
		public double y0 {
			get {
				if (Model != null) return Model.y0;
				else return -1;
			}
			set {
				if (Model != null) Model.y0 = value;
				ResetRange();
			}
		}

		/// <summary>
		/// Represents the upper border of the displayed plotting area.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(1)]
		public double y1 {
			get {
				if (Model != null) return Model.y1;
				else return -1;
			}
			set {
				if (Model != null) Model.y1 = value;
				ResetRange();
			}
		}

		/// <summary>
		/// Represents the lower value of the displayed z-range.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(0)]
		public double z0 {
			get {
				if (Model != null) return Model.z0;
				else return -1;
			}
			set {
				if (Model != null) Model.z0 = value;
				ResetRange();
			}
		}

		/// <summary>
		/// Represents the upper value of the displayed z-range.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(1)]
		public double z1 {
			get {
				if (Model != null) return Model.z1;
				else return -1;
			}
			set {
				if (Model != null) Model.z1 = value;
				ResetRange();
			}
		}

		/// <summary>
		/// If true a x-axis will be drawn at y = 0. The default value is <c>false</c>.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(false)]
		public bool xAxis {
			get {
				if (Model != null) return Model.x.axis;
				else return false;
			}
			set {
				if (Model != null) {
					Model.x.axis = value;
					Model.Invalidate();
				}
			}
		}

		/// <summary>
		/// If true a y-axis will be drawn at x = 0. The default value is <c>false</c>.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(false)]
		public bool yAxis {
			get {
				if (Model != null) return Model.y.axis;
				else return false;
			}
			set {
				if (Model != null) {
					Model.y.axis = value;
					Model.Invalidate();
				}
			}
		}
		/// <summary>
		/// If true a box is drawn around the plotting area. The default value is <c>true</c>.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(false)]
		public bool Border {
			get {
				if (Model != null) return Model.Border;
				else return true;
			}
			set {
				if (Model != null) {
					Model.Border = Model.x.scaleOutside = Model.y.scaleOutside = value;
					Model.Invalidate();
				}
			}
		}
		/// <summary>
		/// If true the x-scale is fixed to the y-scale. The default value is <c>false</c>.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(true)]
		public bool FixXtoY {
			get {
				if (Model != null) return Model.FixXtoY;
				else return false;
			}
			set {
				if (Model != null) Model.FixXtoY = value;
				ResetRange();
			}
		}
		/// <summary>
		/// If true x-rasterlines are drawn. The default value is <c>true</c>.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(true)]
		public bool xRaster {
			get {
				if (Model != null) return Model.x.raster;
				else return true;
			}
			set {
				if (Model != null) {
					Model.x.raster = value;
					Model.Invalidate();
				}
			}
		}
		/// <summary>
		/// If true y-rasterlines are drawn. The default value is <c>true</c>.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(true)]
		public bool yRaster {
			get {
				if (Model != null) return Model.y.raster;
				else return true;
			}
			set {
				if (Model != null) {
					Model.y.raster = value;
					Model.Invalidate();
				}
			}
		}
		/// <summary>
		/// If true a legend is drawn. The default value is <c>false</c>.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(false)]
		public bool Legend {
			get {
				if (Model != null) return Model.Legend;
				else return false;
			}
			set {
				if (Model != null) {
					Model.Legend = value;
					Model.Invalidate();
				}
			}
		}
		/// <summary>
		/// If true, the x-scale is drawn. The default value is <c>true</c>.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(true)]
		public bool xScale {
			get {
				if (Model != null) return Model.x.scale;
				else return true;
			}
			set {
				if (Model != null) {
					Model.x.scale = value;
					Model.Invalidate();
				}
			}
		}
		/// <summary>
		/// If true, the y-scale is drawn. The default value is <c>true</c>.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(true)]
		public bool yScale {
			get {
				if (Model != null) return Model.y.scale;
				else return true;
			}
			set {
				if (Model != null) {
					Model.y.scale = value;
					Model.Invalidate();
				}
			}
		}
		/// <summary>
		/// If true, a z-scale is drawn. The z-scale is only drawn if a corresponding 2D-function is displayed. The default value is <c>true</c>.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(true)]
		public bool zScale {
			get {
				if (Model != null) return Model.z.scale;
				else return true;
			}
			set {
				if (Model != null) {
					Model.z.scale = value;
					Model.Invalidate();
				}
			}
		}
		/// <summary>
		/// If true, the x-grid is drawn. The default value is <c>false</c>.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(false)]
		public bool xGrid {
			get {
				if (Model != null) return Model.x.grid;
				else return false;
			}
			set {
				if (Model != null) {
					Model.x.grid = value;
					Model.Invalidate();
				}
			}
		}
		/// <summary>
		/// If true, a y-grid is drawn. The default value is <c>false</c>.
		/// </summary>
		[Browsable(true), Category("Appearance"), DefaultValue(false)]
		public bool yGrid {
			get {
				if (Model != null) return Model.y.grid;
				else return false;
			}
			set {
				if (Model != null) {
					Model.y.grid = value;
					Model.Invalidate();
				}
			}
		}
		#endregion

		/// <summary>
		/// Returns a System.Drawing.Printing.PrintDocument. You can use this PrintDocument to print the control to a printer.
		/// </summary>
		/// <code>
		/// plotControl plot;
		/// System.Windows.Forms.PrintDialog dialog = new System.Windows.Forms.PrintDialog();
		/// System.Drawing.Printing.PrintDocument doc = plot.GetPrintDocument();
		/// dialog.Document = doc;
		/// DialogResult res = printDialog.ShowDialog();
		/// if (res == DialogResult.OK) {
		///   doc.Print();
		/// }
		/// </code>
		public PlotPrintDocument GetPrintDocument() {
			return new PlotPrintDocument(this);
		}
		/// <summary>
		/// Sets the witdh of the main raster lines in the x-, y- and z-scale. Throws a
		/// <c>System.ArgumentOutOfRangeException</c> if the arguments are invalid.
		/// </summary>
		public void SetRaster(double rx, double ry, double rz) 
		{
			if (rx <= 0 || ry <= 0 || rz <= 0) throw new System.ArgumentOutOfRangeException();
			Model.x.r = rx;
			Model.y.r = ry;
			Model.z.r = rz;
			Model.Invalidate();
		}

		/// <summary>
		/// Sets the displayed range in the plotting area of the control.
		/// </summary>
		/// <param name="x0">The right border of the plotting area.</param>
		/// <param name="x1">The left border of the plotting area.</param>
		/// <param name="y0">The lower border of the plotting area.</param>
		/// <param name="y1">The upper border of the plotting area.</param>
		/// <param name="z0">The lower z-range of the plotting area.</param>
		/// <param name="z1">The upper z-range of the plotting area.</param>
		public void SetRange(double x0, double x1, double y0, double y1, double z0, double z1) {
			try {
				Model.SetRange(x0, x1, y0, y1, z0, z1, Plot.Bounds);
			} catch { MessageBox.Show("Error setting plot range"); }
		}

		/// <summary>
		/// Sets the displayed range in the plotting area of the control.
		/// </summary>
		/// <param name="x0">The right border of the plotting area.</param>
		/// <param name="x1">The left border of the plotting area.</param>
		/// <param name="y0">The lower border of the plotting area.</param>
		/// <param name="y1">The upper border of the plotting area.</param>
		public void SetRange(double x0, double x1, double y0, double y1) {
			SetRange(x0, x1, y0, y1, Model.z0, Model.z1);
		}

		/// <summary>
		/// Sets the displayed range accoring to the property <c>FixXtoY</c>.
		/// </summary>
		public void ResetRange() {
			SetRange(x0, x1, y0, y1, z0, z1);
		}
		/// <summary>
		/// Paints the control.
		/// </summary>
		/// <param name="g">The Graphics object to paint to.</param>
		public void Draw(Graphics g) {
			try {
				Rectangle bounds = new Rectangle(0, 0, Bounds.Width, Bounds.Height);
				Plot.Draw(g, bounds);
			} catch { MessageBox.Show("Error drawing plot"); }
		}
		/// <summary>
		/// Handles the Invalidate event
		/// </summary>
		public void HandleInvalidate() {
			if (Plot.PlotType != Model.PlotType) {
				Model = Model;
			}
			Invalidate();
		}
		/// <summary>
		/// Handles the Update event
		/// </summary>
		public void HandleUpdate(Item x) { Invalidate(); }
		/// <summary>
		/// Handles the Add event
		/// </summary>
		public void HandleAdd(Item x) { Invalidate(); }
		/// <summary>
		/// Handles the Remove event
		/// </summary>
		public void HandleRemove(Item x) { Invalidate(); }
		/// <summary>
		/// Handles the Replace event
		/// </summary>
		public void HandleReplace(Item o, Item n) { Invalidate(); }
		/// <summary>
		/// Handles the Reorder event
		/// </summary>
		public void HandleReorder(ItemList order) { Invalidate(); }
		/// <summary>
		/// Returns false, so the control won't be serialized.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool SerializeHandler { get { return false; } }

		/// <summary>
		/// Paints the control. Inherited from System.Windows.Forms.Control.
		/// </summary>
		protected override void OnPaint(PaintEventArgs pe) {
			pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			Draw(pe.Graphics);
		}
		/// <summary>
		/// Handles the OnMouseDown event
		/// </summary>
		protected override void OnMouseDown(MouseEventArgs e) {
			try {
				Plot.OnMouseDown(new Plot.MouseEventArgs(e, ModifierKeys));
			} catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); } 
		}
		/// <summary>
		/// Handles the OnMouseMove event
		/// </summary>
		protected override void OnMouseMove(MouseEventArgs e) {
			try {
				Plot.OnMouseMove(new Plot.MouseEventArgs(e, ModifierKeys));
			} catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); } 
		}
		/// <summary>
		/// Handles the OnMouseUp event
		/// </summary>
		protected override void OnMouseUp(MouseEventArgs e) {
			try {
				Plot.OnMouseUp(new Plot.MouseEventArgs(e, ModifierKeys));
			} catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); } 
		}
		/// <summary>
		/// Handles the OnMouseWheel event
		/// </summary>
		protected override void OnMouseWheel(MouseEventArgs e) {
			try {
				Plot.OnMouseWheel(new Plot.MouseEventArgs(e, ModifierKeys));
			} catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); } 
		}
		/// <summary>
		/// Handles the OnResize event
		/// </summary>
		protected override void OnResize(EventArgs e) {
			try {
				Plot.OnResize(e);
			} catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); } 
		}
		/// <summary>
		/// Saves the Plot of the PlotControl to an image file.
		/// </summary>
		/// <param name="filename">The filename</param>
		/// <param name="format">The desired image format</param>
		public void SaveAsImage(string filename, ImageFormat format) {
			if (format == ImageFormat.Icon || format == ImageFormat.MemoryBmp ||
				format == ImageFormat.Wmf) throw new NotSupportedException("PlotControl.SaveAsImage: image format not supported.");
			if (format == ImageFormat.Emf) {
				Graphics g = CreateGraphics();
				Plot.SaveAsMetafile(g, filename, Bounds);
			} else {
				Plot.SaveAsImage(filename, Bounds, format);
			}
		}
		/// <summary>
		/// Saves the PlotControl in an image file with the image format specified by the filename extension.
		/// </summary>
		public void SaveAsImage(string filename) {
			ImageFormat fmt = Plot.GetImageFormat(filename);
			SaveAsImage(filename, fmt);
		}

		private void InitializeComponent() {
			this.SuspendLayout();
			// 
			// PlotControl
			// 
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.ResumeLayout(false);

		}

	}
}
