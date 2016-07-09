using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.CodeDom.Compiler;

namespace JohnsHope.FPlot.Library {

	/// <summary>
	/// Describes an obejct that has a modified state.
	/// </summary>
	public interface IModifyable {
		/// <summary>
		/// Indicates if the object has been modified.
		/// </summary>
		bool Modified { get; set; }
	}

	/// <summary>
	/// A default class with predefined function shortcuts. All <see cref="Item">Items</see> derive from DefaultClass, so it's methods
	/// can be called from within the it's source. 
	/// </summary>
	[Serializable]
	public partial class DefaultClass {
		/// <summary>
		/// A shortcut for Math.Abs.
		/// </summary>
		public static double abs(double x) { return Math.Abs(x); }
		/// <summary>
		/// A shortcut for Math.Abs.
		/// </summary>
		public static int abs(int x) { return Math.Abs(x); }
		/// <summary>
		/// A shortcut for Math.Acos.
		/// </summary>
		public static double acos(double x) { return Math.Acos(x); }
		/// <summary>
		/// A shortcut for Math.Asin.
		/// </summary>
		public static double asin(double x) { return Math.Asin(x); }
		/// <summary>
		/// A shortcut for Math.Atan.
		/// </summary>
		public static double atan(double x) { return Math.Atan(x); }
		/// <summary>
		/// A shortcut for Math.Ceiling.
		/// </summary>
		public static double ceiling(double x) { return Math.Ceiling(x); }
		/// <summary>
		/// A shortcut for Math.Cos.
		/// </summary>
		public static double cos(double x) { return Math.Cos(x); }
		/// <summary>
		/// A shortcut for Math.Cosh.
		/// </summary>
		public static double cosh(double x) { return Math.Cosh(x); }
		/// <summary>
		/// A shortcut for Math.Exp.
		/// </summary>
		public static double exp(double x) { return Math.Exp(x); }
		/// <summary>
		/// A shortcut for Math.Floor.
		/// </summary>
		public static double floor(double x) { return Math.Floor(x); }
		/// <summary>
		/// A shortcut for Math.Log.
		/// </summary>
		public static double log(double x) { return Math.Log(x); }
		/// <summary>
		/// A shortcut for Math.Log.
		/// </summary>
		public static double ln(double x) { return Math.Log(x); }
		/// <summary>
		/// A shortcut for Math.Log10.
		/// </summary>
		public static double log10(double x) { return Math.Log10(x); }
		/// <summary>
		/// A shortcut for Math.Max.
		/// </summary>
		public static double max(double x, double y) { return Math.Max(x, y); }
		/// <summary>
		/// A shortcut for Math.Min.
		/// </summary>
		public static double min(double x, double y) { return Math.Min(x, y); }
		/// <summary>
		/// A shortcut for Math.Max.
		/// </summary>
		public static int max(int x, int y) { return Math.Max(x, y); }
		/// <summary>
		/// A shortcut for Math.Min.
		/// </summary>
		public static int min(int x, int y) { return Math.Min(x, y); }
		/// <summary>
		/// A shortcut for Math.Pow.
		/// </summary>
		public static double pow(double x, double y) { return Math.Pow(x, y); }
		/// <summary>
		/// A shortcut for Math.Sin.
		/// </summary>
		public static double sin(double x) { return Math.Sin(x); }
		/// <summary>
		/// A shortcut for Math.Sinh.
		/// </summary>
		public static double sinh(double x) { return Math.Sinh(x); }
		/// <summary>
		/// A shortcut for Math.Sqrt.
		/// </summary>
		public static double sqrt(double x) { return Math.Sqrt(x); }
		/// <summary>
		/// A shortcut for Math.Tan.
		/// </summary>
		public static double tan(double x) { return Math.Tan(x); }
		/// <summary>
		/// A shortcut for Math.Tanh.
		/// </summary>
		public static double tanh(double x) { return Math.Tanh(x); }
		/// <summary>
		/// A shortcut for Math.PI.
		/// </summary>
		public const double pi = Math.PI;
		/// <summary>
		/// A shortcut for Math.E.
		/// </summary>
		public const double e = Math.E;
	}

	/// <summary>
	/// Base class of all plottable items.
	/// </summary>
	[Serializable]
	public class Item: DefaultClass, ICompilable, IModifyable {
		
		private bool modified = false;

		/// <summary>
		/// The <see cref="EventArgs"/> of the <see cref="AddedToList"/> and <see cref="RemovedFromList"/> event handlers.
		/// </summary>
		public class ListEventArgs: EventArgs {
		
			/// <summary>
			/// The <see cref="ItemList"/> the <see cref="Item"/> was added to or removed from.
			/// </summary>
			public ItemList List;

