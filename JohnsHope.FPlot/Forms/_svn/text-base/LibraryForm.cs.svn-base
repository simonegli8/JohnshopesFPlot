using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Text;
using JohnsHope.FPlot.Library;

namespace JohnsHope.FPlot
{
	/// <summary>
	/// Summary description for LibraryForm.
	/// </summary>
	public class LibraryForm : System.Windows.Forms.Form, IResetable, IEditForm
	{
		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.ToolStripMenuItem fileMenu;
		private System.Windows.Forms.ToolStripMenuItem openItem;
		private System.Windows.Forms.ToolStripMenuItem saveItem;
		private System.Windows.Forms.ToolStripMenuItem closeItem;
		private System.Windows.Forms.ToolStripMenuItem compileMenu;
		private System.Windows.Forms.ToolStripMenuItem compileItem;
		private System.Windows.Forms.ToolStripMenuItem optionsItem;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ToolStripMenuItem editMenu;
		private System.Windows.Forms.ToolStripMenuItem undoItem;
		private System.Windows.Forms.ToolStripMenuItem cutItem;
		private System.Windows.Forms.ToolStripMenuItem copyItem;
		private System.Windows.Forms.ToolStripMenuItem pasteItem;

		private MainModel Model;
		private ToolStripSeparator toolStripSeparator1;
		private JohnsHope.FPlot.Library.Library oldLibrary, library;
		private CodeControl editor;
		private ToolStripMenuItem saveToolStripMenuItem;

		private static int index = 0;
		private string oldSource;
		private bool modified, hasFile;
		private ToolStripMenuItem helpToolStripMenuItem;
		private ToolStripMenuItem redoToolStripMenuItem;
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripMenuItem helpToolStripMenuItem1;
		private ToolStripMenuItem helpOnJohnsHopeFPlotLibraryToolStripMenuItem;

		public LibraryForm(MainModel Model, JohnsHope.FPlot.Library.Library library) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.Model = Model;
			oldLibrary = library;
			this.library = new JohnsHope.FPlot.Library.Library();
			if (oldLibrary == null) {
				this.library.Name = Properties.Resources.Library + " " + index++ + ".cs";
			} else {
				this.library.CopyFrom(oldLibrary);
			}
			hasFile = File.Exists(this.library.Filename);
			modified = hasFile && File.ReadAllText(this.library.Filename) != this.library.Source;

			Reset();

