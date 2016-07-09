using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// Represents an object that can be compiled
	/// </summary>
	public interface ICompilable {
		/// <summary>
		/// Returns the source that will be compiled. 
		/// </summary>
		string GetSource();
		/// <summary>
		/// Sets or gets the index of the created dll and of the class-name. A class that implements ICompilable should simply
		/// store the TypeIndex locally and set the class name in the source that will be compiled accordingly.
		/// </summary>
		int TypeIndex { get; set; }
		/// <summary>
		/// Sets or gets the <see cref="CompilerResults"/> of this object.
		/// </summary>
		CompilerResults CompilerResults { get; set;}
		/// <summary>
		/// Returns the position of the first line in the source.
		/// </summary>
		int FirstSourceLine { get; }
	}

	/// <summary>
	/// Describes an ICompilable where no <see cref="CompilerOptions.SourceHeader">SourceHeader</see> header from the compiler
	/// options is inserted into the source.
	/// </summary>
	public interface ICompilableWithNoHeader: ICompilable { }

	/// <summary>
	/// This interface represents a library.
	/// </summary>
	public interface ICompilableLibrary: ICompilableWithNoHeader {
		/// <summary>
		/// Gets the name of the library
		/// </summary>
		string GetName();
		/// <summary>
		/// Returns the version of the library
		/// </summary>
		DateTime Version { get; }
	}
	/// <summary>
	/// Represents a class that can be compiled and loaded.
	/// </summary>
	public interface ICompilableClass: ICompilable {
		/// <summary>
		/// Gets the type name of the class.
		/// </summary>
		string ClassName { get; }
		/// <summary>
		/// Sets an instance of the class.
		/// </summary>
		object ClassInstance { set; }
	}
	/// <summary>
	/// A class describing compiler options.
	/// </summary>
	[Serializable]
	public class CompilerOptions {

		/// <summary>
		/// A class that implements the header section of source files that import namespaces with the "using" keyword.
		/// </summary>
		[Serializable]
		public class SourceHeader: ICompilableWithNoHeader {
			/// <summary>
			/// The text of the header section of source files.
			/// </summary>
			private string text = "using System;using System.Text;using System.IO;using System.Drawing;using System.Collections.Generic;" +
				"using System.Globalization;using System.Diagnostics;using JohnsHope.FPlot.Library;using JohnsHope.Analysis;";

			/// <summary>
			/// The default constructor.
			/// </summary>
			public SourceHeader() {
				Text = text;
			}

			/// <summary>
			/// Gets or sets the text of the source header. When set, the text is sorted automatically.
			/// </summary>
			public string Text {
				get {
					lock (this) return text;
				}
				set {
					lock (this) {
						text = Parser.SortUsingHeader(value);
					}
					CalcLines();
					OnTextChanged();
				}
			}

			/// <summary>
			/// Calles the TextChanged event.
			/// </summary>
			public virtual void OnTextChanged() {
				if (TextChanged != null) TextChanged(this, EventArgs.Empty);
			}

			/// <summary>
			/// Is fired when the <see cref="Text"/> has changed.
			/// </summary>
			public event EventHandler TextChanged;

			/// <summary>
			/// Combines the UsingHeader with the passed text and sorts it.
			/// </summary>
			/// <param name="text"></param>
			public void Combine(string text) {
				Text += text;
			}

			/// <summary>
			/// Combines the UsingHeader with another UsingHeader and sorts it.
			/// </summary>
			/// <param name="h"></param>
			public void Combine(SourceHeader h) {
				if (h != null) Combine(h.Text);
			}

			[NonSerialized]
			private int lines = 1;
			private void CalcLines() { lines = Parser.Lines(text) + 1; }
			/// <summary>
			/// Counts the number of lines in <see cref="Text"/>.
			/// </summary>
			public int Lines { get { return lines; } }

			/// <summary>
			/// Returns the source of the UsingHeader. Used internally by the <see cref="Compiler"/>.
			/// </summary>
			public string GetSource() {
				return text;
			}

			/// <summary>
			/// Gets a thread safe snapshot of this SourceHeader. Used internally by the <see cref="Compiler"/>.
			/// </summary>
			public void GetSnapshot(out string text, out int lines) {
				lock(this) {
					text = this.text;
					lines = Lines;
				}
			}

			[NonSerialized]
			int tindex = 0;
			int ICompilable.TypeIndex { get { return tindex; } set {	tindex = value; } }

			[NonSerialized]
			CompilerResults res = null;
			/// <summary>
			/// Gets or sets the <see cref="CompilerResults"/> of the last compilation.
			/// </summary>
			public CompilerResults CompilerResults { get { return res; } set { res = value; } }

			int ICompilable.FirstSourceLine { get { return 0; }	}

			/// <summary>
			/// Adds a single using directive to the SourceHeader.
			/// </summary>
			/// <param name="ns">The namespace to import.</param>
			public void AddUsing(string ns) {
				if (!string.IsNullOrEmpty(ns)) {
					Text = "using " + ns + ";" + text;
				}
			}

			[OnDeserializing]
			private void Deserializing(StreamingContext sc) {
				tindex = 0;
				res = null;
				lines = 1;
			}

			[OnDeserialized]
			private void Deserialized(StreamingContext sc) {
				CalcLines();
			}
		}

		/// <summary>
		/// A class that describes an Assembly that is imported and is packaged into the *.fplot file upon serialization,
		/// so the *.fplot file works even on machines where the assembly is not present.
		/// </summary>
		[Serializable]
		public class PackagedAssembly {

			string originalFileName = null;
			[NonSerialized]
			string originalName = null;
			byte[] buf = null;
			int dllIndex = -1;
			static int DllIndex = 0;
			DateTime version = DateTime.Now;

			/// <summary>
			/// Checks if the passed filename is a valid assembly path.
			/// </summary>
			/// <param name="file">The path to the assembly.</param>
			public static bool ValidAssembly(string file) {
				string ext = Path.GetExtension(file);
				return  ((ext == ".dll" || ext == ".exe") && File.Exists(file));
			}

			/// <summary>
			/// The default constructor.
			/// </summary>
			public PackagedAssembly() {	}

			/// <summary>
			/// The constructor.
			/// </summary>
			/// <param name="file">Sets the <see cref="OriginalFileName"/> to file.</param>
			public PackagedAssembly(string file): this() {
				OriginalFileName = file;
			}

			/// <summary>
			/// The name of the dll file the PackagedAssembly currently is stored in. This filename does not necessarily correspond to
			/// the <see cref="OriginalFileName"/>. If the assembly was deserialized from a *.fplot file it will be stored under a
			/// custom name in the <see cref="Compiler.TempPath"/>.
			/// </summary>
			public string FileName {
				get {
					if (dllIndex == -1) return originalFileName;
					int pid = Process.GetCurrentProcess().Id;
					string name;
					if (string.IsNullOrEmpty(originalFileName)) name = "PackagedAssembly" + pid.ToString("X") + "." + dllIndex + ".dll";
					else name = Path.GetFileName(originalFileName);
					return Path.Combine(Compiler.TempPath, name);
				}
			}

			/// <summary>
			/// The original filename of the assembly before it was serialized.
			/// </summary>
			public string OriginalFileName {
				get { return originalFileName; }
				set {
					originalFileName = value;
					originalName = Path.GetFileName(value);
					if (dllIndex != -1 && File.Exists(value)) version = File.GetLastWriteTime(value);
				}
			}

			/// <summary>
			/// The original filename without path of the assembly.
			/// </summary>
			public string OriginalName { get { return originalName; } }

			/// <summary>
			/// Saves the assembly under the passed filename.
			/// </summary>
			public void SaveAs(string filename) {
				File.Copy(FileName, filename);
			}

			/// <summary>
			/// The version of the assembly.
			/// </summary>
			public DateTime Version {
				get { return version; }
			}

			[OnSerializing]
			private void OnSerializing(StreamingContext sc) {
				OriginalFileName = originalFileName; // read version DateTime.
				buf = File.ReadAllBytes(FileName);
			}

			[OnSerialized]
			private void OnSerialized(StreamingContext sc) {
				buf = null;
			}

			[OnDeserialized]
			private void OnDeserialized(StreamingContext sc) {
				dllIndex = DllIndex++;
				OriginalFileName = originalFileName;
				if (dllIndex != -1) File.WriteAllBytes(FileName, buf);
				buf = null;
			}
		}

		/// <summary>
		/// A collection of <see cref="PackagedAssembly"/>es.
		/// </summary>
		[Serializable]
		public class PackagedAssemblyCollection: ICollection<PackagedAssembly>, IEnumerable<string> {

			private Dictionary<string, PackagedAssembly> dict = new Dictionary<string, PackagedAssembly>(StringComparer.InvariantCultureIgnoreCase);

			/// <summary>
			/// The default constructor.
			/// </summary>
			public PackagedAssemblyCollection() { }
			
			/// <summary>
			/// A constructor that add a list of assemblies to the collection.
			/// </summary>
			/// <param name="source">The list to copy into the collection.</param>
			public PackagedAssemblyCollection(IEnumerable<PackagedAssembly> source) { AddRange(source);	}

			/// <summary>
			/// A constructor that add a list of assemblies to the collection.
			/// </summary>
			/// <param name="source">The list to copy into the collection.</param>
			public PackagedAssemblyCollection(IEnumerable<string> source) { AddRange(source); }

			/// <summary>
			/// Returns true if the assembly or an newer version of the assembly is contained in the collection.
			/// </summary>
			public bool Contains(PackagedAssembly a) {
				if (a == null) return false;
				PackagedAssembly b = dict[a.OriginalName];
				if (b == null) return false;
				return (a.Version <= b.Version);
			}
			/// <summary>
			/// Returns true if the assembly or an newer version of the assembly is contained in the collection.
			/// </summary>
			public bool Contains(string dll) {
				if (dll == null) return false;
				return dict.ContainsKey(dll);
			}
			/// <summary>
			/// Adds an assembly to the collection.
			/// </summary>
			public void Add(PackagedAssembly item) {
				if (item != null && !Contains(item)) dict[item.OriginalName] = item;
			}
			/// <summary>
			/// Adds a dll to the packaged assemblies collection.
			/// </summary>
			/// <param name="dll">The full filename to the dll.</param>
			public void Add(string dll) {
				if (PackagedAssembly.ValidAssembly(dll)) Add(new PackagedAssembly(dll));
			}

			/// <summary>
			/// Adds a list of PackagedAssemblies to the collection.
			/// </summary>
			/// <param name="list"></param>
			public void AddRange(IEnumerable<PackagedAssembly> list) {
				foreach (PackagedAssembly a in list) Add(a);
			}

			/// <summary>
			/// Adds a list of PackagedAssemblies to the collection.
			/// </summary>
			/// <param name="list"></param>
			public void AddRange(IEnumerable<string> list) {
				foreach (string a in list) Add(a);
			}
			/// <summary>
			/// Clears the collection.
			/// </summary>
 			public void Clear() { dict.Clear(); }
			
			/// <summary>
			/// Copies the collection to an array.
			/// </summary>
			/// <param name="array">The array to copy to.</param>
			/// <param name="arrayIndex">The starting index.</param>
			public void CopyTo(PackagedAssembly[] array, int arrayIndex) { dict.Values.CopyTo(array, arrayIndex); }

			/// <summary>
			/// The number of assemblies in the collection.
			/// </summary>
			public int Count { get { return dict.Count; } }
			
			/// <summary>
			/// always false.
			/// </summary>
			public bool  IsReadOnly {	get { return false; } }

			/// <summary>
			/// Removes the assembly from the collection.
			/// </summary>
			/// <param name="item">The item to remove.</param>
			/// <returns>Returns true if the item was in the collection and removed.</returns>
			public bool Remove(PackagedAssembly item) {
				return item != null && dict.Remove(item.OriginalName);
			}
			/// <summary>
			/// Removes the assembly from the collection.
			/// </summary>
			/// <param name="dll">The full filename of the Assembly to remove.</param>
			/// <returns>Returns true if the item was in the collection and removed.</returns>
			public bool Remove(string dll) {
				return dll != null && dict.Remove(dll);
			}
			/// <summary>
			/// Gets an <see cref="IEnumerator{T}">IEnumerator&lt;PackagedAssembly&gt;</see>.
			/// </summary>
			/// <returns></returns>
			public IEnumerator<PackagedAssembly> GetEnumerator() { return dict.Values.GetEnumerator(); }

			IEnumerator IEnumerable.GetEnumerator() { return dict.Values.GetEnumerator(); }

			IEnumerator<string> IEnumerable<string>.GetEnumerator() { return dict.Keys.GetEnumerator(); }
		}

		/// <summary>
		/// Turns optimization on.
		/// </summary>
		public bool Optimize = true;
		/// <summary>
		/// Allows unsafe code.
		/// </summary>
		public bool AllowUnsafe = false;
		/// <summary>
		/// Turns creation of debug info on.
		/// </summary>
		public bool Debug = true;
		/// <summary>
		/// Turns checks for arithmetic overflow on.
		/// </summary>
		public bool CheckOverflow = true;
		/// <summary>
		/// Sets the warning level.
		/// </summary>
		public int WarningLevel = 4;
		/// <summary>
		/// The referenced dll's. Mscorlib and System.dll and System.Drawing.dll are imported by default.
		/// </summary>
		public List<string> Imports = new List<string>(new string[2] { "System.dll", "System.Drawing.dll" });

		/// <summary>
		/// Imports whose assemblies are packaged into the *.fplot file, so the *.fplot files are portable.
		/// </summary>
		[OptionalField(VersionAdded=312)]
		public PackagedAssemblyCollection PackagedImports = new PackagedAssemblyCollection();

		/// <summary>
		/// A header that is inserted before any compiled source (except for classes that derive from ICompilableWithNoHeader, like
		/// ICompilableLibrary).
		/// </summary>
		[OptionalField(VersionAdded=312)]
		public SourceHeader Header = new SourceHeader();

		/// <summary>
		/// If set to true, when a library is compiled the compiler includes it's namespace into the Compiler.Options.Header automatically.
		/// </summary>
		[OptionalField(VersionAdded=312)]
		public bool AutoUseLibraryNamespace = true;

		/// <summary>
		/// A comparer for import strings. Places import dlls in the order System.*, Microsoft.*, *.*, C:\*.*.
		/// </summary>
		public class ImportComparer: IComparer<string> {
			/// <summary>
			/// A static instance of the comparer.
			/// </summary>
			public static ImportComparer Comparer = new ImportComparer();

			private int Index(string s) {
				if (s.StartsWith("System.", StringComparison.InvariantCultureIgnoreCase)) return 0;
				if (s.StartsWith("Microsoft.", StringComparison.InvariantCultureIgnoreCase)) return 1;
				if (!Path.IsPathRooted(s)) return 2;
				else return 3;
			}
			/// <summary>
			/// Compares two strings.
			/// </summary>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <returns></returns>
			public int Compare(string x, string y) {
				int ix = Index(x), iy = Index(y);
				if (ix != iy) return ix - iy;
				else return string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase);
			}
		}

		/// <summary>
		/// Sorts <see cref="Imports"/> and removes duplicate entries.
		/// </summary>
		/// <param name="Imports">The <see cref="List{T}">List&lt;string&gt;</see> to sort.</param>
		public List<string> SortImports(List<string> Imports) {
			Imports.Sort(ImportComparer.Comparer);
			// remove duplicate items
			for (int i = 0; i < Imports.Count - 1; i++) {
				if (ImportComparer.Comparer.Compare(Imports[i], Imports[i+1]) == 0) Imports.RemoveAt(i--);
			}
			return Imports;
		}

		/// <summary>
		/// Sorts <see cref="Imports"/> and removes duplicate entries.
		/// </summary>
		public List<string> SortImports() { return SortImports(Imports); }

		/// <summary>
		/// Combines two CompilerOptions to the least restrictive level
		/// </summary>
		public void Combine(CompilerOptions Options) {
			if (Options.AllowUnsafe) AllowUnsafe = true;
			if (!Options.AutoUseLibraryNamespace) AutoUseLibraryNamespace = false; 

			int i;
			SortImports();
			foreach (string s in Options.Imports) {
				if ((i = Imports.BinarySearch(s, ImportComparer.Comparer)) < 0) Imports.Insert(~i, s);
			}

			Header.Combine(Options.Header);
			
			foreach (PackagedAssembly a in Options.PackagedImports) {
				PackagedImports.Add(a);
			}
		}

		[OnDeserializing]
		private void OnDeserializing(StreamingContext sc) {
			PackagedImports = new PackagedAssemblyCollection();
			Header = new SourceHeader();
			AutoUseLibraryNamespace = true;
		}
	}

	/// <summary>
	/// This class represents a C# compiler with options.
	/// </summary>
	public static class Compiler {

		private class CompilableItem {
			public WeakReference<ICompilable> w;
			public string src; // The source that was last compiled.
			public string name; // The name of a Library
			public int sourceIndex; // The index for the source file
			public int dllIndex = -1; // The index for the dll file
			public int headerLines = 0;

			public string SourceFile {
				get {
					int pid = Process.GetCurrentProcess().Id;
					return Path.Combine(TempPath, "Source" + pid.ToString("X") + "." + sourceIndex + ".cs");
				}
			}

			public string DllFile { get { return DllName(dllIndex); } }

		}

		private class Lock {
			private const string lockFileName = "lock";
			private string filename;
			public FileStream file;
			bool disposed = false;

			public Lock() {
				Directory.CreateDirectory(TempPath);
				filename = Path.Combine(TempPath, lockFileName);
				AppDomain.CurrentDomain.DomainUnload += DomainUnload;
				AppDomain.CurrentDomain.ProcessExit += DomainUnload;	
				if (!File.Exists(filename)) {
					file = File.Create(filename);
					// ThreadPool.QueueUserWorkItem(new WaitCallback(DeleteTempFiles));
				}
			}

			private void DeleteTempFiles() {
				string[] files = Directory.GetFiles(TempPath, "*.*");
				foreach (string f in files) {
					try {
						if (!File.Exists(filename)) File.Delete(f);
					} catch { }
				}
			}

			private void DeleteTempFiles(object state) { DeleteTempFiles(); }

			public void Dispose() {
				if (!disposed) {
					disposed = true;
					bool isLocked = false;
					if (file != null) file.Close();
					try {
						File.Delete(filename);
					} catch { isLocked = true; }
					if (!isLocked) DeleteTempFiles();
				}
			}

			public void DomainUnload(object sender, EventArgs e) {
				Dispose();
			}

		}

		private const string tempPath = "JohnsHope's FPlot temporary Files";
		private static int DllIndex = 0;
		private static int SourceIndex = 0;
		// A list of all ICompilable's the Compiler knows. If a ICompilableLibrary is compiled,
		// all items in this list will be recompiled.
		private static List<CompilableItem> Items = new List<CompilableItem>();
		/// <summary>
		/// The <see cref="CompilerOptions">compiler options</see>.
		/// </summary>
		public static CompilerOptions Options = new CompilerOptions();
		/// <summary>
		/// The path where the source files and compiled items dll's are stored.
		/// </summary>
		public static string TempPath = Path.Combine(System.IO.Path.GetTempPath(), tempPath);
		private static Lock FileLock = new Lock();
		private static string LibraryDLL = typeof(DefaultClass).Module.FullyQualifiedName;

		private static string DllName(int dllIndex) {
			if (dllIndex == -1) return null;
			int pid = Process.GetCurrentProcess().Id;
			return Path.Combine(TempPath, "Code" + pid.ToString("X") + "." + dllIndex + ".dll");
		}

		/// <summary>
		/// Returns the name of the temporary source file used to compile the ICompilable c.
		/// </summary>
		public static string SourceFile(ICompilable c) {
			if (c != null) {
				CompilableItem item = Item(c);
				if (item != null) return item.SourceFile;
			}
			return "";
		}

		/// <summary>
		/// Returns an <see cref="ICompilable"/> object known to the compiler from the given source file name. This can be used to
		/// resolve source files in stack-trace info. 
		/// </summary>
		/// <param name="sourceFile">The filename as returned by <see cref="Compiler.SourceFile(ICompilable)"/>.</param>
		public static ICompilable ICompilable(string sourceFile) {
			CompilableItem item = Item(sourceFile);
			if (item != null) return item.w.Target;
			else return null;
		}

		/// <summary>
		/// Get's the <see cref="ICompilable"/> object known to the compiler from the given source file name, and gets the number of
		/// header lines that where added in front of the source. This can be used to resolve source files in stack-trace info. 
		/// </summary>
		/// <param name="sourceFile">The filename as returned by <see cref="Compiler.SourceFile(ICompilable)"/>.</param>
		/// <param name="c">The ICompilable that corresponds to the passed source file.</param>
		/// <param name="headerLines">The number of source header lines that where inserted in front of the compiled source.</param>
		public static void ICompilableInfo(string sourceFile, out ICompilable c, out int headerLines) {
			c = null;
			CompilableItem item = Item(sourceFile);
			if (item != null)	c = item.w.Target;
			if (c != null) headerLines = item.headerLines;
			else headerLines = 0;
		}

		private static void ClearItems() {
			#if DEBUG
				int dllIndex = -1;
			#endif
			for (int i = 0; i < Items.Count; i++) {
				if (!Items[i].w.IsAlive || Items[i].w.Target == null) {
					Items.RemoveAt(i);
					i--;
				}
				#if DEBUG // check that all libraries have the same dllIndex.
					else {
						if (Items[i].w.Target is ICompilableLibrary) {
							if (dllIndex == -1) dllIndex = Items[i].dllIndex;
							else if (Items[i].dllIndex != dllIndex) throw new
								ApplicationException("Compiler.ClearItems: Inconsistent dllIndex in library items.");
						}
					}
				#endif
			} 
		}

		private static string[] GetImports(bool library) {
			List<string> list = new List<string>();
			list.Add(LibraryDLL); //import JohnsHope.FPlot.Library.dll
			list.AddRange(Options.Imports);
			ClearItems();
			if (!library) { //add libraries 
				foreach (CompilableItem x in Items) {
					ICompilable item = x.w.Target;
					if (item != null && item is ICompilableLibrary && x.dllIndex != -1) {
						string dll = x.DllFile;
						if (dll != null && !list.Contains(dll)) list.Add(dll);
						break;
					}
				}
			}
			return list.ToArray();
		}
		// Returns the CompilableItem known to the Compiler for a ICompilable. If obj is a outdated Library, the newer Library
		// is returned, otherwise null is returned.
		private static CompilableItem Item(ICompilable obj) {
			if (obj is ICompilableLibrary) {
				ICompilableLibrary lib = (ICompilableLibrary)obj;
				string name = lib.GetName();
				DateTime version = lib.Version;
				foreach (CompilableItem x in Items) {
					ICompilable y = x.w.Target;
					if (y == obj) return x;
					if (y is ICompilableLibrary) {
						ICompilableLibrary yl = (ICompilableLibrary)y;
						if (name == yl.GetName() && version <= yl.Version) return x;
					}
				}
			} else {
				foreach (CompilableItem x in Items) {
					ICompilable item = x.w.Target;
					if (item != null && obj == item) return x;

				}
			}
			return null;
		}

		private static CompilableItem Item(string sourceFile) {
			foreach (CompilableItem item in Items) {
				if (string.Compare(sourceFile, item.SourceFile, true) == 0) return item;
			}
			return null;
		}

		/// <summary>
		/// Returns true if obj is in the Items list.
		/// </summary>
		public static bool Knows(ICompilable obj) {
			return (Item(obj) != null);
		}

		private static CompilableItem AddItem(ICompilable obj) {
			CompilableItem x = Item(obj);
			if (x == null) {
				if (obj is ICompilableLibrary) {
					ICompilableLibrary lib = (ICompilableLibrary)obj;
					string name = lib.GetName();
					DateTime mod = lib.Version;
					for (int i = 0; i < Items.Count; i++) {
						ICompilable y = Items[i].w.Target;
						if (y is ICompilableLibrary) {
							ICompilableLibrary yl = (ICompilableLibrary)y;
							if (name == yl.GetName()) {
								if (mod > yl.Version) { // Replace library with newer one.
									x = new CompilableItem();
									x.w = new WeakReference<ICompilable>(obj);
									x.src = null; // source won't be set until compilation.
									x.name = name;
									x.sourceIndex = SourceIndex++;
									Items[i] = x;
									return x;
								} else return Items[i]; // Don't add an outdated Library.
							}
						}
					}
				}
				x = new CompilableItem();
				x.w = new WeakReference<ICompilable>(obj);
				x.src = null; // source won't be set until compilation.
				x.sourceIndex = SourceIndex++;
				Items.Add(x);
				return x;
			} else return x;
		}
		/// <summary>
		/// Adds obj to the list of <see cref="ICompilable"/>'s that will be recompiled if a <see cref="ICompilableLibrary"/> is compiled.
		/// </summary>
		public static void Add(ICompilable obj) { AddItem(obj); }

		/// <summary>
		/// Compiles an <see cref="ICompilable"/>.
		/// </summary>
		/// <param name="obj">The item to compile.</param>
		/// <param name="recompile">If true the item will be compiled even if the source remained the same</param>
		public static bool Compile(ICompilable obj, bool recompile) {
			lock (FileLock) {
				ClearItems();
				DllIndex++;

				if (obj != null) {

					obj.TypeIndex = DllIndex;

					ICompilableClass c = null;
					if (obj is ICompilableClass) c = (ICompilableClass)obj;

					string dll = DllName(DllIndex);
					string source = obj.GetSource();

					if (source != null) {

						int headerLines;
						string headerText;
						Options.Header.GetSnapshot(out headerText, out headerLines);

						if (!(obj is ICompilableWithNoHeader)) source = headerText + "\n" + source;

						CompilableItem x = Item(obj);

						if (recompile || x == null || source != x.src) {

							string cstr = "/target:library /fullpaths";
							CompilerParameters par = new CompilerParameters(GetImports(obj is ICompilableLibrary)); // get imports before Add 

							x = AddItem(obj);

							par.OutputAssembly = dll;
							par.GenerateInMemory = false;
							par.GenerateExecutable = true;
							if (Options.Optimize) cstr += " /optimize+";
							else cstr += " /optimize-";
							if (Options.CheckOverflow) cstr += " /checked+";
							else cstr += " /checked-";
							par.IncludeDebugInformation = Options.Debug;
							if (Options.AllowUnsafe) cstr += " /unsafe+";
							else cstr += " /unsafe-";
							par.WarningLevel = Options.WarningLevel;
							par.CompilerOptions = cstr;

							List<ICompilable> l = new List<ICompilable>();
							if (!(obj is ICompilableLibrary)) l.Add(obj); // add the single object to the compiled sources.
							else { // add all libraries to the compiled sources
								foreach (CompilableItem y in Items) { // collect all sources from all Libraries
									ICompilable item = y.w.Target;
									if (item != null && item is ICompilableLibrary) l.Add(item);
								}
							}

							List<string> files = new List<string>(); // Write all sources into files
							foreach (ICompilable d in l) {
								CompilableItem y = AddItem(d);
								string src = d.GetSource();
								if (!(d is ICompilableWithNoHeader)) {
									src = headerText + "\n" + src;
									y.headerLines = headerLines;
								}
								y.src = src;
								string filename = SourceFile(d);
								files.Add(filename);
								File.WriteAllText(filename, src);
							}

							CodeDomProvider p = new CSharpCodeProvider();
							CompilerResults res = p.CompileAssemblyFromFile(par, files.ToArray());

							foreach (CompilerError err in res.Errors) { // adjust error lines
								ICompilable ic;
								CompilableItem item = Item(err.FileName);
								if (item != null) ic = item.w.Target;
								else ic = null;
								if (ic != null) err.Line -= item.headerLines + ic.FirstSourceLine;
							}
							obj.CompilerResults = res;

							Assembly a = null;
							if (res.NativeCompilerReturnValue == 0) {
								try {
									a = Assembly.LoadFrom(dll);
								} catch {
									res.NativeCompilerReturnValue = -1;
									res.Errors.Add(new CompilerError(files[0], 0, 0, "0", "Error loading assembly"));
								}
							}

							// delete source files: omit deletion of files to speed up compilation. The files are deleted upon application exit anyway.
							// foreach (string file in files) {
							//	 File.Delete(file);
							// }

							if (res.NativeCompilerReturnValue == 0) {
								if (obj is IModifyable) ((IModifyable)obj).Modified = true;
								res.CompiledAssembly = a;

								if (a != null) { // succesfully compiled & loaded
									if (x != null) { // set source & dllIndex
										x.src = source;
										x.dllIndex = DllIndex;
									}
									if (obj is ICompilableLibrary) { // item is a library, set all libraries dllIndex and recompile all other items.

										if (Options.AutoUseLibraryNamespace) Options.Header.AddUsing(Parser.Namespace(source));

										CompilableItem[] items = Items.ToArray();

										foreach (CompilableItem y in items) { // set all libraries dll index
											ICompilable item = y.w.Target as ICompilable;
											if (item != null && item is ICompilableLibrary) x.dllIndex = DllIndex;
										}

										foreach (CompilableItem y in items) { // recompile all non-library items.
											ICompilable item = y.w.Target as ICompilable;
											if (item != null && !(item is ICompilableLibrary)) Compile(item, true);
										}
									}
								}
								if (c != null) { // item is a compilable class, create a concrete object of that class.
									object instance = null;
									if (a != null) {
										Type t = a.GetType(c.ClassName, false, false);
										if (t != null) {
											instance = t.GetConstructor(Type.EmptyTypes).Invoke(null);
										}
									}
									c.ClassInstance = instance;
									if (instance == null) obj.CompilerResults.Errors.Add(new CompilerError(files[0], 0, 0, "0", "Error creating class instance"));
									return instance != null;
								}
							} else { // there were compilation errors
								if (x != null) x.src = null;
								if (c != null) c.ClassInstance = null;
							}
							return res.NativeCompilerReturnValue == 0;
						} else { // item is already up to date, no need to recompile it.
							return true;
						}
					} else { // source is null.
						if (c != null) c.ClassInstance = null;
						return true;
					}
				} else return false;
			}
		}
		/// <summary>
		/// Compiles an <see cref="ICompilable"/>.
		/// </summary>
		public static bool Compile(ICompilable obj) {
			return Compile(obj, false);
		}
		/// <summary>
		/// Describes a spot in a compiled source, for example where an exception occured.
		/// </summary>
		public class SourceLocation {
			/// <summary>
			/// The filename of the compiled source file where the exception occured.
			/// </summary>
			public string File;
			/// <summary>
			/// The line in the sourcecode that caused the exception.
			/// </summary>
			public int Line;
			/// <summary>
			/// The column in the sourcecode that caused the exception.
			/// </summary>
			public int Column = 0;
			/// <summary>
			/// The Method that caused the exception.
			/// </summary>
			public string Method;
			/// <summary>
			/// The <see cref="ICompilable"/>, where the exception occured.
			/// </summary>
			public ICompilable Source;
			/// <summary>
			/// The Exception that occured.
			/// </summary>
			public Exception Exception;

			/// <summary>
			/// Creates a copy of the SourceLocation.
			/// </summary>
			/// <returns></returns>
			public SourceLocation Clone() {
				SourceLocation s = new SourceLocation();
				s.File = File;
				s.Line = Line;
				s.Column = Column;
				s.Method = Method;
				s.Source = Source;
				s.Exception = Exception;
				return s;
			}
		}
		/// <summary>
		/// Describes the stack trace of an exception. This type is a list of <see cref="SourceLocation"/>'s, which in turn describe
		/// a spot in a <see cref="ICompilable"/>'s source.
		/// </summary>
		public class StackTrace: List<SourceLocation> {
			// <summary>
			// Creates a new StackTrace. You must pass a stack trace string that you get from an Exception object.
			// </summary>
			// <param name="StackTraceString"></param>	
			private StackTrace(string StackTraceString) {
				string[] lines = StackTraceString.Split('\n');
				int headerLines = 0;

				for (int i = 0; i < lines.Length; i++) {
					int j = 0, k = 0;
					bool path = false;
					string str = lines[i];
					SourceLocation s = new SourceLocation();
					// Parse method
					while (j < str.Length-1 && str[j] != '(') j++;
					if (str[j] == '(') {	// string contains a method
						while (j >= 0 && !char.IsWhiteSpace(str[j])) j--;
						if (j < str.Length-2) j++;
						k = j;
						while (k < str.Length && str[k] != ')') k++;
						if (k < str.Length-1) k++;
						s.Method = str.Substring(j, k - j);
					}
					// Parse filename
					j = k;
					while (j < str.Length-1 && str[j] != ':') j++;
					if (str[j] == ':') { // string conatins a path
						path = true;
						while (j > k && !char.IsWhiteSpace(str[j])) j--; // go to the beginning of the path
						if (j < str.Length-2) j++;
						k = str.Length-1;
						while (k > j && str[k] != ':') k--;
						s.File = str.Substring(j, k-j);
						if (j < k) Compiler.ICompilableInfo(s.File, out s.Source, out headerLines);
						else s.Source = null;
					}
					// Parse linenumber
					if (path) {
						j = str.Length-1;
						while (j >= 0 && !char.IsDigit(str[j])) j--;
						k = j+1;
						while (j >= 0 && char.IsDigit(str[j])) j--;
						j++;
						if (j < k) {
							s.Line = int.Parse(str.Substring(j, k-j));
						}
						// decrement linenumber by the position of the first line for known sources
						if (s.Line > 0 && s.Source != null) s.Line -= s.Source.FirstSourceLine + headerLines;
					}
					this.Add(s);
				}
			}
			/// <summary>
			/// Creates a new StackTrace from the <see cref="System.Exception">Exception</see> e.
			/// </summary>
			public StackTrace(Exception e): this(e.StackTrace) {
				foreach (SourceLocation sl in this) sl.Exception = e;
			}
		}
	}
}