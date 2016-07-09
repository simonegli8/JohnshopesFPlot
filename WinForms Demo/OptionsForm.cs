using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using JohnsHope.FPlot.Library;

namespace FPlotDemo
{
	/// <summary>
	/// Summary description for OptionsForm.
	/// </summary>
	public class OptionsForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button scaleFontButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.ComboBox xFormat;
		private System.Windows.Forms.CheckBox showYScale;
		private System.Windows.Forms.CheckBox showXScale;
		private System.Windows.Forms.CheckBox showYAxis;
		private System.Windows.Forms.CheckBox showXAxis;
		private System.Windows.Forms.CheckBox showYRaster;
		private System.Windows.Forms.CheckBox showXRaster;
		private System.Windows.Forms.ComboBox yFormat;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.FontDialog scaleFontForm;
		private System.Windows.Forms.Label yFormatLabel;
		private System.Windows.Forms.Label xFormatLabel;
		private System.Windows.Forms.Label yDigitsLabel;
		private System.Windows.Forms.Label xDigitsLabel;
		private System.Windows.Forms.Button colorButton;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.CheckBox border;
		private System.Windows.Forms.CheckBox legend;
		private System.Windows.Forms.CheckBox zScale;
		private System.Windows.Forms.CheckBox legendBox;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.Label colorLabel;
		private System.Windows.Forms.CheckBox xGrid;
		private System.Windows.Forms.CheckBox yGrid;
		private System.Windows.Forms.HelpProvider helpProvider;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPage;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox lineWidth;
		private CheckBox xfix;
		private CheckBox xlog;
		private CheckBox yfix;
		private CheckBox ylog;
		private CheckBox zlog;
		private TextBox xunit;
		private Label label2;
		private TextBox yunit;
		private Label label3;
		private TextBox zunit;
		private Label label4;
		private NumericUpDown xDigits;
		private NumericUpDown yDigits;
		private Button textFontButton;
		private FontDialog unitsFontForm;
		private Button unitsFontButton;
		private FontDialog legendFontForm;
		private TabPage tabPage5;
		private CheckBox scalePage;
		private PlotControl plot;
		private Button help;

		public bool useFullPrintPage;

		public void Reset()
		{
			PlotModel m = plot.Model;
			showXScale.Checked = m.x.scale; showYScale.Checked = m.y.scale;
			showXAxis.Checked = m.x.axis; showYAxis.Checked = m.y.axis;
			showXRaster.Checked = m.x.raster; showYRaster.Checked = m.y.raster;
			xGrid.Checked = m.x.grid; yGrid.Checked = m.y.grid;
			xlog.Checked = m.xLog; ylog.Checked = m.yLog; zlog.Checked = m.zLog;
			xfix.Checked = m.x.fix; yfix.Checked = m.y.fix;
			legend.Checked = m.Legend; legendBox.Checked = m.LegendBorder;
			border.Checked = m.Border;
			lineWidth.Text = m.ScaleLineWidth.ToString();
			zScale.Checked = m.z.scale;
			xDigits.Value = m.x.digits; yDigits.Value = m.y.digits;
			xFormat.SelectedIndex = (int)m.x.style; yFormat.SelectedIndex = (int)m.y.style;
			scaleFontForm.Font = m.ScaleFont;
			unitsFontForm.Font = m.UnitsFont;
			legendFontForm.Font = m.LegendFont;
			colorDialog.Color = m.ScaleColor;
			colorLabel.BackColor = m.ScaleColor;
			if (m.x.unit == null) xunit.Text = "";
			else xunit.Text = m.x.unit;
			if (m.y.unit == null) yunit.Text = "";
			else yunit.Text = m.y.unit;
			if (m.z.unit == null) zunit.Text = "";
			else zunit.Text = m.z.unit;
			scalePage.Checked = useFullPrintPage;

			xDigits.Enabled = xFormat.Enabled = showXScale.Checked;	yDigits.Enabled = yFormat.Enabled = showYScale.Checked;
			legendBox.Enabled = legend.Checked;
		}

