using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// A class that conatins the enlarge and shrink mouse cursors.
	/// </summary>
	public static class SpecialCursors {
		/// <summary>
		/// A mouse cursor representing an enlarging glass.
		/// </summary>
		public static Cursor EnlargeCursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("JohnsHope.FPlot.Library.Resources.Enlarge.cur"));
		/// <summary>
		/// A mouse cursor representing a shrinking glass.
		/// </summary>
		public static Cursor ShrinkCursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("JohnsHope.FPlot.Library.Resources.Shrink.cur"));
	}
}
