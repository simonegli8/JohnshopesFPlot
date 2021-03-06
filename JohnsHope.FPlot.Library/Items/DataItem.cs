using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.CodeDom.Compiler;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.Serialization;
using System.Globalization;


namespace JohnsHope.FPlot.Library {

	/// <summary>
	/// A <see cref="Item">Item</see> class that implements data with the six columns x, y, z, dx, dy and dz, representing
	/// x, y and z data and the corresponding errors.
	/// </summary>
	[Serializable]
	public class DataItem: DataItemSerializer, ICompilableClass {

		/// <summary>
		/// A compiled instance of the DataItem.
		/// </summary>
		public abstract class Instance: Item {
			/// <summary>
			/// The DataItem the Instance belongs to.
			/// </summary>
			public DataItem Parent;

			/// <summary>
			/// The DataColumn x.
			/// </summary>
			public DataColumn x { get { return Parent.x; } set { Parent.x = value; } }
			/// <summary>
			/// The DataColumn x.
			/// </summary>
			public DataColumn y { get { return Parent.y; } set { Parent.y = value; } }
			/// <summary>
			/// The DataColumn x.
			/// </summary>
			public DataColumn z { get { return Parent.z; } set { Parent.z = value; } }
			/// <summary>
			/// The DataColumn x.
			/// </summary>
			public DataColumn dx { get { return Parent.dx; } set { Parent.dx = value; } }
			/// <summary>
			/// The DataColumn x.
			/// </summary>
			public DataColumn dy { get { return Parent.dy; } set { Parent.dy = value; } }
			/// <summary>
			/// The DataColumn x.
			/// </summary>
			public DataColumn dz { get { return Parent.dz; } set { Parent.dz = value; } }
			/// <summary>
			/// The Length of the columns. 
			/// </summary>
			public int Length {
				get { return Parent.Length; }
				set { Parent.Length = value; }
			}
			/// <summary>
			/// Returns an index to the DataColumns according to the dimensionality and column id
			/// </summary>
			public int Index(int column) {
				return column / Parent.Dimensions * 3 + column % Parent.Dimensions;
			}
			/// <summary>
			/// Returns the DataColumn according to the dimensionality and column id
			/// </summary>
			public DataColumn this[int column] {
				get {
					switch (Index(column)) {
						case 0: return x;
						case 1: return y;
						case 2: return z;
						case 3: return dx;
						case 4: return dy;
						case 5: return dz;
					}
					return null;
				}
			}
			/// <summary>
			/// Set or gets the double element this[column][row].
			/// </summary>
			public double this[int column, int row] {
				get { return this[column][row]; }
				set { this[column][row] = value; }
			}
			/// <summary>
			/// Returns the Dimensions of the Parent DataItem.
			/// </summary>
			public int Dimensions {
				get { return Parent.Dimensions; }
			}
			/// <summary>
			/// Returns the Dimensions of the Parent DataItem.
			/// </summary>
			public bool ErrorColumns {
				get { return Parent.ErrorColumns; }
			}
			/// <summary>
			/// Stores the values from data into the DataItem.
			/// </summary>
			public void CopyFrom(IEnumerable<double> data) {
				int n = 0, c = Dimensions;
				if (ErrorColumns) c = 2*c;
				IEnumerator<double> e = data.GetEnumerator();
				while (e.MoveNext()) this[n%c][n++/c] = e.Current;
			}
			/// <summary>
			/// Reads text numbers from a <see cref="Stream"/> and returns them as a <see cref="BigData"/> object.
			/// </summary>
			/// <param name="stream">The <see cref="Stream"/> to read from</param>
			/// <param name="separators">The characters that separate the numbers</param>
			/// <param name="numberFormat">A <c>System.Globalization.NumberFormat</c> object that describes the number format, for example 3,141
			/// instead of 3.141. You can get a NumberFormat object for the current culture from
			/// <c>System.Globalization.CultureInfo.CurrentUICulture.NumberFormat</c>.</param>
			[Obsolete("Use Text.Data instead.")]
			public static IList<double> ASCIIData(Stream stream, string separators, NumberFormatInfo numberFormat) {
				return Text.Data(stream, separators, numberFormat);
			}
			/// <summary>
			/// Reads text numbers from a <see cref="Stream"/> and returns them as a <see cref="BigData"/> object.
			/// </summary>
			/// <param name="stream">The Stream to read from</param>
			/// <param name="separators">The characters that separate the numbers</param>
			[Obsolete("Use Text.Data instead.")]
			public static IList<double> ASCIIData(Stream stream, string separators) {
				return Text.Data(stream, separators);
			}
			/// <summary>
			/// Reads a IList of double values from a binary stream
			/// </summary>
			/// <param name="stream">The stream to read from</param>
			/// <param name="type">The binary type of the numbers</param>
			/// <param name="BigEndian">If true, the numbers are read in big-endian format</param>
			/// <returns>A IList of double values read</returns>
			[Obsolete("Use Binary.Data instead.")]
			public IList<double> BinaryData(Stream stream, Type type, bool BigEndian) { return Binary.Data(stream, type, BigEndian); }

