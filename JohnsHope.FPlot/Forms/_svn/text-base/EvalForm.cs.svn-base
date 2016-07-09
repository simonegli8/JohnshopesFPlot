using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using JohnsHope.FPlot.Library;
using System.Threading;

namespace JohnsHope.FPlot {
	public partial class EvalForm:Form, IItemEventHandler {
		MainModel Model;

		double fres = double.NaN, X, Y;
		Color color = Color.FromArgb(0, 0, 0, 0);
		FunctionItem f = new FunctionItem();
		Thread thread;
		Exception ex;

		public void HandleUpdate(Item x) { }
		/// <summary>
		/// The Item x has been deleted.
		/// </summary>
		/// <param name="x"></param>
		public void HandleRemove(Item x) { Reset(); }
		/// <summary>
		/// The Item x was added.
		/// </summary>
		public void HandleAdd(Item x) { Reset(); }
		/// <summary>
		/// Replace the Item oldItem with Item newItem.
		/// </summary>
		public void HandleReplace(Item oldItem, Item newItem) { Reset(); }
		/// <summary>
		/// The Items at position n and m are exchanged
		/// </summary>
		public void HandleReorder(ItemList order) { Reset(); }
		/// <summary>
		/// Called when a Model change has occured.
		/// </summary>
		public void HandleInvalidate() { Reset(); }

		public EvalForm(MainModel Model) {
			InitializeComponent();
			function.DropDownStyle = ComboBoxStyle.DropDownList;
			Model.Items.Handlers += this;
			this.Model = Model;

		}

		public void Reset() {
			function.Items.Clear();
			foreach (FunctionItem x in FunctionItem.FunctionItems(Model.Items)) function.Items.Add(x);
			function.Invalidate();
			if (function.Items.Count > 0) {
				function.SelectedIndex = 0;
				Evaluate.Enabled = true;
			} else {
				function.SelectedItem = null;
				Evaluate.Enabled = false;
			}
		}

		void SetRes() {
			lock(this) {
				string s = f.ToString();
				bool col = false;
				if (f is Function1DItem) s += "(x) = ";
				else if (f is Function2DItem) s += "(x, y) = ";
				else if (f is FunctionColorItem) {
					s += "(x, y) = RGBA("; col = true;
				} else s += " = ";
				if (!col)	s += fres.ToString();
				else s += color.R.ToString() + ", " + color.G.ToString() + ", " + color.B.ToString() + ", " + color.A.ToString() + ")";
				res.Text = s;
			}
		}

		void Update(object sender, EventArgs e) {
			y.Enabled = (function.SelectedItem == null || function.SelectedItem is Function2DItem || function.SelectedItem is FunctionColorItem);
			Stop();
		}

		/// <summary>
		/// A boolean value that indicates that the background thread should be stopped.
		/// </summary>
		public void Stop() {
			if (thread != null) { 
				try {	
					thread.Abort();
				} catch { }
				if (thread.ThreadState == ThreadState.AbortRequested) thread.Join(3000);
				thread = null;
				Evaluate.Enabled = true;
			}
		}

		void Stop(object sender, EventArgs e) {
			Stop();
			fres = double.NaN;
			color = Color.FromArgb(0, 0, 0, 0);
			SetRes();
		}

		public void Start(object sender, EventArgs e) {
			Stop();
			f = ((FunctionItem)function.SelectedItem);
			X  = double.NaN;
			Y = double.NaN;
			double.TryParse(x.Text, out X);
			double.TryParse(y.Text, out Y);
			if (f != null) {
				thread = new Thread(new ThreadStart(DoEval));
				thread.Name = "Evaluation Thread";
				Evaluate.Enabled = false;
				thread.Start();
				//DoEvalDebug();
				Thread.Sleep(0);
			}
		}

		delegate void EmptyDelegate();

		public void DoEval() {
			try {
				if (f is Function1DItem) {
					fres = ((Function1DItem)f).f(X);
				} else if (f is Function2DItem) {
					fres = ((Function2DItem)f).f(X, Y);
				} else if (f is FunctionColorItem) {
					color = ((FunctionColorItem)f).f(X, Y);
				}
				Invoke(new EmptyDelegate(Finished));
			} catch (ThreadAbortException) {
				fres = double.NaN;
				color = Color.FromArgb(0, 0, 0, 0);
				Invoke(new EmptyDelegate(Finished));
			} catch (Exception e) {
				fres = double.NaN;
				color = Color.FromArgb(0, 0, 0, 0);
				Invoke(new EmptyDelegate(Finished));
				if (exceptions.Checked) {
					ex = e;
					Invoke(new EmptyDelegate(Message));
				}
				//throw e.InnerException;
			}
		}

		public void DoEvalDebug() {
			try {
				if (f is Function1DItem) {
					fres = ((Function1DItem)f).f(X);
				} else if (f is Function2DItem) {
					fres = ((Function2DItem)f).f(X, Y);
				} else if (f is FunctionColorItem) {
					color = ((FunctionColorItem)f).f(X, Y);
				}
				Invoke(new EmptyDelegate(Finished));
			} catch (ThreadAbortException) {
				fres = double.NaN;
				color = Color.FromArgb(0, 0, 0, 0);
				Finished();
			} catch (Exception e) {
				fres = double.NaN;
				color = Color.FromArgb(0, 0, 0, 0);
				Finished();
				if (exceptions.Checked) {
					ex = e;
					Message();
				}
				//throw e.InnerException;
			}
		}

		void Finished() {
			SetRes();
			Evaluate.Enabled = true;
		}

		void Message() {
			ExceptionForm f = new ExceptionForm(Model, ex);
			f.Show();
		}

		private void closeClick(object sender, EventArgs e) {
			Hide();
		}

	}
}