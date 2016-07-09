using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using JohnsHope.FPlot.Library;

namespace JohnsHope.FPlot
{
	/// <summary>
	/// Summary description for ImageForm.
	/// </summary>
	public class ImageForm : System.Windows.Forms.Form {
		private System.Windows.Forms.TextBox width;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button cancelButton;
		private ImageControl picture;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private PlotControl plot, copy;
		private System.Windows.Forms.Button applyButton;
	
		private Bitmap bitmap;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;

		private Rectangle r;
		private Graphics g;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox height;

		public ImageForm(PlotControl plot) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.plot = plot;
			this.picture = new ImageControl();
			this.picture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.picture.Location = new System.Drawing.Point(0, 35);
			this.picture.Name = "picture";
			this.picture.Size = new System.Drawing.Size(440, 210);
			this.picture.TabIndex = 3;
			this.picture.TabStop = false;
			this.Controls.Add(this.picture);
		}
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (copy != null) copy.Dispose();
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public void DoDispose() {
			Dispose();
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageForm));
			this.width = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.saveButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.applyButton = new System.Windows.Forms.Button();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.label2 = new System.Windows.Forms.Label();
			this.height = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// width
			// 
			this.width.Location = new System.Drawing.Point(96, 9);
			this.width.Name = "width";
			this.width.Size = new System.Drawing.Size(58, 22);
			this.width.TabIndex = 1;
			this.width.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.intKeyPress);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 19);
			this.label1.TabIndex = 2;
			this.label1.Text = "Pixel width:";
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.saveButton.Location = new System.Drawing.Point(10, 257);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(90, 27);
			this.saveButton.TabIndex = 4;
			this.saveButton.Text = "Save...";
			this.saveButton.Click += new System.EventHandler(this.saveClick);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancelButton.Location = new System.Drawing.Point(115, 257);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(90, 27);
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelClick);
			// 
			// applyButton
			// 
			this.applyButton.Location = new System.Drawing.Point(307, 9);
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size(90, 27);
			this.applyButton.TabIndex = 6;
			this.applyButton.Text = "Apply";
			this.applyButton.Click += new System.EventHandler(this.applyClick);
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "gif";
			this.saveFileDialog.Filter = "Image Files (GIF JPEG TIFF BMP PNG EMF)|*.gif;*.jpg;*.jpeg;*.bmp;*.png;*.tif;*.ti" +
    "ff;*.emf";
			this.saveFileDialog.Title = "Save as GIF File";
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.Location = new System.Drawing.Point(231, 257);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(192, 27);
			this.progressBar.TabIndex = 7;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(163, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(87, 19);
			this.label2.TabIndex = 8;
			this.label2.Text = "Pixel height:";
			// 
			// height
			// 
			this.height.Location = new System.Drawing.Point(240, 9);
			this.height.Name = "height";
			this.height.Size = new System.Drawing.Size(58, 22);
			this.height.TabIndex = 9;
			this.height.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.intKeyPress);
			// 
			// ImageForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(432, 288);
			this.Controls.Add(this.height);
			this.Controls.Add(this.width);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.applyButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(432, 212);
			this.Name = "ImageForm";
			this.ShowInTaskbar = false;
			this.Text = "Save as Image";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void ResetImage() {
			int w = int.Parse(width.Text);
			int h = int.Parse(height.Text);
			if (w <= 0) {w = 1; width.Text = "1"; }
			if (h <= 0) {h = 1; height.Text = "1"; }
			r = new Rectangle(0, 0, w, h); 
			bitmap = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			g = Graphics.FromImage(bitmap);
			g.Clear(Color.White);
			copy = (PlotControl)plot.Clone();
			copy.Parent = picture;
			copy.Visible = false;
			copy.Bounds = r;
			lock(copy) {
				copy.ProgressBar = progressBar;
				// TODO copy.Model.Invalidated += CopyNotify;
				g.SmoothingMode = SmoothingMode.AntiAlias;
				copy.Draw(g);
				picture.Image = bitmap;
			}
		}

		public void CopyNotify(PlotModel model) {
			lock(copy) {
				g = Graphics.FromImage(bitmap);
				g.Clear(Color.White);
				g.SmoothingMode = SmoothingMode.AntiAlias;
				copy.Draw(g);
				picture.Image = bitmap;
			}
		}

		public void Reset() {
			Width = plot.Width + 10;
			Height = plot.Height + 105;
			width.Text = plot.Width.ToString();
			height.Text = plot.Height.ToString();
			ResetImage();
		}

		private void saveClick(object sender, System.EventArgs e) {
			if (copy != null) {
				DialogResult res = saveFileDialog.ShowDialog();
				if (res == DialogResult.OK) {
					copy.SaveAsImage(saveFileDialog.FileName);
				}
			}
		}

		private void cancelClick(object sender, System.EventArgs e) {
			this.Hide();
		}

		private void intKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
			e.Handled = !char.IsDigit(e.KeyChar) && ((int)e.KeyChar >= (int)' ');
		}

		private void applyClick(object sender, System.EventArgs e) {
			ResetImage();
		}

	}
}
