using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NativeExcel;
using JohnsHope.FPlot.Library;

namespace JohnsHope.FPlot.AddIns {

	/// <summary>
	/// An implementation of the <see cref="JohnsHope.FPlot.Library.Excel"/> class, that implements reading of Excel files.
	/// </summary>
	public class Excel: JohnsHope.FPlot.Library.Excel {

		/// <summary>
		/// A class that implements an <see cref="IEnumerator{T}">IEnumerator&lt;double&gt;</see> that reads from a
		/// <see cref="StreamReader"/>.
		/// </summary>
		public new class Enumerator: JohnsHope.FPlot.Library.Excel.Enumerator, IEnumerator {

			Stream stream;
			long start;
			IWorkbook book;
			IWorksheet sheet;
			int startRow, endRow, sheetNo, row, col, cols;
			string[] scolumns = null;
			int[] icolumns = null;
			double x = double.NaN;
	
			/// <summary>
			/// A constructor that initializes the Enumerator from a Excel <see cref="Stream"/>.
			/// </summary>
			/// <param name="stream">The <see cref="Stream"/> to read the Excel data from.</param>
			/// <param name="sheet">The worksheet to read from (1 for the first worksheet).</param>
			/// <param name="startRow">The row where the data starts (1 for the first row) or -1 for the first occupied row.</param>
			/// <param name="endRow">The row where the data ends (1 for the first row) or -1 for the last occupied row.</param>
			/// <param name="columns">An array of int values denoting the columns to read the data from.</param>
			public Enumerator(Stream stream, int sheet, int startRow, int endRow, params int[] columns) {
				sheetNo = sheet;
				start = stream.Position;
				this.startRow = startRow;
				this.endRow = endRow;
				icolumns = columns;

				Reset(stream);
			}

			/// <summary>
			/// A constructor that initializes the Enumerator from a Excel <see cref="Stream"/>.
			/// </summary>
			/// <param name="stream">The <see cref="Stream"/> to read the Excel data from.</param>
			/// <param name="sheet">The worksheet to read from (1 for the first worksheet).</param>
			/// <param name="startRow">The row where the data starts (1 for the first row) or -1 for the first occupied row.</param>
			/// <param name="endRow">The row where the data ends (1 for the first row) or -1 for the last occupied row.</param>
			/// <param name="columns">An array of strings denoting the column names of the columns where to read the data from.</param>
			public Enumerator(Stream stream, int sheet, int startRow, int endRow, params string[] columns) {
				sheetNo = sheet;
				start = stream.Position;
				this.startRow = startRow;
				this.endRow = endRow;
				scolumns = columns;

				Reset(stream);
			}

			/// <summary>
			/// The current value.
			/// </summary>
			public double Current { get { return x; } }
			object IEnumerator.Current { get { return x; } }

			/// <summary>
			/// Disposes the Enumerator.
			/// </summary>
			public void Dispose() {
				endRow = -1;
				row = 0;
				book = null;
				sheet = null;
			}

			/// <summary>
			/// Moves to the next value.
			/// </summary>
			/// <returns>Returns false if there is no value available.</returns>
			public bool MoveNext() {
				if (row > endRow) {
					x = double.NaN;
					return false;
				}

				object val;
				if (icolumns != null) val = sheet.Cells[row, icolumns[col]].Value;
				else val = sheet.Cells[scolumns[col] + row.ToString()].Value;

				if (++col >= cols) {
					col = 0;
					row++;
				}

				if (!Excel.Parse(val, out x)) throw new ArgumentException("Cannot convert Cell content to double, at cell " +
					Excel.CellName(row, col));
				
				return true;
			}

			private void Reset(Stream s) {
				stream = s;
				book = NativeExcel.Factory.OpenWorkbook(s);
				if (book != null) {
					if (sheetNo < 1 || sheetNo > book.Worksheets.Count) throw new ArgumentException("Invalid Worksheet " + sheet);

					sheet = book.Worksheets[sheetNo];

					if (scolumns == null) cols = icolumns.Length;
					else cols = scolumns.Length;

					if (startRow == -1) startRow = sheet.UsedRange.Row;
					if (endRow == -1) endRow = sheet.UsedRange.Row + sheet.UsedRange.Rows.Count - 1;
					
					row = startRow;
					col = 0;
					if (endRow < startRow) throw new ArgumentException("You must specify a valid Cell range");
				} else {
					endRow = -1;
					col = row = 0;
					sheet = null;
					throw new ArgumentException("Not a valid Excel file");
				}
				x = double.NaN;
			}

			/// <summary>
			/// Throws a <see cref="NotSupportedException"/>. 
			/// </summary>
			public void Reset() {
				stream.Position = start;
				Reset(stream);
			}
		}

