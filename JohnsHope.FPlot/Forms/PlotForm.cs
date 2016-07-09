using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using JohnsHope.FPlot.Library;
using JohnsHope.FPlot.Properties;

namespace JohnsHope.FPlot {

	public partial class PlotForm: Form, IPlotForm {

		private RangeForm rangeForm;
		private OptionsForm optionsForm;
		public static FormStack<PlotForm> Forms = new FormStack<PlotForm>();
		private AboutForm aboutForm;
		private PageSettings ps = new PageSettings();
		private PrintDocument printDocument;
		private static int index = 0;

		public static event EventHandler FormActivated {
			add { FormStack<PlotForm>.FormActivated += value; }
			remove { FormStack<PlotForm>.FormActivated -= value; }
		}

		public PlotForm(ItemsModel Items, PlotModel Model, Rectangle Bounds) {

			InitializeComponent();

			if (Bounds.Width > 0 && Bounds.Height > 0) this.Bounds = Bounds;
			plotControl.Model = Model;
			plotControl.ProgressBar = Progress;
			plotControl.Plot.NotifyCursor += NotifyCursor;
			ResetName();
			Items.RegisterPlotForm(this);
			if (Model.ItemsModel != Items) Model.ItemsModel = Items;
			printDocument = new PlotPrintDocument(plotControl);

			Forms.Push(this);
		}

		public PlotForm(ItemsModel Items, PlotModel Model): this(Items, Model, new Rectangle(0, 0, 0, 0)) { }

		public PlotForm(ItemsModel Items, Plot.Type Type) : this(Items, new PlotModel(Items, Type)) { }

		public void ResetName() {
			Text = Resources.PlotFrameTitle + " " + index++;
			foreach (Item x in PlotModel) {
				if (!(x is Library.Library)) {
					Text += " " + x.Name;
					break;
				}
			}
			PlotModel.Filename = Text + Resources.FileExtension;
		}

		public PlotModel PlotModel {
			get { return plotControl.Model; }
			set {	plotControl.Model = value; }
		}

		public static PlotForm TopForm {
			get { return Forms.Top; }
		}

		private void NotifyCursor(double x, double y, double z) {
			Coordinates.Text = "x = " + x.ToString() + "; y = " + y.ToString() + "; z = " + z.ToString() + ";";
		}

		public static IPlotForm OpenPlot(ItemsModel Model, ItemsModel.Plot Plot) {
			if (Plot.New) {
				PlotForm f = new PlotForm(Model, Plot.PlotModel, Plot.Bounds);
				f.Show();
				f.Location = new Point(Plot.Bounds.X, Plot.Bounds.Y);
				f.BringToFront();
				return f;
			}
			return null;
		}
		
		public static void OpenPlots(ItemsModel Items) {
			Items.OpenPlots(OpenPlot);
		}

		// Menu commands

		private void SaveClick(object sender, EventArgs e) {
			ItemsModel m = PlotModel.ItemsModel.Separate(this);
			m.Save();
		}

		private void SaveAsClick(object sender, EventArgs e) {
			saveFileDialog.FileName = Text + Resources.FileExtension;
			DialogResult res = saveFileDialog.ShowDialog();
			if (res == DialogResult.OK) {
				string ext = System.IO.Path.GetExtension(saveFileDialog.FileName);
				if (ext == Resources.FileExtension) {
					Text = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
					ItemsModel m = PlotModel.ItemsModel.Separate(this);
					m.Filename = saveFileDialog.FileName;
					m.Save();
				} else { // image file
					try {
						plotControl.SaveAsImage(saveFileDialog.FileName);
					} catch (NotSupportedException) {
						MessageBox.Show(Properties.Resources.ImageFormatNotSupported);
					}
				}
			}
		}

		private void PrintPreviewClick(object sender, EventArgs e) {
			printPreviewDialog.Document = printDocument;
			DialogResult res = printPreviewDialog.ShowDialog();
		}

		private void PageSetupClick(object sender, EventArgs e) {
			pageSetupDialog.Document = printDocument;
			DialogResult res = pageSetupDialog.ShowDialog();
			if (res == DialogResult.OK && System.Globalization.RegionInfo.CurrentRegion.IsMetric) {
			PageSettings settings = printDocument.DefaultPageSettings;
				settings.Margins.Left = (int) (settings.Margins.Left*2.54 + 0.5);
				settings.Margins.Top = (int)(settings.Margins.Top*2.54 + 0.5);
				settings.Margins.Right = (int)(settings.Margins.Right*2.54 + 0.5);
				settings.Margins.Bottom = (int)(settings.Margins.Bottom*2.54 + 0.5);
			}
		}

		private void PrintClick(object sender, EventArgs e) {
			printDialog.Document = printDocument;
			DialogResult res = printDialog.ShowDialog();
			if (res == DialogResult.OK) {
				printDocument.Print();
			}
		}

		private void CloseClick(object sender, EventArgs e) {
			Close();
		}

		private void PlotRangeClick(object sender, EventArgs e) {
			if (rangeForm == null || rangeForm.IsDisposed) rangeForm = new RangeForm(plotControl);
			rangeForm.Reset();
			if (rangeForm.WindowState == FormWindowState.Minimized) rangeForm.WindowState = FormWindowState.Normal;
			rangeForm.Show();
			rangeForm.BringToFront();
		}

		private void OptionsClick(object sender, EventArgs e) {
			if (optionsForm == null || optionsForm.IsDisposed) optionsForm = new OptionsForm(plotControl);
			optionsForm.Reset();
			if (optionsForm.WindowState == FormWindowState.Minimized) optionsForm.WindowState = FormWindowState.Normal;
			optionsForm.Show();
			optionsForm.BringToFront();
		}

		private void ContentClick(object sender, EventArgs e) {
			Help.ShowHelp(this, Resources.HelpFile);
		}

		private void AboutClick(object sender, EventArgs e) {
			if (aboutForm == null || aboutForm.IsDisposed) aboutForm = new AboutForm();
			if (aboutForm.WindowState == FormWindowState.Minimized) aboutForm.WindowState = FormWindowState.Normal;
			aboutForm.Show();
			aboutForm.BringToFront();
		}

	}
}