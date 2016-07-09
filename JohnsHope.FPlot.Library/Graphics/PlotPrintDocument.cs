using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;

namespace JohnsHope.FPlot.Library
{
	/// <summary>
	/// A class that can be used to print the <see cref="PlotControl">PlotControl</see>.
	/// </summary>
	[ToolboxBitmap(typeof(resfinder), "JohnsHope.FPlot.Library.Resources.Printer.ico")]
	public class PlotPrintDocument: PrintDocument {
		private PlotControl plot;
		private PlotModel model;
		private bool useFullPage;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public PlotPrintDocument(PlotControl plot) {
			this.plot = plot;
		}
		/// <summary>
		/// Indicates if the Graph should cover the full page or should be printed with the current width/height ratio. 
		/// </summary>
		public bool UseFullPage {
			get { return useFullPage; }
			set { useFullPage = value; }
		}
		/// <summary>
		/// This method is called when printing starts. It creates a local copy of the
		/// <see cref="PlotModel">PlotModel</see> from the plot that was passed to the constructor. 
		/// </summary>
		protected override void OnBeginPrint(PrintEventArgs e) {
			base.OnBeginPrint (e);
			model = (PlotModel)plot.Model.Clone();
		}

		// TODO Printing resolution
		/// <summary>
		/// This method is called to print an individual page.
		/// </summary>
		protected override void OnPrintPage(System.Drawing.Printing.PrintPageEventArgs e) {
			PlotControl printControl = new PlotControl();
			Rectangle b = e.MarginBounds;
			Graphics g = e.Graphics;
			g.SetClip(b);
			g.TranslateTransform(b.X, b.Y);
			if (useFullPage) {
				printControl.Bounds = new Rectangle(0, 0, b.Width, b.Height);
			}  else {
				float s;
				printControl.Bounds = new Rectangle(0, 0, plot.Width, plot.Height);
				if (b.Width*plot.Height < b.Height*plot.Width) s = (float)b.Width/plot.Width;
				else s = (float)b.Height/plot.Height;
				g.ScaleTransform(s, s);
			}
			printControl.Model = model;
			printControl.SetRange(printControl.x0, printControl.x1, printControl.y0, printControl.y1);
			printControl.SynchDraw = true; // instruct to printControl to draw synchronously, not on a background thread.
			g.SmoothingMode = SmoothingMode.HighQuality;
			printControl.Draw(g);
			printControl.Dispose();
		}
	}
}