using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JohnsHope.FPlot.Library {
	
	/// <summary>
	/// A StringWriter class that implements indenting.
	/// </summary>
	public class StringWriter: System.IO.StringWriter {
		/// <summary>
		/// The number of tabs to indent each line with.
		/// </summary>
		public int Ident = 0;
		int col = 0;

		private void ident() {
			if (col == 0 && Ident > 0) {
				int i = 0;
				while (i++ < Ident) base.Write("\t");
			}
		}
		/// <summary>
		/// Writes a character to the StringWriter.
		/// </summary>
		public override void Write(char value) {
			ident();
			base.Write(value);
		}
		
		/// <summary>
		/// Writes a string to the StringWriter.
		/// </summary>
		public override void Write(string value) {
			ident();
			base.Write(value);
			if (value.Contains(base.NewLine)) col = 0;
		}

		/// <summary>
		/// Writes a character and a new line to the StringWriter.
		/// </summary>
		public override void WriteLine(char c) {
			ident();
			base.WriteLine(c);
			col = 0;
		}
		
		/// <summary>
		/// Writes a line to the StringWriter.
		/// </summary>
		public override void WriteLine(string value) {
			ident();
			base.WriteLine(value);
			col = 0;
		}
	}
}
