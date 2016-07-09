using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using JohnsHope.FPlot.Properties;
using JohnsHope.FPlot.Util;
using JohnsHope.Update;

namespace JohnsHope.FPlot
{
	/// <summary>
	/// Summary description for AboutForm.
	/// </summary>
	public class UpdateForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label AppName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
		//private RegisterForm regForm = new RegisterForm();
		private LinkLabel linkLabel1;
		private PictureBox pictureBox1;
		private Label label4;
		private Label label3;
		private PictureBox pictureBox3;
		private Label label5;
		private LinkLabel linkLabel2;
		private CheckBox autoUpdate;
		private Label newVersion;
		private PictureBox downloadButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UpdateForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			AppName.Text = "JohnsHope's FPlot " + Updater.InstalledVersion.DisplayVersion;
			newVersion.Text = "JohnsHope's FPlot " + Updater.DownloadableVersion.DisplayVersion;
			autoUpdate.Checked  = Updater.InstalledVersion.AutoUpdate;
			downloadButton.Cursor = Cursors.Hand;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateForm));
			this.AppName = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.label5 = new System.Windows.Forms.Label();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.autoUpdate = new System.Windows.Forms.CheckBox();
			this.newVersion = new System.Windows.Forms.Label();
			this.downloadButton = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.downloadButton)).BeginInit();
			this.SuspendLayout();
			// 
			// AppName
			// 
			this.AppName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.AppName.BackColor = System.Drawing.Color.Transparent;
			this.AppName.Font = new System.Drawing.Font("Verdana", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AppName.Location = new System.Drawing.Point(10, 100);
			this.AppName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.AppName.Name = "AppName";
			this.AppName.Size = new System.Drawing.Size(445, 46);
			this.AppName.TabIndex = 0;
			this.AppName.Text = "JohnsHope\'s FPlot";
			this.AppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(9, 309);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(445, 36);
			this.label2.TabIndex = 1;
			this.label2.Text = "Programmed with C# by Simon Egli. This programm is freeware and Open Source\r\n. Yo" +
    "u can download it at:\r\n";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.Location = new System.Drawing.Point(85, 532);
			this.button1.Margin = new System.Windows.Forms.Padding(2);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(297, 22);
			this.button1.TabIndex = 2;
			this.button1.Text = "Continue without Downloading newer Version";
			this.button1.Click += new System.EventHandler(this.okClick);
			// 
			// linkLabel1
			// 
			this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
			this.linkLabel1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.linkLabel1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.linkLabel1.Location = new System.Drawing.Point(142, 345);
			this.linkLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(183, 15);
			this.linkLabel1.TabIndex = 7;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "www.johnshope.com/fplot";
			this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkClick);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.pictureBox1.Image = global::JohnsHope.FPlot.Properties.Resources.JohnsHopeLogo;
			this.pictureBox1.InitialImage = null;
			this.pictureBox1.Location = new System.Drawing.Point(10, 11);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(445, 87);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox1.TabIndex = 8;
			this.pictureBox1.TabStop = false;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold);
			this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label4.Location = new System.Drawing.Point(8, 145);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(447, 46);
			this.label4.TabIndex = 12;
			this.label4.Text = "There is a newer verison of Johns Hope\'s FPlot available for download:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(9, 373);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(357, 42);
			this.label3.TabIndex = 13;
			this.label3.Text = "Johns Hope\'s FPlot is freeware. However you can support further development by a " +
    "donation to Johns Hope Software.";
			// 
			// pictureBox3
			// 
			this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox3.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictureBox3.Image = global::JohnsHope.FPlot.Properties.Resources.PayPalDonate;
			this.pictureBox3.Location = new System.Drawing.Point(372, 373);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(71, 38);
			this.pictureBox3.TabIndex = 15;
			this.pictureBox3.TabStop = false;
			this.pictureBox3.Click += new System.EventHandler(this.DonateClick);
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(8, 432);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(447, 34);
			this.label5.TabIndex = 16;
			this.label5.Text = "If you have discovered a bug or have a feature request or if you want to particip" +
    "ate in the development of FPlot just contact us:";
			// 
			// linkLabel2
			// 
			this.linkLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.linkLabel2.BackColor = System.Drawing.Color.Transparent;
			this.linkLabel2.Cursor = System.Windows.Forms.Cursors.Hand;
			this.linkLabel2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.linkLabel2.Location = new System.Drawing.Point(128, 466);
			this.linkLabel2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(211, 15);
			this.linkLabel2.TabIndex = 17;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "www.johnshope.com/contact";
			this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ContactClick);
			// 
			// autoUpdate
			// 
			this.autoUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.autoUpdate.AutoSize = true;
			this.autoUpdate.BackColor = System.Drawing.Color.Transparent;
			this.autoUpdate.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.autoUpdate.Location = new System.Drawing.Point(13, 495);
			this.autoUpdate.Name = "autoUpdate";
			this.autoUpdate.Size = new System.Drawing.Size(416, 32);
			this.autoUpdate.TabIndex = 18;
			this.autoUpdate.Text = "Always inform me when a new version of JohnsHope\'s FPlot is\r\navailable";
			this.autoUpdate.UseVisualStyleBackColor = false;
			// 
			// newVersion
			// 
			this.newVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.newVersion.BackColor = System.Drawing.Color.Transparent;
			this.newVersion.Font = new System.Drawing.Font("Verdana", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.newVersion.ForeColor = System.Drawing.Color.SlateBlue;
			this.newVersion.Location = new System.Drawing.Point(10, 191);
			this.newVersion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.newVersion.Name = "newVersion";
			this.newVersion.Size = new System.Drawing.Size(445, 33);
			this.newVersion.TabIndex = 19;
			this.newVersion.Text = "JohnsHope\'s FPlot";
			this.newVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// downloadButton
			// 
			this.downloadButton.BackColor = System.Drawing.Color.Transparent;
			this.downloadButton.Image = global::JohnsHope.FPlot.Properties.Resources.downloadButton;
			this.downloadButton.ImageLocation = "";
			this.downloadButton.Location = new System.Drawing.Point(131, 227);
			this.downloadButton.Name = "downloadButton";
			this.downloadButton.Size = new System.Drawing.Size(200, 69);
			this.downloadButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.downloadButton.TabIndex = 20;
			this.downloadButton.TabStop = false;
			this.downloadButton.Click += new System.EventHandler(this.DownloadClick);
			// 
			// UpdateForm
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::JohnsHope.FPlot.Properties.Resources.MandelbrotBright;
			this.ClientSize = new System.Drawing.Size(466, 565);
			this.Controls.Add(this.downloadButton);
			this.Controls.Add(this.newVersion);
			this.Controls.Add(this.autoUpdate);
			this.Controls.Add(this.linkLabel2);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.pictureBox3);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.AppName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UpdateForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Update JohnsHope\'s FPlot";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.downloadButton)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

	
		private void okClick(object sender, System.EventArgs e) {
			Hide();
			Updater.Enabled = autoUpdate.Checked;
		}

		private void DonateClick(object sender, EventArgs e) {
			System.Diagnostics.Process.Start(Resources.DonateURL);			
		}

		private void linkClick(object sender, LinkLabelLinkClickedEventArgs e) {
			System.Diagnostics.Process.Start(Resources.FPlotURL);
		}

		private void ContactClick(object sender, LinkLabelLinkClickedEventArgs e) {
			System.Diagnostics.Process.Start(Resources.ContactURL);
		}

		private void DownloadClick(object sender, EventArgs e) {
			Hide();
			Updater.Enabled = autoUpdate.Checked;
			Updater.StartUpdate();
		}

	}
}
