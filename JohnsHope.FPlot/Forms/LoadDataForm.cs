using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using JohnsHope.FPlot.Library;
using ICSharpCode.TextEditor;
using System.Text;
using System.IO;

namespace JohnsHope.FPlot
{
	/// <summary>
	/// Summary description for LoadDataForm.
	/// </summary>
	public class LoadDataForm : System.Windows.Forms.Form, IEditForm {
		private System.Windows.Forms.Label intro;
		private System.Windows.Forms.Label outro;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private DataItem item;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox separators;
		private System.Windows.Forms.RadioButton floatbutton;
		private System.Windows.Forms.RadioButton doublebutton;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage asciiPage;
		private System.Windows.Forms.TabPage binaryPage;
		private System.Windows.Forms.RadioButton sbytebutton;
		private System.Windows.Forms.RadioButton int16button;
		private System.Windows.Forms.RadioButton int32button;
		private System.Windows.Forms.RadioButton int64button;
		private System.Windows.Forms.RadioButton uint64button;
		private System.Windows.Forms.RadioButton uint32button;
		private System.Windows.Forms.RadioButton uint16button;
		private System.Windows.Forms.RadioButton bytebutton;
		private System.Windows.Forms.RadioButton bigendian;
		private System.Windows.Forms.RadioButton littleendian;
		private System.Windows.Forms.GroupBox byteordering;
		private CheckBox localFormat;
		DataForm data;
		private TabPage wavPage;
		private Label label2;
		private CheckBox allchannels;
		private NumericUpDown channel;
		private TabPage excelPage;
		private NumericUpDown excelEndRow;
		private Label label4;
		private Label label3;
		private NumericUpDown excelStartRow;
		private TextBox excelColumns;
		private Label label5;
		private Label label6;
		private NumericUpDown excelSheet;
		private CheckBox memoryText;
		private CheckBox memoryBinary;
		private CheckBox memoryWAV;
		private CheckBox memoryExcel;
		private Label label7;
		private ComboBox encoding;
		MainModel Model;
		EncodingInfo[] encodings = Encoding.GetEncodings();
		private Label label8;
		private ComboBox samples;
		private TabPage tabPage1;
		private Label label9;
		private ToolStrip toolStrip1;
		private ToolStripButton toolStripButton1;
		private ToolStripButton toolStripButton2;
		private ToolStripButton toolStripButton3;
		private ToolStripButton toolStripButton4;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripButton toolStripButton5;
		private ToolStripButton toolStripButton6;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripButton toolStripButton7;
		private ToolStripLabel length;
		private ToolStripSeparator toolStripSeparator3;
		private CodeControl source;
		private ToolStripButton toolStripButton8;
		string[] encodingnames;

		public LoadDataForm(MainModel Model, DataForm data, DataItem item)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.Model = Model;
			this.Text = Properties.Resources.LoadData + " " + data.Text;

			this.item = item;
			this.data = data;

