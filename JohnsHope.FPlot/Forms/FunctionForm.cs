using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.CodeDom.Compiler;
using JohnsHope.FPlot.Library;
using JohnsHope.FPlot.Properties;
using ICSharpCode.TextEditor;
using System.Diagnostics;

namespace JohnsHope.FPlot
{

	/// <summary>
	/// Summary description for SourceForm.
	/// </summary>
	public class FunctionForm : System.Windows.Forms.Form, IResetable, IEditForm
	{
		private const string introText = "double[] p, dfdp;\n";
		private const string introCText = "double[] p, dfdp;\n";

		enum Dimensions {One, Two, TwoColor}

		private System.Windows.Forms.Label introLabel;
		private System.Windows.Forms.Label outroLabel;
		private CodeControl fSource;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ColorDialog colorDialog;
		private ParForm parForm;
		
		public FunctionItem f, oldItem;

		Dimensions dimensions;

		private Function1DItem style = new Function1DItem();

		MainModel Model;
		private System.Windows.Forms.Button colorButton;
		private Pen pen = new Pen(Color.Black, 2);
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox lineWidth;
		private ToolStrip toolStrip1;
		private ToolStripButton toolStripButton1;
		private ToolStripButton toolStripButton2;
		private ToolStripButton toolStripButton3;
		private ToolStripButton toolStripButton4;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripButton toolStripButton5;
		private ToolStripButton toolStripButton6;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripButton Button1D;
		private ToolStripButton Button2D;
		private ToolStripButton ButtonColor;
		private ToolStripSeparator toolStripSeparator4;
		private ToolStripButton StyleButton;
		private ToolStripButton ParButton;
		private ToolStripSeparator toolStripSeparator5;
		private ToolStripLabel toolStripLabel1;
		private ToolStripTextBox name;
		private ToolStripSeparator toolStripSeparator6;
		private ToolStripButton toolStripButton7;
		private ToolStripButton toolStripButton8;
		private IGradient Gradient = Gradients.List[0].Clone();

		public FunctionForm(MainModel Model, FunctionItem old)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			
			this.Model = Model;
			parForm = new ParForm(Model);
			oldItem = old;
			
