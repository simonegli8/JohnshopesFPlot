using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using JohnsHope.FPlot.Library;

namespace JohnsHope.FPlot {
	public partial class FunctionLineStyleForm: Form {
		Function1DItem f;
		MainModel Model;

		public FunctionLineStyleForm(MainModel m, Function1DItem f) {
			InitializeComponent();
			Model = m;
			lineStyle.Reset();
			Reset(f);
		}

		public void Reset(Function1DItem f) {
			this.f = f;
			lineStyle.SelectedStyle = f.LineStyle;
			this.width.Text = f.LineWidth.ToString();
			this.color.BackColor = f.Color;
			this.Text = f.Name + " - " + Properties.Resources.LineStyle;
		}

		private static Dictionary<Item, FunctionLineStyleForm> Forms = new Dictionary<Item, FunctionLineStyleForm>();

		public static FunctionLineStyleForm New(MainModel m, Function1DItem x) {

			// cleanup Forms
			List<Item> closedKeys = new List<Item>();
			foreach (Item y in Forms.Keys) {
				if (!Forms[y].Visible) closedKeys.Add(y);
			}
			foreach (Item y in closedKeys) Forms.Remove(y);

			FunctionLineStyleForm f = null;
			Forms.TryGetValue(x, out f);
			if (f == null) f = new FunctionLineStyleForm(m, x);
			else f.Reset(x);
			Forms[x] = f;
			if (f.WindowState == FormWindowState.Minimized) f.WindowState = FormWindowState.Normal;
			f.Show();
			f.BringToFront();
			return f;
		}

		private void ColorClick(object sender, EventArgs e) {
			colorDialog.ShowDialog();
			color.BackColor = colorDialog.Color;
		}

		private void ChooseClick(object sender, EventArgs e) {
			f.Color = color.BackColor;
			f.LineStyle = lineStyle.SelectedStyle;
			f.LineWidth = 1;
			try {
				f.LineWidth = float.Parse(width.Text);
			} catch { }
			Model.Items.Update(f);
			Hide();
		}

		private void CancelClick(object sender, EventArgs e) {
			Hide();
		}

	}
}