using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace JohnsHope.Update {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			if (args.Length == 1) {
				Updater.Update(args[0]);
				Application.Run();
			}
		}
	}
}