			Reset();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
					components.Dispose();
				}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FunctionForm));
			this.introLabel = new System.Windows.Forms.Label();
			this.outroLabel = new System.Windows.Forms.Label();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.colorButton = new System.Windows.Forms.Button();
			this.lineWidth = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.fSource = new JohnsHope.FPlot.CodeControl();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.Button1D = new System.Windows.Forms.ToolStripButton();
			this.Button2D = new System.Windows.Forms.ToolStripButton();
			this.ButtonColor = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.StyleButton = new System.Windows.Forms.ToolStripButton();
			this.ParButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.name = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// introLabel
			// 
			this.introLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.introLabel.Font = new System.Drawing.Font("Courier New", 8.747663F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.introLabel.ForeColor = System.Drawing.Color.Blue;
			this.introLabel.Location = new System.Drawing.Point(0, 25);
			this.introLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.introLabel.Name = "introLabel";
			this.introLabel.Size = new System.Drawing.Size(551, 35);
			this.introLabel.TabIndex = 5;
			this.introLabel.Text = "double p[]\r\ndouble f(double x) {";
			// 
			// outroLabel
			// 
			this.outroLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.outroLabel.Font = new System.Drawing.Font("Courier New", 8.747663F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.outroLabel.ForeColor = System.Drawing.Color.Blue;
			this.outroLabel.Location = new System.Drawing.Point(0, 521);
			this.outroLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.outroLabel.Name = "outroLabel";
			this.outroLabel.Size = new System.Drawing.Size(551, 15);
			this.outroLabel.TabIndex = 6;
			this.outroLabel.Text = "}";
			// 
			// colorButton
			// 
			this.colorButton.Location = new System.Drawing.Point(0, 0);
			this.colorButton.Name = "colorButton";
			this.colorButton.Size = new System.Drawing.Size(75, 23);
			this.colorButton.TabIndex = 0;
			// 
			// lineWidth
			// 
			this.lineWidth.Location = new System.Drawing.Point(0, 0);
			this.lineWidth.Name = "lineWidth";
			this.lineWidth.Size = new System.Drawing.Size(100, 20);
			this.lineWidth.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 0;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(0, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 23);
			this.label3.TabIndex = 0;
			// 
			// fSource
			// 
			this.fSource.AllowDrop = true;
			this.fSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.fSource.BackColor = System.Drawing.SystemColors.Control;
			this.fSource.Location = new System.Drawing.Point(0, 55);
			this.fSource.Margin = new System.Windows.Forms.Padding(2);
			this.fSource.Name = "fSource";
			this.fSource.Size = new System.Drawing.Size(551, 469);
			this.fSource.TabIndex = 0;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton4,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripButton3,
            this.toolStripSeparator2,
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripSeparator3,
            this.Button1D,
            this.Button2D,
            this.ButtonColor,
            this.toolStripSeparator4,
            this.StyleButton,
            this.ParButton,
            this.toolStripSeparator5,
            this.toolStripLabel1,
            this.name,
            this.toolStripSeparator6,
            this.toolStripButton7,
            this.toolStripButton8});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(551, 25);
			this.toolStrip1.TabIndex = 44;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = global::JohnsHope.FPlot.Properties.Resources.ok;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "toolStripButton1";
			this.toolStripButton1.ToolTipText = "Apply Changes & Close";
			this.toolStripButton1.Click += new System.EventHandler(this.okClick);
			// 
			// toolStripButton4
			// 
			this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton4.Image = global::JohnsHope.FPlot.Properties.Resources.delete;
			this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton4.Name = "toolStripButton4";
			this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton4.Text = "toolStripButton4";
			this.toolStripButton4.ToolTipText = "Discard Changes & Close";
			this.toolStripButton4.Click += new System.EventHandler(this.cancelClick);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuFileSaveIcon;
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton2.Text = "toolStripButton2";
			this.toolStripButton2.ToolTipText = "Apply Changes";
			this.toolStripButton2.Click += new System.EventHandler(this.applyClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButton3
			// 
			this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
			this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton3.Name = "toolStripButton3";
			this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton3.Text = "toolStripButton3";
			this.toolStripButton3.ToolTipText = "Compile C# Code";
			this.toolStripButton3.Click += new System.EventHandler(this.compileClick);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButton5
			// 
			this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton5.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditUndoIcon;
			this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton5.Name = "toolStripButton5";
			this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton5.Text = "1D";
			this.toolStripButton5.ToolTipText = "Undo";
			this.toolStripButton5.Click += new System.EventHandler(this.Undo);
			// 
			// toolStripButton6
			// 
			this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton6.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditRedoIcon;
			this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton6.Name = "toolStripButton6";
			this.toolStripButton6.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton6.Text = "toolStripButton6";
			this.toolStripButton6.ToolTipText = "Redo";
			this.toolStripButton6.Click += new System.EventHandler(this.Redo);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// Button1D
			// 
			this.Button1D.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.Button1D.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_1D;
			this.Button1D.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.Button1D.Name = "Button1D";
			this.Button1D.Size = new System.Drawing.Size(23, 22);
			this.Button1D.Text = "toolStripButton7";
			this.Button1D.ToolTipText = "1-Dimensional Function";
			this.Button1D.Click += new System.EventHandler(this.Click1D);
			// 
			// Button2D
			// 
			this.Button2D.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.Button2D.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_2D;
			this.Button2D.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.Button2D.Name = "Button2D";
			this.Button2D.Size = new System.Drawing.Size(23, 22);
			this.Button2D.Text = "toolStripButton8";
			this.Button2D.ToolTipText = "2-Dimensional Function";
			this.Button2D.Click += new System.EventHandler(this.Click2D);
			// 
			// ButtonColor
			// 
			this.ButtonColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ButtonColor.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_Color2D;
			this.ButtonColor.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ButtonColor.Name = "ButtonColor";
			this.ButtonColor.Size = new System.Drawing.Size(23, 22);
			this.ButtonColor.Text = "toolStripButton9";
			this.ButtonColor.ToolTipText = "2-Dimensional Function that returns a Color";
			this.ButtonColor.Click += new System.EventHandler(this.ClickColor);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// StyleButton
			// 
			this.StyleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.StyleButton.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_LineStyle;
			this.StyleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.StyleButton.Name = "StyleButton";
			this.StyleButton.Size = new System.Drawing.Size(23, 22);
			this.StyleButton.Text = "toolStripButton10";
			this.StyleButton.ToolTipText = "Drawing Style";
			this.StyleButton.Click += new System.EventHandler(this.StyleClick);
			// 
			// ParButton
			// 
			this.ParButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ParButton.Image = global::JohnsHope.FPlot.Properties.Resources.par;
			this.ParButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ParButton.Name = "ParButton";
			this.ParButton.Size = new System.Drawing.Size(23, 22);
			this.ParButton.Text = "toolStripButton11";
			this.ParButton.ToolTipText = "Parametervalues of the array p";
			this.ParButton.Click += new System.EventHandler(this.parClick);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(42, 22);
			this.toolStripLabel1.Text = "Name:";
			// 
			// name
			// 
			this.name.Name = "name";
			this.name.Size = new System.Drawing.Size(155, 25);
			this.name.ToolTipText = "Name of the function";
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButton7
			// 
			this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton7.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuHelpHelpTopicsIcon;
			this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton7.Name = "toolStripButton7";
			this.toolStripButton7.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton7.Text = "toolStripButton7";
			this.toolStripButton7.ToolTipText = "Help";
			this.toolStripButton7.Click += new System.EventHandler(this.helpClick);
			// 
			// toolStripButton8
			// 
			this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton8.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuHelpTutorialsIcon;
			this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton8.Name = "toolStripButton8";
			this.toolStripButton8.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton8.Text = "toolStripButton8";
			this.toolStripButton8.ToolTipText = "Help on JohnsHope.FPlot.Library...";
			this.toolStripButton8.Click += new System.EventHandler(this.helpLibraryClick);
			// 
			// FunctionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(551, 535);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.fSource);
			this.Controls.Add(this.outroLabel);
			this.Controls.Add(this.introLabel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MinimumSize = new System.Drawing.Size(492, 395);
			this.Name = "FunctionForm";
			this.Text = "Edit Function";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void CreateF() {
			float lw;
			if (!float.TryParse(lineWidth.Text, out lw)) lw = 1;
			if (dimensions == Dimensions.One) {
				Function1DItem f1 = new Function1DItem();
				f1.Source = fSource.Text;
				f1.LineStyle = style.LineStyle;
				f1.LineWidth = style.LineWidth;
				f1.Color = style.Color;
				f = f1;
			} else if (dimensions == Dimensions.Two) {
				Function2DItem f1 = new Function2DItem();
				f1.Source = fSource.Text;
				f1.Gradient = Gradient;
				f = f1;
			} else {
				FunctionColorItem f1 = new FunctionColorItem();
				f1.Source = fSource.Text;
				f = f1;
			}
			f.Name = name.Text;
			this.Text = name.Text;
			if (oldItem != null) f.p = oldItem.p.Clone(); 
		}

		private void compileClick(object sender, System.EventArgs e) {
			CreateF();
			Compiler.Compile(f);
			fSource.MarkErrors(f);
		}

		void ResetDimensions() {
			Button1D.Enabled = Button2D.Enabled = ButtonColor.Enabled = StyleButton.Enabled = true;
			switch (dimensions) {
			case Dimensions.One:
				Button1D.Enabled = false;
				StyleButton.Image = Properties.Resources.Icons_LineStyle;
				introLabel.Text = introText + "double f(double x) {";
				break;
			case Dimensions.Two: Button2D.Enabled = false;
				Button2D.Enabled = false;
				StyleButton.Image = Properties.Resources.Icons_GradientToolIcon;
				introLabel.Text = introText + "double f(double x, double y) {";				
				break;
			case Dimensions.TwoColor: ButtonColor.Enabled = false;
				ButtonColor.Enabled = false;
				StyleButton.Enabled = false;
				StyleButton.Image = Properties.Resources.Icons_GradientToolIcon;
				introLabel.Text = introCText + "System.Drawing.Color f(double x, double y) {";
				break;
			}
			}

		void SetDimensions(Dimensions dim) {
			dimensions = dim;
			ResetDimensions();
		}

		public void Reset() {
			if (oldItem == null) {
				dimensions = Dimensions.One;
				fSource.Text = "return 0;";
			} else {
				if (oldItem is Function1DItem) {
					SetDimensions(Dimensions.One);
				} else if (oldItem is Function2DItem) {
					SetDimensions(Dimensions.Two);
				} else if (oldItem is FunctionColorItem) {
					SetDimensions(Dimensions.TwoColor);
				} else throw new System.ApplicationException("invalid item-type");
				name.Text = oldItem.Name;
				this.Text = oldItem.Name;
				if (oldItem is Function1DItem) {
					Function1DItem f1 = (Function1DItem)oldItem;
					fSource.Text = f1.Source;
					style.Color = f1.Color;
					style.LineWidth = f1.LineWidth;
					style.LineStyle = f1.LineStyle;
				} else if (oldItem is Function2DItem) {
					Function2DItem f1 = (Function2DItem)oldItem;
					fSource.Text = f1.Source;
					Gradient = f1.Gradient;
				} else if (oldItem is FunctionColorItem) {
					FunctionColorItem f1 = (FunctionColorItem)oldItem;
					fSource.Text = f1.Source;
				}
			}

		}

		private void Apply()
		{
			CreateF();
			Compiler.Compile(f);
			fSource.MarkErrors(f);

			if (oldItem == null || !Model.Items.Contains(oldItem)) Model.Items.Add(f);
			else Model.Items.Replace(oldItem, f);

			oldItem = f;
		}

		private void cancelClick(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		private void applyClick(object sender, System.EventArgs e)
		{
			Apply();
			Reset();
		}

		private void okClick(object sender, System.EventArgs e)
		{
			Apply();
			this.Hide();
		}

		private void dimensionChanged(object sender, System.EventArgs e) {
			ResetDimensions();
		}

		private void parClick(object sender, System.EventArgs e) {
			Apply();
			if (parForm.IsDisposed) parForm = new ParForm(Model);
			parForm.Reset(f);
			if (parForm.WindowState == FormWindowState.Minimized) parForm.WindowState = FormWindowState.Normal;
			parForm.Show();
			parForm.BringToFront();
		}

		private void helpClick(object sender, System.EventArgs e) {
			Help.ShowHelp(this, Properties.Resources.HelpFile, "FunctionForm.html");
		}

		private void StyleClick(object sender, EventArgs e) {
			if (dimensions == Dimensions.One) {
				style.Name = name.Text;
				FunctionLineStyleForm.New(Model, style);
			} else if (dimensions == Dimensions.Two) {
				Gradient = GradientForm.ShowDialog();
				Model.Items.Update(f);
			} else Debug.Fail("No Style for Color function");
		}

		private void NameChanged(object sender, EventArgs e) {
			this.Text = name.Text;
		}

		public void Edit(SourceLocation loc) {
			fSource.Edit(loc);
		}

		public void Undo(object sender, EventArgs e) {
			fSource.Editor.Undo();
		}

		public void Redo(object sender, EventArgs e) {
			fSource.Editor.Redo();
		}

		public void Click1D(object sender, EventArgs e) { SetDimensions(Dimensions.One); }
		public void Click2D(object sender, EventArgs e) { SetDimensions(Dimensions.Two); }
		public void ClickColor(object sender, EventArgs e) { SetDimensions(Dimensions.TwoColor); }

		public void helpLibraryClick(object sender, EventArgs e) {
			Help.ShowHelp(this, Properties.Resources.LibraryHelpFile);
		}

	}
}