		/// <summary>
		/// A <see cref="DataReader"/> class that implements reading numbers from a <see cref="StreamReader"/>.
		/// </summary>
		public new class DataReader: JohnsHope.FPlot.Library.Excel.DataReader {
			/// <summary>
			/// A constructor that initializes the DataReader from an Excel <see cref="Stream"/>.
			/// </summary>
			/// <param name="stream">The <see cref="Stream"/> to read the Excel data from.</param>
			/// <param name="sheet">The worksheet to read from (1 for the first worksheet).</param>
			/// <param name="startRow">The row where the data starts (1 for the first row) or -1 for the first occupied row.</param>
			/// <param name="endRow">The row where the data ends (1 for the first row) or -1 for the last occupied row.</param>
			/// <param name="columns">An array of int values denoting the columns to read the data from.</param>
			public DataReader(Stream stream, int sheet, int startRow, int endRow, params int[] columns):
				base(new Enumerator(stream, sheet, startRow, endRow, columns)) { }

			/// <summary>
			/// A constructor that initializes the DataReader from an Excel <see cref="Stream"/>.
			/// </summary>
			/// <param name="stream">The <see cref="Stream"/> to read the Excel data from.</param>
			/// <param name="sheet">The worksheet to read from (1 for the first worksheet).</param>
			/// <param name="startRow">The row where the data starts (1 for the first row) or -1 for the first occupied row.</param>
			/// <param name="endRow">The row where the data ends (1 for the first row) or -1 for the last occupied row.</param>
			/// <param name="columns">An array of strings denoting the column names of the columns where to read the data from.</param>
			public DataReader(Stream stream, int sheet, int startRow, int endRow, params string[] columns):
				base(new Enumerator(stream, sheet, startRow, endRow, columns)) { }
		}

		/// <summary>
		/// Returns a DataReader to read from an Excel <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read the Excel data from.</param>
		/// <param name="sheet">The worksheet to read from (1 for the first worksheet).</param>
		/// <param name="startRow">The row where the data starts (1 for the first row) or -1 for the first occupied row.</param>
		/// <param name="endRow">The row where the data ends (1 for the first row) or -1 for the last occupied row.</param>
		/// <param name="columns">An array of int values denoting the columns to read the data from.</param>
		public override JohnsHope.FPlot.Library.Excel.DataReader GetReader(Stream stream, int sheet, int startRow, int endRow, params int[] columns) {
			return new DataReader(stream, sheet, startRow, endRow, columns);
		}

		/// <summary>
		/// Returns a DataReader to read from an Excel <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read the Excel data from.</param>
		/// <param name="sheet">The worksheet to read from (1 for the first worksheet).</param>
		/// <param name="startRow">The row where the data starts (1 for the first row) or -1 for the first occupied row.</param>
		/// <param name="endRow">The row where the data ends (1 for the first row) or -1 for the last occupied row.</param>
		/// <param name="columns">An array of strings denoting the column names of the columns where to read the data from.</param>
		public override JohnsHope.FPlot.Library.Excel.DataReader GetReader(Stream stream, int sheet, int startRow, int endRow, params string[] columns) {
			return new DataReader(stream, sheet, startRow, endRow, columns);
		}

		/// <summary>
		/// Initializes the static variable <see cref="JohnsHope.FPlot.Library.Excel.Implementation"/> to a JohnsHope.FPlot.AddIns.Excel
		/// instance.
		/// </summary>
		public static void Init() {
			JohnsHope.FPlot.Library.Excel.Implementation = new Excel();
		}

	}
}
