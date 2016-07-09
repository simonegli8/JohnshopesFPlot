using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;
using JohnsHope.FPlot.Library;
using JohnsHope.FPlot;
using JohnsHope.FPlot.Properties;
using JohnsHope.FPlot.Util;

namespace JohnsHope.FPlot
{
	/// <summary>
	/// The main form of the Application
	/// </summary>
	public class MainForm: System.Windows.Forms.Form, IItemEventHandler
	{
		const bool hideUndo = true; //TODO: Implement Undo

		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private OpenFileDialog newFileDialog;

		private FitForm fitForm;
		public AboutForm aboutForm;
		public EvalForm evalForm;
		public MainModel Model;
		public ItemsGrid grid;
		private CheckItems check;

		private ToolStripMenuItem fileMenu;
		private ToolStripMenuItem newItem;
		private ToolStripMenuItem openItem;
		private ToolStripMenuItem saveItem;
		private ToolStripMenuItem saveAsItem;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripMenuItem exitItem;
		private ToolStripMenuItem developMenu;
		private ToolStripMenuItem debugItem;
		private ToolStripMenuItem compilerItem;
		private ToolStripMenuItem fitMenu;
		private ToolStripMenuItem evalItem;
		private ToolStripMenuItem fitItem;
		private ToolStripMenuItem helpMenu;
		private ToolStripMenuItem helpContents;
		private ToolStripMenuItem helpAbout;
		private MenuStrip mainMenu;
		private ToolStripMenuItem plotMenu;
		private ToolStripMenuItem new2DPlotToolStripMenuItem;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripMenuItem applySelectionItem;
		private ToolStripMenuItem editToolStripMenuItem;
		private ToolStripMenuItem newFunctionToolStripMenuItem;
		private ToolStripSeparator toolStripMenuItem1;
		private OpenFileDialog newLibraryDialog;
		private ToolStripMenuItem new3DPlotToolStripMenuItem;
		private ToolStripSeparator toolStripMenuItem3;
		private ToolStripMenuItem closeToolStripMenuItem;
		private IContainer components;
		private ToolStripMenuItem copyToolStripMenuItem;
		private ToolStripMenuItem pasteToolStripMenuItem;
		private ToolStripMenuItem functionToolStripMenuItem;
		private ToolStripMenuItem dataToolStripMenuItem;
		private ToolStripMenuItem libraryToolStripMenuItem;
		private ToolStrip toolStrip1;
		private ToolStripButton toolStripButton1;
		private ToolStripButton toolStripButton2;
		private ToolStripButton toolStripButton3;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripButton toolStripButton5;
		private ToolStripButton toolStripButton6;
		private ToolStripButton toolStripButton7;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripButton toolStripButton10;
		private ToolStripButton toolStripButton8;
		private ToolStripButton toolStripButton9;
		private ToolStripButton toolStripButton11;
		private ToolStrip toolStrip2;
		private ToolStripButton EditButton;
		private ToolStripButton ParButton;
		private ToolStripSeparator StyleSeparator;
		private ToolStripButton New2DPlotButton;
		private ToolStripSeparator undoMenuSeprarator;
		private ToolStripMenuItem undoToolStripMenuItem;
		private ToolStripMenuItem redoToolStripMenuItem;
		private ToolStripButton UndoButton;
		private ToolStripButton RedoButton;
		private ToolStripSeparator UndoSeparator;
		private ToolStripSeparator PlotSeparator;
		private ToolStripButton FitButton;
		private ToolStripButton EvalButton;
		private ToolStripSeparator CalcSeparator;
		private ToolStripButton CommandButton;
		private ToolStripButton CompilerOptionsButton;
		private ToolStripButton StyleButton;
		private ToolStripButton toolStripButton4;
		private ToolStripButton UpdatePlotButton;
		private ToolStripSeparator toolStripSeparator9;
		private ToolStripButton toolStripButton12;
		private ToolStripButton toolStripButton13;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private DataGridViewTextBoxColumn Column1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn24;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn25;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn26;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn27;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn28;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn29;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn30;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn31;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn32;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn33;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn34;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn35;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn36;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn37;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn38;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn39;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn40;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn41;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn42;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn43;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn44;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn45;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn46;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn47;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn48;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn49;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn50;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn51;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn52;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn53;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn54;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn55;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn56;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn57;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn58;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn59;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn60;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn61;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn62;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn63;
		private ToolStripMenuItem deleteSelectionToolStripMenuItem;
		//private ToolStripMenuItem AddItem;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			Model = new MainModel(this, grid);

			grid.Reset();
			grid.SelectionChanged += SelectionChanged;

			fitForm = new FitForm(Model);
			aboutForm = new AboutForm();
			PlotForm.FormActivated += NotifyTopForm;
			check = new CheckItems(this);
			Model.Items.Handlers += this;

			UndoButton.Visible = RedoButton.Visible = UndoSeparator.Visible = 
				undoMenuSeprarator.Visible = undoToolStripMenuItem.Visible = redoToolStripMenuItem.Visible = !hideUndo;
	
