using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// The class that holds the thread that calculates the function values in the background.
	/// </summary>
	[Serializable]
	public class PaintThread {
		private const int delay = 500;
		[NonSerialized]
		private int t;
		[NonSerialized]
		private Thread thread;
		/// <summary>
		/// A value that indicates if the calculation thread has finished.
		/// </summary>
		[NonSerialized]
		public bool DrawDone = false;
		/// <summary>
		/// If set to false the drawing of the plot is asynchronous.
		/// </summary>
		public bool SynchDraw = false;
		/// <summary>
		/// The parent control associated with the PainterThread.
		/// </summary>
		[NonSerialized]
		public Control Parent;
		/// <summary>
		/// The ProgressBar used to display the calculation progress.
		/// </summary>
		[NonSerialized]
		public ProgressBar ProgressBar;
		/// <summary>
		/// The plotting area in device coordinates.
		/// </summary>
		public Rectangle Bounds;
		/// <summary>
		/// The Model that contains the IDrawable
		/// </summary>
		public PlotModel Model;
		/// <summary>
		/// The constructor of the PaintThread.
		/// </summary>
		public PaintThread(PlotModel Model) {
			this.Model = Model;
			DrawStop = true;
			DrawDone = true;
		}
		/// <summary>
		/// Copies from another PaintThread using a deep copy.
		/// </summary>
		public void CopyFrom(PaintThread p) {
			t = p.t;
			Model = p.Model;
			Parent = p.Parent;
			this.ProgressBar = p.ProgressBar;
			progress = p.progress;
			MaxProgress = p.MaxProgress;
			DrawStop = true;
			DrawDone = true;
			SynchDraw = p.SynchDraw;
		}
		/// <summary>
		/// Creates a deep copy of the PaintThread.
		/// </summary>
		public PaintThread Clone() {
			PaintThread p = new PaintThread(Model);
			p.CopyFrom(this);
			return p;
		}
		/// <summary>
		/// The maximum number of steps.
		/// </summary>
		[NonSerialized]
		public int MaxProgress = 0;
		
		[NonSerialized]
		private int progress = -1;

		private delegate void StepHandler();
		
		private void OnStep() {
			if (this.ProgressBar != null) lock (this.ProgressBar) {
					ProgressBar.Visible = Progress >= 0;
					if (Progress >= 0) ProgressBar.Value = Progress;
				}
		}
		/// <summary>
		/// Updates the ProgressBar.Value, using <c>Parent.Invoke</c>, e.g. in a thread safe way.
		/// </summary>
		public int Progress {
			get {
				return progress;
			}
			set {
				progress = value;
				if (System.Environment.TickCount - t > delay) {
					t = System.Environment.TickCount;
					if (Parent != null && ProgressBar != null && Parent.IsHandleCreated && Parent.InvokeRequired)
						Parent.Invoke(new StepHandler(OnStep));
					else OnStep();
				}
			}
		}
		/// <summary>
		/// The main routine of the thread.
		/// </summary>
		public void DoRecalc() {
			DrawDone = false;
			t = System.Environment.TickCount;
			Progress = 0;
			bool HasChanged = false;
			Painter[] painters;
			lock (Model.Painters) {
				painters = new Painter[Model.Painters.Count];
				Model.Painters.Values.CopyTo(painters, 0);
			}
			foreach (Painter p in painters) {
				if (DrawStop) break;
				if (p != null && p.Recalc) {
					p.Calc((Plot)this);
					HasChanged = true;
				}
			}
			DrawDone = true;
			Progress = -1;
			if (!DrawStop && !SynchDraw && HasChanged) {
				Model.Modified = false;
				if (Parent != null) Parent.Invalidate();
			}
		}
		[NonSerialized]
		private bool stop = true;
		/// <summary>
		/// A boolean value that indicates that the background thread should be stopped.
		/// </summary>
		public bool DrawStop {
			get { return stop; }
			set {
				stop = value;
				if (value) {
					if (thread != null) {
						try {
							thread.Abort();
						} catch { }
						thread.Join(3000);
					}
					thread = null;
				}
			}
		}

		/// <summary>
		/// This routine starts a new PaintThread depending on wether the plotting area or the plotted
		/// items have changed.
		/// </summary>
		public void DrawStart(Plot plot) {
			bool calcall;
			Model = plot.Model;
			ProgressBar = plot.ProgressBar;
			Parent = plot.Parent;
			SynchDraw = plot.SynchDraw;
			calcall = ((Bounds.Width != plot.Bounds.Width) || (Bounds.Height != plot.Bounds.Height));
			Bounds = plot.Bounds;
			if ((Model.Modified || calcall) && Bounds.Width > 0 && Bounds.Height > 0) {
				DrawStop = true;
				if (calcall) Model.Modified = true;
				Progress = 0;
				MaxProgress = 1;
				foreach (Painter p in Model.Painters.Values) {
					if (p != null) {
						p.Modified |= p.Item.Modified;
						//TODO Item.Modified
						// p.Item.Modified = false;
						if (p.Modified) p.Start((Plot)this);
						p.Modified = false;
					}
				}
				if (this.ProgressBar != null) {
					lock (this.ProgressBar) {
						this.ProgressBar.Maximum = MaxProgress;
						this.ProgressBar.Value = 0;
					}
				}
				DrawStop = false;
				thread = new Thread(new ThreadStart(DoRecalc));
				thread.Name = "JohnsHope's FPlot Paint Thread";
				DrawDone = false;
				thread.Start();
				Thread.Sleep(0);
			}
		}
		/// <summary>
		/// Paints all Items, according to the data that was calculated by Start
		/// </summary>
		public void DrawItems(Graphics g) {
			while (SynchDraw && !DrawDone) Application.DoEvents();
			Region clip0;
			clip0 = g.Clip.Clone();
			g.IntersectClip(Bounds);
			for (int i = Model.Count-1; i >= 0; i--) {
				Painter p = Model.Painters[Model[i]];
				if (p != null) p.Paint(g, (Plot)this);
			}
			g.Clip = clip0;
		}
	}

}
