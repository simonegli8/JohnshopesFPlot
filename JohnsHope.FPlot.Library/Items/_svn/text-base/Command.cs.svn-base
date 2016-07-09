using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// A class that represents a C# statement that can be executed.
	/// </summary>
	public class Command: ICompilable, ICompilableClass {
		/// <summary>
		/// The base class of a compiled instance. Used internally.
		/// </summary>
		public class Instance: DefaultClass {
			/// <summary>
			/// Runs the C# statement.
			/// </summary>
			public virtual void Run() { }
		}
		private Instance instance = new Instance();
		private CompilerResults res;
		/// <summary>
		/// If true, the command was compiled successfully.
		/// </summary>
		public bool Compiled = false;
		/// <summary>
		/// The Exception that occured during execution of the command or null if there was no exception.
		/// </summary>
		public Exception Exception = null;
		/// <summary>
		/// The results from the compilation of the item.
		/// </summary>
		public CompilerResults CompilerResults {
			get { return res; }
			set {
				res = value;
				Compiled = !res.Errors.HasErrors;
			}
		}
		private int typeindex = 0;
		/// <summary>
		/// The <see cref="ICompilable.TypeIndex">TypeIndex</see> used by the compiler. Used internally by the <see cref="Compiler"/>.
		/// </summary>
		public virtual int TypeIndex {
			get { return typeindex; }
			set { typeindex = value; }
		}
		/// <summary>
		/// Returns the linenumber of the first source line. Always returns 1.
		/// </summary>
		public int FirstSourceLine {
			get { return 1; }
		}
		/// <summary>
		/// The source of the command
		/// </summary>
		public string Source = null;
		/// <summary>
		/// Returns the source of the command. Used internally by the <see cref="Compiler"/>.
		/// </summary>
		public string GetSource() {
			if (Source == null) return null;
			return "namespace JohnsHope.FPlot.Library.Code{public class Item" + TypeIndex +
				":JohnsHope.FPlot.Library.Command.Instance{public override void Run(){\n" +	Source + "}}}";
		}

		/// <summary>
		/// Sets the class instance. Used internally by the <see cref="Compiler"/>.
		/// </summary>
		public object ClassInstance {
			set {
				if (value != null) instance = (Instance)value;
				else instance = new Instance();
			}
		}
		/// <summary>
		/// Gets the class name for the compiler. Used internally by the <see cref="Compiler"/>.
		/// </summary>
		public string ClassName {
			get { return "JohnsHope.FPlot.Library.Code.Item" + TypeIndex; }
		}
		/// <summary>
		/// Runs the command after it was successfully compiled.
		/// </summary>
		public void Run() {
			Exception = null;
			try {
				instance.Run();
			} catch (Exception e) {
				Exception = e;
			}
		}
		/// <summary>
		/// The default constructor.
		/// </summary>
		public Command() { }
		/// <summary>
		/// This constructor automatically compiles and runs the code passed in source. If the code could not be compiled, 
		/// <see cref="Compiled"/> will be false. If there was an <see cref="System.Exception">Exception</see> during execution
		/// of the code, <see cref="Exception"/> will contain the Exception,
		/// otherwise it will be null.
		/// </summary>
		public Command(string source) {
			Source = source;
			Compiler.Compile(this);
			if (Compiled) Run();
		}
		/// <summary>
		/// Copies from another Command.
		/// </summary>
		public void CopyFrom(Command cmd) {
			instance = cmd.instance;
			res = cmd.res;
			Compiled = cmd.Compiled;
			Exception = cmd.Exception;
			typeindex = cmd.typeindex;
			Source = cmd.Source;
		}
		/// <summary>
		/// Creates a copy of the command.
		/// </summary>
		public Command Clone() {
			Command cmd = new Command();
			cmd.CopyFrom(this);
			return cmd;
		}
		/// <summary>
		/// Returns the Source of the command. 
		/// </summary>
		public override string ToString() {
			return Source;
		}
	}
}
