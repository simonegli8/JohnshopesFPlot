using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using JohnsHope.Update;
using JohnsHope.FPlot.Library;
using JohnsHope.FPlot.Properties;

namespace JohnsHope.FPlot.Util {
	public class Start {

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] argsarray) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			List<string> args = new List<string>(argsarray);
			Excel.Init();

			// if (Args.Count > 0 && Args[0] == "/t") { Test.Run(); return; }
			if (args.Count > 0 && args[0] == "/u") {
				Updater.DelayDays = 0;
				args.RemoveAt(0);
			}

			MainForm form = new MainForm();
			form.Show();

			Updater.Start();

			Gradients.Reset();

			if (args.Count > 0) { // Load file if passed as argument
				try {
					form.Model.Load(args[0]);
				} catch { MessageBox.Show(Resources.FileOpenFail.Replace("#file", args[0])); }
			}
			Application.Run(form); // FPlot runs normally
		}

	}
}
