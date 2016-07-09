using System;
using System.CodeDom.Compiler;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// Describes a item that has line attributes.
	/// </summary>
	public interface ILine {
		/// <summary>
		/// The color of the line.
		/// </summary>
		Color Color { get; set;}
		/// <summary>
		/// The width of the line.
		/// </summary>
		float LineWidth { get; set;}
		/// <summary>
		/// The DashStyle of the line.
		/// </summary>
		DashStyle LineStyle { get; set;}
	}
	/// <summary>
	/// An interface that indicates that a item is a compiled instance.
	/// </summary>
	public interface IInstance { }
	/*
	/// <summary>
	/// An interface describing the arguments for drawing operations.
	/// </summary>
	public class PaintArgs {
		/// <summary>
		/// The plotting area in device coordinates.
		/// </summary>
		public Rectangle Bounds;
		/// <summary>
		/// The maximum number of steps.
		/// </summary>
		public int MaxProgress;
		private int progress = 0;
		/// <summary>
		/// The number of occured steps.
		/// </summary>
		public virtual int Progress { get { return progress; } set { progress = value; } }
		/// <summary>
		/// The Model that contains the IDrawable
		/// </summary>
		public PlotModel Model;
		private bool stop;
		/// <summary>
		/// A value that indicates that the thread should abort.
		/// </summary>
		public virtual bool DrawStop {
			get { return stop; }
			set { stop = value; }
		}
		/// <summary>
		/// A pen that can be used in drawing.
		/// </summary>
		public Pen Pen = new Pen(Color.Black);
		/// <summary>
		/// A brush that can be used for drawing.
		/// </summary>
		public Brush Brush = new SolidBrush(Color.Black);
		/// <summary>
		/// Copies from another PaintArgs with a deep copy.
		/// </summary>
		public void CopyFrom(PaintArgs src) {
			Bounds = src.Bounds;
			MaxProgress = src.MaxProgress;
			Progress = src.Progress;
			DrawStop = src.DrawStop;
		}

		/// <summary>
		/// Gets the X value in a 2D Plot from a x double value.
		/// </summary>
		public float GetX2D(double x) {
			return (float)Model.x.DeviceCoordinate(x, Bounds);
		}
		/// <summary>
		/// Gets the Y coordinate in a 2D Plot from a y double value.
		/// </summary>
		public float GetY2D(double y) {
			return (float)Model.y.DeviceCoordinate(y, Bounds);
		}
		/// <summary>
		/// Gets the x value in the original coordinate system from a X value between 0 and Width.
		/// </summary>
		public double Getx2D(int X) {
			return Model.x.WorldCoordinate(X, Bounds);
		}
		/// <summary>
		/// Gets the y value in the original coordinate system from a Y value between 0 and Height.
		/// </summary>
		public double Gety2D(int Y) {
			return Model.y.WorldCoordinate(Y, Bounds);
		}

	}
	*/


	/// <summary>
	/// Describes a painter that paints an item in a <see cref="Plot">Plot</see>.
	/// </summary>
	public class Painter {
		/// <summary>
		/// Indicates if the Item of the Painter has been modified.
		/// </summary>
		public bool Modified = true;
		/// <summary>
		/// Indicates if the painter must recalc the item's image.
		/// </summary>
		public bool Recalc;
		/// <summary>
		/// The item the Painter is painting.
		/// </summary>
		public Item Item;
		/// <summary>
		/// The PlotModel the Painter belongs to. 
		/// </summary>
		public PlotModel Model;
		/// <summary>
		/// The Range of the Plot, from (x0, y0, z0) to (x1, y1, z1)
		/// </summary>
		public double x0, y0, z0, x1, y1, z1;
		/// <summary>
		/// Constructor of a Painter.
		/// </summary>
		public Painter(PlotModel Model, Item Item) {
			this.Model = Model;
			this.Item = Item;
		}
		/// <summary>
		/// Prepares the Painter for painting. The default implementation of this routine is:
		/// <code>
		///   if (Modified) {
		///     Modified = false; Recalc = true;
		///  	}
		/// </code>
		/// </summary>
		public virtual void Start(Plot p) {
			if (Modified) {
				Modified = false;
				Recalc = true;
			}
		}
		/// <summary>
		/// Recalculates the image data in order to paint the Painter fast. This routine is called from the separate 
		/// Painter thread. If p.StopDraw gets true during execution, the recalculation can be aborted. In order to be
		/// thread safe, this routine must lock the painter e.g. with <c>lock(this)</c> upon making changes
		/// to the painter.
		/// </summary>
		public virtual void Calc(Plot p) { }
		/// <summary>
		/// Paints the calculated image data of the Painter to the Grapics g. In order to be thread safe,
		/// this routine must try to aquire a lock on the painter e.g. with either <c>Monitor.TryEnter(this)</c> or 
		/// <c>lock(this)</c> upon reading from the Painter. If the Painter is locked, the Painter is in the process of 
		/// recalculating and the Painter cannot yet be painted.
		/// </summary>
		public virtual void Paint(Graphics g, Plot p) { }
	}

}