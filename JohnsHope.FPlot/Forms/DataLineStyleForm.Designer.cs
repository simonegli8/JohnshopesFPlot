namespace JohnsHope.FPlot {
	partial class DataLineStyleForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataLineStyleForm));
			this.button1 = new System.Windows.Forms.Button();
			this.lineStyle = new JohnsHope.FPlot.Library.LineStyleChooser();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.color = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.width = new System.Windows.Forms.TextBox();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.line = new System.Windows.Forms.CheckBox();
			this.marks = new System.Windows.Forms.CheckBox();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Location = new System.Drawing.Point(12, 127);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(158, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Choose";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.ChooseClick);
			// 
			// lineStyle
			// 
			this.lineStyle.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lineStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.lineStyle.FormattingEnabled = true;
			this.lineStyle.Location = new System.Drawing.Point(77, 19);
			this.lineStyle.Name = "lineStyle";
			this.lineStyle.SelectedStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			this.lineStyle.Size = new System.Drawing.Size(121, 21);
			this.lineStyle.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(54, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Line style:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 51);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(34, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Color:";
			// 
			// color
			// 
			this.color.Location = new System.Drawing.Point(74, 43);
			this.color.Name = "color";
			this.color.Size = new System.Drawing.Size(23, 25);
			this.color.TabIndex = 4;
			this.color.Click += new System.EventHandler(this.ColorClick);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 82);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(58, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "Line width:";
			// 
			// width
			// 
			this.width.Location = new System.Drawing.Point(77, 79);
			this.width.Name = "width";
			this.width.Size = new System.Drawing.Size(100, 20);
			this.width.TabIndex = 6;
			// 
			// line
			// 
			this.line.Location = new System.Drawing.Point(235, 38);
			this.line.Name = "line";
			this.line.Size = new System.Drawing.Size(136, 24);
			this.line.TabIndex = 36;
			this.line.Text = "Join points with a line";
			// 
			// marks
			// 
			this.marks.Location = new System.Drawing.Point(235, 14);
			this.marks.Name = "marks";
			this.marks.Size = new System.Drawing.Size(112, 24);
			this.marks.TabIndex = 37;
			this.marks.Text = "Draw error marks";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(201, 127);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 38;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.CancelClick);
			// 
			// DataLineStyleForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(386, 161);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.line);
			this.Controls.Add(this.marks);
			this.Controls.Add(this.width);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.color);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lineStyle);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DataLineStyleForm";
			this.ShowInTaskbar = false;
			this.Text = "Data line style";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private JohnsHope.FPlot.Library.LineStyleChooser lineStyle;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label color;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox width;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.CheckBox line;
		private System.Windows.Forms.CheckBox marks;
		private System.Windows.Forms.Button button2;
	}
}