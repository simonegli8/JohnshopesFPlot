using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using JohnsHope.FPlot.Library;
using JohnsHope.FPlot;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;

namespace JohnsHope.FPlot
{

	/// <summary>
	/// Summary description for MarquardtForm.
	/// </summary>
	public class FitForm: System.Windows.Forms.Form, IItemEventHandler {

		class DropDownListItem<T> where T: Item  {
			public T Item;

			public DropDownListItem(T item) {
				Item = item;
			}

			public DropDownListItem(Item item) {
				Item = (T)item;
			}

			public override string ToString() {
				if (Item != null) return Item.Name;
				else return string.Empty;
			}

			public static DropDownListItem<T> ListItem(IEnumerable collection, T item) {
				foreach (DropDownListItem<T> x in collection) {
					if (x.Item == item) return x;
				}
				return null;
			}
		}
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private MainModel model;
		private Function1DItem f = null, oldf = null;
		private DataItem dataItem = null;
		private CovarianceForm covarForm;
		private SourceGrid2.Grid grid;
		private Fit fit;
		private int plength = 0;
		//private bool animate;
		//private int oldStepInterval;
		const int ProgressInterval = 125;
		private ToolStrip toolStrip1;
		private ToolStrip toolStrip2;
		private ToolStripButton toolStripButton1;
		private ToolStripButton toolStripButton2;
		private ToolStripDropDownButton algorithmMenu;
		private ToolStripMenuItem marquardtAlgorithm;
		private ToolStripMenuItem nelderMeadAlgorithm;
		private ToolStrip toolStrip3;
		private ToolStripButton start;
		private ToolStripButton stop;
		private ToolStripLabel toolStripLabel2;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripButton cancel;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripButton undo;
		private ToolStripButton redo;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripButton covariance;
		private ToolStripSeparator toolStripSeparator4;
		private ToolStripButton chisq;
		private ToolStripButton Q;
		private ToolStripButton neval;
		private ToolStripButton helpButton;
		private ToolStripSeparator toolStripSeparator5;

		private delegate void InvokeHandler(Fit fit, bool isCompleted);
		private InvokeHandler invoke;
		private ToolStripComboBox function;
		private ToolStripComboBox data;
		private ToolStripDropDownButton progress;
		private ToolStripMenuItem full;
		private ToolStripMenuItem moderate;
		private ToolStripMenuItem none;
		
		const int ChiSquareWidth = 15;
		const int ModerateStepInterval = 5000; // 5 seconds
		int step = 1, interval;
		int notifyLevel = 0;

		public FitForm(MainModel model)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.model = model;
			covarForm = new CovarianceForm();

			fit = new Fit();
			fit.Step += Step;
			fit.EnableUndo = true;
			fit.UseThreadPool = false;
			fit.Algorithm = Fit.FitAlgorithm.Marquardt;
			fit.StepInterval = 250;
			interval = ModerateStepInterval / fit.StepInterval;
			Fitp = new bool[0];
			//grid.Validating += new CancelEventHandler(gridValidating);

			ResetAlgorithm();
			ResetProgress();

			invoke = new InvokeHandler(StepInvoke);
			if (this.GetType().IsSerializable) throw new Exception("FitForm must not be serializable");
			model.Items.Handlers += this;

			Reset();