			/// <summary>
			/// Reads data from a WAV (PCM) file stream and returns them as a <see cref="BigData">BigData</see> object.
			/// </summary>
			/// <param name="stream">The WAV file stream to read from</param>
			/// <param name="channel">The channel of the WAV data that is read. Only this channel will be read.</param>
			/// <param name="SampleRate">The sampling rate of the WAV data in Hz.</param>
			/// <returns>An <c>IList</c> of double values read.</returns>
			[Obsolete("Use WAV.Data instead.")]
			public static IList<double> WAVData(Stream stream, int channel, out double SampleRate) { return WAV.Load(stream, channel, out SampleRate); }
			/// <summary>
			/// Reads data from a WAV (PCM) file stream and returns them as a <see cref="BigData">BigData</see> object.
			/// All channels of the WAV file are read.
			/// </summary>
			/// <param name="stream">The WAV file stream to read from</param>
			/// <param name="SampleRate">The sampling rate of the WAV data in Hz.</param>
			/// <returns>An <c>IList</c> of double values read.</returns>
			[Obsolete("Use WAV.Data instead.")]
			public static IList<double> WAVData(Stream stream, out double SampleRate) { return WAV.Load(stream, out SampleRate); }
			/// <summary>
			/// Loads the Instance from a stream.
			/// </summary>
			public abstract void Load(Stream stream);
			/// <summary>
			/// Creates a deep copy of the instance.
			/// </summary>
			public new abstract Item Clone();
		}
		
		//* private int length;
		/*
		/// <summary>
		/// A C# source string that specfies source code used to load the DataItem from a Stream.
		/// </summary>
		/// <remarks>
		/// The source
		/// is of the following form:
		/// <code>
		/// double[] x, y, z, dx, dy, dz;
		/// void OnLoad(Stream stream) {
		///   "loadsource";
		/// }
		/// </code>
		/// <example>
		/// An example code to load text data would be:
		/// <code>
		/// IList&lt;double&gt; d = Text.Data(stream, "; \n");
		/// int n = 0;
		/// while (n &lt; d.Count) {
		///   x[n/4] = d[n++];
		///   y[n/4] = d[n++];
		///   dx[n/4] = d[n++];
		///   dy[n/4] = d[n++];
		/// }
		/// </code>
		/// or for binary data of type ushort:
		/// <code>
		/// IList&lt;double&gt; d = Binary.Data(stream, typeof(ushort), false);
		/// ...
		/// </code>
		/// The code can assing the x, y, z, dx, dy and dz arrays and their size will be adjusted automatically.
		/// </example>
		/// </remarks>
		*/
		//* public string Source;
		
		/// <summary>
		/// The DataColumn x.
		/// </summary>
		public new DataColumn x { get { return base.x; } set { value.Parent = this; base.x = value; } }
		/// <summary>
		/// The DataColumn y.
		/// </summary>
		public new DataColumn y { get { return base.y; } set { value.Parent = this; base.y = value; } }
		/// <summary>
		/// The DataColumn z.
		/// </summary>
		public new DataColumn z { get { return base.z; } set { value.Parent = this; base.z = value; } }
		/// <summary>
		/// The DataColumn dx.
		/// </summary>
		public new DataColumn dx { get { return base.dx; } set { value.Parent = this; base.dx = value; } }
		/// <summary>
		/// The DataColumn dy.
		/// </summary>
		public new DataColumn dy { get { return base.dy; } set { value.Parent = this; base.dy = value; } }
		/// <summary>
		/// The DataColumn dz.
		/// </summary>
		public new DataColumn dz { get { return base.dz; } set { value.Parent = this; base.dz = value; } }

		[OptionalField(VersionAdded=312)]
		private bool useLowMemory = false;

		[OnDeserializing]
		private void Deserializing(StreamingContext sc) {
			useLowMemory = false;
		}

		/*
		private int dim;
		private bool err;
		*/

		/// <summary>
		/// The number of dimensions od the DataItem. If Dimensions == 1, only the x column is valid,
		/// if Dimensions == 2, the x and y columns are valid etc.
		/// </summary>
		public int Dimensions {
			get { return dim; }
			set {
				if (value < 1 || value > 3) throw new ArgumentOutOfRangeException("Dimensions", "Dimensions must lie between 1 and 3.");
				if (dim > value) {
					if (value < 3) {
						z.Source = dz.Source = "double.NaN";
						Compiler.Compile(z); Compiler.Compile(dz);
					}
					if (value < 2) {
						y.Source = dy.Source = "double.NaN";
						Compiler.Compile(y); Compiler.Compile(dy);
					}
				} else if (dim < value) {
					if (value > 2) {
						z.Source = dz.Source = null;
					}
					if (value > 1 && dim < 2) {
						y.Source = dy.Source = null;
					}
				}
				dim = value;
			}
		}
		/// <summary>
		/// If true the DataItem contains error columns
		/// </summary>
		public bool ErrorColumns {
			get { return err; }
			set {
				if (value != err) {
					if (!value) {
						dx.Source = dy.Source = dz.Source = "double.NaN";
						Compiler.Compile(dx);	Compiler.Compile(dy); Compiler.Compile(dz);
					} else {
						dx.Source = dy.Source = dz.Source = null;
					}
				}
				err = value;
			}
		}
		/// <summary>
		/// Gets or sets a value that indicates that the Set*Source methods will use a more complicated syntax but the resulting code
		/// will require less memory to load data.
		/// </summary>
		public bool UseLowMemory { get { return useLowMemory; } set { useLowMemory = value; } }
		/*
		/// <summary>
		/// If true, points are joined by a line.
		/// </summary>
		public bool Lines = false;
		/// <summary>
		/// If true, for each point a error mark is drawn.
		/// </summary>
		public bool Marks = true;
		/// <summary>
		/// If set to true, the area below the points will be filled with <see cref="FillColor">FillColor</see>. This functionality is not yet implemented.
		/// </summary>
		public bool FillArea = false;
		/// <summary>
		/// The color with which the area below the point will be filled. This functionality is not yet implemented.
		/// </summary>
		public Color FillColor = Color.LightBlue;

		private Color color = Color.Black;
		*/

		/// <summary>
		/// The color the Data is drawn with.
		/// </summary>
		public Color Color {
			get { return color; }
			set { color = value; }
		}

		//* private float lineWidth = 1;
		
