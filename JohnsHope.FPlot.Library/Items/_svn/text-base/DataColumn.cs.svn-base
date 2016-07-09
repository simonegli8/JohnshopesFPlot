using System;
using System.Runtime.Serialization;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace JohnsHope.FPlot.Library {

	/// <summary>
	/// Represents a column of data in a matrix with x, y, z, dx, dy and dz vectors.
	/// The values of the data array can either be drawn from a <see cref="BigData">BigData</see> array or from a formula
	/// specified in the property <see cref="DataColumn.Source">Source</see>.
	/// </summary>
	[Serializable]
	public class DataColumn: DataColumnSerializer, ICompilableClass {
		/// <summary>
		/// A <see cref="DataColumn"/> compiled instance object
		/// </summary>
		public class Instance: Item {
			/// <summary>
			/// The <see cref="DataItem"/> this instance belongs to.
			/// </summary>
			public DataItem Parent;
			/// <summary>
			/// The Length of the data.
			/// </summary>
			public double Length {
				get { return Parent.Length; }
				set { Parent.Length = (int)value; }
			}
			/// <summary>
			/// The x array.
			/// </summary>
			public Data x {
				get {
					if (Parent != null) return Parent.x;
					else return null;
				}
				set {
					if (Parent != null) {
						if (value is DataColumn) Parent.x = (DataColumn)value;
						else Parent.x = new DataColumn(value);
					}
				}
			}

			/// <summary>
			/// The y array.
			/// </summary>
			public Data y {
				get {
					if (Parent != null) return Parent.y;
					else return null;
				}
				set {
					if (Parent != null) {
						if (value is DataColumn) Parent.y = (DataColumn)value;
						else Parent.y = new DataColumn(value);
					}
				}
			}
			/// <summary>
			/// The z array.
			/// </summary>
			public Data z {
				get {
					if (Parent != null) return Parent.z;
					else return null;
				}
				set {
					if (Parent != null) {
						if (value is DataColumn) Parent.z = (DataColumn)value;
						else Parent.z = new DataColumn(value);
					}
				}
			}
			/// <summary>
			/// The dx array.
			/// </summary>
			public Data dx {
				get {
					if (Parent != null) return Parent.dx;
					else return null;
				}
				set {
					if (Parent != null) {
						if (value is DataColumn) Parent.dx = (DataColumn)value;
						else Parent.dx = new DataColumn(value);
					}
				}
			}
			/// <summary>
			/// The dy array.
			/// </summary>
			public Data dy {
				get {
					if (Parent != null) return Parent.dy;
					else return null;
				}
				set {
					if (Parent != null) {
						if (value is DataColumn) Parent.dy = (DataColumn)value;
						else Parent.dy = new DataColumn(value);
					}
				}
			}
			/// <summary>
			/// The dz array.
			/// </summary>
			public Data dz {
				get {
					if (Parent != null) return Parent.dz;
					else return null;
				}
				set {
					if (Parent != null) {
						if (value is DataColumn) Parent.dz = (DataColumn)value;
						else Parent.dz = new DataColumn(value);
					}
				}
			}
			/// <summary>
			/// Is obsolete. Does nothing.
			/// </summary>
			[Obsolete("Does nothing")]
			public void Reset() { }
			/// <summary>
			/// The indexer.
			/// </summary>
			public virtual double this[int n] {
				get { return double.NaN; }
			}
			/// <summary>
			/// The indexer with a double argumnent
			/// </summary>
			public double this[double n] {
				get { return this[(int)n]; }
			}
			/// <summary>
			/// Copies from another instance.
			/// </summary>
			public virtual void CopyFrom(Instance src) {
				Parent = src.Parent;
			}
			/// <summary>
			/// Creates a deep copy.
			/// </summary>
			/// <returns></returns>
			public override Item Clone() {
				Instance obj = new Instance();
				obj.CopyFrom(this);
				return obj;
			}
		}
	
		/// <summary>
		/// The parent <see cref="DataItem"/>.
		/// </summary>
		public new DataItem Parent {
			get { return base.Parent; }
			set {
				base.Parent = value;
				if (instance != null) {
					instance.Parent = value;
				}
			}
		}

		[NonSerialized]
		private Instance instance = new Instance();
		[NonSerialized]
		private int typeindex = 0;
		[NonSerialized]
		CompilerResults compilerresults;

		[NonSerialized]
		private Type _Type = typeof(double);

		/// <summary>
		/// The base type of the DataColumn.
		/// </summary>
		public new Type Type { get { return base.Type; } set { base.Type = value; } }

		/// <summary>
		/// The default constructor.
		/// </summary>
		public DataColumn(): base() {}
		/// <summary>
		/// A constructor setting the parent <see cref="DataItem">DataItem</see>.
		/// </summary>
		[Obsolete("Use DataColumn() instead.")]
		public DataColumn(DataItem Parent) { this.Parent = Parent; }

		/// <summary>
		/// A constructor setting the parent <see cref="DataItem">DataItem</see>.
		/// </summary>
		public DataColumn(IEnumerable<double> data): base(data) { }

		[OnDeserialized]
		private void Deserialized(StreamingContext c) {
			if (source != null) Compiler.Compile(this);
		}

		/// <summary>
		/// Gets or sets the formula used for this DataColumn. The formula must be a regular C# expression that can refer to the variables
		/// <c>n</c>, <c>Length</c>, <c>x[n]</c>, <c>y[n]</c>, <c>z[n]</c>, <c>dx[n]</c>, <c>dy[n]</c> and <c>dz[n]</c>. 
		/// </summary>
		public virtual string Source {
			get{ return source; }
			set{
				if (value != null && value.Trim() != "") {
					base.Length = 0;
					source = value;
				} else {
					if (Parent != null) base.Length = Parent.Length;
					source = null;
				}
			}
		}
		/// <summary>
		/// Gets or sets the length of this DataColumn.
		/// </summary>
		public override int Length {
			get {return Parent.Length;}
			set {
				if (source == null) base.Length = value;
				if (Parent.Length != value) { Parent.Length = value; Modified = true; }
			}
		}

		/// <summary>
		/// The indexer of the DataColumn.
		/// </summary>
		public override double this[int i] {
			get {
				if (source == null) return base[i];
				else return instance[i];
			}
			set {
				if (source == null) {
					if ((autoResize) && (i >= length)) Length = i+1;
					base[i] = value;
				} else throw new InvalidOperationException("data cannot be changed.");
			}
		}

		/// <summary>
		/// A value indicating if the DataColumn can store values, or if it is readonly.
		/// </summary>
		public virtual bool CanWrite { get { return source == null; } }
		
		/// <summary>
		/// Copies from another DataColumn with either a deep or shallow copy, depending on the value of
		/// the <see cref="Data.deepCopy">deepCopy</see> field. 
		/// </summary>
		public void CopyFrom(DataColumn src) {
			base.CopyFrom(src);
			if (src.source != null) source = (string)src.source.Clone();
			else source = null;
			instance = (Instance)src.instance.Clone();
			Type = src.Type;
			Parent = src.Parent;
		}
		/// <summary>
		/// Creates either a deep or a shallow copy, depending on the value of the <c>deepCopy"</c> field.
		/// </summary>
		/// <param name="Parent">The parent <see cref="DataItem">DataItem</see> of the copy.</param>
		[Obsolete("Use Clone() instead.")]
		public virtual DataColumn Clone(DataItem Parent) {
			DataColumn d = new DataColumn();
			d.Parent = Parent;
			d.CopyFrom(this);
			return d;
		}
		/// <summary>
		/// Creates either a deep or a shallow copy, depending on the value of the <c>deepCopy"</c> field.
		/// </summary>
		public new virtual DataColumn Clone() {
			DataColumn d = new DataColumn();
			d.CopyFrom(this);
			return d;
		}

		/// <summary>
		/// The TypeIndex used for the compiler.
		/// </summary>
		public int TypeIndex {
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
		/// Returns the source for the compiler.
		/// </summary>
		public virtual string GetSource() {
			if (Source == "double.NaN") return null;
			return "namespace JohnsHope.FPlot.Library.Code{public class Item" + TypeIndex + 
				":JohnsHope.FPlot.Library.DataColumn.Instance{public override double this[int _i]{" +
				"get{double n = _i; Reset(); return \n" + source +
				";}}public override JohnsHope.FPlot.Library.Item Clone(){return new Item" + TypeIndex + "();}}}";
		}
		/// <summary>
		/// Gets the classname for the compiler.
		/// </summary>
		public string ClassName {
			get { return "JohnsHope.FPlot.Library.Code.Item" + TypeIndex; }
		}
		/// <summary>
		/// Sets the class instance (used by the compiler).
		/// </summary>
		public object ClassInstance {
			set {
				if (value != null && Source != null && Source != "double.NaN") {
					instance = (Instance)value;
					instance.Parent = Parent;
					base.Length = 0;
				} else instance = new Instance();
			}
		}
		/// <summary>
		/// The results of the compilation.
		/// </summary>
		public CompilerResults CompilerResults {
			get { return compilerresults; }
			set { compilerresults = value; }
		}
		/// <summary>
		/// Returns a new DataColumn that is computed after the given formula
		/// </summary>
		[Obsolete("Use FormulaColumn(string) instead.")]
		public static DataColumn FormulaColumn(DataItem Parent, string formula) {
			DataColumn c = new DataColumn(Parent);
			c.Source = formula;
			Compiler.Compile(c);
			return c;
		}
		/// <summary>
		/// Returns a new DataColumn that is computed after the given formula
		/// </summary>
		public static DataColumn FormulaColumn(string formula) {
			DataColumn c = new DataColumn();
			c.Source = formula;
			Compiler.Compile(c);
			return c;
		}

		private class NaNColumnInstance: Instance {
			public override double this[int n] { get { return double.NaN; } }
			public override Item Clone() {
				NaNColumnInstance nan = new NaNColumnInstance();
				nan.Parent = Parent;
				return nan;
			}
		}

		/// <summary>
		/// Returns a new DataColumn that always returns double.NaN
		/// </summary>
		[Obsolete("Use NaNColumn() instead.")]
		public static DataColumn NaNColumn(DataItem Parent) {
			DataColumn c = new DataColumn(Parent);
			c.source = "double.NaN";
			c.instance = new NaNColumnInstance();
			return c;
		}
		/// <summary>
		/// Returns a new DataColumn that always returns double.NaN
		/// </summary>
		public static DataColumn NaNColumn() {
			DataColumn c = new DataColumn();
			c.source = "double.NaN";
			c.instance = new NaNColumnInstance();
			return c;
		}
		/// <summary>
		/// Compiles the <c>DataColumn</c>.
		/// </summary>
		/// <returns>Returns true if the <c>DataColumn</c> was successfully compiled.</returns>
		public bool Compile() {
			return Compiler.Compile(this);
		}
	}
	}