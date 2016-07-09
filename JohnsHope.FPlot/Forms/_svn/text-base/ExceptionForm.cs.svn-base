using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JohnsHope.FPlot {
	public partial class ExceptionForm:Form {
		MainModel Model;

		public ExceptionForm(MainModel Model, Exception e) {
			this.Model = Model;
			grid = new StackGrid(Model);
			InitializeComponent();
			grid.Model = Model;
			message.Text = Properties.Resources.ExceptionIntro + " " + e.Message;
			grid.Reset(e);
		}

		private void ok_Click(object sender, EventArgs e) {
			Hide();
		}

		private void ConsoleClick(object sender, EventArgs e) {
			if (Model.ConsoleForm.WindowState == FormWindowState.Minimized) Model.ConsoleForm.WindowState = FormWindowState.Normal;
			Model.ConsoleForm.Show();
			Model.ConsoleForm.BringToFront();
		}

	}
}