		/// <summary>
		/// The line width the Data is drawn with. 
		/// </summary>
		public float LineWidth {
			get { return lineWidth; }
			set { lineWidth = value; }
		}
		//* private DashStyle lineStyle = DashStyle.Solid;
		/// <summary>
		/// The line style used to draw the item.
		/// </summary>
		public DashStyle LineStyle {
			get { return lineStyle; }
			set { lineStyle = value; }
		}

		[NonSerialized]
		private Instance instance = null;

		/// <summary>
		/// Copies from another DataItem.
		/// </summary>
		public override void CopyFrom(Item src) {
			base.CopyFrom(src);
			DataItem d = (DataItem)src;
			length = d.length;
			Source = d.Source;
			x = d.x.Clone(); y = d.y.Clone(); z = d.z.Clone();
			dx = d.dx.Clone(); dy = d.dy.Clone(); dz = d.dz.Clone();
			Lines = d.Lines;
			Marks = d.Marks;
			color = d.color;
			lineWidth = d.lineWidth;
			lineStyle = d.lineStyle;
			FillArea = d.FillArea;
			FillColor = d.FillColor;
			Compiler.Compile(this);
		}
		/// <summary>
		/// The default constructor.
		/// </summary>
		public DataItem() {
			x = new DataColumn();
			y = new DataColumn();
			z = new DataColumn();
			dx = new DataColumn();
			dy = new DataColumn();
			dz = new DataColumn();
			dim = 3;
			err = true;
			Length = 0;
		}
		/// <summary>
		/// Returns an index to the DataColumns according to the dimensionality and column id
		/// </summary>
		public int Index(int column) {
			return column / Dimensions * 3 + column % Dimensions;
		}
		/// <summary>
		/// Returns the DataColumn according to the dimensionality and column id
		/// </summary>
		public DataColumn this[int column] {
			get {
				switch (Index(column)) {
				case 0: return x;
				case 1: return y;
				case 2: return z;
				case 3: return dx;
				case 4: return dy;
				case 5: return dz;
				}
				return null;
			}
		}
		/// <summary>
		/// Sets or gets the length of the data.
		/// </summary>
		public int Length {
			get { return length; }
			set {
				length = value;
				x.Length = y.Length = z.Length = dx.Length = dy.Length = dz.Length = value;
			}
		}
		/// <summary>
		/// Indicates if the DataItem was modified. This value is automatically set, if data is changed.
		/// </summary>
		public override bool Modified {
			get {
				bool b;
				lock (this) {
					b = base.Modified || x.Modified || y.Modified || z.Modified || dx.Modified || dy.Modified || dz.Modified;
				}
				return b;
			}
			set {
				lock (this) {
					if (value == false) {
						x.Modified = y.Modified = z.Modified = dx.Modified = dy.Modified = dz.Modified = false;
					}
					base.Modified = value;
				}
			}
		}
		/// <summary>
		/// Returns the sourcecode of the loadsource and the formulas of the DataColumns.
		/// </summary>
		public override string GetSource() {
			return "namespace JohnsHope.FPlot.Library.Code{public class Item" + TypeIndex +
				":JohnsHope.FPlot.Library.DataItem.Instance{public override void Load(System.IO.Stream stream){" + Source + "}" +
				"public override JohnsHope.FPlot.Library.Item Clone(){return new Item" + TypeIndex + "();}}}";
		}
		/// <summary>
		/// Gets the class name for the compiler.
		/// </summary>
		public string ClassName {
			get { return "JohnsHope.FPlot.Library.Code.Item" + TypeIndex; }
		}
		/// <summary>
		/// Sets the class instance for the compiler.
		/// </summary>
		public object ClassInstance {
			set {
				if (value != null) {
					instance = (Instance)value;
					instance.Parent = this;
					instance.x = x; instance.y = y; instance.z = z;
					instance.dx = dx; instance.dy = dy; instance.dz = dz;
				} else instance = null;
			}
		}
		/// <summary>
		/// Overrides Item.Compile. Compiles the DataItem and all DataColumns x, y, z, dx, dy, dz.
		/// </summary>
		/// <returns>Returns true if the DataItem and all DataColumns are compiled without errors.</returns>
		public override bool Compile() {
			bool res = true;
			if (x.Source != null) res = x.Compile() && res;
			if (y.Source != null) res = y.Compile() && res;
			if (z.Source != null) res = z.Compile() && res;
			if (dx.Source != null) res = dx.Compile() && res;
			if (dy.Source != null) res = dy.Compile() && res;
			if (dz.Source != null) res = dz.Compile() && res;
			return base.Compile() && res;
		}