			editor.TextChanged += Modify;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				Apply();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LibraryForm));
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.openItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.closeItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.undoItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.cutItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteItem = new System.Windows.Forms.ToolStripMenuItem();
			this.compileMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.compileItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.helpOnJohnsHopeFPlotLibraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.editor = new JohnsHope.FPlot.CodeControl();
			this.mainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.editMenu,
            this.compileMenu,
            this.helpToolStripMenuItem});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
			this.mainMenu.Size = new System.Drawing.Size(538, 24);
			this.mainMenu.TabIndex = 0;
			// 
			// fileMenu
			// 
			this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openItem,
            this.saveToolStripMenuItem,
            this.saveItem,
            this.toolStripSeparator1,
            this.closeItem});
			this.fileMenu.Name = "fileMenu";
			this.fileMenu.Size = new System.Drawing.Size(37, 20);
			this.fileMenu.Text = "&File";
			// 
			// openItem
			// 
			this.openItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuFileOpenIcon;
			this.openItem.Name = "openItem";
			this.openItem.Size = new System.Drawing.Size(121, 22);
			this.openItem.Text = "&Open...";
			this.openItem.Click += new System.EventHandler(this.openClick);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuFileSaveIcon;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveClick);
			// 
			// saveItem
			// 
			this.saveItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuFileSaveAsIcon;
			this.saveItem.Name = "saveItem";
			this.saveItem.Size = new System.Drawing.Size(121, 22);
			this.saveItem.Text = "Save &as...";
			this.saveItem.Click += new System.EventHandler(this.saveAsClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(118, 6);
			// 
			// closeItem
			// 
			this.closeItem.Name = "closeItem";
			this.closeItem.Size = new System.Drawing.Size(121, 22);
			this.closeItem.Text = "&Close";
			this.closeItem.Click += new System.EventHandler(this.closeClick);
			// 
			// editMenu
			// 
			this.editMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoItem,
            this.redoToolStripMenuItem,
            this.toolStripMenuItem1,
            this.cutItem,
            this.copyItem,
            this.pasteItem});
			this.editMenu.Name = "editMenu";
			this.editMenu.Size = new System.Drawing.Size(39, 20);
			this.editMenu.Text = "&Edit";
			// 
			// undoItem
			// 
			this.undoItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditUndoIcon;
			this.undoItem.Name = "undoItem";
			this.undoItem.Size = new System.Drawing.Size(103, 22);
			this.undoItem.Text = "&Undo";
			this.undoItem.Click += new System.EventHandler(this.undoClick);
			// 
			// redoToolStripMenuItem
			// 
			this.redoToolStripMenuItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditRedoIcon;
			this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
			this.redoToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.redoToolStripMenuItem.Text = "&Redo";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 6);
			// 
			// cutItem
			// 
			this.cutItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditCutIcon;
			this.cutItem.Name = "cutItem";
			this.cutItem.Size = new System.Drawing.Size(103, 22);
			this.cutItem.Text = "Cu&t";
			this.cutItem.Click += new System.EventHandler(this.cutClick);
			// 
			// copyItem
			// 
			this.copyItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditCopyIcon;
			this.copyItem.Name = "copyItem";
			this.copyItem.Size = new System.Drawing.Size(103, 22);
			this.copyItem.Text = "&Copy";
			this.copyItem.Click += new System.EventHandler(this.copyClick);
			// 
			// pasteItem
			// 
			this.pasteItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditPasteIcon;
			this.pasteItem.Name = "pasteItem";
			this.pasteItem.Size = new System.Drawing.Size(103, 22);
			this.pasteItem.Text = "&Paste";
			this.pasteItem.Click += new System.EventHandler(this.pasteClick);
			// 
			// compileMenu
			// 
			this.compileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compileItem,
            this.optionsItem});
			this.compileMenu.Name = "compileMenu";
			this.compileMenu.Size = new System.Drawing.Size(64, 20);
			this.compileMenu.Text = "&Compile";
			// 
			// compileItem
			// 
			this.compileItem.Image = ((System.Drawing.Image)(resources.GetObject("compileItem.Image")));
			this.compileItem.Name = "compileItem";
			this.compileItem.Size = new System.Drawing.Size(175, 22);
			this.compileItem.Text = "&Compile...";
			this.compileItem.Click += new System.EventHandler(this.compileClick);
			// 
			// optionsItem
			// 
			this.optionsItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuLayersLayerPropertiesIcon;
			this.optionsItem.Name = "optionsItem";
			this.optionsItem.Size = new System.Drawing.Size(175, 22);
			this.optionsItem.Text = "Compiler &options...";
			this.optionsItem.Click += new System.EventHandler(this.optionsClick);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.helpOnJohnsHopeFPlotLibraryToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// helpToolStripMenuItem1
			// 
			this.helpToolStripMenuItem1.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuHelpHelpTopicsIcon;
			this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
			this.helpToolStripMenuItem1.Size = new System.Drawing.Size(256, 22);
			this.helpToolStripMenuItem1.Text = "Help...";
			// 
			// helpOnJohnsHopeFPlotLibraryToolStripMenuItem
			// 
			this.helpOnJohnsHopeFPlotLibraryToolStripMenuItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuHelpTutorialsIcon;
			this.helpOnJohnsHopeFPlotLibraryToolStripMenuItem.Name = "helpOnJohnsHopeFPlotLibraryToolStripMenuItem";
			this.helpOnJohnsHopeFPlotLibraryToolStripMenuItem.Size = new System.Drawing.Size(256, 22);
			this.helpOnJohnsHopeFPlotLibraryToolStripMenuItem.Text = "Help on JohnsHope.FPlot.Library...";
			this.helpOnJohnsHopeFPlotLibraryToolStripMenuItem.Click += new System.EventHandler(this.helpClick);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "Source files (*.cs; *.c; *.cpp; *.txt)|*.cs;*.c;*.cpp;*.txt|All files|*.*";
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "cs";
			this.saveFileDialog.Filter = "Source files (*.cs; *.c; *.cpp; *.txt)|*.cs;*.c;*.cpp;*.txt|All files|*.*";
			// 
			// editor
			// 
			this.editor.AllowDrop = true;
			this.editor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.editor.BackColor = System.Drawing.SystemColors.Control;
			this.editor.Location = new System.Drawing.Point(0, 25);
			this.editor.Margin = new System.Windows.Forms.Padding(2);
			this.editor.Name = "editor";
			this.editor.Size = new System.Drawing.Size(538, 517);
			this.editor.TabIndex = 1;
			// 
			// LibraryForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(538, 542);
			this.Controls.Add(this.editor);
			this.Controls.Add(this.mainMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.mainMenu;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "LibraryForm";
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		public void SetTitle() {
			string name = library.Name;
			if (modified) name += "*";
			this.Text = name + " " + Properties.Resources.Version + ": " + library.Version.ToShortDateString();
		}

		public void Modify(object sender, EventArgs e) { modified = hasFile; SetTitle(); } 

		public void Reset() {
			oldSource = library.Source;
			editor.Text = library.Source;
			SetTitle();
		}

		public void Apply() {
			library.Source = editor.Text;
			if (oldLibrary == null) {
				Model.Items.Add(library); oldLibrary = library;
			} else if (oldLibrary != library) {
				Model.Items.Replace(oldLibrary, library); oldLibrary = library;
			}
			if (oldSource != library.Source) library.Version = DateTime.Now; 
		}

		private void closeClick(object sender, System.EventArgs e) {
			Apply();
			this.Hide();
		}

		private void openClick(object sender, System.EventArgs e) {
			DialogResult res = openFileDialog.ShowDialog();
			if (res == DialogResult.OK) {
				editor.Editor.LoadFile(openFileDialog.FileName, true, true);
				modified = false;
				hasFile = true;
				library.Filename = openFileDialog.FileName;
				library.Name = Path.GetFileName(openFileDialog.FileName);
				Apply();
				library.Version = File.GetLastWriteTime(openFileDialog.FileName);
				Model.Items.Update(library);
				Reset();
				Compiler.Compile(library);
			}
		}

		private void saveClick(object sender, EventArgs e) {
			Apply();
			try {
				if (string.IsNullOrEmpty(library.Filename)) {
					saveFileDialog.FileName = library.Name;
					DialogResult res = saveFileDialog.ShowDialog();
					if (res == DialogResult.OK) {
						library.SaveAs(saveFileDialog.FileName);
						modified = false;
						hasFile = true;
						Model.Items.Update(library);
						Reset();
					}
				} else {
					library.Save();
					modified = false;
					hasFile = true;
					SetTitle();
				}
			} catch (Exception) {
				MessageBox.Show("Error saving file " + library.Filename);
			}
		}

		private void saveAsClick(object sender, System.EventArgs e) {
			Apply();
			saveFileDialog.FileName = library.Name;
			DialogResult res = saveFileDialog.ShowDialog();
			if (res == DialogResult.OK) {
				try {
					library.SaveAs(saveFileDialog.FileName);
					modified = false;
					hasFile = true;
				} catch (Exception) {
					MessageBox.Show("Error saving file " + saveFileDialog.FileName);
				}
				Model.Items.Update(library);
				Reset();
			}
		}

		private void compileClick(object sender, System.EventArgs e) {
			Apply();
			library.Modified = false;
			Compiler.Compile(library);
			editor.MarkErrors(library);
			if (library.Modified) Model.Items.Invalidate();
			if (hasFile) {
				try {
					library.Save();
					modified = false;
					hasFile = true;
					SetTitle();
				} catch (Exception) {
					MessageBox.Show("Error saving file " + library.Filename);
				}
			}
		}

		private void optionsClick(object sender, System.EventArgs e) {
			CompilerOptionsForm.ShowForm();
		}

		private void undoClick(object sender, System.EventArgs e) {
			editor.Editor.Undo();
		}

		private void cutClick(object sender, System.EventArgs e) {
			editor.Editor.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);
		}

		private void copyClick(object sender, System.EventArgs e) {
			editor.Editor.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);
		}

		private void pasteClick(object sender, System.EventArgs e) {
			editor.Editor.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);
		}
		
		public void helpClick(object sender, EventArgs e) {
			Help.ShowHelp(this, Properties.Resources.HelpFile, "LibraryFrom.html");
		}

		public void helpLibraryClick(object sender, EventArgs e) {
			Help.ShowHelp(this, Properties.Resources.LibraryHelpFile);
		}

		public void Edit(SourceLocation loc) {
			editor.Edit(loc);
		}


	}
}
