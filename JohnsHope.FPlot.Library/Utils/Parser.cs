using System;
using System.Collections.Generic;
using System.Text;

namespace JohnsHope.FPlot.Library {

	/// <summary>
	/// A class that implements basic parsing of C# texts.
	/// </summary>
	public class Parser {

		static readonly int NL = "namespace".Length;
		static readonly int UL = "using".Length;


		/// <summary>
		/// Counts the number of lines in the passed code.
		/// </summary>
		public static int Lines(string code) {
			int n = 0;
			char[] c = code.ToCharArray();
			for (int i = 0; i < c.Length; i++) {
				if (c[i] == '\n') n++;
			}
			return n;
		}

		/// <summary>
		/// Removes comments from the passed code.
		/// </summary>
		public static string RemoveComments(string code) {
			int s, e;

			StringBuilder str = new StringBuilder();	// convert usings & stats to string

			s = 0;

			do {
				e = code.IndexOf('/', s);

				while (e >= 0 && e < code.Length - 1 && code[e+1] != '/' && code[e+1] != '*') e = code.IndexOf('/', e+1);

				if (e >= 0 && e < code.Length - 1) { // found a comment
					if (s < e) str.Append(code.Substring(s, e - s));
					if (code[e+1] == '*') { // multi line comment
						s = code.IndexOf("*/", e+2);
						if (s == -1) break;
						s += 2;
					} else { // single line comment
						s = code.IndexOf('\n', e+2);
						if (s == -1) break;
						else if (!(s > 0 && code[s-1] == '\r') && (s < code.Length - 1 && code[s+1] == '\r')) s += 2;
						else s++;
						str.AppendLine();
					}
				} else {
					if (s < code.Length) str.Append(code.Substring(s, code.Length - s));
					break;
				}
			} while (true);

			return str.ToString();
		}

		/// <summary>
		/// Returns the main namespace of the code.
		/// </summary>
		public static string Namespace(string code) {
			code = RemoveComments(code);
			int s, e;
			s = code.IndexOf("namespace");
			while (s >= 0 && !(s > 0 && char.IsWhiteSpace(code[s-1])) && !(s < code.Length - NL && char.IsWhiteSpace(code[s+NL])))
				s = code.IndexOf("namespace", s + NL);
			if (s >= 0) {
				s += NL;
				e = code.IndexOf('{', s);
				if (e > s) return code.Substring(s, e - s).Trim();
			}
			return "";
		}


		private static int UsingIndex(string a) {
			if (!a.StartsWith("using ")) return 4;
			else if (a == "using System") return 0;
			else if (a.StartsWith("using System.")) return 1;
			else if (a.StartsWith("using Microsoft.")) return 2;
			else if (a.Contains(".")) return 3;
			else return 4;
		}

		private static int UsingComparer(string a, string b) {
			int ai = UsingIndex(a), bi = UsingIndex(b);
			if (ai < bi) return -1;
			else if (ai > bi) return 1;
			else return string.Compare(a, b);
		}

		/// <summary>
		/// Reformats the passed code, sorting the using directives at the top and removing duplicate using directives.
		/// </summary>
		public static string SortUsingHeader(string code) {
			int i, s, ws;
			string stat;

			code = RemoveComments(code);

			StringBuilder str = new StringBuilder();

			List<string> stats = new List<string>(code.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries));
			List<string> usings = new List<string>();

			// extract using statements
			for (i = 0; i < stats.Count; i++) {
				stat = stats[i];
				s = stat.IndexOf("using");
				while (s >= 0 && s < stat.Length - UL && // search for using directive
					(!(s == 0 || char.IsWhiteSpace(stat[s-1])) || 
					!char.IsWhiteSpace(stat[s+UL]) ||
					stat.IndexOf('(', s + UL) != -1)) s = stat.IndexOf("using", s + UL);
				
				if (s >= 0 && s < stat.Length - UL) { // found a using directive
					usings.Add(stat.Substring(s, stat.Length - s).Trim());
					ws = s;
					while (ws > 0 && char.IsWhiteSpace(stat[ws-1])) ws--;	// remove white space in front of using directive.
					stat = stat.Remove(ws, stat.Length - ws);
					if (i < stats.Count - 1) {
						stats.RemoveAt(i);
						stats[i] = stat + stats[i--];
					} else stats[i] = stat;
				}
			}

			// remove empty stats
			for (i = 0; i < stats.Count; i++) if (stats[i].Trim() == "") stats.RemoveAt(i--);

			usings.Sort(new Comparison<string>(UsingComparer));

			// write usings and stats to str
			for (i = 0; i < usings.Count; i++) {
				if (i == 0 || usings[i] != usings[i-1]) str.AppendLine(usings[i] + ";");
			}

			i = 0;
			if (i < stats.Count) str.Append(stats[i++]);
			while (i < stats.Count) str.Append(";" + stats[i++]);

			code = str.ToString().Trim();

			// remove double empty lines
			str = new StringBuilder();
			stats = new List<string>(code.Split('\n'));
			for (i = 0; i < stats.Count; i++) {
				stat = stats[i].Trim();
				if (stat != "" || (i < stats.Count - 1 && stats[i+1].Trim() != "")) str.AppendLine(stat);
			}

			return str.ToString();
		}
	}
}
