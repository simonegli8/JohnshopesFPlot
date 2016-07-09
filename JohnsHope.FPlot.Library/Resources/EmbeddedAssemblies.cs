using System;
using System.IO;
using System.Runtime.Serialization;

namespace JohnsHope.FPlot.Library {

	/// <summary>
	/// A <see cref="Item"/> class that represents an assembly that will be referenced by the <see cref="Compiler"/> and will be embedded into the *.fplot file so the file is portable even if the assembly is
	/// missing on the computer where the file is opened.
	/// </summary>
	[Serializable]
	public class EmbeddedAssembly: Item {

		[NonSerialized]
		private int IncludedInLists = 0;

		private CompilerOptions.PackagedAssembly assembly;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public EmbeddedAssembly() { }

		/// <summary>
		/// A constructor that takes an assembly filename as an argument
		/// </summary>
		/// <param name="dll">The full filename of the assembly.</param>
		/// <exception cref="ArgumentException">Throws an Exception if the passed assembly filename is not valid or if the assembly file does not exist.</exception>
		public EmbeddedAssembly(string dll) {
			if (CompilerOptions.PackagedAssembly.ValidAssembly(dll)) {
				Assembly = dll;
			} else throw new ArgumentException("EmbeddedAssembly: dll is not a valid assembly filename.");
		}

		private void RemoveFromCompilerImports() {
			if (assembly != null && Compiler.Options.PackagedImports.Contains(assembly)) Compiler.Options.PackagedImports.Remove(assembly);
		}

		private void AddToCompilerImports() {
			if (assembly != null && !Compiler.Options.PackagedImports.Contains(assembly)) Compiler.Options.PackagedImports.Add(assembly);
		}

		/// <summary>
		/// Adds the assembly to the <see cref="CompilerOptions.PackagedImports"/> collection if it is not yet contaied in an <see cref="ItemList"/>.
		/// </summary>
		/// <param name="List">The <see cref="ItemList"/> the <see cref="Item"/> was added to.</param>
		protected override void OnAddedToList(ItemList List) {
			if (IncludedInLists++ == 0) AddToCompilerImports();
			base.OnAddedToList(List);
		}

		/// <summary>
		/// Removes the assembly from the <see cref="CompilerOptions.PackagedImports"/> collection if it is not contained in any <see cref="ItemList"/> anymore.
 		/// </summary>
		/// <param name="List">The <see cref="ItemList"/> the <see cref="Item"/> was removed from.</param>
		protected override void OnRemovedFromList(ItemList List) {
			if (--IncludedInLists == 0) RemoveFromCompilerImports();
			base.OnRemovedFromList(List);
		} 

		/// <summary>
		/// Gets or sets the full path to the embedded assembly.
		/// </summary>
		/// <exception cref="ArgumentException">Throws an Exception if the passed string is not a valid filename or if the assembly file does not exist.</exception>
		public string Assembly {
			get { return assembly.FileName; }
			set {
				if (assembly == null || assembly.OriginalFileName != value) {
					if (IncludedInLists > 0) RemoveFromCompilerImports();
					if (!string.IsNullOrEmpty(value)) {
						if (CompilerOptions.PackagedAssembly.ValidAssembly(value)) {
							assembly = new CompilerOptions.PackagedAssembly(value);
						} else throw new ArgumentException("EmbeddedAssembly.Assembly: not a valid assembly filename or assembly file does not exist.");
					} else assembly = null;
				}
				if (IncludedInLists > 0) AddToCompilerImports();
				Name = Path.GetFileName(value);
			}
		}

		/// <summary>
		/// Always returns null, since this <see cref="Item"/> type needs not to be compiled.
		/// </summary>
		/// <returns></returns>
		public override string GetSource() { return null; }

		/// <summary>
		/// Returns a short name for this <see cref="Item"/> type.
		/// </summary>
		public override string TypeName() { return "Embedded assembly"; } 

		[OnDeserialized]
		private void Deserialized(StreamingContext sc) {
			IncludedInLists = 0;
		}


	}


}