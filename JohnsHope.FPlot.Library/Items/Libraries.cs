using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// This class represents a source library that can be compiled and imported by the other Items.
	/// You can implement library routines in this class, compile them and import them in a function.
	/// </summary>
	[Serializable]
	public class Library: Item, ICompilableLibrary {
		/// <summary>
		/// The complete source code of the library.
		/// </summary>
		public string Source = "";
		/// <summary>
		/// The default constructor.
		/// </summary>
		public Library() : base() { version = DateTime.Now; }
		/// <summary>
		/// Returns the source of the library.
		/// </summary>
		public override string GetSource() { return Source; }
		/// <summary>
		/// Returns the name of the Library.
		/// </summary>
		/// <returns></returns>
		public string GetName() {
			return Name;
		}
		/// <summary>
		/// The complete filename of the Library incl. path.
		/// </summary>
		public string Filename;

		DateTime version;
		/// <summary>
		/// The modification time of the Library.
		/// </summary>
		public DateTime Version {
			get { return version; }
			set { version = value; }
		}

		/// <summary>
		/// Copies from another Library.
		/// </summary>
		public void CopyFrom(Library source) {
			base.CopyFrom(source);
			this.Source = source.Source;
			this.Version = source.Version;
			this.Filename = source.Filename;
		}
		/// <summary>
		/// Loads the Library from a text file.
		/// </summary>
		/// <param name="filename">The file to load</param>
		public void Load(string filename) {
			Source = File.ReadAllText(filename);
			Name = Path.GetFileName(filename);
			Filename = filename;
		}
		/// <summary>
		/// Saves the 
		/// </summary>
		public void Save() {
			if (string.IsNullOrEmpty(Filename)) throw new ArgumentException("Cannot save Library becaus Filename is empty");
			File.WriteAllText(Filename, Source);
		}
		/// <summary>
		/// Saves the Library in the specified file.
		/// </summary>
		/// <param name="filename">The filename used to save the Library</param>
		public void SaveAs(string filename) {
			File.WriteAllText(filename, Source);
			Name = Path.GetFileName(filename);
			Filename = filename;
		}

		/// <summary>
		/// Returns the namespace extracted from <see cref="Source"/>.
		/// </summary>
		public string Namespace { get { return Parser.Namespace(Source); } }
	}
}