			SelectionChanged(this, EventArgs.Empty);
			ResetCalcButtons();
		}
		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.newFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.newItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.saveItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.exitItem = new System.Windows.Forms.ToolStripMenuItem();
			this.developMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.debugItem = new System.Windows.Forms.ToolStripMenuItem();
			this.compilerItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fitMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.fitItem = new System.Windows.Forms.ToolStripMenuItem();
			this.evalItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.helpContents = new System.Windows.Forms.ToolStripMenuItem();
			this.helpAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newFunctionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.functionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.libraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.deleteSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.undoMenuSeprarator = new System.Windows.Forms.ToolStripSeparator();
			this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.plotMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.new2DPlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.new3DPlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.applySelectionItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newLibraryDialog = new System.Windows.Forms.OpenFileDialog();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.UndoButton = new System.Windows.Forms.ToolStripButton();
			this.RedoButton = new System.Windows.Forms.ToolStripButton();
			this.UndoSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton12 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton13 = new System.Windows.Forms.ToolStripButton();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.EditButton = new System.Windows.Forms.ToolStripButton();
			this.ParButton = new System.Windows.Forms.ToolStripButton();
			this.StyleButton = new System.Windows.Forms.ToolStripButton();
			this.StyleSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.New2DPlotButton = new System.Windows.Forms.ToolStripButton();
			this.UpdatePlotButton = new System.Windows.Forms.ToolStripButton();
			this.PlotSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.FitButton = new System.Windows.Forms.ToolStripButton();
			this.EvalButton = new System.Windows.Forms.ToolStripButton();
			this.CalcSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.CommandButton = new System.Windows.Forms.ToolStripButton();
			this.CompilerOptionsButton = new System.Windows.Forms.ToolStripButton();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn26 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn27 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn28 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn29 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn30 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn31 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn32 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn33 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn34 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn35 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn36 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn37 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn38 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn39 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn40 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn41 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn42 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn43 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn44 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn45 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn46 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn47 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn48 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn49 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn50 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn51 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn55 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn56 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn57 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn52 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn53 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn54 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn58 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn59 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn60 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.grid = new JohnsHope.FPlot.ItemsGrid();
			this.dataGridViewTextBoxColumn61 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn62 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn63 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.mainMenu.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
			this.SuspendLayout();
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "*.fplot";
			this.openFileDialog.Filter = "Plot files (*.fplot)|*.fplot";
			this.openFileDialog.Title = "Open items";
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "*.fplot";
			this.saveFileDialog.Filter = "Plot files (*.fplot)|*.fplot";
			this.saveFileDialog.Title = "Save items";
			// 
			// newFileDialog
			// 
			this.newFileDialog.CheckFileExists = false;
			this.newFileDialog.DefaultExt = "*.fplot";
			this.newFileDialog.Filter = "Function files (*.fplot)|*.fplot";
			this.newFileDialog.Title = "New item collection";
			// 
			// fileMenu
			// 
			this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newItem,
            this.openItem,
            this.closeToolStripMenuItem,
            this.toolStripMenuItem3,
            this.saveItem,
            this.saveAsItem,
            this.toolStripSeparator3,
            this.exitItem});
			this.fileMenu.Name = "fileMenu";
			this.fileMenu.Size = new System.Drawing.Size(37, 20);
			this.fileMenu.Text = "&File";
			// 
			// newItem
			// 
			this.newItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuFileNewIcon;
			this.newItem.Name = "newItem";
			this.newItem.Size = new System.Drawing.Size(123, 22);
			this.newItem.Text = "&New...";
			this.newItem.Click += new System.EventHandler(this.newItem_Click);
			// 
			// openItem
			// 
			this.openItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuFileOpenIcon;
			this.openItem.Name = "openItem";
			this.openItem.Size = new System.Drawing.Size(123, 22);
			this.openItem.Text = "&Open...";
			this.openItem.Click += new System.EventHandler(this.openItem_Click);
			// 
			// closeToolStripMenuItem
			// 
			this.closeToolStripMenuItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuFileCloseIcon;
			this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
			this.closeToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
			this.closeToolStripMenuItem.Text = "&Close";
			this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeItem_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(120, 6);
			// 
			// saveItem
			// 
			this.saveItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuFileSaveIcon;
			this.saveItem.Name = "saveItem";
			this.saveItem.Size = new System.Drawing.Size(123, 22);
			this.saveItem.Text = "&Save";
			this.saveItem.Click += new System.EventHandler(this.saveItem_Click);
			// 
			// saveAsItem
			// 
			this.saveAsItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuFileSaveAsIcon;
			this.saveAsItem.Name = "saveAsItem";
			this.saveAsItem.Size = new System.Drawing.Size(123, 22);
			this.saveAsItem.Text = "Save &As...";
			this.saveAsItem.Click += new System.EventHandler(this.saveAsItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(120, 6);
			// 
			// exitItem
			// 
			this.exitItem.Name = "exitItem";
			this.exitItem.Size = new System.Drawing.Size(123, 22);
			this.exitItem.Text = "E&xit";
			this.exitItem.Click += new System.EventHandler(this.exitClick);
			// 
			// developMenu
			// 
			this.developMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debugItem,
            this.compilerItem});
			this.developMenu.Name = "developMenu";
			this.developMenu.Size = new System.Drawing.Size(62, 20);
			this.developMenu.Text = "&Develop";
			// 
			// debugItem
			// 
			this.debugItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_cmd;
			this.debugItem.Name = "debugItem";
			this.debugItem.Size = new System.Drawing.Size(177, 22);
			this.debugItem.Text = "&Console Window...";
			this.debugItem.Click += new System.EventHandler(this.consoleClick);
			// 
			// compilerItem
			// 
			this.compilerItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuLayersLayerPropertiesIcon;
			this.compilerItem.Name = "compilerItem";
			this.compilerItem.Size = new System.Drawing.Size(177, 22);
			this.compilerItem.Text = "&Compiler Options...";
			this.compilerItem.Click += new System.EventHandler(this.compilerClick);
			// 
			// fitMenu
			// 
			this.fitMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fitItem,
            this.evalItem});
			this.fitMenu.Name = "fitMenu";
			this.fitMenu.Size = new System.Drawing.Size(42, 20);
			this.fitMenu.Text = "&Calc";
			// 
			// fitItem
			// 
			this.fitItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_Fit;
			this.fitItem.Name = "fitItem";
			this.fitItem.Size = new System.Drawing.Size(177, 22);
			this.fitItem.Text = "&Fit...";
			this.fitItem.Click += new System.EventHandler(this.fitClick);
			// 
			// evalItem
			// 
			this.evalItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_Eval;
			this.evalItem.Name = "evalItem";
			this.evalItem.Size = new System.Drawing.Size(177, 22);
			this.evalItem.Text = "&Evaluate Function...";
			this.evalItem.Click += new System.EventHandler(this.evalClick);
			// 
			// helpMenu
			// 
			this.helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpContents,
            this.helpAbout});
			this.helpMenu.Name = "helpMenu";
			this.helpMenu.Size = new System.Drawing.Size(44, 20);
			this.helpMenu.Text = "&Help";
			// 
			// helpContents
			// 
			this.helpContents.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuHelpHelpTopicsIcon;
			this.helpContents.Name = "helpContents";
			this.helpContents.Size = new System.Drawing.Size(131, 22);
			this.helpContents.Text = "&Contents...";
			this.helpContents.Click += new System.EventHandler(this.helpContentsClick);
			// 
			// helpAbout
			// 
			this.helpAbout.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_JohnsHope;
			this.helpAbout.Name = "helpAbout";
			this.helpAbout.Size = new System.Drawing.Size(131, 22);
			this.helpAbout.Text = "&About...";
			this.helpAbout.Click += new System.EventHandler(this.helpAbout_Click);
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.editToolStripMenuItem,
            this.plotMenu,
            this.fitMenu,
            this.developMenu,
            this.helpMenu});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
			this.mainMenu.Size = new System.Drawing.Size(380, 24);
			this.mainMenu.TabIndex = 11;
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFunctionToolStripMenuItem,
            this.toolStripMenuItem1,
            this.deleteSelectionToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.undoMenuSeprarator,
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// newFunctionToolStripMenuItem
			// 
			this.newFunctionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.functionToolStripMenuItem,
            this.dataToolStripMenuItem,
            this.libraryToolStripMenuItem});
			this.newFunctionToolStripMenuItem.Name = "newFunctionToolStripMenuItem";
			this.newFunctionToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.newFunctionToolStripMenuItem.Text = "&New";
			this.newFunctionToolStripMenuItem.Click += new System.EventHandler(this.addFunctionClick);
			// 
			// functionToolStripMenuItem
			// 
			this.functionToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("functionToolStripMenuItem.Image")));
			this.functionToolStripMenuItem.Name = "functionToolStripMenuItem";
			this.functionToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.functionToolStripMenuItem.Text = "&Function...";
			this.functionToolStripMenuItem.Click += new System.EventHandler(this.addFunctionClick);
			// 
			// dataToolStripMenuItem
			// 
			this.dataToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("dataToolStripMenuItem.Image")));
			this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
			this.dataToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.dataToolStripMenuItem.Text = "&Data...";
			this.dataToolStripMenuItem.Click += new System.EventHandler(this.dataClick);
			// 
			// libraryToolStripMenuItem
			// 
			this.libraryToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("libraryToolStripMenuItem.Image")));
			this.libraryToolStripMenuItem.Name = "libraryToolStripMenuItem";
			this.libraryToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.libraryToolStripMenuItem.Text = "&Library...";
			this.libraryToolStripMenuItem.Click += new System.EventHandler(this.libraryClick);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 6);
			// 
			// deleteSelectionToolStripMenuItem
			// 
			this.deleteSelectionToolStripMenuItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditCutIcon;
			this.deleteSelectionToolStripMenuItem.Name = "deleteSelectionToolStripMenuItem";
			this.deleteSelectionToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.deleteSelectionToolStripMenuItem.Text = "Cut";
			this.deleteSelectionToolStripMenuItem.Click += new System.EventHandler(this.cutClick);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditCopyIcon;
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyClick);
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditPasteIcon;
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.pasteToolStripMenuItem.Text = "Paste";
			this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteClick);
			// 
			// undoMenuSeprarator
			// 
			this.undoMenuSeprarator.Name = "undoMenuSeprarator";
			this.undoMenuSeprarator.Size = new System.Drawing.Size(100, 6);
			// 
			// undoToolStripMenuItem
			// 
			this.undoToolStripMenuItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditUndoIcon;
			this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
			this.undoToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.undoToolStripMenuItem.Text = "Undo";
			// 
			// redoToolStripMenuItem
			// 
			this.redoToolStripMenuItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditRedoIcon;
			this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
			this.redoToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.redoToolStripMenuItem.Text = "Redo";
			// 
			// plotMenu
			// 
			this.plotMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.new2DPlotToolStripMenuItem,
            this.new3DPlotToolStripMenuItem,
            this.toolStripMenuItem2,
            this.applySelectionItem});
			this.plotMenu.Name = "plotMenu";
			this.plotMenu.Size = new System.Drawing.Size(40, 20);
			this.plotMenu.Text = "&Plot";
			// 
			// new2DPlotToolStripMenuItem
			// 
			this.new2DPlotToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("new2DPlotToolStripMenuItem.Image")));
			this.new2DPlotToolStripMenuItem.Name = "new2DPlotToolStripMenuItem";
			this.new2DPlotToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
			this.new2DPlotToolStripMenuItem.Text = "New &2D Plot";
			this.new2DPlotToolStripMenuItem.Click += new System.EventHandler(this.new2DPlotToolStripMenuItem_Click);
			// 
			// new3DPlotToolStripMenuItem
			// 
			this.new3DPlotToolStripMenuItem.Name = "new3DPlotToolStripMenuItem";
			this.new3DPlotToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
			this.new3DPlotToolStripMenuItem.Text = "New &3D Plot";
			this.new3DPlotToolStripMenuItem.Visible = false;
			this.new3DPlotToolStripMenuItem.Click += new System.EventHandler(this.Plot3DClick);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(237, 6);
			// 
			// applySelectionItem
			// 
			this.applySelectionItem.Enabled = false;
			this.applySelectionItem.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_UpdatePlot;
			this.applySelectionItem.Name = "applySelectionItem";
			this.applySelectionItem.Size = new System.Drawing.Size(240, 22);
			this.applySelectionItem.Text = "Apply Selection To Current Plot";
			this.applySelectionItem.Click += new System.EventHandler(this.apply_Click);
			// 
			// newLibraryDialog
			// 
			this.newLibraryDialog.CheckFileExists = false;
			this.newLibraryDialog.CheckPathExists = false;
			this.newLibraryDialog.DefaultExt = "*.cs";
			this.newLibraryDialog.Filter = "C# files (*.cs)|*.cs";
			this.newLibraryDialog.Title = "New library";
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton10,
            this.toolStripSeparator1,
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripButton7,
            this.toolStripSeparator2,
            this.UndoButton,
            this.RedoButton,
            this.UndoSeparator,
            this.toolStripButton8,
            this.toolStripButton9,
            this.toolStripButton11,
            this.toolStripSeparator9,
            this.toolStripButton12,
            this.toolStripButton13});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(380, 25);
			this.toolStrip1.TabIndex = 13;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuFileNewIcon;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "toolStripButton1";
			this.toolStripButton1.ToolTipText = "New...";
			this.toolStripButton1.Click += new System.EventHandler(this.newItem_Click);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuFileOpenIcon;
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton2.Text = "toolStripButton2";
			this.toolStripButton2.ToolTipText = "Open...";
			this.toolStripButton2.Click += new System.EventHandler(this.openItem_Click);
			// 
			// toolStripButton3
			// 
			this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton3.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuFileSaveIcon;
			this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton3.Name = "toolStripButton3";
			this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton3.Text = "toolStripButton3";
			this.toolStripButton3.ToolTipText = "Save";
			this.toolStripButton3.Click += new System.EventHandler(this.saveItem_Click);
			// 
			// toolStripButton4
			// 
			this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton4.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuFileSaveAsIcon;
			this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton4.Name = "toolStripButton4";
			this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton4.Text = "toolStripButton4";
			this.toolStripButton4.ToolTipText = "Save As...";
			this.toolStripButton4.Click += new System.EventHandler(this.saveAsItem_Click);
			// 
			// toolStripButton10
			// 
			this.toolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton10.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuFileCloseIcon;
			this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton10.Name = "toolStripButton10";
			this.toolStripButton10.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton10.Text = "toolStripButton10";
			this.toolStripButton10.ToolTipText = "Close";
			this.toolStripButton10.Click += new System.EventHandler(this.closeItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButton5
			// 
			this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton5.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditCutIcon;
			this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton5.Name = "toolStripButton5";
			this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton5.Text = "toolStripButton5";
			this.toolStripButton5.ToolTipText = "Cut";
			this.toolStripButton5.Click += new System.EventHandler(this.cutClick);
			// 
			// toolStripButton6
			// 
			this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton6.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditCopyIcon;
			this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton6.Name = "toolStripButton6";
			this.toolStripButton6.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton6.Text = "toolStripButton6";
			this.toolStripButton6.ToolTipText = "Copy";
			this.toolStripButton6.Click += new System.EventHandler(this.copyClick);
			// 
			// toolStripButton7
			// 
			this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton7.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditPasteIcon;
			this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton7.Name = "toolStripButton7";
			this.toolStripButton7.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton7.Text = "toolStripButton7";
			this.toolStripButton7.ToolTipText = "Paste";
			this.toolStripButton7.Click += new System.EventHandler(this.pasteClick);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// UndoButton
			// 
			this.UndoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.UndoButton.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditUndoIcon;
			this.UndoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.UndoButton.Name = "UndoButton";
			this.UndoButton.Size = new System.Drawing.Size(23, 22);
			this.UndoButton.Text = "toolStripButton21";
			this.UndoButton.ToolTipText = "Undo";
			// 
			// RedoButton
			// 
			this.RedoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.RedoButton.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditRedoIcon;
			this.RedoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RedoButton.Name = "RedoButton";
			this.RedoButton.Size = new System.Drawing.Size(23, 22);
			this.RedoButton.Text = "toolStripButton22";
			this.RedoButton.ToolTipText = "Redo";
			// 
			// UndoSeparator
			// 
			this.UndoSeparator.Name = "UndoSeparator";
			this.UndoSeparator.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButton8
			// 
			this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton8.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton8.Image")));
			this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton8.Name = "toolStripButton8";
			this.toolStripButton8.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton8.Text = "toolStripButton8";
			this.toolStripButton8.ToolTipText = "New Function...";
			this.toolStripButton8.Click += new System.EventHandler(this.addFunctionClick);
			// 
			// toolStripButton9
			// 
			this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton9.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton9.Image")));
			this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton9.Name = "toolStripButton9";
			this.toolStripButton9.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton9.Text = "toolStripButton9";
			this.toolStripButton9.ToolTipText = "New Data...";
			this.toolStripButton9.Click += new System.EventHandler(this.dataClick);
			// 
			// toolStripButton11
			// 
			this.toolStripButton11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton11.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton11.Image")));
			this.toolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton11.Name = "toolStripButton11";
			this.toolStripButton11.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton11.Text = "toolStripButton11";
			this.toolStripButton11.ToolTipText = "New Library...";
			this.toolStripButton11.Click += new System.EventHandler(this.libraryClick);
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButton12
			// 
			this.toolStripButton12.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton12.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuHelpHelpTopicsIcon;
			this.toolStripButton12.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton12.Name = "toolStripButton12";
			this.toolStripButton12.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton12.Text = "toolStripButton12";
			this.toolStripButton12.ToolTipText = "Help...";
			this.toolStripButton12.Click += new System.EventHandler(this.helpContentsClick);
			// 
			// toolStripButton13
			// 
			this.toolStripButton13.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton13.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_JohnsHope;
			this.toolStripButton13.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton13.Name = "toolStripButton13";
			this.toolStripButton13.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton13.Text = "toolStripButton13";
			this.toolStripButton13.ToolTipText = "About...";
			this.toolStripButton13.Click += new System.EventHandler(this.helpAbout_Click);
			// 
			// toolStrip2
			// 
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditButton,
            this.ParButton,
            this.StyleButton,
            this.StyleSeparator,
            this.New2DPlotButton,
            this.UpdatePlotButton,
            this.PlotSeparator,
            this.FitButton,
            this.EvalButton,
            this.CalcSeparator,
            this.CommandButton,
            this.CompilerOptionsButton});
			this.toolStrip2.Location = new System.Drawing.Point(0, 49);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(380, 25);
			this.toolStrip2.TabIndex = 14;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// EditButton
			// 
			this.EditButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.EditButton.Image = global::JohnsHope.FPlot.Properties.Resources.glass;
			this.EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.EditButton.Name = "EditButton";
			this.EditButton.Size = new System.Drawing.Size(23, 22);
			this.EditButton.Text = "EditButton";
			this.EditButton.ToolTipText = "Edit...";
			this.EditButton.Click += new System.EventHandler(this.editClick);
			// 
			// ParButton
			// 
			this.ParButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ParButton.Image = global::JohnsHope.FPlot.Properties.Resources.par;
			this.ParButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ParButton.Name = "ParButton";
			this.ParButton.Size = new System.Drawing.Size(23, 22);
			this.ParButton.Text = "ParButton";
			this.ParButton.ToolTipText = "Edit Function Parameters...";
			this.ParButton.Click += new System.EventHandler(this.parClick);
			// 
			// StyleButton
			// 
			this.StyleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.StyleButton.Image = ((System.Drawing.Image)(resources.GetObject("StyleButton.Image")));
			this.StyleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.StyleButton.Name = "StyleButton";
			this.StyleButton.Size = new System.Drawing.Size(23, 22);
			this.StyleButton.Text = "toolStripButton25";
			this.StyleButton.ToolTipText = "Choose Item Style...";
			this.StyleButton.Click += new System.EventHandler(this.lineStyleClick);
			// 
			// StyleSeparator
			// 
			this.StyleSeparator.Name = "StyleSeparator";
			this.StyleSeparator.Size = new System.Drawing.Size(6, 25);
			// 
			// New2DPlotButton
			// 
			this.New2DPlotButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.New2DPlotButton.Image = ((System.Drawing.Image)(resources.GetObject("New2DPlotButton.Image")));
			this.New2DPlotButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.New2DPlotButton.Name = "New2DPlotButton";
			this.New2DPlotButton.Size = new System.Drawing.Size(23, 22);
			this.New2DPlotButton.Text = "toolStripButton14";
			this.New2DPlotButton.ToolTipText = "New 2D Plot...";
			this.New2DPlotButton.Click += new System.EventHandler(this.new2DPlotToolStripMenuItem_Click);
			// 
			// UpdatePlotButton
			// 
			this.UpdatePlotButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.UpdatePlotButton.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_UpdatePlot;
			this.UpdatePlotButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.UpdatePlotButton.Name = "UpdatePlotButton";
			this.UpdatePlotButton.Size = new System.Drawing.Size(23, 22);
			this.UpdatePlotButton.Text = "toolStripButton26";
			this.UpdatePlotButton.ToolTipText = "Apply Selection to Plot";
			this.UpdatePlotButton.Click += new System.EventHandler(this.apply_Click);
			// 
			// PlotSeparator
			// 
			this.PlotSeparator.Name = "PlotSeparator";
			this.PlotSeparator.Size = new System.Drawing.Size(6, 25);
			// 
			// FitButton
			// 
			this.FitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.FitButton.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_Fit;
			this.FitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.FitButton.Name = "FitButton";
			this.FitButton.Size = new System.Drawing.Size(23, 22);
			this.FitButton.Text = "toolStripButton15";
			this.FitButton.ToolTipText = "Fit Function to Data...";
			this.FitButton.Click += new System.EventHandler(this.fitClick);
			// 
			// EvalButton
			// 
			this.EvalButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.EvalButton.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_Eval;
			this.EvalButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.EvalButton.Name = "EvalButton";
			this.EvalButton.Size = new System.Drawing.Size(23, 22);
			this.EvalButton.Text = "toolStripButton16";
			this.EvalButton.ToolTipText = "Evaluate Function...";
			this.EvalButton.Click += new System.EventHandler(this.evalClick);
			// 
			// CalcSeparator
			// 
			this.CalcSeparator.Name = "CalcSeparator";
			this.CalcSeparator.Size = new System.Drawing.Size(6, 25);
			// 
			// CommandButton
			// 
			this.CommandButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.CommandButton.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_cmd;
			this.CommandButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.CommandButton.Name = "CommandButton";
			this.CommandButton.Size = new System.Drawing.Size(23, 22);
			this.CommandButton.Text = "toolStripButton17";
			this.CommandButton.ToolTipText = "C# Console";
			this.CommandButton.Click += new System.EventHandler(this.consoleClick);
			// 
			// CompilerOptionsButton
			// 
			this.CompilerOptionsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.CompilerOptionsButton.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuLayersLayerPropertiesIcon;
			this.CompilerOptionsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.CompilerOptionsButton.Name = "CompilerOptionsButton";
			this.CompilerOptionsButton.Size = new System.Drawing.Size(23, 22);
			this.CompilerOptionsButton.Text = "toolStripButton18";
			this.CompilerOptionsButton.ToolTipText = "Compiler Options...";
			this.CompilerOptionsButton.Click += new System.EventHandler(this.compilerClick);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			// 
			// dataGridViewTextBoxColumn10
			// 
			this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn10.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn10.HeaderText = "Name";
			this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
			this.dataGridViewTextBoxColumn10.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn11
			// 
			this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn11.DataPropertyName = "Style";
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn11.DefaultCellStyle = dataGridViewCellStyle1;
			this.dataGridViewTextBoxColumn11.HeaderText = "Style";
			this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
			this.dataGridViewTextBoxColumn11.ReadOnly = true;
			this.dataGridViewTextBoxColumn11.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn11.Width = 40;
			// 
			// dataGridViewTextBoxColumn12
			// 
			this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn12.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn12.HeaderText = "Type";
			this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
			this.dataGridViewTextBoxColumn12.ReadOnly = true;
			this.dataGridViewTextBoxColumn12.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn12.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn12.Width = 80;
			// 
			// Column1
			// 
			this.Column1.DataPropertyName = "Name";
			this.Column1.HeaderText = "Column1";
			this.Column1.Name = "Column1";
			// 
			// dataGridViewTextBoxColumn7
			// 
			this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn7.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn7.HeaderText = "Name";
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn8
			// 
			this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn8.DataPropertyName = "Style";
			dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn8.DefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridViewTextBoxColumn8.HeaderText = "Style";
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			this.dataGridViewTextBoxColumn8.ReadOnly = true;
			this.dataGridViewTextBoxColumn8.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn8.Width = 40;
			// 
			// dataGridViewTextBoxColumn9
			// 
			this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn9.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn9.HeaderText = "Type";
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.ReadOnly = true;
			this.dataGridViewTextBoxColumn9.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn9.Width = 80;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn4.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn4.HeaderText = "Name";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn5
			// 
			this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn5.DataPropertyName = "Style";
			dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle3;
			this.dataGridViewTextBoxColumn5.HeaderText = "Style";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn5.Width = 40;
			// 
			// dataGridViewTextBoxColumn6
			// 
			this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn6.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn6.HeaderText = "Type";
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn6.Width = 80;
			// 
			// dataGridViewTextBoxColumn13
			// 
			this.dataGridViewTextBoxColumn13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn13.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn13.HeaderText = "Name";
			this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
			this.dataGridViewTextBoxColumn13.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn13.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn14
			// 
			this.dataGridViewTextBoxColumn14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn14.DataPropertyName = "Style";
			dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn14.DefaultCellStyle = dataGridViewCellStyle4;
			this.dataGridViewTextBoxColumn14.HeaderText = "Style";
			this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
			this.dataGridViewTextBoxColumn14.ReadOnly = true;
			this.dataGridViewTextBoxColumn14.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn14.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn14.Width = 40;
			// 
			// dataGridViewTextBoxColumn15
			// 
			this.dataGridViewTextBoxColumn15.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn15.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn15.HeaderText = "Type";
			this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
			this.dataGridViewTextBoxColumn15.ReadOnly = true;
			this.dataGridViewTextBoxColumn15.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn15.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn15.Width = 80;
			// 
			// dataGridViewTextBoxColumn16
			// 
			this.dataGridViewTextBoxColumn16.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn16.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn16.HeaderText = "Name";
			this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
			this.dataGridViewTextBoxColumn16.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn16.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn17
			// 
			this.dataGridViewTextBoxColumn17.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn17.DataPropertyName = "Style";
			dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn17.DefaultCellStyle = dataGridViewCellStyle5;
			this.dataGridViewTextBoxColumn17.HeaderText = "Style";
			this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
			this.dataGridViewTextBoxColumn17.ReadOnly = true;
			this.dataGridViewTextBoxColumn17.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn17.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn17.Width = 40;
			// 
			// dataGridViewTextBoxColumn18
			// 
			this.dataGridViewTextBoxColumn18.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn18.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn18.HeaderText = "Type";
			this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
			this.dataGridViewTextBoxColumn18.ReadOnly = true;
			this.dataGridViewTextBoxColumn18.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn18.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn18.Width = 80;
			// 
			// dataGridViewTextBoxColumn19
			// 
			this.dataGridViewTextBoxColumn19.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn19.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn19.HeaderText = "Name";
			this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
			this.dataGridViewTextBoxColumn19.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn19.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn20
			// 
			this.dataGridViewTextBoxColumn20.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn20.DataPropertyName = "Style";
			dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn20.DefaultCellStyle = dataGridViewCellStyle6;
			this.dataGridViewTextBoxColumn20.HeaderText = "Style";
			this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
			this.dataGridViewTextBoxColumn20.ReadOnly = true;
			this.dataGridViewTextBoxColumn20.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn20.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn20.Width = 40;
			// 
			// dataGridViewTextBoxColumn21
			// 
			this.dataGridViewTextBoxColumn21.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn21.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn21.HeaderText = "Type";
			this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
			this.dataGridViewTextBoxColumn21.ReadOnly = true;
			this.dataGridViewTextBoxColumn21.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn21.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn21.Width = 80;
			// 
			// dataGridViewTextBoxColumn22
			// 
			this.dataGridViewTextBoxColumn22.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn22.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn22.HeaderText = "Name";
			this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
			this.dataGridViewTextBoxColumn22.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn22.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn23
			// 
			this.dataGridViewTextBoxColumn23.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn23.DataPropertyName = "Style";
			dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn23.DefaultCellStyle = dataGridViewCellStyle7;
			this.dataGridViewTextBoxColumn23.HeaderText = "Style";
			this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
			this.dataGridViewTextBoxColumn23.ReadOnly = true;
			this.dataGridViewTextBoxColumn23.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn23.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn23.Width = 40;
			// 
			// dataGridViewTextBoxColumn24
			// 
			this.dataGridViewTextBoxColumn24.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn24.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn24.HeaderText = "Type";
			this.dataGridViewTextBoxColumn24.Name = "dataGridViewTextBoxColumn24";
			this.dataGridViewTextBoxColumn24.ReadOnly = true;
			this.dataGridViewTextBoxColumn24.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn24.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn24.Width = 80;
			// 
			// dataGridViewTextBoxColumn25
			// 
			this.dataGridViewTextBoxColumn25.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn25.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn25.HeaderText = "Name";
			this.dataGridViewTextBoxColumn25.Name = "dataGridViewTextBoxColumn25";
			this.dataGridViewTextBoxColumn25.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn25.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn26
			// 
			this.dataGridViewTextBoxColumn26.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn26.DataPropertyName = "Style";
			dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn26.DefaultCellStyle = dataGridViewCellStyle8;
			this.dataGridViewTextBoxColumn26.HeaderText = "Style";
			this.dataGridViewTextBoxColumn26.Name = "dataGridViewTextBoxColumn26";
			this.dataGridViewTextBoxColumn26.ReadOnly = true;
			this.dataGridViewTextBoxColumn26.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn26.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn26.Width = 40;
			// 
			// dataGridViewTextBoxColumn27
			// 
			this.dataGridViewTextBoxColumn27.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn27.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn27.HeaderText = "Type";
			this.dataGridViewTextBoxColumn27.Name = "dataGridViewTextBoxColumn27";
			this.dataGridViewTextBoxColumn27.ReadOnly = true;
			this.dataGridViewTextBoxColumn27.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn27.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn27.Width = 80;
			// 
			// dataGridViewTextBoxColumn28
			// 
			this.dataGridViewTextBoxColumn28.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn28.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn28.HeaderText = "Name";
			this.dataGridViewTextBoxColumn28.Name = "dataGridViewTextBoxColumn28";
			this.dataGridViewTextBoxColumn28.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn28.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn29
			// 
			this.dataGridViewTextBoxColumn29.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn29.DataPropertyName = "Style";
			dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn29.DefaultCellStyle = dataGridViewCellStyle9;
			this.dataGridViewTextBoxColumn29.HeaderText = "Style";
			this.dataGridViewTextBoxColumn29.Name = "dataGridViewTextBoxColumn29";
			this.dataGridViewTextBoxColumn29.ReadOnly = true;
			this.dataGridViewTextBoxColumn29.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn29.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn29.Width = 40;
			// 
			// dataGridViewTextBoxColumn30
			// 
			this.dataGridViewTextBoxColumn30.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn30.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn30.HeaderText = "Type";
			this.dataGridViewTextBoxColumn30.Name = "dataGridViewTextBoxColumn30";
			this.dataGridViewTextBoxColumn30.ReadOnly = true;
			this.dataGridViewTextBoxColumn30.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn30.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn30.Width = 80;
			// 
			// dataGridViewTextBoxColumn31
			// 
			this.dataGridViewTextBoxColumn31.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn31.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn31.HeaderText = "Name";
			this.dataGridViewTextBoxColumn31.Name = "dataGridViewTextBoxColumn31";
			this.dataGridViewTextBoxColumn31.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn31.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn32
			// 
			this.dataGridViewTextBoxColumn32.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn32.DataPropertyName = "Style";
			dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn32.DefaultCellStyle = dataGridViewCellStyle10;
			this.dataGridViewTextBoxColumn32.HeaderText = "Style";
			this.dataGridViewTextBoxColumn32.Name = "dataGridViewTextBoxColumn32";
			this.dataGridViewTextBoxColumn32.ReadOnly = true;
			this.dataGridViewTextBoxColumn32.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn32.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn32.Width = 40;
			// 
			// dataGridViewTextBoxColumn33
			// 
			this.dataGridViewTextBoxColumn33.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn33.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn33.HeaderText = "Type";
			this.dataGridViewTextBoxColumn33.Name = "dataGridViewTextBoxColumn33";
			this.dataGridViewTextBoxColumn33.ReadOnly = true;
			this.dataGridViewTextBoxColumn33.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn33.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn33.Width = 80;
			// 
			// dataGridViewTextBoxColumn34
			// 
			this.dataGridViewTextBoxColumn34.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn34.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn34.HeaderText = "Name";
			this.dataGridViewTextBoxColumn34.Name = "dataGridViewTextBoxColumn34";
			this.dataGridViewTextBoxColumn34.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn34.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn35
			// 
			this.dataGridViewTextBoxColumn35.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn35.DataPropertyName = "Style";
			dataGridViewCellStyle11.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn35.DefaultCellStyle = dataGridViewCellStyle11;
			this.dataGridViewTextBoxColumn35.HeaderText = "Style";
			this.dataGridViewTextBoxColumn35.Name = "dataGridViewTextBoxColumn35";
			this.dataGridViewTextBoxColumn35.ReadOnly = true;
			this.dataGridViewTextBoxColumn35.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn35.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn35.Width = 40;
			// 
			// dataGridViewTextBoxColumn36
			// 
			this.dataGridViewTextBoxColumn36.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn36.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn36.HeaderText = "Type";
			this.dataGridViewTextBoxColumn36.Name = "dataGridViewTextBoxColumn36";
			this.dataGridViewTextBoxColumn36.ReadOnly = true;
			this.dataGridViewTextBoxColumn36.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn36.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn36.Width = 80;
			// 
			// dataGridViewTextBoxColumn37
			// 
			this.dataGridViewTextBoxColumn37.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn37.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn37.HeaderText = "Name";
			this.dataGridViewTextBoxColumn37.Name = "dataGridViewTextBoxColumn37";
			this.dataGridViewTextBoxColumn37.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn37.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn38
			// 
			this.dataGridViewTextBoxColumn38.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn38.DataPropertyName = "Style";
			dataGridViewCellStyle12.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn38.DefaultCellStyle = dataGridViewCellStyle12;
			this.dataGridViewTextBoxColumn38.HeaderText = "Style";
			this.dataGridViewTextBoxColumn38.Name = "dataGridViewTextBoxColumn38";
			this.dataGridViewTextBoxColumn38.ReadOnly = true;
			this.dataGridViewTextBoxColumn38.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn38.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn38.Width = 40;
			// 
			// dataGridViewTextBoxColumn39
			// 
			this.dataGridViewTextBoxColumn39.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn39.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn39.HeaderText = "Type";
			this.dataGridViewTextBoxColumn39.Name = "dataGridViewTextBoxColumn39";
			this.dataGridViewTextBoxColumn39.ReadOnly = true;
			this.dataGridViewTextBoxColumn39.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn39.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn39.Width = 80;
			// 
			// dataGridViewTextBoxColumn40
			// 
			this.dataGridViewTextBoxColumn40.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn40.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn40.HeaderText = "Name";
			this.dataGridViewTextBoxColumn40.Name = "dataGridViewTextBoxColumn40";
			this.dataGridViewTextBoxColumn40.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn40.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn41
			// 
			this.dataGridViewTextBoxColumn41.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn41.DataPropertyName = "Style";
			dataGridViewCellStyle13.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn41.DefaultCellStyle = dataGridViewCellStyle13;
			this.dataGridViewTextBoxColumn41.HeaderText = "Style";
			this.dataGridViewTextBoxColumn41.Name = "dataGridViewTextBoxColumn41";
			this.dataGridViewTextBoxColumn41.ReadOnly = true;
			this.dataGridViewTextBoxColumn41.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn41.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn41.Width = 40;
			// 
			// dataGridViewTextBoxColumn42
			// 
			this.dataGridViewTextBoxColumn42.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn42.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn42.HeaderText = "Type";
			this.dataGridViewTextBoxColumn42.Name = "dataGridViewTextBoxColumn42";
			this.dataGridViewTextBoxColumn42.ReadOnly = true;
			this.dataGridViewTextBoxColumn42.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn42.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn42.Width = 80;
			// 
			// dataGridViewTextBoxColumn43
			// 
			this.dataGridViewTextBoxColumn43.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn43.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn43.HeaderText = "Name";
			this.dataGridViewTextBoxColumn43.Name = "dataGridViewTextBoxColumn43";
			this.dataGridViewTextBoxColumn43.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn43.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn44
			// 
			this.dataGridViewTextBoxColumn44.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn44.DataPropertyName = "Style";
			dataGridViewCellStyle14.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn44.DefaultCellStyle = dataGridViewCellStyle14;
			this.dataGridViewTextBoxColumn44.HeaderText = "Style";
			this.dataGridViewTextBoxColumn44.Name = "dataGridViewTextBoxColumn44";
			this.dataGridViewTextBoxColumn44.ReadOnly = true;
			this.dataGridViewTextBoxColumn44.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn44.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn44.Width = 40;
			// 
			// dataGridViewTextBoxColumn45
			// 
			this.dataGridViewTextBoxColumn45.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn45.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn45.HeaderText = "Type";
			this.dataGridViewTextBoxColumn45.Name = "dataGridViewTextBoxColumn45";
			this.dataGridViewTextBoxColumn45.ReadOnly = true;
			this.dataGridViewTextBoxColumn45.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn45.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn45.Width = 80;
			// 
			// dataGridViewTextBoxColumn46
			// 
			this.dataGridViewTextBoxColumn46.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn46.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn46.HeaderText = "Name";
			this.dataGridViewTextBoxColumn46.Name = "dataGridViewTextBoxColumn46";
			this.dataGridViewTextBoxColumn46.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn46.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn47
			// 
			this.dataGridViewTextBoxColumn47.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn47.DataPropertyName = "Style";
			dataGridViewCellStyle15.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn47.DefaultCellStyle = dataGridViewCellStyle15;
			this.dataGridViewTextBoxColumn47.HeaderText = "Style";
			this.dataGridViewTextBoxColumn47.Name = "dataGridViewTextBoxColumn47";
			this.dataGridViewTextBoxColumn47.ReadOnly = true;
			this.dataGridViewTextBoxColumn47.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn47.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn47.Width = 40;
			// 
			// dataGridViewTextBoxColumn48
			// 
			this.dataGridViewTextBoxColumn48.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn48.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn48.HeaderText = "Type";
			this.dataGridViewTextBoxColumn48.Name = "dataGridViewTextBoxColumn48";
			this.dataGridViewTextBoxColumn48.ReadOnly = true;
			this.dataGridViewTextBoxColumn48.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn48.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn48.Width = 80;
			// 
			// dataGridViewTextBoxColumn49
			// 
			this.dataGridViewTextBoxColumn49.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn49.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn49.HeaderText = "Name";
			this.dataGridViewTextBoxColumn49.Name = "dataGridViewTextBoxColumn49";
			this.dataGridViewTextBoxColumn49.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn49.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn50
			// 
			this.dataGridViewTextBoxColumn50.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn50.DataPropertyName = "Style";
			dataGridViewCellStyle16.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn50.DefaultCellStyle = dataGridViewCellStyle16;
			this.dataGridViewTextBoxColumn50.HeaderText = "Style";
			this.dataGridViewTextBoxColumn50.Name = "dataGridViewTextBoxColumn50";
			this.dataGridViewTextBoxColumn50.ReadOnly = true;
			this.dataGridViewTextBoxColumn50.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn50.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn50.Width = 40;
			// 
			// dataGridViewTextBoxColumn51
			// 
			this.dataGridViewTextBoxColumn51.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn51.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn51.HeaderText = "Type";
			this.dataGridViewTextBoxColumn51.Name = "dataGridViewTextBoxColumn51";
			this.dataGridViewTextBoxColumn51.ReadOnly = true;
			this.dataGridViewTextBoxColumn51.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn51.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn51.Width = 80;
			// 
			// dataGridViewTextBoxColumn55
			// 
			this.dataGridViewTextBoxColumn55.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn55.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn55.HeaderText = "Name";
			this.dataGridViewTextBoxColumn55.Name = "dataGridViewTextBoxColumn55";
			this.dataGridViewTextBoxColumn55.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn55.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn56
			// 
			this.dataGridViewTextBoxColumn56.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn56.DataPropertyName = "Style";
			dataGridViewCellStyle17.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn56.DefaultCellStyle = dataGridViewCellStyle17;
			this.dataGridViewTextBoxColumn56.HeaderText = "Style";
			this.dataGridViewTextBoxColumn56.Name = "dataGridViewTextBoxColumn56";
			this.dataGridViewTextBoxColumn56.ReadOnly = true;
			this.dataGridViewTextBoxColumn56.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn56.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn56.Width = 40;
			// 
			// dataGridViewTextBoxColumn57
			// 
			this.dataGridViewTextBoxColumn57.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn57.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn57.HeaderText = "Type";
			this.dataGridViewTextBoxColumn57.Name = "dataGridViewTextBoxColumn57";
			this.dataGridViewTextBoxColumn57.ReadOnly = true;
			this.dataGridViewTextBoxColumn57.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn57.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn57.Width = 80;
			// 
			// dataGridViewTextBoxColumn52
			// 
			this.dataGridViewTextBoxColumn52.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn52.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn52.HeaderText = "Name";
			this.dataGridViewTextBoxColumn52.Name = "dataGridViewTextBoxColumn52";
			this.dataGridViewTextBoxColumn52.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn52.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn53
			// 
			this.dataGridViewTextBoxColumn53.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn53.DataPropertyName = "Style";
			dataGridViewCellStyle18.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn53.DefaultCellStyle = dataGridViewCellStyle18;
			this.dataGridViewTextBoxColumn53.HeaderText = "Style";
			this.dataGridViewTextBoxColumn53.Name = "dataGridViewTextBoxColumn53";
			this.dataGridViewTextBoxColumn53.ReadOnly = true;
			this.dataGridViewTextBoxColumn53.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn53.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn53.Width = 40;
			// 
			// dataGridViewTextBoxColumn54
			// 
			this.dataGridViewTextBoxColumn54.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn54.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn54.HeaderText = "Type";
			this.dataGridViewTextBoxColumn54.Name = "dataGridViewTextBoxColumn54";
			this.dataGridViewTextBoxColumn54.ReadOnly = true;
			this.dataGridViewTextBoxColumn54.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn54.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn54.Width = 80;
			// 
			// dataGridViewTextBoxColumn58
			// 
			this.dataGridViewTextBoxColumn58.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn58.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn58.HeaderText = "Name";
			this.dataGridViewTextBoxColumn58.Name = "dataGridViewTextBoxColumn58";
			this.dataGridViewTextBoxColumn58.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn58.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn59
			// 
			this.dataGridViewTextBoxColumn59.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn59.DataPropertyName = "Style";
			dataGridViewCellStyle19.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn59.DefaultCellStyle = dataGridViewCellStyle19;
			this.dataGridViewTextBoxColumn59.HeaderText = "Style";
			this.dataGridViewTextBoxColumn59.Name = "dataGridViewTextBoxColumn59";
			this.dataGridViewTextBoxColumn59.ReadOnly = true;
			this.dataGridViewTextBoxColumn59.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn59.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn59.Width = 40;
			// 
			// dataGridViewTextBoxColumn60
			// 
			this.dataGridViewTextBoxColumn60.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn60.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn60.HeaderText = "Type";
			this.dataGridViewTextBoxColumn60.Name = "dataGridViewTextBoxColumn60";
			this.dataGridViewTextBoxColumn60.ReadOnly = true;
			this.dataGridViewTextBoxColumn60.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn60.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn60.Width = 80;
			// 
			// grid
			// 
			this.grid.AllowDrop = true;
			this.grid.AllowUserToAddRows = false;
			this.grid.AllowUserToResizeColumns = false;
			this.grid.AllowUserToResizeRows = false;
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.AutoGenerateColumns = false;
			this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
			this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle20.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(202)))), ((int)(((byte)(144)))));
			dataGridViewCellStyle20.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.grid.DefaultCellStyle = dataGridViewCellStyle20;
			this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.grid.Items = null;
			this.grid.Location = new System.Drawing.Point(0, 76);
			this.grid.Margin = new System.Windows.Forms.Padding(2);
			this.grid.Name = "grid";
			this.grid.RowHeadersVisible = false;
			this.grid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			this.grid.RowTemplate.Height = 18;
			this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grid.ShowCellToolTips = false;
			this.grid.Size = new System.Drawing.Size(380, 253);
			this.grid.TabIndex = 12;
			this.grid.VirtualMode = true;
			// 
			// dataGridViewTextBoxColumn61
			// 
			this.dataGridViewTextBoxColumn61.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn61.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn61.HeaderText = "Name";
			this.dataGridViewTextBoxColumn61.Name = "dataGridViewTextBoxColumn61";
			this.dataGridViewTextBoxColumn61.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn61.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn62
			// 
			this.dataGridViewTextBoxColumn62.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn62.DataPropertyName = "Style";
			dataGridViewCellStyle21.ForeColor = System.Drawing.Color.Blue;
			this.dataGridViewTextBoxColumn62.DefaultCellStyle = dataGridViewCellStyle21;
			this.dataGridViewTextBoxColumn62.HeaderText = "Style";
			this.dataGridViewTextBoxColumn62.Name = "dataGridViewTextBoxColumn62";
			this.dataGridViewTextBoxColumn62.ReadOnly = true;
			this.dataGridViewTextBoxColumn62.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn62.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn62.Width = 40;
			// 
			// dataGridViewTextBoxColumn63
			// 
			this.dataGridViewTextBoxColumn63.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn63.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn63.HeaderText = "Type";
			this.dataGridViewTextBoxColumn63.Name = "dataGridViewTextBoxColumn63";
			this.dataGridViewTextBoxColumn63.ReadOnly = true;
			this.dataGridViewTextBoxColumn63.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn63.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn63.Width = 80;
			// 
			// MainForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(380, 329);
			this.Controls.Add(this.toolStrip2);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.grid);
			this.Controls.Add(this.mainMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.mainMenu;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MinimumSize = new System.Drawing.Size(319, 149);
			this.Name = "MainForm";
			this.Text = "JohnsHope\'s FPlot";
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion



		public class CheckItems: IItemEventHandler {
			MainForm f;
			
			void ResetMenu() {
				f.fitItem.Enabled = f.Model.Items.IsFitable();
				f.evalItem.Enabled = f.Model.Items.IsEvalable();
			}
			
			public CheckItems(MainForm form) {
				f = form;
				form.Model.Items.Handlers += this;
				ResetMenu();
			}

			public void HandleUpdate(Item x) { ResetMenu(); }
			public void HandleRemove(Item x) {	ResetMenu(); }
			public void HandleAdd(Item x) { ResetMenu(); }
			public void HandleReplace(Item oldItem, Item newItem) { ResetMenu(); }
			public void HandleReorder(ItemList order) { }
			public void HandleInvalidate() { ResetMenu(); }
		}

		private void exitClick(object sender, System.EventArgs e) {
			Application.Exit();
		}

		private void saveAsItem_Click(object sender, System.EventArgs e) {
			saveFileDialog.FileName = Model.Items.Filename;
			DialogResult res = saveFileDialog.ShowDialog();
			if (res == DialogResult.OK)	Model.Save(saveFileDialog.FileName);
		}

		private void openItem_Click(object sender, System.EventArgs e) {
      DialogResult res = openFileDialog.ShowDialog();
			if (res == DialogResult.OK) {
				try {
					Model.Load(openFileDialog.FileName);
				} catch { MessageBox.Show(Resources.FileOpenFail.Replace("#file", openFileDialog.FileName));
				}
			}
		}

		private void newItem_Click(object sender, System.EventArgs e) {
			DialogResult res;
			if (Model.Items.Count > 0) {
				res = MessageBox.Show(Resources.NewWarning, Resources.WarningCaption, MessageBoxButtons.OKCancel);
			} else res = DialogResult.OK;
			if (res == DialogResult.OK) {
				res = newFileDialog.ShowDialog();
				if (res == DialogResult.OK) {
					Model.Reset(newFileDialog.FileName);
				}
			}
		}

		private void closeItem_Click(object sender, System.EventArgs e) {
			DialogResult res;
			if (Model.Items.Count > 0) {
				res = MessageBox.Show(Resources.CloseWarning, Resources.WarningCaption, MessageBoxButtons.OKCancel);
			} else res = DialogResult.OK;
			if (res == DialogResult.OK) {
				Model.Reset();
			}
		}

		private void saveItem_Click(object sender, System.EventArgs e) {
			Model.Save();
		}

		static int findex = 1;
		private void addFunctionClick(object sender, System.EventArgs e) {
			Function1DItem f = new Function1DItem();
			f.Name = Resources.Function + " " + findex++;
			Model.Items.Add(f);
			Model.NewEditForm(f).Show();
		}

		private void libraryClick(object sender, System.EventArgs e) {
			DialogResult res;
			res = newLibraryDialog.ShowDialog();
			if (res == DialogResult.OK) {
				Library.Library l = new Library.Library();
				l.Filename = newLibraryDialog.FileName;
				l.Name = Path.GetFileName(newLibraryDialog.FileName);
				try {
					l.Load(newLibraryDialog.FileName);
				} catch { }
				Model.Items.Add(l);
				Model.NewEditForm(l).Show();
			}
		}

		static int dindex = 1;
		private void dataClick(object sender, System.EventArgs e) {
			DataItem d = new DataItem();
			d.Name = Resources.Data + " " + dindex++;
			d.Dimensions = 2;
			Model.Items.Add(d);
			Model.NewEditForm(d).Show();
		}

		private void compilerClick(object sender, System.EventArgs e) {
			CompilerOptionsForm.ShowForm();
		}

		private void consoleClick(object sender, System.EventArgs e) {
			if (Model.ConsoleForm.IsDisposed) Model.ConsoleForm = new ConsoleForm(Model);
			if (Model.ConsoleForm.WindowState == FormWindowState.Minimized) Model.ConsoleForm.WindowState = FormWindowState.Normal;
			Model.ConsoleForm.Show();
			Model.ConsoleForm.BringToFront();
		}

		private void fitClick(object sender, System.EventArgs e) {
			if (fitForm.IsDisposed) fitForm = new FitForm(Model);

			// apply selection to fitForm
			ItemList selection = new ItemList();
			grid.GetSelection(selection);
			bool fsel = false, dsel = false;
			foreach (Item x in selection) {
				if (x is Function1DItem) {
					if (((Function1DItem)x).Fitable && !fsel) {
						fitForm.Function = (Function1DItem)x;
						fsel = true;
					}
				} else if (x is DataItem && !dsel) {
					fitForm.Data = (DataItem)x;
					dsel = true;
				}
			}

			fitForm.Reset();
			if (fitForm.WindowState == FormWindowState.Minimized) fitForm.WindowState = FormWindowState.Normal;
			fitForm.Show();
			fitForm.BringToFront();
		}

		private void evalClick(object sender, System.EventArgs e) {
			if (evalForm == null || evalForm.IsDisposed) evalForm = new EvalForm(Model);
			evalForm.Reset();
			if (evalForm.WindowState == FormWindowState.Minimized) evalForm.WindowState = FormWindowState.Normal;
			evalForm.Show();
			evalForm.BringToFront();
		}

		private void helpContentsClick(object sender, System.EventArgs e) {
			Help.ShowHelp(this, Resources.HelpFile);
		}

		private void helpAbout_Click(object sender, System.EventArgs e) {
			if (aboutForm.IsDisposed) aboutForm = new AboutForm();
			if (aboutForm.WindowState == FormWindowState.Minimized) aboutForm.WindowState = FormWindowState.Normal;
			aboutForm.Show();
			aboutForm.BringToFront();
		}

		private void new2DPlotToolStripMenuItem_Click(object sender, EventArgs e) {
			PlotForm plot = new PlotForm(Model.Items, Plot.Type.Plot2D);
			grid.GetSelection(plot.PlotModel);
			plot.ResetName();
			plot.Show();
			plot.BringToFront();
		}

		private void NotifyTopForm(object sender, EventArgs e) {
			PlotForm form = sender as PlotForm;
			if (sender != null) {
				grid.SetSelection(PlotForm.TopForm.PlotModel);
				applySelectionItem.Enabled = true;
				applySelectionItem.Text = Resources.ApplySelection + PlotForm.TopForm.Text;
			}
		}

		private void apply_Click(object sender, EventArgs e) {
			if (PlotForm.TopForm != null) grid.GetSelection(PlotForm.TopForm.PlotModel);
		}

		private void Plot3DClick(object sender, EventArgs e) {
			PlotForm plot = new PlotForm(Model.Items, Plot.Type.Plot3DSurface);
			grid.GetSelection(plot.PlotModel);
			plot.Show();
			plot.BringToFront();
		}

		private void cutClick(object sender, EventArgs e) {
			grid.Cut();
		}

		private void copyClick(object sender, EventArgs e) {
			grid.Copy();
		}

		private void pasteClick(object sender, EventArgs e) {
			grid.Paste();
		}

		private Item SelectedItem() {
			if (grid.SelectedRows.Count == 1) return Model.Items[grid.SelectedRows[0].Index];
			return null;
		}

		private void editClick(object sender, EventArgs e) {
			Model.NewEditForm(SelectedItem());
		}

		private void parClick(object sender, EventArgs e) {
			FunctionItem f = SelectedItem() as FunctionItem;
			if (f != null) ParForm.New(Model, f);
		}

		private void lineStyleClick(object sender, EventArgs e) {
			Item x = SelectedItem();
			if (x != null) {
				if (x is DataItem) DataLineStyleForm.New(Model, (DataItem)x);
				else if (x is Function1DItem) FunctionLineStyleForm.New(Model, (Function1DItem)x);
				else if (x is Function2DItem) {
					((Function2DItem)x).Gradient = GradientForm.ShowDialog();
					Model.Items.Update(x);
				}
			}
		}

		private void gradientClick(object sender, EventArgs e) {
			Function2DItem x = SelectedItem() as Function2DItem;
			if (x != null) {
				x.Gradient = GradientForm.ShowDialog();
				Model.Items.Update(x);
			}
		}

		private void SelectionChanged(object sender, EventArgs e) {
			Item item = SelectedItem();
			if (item != null) {
				EditButton.Enabled = true;
				ParButton.Enabled = item is FunctionItem && ((FunctionItem)item).Fitable;
				bool gradient = item is Function2DItem;
				StyleButton.Enabled = item is Function1DItem || item is DataItem || gradient;
				if (gradient) StyleButton.Image = Properties.Resources.Icons_GradientToolIcon;
				else StyleButton.Image = Properties.Resources.Icons_LineStyle;
			} else {
				EditButton.Enabled = ParButton.Enabled = StyleButton.Enabled = false;
			}
		}

		private void ResetCalcButtons() {
			bool hasFunction = false, hasFitable1DFunction = false, hasData = false;
			foreach (Item x in Model.Items) {
				if (x is FunctionItem) {
					hasFunction = true;
					hasFitable1DFunction = x is Function1DItem && ((FunctionItem)x).Fitable;
				} else if (x is DataItem) hasData = true;
			}
			FitButton.Enabled = hasFitable1DFunction && hasData;
			EvalButton.Enabled = hasFunction;
		}

		#region IItemEventHandler Members

		public void HandleUpdate(Item x) { ResetCalcButtons(); }
		public void HandleRemove(Item x) { ResetCalcButtons(); }
		public void HandleAdd(Item x) { ResetCalcButtons(); }
		public void HandleReplace(Item oldItem, Item newItem) { ResetCalcButtons(); }
		public void HandleReorder(ItemList order) { }
		public void HandleInvalidate() { ResetCalcButtons(); }

		#endregion

	}
}
