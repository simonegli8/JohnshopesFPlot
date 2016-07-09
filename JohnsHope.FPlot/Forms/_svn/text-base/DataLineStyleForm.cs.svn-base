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
	public partial class DataLineStyleForm: Form {
		DataItem data;
		MainModel Model;

		public DataLineStyleForm(MainModel m, DataItem data) {
			InitializeComponent();
			Model = m;
			lineStyle.Reset();
			Reset(data);
		}

		public void Reset(DataItem data) {
			this.data = data;
			lineStyle.SelectedStyle = data.LineStyle;
			width.Text = data.LineWidth.ToString();
			color.BackColor = data.Color;
			marks.Checked = data.Marks;
			line.Checked = data.Lines;
			this.Text = data.Name + " - " + Properties.Resources.LineStyle;

		}

		private static Dictionary<Item, DataLineStyleForm> Forms = new Dictionary<Item, DataLineStyleForm>();

		public static DataLineStyleForm New(MainModel m, DataItem x) {

			// cleanup Forms
			List<Item> closedKeys = new List<Item>();
			foreach (Item y in Forms.Keys) {
				if (!Forms[y].Visible) closedKeys.Add(y);
			}
			foreach (Item y in closedKeys) Forms.Remove(y);

			DataLineStyleForm f = null;
			Forms.TryGetValue(x, out f);
			if (f == null) f = new DataLineStyleForm(m, x);
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
			data.Color = color.BackColor;
			data.LineStyle = lineStyle.SelectedStyle;
			data.LineWidth = 1;
			data.Marks = marks.Checked;
			data.Lines = line.Checked;
			try {
				data.LineWidth = float.Parse(width.Text);
			} catch { }
			Model.Items.Update(data);
			Hide();
		}

		private void CancelClick(object sender, EventArgs e) {
			Hide();
		}

	}
}