			function.SelectedIndexChanged += parChanged;
			data.SelectedIndexChanged += parChanged;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
				model.Items.Handlers -= this;
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FitForm));
			this.grid = new SourceGrid2.Grid();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.start = new System.Windows.Forms.ToolStripButton();
			this.stop = new System.Windows.Forms.ToolStripButton();
			this.cancel = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.undo = new System.Windows.Forms.ToolStripButton();
			this.redo = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.helpButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.covariance = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.neval = new System.Windows.Forms.ToolStripButton();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.function = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.data = new System.Windows.Forms.ToolStripComboBox();
			this.algorithmMenu = new System.Windows.Forms.ToolStripDropDownButton();
			this.marquardtAlgorithm = new System.Windows.Forms.ToolStripMenuItem();
			this.nelderMeadAlgorithm = new System.Windows.Forms.ToolStripMenuItem();
			this.progress = new System.Windows.Forms.ToolStripDropDownButton();
			this.full = new System.Windows.Forms.ToolStripMenuItem();
			this.moderate = new System.Windows.Forms.ToolStripMenuItem();
			this.none = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip3 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this.chisq = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.Q = new System.Windows.Forms.ToolStripButton();
			this.toolStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.toolStrip3.SuspendLayout();
			this.SuspendLayout();
			// 
			// grid
			// 
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.AutoSizeMinHeight = 10;
			this.grid.AutoSizeMinWidth = 10;
			this.grid.AutoStretchColumnsToFitWidth = false;
			this.grid.AutoStretchRowsToFitHeight = false;
			this.grid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.grid.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None;
			this.grid.CustomSort = false;
			this.grid.GridToolTipActive = true;
			this.grid.Location = new System.Drawing.Point(0, 27);
			this.grid.Margin = new System.Windows.Forms.Padding(2);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(380, 374);
			this.grid.SpecialKeys = ((SourceGrid2.GridSpecialKeys)(((((((((((SourceGrid2.GridSpecialKeys.Ctrl_C | SourceGrid2.GridSpecialKeys.Ctrl_V) 
            | SourceGrid2.GridSpecialKeys.Ctrl_X) 
            | SourceGrid2.GridSpecialKeys.Delete) 
            | SourceGrid2.GridSpecialKeys.Arrows) 
            | SourceGrid2.GridSpecialKeys.Tab) 
            | SourceGrid2.GridSpecialKeys.PageDownUp) 
            | SourceGrid2.GridSpecialKeys.Enter) 
            | SourceGrid2.GridSpecialKeys.Escape) 
            | SourceGrid2.GridSpecialKeys.Control) 
            | SourceGrid2.GridSpecialKeys.Shift)));
			this.grid.TabIndex = 23;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.start,
            this.stop,
            this.cancel,
            this.toolStripSeparator2,
            this.undo,
            this.redo,
            this.toolStripSeparator3,
            this.helpButton,
            this.toolStripSeparator5,
            this.covariance,
            this.toolStripSeparator4,
            this.neval});
			this.toolStrip1.Location = new System.Drawing.Point(0, 426);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(380, 27);
			this.toolStrip1.TabIndex = 27;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// start
			// 
			this.start.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.start.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_Run;
			this.start.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.start.Name = "start";
			this.start.Size = new System.Drawing.Size(23, 24);
			this.start.Text = "toolStripButton3";
			this.start.ToolTipText = "Start fitting";
			this.start.Click += new System.EventHandler(this.startClick);
			// 
			// stop
			// 
			this.stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.stop.Image = global::JohnsHope.FPlot.Properties.Resources.Icon_Pause;
			this.stop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.stop.Name = "stop";
			this.stop.Size = new System.Drawing.Size(23, 24);
			this.stop.Text = "toolStripButton4";
			this.stop.ToolTipText = "Stop fitting";
			this.stop.Click += new System.EventHandler(this.abortClick);
			// 
			// cancel
			// 
			this.cancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cancel.Image = global::JohnsHope.FPlot.Properties.Resources.delete;
			this.cancel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(23, 24);
			this.cancel.Text = "toolStripButton5";
			this.cancel.ToolTipText = "Exit";
			this.cancel.Click += new System.EventHandler(this.closeClick);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
			// 
			// undo
			// 
			this.undo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.undo.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditUndoIcon;
			this.undo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.undo.Name = "undo";
			this.undo.Size = new System.Drawing.Size(23, 24);
			this.undo.Text = "toolStripButton6";
			this.undo.ToolTipText = "Undo last fit step";
			this.undo.Click += new System.EventHandler(this.UndoClick);
			// 
			// redo
			// 
			this.redo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.redo.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditRedoIcon;
			this.redo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.redo.Name = "redo";
			this.redo.Size = new System.Drawing.Size(23, 24);
			this.redo.Text = "toolStripButton7";
			this.redo.ToolTipText = "Redo last fit step";
			this.redo.Click += new System.EventHandler(this.RedoClick);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
			// 
			// helpButton
			// 
			this.helpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.helpButton.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuHelpHelpTopicsIcon;
			this.helpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.helpButton.Name = "helpButton";
			this.helpButton.Size = new System.Drawing.Size(23, 24);
			this.helpButton.Text = "toolStripButton12";
			this.helpButton.ToolTipText = "Show Help...";
			this.helpButton.Click += new System.EventHandler(this.helpClick);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
			// 
			// covariance
			// 
			this.covariance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.covariance.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_PlusMinus;
			this.covariance.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.covariance.Name = "covariance";
			this.covariance.Size = new System.Drawing.Size(23, 24);
			this.covariance.Text = "toolStripButton8";
			this.covariance.ToolTipText = "Show Covariance Matrix";
			this.covariance.Click += new System.EventHandler(this.covarClick);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
			// 
			// neval
			// 
			this.neval.Font = new System.Drawing.Font("Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.neval.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_NOfF;
			this.neval.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.neval.Name = "neval";
			this.neval.Size = new System.Drawing.Size(37, 24);
			this.neval.Text = "0";
			this.neval.ToolTipText = "Number of Function Evaluations";
			// 
			// toolStrip2
			// 
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.function,
            this.toolStripButton2,
            this.data,
            this.algorithmMenu,
            this.progress});
			this.toolStrip2.Location = new System.Drawing.Point(0, 0);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(380, 25);
			this.toolStrip2.TabIndex = 28;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_Function;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "toolStripButton1";
			this.toolStripButton1.ToolTipText = "Function to Fit for";
			// 
			// function
			// 
			this.function.AutoToolTip = true;
			this.function.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.function.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.function.Name = "function";
			this.function.Size = new System.Drawing.Size(130, 25);
			this.function.ToolTipText = "Function to Fit for";
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_Data;
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton2.Text = "toolStripButton2";
			this.toolStripButton2.ToolTipText = "Data to Fit to";
			// 
			// data
			// 
			this.data.AutoToolTip = true;
			this.data.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.data.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.data.Name = "data";
			this.data.Size = new System.Drawing.Size(130, 25);
			this.data.ToolTipText = "Data to  Fit to";
			// 
			// algorithmMenu
			// 
			this.algorithmMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.algorithmMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.marquardtAlgorithm,
            this.nelderMeadAlgorithm});
			this.algorithmMenu.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuLayersLayerPropertiesIcon;
			this.algorithmMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.algorithmMenu.Name = "algorithmMenu";
			this.algorithmMenu.Size = new System.Drawing.Size(29, 22);
			this.algorithmMenu.Text = "toolStripDropDownButton1";
			this.algorithmMenu.ToolTipText = "Fit Algorithm";
			// 
			// marquardtAlgorithm
			// 
			this.marquardtAlgorithm.Checked = true;
			this.marquardtAlgorithm.CheckState = System.Windows.Forms.CheckState.Checked;
			this.marquardtAlgorithm.Name = "marquardtAlgorithm";
			this.marquardtAlgorithm.Size = new System.Drawing.Size(247, 22);
			this.marquardtAlgorithm.Text = "Levenberg-Marquardt Algorithm";
			this.marquardtAlgorithm.Click += new System.EventHandler(this.AlgorithmClick);
			// 
			// nelderMeadAlgorithm
			// 
			this.nelderMeadAlgorithm.Name = "nelderMeadAlgorithm";
			this.nelderMeadAlgorithm.Size = new System.Drawing.Size(247, 22);
			this.nelderMeadAlgorithm.Text = "Nelder Mead Algorithm";
			this.nelderMeadAlgorithm.Click += new System.EventHandler(this.AlgorithmClick);
			// 
			// progress
			// 
			this.progress.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.progress.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.full,
            this.moderate,
            this.none});
			this.progress.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_Progress;
			this.progress.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.progress.Name = "progress";
			this.progress.Size = new System.Drawing.Size(29, 22);
			this.progress.Text = "toolStripDropDownButton1";
			this.progress.ToolTipText = "Show Progress Options";
			// 
			// full
			// 
			this.full.AutoToolTip = true;
			this.full.CheckOnClick = true;
			this.full.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_Slow_Progress;
			this.full.Name = "full";
			this.full.Size = new System.Drawing.Size(205, 22);
			this.full.Text = "Show Full Progress";
			this.full.ToolTipText = "Updates all Views after every Fitting Step. Fitting will be very slow.";
			this.full.Click += new System.EventHandler(this.ProgressClick);
			// 
			// moderate
			// 
			this.moderate.AutoToolTip = true;
			this.moderate.CheckOnClick = true;
			this.moderate.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_Moderate_Progress;
			this.moderate.Name = "moderate";
			this.moderate.Size = new System.Drawing.Size(205, 22);
			this.moderate.Text = "Show Moderate Progress";
			this.moderate.ToolTipText = "Updates all Views every 5 Seconds.";
			this.moderate.Click += new System.EventHandler(this.ProgressClick);
			// 
			// none
			// 
			this.none.AutoToolTip = true;
			this.none.CheckOnClick = true;
			this.none.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_Progress;
			this.none.Name = "none";
			this.none.Size = new System.Drawing.Size(205, 22);
			this.none.Text = "Show No Progress";
			this.none.ToolTipText = "Fitting operates at full Speed, but no visual Feedback is shown.";
			this.none.Click += new System.EventHandler(this.ProgressClick);
			// 
			// toolStrip3
			// 
			this.toolStrip3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.chisq,
            this.toolStripSeparator1,
            this.Q});
			this.toolStrip3.Location = new System.Drawing.Point(0, 399);
			this.toolStrip3.Name = "toolStrip3";
			this.toolStrip3.Size = new System.Drawing.Size(380, 27);
			this.toolStrip3.TabIndex = 29;
			this.toolStrip3.Text = "toolStrip3";
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size(0, 24);
			// 
			// chisq
			// 
			this.chisq.Font = new System.Drawing.Font("Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.chisq.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_Chisquare;
			this.chisq.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.chisq.Name = "chisq";
			this.chisq.Size = new System.Drawing.Size(37, 24);
			this.chisq.Text = "0";
			this.chisq.ToolTipText = "Chisquare";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
			// 
			// Q
			// 
			this.Q.Font = new System.Drawing.Font("Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.Q.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_Q;
			this.Q.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.Q.Name = "Q";
			this.Q.Size = new System.Drawing.Size(37, 24);
			this.Q.Text = "0";
			this.Q.ToolTipText = "Q (Goodness of Fit)";
			// 
			// FitForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(380, 453);
			this.Controls.Add(this.toolStrip3);
			this.Controls.Add(this.toolStrip2);
			this.Controls.Add(this.grid);
			this.Controls.Add(this.toolStrip1);
			this.HelpButton = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MinimumSize = new System.Drawing.Size(396, 379);
			this.Name = "FitForm";
			this.Text = "Fit";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.toolStrip3.ResumeLayout(false);
			this.toolStrip3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private bool[] Fitp {
			get { return fit.Fitp; }
			set { fit.Fitp = value; }
		}

		private float[,] CovarianceMatrix {
			get {
				lock (fit) {
					if (fit.IsCompleted) return fit.CovarianceMatrix;
					else return null;
				}
			}
		}

		private string Fixed(int digits, float x) {
			string str = x.ToString(CultureInfo.InvariantCulture.NumberFormat);
			if (str.Length > digits) return str;
			digits = 2*digits + 1;
			foreach (char c in str) {
				if (char.IsDigit(c) || c == '+' || c == '-' || c == 'E' || c == 'D' || c == 'F') digits -= 2;
				if (c == '.' || c == ',') digits--;
			}
			char[] s = new char[str.Length + digits];
			str.CopyTo(0, s, 0, str.Length);
			while (--digits > 0) s[str.Length + digits] = ' ';
			return new string(s);
		}

		void ResetFitp() {
			lock(this) {
				if (f != null && f != oldf) {
					Fitp = new bool[f.p.Length];
					for (int i = 0; i < f.p.Length; i++) {
						Fitp[i] = true;
					}
				}
			}
		}
		
		void ResetGrid() {
			lock(this) {
				if (f != null) {
					if (f != oldf || f.Modified) {
						grid.ColumnsCount = 4;
						grid.RowsCount = f.p.Length + 1;
						grid[0, 0] = new SourceGrid2.Cells.Real.ColumnHeader("n");
						grid[0, 1] = new SourceGrid2.Cells.Real.ColumnHeader("fit");
						grid[0, 2] = new SourceGrid2.Cells.Real.ColumnHeader("p[n]");
						grid[0, 3] = new SourceGrid2.Cells.Real.ColumnHeader("±Δp[n]");
						for (int i = 0; i < f.p.Length; i++) {
							grid[i+1, 0] = new SourceGrid2.Cells.Real.RowHeader(i.ToString());
							grid[i+1, 1] = new SourceGrid2.Cells.Real.CheckBox(Fitp[i]);
							grid[i+1, 2] = new FpCell(f, i, model);
							if (CovarianceMatrix == null) {
								grid[i+1, 3] = new SourceGrid2.Cells.Real.Cell("", typeof(string));
							} else {
								grid[i+1, 3] = new SourceGrid2.Cells.Real.Cell(Math.Sqrt(CovarianceMatrix[i, i]), typeof(double));
							}
							grid[i+1, 3].DataModel.EnableEdit = false;
						}
					}

					plength = f.p.Length;
				}
				if (f == null) {
					grid.RowsCount = 1;
					grid.ColumnsCount = 4;
					grid[0, 0] = new SourceGrid2.Cells.Real.ColumnHeader("n");
					grid[0, 1] = new SourceGrid2.Cells.Real.ColumnHeader("fit");
					grid[0, 2] = new SourceGrid2.Cells.Real.ColumnHeader("p[n]");
					grid[0, 3] = new SourceGrid2.Cells.Real.ColumnHeader("±Δp[n]");
					Fitp = new bool[0];

				}
				grid.AutoSize();
			}
		}

		public void ResetPar() {
			lock(this) {
				if (function.SelectedItem != null) f = ((DropDownListItem<Function1DItem>)function.SelectedItem).Item;
				else f = null;

				if (data.SelectedItem != null) dataItem = ((DropDownListItem<DataItem>)data.SelectedItem).Item;
				else dataItem = null;
			}
			if (fit.Function != f || fit.Data != dataItem) {
				fit.Function = f;
				fit.Data = dataItem;
				ResetFitp();
				ResetGrid();
				lock (this) {
					Q.Text = "0";
					chisq.Text = Fixed(ChiSquareWidth, 0);
					neval.Text = "0";
					oldf = f;
				}
				ResetButtons();
			}
		}

		void ResetErr() {
			lock(this) {
				for (int i = 0; i < f.p.Length; i++) {
					if (CovarianceMatrix != null) grid[i+1, 3].Value = Math.Sqrt(Math.Abs(CovarianceMatrix[i, i]));
					else grid[i+1, 3].Value = null;
				}
				grid.AutoSize();
			}
		}

		void ResetP() {
			lock (this) {
				for (int i = 0; i < f.p.Length; i++) {
					grid[i+1, 1].Value = Fitp[i];
					grid[i+1, 2].Value = f.p[i];
					if (CovarianceMatrix != null) grid[i+1, 3].Value = Math.Sqrt(Math.Abs(CovarianceMatrix[i, i]));
					else grid[i+1, 3].Value = null;
				}
				grid.AutoSize();
			}
		}

		void ResetButtons() {
			lock (this) {
				bool fitpok = false;
				for (int i = 0; i < Fitp.Length; i++) {
					if (Fitp[i]) fitpok = true;
				}
				start.Enabled = f != null && dataItem != null && fitpok && fit.IsCompleted;
				undo.Enabled = fit.CanUndo;
				redo.Enabled = fit.CanRedo;
				covariance.Enabled = fit.NEval > 0;
				stop.Enabled = !fit.IsCompleted;
			}
		}

		void ResetLists() {
			lock(this) {

				notifyLevel++;
				Function1DItem f;
				if (function.SelectedItem != null) f = ((DropDownListItem<Function1DItem>)function.SelectedItem).Item;
				else f = null;

				DataItem d;
				if (data.SelectedItem != null) d = ((DropDownListItem<DataItem>)data.SelectedItem).Item;
				else d = null;
 
				function.Items.Clear();
				data.Items.Clear();
				foreach (Item x in model.Items) {
					if ((x is Function1DItem) && ((Function1DItem)x).Fitable) {
						function.Items.Add(new DropDownListItem<Function1DItem>(x));
					}
					if (x is DataItem) {
						data.Items.Add(new DropDownListItem<DataItem>(x));
					}
				}

				DropDownListItem<DataItem> selectedd = DropDownListItem<DataItem>.ListItem(data.Items, d); 
				if (selectedd == null && data.Items.Count > 0) selectedd = (DropDownListItem<DataItem>)data.Items[0];

				DropDownListItem<Function1DItem> selectedf = DropDownListItem<Function1DItem>.ListItem(function.Items, f);
				if (selectedf == null && function.Items.Count > 0) selectedf = (DropDownListItem<Function1DItem>)function.Items[0];

				if (data.SelectedItem != selectedd || function.SelectedItem != selectedf) {
					data.SelectedItem = selectedd;
					function.SelectedItem = selectedf;
					ResetPar();
				}

				notifyLevel--;
			}
		}

		public void ResetAlgorithm() {
			marquardtAlgorithm.Checked = fit.Algorithm == Fit.FitAlgorithm.Marquardt;
			nelderMeadAlgorithm.Checked = fit.Algorithm == Fit.FitAlgorithm.NelderMead;
		}

		public void ResetProgress() {
			full.Checked = moderate.Checked = none.Checked = false;
			if (interval == int.MaxValue) {
				none.Checked = true;
				progress.Image = Properties.Resources.Icons_Progress;
			} else if (interval == 1) {
				full.Checked = true;
				progress.Image = Properties.Resources.Icons_Slow_Progress;
			} else {
				moderate.Checked = true;
				progress.Image = Properties.Resources.Icons_Moderate_Progress;
			}
		}

		public void Reset() {
			ResetAlgorithm();
			ResetLists();
			ResetPar();
			ResetButtons();
		}

		public Function1DItem Function {
			get { return f; }
			set {
				if (f != value && value != null) {
					DropDownListItem<Function1DItem> item = DropDownListItem<Function1DItem>.ListItem(function.Items, value);
					if (item != null) function.SelectedItem = item;
				}
			}
		}

		public DataItem Data {
			get { return dataItem; }
			set {
				if (dataItem != value && value != null) {
					DropDownListItem<DataItem> item = DropDownListItem<DataItem>.ListItem(data.Items, value);
					if (item != null) data.SelectedItem = item;
				}
			}
		}

		private void AlgorithmChanged(object sender, EventArgs e) {
			if ((sender == marquardtAlgorithm) == marquardtAlgorithm.Checked)  fit.Algorithm = Fit.FitAlgorithm.Marquardt;
			else fit.Algorithm = Fit.FitAlgorithm.NelderMead;
			ResetAlgorithm();
		}

		private void UpdateForm() {
			model.Items.Update(f);
			ResetButtons();
			ResetP();

			if (fit.NEval == 0) {
				Q.Text = neval.Text = "0";
				chisq.Text = Fixed(ChiSquareWidth, 0);
			} else {
				Q.Text = fit.Q.ToString();
				chisq.Text = Fixed(ChiSquareWidth, fit.ChiSquare);
				neval.Text = fit.NEval.ToString();
			}
		}

		private bool Apply() {
			bool fitpok = false;
			Fitp = new bool[f.p.Length];
			for (int i = 0; i < f.p.Length; i++) {
				Fitp[i] = (bool)grid[i + 1, 1].Value;
				fitpok = fitpok || Fitp[i];
				//f.p[i] = (double)grid[i + 1, 2].Value;
			}
			//model.Items.Update(f);
			start.Enabled = (f != null && dataItem != null && fitpok);			
			fit.Function = f;
			fit.Data = dataItem;
			fit.Fitp = Fitp;
			if (marquardtAlgorithm.Checked) fit.Algorithm = Fit.FitAlgorithm.Marquardt;
			else fit.Algorithm = Fit.FitAlgorithm.NelderMead;
			return fitpok;
		}

		private void StepInvoke(Fit fit, bool isCompleted) {
			if (isCompleted) {
				if (fit.Error && !(fit.Exception is ThreadAbortException)) {
					string msg;
					if (fit.Algorithm == Fit.FitAlgorithm.Marquardt) msg = Properties.Resources.MarquardtError;
					else msg = Properties.Resources.NelderMeadError;
					msg += " " + fit.Exception.Message;
					MessageBox.Show(msg, Properties.Resources.FitError);
				}
				for (int i = 0; i < f.p.Length; i++) {
					grid[i+1, 2].Value = f.p[i];
				}
				/*
				model.Items.Update(f); // this line must lie before the setting of covar, since it set's covar to null.
				Q.Text  = fit.Q.ToString();
				ResetButtons();
				ResetP();
				*/
				UpdateForm();
			} else {
				chisq.Text = Fixed(ChiSquareWidth, fit.ChiSquare);
				neval.Text = fit.NEval.ToString();
				if (step++ % interval == 0) {
					step  = 1;
					model.Items.Update(f);
				}
			}
		}

		private void Step(Fit fit) {
			try {
				Invoke(invoke, fit, fit.IsCompleted);
			}
			catch {	}
		}
		
		private void startClick(object sender, System.EventArgs e) {
			step = 1;
			Apply();
			
			start.Enabled = covariance.Enabled = false;
			stop.Enabled = true;
			IAsyncResult result = fit.BeginSolve(null, null);
		}

		private void closeClick(object sender, System.EventArgs e) {
			this.Hide();
		}

		private void parChanged(object sender, System.EventArgs e) {
			Debug.Assert(notifyLevel >= 0);
			if (notifyLevel == 0) ResetPar();
		}

		private void helpClick(object sender, System.EventArgs e) {
			Help.ShowHelp(this, Properties.Resources.HelpFile, "FitForm.html");
		}

		private void covarClick(object sender, System.EventArgs e) {
			if (covarForm.IsDisposed) covarForm = new CovarianceForm();
			this.Cursor = Cursors.WaitCursor;
			fit.EvalCovarianceMatrix();
			ResetErr();
			if (CovarianceMatrix != null) {
				covarForm.Reset(CovarianceMatrix);
				if (covarForm.WindowState == FormWindowState.Minimized) covarForm.WindowState = FormWindowState.Normal;
				covarForm.Show();
				covarForm.BringToFront();
			}
			this.Cursor = Cursors.Arrow;
		}

		private void UndoClick(object sender, EventArgs e) {
			if (f != null && fit.CanUndo) {
				fit.Undo();
				UpdateForm();
			}
		}

		private void RedoClick(object sender, EventArgs e) {
			if (f != null && fit.CanRedo) {
				fit.Redo();
				UpdateForm();
			}
		}

		public void HandleUpdate(Item item) {
			ResetLists();
			if (item == f) {
				ResetGrid(); oldf = f;
			}
		}

		public void HandleRemove(Item item) {
			ResetLists();
			if (item == f || item == dataItem) ResetPar();
		}

		public void HandleAdd(Item item) { ResetLists(); }

		public void HandleReplace(Item oldItem, Item newItem) {
			ResetLists();
			if (oldItem == f || oldItem == dataItem) ResetPar();
		}

		public void HandleReorder(ItemList list) { ResetLists(); }

		public void HandleInvalidate() { Reset(); }

		private void abortClick(object sender, EventArgs e) {
			fit.Abort();
		}

		private void ProgressClick(object sender, EventArgs e) {
			if (sender == full) interval = 1;
			else if (sender == moderate) interval = ModerateStepInterval / fit.StepInterval;
			else if (sender == none) interval = int.MaxValue;
			ResetProgress();
		}

		private void AlgorithmClick(object sender, EventArgs e) {
			if (sender == marquardtAlgorithm) fit.Algorithm = Fit.FitAlgorithm.Marquardt;
			else fit.Algorithm = Fit.FitAlgorithm.NelderMead;
			ResetAlgorithm();
		}

	}
}
