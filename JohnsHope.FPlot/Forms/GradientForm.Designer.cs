namespace JohnsHope.FPlot {
	partial class GradientForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GradientForm));
			this.label0 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.label2 = new System.Windows.Forms.Label();
			this.upperColor = new System.Windows.Forms.Label();
			this.lowerColor = new System.Windows.Forms.Label();
			this.groupBox = new System.Windows.Forms.GroupBox();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.import = new System.Windows.Forms.Button();
			this.Choose = new System.Windows.Forms.Button();
			this.gradientChooser = new JohnsHope.FPlot.Library.GradientChooser();
			this.groupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// label0
			// 
			this.label0.AutoSize = true;
			this.label0.Location = new System.Drawing.Point(12, 9);
			this.label0.Name = "label0";
			this.label0.Size = new System.Drawing.Size(73, 13);
			this.label0.TabIndex = 0;
			this.label0.Text = "Gradient type:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(62, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Upper color";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 58);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(62, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Lower color";
			// 
			// upperColor
			// 
			this.upperColor.BackColor = System.Drawing.Color.White;
			this.upperColor.Location = new System.Drawing.Point(102, 25);
			this.upperColor.Name = "upperColor";
			this.upperColor.Size = new System.Drawing.Size(25, 26);
			this.upperColor.TabIndex = 4;
			this.upperColor.Click += new System.EventHandler(this.UpperClick);
			// 
			// lowerColor
			// 
			this.lowerColor.BackColor = System.Drawing.Color.Black;
			this.lowerColor.Location = new System.Drawing.Point(102, 51);
			this.lowerColor.Name = "lowerColor";
			this.lowerColor.Size = new System.Drawing.Size(25, 26);
			this.lowerColor.TabIndex = 5;
			this.lowerColor.Click += new System.EventHandler(this.LowerClick);
			// 
			// groupBox
			// 
			this.groupBox.Controls.Add(this.label2);
			this.groupBox.Controls.Add(this.lowerColor);
			this.groupBox.Controls.Add(this.label1);
			this.groupBox.Controls.Add(this.upperColor);
			this.groupBox.Location = new System.Drawing.Point(187, 12);
			this.groupBox.Name = "groupBox";
			this.groupBox.Size = new System.Drawing.Size(147, 91);
			this.groupBox.TabIndex = 6;
			this.groupBox.TabStop = false;
			this.groupBox.Text = "Linear gradient colors";
			// 
			// openFileDialog
			// 
			this.openFileDialog.FileName = ".xaml";
			this.openFileDialog.Filter = "XAML Files|*.xaml|GIMP Gradient Files|*.ggr";
			// 
			// import
			// 
			this.import.Location = new System.Drawing.Point(12, 70);
			this.import.Name = "import";
			this.import.Size = new System.Drawing.Size(147, 23);
			this.import.TabIndex = 7;
			this.import.Text = "Import Gradient...";
			this.import.UseVisualStyleBackColor = true;
			this.import.Click += new System.EventHandler(this.ImportClick);
			// 
			// Choose
			// 
			this.Choose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Choose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Choose.Location = new System.Drawing.Point(96, 123);
			this.Choose.Name = "Choose";
			this.Choose.Size = new System.Drawing.Size(147, 23);
			this.Choose.TabIndex = 8;
			this.Choose.Text = "Choose";
			this.Choose.UseVisualStyleBackColor = true;
			this.Choose.Click += new System.EventHandler(this.ChooseClick);
			// 
			// gradientChooser
			// 
			this.gradientChooser.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.gradientChooser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.gradientChooser.FormattingEnabled = true;
			this.gradientChooser.Items.AddRange(new object[] {
            "0",
            "1"});
			this.gradientChooser.Location = new System.Drawing.Point(12, 34);
			this.gradientChooser.Name = "gradientChooser";
			this.gradientChooser.Size = new System.Drawing.Size(150, 21);
			this.gradientChooser.TabIndex = 1;
			this.gradientChooser.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);
			// 
			// GradientForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(348, 158);
			this.Controls.Add(this.Choose);
			this.Controls.Add(this.import);
			this.Controls.Add(this.groupBox);
			this.Controls.Add(this.gradientChooser);
			this.Controls.Add(this.label0);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "GradientForm";
			this.Text = "Gradient";
			this.groupBox.ResumeLayout(false);
			this.groupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label0;
		private JohnsHope.FPlot.Library.GradientChooser gradientChooser;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label upperColor;
		private System.Windows.Forms.Label lowerColor;
		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button import;
		private System.Windows.Forms.Button Choose;
	}
}