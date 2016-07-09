using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// A class implements reading data from Excel-Sheets.
	/// Normally the functions of this class are disabled and throw an exception. In order to activate the 
	/// functions you must reference JohnsHope.FPlot.Excel.dll in your project which implements an extension of Excel that
	/// has the working functions in it. If you load the JohnsHope.FPlot.Excel.dll and call JohnsHope.FPlot.ExcelData.Excel.Init,
	/// it will assign it's implementation of the Excel class to the static variable Excel.Implementation,
	/// so that the Excel methods can use this implementation. JohnsHope.FPlot.Excel relies on a commercial dll NativeExcel.dll.
	/// If you use the Excel functionality in your projects, you must purchase a valid developpers license for NativeExcel for 120$
	/// from <a href="http://www.nika-soft.com">www.nika-soft.com</a>.
	/// </summary>
	public class Excel {

		//BUG read data Enumerator.Current not implemented in Excel.Data

		/// <summary>
		/// An instance of an derived ExcelReader class that implements reading from Excel files. 
		/// </summary>
		public static Excel Implementation = new Excel();

		/// <summary>
		/// A class that implements a <see cref="IEnumerator{T}">IEnumerator&lt;double&gt;</see> that reads from an Excel <see cref="Stream"/>.
		/// </summary>
		public class Enumerator: IEnumerator<double> {
			#region IEnumerator<double> Members

			double IEnumerator<double>.Current {
				get { throw new NotImplementedException(); }
			}

			#endregion

			#region IDisposable Members

			void IDisposable.Dispose() {
				throw new NotImplementedException();
			}

			#endregion

			#region IEnumerator Members

			object System.Collections.IEnumerator.Current {
				get { throw new NotImplementedException(); }
			}

			bool System.Collections.IEnumerator.MoveNext() {
				throw new NotImplementedException();
			}

			void System.Collections.IEnumerator.Reset() {
				throw new NotImplementedException();
			}

			#endregion
		}

		/// <summary>
		/// A <see cref="DataReader"/> class that implements reading numbers from an Excel <see cref="Stream"/>.
		/// </summary>
		public class DataReader: JohnsHope.FPlot.Library.DataReader {
			/// <summary>
			/// A constructor that initializes the DataReader with a IEnumerable&lt;double&gt;.
			/// </summary>
			/// <param name="source">The source to initilaize the DataReader with.</param>
			public DataReader(IEnumerable<double> source): base(source) { }
			/// <summary>
			/// A constructor that initializes the DataReader with a IEnumerator&lt;double&gt;.
			/// </summary>
			/// <param name="source">The source to initilaize the DataReader with.</param>
			public DataReader(IEnumerator<double> source) : base(source) { }

		}

		/// <summary>
		/// A <see cref="DataItem.Instance"/> class that implements fast loading of text data. This class is used internally
		/// by the method <see cref="DataItem.LoadExcel(string, string[])"/>.
		/// </summary>
		public class Loader: DataItem.Instance {

			int sheet, startRow, endRow;
			int[] icolumns;
			string[] scolumns;

			/// <summary>
			/// The default constructor.
			/// </summary>
			public Loader() {
				sheet = 1;
				startRow = -1;
				endRow = -1;
			}

			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="columns">The columns to read from.</param>
			public Loader(params string[] columns): this() {
				scolumns = columns;
			}

			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="startRow">The row where the data starts (1 for the first row) or -1 for the first occupied row.</param>
			/// <param name="columns">The columns to read from.</param>
			public Loader(int startRow, params string[] columns)
				: this(columns) {
				this.startRow = startRow;
			}

			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="sheet">The worksheet to read from (1 for the first worksheet).</param>
			/// <param name="startRow">The row where the data starts (1 for the first row) or -1 for the first occupied row.</param>
			/// <param name="columns">The columns to read from.</param>
			public Loader(int sheet, int startRow, params string[] columns)
				: this(startRow, columns) {
				this.sheet = sheet;
			}

			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="sheet">The worksheet to read from (1 for the first worksheet).</param>
			/// <param name="startRow">The row where the data starts (1 for the first row) or -1 for the first occupied row.</param>
			/// <param name="endRow">The row where the data ends (1 for the first row) or -1 for the last occupied row.</param>
			/// <param name="columns">The columns to read from.</param>
			public Loader(int sheet, int startRow, int endRow, params string[] columns)
				: this(sheet, startRow, columns) {
				this.endRow = endRow;
			}

			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="sheet">The worksheet to read from (1 for the first worksheet).</param>
			/// <param name="startRow">The row where the data starts (1 for the first row) or -1 for the first occupied row.</param>
			/// <param name="endRow">The row where the data ends (1 for the first row) or -1 for the last occupied row.</param>
			/// <param name="columns">The columns to read from.</param>
			public Loader(int sheet, int startRow, int endRow, params int[] columns)
				: this() {
				this.sheet = sheet;
				this.startRow = startRow;
				this.endRow = endRow;
				icolumns = columns;
			}

			/// <summary>
			/// Copies the Loader from another Loader.
			/// </summary>
			public void CopyFrom(Loader src) {
				base.CopyFrom(src);
				sheet = src.sheet;
				startRow = src.startRow;
				endRow = src.endRow;
				icolumns = src.icolumns;
				scolumns = src.scolumns;
			}

			/// <summary>
			/// Loads the <see cref="DataItem"/> from a <see cref="Stream"/>.
			/// </summary>
			public override void Load(Stream stream) {
				DataReader r;
				if (icolumns == null) r = Excel.Reader(stream, sheet, startRow, endRow, icolumns);
				else r = Excel.Reader(stream, sheet, startRow, endRow, scolumns);
				Parent.CopyFrom(r);
				r.Dispose();
			}

			/// <summary>
			/// Creates a copy of the Loader.
			/// </summary>
			public override Item Clone() {
				Loader l = new Loader();
				l.CopyFrom(this);
				return l;
			}

			/// <summary>
			/// Always returns null, because this <see cref="Item"/> needs not to be compiled.
			/// </summary>
			public override string GetSource() { return null; }

			/// <summary>
			/// Does nothing and always returns true, because this <see cref="Item"/> needs not to be compiled.
			/// </summary>
			public override bool Compile() { return true; }
		}
		/// <summary>
		/// Converts row and column numbers to Excel cell names, e.g. 3,4 to "D3" or 5,6 to "F6".
		/// </summary>
		public static string CellName(int row, int col) {
			string name = "";
			col--;

			while (col > 26) {
				name = (char)('A' + col%26) + name;
				col /= 26;
			}
			return name + row.ToString();
		}
		/// <summary>
		/// Parses an Excel cell value to a double value.
		/// </summary>
		public static bool Parse(object val, out double x) {
			x = double.NaN;
			if (val is string) return !double.TryParse((string)val, out x);
			else if (val is byte) x = (double)(byte)val;
			else if (val is sbyte) x = (double)(sbyte)val;
			else if (val is short) x = (double)(short)val;
			else if (val is ushort) x = (double)(ushort)val;
			else if (val is int) x = (double)(int)val;
			else if (val is uint) x = (double)(uint)val;
			else if (val is long) x = (double)(long)val;
			else if (val is ulong) x = (double)(ulong)val;
			else if (val is float) x = (double)(float)val;
			else if (val is double) x = (double)val;
			else if (val is decimal) x = (double)(decimal)val;
			else return false;
			return true;
		}

		/// <summary>
		/// Gets a <see cref="DataReader"/> that reads from an Excel file stream. Used internally.
		/// </summary>
		public virtual DataReader GetReader(Stream stream, int sheet, int startRow, int endRow, params int[] columns) {
			throw new NotSupportedException("Reading of Excel files is not supported. In order to enable reading of Excel data, " +
				"reference JohnsHope.FPlot.Excel.dll and purchase NativeExcel.dll at www.nika-soft.com");
		}

		/// <summary>
		/// Gets a <see cref="DataReader"/> that reads from an Excel file stream. Used internally.
		/// </summary>
		public virtual DataReader GetReader(Stream stream, int sheet, int startRow, int endRow, params string[] columns) {
			throw new NotSupportedException("Reading of Excel files is not supported. In order to enable reading of Excel data, " +
				"reference JohnsHope.FPlot.Excel.dll and purchase NativeExcel.dll at www.nika-soft.com");
		}

		/// <summary>
		/// Gets a <see cref="DataReader"/> that reads from an Excel <see cref="Stream"/>. The numbers are entered into the list row by row.
		/// In order to enable reading of Excel data, reference JohnsHope.FPlot.Excel.dll wich uses NativeExcel.dll.
		/// You can purchase NativeExcel.dll at <a href="http://www.nika-soft.com">www.nika-soft.com</a>
		/// </summary>
		/// <param name="stream">The Excel <see cref="Stream"/>.</param>
		/// <param name="sheet">The Excel worksheet to read from. (1 for the first worksheet)</param>
		/// <param name="startRow">The row where the number data starts. (1 for the first row)</param>
		/// <param name="endRow">The row where the number data ends. (1 for the first row)</param>
		/// <param name="columns">The columns that contain the number data</param>
		/// <returns></returns>
		public static DataReader Reader(Stream stream, int sheet, int startRow, int endRow, params int[] columns) {
			return Implementation.GetReader(stream, sheet, startRow, endRow, columns);
		}

		/// <summary>
		/// Gets a <see cref="DataReader"/> that reads from an Excel <see cref="Stream"/>. The numbers are entered into the list row by row.
		/// In order to enable reading of Excel data, reference JohnsHope.FPlot.Excel.dll wich uses NativeExcel.dll.
		/// You can purchase NativeExcel.dll at <a href="http://www.nika-soft.com">www.nika-soft.com</a>
		/// </summary>
		/// <param name="stream">The Excel <see cref="Stream"/>.</param>
		/// <param name="sheet">The Excel worksheet to read from. (1 for the first worksheet)</param>
		/// <param name="startRow">The row where the number data starts. (1 for the first row)</param>
		/// <param name="endRow">The row where the number data ends. (1 for the first row)</param>
		/// <param name="columns">The columns that contain the number data</param>
		/// <returns></returns>
		public static DataReader Reader(Stream stream, int sheet, int startRow, int endRow, params string[] columns) {
			return Implementation.GetReader(stream, sheet, startRow, endRow, columns);
		}

		/// <summary>
		/// Gets a <see cref="DataReader"/> that reads from an Excel <see cref="Stream"/>. The numbers are entered into the list row by row.
		/// In order to enable reading of Excel data, reference JohnsHope.FPlot.Excel.dll wich uses NativeExcel.dll.
		/// You can purchase NativeExcel.dll at <a href="http://www.nika-soft.com">www.nika-soft.com</a>
		/// </summary>
		/// <param name="stream">The Excel <see cref="Stream"/>.</param>
		/// <param name="sheet">The Excel worksheet to read from. (1 for the first worksheet)</param>
		/// <param name="startRow">The row where the number data starts. (1 for the first row)</param>
		/// <param name="columns">The columns that contain the number data</param>
		/// <returns></returns>
		public static DataReader Reader(Stream stream, int sheet, int startRow, params string[] columns) {
			return Implementation.GetReader(stream, sheet, startRow, -1, columns);
		}

		/// <summary>
		/// Gets a <see cref="DataReader"/> that reads from an Excel <see cref="Stream"/>. The numbers are entered into the list row by row.
		/// In order to enable reading of Excel data, reference JohnsHope.FPlot.Excel.dll wich uses NativeExcel.dll.
		/// You can purchase NativeExcel.dll at <a href="http://www.nika-soft.com">www.nika-soft.com</a>
		/// </summary>
		/// <param name="stream">The Excel <see cref="Stream"/>.</param>
		/// <param name="startRow">The row where the number data starts. (1 for the first row)</param>
		/// <param name="columns">The columns that contain the number data</param>
		/// <returns></returns>
		public static DataReader Reader(Stream stream, int startRow, params string[] columns) {
			return Implementation.GetReader(stream, 1, startRow, -1, columns);
		}

		/// <summary>
		/// Gets a <see cref="DataReader"/> that reads from an Excel <see cref="Stream"/>. The numbers are entered into the list row by row.
		/// In order to enable reading of Excel data, reference JohnsHope.FPlot.Excel.dll wich uses NativeExcel.dll.
		/// You can purchase NativeExcel.dll at <a href="http://www.nika-soft.com">www.nika-soft.com</a>
		/// </summary>
		/// <param name="stream">The Excel <see cref="Stream"/>.</param>
		/// <param name="columns">The columns that contain the number data</param>
		/// <returns></returns>
		public static DataReader Reader(Stream stream, params string[] columns) {
			return Implementation.GetReader(stream, 1, -1, -1, columns);
		}

		/// <summary>
		/// Reads data from an Excel <see cref="Stream"/>. The numbers are entered into the list row by row.
		/// In order to enable reading of Excel data, reference JohnsHope.FPlot.Excel.dll wich uses NativeExcel.dll.
		/// You can purchase NativeExcel.dll at <a href="http://www.nika-soft.com">www.nika-soft.com</a>
		/// </summary>
		/// <param name="stream">The Excel <see cref="Stream"/>.</param>
		/// <param name="sheet">The Excel worksheet to read from. (1 for the first worksheet)</param>
		/// <param name="startRow">The row where the number data starts. (1 for the first row)</param>
		/// <param name="endRow">The row where the number data ends. (1 for the first row)</param>
		/// <param name="columns">The columns that contain the number data</param>
		/// <returns>Returns the numbers read in a <see cref="BigData"/> object.</returns>
		public static BigData Data(Stream stream, int sheet, int startRow, int endRow, params int[] columns) {
			return new BigData(Reader(stream, sheet, startRow, endRow, columns));
		}

		/// <summary>
		/// Reads Data from an Excel <see cref="Stream"/>. The numbers are entered into the list row by row.
		/// In order to enable reading of Excel data, reference JohnsHope.FPlot.Excel.dll wich uses NativeExcel.dll.
		/// You can purchase NativeExcel.dll at <a href="http://www.nika-soft.com">www.nika-soft.com</a>
		/// </summary>
		/// <param name="stream">The Excel <see cref="Stream"/>.</param>
		/// <param name="sheet">The Excel worksheet to read from. (1 for the first worksheet)</param>
		/// <param name="startRow">The row where the number data starts. (1 for the first row)</param>
		/// <param name="endRow">The row where the number data ends. (1 for the first row)</param>
		/// <param name="columns">The columns that contain the number data</param>
		/// <returns>Returns the numbers read in a <see cref="BigData"/> object.</returns>
		public static BigData Data(Stream stream, int sheet, int startRow, int endRow, params string[] columns) {
			return new BigData(Reader(stream, sheet, startRow, endRow, columns));
		}

		/// <summary>
		/// Reads Data from an Excel <see cref="Stream"/>. The numbers are entered into the list row by row.
		/// In order to enable reading of Excel data, reference JohnsHope.FPlot.Excel.dll wich uses NativeExcel.dll.
		/// You can purchase NativeExcel.dll at <a href="http://www.nika-soft.com">www.nika-soft.com</a>
		/// </summary>
		/// <param name="stream">The Excel <see cref="Stream"/>.</param>
		/// <param name="sheet">The Excel worksheet to read from. (1 for the first worksheet)</param>
		/// <param name="startRow">The row where the number data starts. (1 for the first row)</param>
		/// <param name="columns">The columns that contain the number data</param>
		/// <returns>Returns the numbers read in a <see cref="BigData"/> object.</returns>
		public static BigData Data(Stream stream, int sheet, int startRow, params string[] columns) {
			return new BigData(Reader(stream, sheet, startRow, columns));
		}

		/// <summary>
		/// Reads Data from an Excel <see cref="Stream"/>. The numbers are entered into the list row by row.
		/// In order to enable reading of Excel data, reference JohnsHope.FPlot.Excel.dll wich uses NativeExcel.dll.
		/// You can purchase NativeExcel.dll at <a href="http://www.nika-soft.com">www.nika-soft.com</a>
		/// </summary>
		/// <param name="stream">The Excel <see cref="Stream"/>.</param>
		/// <param name="startRow">The row where the number data starts. (1 for the first row)</param>
		/// <param name="columns">The columns that contain the number data</param>
		/// <returns>Returns the numbers read in a <see cref="BigData"/> object.</returns>
		public static BigData Data(Stream stream, int startRow, params string[] columns) { return Data(stream, 1, startRow, columns); }

		/// <summary>
		/// Reads Data from an Excel <see cref="Stream"/>. The numbers are entered into the list row by row.
		/// In order to enable reading of Excel data, reference JohnsHope.FPlot.Excel.dll wich uses NativeExcel.dll.
		/// You can purchase NativeExcel.dll at <a href="http://www.nika-soft.com">www.nika-soft.com</a>
		/// </summary>
		/// <param name="stream">The Excel <see cref="Stream"/>.</param>
		/// <param name="columns">The columns that contain the number data</param>
		/// <returns>Returns the numbers read in a <see cref="BigData"/> object.</returns>
		public static BigData Data(Stream stream, params string[] columns) { return Data(stream, 1, 1, columns); }

	}

}