		private void Apply()
		{
			PlotModel m = plot.Model;
			m.x.scale = showXScale.Checked; m.y.scale = showYScale.Checked;
			m.x.axis = showXAxis.Checked; m.y.axis = showYAxis.Checked;
			m.x.raster = showXRaster.Checked; m.y.raster = showYRaster.Checked;
			m.x.grid = xGrid.Checked; m.y.grid = yGrid.Checked;
			m.Legend = legend.Checked; m.LegendBorder = legendBox.Checked;
			m.xLog = xlog.Checked; m.yLog = ylog.Checked; m.zLog = zlog.Checked;
			m.x.fix = xfix.Checked; m.y.fix = yfix.Checked;
			if (xunit.Text != "") m.x.unit = xunit.Text;
			else m.x.unit = null;
			if (yunit.Text != "") m.y.unit = yunit.Text;
			else m.y.unit = null;
			if (zunit.Text != "") m.z.unit = zunit.Text;
			else m.z.unit = null;
			m.z.scale = zScale.Checked;
			m.Border = border.Checked;
			try {	m.ScaleLineWidth = float.Parse(lineWidth.Text);
			} catch {m.ScaleLineWidth = 1;}
 			m.x.style = (NumberStyle)xFormat.SelectedIndex;
			m.y.style = (NumberStyle)yFormat.SelectedIndex;
			m.x.digits = (int)xDigits.Value; m.y.digits = (int)yDigits.Value;
			m.ScaleFont = scaleFontForm.Font;
			m.UnitsFont = unitsFontForm.Font;
			m.LegendFont = legendFontForm.Font;
			m.ScaleColor = colorDialog.Color;

			useFullPrintPage = scalePage.Checked;

			Reset();
			plot.Invalidate();
		}

