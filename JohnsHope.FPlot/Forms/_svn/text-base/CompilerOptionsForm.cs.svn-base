using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Windows.Forms;
using JohnsHope.FPlot.Library;

namespace JohnsHope.FPlot
{
	/// <summary>
	/// Summary description for CompilerOptionsForm.
	/// </summary>
	public class CompilerOptionsForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button imports;
		private System.Windows.Forms.CheckBox overflow;
		private System.Windows.Forms.CheckBox debug;
		private System.Windows.Forms.CheckBox Options;
		private System.Windows.Forms.CheckBox allowunsafe;
		private System.Windows.Forms.ComboBox warnings;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button ok;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button cancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private CheckBox errorCodes;
		private Label label2;
		private CodeControl sourceHeader;
		private ImportForm importForm;
		private EventHandler handler;
		private CheckBox autoAddNamespace;
		private Button button1;
		public static CompilerOptionsForm Form = null; // A global static intance of the CompilerOptionsForm.

		public CompilerOptionsForm()
		{
			Form = this;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			importForm = new ImportForm();
			warnings.DropDownStyle = ComboBoxStyle.DropDownList;
			Compiler.Options.Header.TextChanged += handler = new EventHandler(Invalidate);
			Reset();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				Compiler.Options.Header.TextChanged -= handler;
				if(components != null) {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompilerOptionsForm));
			this.imports = new System.Windows.Forms.Button();
			this.overflow = new System.Windows.Forms.CheckBox();
			this.debug = new System.Windows.Forms.CheckBox();
			this.Options = new System.Windows.Forms.CheckBox();
			this.allowunsafe = new System.Windows.Forms.CheckBox();
			this.warnings = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ok = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cancel = new System.Windows.Forms.Button();
			this.errorCodes = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.sourceHeader = new JohnsHope.FPlot.CodeControl();
			this.autoAddNamespace = new System.Windows.Forms.CheckBox();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// imports
			// 
			this.imports.Location = new System.Drawing.Point(11, 11);
			this.imports.Margin = new System.Windows.Forms.Padding(2);
			this.imports.Name = "imports";
			this.imports.Size = new System.Drawing.Size(115, 22);
			this.imports.TabIndex = 0;
			this.imports.Text = "Imported libraries...";
			this.imports.Click += new System.EventHandler(this.impClick);
			// 
			// overflow
			// 
			this.overflow.Location = new System.Drawing.Point(8, 49);
			this.overflow.Margin = new System.Windows.Forms.Padding(2);
			this.overflow.Name = "overflow";
			this.overflow.Size = new System.Drawing.Size(168, 17);
			this.overflow.TabIndex = 1;
			this.overflow.Text = "Check arithmetic overflow";
			// 
			// debug
			// 
			this.debug.Location = new System.Drawing.Point(8, 75);
			this.debug.Margin = new System.Windows.Forms.Padding(2);
			this.debug.Name = "debug";
			this.debug.Size = new System.Drawing.Size(168, 17);
			this.debug.TabIndex = 2;
			this.debug.Text = "Include debug info";
			// 
			// Options
			// 
			this.Options.Location = new System.Drawing.Point(8, 101);
			this.Options.Margin = new System.Windows.Forms.Padding(2);
			this.Options.Name = "Options";
			this.Options.Size = new System.Drawing.Size(168, 17);
			this.Options.TabIndex = 3;
			this.Options.Text = "Optimize code";
			// 
			// allowunsafe
			// 
			this.allowunsafe.Location = new System.Drawing.Point(8, 127);
			this.allowunsafe.Margin = new System.Windows.Forms.Padding(2);
			this.allowunsafe.Name = "allowunsafe";
			this.allowunsafe.Size = new System.Drawing.Size(168, 17);
			this.allowunsafe.TabIndex = 4;
			this.allowunsafe.Text = "Allow unsafe constructs";
			// 
			// warnings
			// 
			this.warnings.Items.AddRange(new object[] {
            "No warnings",
            "Level 1",
            "Level 2",
            "Level 3",
            "Level 4"});
			this.warnings.Location = new System.Drawing.Point(291, 47);
			this.warnings.Margin = new System.Windows.Forms.Padding(2);
			this.warnings.Name = "warnings";
			this.warnings.Size = new System.Drawing.Size(107, 21);
			this.warnings.TabIndex = 5;
			this.warnings.Text = "No warnings";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(206, 51);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 15);
			this.label1.TabIndex = 6;
			this.label1.Text = "Warning level:";
			// 
			// ok
			// 
			this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ok.Location = new System.Drawing.Point(4, 493);
			this.ok.Margin = new System.Windows.Forms.Padding(2);
			this.ok.Name = "ok";
			this.ok.Size = new System.Drawing.Size(72, 23);
			this.ok.TabIndex = 7;
			this.ok.Text = "Ok";
			this.ok.Click += new System.EventHandler(this.okClick);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(5, 480);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox1.Size = new System.Drawing.Size(399, 8);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			// 
			// cancel
			// 
			this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new System.Drawing.Point(156, 493);
			this.cancel.Margin = new System.Windows.Forms.Padding(2);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(72, 23);
			this.cancel.TabIndex = 9;
			this.cancel.Text = "Cancel";
			this.cancel.Click += new System.EventHandler(this.cancelClick);
			// 
			// errorCodes
			// 
			this.errorCodes.Location = new System.Drawing.Point(209, 75);
			this.errorCodes.Margin = new System.Windows.Forms.Padding(2);
			this.errorCodes.Name = "errorCodes";
			this.errorCodes.Size = new System.Drawing.Size(168, 17);
			this.errorCodes.TabIndex = 10;
			this.errorCodes.Text = "Show error codes";
			this.errorCodes.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 160);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(245, 13);
			this.label2.TabIndex = 12;
			this.label2.Text = "Global source header (containing using directives):";
			// 
			// sourceHeader
			// 
			this.sourceHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.sourceHeader.BackColor = System.Drawing.SystemColors.Control;
			this.sourceHeader.Location = new System.Drawing.Point(8, 176);
			this.sourceHeader.Name = "sourceHeader";
			this.sourceHeader.Size = new System.Drawing.Size(396, 296);
			this.sourceHeader.TabIndex = 13;
			// 
			// autoAddNamespace
			// 
			this.autoAddNamespace.Location = new System.Drawing.Point(209, 97);
			this.autoAddNamespace.Name = "autoAddNamespace";
			this.autoAddNamespace.Size = new System.Drawing.Size(168, 45);
			this.autoAddNamespace.TabIndex = 14;
			this.autoAddNamespace.Text = "Automatically add namespace to global source header when compiling a library";
			this.autoAddNamespace.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button1.Location = new System.Drawing.Point(80, 493);
			this.button1.Margin = new System.Windows.Forms.Padding(2);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 23);
			this.button1.TabIndex = 15;
			this.button1.Text = "Apply";
			this.button1.Click += new System.EventHandler(this.applyClick);
			// 
			// CompilerOptionsForm
			// 
			this.AcceptButton = this.ok;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(413, 522);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.autoAddNamespace);
			this.Controls.Add(this.sourceHeader);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.errorCodes);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.ok);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.warnings);
			this.Controls.Add(this.allowunsafe);
			this.Controls.Add(this.Options);
			this.Controls.Add(this.debug);
			this.Controls.Add(this.overflow);
			this.Controls.Add(this.imports);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MinimumSize = new System.Drawing.Size(419, 468);
			this.Name = "CompilerOptionsForm";
			this.ShowInTaskbar = false;
			this.Text = "Compiler options";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

	
		public void Reset() {
			overflow.Checked = Compiler.Options.CheckOverflow;
			debug.Checked = Compiler.Options.Debug;
			Options.Checked = Compiler.Options.Optimize;
			allowunsafe.Checked = Compiler.Options.AllowUnsafe;
			warnings.SelectedIndex = Compiler.Options.WarningLevel;
			sourceHeader.Text = Compiler.Options.Header.Text;
			autoAddNamespace.Checked = Compiler.Options.AutoUseLibraryNamespace;
			errorCodes.Checked = CompilerErrors.ShowErrorNumbers;
		}

		public void Invalidate(object sender, EventArgs e) {
			Reset();
			sourceHeader.Invalidate();
		}

		public bool Apply() {
			Compiler.Options.CheckOverflow = overflow.Checked;
			Compiler.Options.Debug = debug.Checked;
			Compiler.Options.Optimize = Options.Checked;
			Compiler.Options.AllowUnsafe = allowunsafe.Checked;
			Compiler.Options.WarningLevel = warnings.SelectedIndex;
			Compiler.Options.AutoUseLibraryNamespace = autoAddNamespace.Checked; 
			CompilerErrors.ShowErrorNumbers = errorCodes.Checked;

			CompilerOptions.SourceHeader h = new CompilerOptions.SourceHeader();
			h.Text = sourceHeader.Text;
			sourceHeader.Text = h.Text;
			Compiler.Compile(h);
			if (h.CompilerResults.Errors.HasErrors) {
				sourceHeader.MarkErrors(h);
				return false;
			} else {
				Compiler.Options.Header.Text = h.Text;
				return true;
			}
		}
		
		private void impClick(object sender, System.EventArgs e) {
			if (importForm.IsDisposed) importForm = new ImportForm();
			importForm.Reset();
			if (importForm.WindowState == FormWindowState.Minimized) importForm.WindowState = FormWindowState.Normal;
			importForm.Show();
			importForm.BringToFront();
		}

		private void okClick(object sender, System.EventArgs e) {
			if (Apply()) this.Hide();
		}

		private void applyClick(object sender, System.EventArgs e) {
			Apply();
		}

		private void cancelClick(object sender, System.EventArgs e) {
			this.Hide();
		}

		public static void ShowForm() {
			if (Form == null || Form.IsDisposed) {
				Form = new CompilerOptionsForm();
			}
			if (Form.WindowState == FormWindowState.Minimized) Form.WindowState = FormWindowState.Normal;
			Form.Show();
			Form.BringToFront();
		}

		public static void MarkErrors(ICompilable item, CompilerError err) {
			ShowForm();
			if (Form != null && !Form.IsDisposed) {
				Form.sourceHeader.MarkErrors(item, true);
				int line = err.Line;
				if (line < 0) line = CodeControl.ErrorLine(item, true, line);
				Form.sourceHeader.SetCaret(line, err.Column);
			}
		}

	}
}
