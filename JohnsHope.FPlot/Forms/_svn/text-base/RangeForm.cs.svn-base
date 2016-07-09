using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using JohnsHope.FPlot.Library;

namespace JohnsHope.FPlot
{
	/// <summary>
	/// Summary description for RangeForm.
	/// </summary>
	public class RangeForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox x0TextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TextBox y0TextBox;
		private System.Windows.Forms.TextBox y1TextBox;
		private System.Windows.Forms.TextBox x1TextBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox fixytox;
		private System.Windows.Forms.TextBox z1TextBox;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox z0TextBox;
		private System.Windows.Forms.Label label8;
		private CheckBox xlog;
		private CheckBox ylog;
		private CheckBox zlog;
		private PlotControl plot;

		public RangeForm(PlotControl plot)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.plot = plot;
			x0TextBox.KeyPress += numberKeyPress;
			y0TextBox.KeyPress += numberKeyPress;
			x1TextBox.KeyPress += numberKeyPress;
			y1TextBox.KeyPress += numberKeyPress;
			z0TextBox.KeyPress += numberKeyPress;
			z1TextBox.KeyPress += numberKeyPress;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RangeForm));
			this.label1 = new System.Windows.Forms.Label();
			this.x0TextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.y0TextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.y1TextBox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.x1TextBox = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.applyButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.fixytox = new System.Windows.Forms.CheckBox();
			this.z1TextBox = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.z0TextBox = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.xlog = new System.Windows.Forms.CheckBox();
			this.ylog = new System.Windows.Forms.CheckBox();
			this.zlog = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 30);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(14, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "x";
			// 
			// x0TextBox
			// 
			this.x0TextBox.Location = new System.Drawing.Point(22, 30);
			this.x0TextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.x0TextBox.Name = "x0TextBox";
			this.x0TextBox.Size = new System.Drawing.Size(150, 20);
			this.x0TextBox.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 53);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(14, 22);
			this.label2.TabIndex = 2;
			this.label2.Text = "y";
			// 
			// y0TextBox
			// 
			this.y0TextBox.Location = new System.Drawing.Point(22, 53);
			this.y0TextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.y0TextBox.Name = "y0TextBox";
			this.y0TextBox.Size = new System.Drawing.Size(150, 20);
			this.y0TextBox.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 7);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(36, 15);
			this.label3.TabIndex = 4;
			this.label3.Text = "From:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(183, 7);
			this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(22, 15);
			this.label4.TabIndex = 5;
			this.label4.Text = "To:";
			// 
			// y1TextBox
			// 
			this.y1TextBox.Location = new System.Drawing.Point(199, 53);
			this.y1TextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.y1TextBox.Name = "y1TextBox";
			this.y1TextBox.Size = new System.Drawing.Size(150, 20);
			this.y1TextBox.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(183, 53);
			this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(15, 22);
			this.label5.TabIndex = 8;
			this.label5.Text = "y";
			// 
			// x1TextBox
			// 
			this.x1TextBox.Location = new System.Drawing.Point(199, 30);
			this.x1TextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.x1TextBox.Name = "x1TextBox";
			this.x1TextBox.Size = new System.Drawing.Size(150, 20);
			this.x1TextBox.TabIndex = 7;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(183, 30);
			this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(15, 15);
			this.label6.TabIndex = 6;
			this.label6.Text = "x";
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(8, 138);
			this.okButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(68, 22);
			this.okButton.TabIndex = 10;
			this.okButton.Text = "&Ok";
			this.okButton.Click += new System.EventHandler(this.okClick);
			// 
			// applyButton
			// 
			this.applyButton.Location = new System.Drawing.Point(89, 138);
			this.applyButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size(68, 22);
			this.applyButton.TabIndex = 11;
			this.applyButton.Text = "&Apply";
			this.applyButton.Click += new System.EventHandler(this.applyClick);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(172, 138);
			this.cancelButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(68, 22);
			this.cancelButton.TabIndex = 12;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelClick);
			// 
			// fixytox
			// 
			this.fixytox.Location = new System.Drawing.Point(8, 105);
			this.fixytox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.fixytox.Name = "fixytox";
			this.fixytox.Size = new System.Drawing.Size(129, 23);
			this.fixytox.TabIndex = 13;
			this.fixytox.Text = "Fix x-Scale to y-Scale";
			// 
			// z1TextBox
			// 
			this.z1TextBox.Location = new System.Drawing.Point(199, 75);
			this.z1TextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.z1TextBox.Name = "z1TextBox";
			this.z1TextBox.Size = new System.Drawing.Size(150, 20);
			this.z1TextBox.TabIndex = 17;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(183, 75);
			this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(15, 23);
			this.label7.TabIndex = 16;
			this.label7.Text = "z";
			// 
			// z0TextBox
			// 
			this.z0TextBox.Location = new System.Drawing.Point(22, 75);
			this.z0TextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.z0TextBox.Name = "z0TextBox";
			this.z0TextBox.Size = new System.Drawing.Size(150, 20);
			this.z0TextBox.TabIndex = 15;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 75);
			this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(14, 23);
			this.label8.TabIndex = 14;
			this.label8.Text = "z";
			// 
			// xlog
			// 
			this.xlog.AutoSize = true;
			this.xlog.Location = new System.Drawing.Point(256, 98);
			this.xlog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.xlog.Name = "xlog";
			this.xlog.Size = new System.Drawing.Size(80, 17);
			this.xlog.TabIndex = 18;
			this.xlog.Text = "Logarithmic";
			this.xlog.UseVisualStyleBackColor = true;
			this.xlog.Visible = false;
			// 
			// ylog
			// 
			this.ylog.AutoSize = true;
			this.ylog.Location = new System.Drawing.Point(256, 120);
			this.ylog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.ylog.Name = "ylog";
			this.ylog.Size = new System.Drawing.Size(80, 17);
			this.ylog.TabIndex = 19;
			this.ylog.Text = "Logarithmic";
			this.ylog.UseVisualStyleBackColor = true;
			this.ylog.Visible = false;
			// 
			// zlog
			// 
			this.zlog.AutoSize = true;
			this.zlog.Location = new System.Drawing.Point(256, 143);
			this.zlog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.zlog.Name = "zlog";
			this.zlog.Size = new System.Drawing.Size(80, 17);
			this.zlog.TabIndex = 20;
			this.zlog.Text = "Logarithmic";
			this.zlog.UseVisualStyleBackColor = true;
			this.zlog.Visible = false;
			// 
			// RangeForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(361, 165);
			this.Controls.Add(this.zlog);
			this.Controls.Add(this.ylog);
			this.Controls.Add(this.xlog);
			this.Controls.Add(this.z1TextBox);
			this.Controls.Add(this.z0TextBox);
			this.Controls.Add(this.y1TextBox);
			this.Controls.Add(this.x1TextBox);
			this.Controls.Add(this.y0TextBox);
			this.Controls.Add(this.x0TextBox);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.fixytox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.applyButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Name = "RangeForm";
			this.ShowInTaskbar = false;
			this.Text = "Set view range";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		public void Reset() {
			PlotModel m = plot.Model;
			x0TextBox.Text = m.x0.ToString();
			y0TextBox.Text = m.y0.ToString();
			x1TextBox.Text = m.x1.ToString();
			y1TextBox.Text = m.y1.ToString();
			z0TextBox.Text = m.z0.ToString();
			z1TextBox.Text = m.z1.ToString();
			fixytox.Checked = m.FixXtoY;
			xlog.Checked = m.xLog; ylog.Checked = m.yLog; zlog.Checked = m.zLog;
		}
		
		private void Apply() {
			try {
				double x0 = double.Parse(x0TextBox.Text);
				double y0 = double.Parse(y0TextBox.Text);
				double x1 = double.Parse(x1TextBox.Text);
				double y1 = double.Parse(y1TextBox.Text);
				double z0 = double.Parse(z0TextBox.Text);
				double z1 = double.Parse(z1TextBox.Text);
			
				plot.Model.FixXtoY = fixytox.Checked;
				plot.Model.xLog = xlog.Checked; plot.Model.yLog = ylog.Checked; plot.Model.zLog = zlog.Checked;
				plot.SetRange(x0, x1, y0, y1, z0, z1);
				Reset();
			} catch {
				DialogResult res = MessageBox.Show("One of the entered numbers is invalid.");
				throw new System.ApplicationException();
			}
		}

		private void numberKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled = !char.IsDigit(e.KeyChar) && ((int)e.KeyChar >= (int)' ') && (e.KeyChar != '.') &&
				(e.KeyChar != ',') && (e.KeyChar != '-') && (e.KeyChar != 'E') && (e.KeyChar != 'e');
		}

		private void okClick(object sender, System.EventArgs e)
		{
			try { Apply(); this.Hide(); }
			catch {}
			Reset();
		}

		private void applyClick(object sender, System.EventArgs e)
		{
			try { Apply(); }
			catch {}
			Reset();
		}

		private void cancelClick(object sender, System.EventArgs e)
		{
			this.Hide();
		}

	}
}