			/// <summary>
			/// The default constructor.
			/// </summary>
			public ListEventArgs() {}

			/// <summary>
			/// A constructor that initializes the List field.
			/// </summary>
			public ListEventArgs(ItemList List) { this.List = List; }  
		
		}

		/// <summary>
		/// Indicates if the item has been modified and not yet painted.
		/// </summary>
		public virtual bool Modified {
			get { return modified; }
			set { modified = value;	}
		}

		/// <summary>
		/// The name of the item.
		/// </summary>
		public string Name = "";

		[NonSerialized]
		private CompilerResults res;

		/// <summary>
		/// If true compilation was successfull.
		/// </summary>
		public bool Compiled {
			get { return res != null && res.Errors != null && !res.Errors.HasErrors; }
		}

		/// <summary>
		/// The results from the compilation of the item.
		/// </summary>
		public CompilerResults CompilerResults { get { return res; } set {	res = value; } }

		/// <summary>
		/// Returns the first line for Items. Always returns 0.
		/// </summary>
		public virtual int FirstSourceLine {
			get { return 0; }
		}

		/// <summary>
		/// Returns the source of the item for the compiler.
		/// </summary>
		/// <returns></returns>
		public virtual string GetSource() { return null; }

		/// <summary>
		/// Initializes an <c>Item</c>.
		/// </summary>
		public Item() { }

		private int typeindex = 0;

		/// <summary>
		/// The TypeIndex used by the compiler.
		/// </summary>
		public virtual int TypeIndex {
			get { return typeindex; }
			set { typeindex = value; }
		}

		/// <summary>
		/// Gets a string array with the errors of the compilation.
		/// </summary>
		public CompilerErrorCollection Errors {
			get {
				if (CompilerResults != null) return CompilerResults.Errors;
				else return null;
			}
		}

		/// <summary>
		/// Copies from another Item.
		/// </summary>
		public virtual void CopyFrom(Item src) {
			modified = src.modified;
			Name = src.Name;
			typeindex = src.typeindex;
			res = src.res;
		}

		/// <summary>
		/// Creates a copy of the Item.
		/// </summary>
		public virtual Item Clone() {
			Item f = new Item();
			f.CopyFrom(this);
			return f;
		}
	
		/// <summary>
		/// Gets a painter for the item
		/// </summary>
		public virtual Painter Painter(PlotModel model) { return null; }

		/// <summary>
		/// Indicates if the item can be painted to a plot of the specified type.
		/// </summary>
		public bool CanPaintTo(PlotModel model) { return Painter(model) != null; }

		/// <summary>
		/// Returns a user friendly name for the Item-Type.
		/// </summary>
		public virtual string TypeName() {
			return this.GetType().Name;
		}

		/// <summary>
		/// Returns Name
		/// </summary>
		public override string ToString() {
			return Name;
		}

		/// <summary>
		/// Compiles the Item.
		/// </summary>
		/// <returns>Returns true if compilation was successfull</returns>
		public virtual bool Compile() {
			Compiler.Compile(this);
			return Compiled;
		}

		/// <summary>
		/// An event that is raised when the Item is added to an <see cref="ItemList"/>.
		/// </summary>
		public event EventHandler<ListEventArgs> AddedToList;

		/// <summary>
		/// An event that is raised when the Item is removed from an <see cref="ItemList"/>.
		/// </summary>
		public event EventHandler<ListEventArgs> RemovedFromList;

		/// <summary>
		/// Raises the AddedToList event.
		/// </summary>
		/// <param name="List">The <see cref="ItemList"/> the Item was added to.</param>
		protected virtual void OnAddedToList(ItemList List) {
			if (AddedToList != null) AddedToList(this, new ListEventArgs(List));
		}

		/// <summary>
		/// Raises the RemovedFromList event.
		/// </summary>
		/// <param name="List">The <see cref="ItemList"/> the Item was removed from to.</param>
		protected virtual void OnRemovedFromList(ItemList List) {
			if (RemovedFromList != null) RemovedFromList(this, new ListEventArgs(List));
		}

		/// <summary>
		/// Raises the AddedToList event.
		/// </summary>
		/// <param name="List">The <see cref="ItemList"/> the Item was added to.</param>
		public virtual void RaiseAddedToList(ItemList List) { OnAddedToList(List); }

		/// <summary>
		/// Raises the RemovedFromList event.
		/// </summary>
		/// <param name="List">The <see cref="ItemList"/> the Item was removed from to.</param>
		public virtual void RaiseRemovedFromList(ItemList List) { OnRemovedFromList(List); }

	}


}
