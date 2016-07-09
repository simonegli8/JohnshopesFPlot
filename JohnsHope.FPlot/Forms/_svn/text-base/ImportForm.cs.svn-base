using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using JohnsHope.FPlot.Library;
using System.IO;
using System.Collections.Generic;

namespace JohnsHope.FPlot
{
	/// <summary>
	/// Summary description for importForm.
	/// </summary>
	public class ImportForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button ok;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button fileButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private ImportsGrid importsGrid1;
		private System.Windows.Forms.Button cancel;

		public ImportForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportForm));
			this.ok = new System.Windows.Forms.Button();
			this.fileButton = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.cancel = new System.Windows.Forms.Button();
			this.importsGrid1 = new JohnsHope.FPlot.ImportsGrid();
			this.SuspendLayout();
			// 
			// ok
			// 
			this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ok.Location = new System.Drawing.Point(10, 256);
			this.ok.Name = "ok";
			this.ok.Size = new System.Drawing.Size(86, 27);
			this.ok.TabIndex = 1;
			this.ok.Text = "Ok";
			this.ok.Click += new System.EventHandler(this.okClick);
			// 
			// fileButton
			// 
			this.fileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.fileButton.Location = new System.Drawing.Point(221, 256);
			this.fileButton.Name = "fileButton";
			this.fileButton.Size = new System.Drawing.Size(115, 27);
			this.fileButton.TabIndex = 3;
			this.fileButton.Text = "Import file...";
			this.fileButton.Click += new System.EventHandler(this.fileClick);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "Libraries (*.dll;*.exe) | *.dll; *.exe";
			this.openFileDialog.Multiselect = true;
			// 
			// cancel
			// 
			this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new System.Drawing.Point(115, 256);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(87, 27);
			this.cancel.TabIndex = 4;
			this.cancel.Text = "Cancel";
			this.cancel.Click += new System.EventHandler(this.cancelClick);
			// 
			// importsGrid1
			// 
			this.importsGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.importsGrid1.Location = new System.Drawing.Point(10, 14);
			this.importsGrid1.Name = "importsGrid1";
			this.importsGrid1.Size = new System.Drawing.Size(534, 235);
			this.importsGrid1.TabIndex = 5;
			// 
			// ImportForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(559, 287);
			this.Controls.Add(this.importsGrid1);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.fileButton);
			this.Controls.Add(this.ok);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(355, 194);
			this.Name = "ImportForm";
			this.ShowInTaskbar = false;
			this.Text = "Import Assemblies";
			this.ResumeLayout(false);

		}
		#endregion

		private void okClick(object sender, System.EventArgs e) {
			importsGrid1.Commit();
			this.Hide();
		}

		public void Reset() {
			importsGrid1.Reset();
		}

		private void fileClick(object sender, System.EventArgs e) {
			DialogResult res = openFileDialog.ShowDialog();
			if (res == DialogResult.OK) {
				importsGrid1.Add(openFileDialog.FileName);
			}
		}

		private void cancelClick(object sender, System.EventArgs e) {
			this.Hide();
		}

	}
}