		public OptionsForm(PlotControl plot)
		{
			//
			// Required for Windows Form Designer support
			//
			this.plot = plot;
			
			InitializeComponent();

			xDigits.KeyPress += intKeyPress;
			yDigits.KeyPress += intKeyPress;
			xFormat.DropDownStyle = ComboBoxStyle.DropDownList;
			yFormat.DropDownStyle = ComboBoxStyle.DropDownList;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.colorButton = new System.Windows.Forms.Button();
			this.yFormatLabel = new System.Windows.Forms.Label();
			this.yFormat = new System.Windows.Forms.ComboBox();
			this.xFormatLabel = new System.Windows.Forms.Label();
			this.xFormat = new System.Windows.Forms.ComboBox();
			this.yDigitsLabel = new System.Windows.Forms.Label();
			this.xDigitsLabel = new System.Windows.Forms.Label();
			this.scaleFontButton = new System.Windows.Forms.Button();
			this.showYScale = new System.Windows.Forms.CheckBox();
			this.showXScale = new System.Windows.Forms.CheckBox();
			this.showXRaster = new System.Windows.Forms.CheckBox();
			this.showYRaster = new System.Windows.Forms.CheckBox();
			this.showXAxis = new System.Windows.Forms.CheckBox();
			this.showYAxis = new System.Windows.Forms.CheckBox();
			this.scaleFontForm = new System.Windows.Forms.FontDialog();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.applyButton = new System.Windows.Forms.Button();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.border = new System.Windows.Forms.CheckBox();
			this.legend = new System.Windows.Forms.CheckBox();
			this.zScale = new System.Windows.Forms.CheckBox();
			this.legendBox = new System.Windows.Forms.CheckBox();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.xDigits = new System.Windows.Forms.NumericUpDown();
			this.xunit = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.xfix = new System.Windows.Forms.CheckBox();
			this.xlog = new System.Windows.Forms.CheckBox();
			this.xGrid = new System.Windows.Forms.CheckBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.yDigits = new System.Windows.Forms.NumericUpDown();
			this.yunit = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.yfix = new System.Windows.Forms.CheckBox();
			this.ylog = new System.Windows.Forms.CheckBox();
			this.yGrid = new System.Windows.Forms.CheckBox();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.zunit = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.zlog = new System.Windows.Forms.CheckBox();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.textFontButton = new System.Windows.Forms.Button();
			this.tabPage = new System.Windows.Forms.TabPage();
			this.unitsFontButton = new System.Windows.Forms.Button();
			this.lineWidth = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.colorLabel = new System.Windows.Forms.Label();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.scalePage = new System.Windows.Forms.CheckBox();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.unitsFontForm = new System.Windows.Forms.FontDialog();
			this.legendFontForm = new System.Windows.Forms.FontDialog();
			this.help = new System.Windows.Forms.Button();
			this.tabControl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.xDigits)).BeginInit();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.yDigits)).BeginInit();
			this.tabPage3.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.tabPage.SuspendLayout();
			this.tabPage5.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(8, 192);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(287, 8);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// colorButton
			// 
			this.colorButton.Location = new System.Drawing.Point(118, 110);
			this.colorButton.Name = "colorButton";
			this.colorButton.Size = new System.Drawing.Size(72, 23);
			this.colorButton.TabIndex = 13;
			this.colorButton.Text = "Color ...";
			this.colorButton.Click += new System.EventHandler(this.colorClick);
			// 
			// yFormatLabel
			// 
			this.yFormatLabel.Location = new System.Drawing.Point(119, 38);
			this.yFormatLabel.Name = "yFormatLabel";
			this.yFormatLabel.Size = new System.Drawing.Size(43, 17);
			this.yFormatLabel.TabIndex = 12;
			this.yFormatLabel.Text = "Format:";
			// 
			// yFormat
			// 
			this.yFormat.Items.AddRange(new object[] {
            "Normal",
            "Fixedpoint",
            "Scientific"});
			this.yFormat.Location = new System.Drawing.Point(168, 35);
			this.yFormat.Name = "yFormat";
			this.yFormat.Size = new System.Drawing.Size(87, 21);
			this.yFormat.TabIndex = 11;
			// 
			// xFormatLabel
			// 
			this.xFormatLabel.Location = new System.Drawing.Point(119, 38);
			this.xFormatLabel.Name = "xFormatLabel";
			this.xFormatLabel.Size = new System.Drawing.Size(48, 23);
			this.xFormatLabel.TabIndex = 10;
			this.xFormatLabel.Text = "Format:";
			// 
			// xFormat
			// 
			this.xFormat.Items.AddRange(new object[] {
            "Normal",
            "Fixedpoint",
            "Scientific"});
			this.xFormat.Location = new System.Drawing.Point(168, 35);
			this.xFormat.Name = "xFormat";
			this.xFormat.Size = new System.Drawing.Size(87, 21);
			this.xFormat.TabIndex = 9;
			// 
			// yDigitsLabel
			// 
			this.yDigitsLabel.Location = new System.Drawing.Point(119, 8);
			this.yDigitsLabel.Name = "yDigitsLabel";
			this.yDigitsLabel.Size = new System.Drawing.Size(100, 23);
			this.yDigitsLabel.TabIndex = 8;
			this.yDigitsLabel.Text = "Number of digits:";
			// 
			// xDigitsLabel
			// 
			this.xDigitsLabel.Location = new System.Drawing.Point(119, 8);
			this.xDigitsLabel.Name = "xDigitsLabel";
			this.xDigitsLabel.Size = new System.Drawing.Size(100, 23);
			this.xDigitsLabel.TabIndex = 6;
			this.xDigitsLabel.Text = "Number of digits:";
			// 
			// scaleFontButton
			// 
			this.scaleFontButton.Location = new System.Drawing.Point(8, 81);
			this.scaleFontButton.Name = "scaleFontButton";
			this.scaleFontButton.Size = new System.Drawing.Size(84, 23);
			this.scaleFontButton.TabIndex = 4;
			this.scaleFontButton.Text = "Scale font ...";
			this.scaleFontButton.Click += new System.EventHandler(this.scaleFontButtonClick);
			// 
			// showYScale
			// 
			this.showYScale.Location = new System.Drawing.Point(8, 3);
			this.showYScale.Name = "showYScale";
			this.showYScale.Size = new System.Drawing.Size(104, 24);
			this.showYScale.TabIndex = 1;
			this.showYScale.Text = "Show y-scale";
			this.showYScale.CheckStateChanged += new System.EventHandler(this.yScaleChanged);
			// 
			// showXScale
			// 
			this.showXScale.Location = new System.Drawing.Point(8, 3);
			this.showXScale.Name = "showXScale";
			this.showXScale.Size = new System.Drawing.Size(104, 24);
			this.showXScale.TabIndex = 0;
			this.showXScale.Text = "Show x-scale";
			this.showXScale.CheckStateChanged += new System.EventHandler(this.xScaleChanged);
			// 
			// showXRaster
			// 
			this.showXRaster.Location = new System.Drawing.Point(8, 33);
			this.showXRaster.Name = "showXRaster";
			this.showXRaster.Size = new System.Drawing.Size(112, 24);
			this.showXRaster.TabIndex = 0;
			this.showXRaster.Text = "Show x-raster";
			// 
			// showYRaster
			// 
			this.showYRaster.Location = new System.Drawing.Point(8, 33);
			this.showYRaster.Name = "showYRaster";
			this.showYRaster.Size = new System.Drawing.Size(112, 24);
			this.showYRaster.TabIndex = 1;
			this.showYRaster.Text = "Show y-raster";
			// 
			// showXAxis
			// 
			this.showXAxis.Location = new System.Drawing.Point(8, 63);
			this.showXAxis.Name = "showXAxis";
			this.showXAxis.Size = new System.Drawing.Size(112, 24);
			this.showXAxis.TabIndex = 2;
			this.showXAxis.Text = "Show x=0 axis";
			// 
			// showYAxis
			// 
			this.showYAxis.Location = new System.Drawing.Point(8, 63);
			this.showYAxis.Name = "showYAxis";
			this.showYAxis.Size = new System.Drawing.Size(112, 24);
			this.showYAxis.TabIndex = 3;
			this.showYAxis.Text = "Show y=0 axis";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.okButton.Location = new System.Drawing.Point(8, 208);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(58, 24);
			this.okButton.TabIndex = 11;
			this.okButton.Text = "Ok";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancelButton.Location = new System.Drawing.Point(132, 208);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(54, 24);
			this.cancelButton.TabIndex = 12;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// applyButton
			// 
			this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.applyButton.Location = new System.Drawing.Point(72, 208);
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size(54, 24);
			this.applyButton.TabIndex = 13;
			this.applyButton.Text = "Apply";
			this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
			// 
			// border
			// 
			this.border.Location = new System.Drawing.Point(8, 8);
			this.border.Name = "border";
			this.border.Size = new System.Drawing.Size(128, 24);
			this.border.TabIndex = 14;
			this.border.Text = "Show bounding box";
			// 
			// legend
			// 
			this.legend.Location = new System.Drawing.Point(8, 8);
			this.legend.Name = "legend";
			this.legend.Size = new System.Drawing.Size(104, 24);
			this.legend.TabIndex = 15;
			this.legend.Text = "Show legend";
			this.legend.CheckedChanged += new System.EventHandler(this.legendChanged);
			// 
			// zScale
			// 
			this.zScale.Location = new System.Drawing.Point(8, 8);
			this.zScale.Name = "zScale";
			this.zScale.Size = new System.Drawing.Size(104, 24);
			this.zScale.TabIndex = 16;
			this.zScale.Text = "Show z-scale";
			// 
			// legendBox
			// 
			this.legendBox.Location = new System.Drawing.Point(8, 32);
			this.legendBox.Name = "legendBox";
			this.legendBox.Size = new System.Drawing.Size(160, 24);
			this.legendBox.TabIndex = 17;
			this.legendBox.Text = "Show box around legend";
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabPage1);
			this.tabControl.Controls.Add(this.tabPage2);
			this.tabControl.Controls.Add(this.tabPage3);
			this.tabControl.Controls.Add(this.tabPage4);
			this.tabControl.Controls.Add(this.tabPage);
			this.tabControl.Controls.Add(this.tabPage5);
			this.tabControl.Location = new System.Drawing.Point(12, 12);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(284, 176);
			this.tabControl.TabIndex = 18;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.xDigits);
			this.tabPage1.Controls.Add(this.xunit);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.xfix);
			this.tabPage1.Controls.Add(this.xlog);
			this.tabPage1.Controls.Add(this.xGrid);
			this.tabPage1.Controls.Add(this.showXScale);
			this.tabPage1.Controls.Add(this.showXRaster);
			this.tabPage1.Controls.Add(this.showXAxis);
			this.tabPage1.Controls.Add(this.xDigitsLabel);
			this.tabPage1.Controls.Add(this.xFormat);
			this.tabPage1.Controls.Add(this.xFormatLabel);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(276, 150);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "x-Axis";
			// 
			// xDigits
			// 
			this.xDigits.Location = new System.Drawing.Point(209, 6);
			this.xDigits.Name = "xDigits";
			this.xDigits.Size = new System.Drawing.Size(46, 20);
			this.xDigits.TabIndex = 16;
			// 
			// xunit
			// 
			this.xunit.Location = new System.Drawing.Point(45, 120);
			this.xunit.Name = "xunit";
			this.xunit.Size = new System.Drawing.Size(210, 20);
			this.xunit.TabIndex = 15;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(5, 123);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(34, 13);
			this.label2.TabIndex = 14;
			this.label2.Text = "Units:";
			// 
			// xfix
			// 
			this.xfix.AutoSize = true;
			this.xfix.Location = new System.Drawing.Point(122, 97);
			this.xfix.Name = "xfix";
			this.xfix.Size = new System.Drawing.Size(51, 17);
			this.xfix.TabIndex = 13;
			this.xfix.Text = "Fixed";
			this.xfix.UseVisualStyleBackColor = true;
			// 
			// xlog
			// 
			this.xlog.AutoSize = true;
			this.xlog.Location = new System.Drawing.Point(122, 67);
			this.xlog.Name = "xlog";
			this.xlog.Size = new System.Drawing.Size(80, 17);
			this.xlog.TabIndex = 12;
			this.xlog.Text = "Logarithmic";
			this.xlog.UseVisualStyleBackColor = true;
			this.xlog.Visible = false;
			// 
			// xGrid
			// 
			this.xGrid.Location = new System.Drawing.Point(8, 93);
			this.xGrid.Name = "xGrid";
			this.xGrid.Size = new System.Drawing.Size(112, 24);
			this.xGrid.TabIndex = 11;
			this.xGrid.Text = "Show x-grid";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.yDigits);
			this.tabPage2.Controls.Add(this.yunit);
			this.tabPage2.Controls.Add(this.label3);
			this.tabPage2.Controls.Add(this.yfix);
			this.tabPage2.Controls.Add(this.ylog);
			this.tabPage2.Controls.Add(this.yGrid);
			this.tabPage2.Controls.Add(this.yDigitsLabel);
			this.tabPage2.Controls.Add(this.showYScale);
			this.tabPage2.Controls.Add(this.yFormatLabel);
			this.tabPage2.Controls.Add(this.yFormat);
			this.tabPage2.Controls.Add(this.showYRaster);
			this.tabPage2.Controls.Add(this.showYAxis);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(276, 150);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "y-Axis";
			// 
			// yDigits
			// 
			this.yDigits.Location = new System.Drawing.Point(209, 8);
			this.yDigits.Name = "yDigits";
			this.yDigits.Size = new System.Drawing.Size(46, 20);
			this.yDigits.TabIndex = 18;
			// 
			// yunit
			// 
			this.yunit.Location = new System.Drawing.Point(45, 120);
			this.yunit.Name = "yunit";
			this.yunit.Size = new System.Drawing.Size(210, 20);
			this.yunit.TabIndex = 17;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(5, 123);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(34, 13);
			this.label3.TabIndex = 16;
			this.label3.Text = "Units:";
			// 
			// yfix
			// 
			this.yfix.AutoSize = true;
			this.yfix.Location = new System.Drawing.Point(122, 97);
			this.yfix.Name = "yfix";
			this.yfix.Size = new System.Drawing.Size(51, 17);
			this.yfix.TabIndex = 15;
			this.yfix.Text = "Fixed";
			this.yfix.UseVisualStyleBackColor = true;
			// 
			// ylog
			// 
			this.ylog.AutoSize = true;
			this.ylog.Location = new System.Drawing.Point(122, 67);
			this.ylog.Name = "ylog";
			this.ylog.Size = new System.Drawing.Size(80, 17);
			this.ylog.TabIndex = 14;
			this.ylog.Text = "Logarithmic";
			this.ylog.UseVisualStyleBackColor = true;
			this.ylog.Visible = false;
			// 
			// yGrid
			// 
			this.yGrid.Location = new System.Drawing.Point(8, 93);
			this.yGrid.Name = "yGrid";
			this.yGrid.Size = new System.Drawing.Size(112, 24);
			this.yGrid.TabIndex = 13;
			this.yGrid.Text = "Show y-grid";
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.zunit);
			this.tabPage3.Controls.Add(this.label4);
			this.tabPage3.Controls.Add(this.zlog);
			this.tabPage3.Controls.Add(this.zScale);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(276, 150);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "z-Axis";
			// 
			// zunit
			// 
			this.zunit.Location = new System.Drawing.Point(45, 117);
			this.zunit.Name = "zunit";
			this.zunit.Size = new System.Drawing.Size(210, 20);
			this.zunit.TabIndex = 19;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(5, 120);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(34, 13);
			this.label4.TabIndex = 18;
			this.label4.Text = "Units:";
			// 
			// zlog
			// 
			this.zlog.AutoSize = true;
			this.zlog.Location = new System.Drawing.Point(8, 38);
			this.zlog.Name = "zlog";
			this.zlog.Size = new System.Drawing.Size(80, 17);
			this.zlog.TabIndex = 17;
			this.zlog.Text = "Logarithmic";
			this.zlog.UseVisualStyleBackColor = true;
			this.zlog.Visible = false;
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.textFontButton);
			this.tabPage4.Controls.Add(this.legendBox);
			this.tabPage4.Controls.Add(this.legend);
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(276, 150);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Legend";
			// 
			// textFontButton
			// 
			this.textFontButton.Location = new System.Drawing.Point(8, 96);
			this.textFontButton.Name = "textFontButton";
			this.textFontButton.Size = new System.Drawing.Size(75, 23);
			this.textFontButton.TabIndex = 18;
			this.textFontButton.Text = "Font...";
			this.textFontButton.UseVisualStyleBackColor = true;
			this.textFontButton.Click += new System.EventHandler(this.legendFontButtonClick);
			// 
			// tabPage
			// 
			this.tabPage.Controls.Add(this.unitsFontButton);
			this.tabPage.Controls.Add(this.lineWidth);
			this.tabPage.Controls.Add(this.label1);
			this.tabPage.Controls.Add(this.border);
			this.tabPage.Controls.Add(this.scaleFontButton);
			this.tabPage.Controls.Add(this.colorButton);
			this.tabPage.Controls.Add(this.colorLabel);
			this.tabPage.Location = new System.Drawing.Point(4, 22);
			this.tabPage.Name = "tabPage";
			this.tabPage.Size = new System.Drawing.Size(276, 150);
			this.tabPage.TabIndex = 4;
			this.tabPage.Text = "Scale";
			// 
			// unitsFontButton
			// 
			this.unitsFontButton.Location = new System.Drawing.Point(8, 110);
			this.unitsFontButton.Name = "unitsFontButton";
			this.unitsFontButton.Size = new System.Drawing.Size(84, 23);
			this.unitsFontButton.TabIndex = 20;
			this.unitsFontButton.Text = "Units font ...";
			this.unitsFontButton.Click += new System.EventHandler(this.unitsFontButtonClick);
			// 
			// lineWidth
			// 
			this.lineWidth.Location = new System.Drawing.Point(72, 42);
			this.lineWidth.Name = "lineWidth";
			this.lineWidth.Size = new System.Drawing.Size(64, 20);
			this.lineWidth.TabIndex = 16;
			this.lineWidth.Text = "1";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(5, 45);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 23);
			this.label1.TabIndex = 15;
			this.label1.Text = "Line width:";
			// 
			// colorLabel
			// 
			this.colorLabel.Location = new System.Drawing.Point(196, 109);
			this.colorLabel.Name = "colorLabel";
			this.colorLabel.Size = new System.Drawing.Size(24, 24);
			this.colorLabel.TabIndex = 19;
			// 
			// tabPage5
			// 
			this.tabPage5.Controls.Add(this.scalePage);
			this.tabPage5.Location = new System.Drawing.Point(4, 22);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage5.Size = new System.Drawing.Size(276, 150);
			this.tabPage5.TabIndex = 5;
			this.tabPage5.Text = "Printing";
			// 
			// scalePage
			// 
			this.scalePage.AutoSize = true;
			this.scalePage.Location = new System.Drawing.Point(6, 18);
			this.scalePage.Name = "scalePage";
			this.scalePage.Size = new System.Drawing.Size(152, 17);
			this.scalePage.TabIndex = 0;
			this.scalePage.Text = "Scale plot to fit entire page";
			// 
			// helpProvider
			// 
			this.helpProvider.HelpNamespace = "..\\help\\FPlot.chm";
			// 
			// help
			// 
			this.help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.help.Location = new System.Drawing.Point(238, 208);
			this.help.Name = "help";
			this.help.Size = new System.Drawing.Size(54, 24);
			this.help.TabIndex = 19;
			this.help.Text = "Help ...";
			this.help.UseVisualStyleBackColor = true;
			this.help.Click += new System.EventHandler(this.helpClick);
			// 
			// OptionsForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(307, 246);
			this.Controls.Add(this.help);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.applyButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "OptionsForm";
			this.ShowInTaskbar = false;
			this.Text = "Options";
			this.tabControl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.xDigits)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.yDigits)).EndInit();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.tabPage4.ResumeLayout(false);
			this.tabPage.ResumeLayout(false);
			this.tabPage.PerformLayout();
			this.tabPage5.ResumeLayout(false);
			this.tabPage5.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void intKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled = !char.IsDigit(e.KeyChar) && ((int)e.KeyChar >= (int)' ');
		}
		
		private void okButton_Click(object sender, EventArgs e)
		{
			Apply();
			this.Hide();
		}

		private void applyButton_Click(object sender, EventArgs e)
		{
			Apply();
			Reset();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.Hide();
		}

		private void xScaleChanged(object sender, EventArgs e)
		{
			xDigits.Enabled = xFormat.Enabled = showXScale.Checked;
		}

		private void yScaleChanged(object sender, EventArgs e)
		{
			yDigits.Enabled = yFormat.Enabled = showYScale.Checked;
		}

		private void colorClick(object sender, EventArgs e) {
			colorDialog.ShowDialog();
			colorLabel.BackColor = colorDialog.Color;
		}

		private void legendChanged(object sender, EventArgs e) {
			legendBox.Enabled = legend.Checked;
		}

		private void legendFontButtonClick(object sender, EventArgs e) {
			legendFontForm.ShowDialog();
		}

		private void unitsFontButtonClick(object sender, EventArgs e) {
			unitsFontForm.ShowDialog();
		}
		
		private void scaleFontButtonClick(object sender, EventArgs e) {
			scaleFontForm.ShowDialog();
		}

		private void helpClick(object sender, EventArgs e) {
			Help.ShowHelp(this, "../help/FPlot.chm", "OptionsForm.html");
		}

	}
}
