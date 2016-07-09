using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

using JohnsHope.FPlot.Library;

namespace FPlotDemo {
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form {
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button options;
		private System.Windows.Forms.Button sin;
		private System.Windows.Forms.Button cos;
		private System.Windows.Forms.Button data;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.ProgressBar progressBar1;
		private PlotControl plot;
		private System.Windows.Forms.Button loadwav;
		private Button saveButton;
		private SaveFileDialog saveFileDialog;
		private Button button2;
		private Button button5;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			plot.ProgressBar = progressBar1;
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
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.plot = new JohnsHope.FPlot.Library.PlotControl();
			this.button1 = new System.Windows.Forms.Button();
			this.options = new System.Windows.Forms.Button();
			this.sin = new System.Windows.Forms.Button();
			this.cos = new System.Windows.Forms.Button();
			this.data = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.loadwav = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.button2 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// plot
			// 
			this.plot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.plot.BackColor = System.Drawing.Color.White;
			this.plot.Border = true;
			this.plot.Cursor = System.Windows.Forms.Cursors.Cross;
			this.plot.FixXtoY = false;
			this.plot.Location = new System.Drawing.Point(0, 0);
			this.plot.Name = "plot";
			this.plot.ProgressBar = null;
			this.plot.Size = new System.Drawing.Size(350, 387);
			this.plot.TabIndex = 0;
			this.plot.Text = "plot";
			this.plot.x0 = -8;
			this.plot.x1 = 4;
			this.plot.xGrid = true;
			this.plot.y0 = -4.1973373870943052;
			this.plot.y1 = 4.2374452216013472;
			this.plot.yGrid = true;
			this.plot.z0 = 0;
			this.plot.z1 = 20;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(366, 323);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Quit";
			this.button1.Click += new System.EventHandler(this.quitClick);
			// 
			// options
			// 
			this.options.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.options.Location = new System.Drawing.Point(366, 56);
			this.options.Name = "options";
			this.options.Size = new System.Drawing.Size(104, 24);
			this.options.TabIndex = 2;
			this.options.Text = "Options...";
			this.options.Click += new System.EventHandler(this.optionsClick);
			// 
			// sin
			// 
			this.sin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.sin.Location = new System.Drawing.Point(366, 86);
			this.sin.Name = "sin";
			this.sin.Size = new System.Drawing.Size(104, 23);
			this.sin.TabIndex = 3;
			this.sin.Text = "Add sin(x)...";
			this.sin.Click += new System.EventHandler(this.sinClick);
			// 
			// cos
			// 
			this.cos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cos.Location = new System.Drawing.Point(366, 115);
			this.cos.Name = "cos";
			this.cos.Size = new System.Drawing.Size(104, 24);
			this.cos.TabIndex = 4;
			this.cos.Text = "Add cos(x)...";
			this.cos.Click += new System.EventHandler(this.cosClick);
			// 
			// data
			// 
			this.data.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.data.Location = new System.Drawing.Point(366, 205);
			this.data.Name = "data";
			this.data.Size = new System.Drawing.Size(104, 23);
			this.data.TabIndex = 5;
			this.data.Text = "Load text data...";
			this.data.Click += new System.EventHandler(this.textClick);
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button3.Location = new System.Drawing.Point(366, 145);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(104, 24);
			this.button3.TabIndex = 7;
			this.button3.Text = "Add gaussian...";
			this.button3.Click += new System.EventHandler(this.gaussClick);
			// 
			// button4
			// 
			this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button4.Location = new System.Drawing.Point(366, 175);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(104, 24);
			this.button4.TabIndex = 8;
			this.button4.Text = "Add Mandelbrot...";
			this.button4.Click += new System.EventHandler(this.mandelbrotClick);
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar1.Location = new System.Drawing.Point(366, 363);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(104, 16);
			this.progressBar1.TabIndex = 9;
			// 
			// loadwav
			// 
			this.loadwav.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.loadwav.Location = new System.Drawing.Point(366, 234);
			this.loadwav.Name = "loadwav";
			this.loadwav.Size = new System.Drawing.Size(104, 23);
			this.loadwav.TabIndex = 10;
			this.loadwav.Text = "Load WAV file...";
			this.loadwav.Click += new System.EventHandler(this.wavClick);
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Location = new System.Drawing.Point(366, 263);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(104, 23);
			this.saveButton.TabIndex = 11;
			this.saveButton.Text = "Save bitmap...";
			this.saveButton.Click += new System.EventHandler(this.saveButtonClick);
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.Filter = "Image Files (GIF JPEG TIFF BMP PNG EMF)|*.gif;*.jpg;*.jpeg;*.bmp;*.png;*.tif;*.ti" +
    "ff;*.emf";
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(366, 292);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(104, 23);
			this.button2.TabIndex = 12;
			this.button2.Text = "Fitting demo...";
			this.button2.Click += new System.EventHandler(this.fitDemo);
			// 
			// button5
			// 
			this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button5.Location = new System.Drawing.Point(366, 12);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(104, 24);
			this.button5.TabIndex = 13;
			this.button5.Text = "Clear";
			this.button5.Click += new System.EventHandler(this.clearClick);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(478, 384);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.loadwav);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.data);
			this.Controls.Add(this.cos);
			this.Controls.Add(this.sin);
			this.Controls.Add(this.options);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.plot);
			this.Name = "MainForm";
			this.Text = "FPlot Demo";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new MainForm());
		}

		private void quitClick(object sender, EventArgs e) {
			Application.Exit();
		}

		private void clearClick(object sender, EventArgs e) {
			plot.Model.Clear();
			plot.SetRange(-8, 4, -4, 4);
		}

		private void optionsClick(object sender, EventArgs e) {
			OptionsForm form = new OptionsForm(plot);
			form.Reset();
			form.ShowDialog();
		}

		private void sinClick(object sender, EventArgs e) { Demo.Sin(plot); }

		private void cosClick(object sender, EventArgs e) { Demo.Cos(plot); }

		private void gaussClick(object sender, EventArgs e) { Demo.Gauss(plot); }

		private void mandelbrotClick(object sender, EventArgs e) { Demo.Mandelbrot(plot); }

		private void textClick(object sender, EventArgs e) { Demo.Text(plot); }

		private void wavClick(object sender, EventArgs e) { Demo.WAV(plot); }

		private void fitDemo(object sender, EventArgs e) {
			plot.SetRange(-8, 4, 0, 4);
			FitDemo demo = new FitDemo(plot);
			demo.Run();
		}

		private void saveButtonClick(object sender, EventArgs e) {
			DialogResult res = saveFileDialog.ShowDialog();
			if (res == DialogResult.OK) {
				// Save plot as image.
				plot.SaveAsImage(saveFileDialog.FileName);
			}
		}

	}
}