			ResetExcelColumns();
			ResetEncodings();
			samples.SelectedIndex = 1;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadDataForm));
			this.intro = new System.Windows.Forms.Label();
			this.outro = new System.Windows.Forms.Label();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.label9 = new System.Windows.Forms.Label();
			this.asciiPage = new System.Windows.Forms.TabPage();
			this.label7 = new System.Windows.Forms.Label();
			this.encoding = new System.Windows.Forms.ComboBox();
			this.memoryText = new System.Windows.Forms.CheckBox();
			this.localFormat = new System.Windows.Forms.CheckBox();
			this.separators = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.binaryPage = new System.Windows.Forms.TabPage();
			this.memoryBinary = new System.Windows.Forms.CheckBox();
			this.byteordering = new System.Windows.Forms.GroupBox();
			this.littleendian = new System.Windows.Forms.RadioButton();
			this.bigendian = new System.Windows.Forms.RadioButton();
			this.uint64button = new System.Windows.Forms.RadioButton();
			this.uint32button = new System.Windows.Forms.RadioButton();
			this.uint16button = new System.Windows.Forms.RadioButton();
			this.bytebutton = new System.Windows.Forms.RadioButton();
			this.int64button = new System.Windows.Forms.RadioButton();
			this.int32button = new System.Windows.Forms.RadioButton();
			this.int16button = new System.Windows.Forms.RadioButton();
			this.doublebutton = new System.Windows.Forms.RadioButton();
			this.floatbutton = new System.Windows.Forms.RadioButton();
			this.sbytebutton = new System.Windows.Forms.RadioButton();
			this.wavPage = new System.Windows.Forms.TabPage();
			this.label8 = new System.Windows.Forms.Label();
			this.samples = new System.Windows.Forms.ComboBox();
			this.memoryWAV = new System.Windows.Forms.CheckBox();
			this.channel = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.allchannels = new System.Windows.Forms.CheckBox();
			this.excelPage = new System.Windows.Forms.TabPage();
			this.memoryExcel = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.excelSheet = new System.Windows.Forms.NumericUpDown();
			this.excelColumns = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.excelEndRow = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.excelStartRow = new System.Windows.Forms.NumericUpDown();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.length = new System.Windows.Forms.ToolStripLabel();
			this.source = new JohnsHope.FPlot.CodeControl();
			this.tabControl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.asciiPage.SuspendLayout();
			this.binaryPage.SuspendLayout();
			this.byteordering.SuspendLayout();
			this.wavPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.channel)).BeginInit();
			this.excelPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.excelSheet)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.excelEndRow)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.excelStartRow)).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// intro
			// 
			this.intro.BackColor = System.Drawing.Color.Transparent;
			this.intro.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.intro.ForeColor = System.Drawing.Color.Blue;
			this.intro.Location = new System.Drawing.Point(3, 157);
			this.intro.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.intro.Name = "intro";
			this.intro.Size = new System.Drawing.Size(573, 49);
			this.intro.TabIndex = 3;
			this.intro.Text = "int Length;\r\ndouble[] x, y, z, dx, dy, dz\r\npublic void OnLoad(System.IO.Stream st" +
    "ream) {";
			// 
			// outro
			// 
			this.outro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.outro.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.outro.ForeColor = System.Drawing.Color.Blue;
			this.outro.Location = new System.Drawing.Point(3, 559);
			this.outro.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.outro.Name = "outro";
			this.outro.Size = new System.Drawing.Size(94, 18);
			this.outro.TabIndex = 7;
			this.outro.Text = "}";
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "All files (*.*)|*.*";
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabPage1);
			this.tabControl.Controls.Add(this.asciiPage);
			this.tabControl.Controls.Add(this.binaryPage);
			this.tabControl.Controls.Add(this.wavPage);
			this.tabControl.Controls.Add(this.excelPage);
			this.tabControl.Location = new System.Drawing.Point(6, 27);
			this.tabControl.Margin = new System.Windows.Forms.Padding(2);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(574, 131);
			this.tabControl.TabIndex = 10;
			this.tabControl.Click += new System.EventHandler(this.SetSource);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.label9);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(566, 105);
			this.tabPage1.TabIndex = 4;
			this.tabPage1.Text = "Custom";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(14, 19);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(532, 59);
			this.label9.TabIndex = 0;
			this.label9.Text = "Please edit the code below, that will load the data from a System.IO.Stream into " +
    "the arrays x, y, z, dx, dy, dz. The size of the arrays is adjusted automatically" +
    ".";
			// 
			// asciiPage
			// 
			this.asciiPage.Controls.Add(this.label7);
			this.asciiPage.Controls.Add(this.encoding);
			this.asciiPage.Controls.Add(this.memoryText);
			this.asciiPage.Controls.Add(this.localFormat);
			this.asciiPage.Controls.Add(this.separators);
			this.asciiPage.Controls.Add(this.label1);
			this.asciiPage.Location = new System.Drawing.Point(4, 22);
			this.asciiPage.Margin = new System.Windows.Forms.Padding(2);
			this.asciiPage.Name = "asciiPage";
			this.asciiPage.Size = new System.Drawing.Size(566, 105);
			this.asciiPage.TabIndex = 0;
			this.asciiPage.Text = "Text";
			this.asciiPage.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(185, 42);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(78, 13);
			this.label7.TabIndex = 5;
			this.label7.Text = "Text encoding:";
			// 
			// encoding
			// 
			this.encoding.FormattingEnabled = true;
			this.encoding.Location = new System.Drawing.Point(188, 70);
			this.encoding.Name = "encoding";
			this.encoding.Size = new System.Drawing.Size(196, 21);
			this.encoding.TabIndex = 4;
			this.encoding.SelectedIndexChanged += new System.EventHandler(this.SetSource);
			// 
			// memoryText
			// 
			this.memoryText.AutoSize = true;
			this.memoryText.Location = new System.Drawing.Point(10, 74);
			this.memoryText.Name = "memoryText";
			this.memoryText.Size = new System.Drawing.Size(105, 17);
			this.memoryText.TabIndex = 3;
			this.memoryText.Text = "Use less memory";
			this.memoryText.UseVisualStyleBackColor = true;
			this.memoryText.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// localFormat
			// 
			this.localFormat.AutoSize = true;
			this.localFormat.Location = new System.Drawing.Point(10, 41);
			this.localFormat.Margin = new System.Windows.Forms.Padding(2);
			this.localFormat.Name = "localFormat";
			this.localFormat.Size = new System.Drawing.Size(141, 17);
			this.localFormat.TabIndex = 2;
			this.localFormat.Text = "Localized number format";
			this.localFormat.UseVisualStyleBackColor = true;
			this.localFormat.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// separators
			// 
			this.separators.Location = new System.Drawing.Point(101, 7);
			this.separators.Margin = new System.Windows.Forms.Padding(2);
			this.separators.Name = "separators";
			this.separators.Size = new System.Drawing.Size(66, 20);
			this.separators.TabIndex = 1;
			this.separators.Text = ",; \\t\\n\\r";
			this.separators.TextChanged += new System.EventHandler(this.SetSource);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(7, 10);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(90, 22);
			this.label1.TabIndex = 0;
			this.label1.Text = "Separator chars:";
			// 
			// binaryPage
			// 
			this.binaryPage.Controls.Add(this.memoryBinary);
			this.binaryPage.Controls.Add(this.byteordering);
			this.binaryPage.Controls.Add(this.uint64button);
			this.binaryPage.Controls.Add(this.uint32button);
			this.binaryPage.Controls.Add(this.uint16button);
			this.binaryPage.Controls.Add(this.bytebutton);
			this.binaryPage.Controls.Add(this.int64button);
			this.binaryPage.Controls.Add(this.int32button);
			this.binaryPage.Controls.Add(this.int16button);
			this.binaryPage.Controls.Add(this.doublebutton);
			this.binaryPage.Controls.Add(this.floatbutton);
			this.binaryPage.Controls.Add(this.sbytebutton);
			this.binaryPage.Location = new System.Drawing.Point(4, 22);
			this.binaryPage.Margin = new System.Windows.Forms.Padding(2);
			this.binaryPage.Name = "binaryPage";
			this.binaryPage.Size = new System.Drawing.Size(566, 105);
			this.binaryPage.TabIndex = 1;
			this.binaryPage.Text = "Binary";
			this.binaryPage.UseVisualStyleBackColor = true;
			// 
			// memoryBinary
			// 
			this.memoryBinary.AutoSize = true;
			this.memoryBinary.Location = new System.Drawing.Point(222, 79);
			this.memoryBinary.Name = "memoryBinary";
			this.memoryBinary.Size = new System.Drawing.Size(105, 17);
			this.memoryBinary.TabIndex = 22;
			this.memoryBinary.Text = "Use less memory";
			this.memoryBinary.UseVisualStyleBackColor = true;
			this.memoryBinary.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// byteordering
			// 
			this.byteordering.Controls.Add(this.littleendian);
			this.byteordering.Controls.Add(this.bigendian);
			this.byteordering.Location = new System.Drawing.Point(222, 7);
			this.byteordering.Margin = new System.Windows.Forms.Padding(2);
			this.byteordering.Name = "byteordering";
			this.byteordering.Padding = new System.Windows.Forms.Padding(2);
			this.byteordering.Size = new System.Drawing.Size(89, 62);
			this.byteordering.TabIndex = 21;
			this.byteordering.TabStop = false;
			this.byteordering.Text = "Byte ordering";
			// 
			// littleendian
			// 
			this.littleendian.Checked = true;
			this.littleendian.Location = new System.Drawing.Point(8, 37);
			this.littleendian.Margin = new System.Windows.Forms.Padding(2);
			this.littleendian.Name = "littleendian";
			this.littleendian.Size = new System.Drawing.Size(76, 23);
			this.littleendian.TabIndex = 1;
			this.littleendian.TabStop = true;
			this.littleendian.Text = "Little Endian";
			this.littleendian.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// bigendian
			// 
			this.bigendian.Location = new System.Drawing.Point(8, 15);
			this.bigendian.Margin = new System.Windows.Forms.Padding(2);
			this.bigendian.Name = "bigendian";
			this.bigendian.Size = new System.Drawing.Size(68, 23);
			this.bigendian.TabIndex = 0;
			this.bigendian.Text = "Big Endian";
			this.bigendian.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// uint64button
			// 
			this.uint64button.Location = new System.Drawing.Point(80, 75);
			this.uint64button.Margin = new System.Windows.Forms.Padding(2);
			this.uint64button.Name = "uint64button";
			this.uint64button.Size = new System.Drawing.Size(57, 23);
			this.uint64button.TabIndex = 20;
			this.uint64button.Text = "UInt64";
			this.uint64button.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// uint32button
			// 
			this.uint32button.Location = new System.Drawing.Point(80, 53);
			this.uint32button.Margin = new System.Windows.Forms.Padding(2);
			this.uint32button.Name = "uint32button";
			this.uint32button.Size = new System.Drawing.Size(57, 22);
			this.uint32button.TabIndex = 19;
			this.uint32button.Text = "UInt32";
			this.uint32button.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// uint16button
			// 
			this.uint16button.Checked = true;
			this.uint16button.Location = new System.Drawing.Point(80, 30);
			this.uint16button.Margin = new System.Windows.Forms.Padding(2);
			this.uint16button.Name = "uint16button";
			this.uint16button.Size = new System.Drawing.Size(57, 23);
			this.uint16button.TabIndex = 18;
			this.uint16button.TabStop = true;
			this.uint16button.Text = "UInt16";
			this.uint16button.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// bytebutton
			// 
			this.bytebutton.Location = new System.Drawing.Point(80, 7);
			this.bytebutton.Margin = new System.Windows.Forms.Padding(2);
			this.bytebutton.Name = "bytebutton";
			this.bytebutton.Size = new System.Drawing.Size(57, 23);
			this.bytebutton.TabIndex = 17;
			this.bytebutton.Text = "Byte";
			this.bytebutton.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// int64button
			// 
			this.int64button.Location = new System.Drawing.Point(8, 75);
			this.int64button.Margin = new System.Windows.Forms.Padding(2);
			this.int64button.Name = "int64button";
			this.int64button.Size = new System.Drawing.Size(57, 23);
			this.int64button.TabIndex = 16;
			this.int64button.Text = "Int64";
			this.int64button.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// int32button
			// 
			this.int32button.Location = new System.Drawing.Point(8, 53);
			this.int32button.Margin = new System.Windows.Forms.Padding(2);
			this.int32button.Name = "int32button";
			this.int32button.Size = new System.Drawing.Size(57, 22);
			this.int32button.TabIndex = 15;
			this.int32button.Text = "Int32";
			this.int32button.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// int16button
			// 
			this.int16button.Location = new System.Drawing.Point(8, 30);
			this.int16button.Margin = new System.Windows.Forms.Padding(2);
			this.int16button.Name = "int16button";
			this.int16button.Size = new System.Drawing.Size(57, 23);
			this.int16button.TabIndex = 14;
			this.int16button.Text = "Int16";
			this.int16button.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// doublebutton
			// 
			this.doublebutton.Location = new System.Drawing.Point(152, 7);
			this.doublebutton.Margin = new System.Windows.Forms.Padding(2);
			this.doublebutton.Name = "doublebutton";
			this.doublebutton.Size = new System.Drawing.Size(66, 23);
			this.doublebutton.TabIndex = 13;
			this.doublebutton.Text = "double";
			this.doublebutton.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// floatbutton
			// 
			this.floatbutton.Location = new System.Drawing.Point(152, 30);
			this.floatbutton.Margin = new System.Windows.Forms.Padding(2);
			this.floatbutton.Name = "floatbutton";
			this.floatbutton.Size = new System.Drawing.Size(50, 23);
			this.floatbutton.TabIndex = 12;
			this.floatbutton.Text = "float";
			this.floatbutton.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// sbytebutton
			// 
			this.sbytebutton.Location = new System.Drawing.Point(8, 7);
			this.sbytebutton.Margin = new System.Windows.Forms.Padding(2);
			this.sbytebutton.Name = "sbytebutton";
			this.sbytebutton.Size = new System.Drawing.Size(57, 23);
			this.sbytebutton.TabIndex = 8;
			this.sbytebutton.Text = "SByte";
			this.sbytebutton.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// wavPage
			// 
			this.wavPage.Controls.Add(this.label8);
			this.wavPage.Controls.Add(this.samples);
			this.wavPage.Controls.Add(this.memoryWAV);
			this.wavPage.Controls.Add(this.channel);
			this.wavPage.Controls.Add(this.label2);
			this.wavPage.Controls.Add(this.allchannels);
			this.wavPage.Location = new System.Drawing.Point(4, 22);
			this.wavPage.Margin = new System.Windows.Forms.Padding(2);
			this.wavPage.Name = "wavPage";
			this.wavPage.Padding = new System.Windows.Forms.Padding(2);
			this.wavPage.Size = new System.Drawing.Size(566, 105);
			this.wavPage.TabIndex = 2;
			this.wavPage.Text = "WAV (PCM)";
			this.wavPage.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(5, 79);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(87, 13);
			this.label8.TabIndex = 6;
			this.label8.Text = "Samples column:";
			// 
			// samples
			// 
			this.samples.FormattingEnabled = true;
			this.samples.Items.AddRange(new object[] {
            "None",
            "x",
            "y",
            "z",
            "dx",
            "dy",
            "dz"});
			this.samples.Location = new System.Drawing.Point(102, 76);
			this.samples.Name = "samples";
			this.samples.Size = new System.Drawing.Size(60, 21);
			this.samples.TabIndex = 5;
			this.samples.SelectedIndexChanged += new System.EventHandler(this.SetSource);
			// 
			// memoryWAV
			// 
			this.memoryWAV.AutoSize = true;
			this.memoryWAV.Location = new System.Drawing.Point(194, 16);
			this.memoryWAV.Name = "memoryWAV";
			this.memoryWAV.Size = new System.Drawing.Size(105, 17);
			this.memoryWAV.TabIndex = 4;
			this.memoryWAV.Text = "Use less memory";
			this.memoryWAV.UseVisualStyleBackColor = true;
			this.memoryWAV.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// channel
			// 
			this.channel.Location = new System.Drawing.Point(102, 43);
			this.channel.Margin = new System.Windows.Forms.Padding(2);
			this.channel.Name = "channel";
			this.channel.Size = new System.Drawing.Size(47, 20);
			this.channel.TabIndex = 3;
			this.channel.ValueChanged += new System.EventHandler(this.SetSource);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(5, 45);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(93, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "This channel only:";
			// 
			// allchannels
			// 
			this.allchannels.AutoSize = true;
			this.allchannels.Location = new System.Drawing.Point(8, 16);
			this.allchannels.Margin = new System.Windows.Forms.Padding(2);
			this.allchannels.Name = "allchannels";
			this.allchannels.Size = new System.Drawing.Size(83, 17);
			this.allchannels.TabIndex = 0;
			this.allchannels.Text = "All channels";
			this.allchannels.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// excelPage
			// 
			this.excelPage.Controls.Add(this.memoryExcel);
			this.excelPage.Controls.Add(this.label6);
			this.excelPage.Controls.Add(this.excelSheet);
			this.excelPage.Controls.Add(this.excelColumns);
			this.excelPage.Controls.Add(this.label5);
			this.excelPage.Controls.Add(this.excelEndRow);
			this.excelPage.Controls.Add(this.label4);
			this.excelPage.Controls.Add(this.label3);
			this.excelPage.Controls.Add(this.excelStartRow);
			this.excelPage.Location = new System.Drawing.Point(4, 22);
			this.excelPage.Margin = new System.Windows.Forms.Padding(2);
			this.excelPage.Name = "excelPage";
			this.excelPage.Padding = new System.Windows.Forms.Padding(2);
			this.excelPage.Size = new System.Drawing.Size(566, 105);
			this.excelPage.TabIndex = 3;
			this.excelPage.Text = "Excel";
			this.excelPage.UseVisualStyleBackColor = true;
			// 
			// memoryExcel
			// 
			this.memoryExcel.AutoSize = true;
			this.memoryExcel.Location = new System.Drawing.Point(223, 8);
			this.memoryExcel.Name = "memoryExcel";
			this.memoryExcel.Size = new System.Drawing.Size(105, 17);
			this.memoryExcel.TabIndex = 8;
			this.memoryExcel.Text = "Use less memory";
			this.memoryExcel.UseVisualStyleBackColor = true;
			this.memoryExcel.CheckedChanged += new System.EventHandler(this.SetSource);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(5, 7);
			this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(38, 13);
			this.label6.TabIndex = 7;
			this.label6.Text = "Sheet:";
			// 
			// excelSheet
			// 
			this.excelSheet.Location = new System.Drawing.Point(66, 7);
			this.excelSheet.Margin = new System.Windows.Forms.Padding(2);
			this.excelSheet.Name = "excelSheet";
			this.excelSheet.Size = new System.Drawing.Size(54, 20);
			this.excelSheet.TabIndex = 6;
			this.excelSheet.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.excelSheet.ValueChanged += new System.EventHandler(this.SetSource);
			// 
			// excelColumns
			// 
			this.excelColumns.Location = new System.Drawing.Point(66, 80);
			this.excelColumns.Margin = new System.Windows.Forms.Padding(2);
			this.excelColumns.Name = "excelColumns";
			this.excelColumns.Size = new System.Drawing.Size(120, 20);
			this.excelColumns.TabIndex = 5;
			this.excelColumns.Text = "A, B, C";
			this.excelColumns.TextChanged += new System.EventHandler(this.SetSource);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(5, 81);
			this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(50, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "Columns:";
			// 
			// excelEndRow
			// 
			this.excelEndRow.Location = new System.Drawing.Point(66, 56);
			this.excelEndRow.Margin = new System.Windows.Forms.Padding(2);
			this.excelEndRow.Name = "excelEndRow";
			this.excelEndRow.Size = new System.Drawing.Size(54, 20);
			this.excelEndRow.TabIndex = 3;
			this.excelEndRow.ValueChanged += new System.EventHandler(this.SetSource);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(5, 57);
			this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(54, 13);
			this.label4.TabIndex = 2;
			this.label4.Text = "End Row:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(5, 32);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(57, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "Start Row:";
			// 
			// excelStartRow
			// 
			this.excelStartRow.Location = new System.Drawing.Point(66, 32);
			this.excelStartRow.Margin = new System.Windows.Forms.Padding(2);
			this.excelStartRow.Name = "excelStartRow";
			this.excelStartRow.Size = new System.Drawing.Size(54, 20);
			this.excelStartRow.TabIndex = 0;
			this.excelStartRow.ValueChanged += new System.EventHandler(this.SetSource);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripSeparator1,
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripSeparator2,
            this.toolStripButton7,
            this.toolStripButton8,
            this.toolStripSeparator3,
            this.length});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(584, 25);
			this.toolStrip1.TabIndex = 11;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuFileOpenIcon;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "toolStripButton1";
			this.toolStripButton1.ToolTipText = "Read File...";
			this.toolStripButton1.Click += new System.EventHandler(this.fileClick);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.Image = global::JohnsHope.FPlot.Properties.Resources.ok;
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton2.Text = "toolStripButton2";
			this.toolStripButton2.ToolTipText = "Apply Changes & Close";
			this.toolStripButton2.Click += new System.EventHandler(this.okClick);
			// 
			// toolStripButton3
			// 
			this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton3.Image = global::JohnsHope.FPlot.Properties.Resources.delete;
			this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton3.Name = "toolStripButton3";
			this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton3.ToolTipText = "Discard Changes & Close";
			this.toolStripButton3.Click += new System.EventHandler(this.CancelClick);
			// 
			// toolStripButton4
			// 
			this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
			this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton4.Name = "toolStripButton4";
			this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton4.Text = "toolStripButton4";
			this.toolStripButton4.ToolTipText = "Compile";
			this.toolStripButton4.Click += new System.EventHandler(this.CompileClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButton5
			// 
			this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton5.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditUndoIcon;
			this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton5.Name = "toolStripButton5";
			this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton5.Text = "toolStripButton5";
			this.toolStripButton5.ToolTipText = "Undo Text Editor Changes";
			this.toolStripButton5.Click += new System.EventHandler(this.UndoClick);
			// 
			// toolStripButton6
			// 
			this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton6.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuEditRedoIcon;
			this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton6.Name = "toolStripButton6";
			this.toolStripButton6.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton6.Text = "toolStripButton6";
			this.toolStripButton6.ToolTipText = "Redo Text Editor Changes";
			this.toolStripButton6.Click += new System.EventHandler(this.RedoClick);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButton7
			// 
			this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton7.Image = global::JohnsHope.FPlot.Properties.Resources.Icons_MenuHelpHelpTopicsIcon;
			this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton7.Name = "toolStripButton7";
			this.toolStripButton7.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton7.Text = "toolStripButton7";
			this.toolStripButton7.ToolTipText = "Show Help...";
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
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// length
			// 
			this.length.Name = "length";
			this.length.Size = new System.Drawing.Size(56, 22);
			this.length.Text = "Length: 0";
			this.length.ToolTipText = "The Number of Data Records";
			// 
			// source
			// 
			this.source.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.source.BackColor = System.Drawing.SystemColors.Control;
			this.source.Location = new System.Drawing.Point(1, 202);
			this.source.Name = "source";
			this.source.Size = new System.Drawing.Size(582, 360);
			this.source.TabIndex = 12;
			// 
			// LoadDataForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 578);
			this.Controls.Add(this.source);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.outro);
			this.Controls.Add(this.intro);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MinimumSize = new System.Drawing.Size(384, 365);
			this.Name = "LoadDataForm";
			this.Text = "Load Data";
			this.tabControl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.asciiPage.ResumeLayout(false);
			this.asciiPage.PerformLayout();
			this.binaryPage.ResumeLayout(false);
			this.binaryPage.PerformLayout();
			this.byteordering.ResumeLayout(false);
			this.wavPage.ResumeLayout(false);
			this.wavPage.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.channel)).EndInit();
			this.excelPage.ResumeLayout(false);
			this.excelPage.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.excelSheet)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.excelEndRow)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.excelStartRow)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion


		public void ResetExcelColumns() {
			DataItem temp = item;
			item = null;

			int ExcelColumns = temp.Dimensions;
			if (temp.ErrorColumns) ExcelColumns *= 2;
			excelColumns.Text = "";
			int i;
			for (i = 0; i < ExcelColumns-1; i++) {
				excelColumns.Text += ((char)((int)'A' + i)).ToString() + ", ";
			}
			if (i < ExcelColumns) excelColumns.Text += ((char)((int)'A' + i)).ToString();

			item = temp;
		}

		public void ResetLength() {
			length.Text = Properties.Resources.Length + ": " + item.Length;
		}

		public void Reset() {
			ResetExcelColumns();
			ResetLength();
			SetSource();
		}

		public void ResetEncodings() {
			encodingnames = new string[encodings.Length];
			int i;
			for(i = 0; i < encodings.Length; i++) {
				encodingnames[encodings.Length - i - 1] = encodings[i].Name;
			}
			encoding.Items.AddRange(encodingnames);
			i = 0;
			while (i < encodingnames.Length && encodingnames[i] != "utf-8") i++;
			if (i == encodingnames.Length) i = 0;

			encoding.SelectedIndex = i;
		}

		public void Apply() {
			item.Source = source.Text;
			item.Compile();
			source.MarkErrors(item);
		}

		public void SetSource() {
			if (item != null) {
				byteordering.Enabled = littleendian.Enabled = bigendian.Enabled =
					(!sbytebutton.Checked && !bytebutton.Checked && !floatbutton.Checked && !doublebutton.Checked);
				channel.Enabled = !allchannels.Checked;
				StringBuilder b = new StringBuilder();
				if (tabControl.SelectedTab == asciiPage) {
					item.UseLowMemory = memoryText.Checked;
					item.SetLoadTextSource(separators.Text, localFormat.Checked, Encoding.GetEncoding(encodingnames[encoding.SelectedIndex]));
				} else if (tabControl.SelectedTab == binaryPage) {
					item.UseLowMemory = memoryBinary.Checked;
					Type t = null;
					if (sbytebutton.Checked) t = typeof(sbyte);
					else if (int16button.Checked) t = typeof(short);
					else if (int32button.Checked) t = typeof(int);
					else if (int64button.Checked) t = typeof(long);
					else if (bytebutton.Checked) t = typeof(byte);
					else if (uint16button.Checked) t = typeof(ushort);
					else if (uint32button.Checked) t = typeof(uint);
					else if (uint64button.Checked) t = typeof(ulong);
					else if (floatbutton.Checked) t = typeof(float);
					else if (doublebutton.Checked) t = typeof(double);
					item.SetLoadBinarySource(t, bigendian.Checked);
				} else if (tabControl.SelectedTab == wavPage) {
					item.UseLowMemory = memoryWAV.Checked;
					DataColumn s;
					switch (samples.SelectedIndex) {
					case 0: s = null; break;
					case 1: s = item.x; break;
					case 2: s = item.y; break;
					case 3: s = item.z; break;
					case 4: s = item.dx; break;
					case 5: s = item.dy; break;
					case 6: s = item.dz; break;
					default: s = null; break;
					}
					if (allchannels.Checked) item.SetLoadWAVSource(s);
					else item.SetLoadWAVSource((int)channel.Value, s);
				} else if (tabControl.SelectedTab == excelPage) {
					item.UseLowMemory = memoryExcel.Checked;
					string[] cols = excelColumns.Text.Split(',', ';');
					int col;
					for (int i = 0; i < cols.Length; i++) {
						if (int.TryParse(cols[i], out col)) {
							cols[i] = ((char)(((int)'A') + col - 1)).ToString();
						} else {
							cols[i] = cols[i].Trim();
						}
					}
					if (excelEndRow.Value <= 0 && excelStartRow.Value <= 0 && excelSheet.Value == 1) {
						item.SetLoadExcelSource(cols);
					} else if (excelEndRow.Value <= 0 && excelSheet.Value == 1) {
						item.SetLoadExcelSource((int)excelStartRow.Value, cols);
					} else item.SetLoadExcelSource((int)excelSheet.Value, (int)excelStartRow.Value, (int)excelEndRow.Value, cols);
				}

				source.Text = item.Source;
				source.Refresh();
				source.Invalidate();
			}
		} 

		public void SetSource(object sender, System.EventArgs e) { SetSource(); }


		private void fileClick(object sender, System.EventArgs e) {
			Apply();
			if (item.Compiled) {
				DialogResult res = openFileDialog.ShowDialog();
				if (res == DialogResult.OK) {
					if (File.Exists(openFileDialog.FileName)) {
						try {
							item.Load(openFileDialog.FileName);
						} catch (Exception ex) {
							ExceptionForm f = new ExceptionForm(Model, ex);
							f.Show();
							f.BringToFront();
						}
					} else MessageBox.Show(Properties.Resources.FileNotFound + " " + openFileDialog.FileName);
					ResetLength();
				}
			}
			data.Reset();
		}

		private void okClick(object sender, System.EventArgs e) {
			Apply();
			this.Hide();
		}

		private void helpClick(object sender, System.EventArgs e) {
			Help.ShowHelp(this, Properties.Resources.HelpFile, "LoadForm.html");
		}

		private void CompileClick(object sender, EventArgs e) {
			Apply();
		}

		public void Edit(SourceLocation loc) { source.Edit(loc); }

		private void CancelClick(object sender, EventArgs e) {
			this.Hide(); 
		}

		private void UndoClick(object sender, EventArgs e) {
			source.Editor.Undo();
		}

		private void RedoClick(object sender, EventArgs e) {
			source.Editor.Redo();
		}

		public void helpLibraryClick(object sender, EventArgs e) {
			Help.ShowHelp(this, Properties.Resources.LibraryHelpFile);
		}

	}
}