		/// <summary>
		/// This routine loads the Data from a Stream. This method can be overriden an is used by the method Load.
		/// The method must load the data and assign it to the x, y, z, dx, dy and dz arrays. The Length of the arrays is
		/// adapted automatically.
		/// </summary>
		public virtual void OnLoad(Stream stream) {
			if (instance != null) {
				instance.Parent = this;
				instance.Load(stream);
			}
		}
		/// <summary>
		/// This routine loads the DataItem from a Stream. 
		/// </summary>
		/// <param name="stream">The stream to load from</param>
		public void Load(Stream stream) {
			Length = 0;
			bool xs = x.AutoResize, ys = y.AutoResize, zs = z.AutoResize;
			bool dxs = dx.AutoResize, dys = dy.AutoResize, dzs = dz.AutoResize;
			x.AutoResize = y.AutoResize = dx.AutoResize = dy.AutoResize = true;
			OnLoad(stream);
			x.deepCopy = y.deepCopy = dx.deepCopy = dy.deepCopy = true;
			x.AutoResize = xs; y.AutoResize = ys; z.AutoResize = zs;
			dx.AutoResize = dxs; dy.AutoResize = dys; z.AutoResize = dzs;
		}
		/// <summary>
		/// This routine loads the <c>DataItem</c> from a file.
		/// </summary>
		/// <param name="filename">The filename of the file to load from.</param>
		public void Load(string filename) {
			using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read)) {
				Load(stream);
			}
		}
		/// <summary>
		/// Returns a Painter for the <c>DataItem</c>.
		/// </summary>
		public override Painter Painter(PlotModel model) {
			return new DataItemPainter2D(model, this);
		}
		/// <summary>
		/// Gets a user friendly name for the Item type.
		/// </summary>
		public override string TypeName() {
			return "Data";
		}
		/// <summary>
		/// Returns all <c>DataItems</c> in a given <see href="ItemList">ItemList</see>
		/// </summary>
		public static ItemList DataItems(ItemList list) {
			ItemList res = new ItemList();
			foreach (Item x in list) {
				if (x is DataItem) res.Add(x);
			}
			return res;
		}
		/// <summary>
		/// Stores the values from data into the DataItem
		/// </summary>
		public void CopyFrom(IEnumerable<double> data) {
			int c = Dimensions;
			if (ErrorColumns) c = 2*c;
			IEnumerator<double> e = data.GetEnumerator();
			int i = 0;
			while (i < c && !this[i].CanWrite) i++;
			if (i == c) return; // there is no column in the DataItem that can be written to.
			i = 0;
			while (e.MoveNext()) {
				while (!this[i%c].CanWrite) i++;
				this[i%c][i/c] = e.Current;
				i++;
			}
		}
		/// <summary>
		/// Gets the source used to store the data in a <c>IList&lt;double&gt;</c> into the <c>DataItem</c>.
		/// </summary>
		/// <param name="data">The name of the variable the data is stored in.</param>
		/// <param name="str">The <c>StringBuilder</c> the resulting source code is appended to.</param>
		public void GetReadSource(string data, StringWriter str) {
			bool err = ErrorColumns;
			int dim = Dimensions, f = 1, n;
			if (err) f = 2;
			n = dim*f;
			if (!x.CanWrite) n--;
			if (dim > 1 && !y.CanWrite) n--;
			if (dim > 2 && !z.CanWrite) n--;
			if (err && !dx.CanWrite) n--;
			if (err && dim > 1 && !dy.CanWrite) n--;
			if (err && dim > 2 && !dz.CanWrite) n--;
			str.WriteLine("int n = 0;");
			if (useLowMemory) {
				str.WriteLine("while (" + data + ".HasNext) {");
				str.Ident++;
				if (x.CanWrite) str.WriteLine("x[n] = " + data + ".Read();");
				if (dim >= 2 && y.CanWrite) str.WriteLine("y[n] = " + data + ".Read();");
				if (dim >= 3 && z.CanWrite) str.WriteLine("z[n] = " + data + ".Read();");
				if (err && dim >= 1 && dx.CanWrite) str.WriteLine("dx[n] = " + data + ".Read();");
				if (err && dim >= 2 && dy.CanWrite) str.WriteLine("dy[n] = " + data + ".Read();");
				if (err && dim >= 3 && dz.CanWrite) str.WriteLine("dz[n] = " + data + ".Read();");
				str.WriteLine("n++;");
				str.Ident--;
				str.WriteLine("}");
			} else {
				string ndivm;
				if (n == 1) ndivm = "n";
				else ndivm = "n/" + n;
				str.WriteLine("while (n < " + data + ".Count) {");
				str.Ident++;
				if (x.CanWrite) str.WriteLine("x[" + ndivm + "] = " + data + "[n++];");
				if (dim >= 2 && y.CanWrite) str.WriteLine("y[" + ndivm + "] = " + data + "[n++];");
				if (dim >= 3 && z.CanWrite) str.WriteLine("z[" + ndivm + "] = " + data + "[n++];");
				if (err && dim >= 1 && dx.CanWrite) str.WriteLine("dx[" + ndivm + "] = " + data + "[n++];");
				if (err && dim >= 2 && dy.CanWrite) str.WriteLine("dy[" + ndivm + "] = " + data + "[n++];");
				if (err && dim >= 3 && dz.CanWrite) str.WriteLine("dz[" + ndivm + "] = " + data + "[n++];");
				str.Ident--;
				str.WriteLine("}");
			}
		}

		/// <summary>
		/// Sets the <see cref="DataItemSerializer.Source"/> of this <c>DataItem</c> to source code that loads an text file.
		/// </summary>
		/// <param name="separators">The string of separator characters that separates individual numbers.</param>
		/// <param name="LocalizedNumberFormat">If true, the localized number format is used, for example 3,141
		/// instead of 3.141.</param>
		/// <param name="encoding">The text encoding to use.</param>
		public void SetLoadTextSource(string separators, bool LocalizedNumberFormat, Encoding encoding) {
			StringWriter str = new StringWriter();
			separators = separators.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t").Replace("\"", "\\\"");
			if (useLowMemory) {
				if (encoding == null || encoding == Encoding.UTF8) str.WriteLine("using (StreamReader reader = new StreamReader(stream)) {");
				else str.WriteLine("using (StreamReader reader = new StreamReader(stream, " + Text.EncodingSource(encoding) + ")) {");
				str.Ident++;
				if (LocalizedNumberFormat)
					str.WriteLine("DataReader data = Text.Reader(reader, \"" + separators + "\", NumberFormatInfo.CurrentInfo);");
				else str.WriteLine("DataReader data = Text.Reader(reader, \"" + separators + "\");");
				GetReadSource("data", str);
				str.Ident--;
				str.WriteLine("}");
			} else {
				if (encoding == null || encoding == Encoding.UTF8) {
					if (LocalizedNumberFormat)
						str.WriteLine("IList<double> data = Text.Data(stream, \"" + separators + "\", NumberFormatInfo.CurrentInfo);");
					else str.WriteLine("IList<double> data = Text.Data(stream, \"" + separators + "\");");
				}  else {
					if (LocalizedNumberFormat)
						str.WriteLine("IList<double> data = Text.Data(stream, " + Text.EncodingSource(encoding) + ", \"" + separators + 
							"\", NumberFormatInfo.CurrentInfo);");
					else str.WriteLine("IList<double> data = Text.Data(stream, " + Text.EncodingSource(encoding) + ", \"" + separators + "\");");
				}
				
				GetReadSource("data", str);
			}
			Source = str.ToString();
		}
		/// <summary>
		/// Sets the <see cref="DataItemSerializer.Source"/> of this <c>DataItem</c> to source code that loads an text file. This method is obsolete,
		/// use SetLoadTextSource instead.
		/// </summary>
		/// <param name="separators">The string of separator characters that separates individual numbers.</param>
		/// <param name="LocalizedNumberFormat">If true, the localized number format is used, for example 3,141
		/// instead of 3.141.</param>
		[Obsolete("Use SetLoadTextSource instead.")]
		public void SetLoadASCIISource(string separators, bool LocalizedNumberFormat) {
			SetLoadTextSource(separators, LocalizedNumberFormat, Encoding.UTF8);
		}
		/// <summary>
		/// Sets the <see cref="DataItemSerializer.Source"/> of this <c>DataItem</c> to source code that loads a binary file.
		/// </summary>
		/// <param name="type">The data type of an individual number in the file</param>
		/// <param name="BigEndian">If true big endian byte ordering is assumed, otherwise little endian.</param>
		public void SetLoadBinarySource(Type type, bool BigEndian) {
			StringWriter str = new StringWriter();
			string typename;
			if (type == typeof(sbyte)) typename = "sbyte";
			else if (type == typeof(short)) typename = "short";
			else if (type == typeof(int)) typename = "int";
			else if (type == typeof(long)) typename = "long";
			else if (type == typeof(byte)) typename = "byte";
			else if (type == typeof(ushort)) typename = "ushort";
			else if (type == typeof(uint)) typename = "uint";
			else if (type == typeof(ulong)) typename = "ulong";
			else if (type == typeof(float)) typename = "float";
			else if (type == typeof(double)) typename = "double";
			else typename = type.FullName;
			if (useLowMemory) {
				if (BigEndian) str.WriteLine("DataReader data = Binary.Reader(stream, typeof(" + typename + "), true);");
				else str.WriteLine("DataReader data = Binary.Reader(stream, typeof(" + typename + "));");
			} else {
				if (BigEndian) str.WriteLine("IList<double> data = Binary.Data(stream, typeof(" + typename + "), true);");
				else str.WriteLine("IList<double> data = Binary.Data(stream, typeof(" + typename + "));");
			}
			GetReadSource("data", str);
			Source = str.ToString();
		}
		/// <summary>
		/// If passed to <see cref="SetLoadWAVSource(int, DataColumn)">SetLoadWAVSource</see>, all channels are loaded.
		/// </summary>
		const int AllChannels = WAV.AllChannels;
		/// <summary>
		///  Sets the <see cref="DataItemSerializer.Source"/> of this <c>DataItem</c> to source code that loads a WAV file.
		/// </summary>
		/// <param name="channel">The WAV file channel to load.</param>
		/// <param name="sampleColumn">The <see cref="DataColumn"/> where to store the equally spaced sample points.</param>
		public void SetLoadWAVSource(int channel, DataColumn sampleColumn) {
			StringWriter str = new StringWriter();
			x.Source = "n";
			str.WriteLine("double sampleRate;");
			if (useLowMemory) {
				if (channel == AllChannels) str.WriteLine("DataReader data = WAV.Reader(stream, out sampleRate);");
				else str.WriteLine("DataReader data = WAV.Reader(stream, " + channel.ToString() + ", out sampleRate);");
			} else {
				if (channel == AllChannels) str.WriteLine("IList<double> data = WAV.Data(stream, out sampleRate);");
				else str.WriteLine("IList<double> data = WAV.Data(stream, " + channel.ToString() + ", out sampleRate);");
			}
			if (sampleColumn == x) str.WriteLine("x = WAV.Samples(sampleRate);");
			else if (sampleColumn == y) str.WriteLine("y = WAV.Samples(sampleRate);");
			else if (sampleColumn == z) str.WriteLine("z = WAV.Samples(sampleRate);");
			else if (sampleColumn == dx) str.WriteLine("dx = WAV.Samples(sampleRate);");
			else if (sampleColumn == dy) str.WriteLine("dy = WAV.Samples(sampleRate);");
			else if (sampleColumn == dz) str.WriteLine("dz = WAV.Samples(sampleRate);");
			GetReadSource("data", str);
			Source = str.ToString();
		}
		/// <summary>
		///  Sets the <see cref="DataItemSerializer.Source"/> of this <c>DataItem</c> to source code that loads a WAV file.
		/// </summary>
		/// <param name="sampleColumn">The <see cref="DataColumn"/> where to store the equally spaced sample points.</param>
		public void SetLoadWAVSource(DataColumn sampleColumn) { SetLoadWAVSource(AllChannels, sampleColumn); }
		/// <summary>
		/// Sets the <see cref="DataItemSerializer.Source"/> of this <c>DataItem</c> to source code that loads an Excel file.
		/// </summary>
		public void SetLoadExcelSource(int sheet, int startRow, int endRow, params int[] columns) {
			StringWriter str = new StringWriter();
			if (useLowMemory) str.Write("DataReader data = Excel.Reader(stream, ");
			else str.Write("IList<double> data = Excel.Data(stream, ");
			str.Write(sheet.ToString() + ", " + startRow.ToString() + ", " + endRow.ToString());
			foreach (int col in columns) str.Write(", " + col.ToString());
			str.WriteLine(");");
			GetReadSource("data", str);
			Source = str.ToString();
		}
		/// <summary>
		/// Sets the <see cref="DataItemSerializer.Source"/> of this <c>DataItem</c> to source code that loads an Excel file.
		/// </summary>
		public void SetLoadExcelSource(int sheet, int startRow, int endRow, params string[] columns) {
			StringWriter str = new StringWriter();
			if (useLowMemory) str.Write("DataReader data = Excel.Reader(stream, ");
			else str.Write("IList<double> data = Excel.Data(stream, ");
			str.Write(sheet.ToString() + ", " + startRow.ToString() + ", " + endRow.ToString());
			foreach (string col in columns) str.Write(", \"" + col + "\"");
			str.WriteLine(");");
			GetReadSource("data", str);
			Source = str.ToString();
		}
		/// <summary>
		/// Sets the <see cref="DataItemSerializer.Source"/> of this <c>DataItem</c> to source code that loads an Excel file.
		/// </summary>
		public void SetLoadExcelSource(int sheet, int startRow, params string[] columns) {
			StringWriter str = new StringWriter();
			if (useLowMemory) str.Write("DataReader data = Excel.Reader(stream, ");
			else str.Write("IList<double> data = Excel.Data(stream, ");
			str.Write(sheet.ToString() + ", " + startRow.ToString());
			foreach (string col in columns) str.Write(", \"" + col + "\"");
			str.WriteLine(");");
			GetReadSource("data", str);
			Source = str.ToString();
		}
		/// <summary>
		/// Sets the <see cref="DataItemSerializer.Source"/> of this <c>DataItem</c> to source code that loads an Excel file.
		/// </summary>
		public void SetLoadExcelSource(int startRow, params string[] columns) {
			StringWriter str = new StringWriter();
			if (useLowMemory) str.Write("DataReader data = Excel.Reader(stream, ");
			else str.Write("IList<double> data = Excel.Data(stream, ");
			str.Write(startRow.ToString());
			foreach (string col in columns) str.Write(", \"" + col + "\"");
			str.WriteLine(");");
			GetReadSource("data", str);
			Source = str.ToString();
		}
		/// <summary>
		/// Sets the <see cref="DataItemSerializer.Source"/> of this <c>DataItem</c> to source code that loads an Excel file.
		/// </summary>
		public void SetLoadExcelSource(params string[] columns) {
			StringWriter str = new StringWriter();
			if (useLowMemory) str.Write("DataReader data = Excel.Reader(stream");
			else str.Write("IList<double> data = Excel.Data(stream");
			foreach (string col in columns) str.Write(", \"" + col + "\"");
			str.WriteLine(");");
			GetReadSource("data", str);
			Source = str.ToString();
		}
		
		/// <summary>
		/// Loads an text file into the <c>DataItem</c>.
		/// </summary>
		/// <param name="filename">The file to load</param>
		/// <param name="separators">The string of separator characters that separates individual numbers.</param>
		/// <param name="numberFormat">The number format used, for example 3,141
		/// instead of 3.141.</param>
		/// <param name="encoding">The text encoding to use.</param>
		public void LoadText(string filename, string separators, NumberFormatInfo numberFormat, Encoding encoding) {
			instance = new Text.Loader(separators, numberFormat, encoding);
			Load(filename);
		}

		/// <summary>
		/// Loads an text file into the <c>DataItem</c>.
		/// </summary>
		/// <param name="filename">The file to load</param>
		/// <param name="separators">The string of separator characters that separates individual numbers.</param>
		public void LoadText(string filename, string separators) {
			instance = new Text.Loader(separators);
			Load(filename);
		}

		/// <summary>
		/// Loads an text file into the <c>DataItem</c>.
		/// </summary>
		/// <param name="filename">The file to load</param>
		/// <param name="separators">The string of separator characters that separates individual numbers.</param>
		/// <param name="LocalizedNumberFormat">If true, the localized number format is used, for example 3,141
		/// instead of 3.141.</param>
		[Obsolete("Use LoadText instead.")]
		public void LoadASCII(string filename, string separators, bool LocalizedNumberFormat) {
			if (LocalizedNumberFormat) LoadText(filename, separators, NumberFormatInfo.CurrentInfo, Encoding.UTF8);
			else LoadText(filename, separators, NumberFormatInfo.InvariantInfo, Encoding.UTF8);
		}
		/// <summary>
		/// Loads an ASCII file into the <c>DataItem</c>.
		/// </summary>
		/// <param name="filename">The file to load</param>
		/// <param name="separators">The string of separator characters that separates individual numbers.</param>
		[Obsolete("Use LoadText instead.")]
		public void LoadASCII(string filename, string separators) { LoadText(filename, separators); }
		/// <summary>
		/// Loads an binary file into the <c>DataItem</c>.
		/// </summary>
		/// <param name="filename">The file to load</param>
		/// <param name="type">The data type of an individual number in the file</param>
		/// <param name="BigEndian">If true big endian byte ordering is assumed, otherwise little endian.</param>
		public void LoadBinary(string filename, Type type, bool BigEndian) {
			instance = new Binary.Loader(type, BigEndian);
			Load(filename);
		}

		/// <summary>
		/// Loads an binary file into the <c>DataItem</c>.
		/// </summary>
		/// <param name="filename">The file to load</param>
		/// <param name="type">The data type of an individual number in the file</param>
		public void LoadBinary(string filename, Type type) { LoadBinary(filename, type, false); }

		private void AssignDataColumn(ref DataColumn target, DataColumn source) {
			if (target.Parent == this) { 
				if (target == this.x) x = source;
				else if (target == this.y) y = source;
				else if (target == this.z) z = source;
				else if (target == this.dx) dx = source;
				else if (target == this.dy) dy = source;
				else if (target == this.dz) dz = source;
			}
			target = source;
		}

		/// <summary>
		/// Loads an binary file into the <c>DataItem</c>.
		/// </summary>
		/// <param name="filename">The file to load</param>
		/// <param name="channel">The WAV file channel to load.</param>
		/// <param name="sampleColumn">The <see cref="DataColumn"/> that will be set to the equally spaced sample points.</param>
		public void LoadWAV(string filename, int channel, out DataColumn sampleColumn) {
			WAV.Loader loader = new WAV.Loader(channel);
			sampleColumn = loader.Samples();
			instance = loader;
			Load(filename);
			((WAV.SamplesColumn)sampleColumn).SampleRate = loader.SampleRate;
		}

		/// <summary>
		/// Loads an binary file into the <c>DataItem</c>.
		/// </summary>
		/// <param name="filename">The file to load</param>
		/// <param name="sampleColumn">The <see cref="DataColumn"/> that will be set to the equally spaced sample points.</param>
		public void LoadWAV(string filename, out DataColumn sampleColumn) {
			WAV.Loader loader = new WAV.Loader();
			sampleColumn = loader.Samples();
			instance = loader;
			Load(filename);
			((WAV.SamplesColumn)sampleColumn).SampleRate = loader.SampleRate;
		}

		/// <summary>
		/// Loads an binary file into the <c>DataItem</c>.
		/// </summary>
		/// <param name="filename">The file to load</param>
		/// <param name="channel">The WAV file channel to load.</param>
		/// <param name="sampleColumn">The <see cref="DataColumn"/> that will be set to the equally spaced sample points. Must be one of
		/// the x, y, z, dx, dy, dz DataColumns of the DataItem.</param>
		public void LoadWAV(string filename, int channel, DataColumn sampleColumn) {
			WAV.Loader loader = new WAV.Loader(channel);
			AssignDataColumn(ref sampleColumn, loader.Samples());
			instance = loader;
			Load(filename);
			((WAV.SamplesColumn)sampleColumn).SampleRate = loader.SampleRate;
		}

		/// <summary>
		/// Loads an binary file into the <c>DataItem</c>.
		/// </summary>
		/// <param name="filename">The file to load</param>
		/// <param name="sampleColumn">The <see cref="DataColumn"/> that will be set to the equally spaced sample points. Must be one of
		/// the x, y, z, dx, dy, dz DataColumns of the DataItem.</param>
		public void LoadWAV(string filename, DataColumn sampleColumn) {
			WAV.Loader loader = new WAV.Loader();
			AssignDataColumn(ref sampleColumn, loader.Samples());
			instance = loader;
			Load(filename);
			((WAV.SamplesColumn)sampleColumn).SampleRate = loader.SampleRate;
		}

		/// <summary>
		/// Loads an binary file into the <c>DataItem</c>. The x <see cref="DataColumn"/> of the DataItem is set to equally spaced
		/// sample points.Marks
		/// </summary>
		/// <param name="filename">The file to load</param>
		/// <param name="channel">The WAV file channel to load.</param>
		public void LoadWAV(string filename, int channel) {
			WAV.Loader loader = new WAV.Loader(channel);
			x = loader.Samples();
			instance = loader;
			Load(filename);
			((WAV.SamplesColumn)x).SampleRate = loader.SampleRate;
		}

		/// <summary>
		/// Loads an binary file into the <c>DataItem</c>. The x <see cref="DataColumn"/> of the DataItem is set to equally spaced
		/// sample points.
		/// </summary>
		/// <param name="filename">The file to load</param>
		public void LoadWAV(string filename) {
			WAV.Loader loader = new WAV.Loader();
			x = loader.Samples();
			instance = loader;
			Load(filename);
			((WAV.SamplesColumn)x).SampleRate = loader.SampleRate;
		}


		/// <summary>
		/// Loads an Excel file into the <c>DataItem</c>. This functionality is only provided, if JohnsHope.FPlot.Excel.dll is loaded.
		/// </summary>
		/// <param name="filename">The file to load</param>
		/// <param name="sheet">The Excel worksheet (1 for the first worksheet)</param>
		/// <param name="startRow">The row where the numbers data start. (1 for the first row)</param>
		/// <param name="endRow">The row where the numbers data ends. (1 for the first row)</param>
		/// <param name="columns">The columns where the numbers are in. (1 for the first column)</param>
		public void LoadExcel(string filename, int sheet, int startRow, int endRow, params int[] columns) {
			instance = new Excel.Loader(sheet, startRow, endRow, columns);
			Load(filename);
		}
		/// <summary>
		/// Loads an Excel file into the <c>DataItem</c>. This functionality is only provided, if JohnsHope.FPlot.Excel.dll is loaded.
		/// </summary>
		/// <param name="filename">The file to load</param>
		/// <param name="sheet">The Excel worksheet (1 for the first worksheet)</param>
		/// <param name="startRow">The row where the numbers data start. (1 for the first row)</param>
		/// <param name="endRow">The row where the numbers data ends. (1 for the first row)</param>
		/// <param name="columns">The columns where the numbers are in. (like "A" for the first column, "B" for the second etc.)</param>
		public void LoadExcel(string filename, int sheet, int startRow, int endRow, params string[] columns) {
			instance = new Excel.Loader(sheet, startRow, endRow, columns);
			Load(filename);
		}
		/// <summary>
		/// Loads an Excel file into the <c>DataItem</c>. This functionality is only provided, if JohnsHope.FPlot.Excel.dll is loaded.
		/// </summary>
		/// <param name="filename">The file to load</param>
		/// <param name="sheet">The Excel worksheet (1 for the first worksheet)</param>
		/// <param name="startRow">The row where the numbers data start. (1 for the first row)</param>
		/// <param name="columns">The columns where the numbers are in. (like "A" for the first column, "B" for the second etc.)</param>
		public void LoadExcel(string filename, int sheet, int startRow, params string[] columns) {
			instance = new Excel.Loader(sheet, startRow, columns);
			Load(filename);
		}
		/// <summary>
		/// Loads an Excel file into the <c>DataItem</c>. This functionality is only provided, if JohnsHope.FPlot.Excel.dll is loaded.
		/// </summary>
		/// <param name="filename">The file to load</param>
		/// <param name="startRow">The row where the numbers data start. (1 for the first row)</param>
		/// <param name="columns">The columns where the numbers are in. (like "A" for the first column, "B" for the second etc.)</param>
		public void LoadExcel(string filename, int startRow, params string[] columns) {
			instance = new Excel.Loader(startRow, columns);
			Load(filename);
		}
		/// <summary>
		/// Loads an Excel file into the <c>DataItem</c>. This functionality is only provided, if JohnsHope.FPlot.Excel.dll is loaded.
		/// Source will be overwritten.
		/// </summary>
		/// <param name="filename">The file to load</param>
		/// <param name="columns">The columns where the numbers are in. (like "A" for the first column, "B" for the second etc.)</param>
		public void LoadExcel(string filename, params string[] columns) {
			instance = new Excel.Loader(columns);
			Load(filename);
		}
	}
	/// <summary>
	/// A 2D <see cref="Painter">Painter</see> for a DataItem.
	/// </summary>
	public class DataItemPainter2D: Painter {
		//TODO DataItemPainter2D-Samples
		//TODO custom Markers

		private const int step = 1000;
		private DataItem data;
		// The image to draw
		[NonSerialized]
		private BitmapBuilder cache;
		/// <summary>
		/// The constructor.
		/// </summary>
		public DataItemPainter2D(PlotModel Model, Item Item) : base(Model, Item) { data = (DataItem)Item; }

		/// <summary>
		/// Starts recalculation of the Painter
		/// </summary>
		public override void Start(Plot plot) {
			base.Start(plot);
			if (Recalc) {
				cache = new BitmapBuilder(plot.Bounds);
				cache.Graphics.Clear(Color.Transparent);
				plot.MaxProgress += data.Length/step;
			}
		}
		/// <summary>
		/// Recalculates the Painter's image data
		/// </summary>
		public override void Calc(Plot plot) {
			double xw, yw, x, y;
			Plot2D plot2D = (Plot2D)plot;
			PointF[] pts = new PointF[data.Length];
			BitmapBuilder bmp = this.cache;
			Graphics g = bmp.Graphics;
			Pen Pen = new Pen(new SolidBrush(data.Color));
			xw = (Model.x1 - Model.x0)/plot.Bounds.Width;
			yw = (Model.y1 - Model.y0)/plot.Bounds.Height;
			float X, Y, DX, DY, D = 2 + data.LineWidth;
			bool xNaN, yNaN;
			for (int i = 0; i < data.Length; i++) {
				if (plot.DrawStop) break;
				try {
					// X = (float)((d.x[i] - Model.x0)/xw);
					// Y = (float)((d.y[i] - Model.y0)/yw);
					if (data.Dimensions == 1) {
						x = i;
						y = data.x[i];
					} else {
						x = data.x[i];
						y = data.y[i];
					}

					X = plot2D.DeviceCoordinateX(x);
					Y = plot2D.DeviceCoordinateY(y);
					DX = (float)(data.dx[i]/xw)/2;
					if (float.IsNaN(DX)) {
						DX = D*3.22F;
						xNaN = true;
					} else xNaN = false;
					DY = (float)(data.dy[i]/yw)/2;
					if (float.IsNaN(DY)) {
						DY = D*3.22F;
						yNaN = true;
					} else yNaN = false;
					pts[i].X = X; pts[i].Y = Y;
					if (data.Marks) {
						lock (this) {
							Pen.DashStyle = DashStyle.Solid;
							Pen.Width = data.LineWidth;
							if (DX < 3) DX = D*1.66F;
							else if (!xNaN) {
								g.DrawLine(Pen, X-DX, Y-D, X-DX, Y+D);
								g.DrawLine(Pen, X+DX, Y-D, X+DX, Y+D);
							}
							if (DY < 3) DY = D*1.66F;
							else if (!yNaN) {
								g.DrawLine(Pen, X-D, Y-DY, X+D, Y-DY);
								g.DrawLine(Pen, X-D, Y+DY, X+D, Y+DY);
							}
							g.DrawLine(Pen, X-DX, Y, X+DX, Y);
							g.DrawLine(Pen, X, Y-DY, X, Y+DY);
						}
					}
				} catch (System.Threading.ThreadAbortException ex) {
					throw ex;
				} catch { }
				if ((i+1)%step == 0) plot2D.Progress++;
			}
			if (data.Lines && !plot2D.DrawStop) {
				Pen.DashStyle = data.LineStyle;
				Pen.Width = data.LineWidth;
				try {
					lock (this) {
						g.DrawLines(Pen, pts);
					}
				} catch (System.Threading.ThreadAbortException ex) {
					throw ex;
				} catch { }
			}
			Recalc = plot2D.DrawStop;
		}
		/// <summary>
		/// Paints the Painter.
		/// </summary>
		public override void Paint(Graphics g, Plot plot) {
			cache.TryPaint(g);
		}
	}
}
