using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using JohnsHope.FPlot.Library;
using JohnsHope.FPlot.Properties;

namespace JohnsHope.FPlot {
	public partial class GradientForm: Form {
		public IGradient SelectedGradient = Gradients.List[0];

		public GradientForm() {
			InitializeComponent();
			upperColor.BackColor = Gradients.LinearGradient.UpperColor;
			lowerColor.BackColor = Gradients.LinearGradient.LowerColor;
			gradientChooser.Reset();
		}

		private void ImportClick(object sender, EventArgs e) {
			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				string src = openFileDialog.FileName;
				string dest = Path.Combine(Resources.GradientPath, Path.GetFileName(src));
				System.IO.File.Copy(src, dest);
				try {
					Gradients.Reset();
				} catch (Exception ex) {
					string file = "";
					if (ex.Data.Contains("filename")) file = ex.Data["filename"] as string;
					MessageBox.Show(Properties.Resources.GIMPError.Replace("$f", file));
				}
				gradientChooser.Reset();
				gradientChooser.Refresh();
			}
		}

		private void UpperClick(object sender, EventArgs e) {
			IGradient g = gradientChooser.SelectedGradient;
			if (g is LinearGradient) {
				LinearGradient lg = (LinearGradient)g;
				colorDialog.ShowDialog();
				lg.UpperColor = colorDialog.Color;
				upperColor.BackColor = colorDialog.Color;
				Gradients.LinearGradient.UpperColor = colorDialog.Color;
				gradientChooser.Refresh();
			}
		}

		private void LowerClick(object sender, EventArgs e) {
			IGradient g = gradientChooser.SelectedGradient;
			if (g is LinearGradient) {
				LinearGradient lg = (LinearGradient)g;
				colorDialog.ShowDialog();
				lg.UpperColor = colorDialog.Color;
				lowerColor.BackColor = colorDialog.Color;
				Gradients.LinearGradient.LowerColor = colorDialog.Color;
				gradientChooser.Refresh();
			}
		}

		private void ChooseClick(object sender, EventArgs e) {
			SelectedGradient = gradientChooser.SelectedGradient.Clone();
		}

		private void SelectedIndexChanged(object sender, EventArgs e) {
			groupBox.Enabled = gradientChooser.SelectedGradient is LinearGradient;
		}

		public static new IGradient ShowDialog() {
			GradientForm f = new GradientForm();
			((Form)f).ShowDialog();
			return f.SelectedGradient;
		}

	}
}