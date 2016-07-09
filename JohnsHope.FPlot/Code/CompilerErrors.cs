using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;

namespace JohnsHope.FPlot {
	/// <summary>
	/// A class that converts a CompilerErrorCollection to a text representation.
	/// </summary>
	public static class CompilerErrors {
		/// <summary>
		/// If true, the error numbers are also displayed
		/// </summary>
		public static bool ShowErrorNumbers = false;
		/// <summary>
		/// Converts a CompilerErrorCollection to an array of strings. 
		/// </summary>
		/// <param name="errors">The errors to convert</param>
		public static string ToString(CompilerErrorCollection errors) {
			StringBuilder text = new StringBuilder();
			if (errors != null) {
				foreach (CompilerError err in errors) {
					text.Append(err.Line);
					text.Append(", ");
					text.Append(err.Column);
					if (err.IsWarning) text.Append(Properties.Resources.Warning);
					text.Append(": ");
					text.AppendLine(Message(err));
				}
			}
			return text.ToString();
		}
		/// <summary>
		/// Converts a CompilerErrorCollection to an array of strings. 
		/// </summary>
		/// <param name="errors">The errors to convert</param>
		public static string Message(CompilerError err) {
			string s = null;
			if (err != null) {
				s = err.ErrorText;
				if (ShowErrorNumbers) s += " [" + err.ErrorNumber + "] ";
			}
			return s;
		}
	}

}
