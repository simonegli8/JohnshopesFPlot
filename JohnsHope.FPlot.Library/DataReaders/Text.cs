using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;

namespace JohnsHope.FPlot.Library {

	/// <summary>
	/// A class that implements reading of numbers from <see cref="StreamReader"/>s or text <see cref="Stream"/>s.
	/// </summary>
	public class Text {

		/// <summary>
		/// A class that implements an <see cref="IEnumerator{T}">IEnumerator&lt;double&gt;</see> that reads from a <see cref="StreamReader"/>.
		/// </summary>
		public class Enumerator: IEnumerator<double> {
			const int M = 1024;

			StreamReader r;
			int pos = 0, N = 0;
			double x = double.NaN;
			char[] buf = new char[M];
			char[] separators;
			string overlap = "";
			NumberFormatInfo format;

			/// <summary>
			/// A constructor that initializes the Enumerator from a <see cref="StreamReader"/>.
			/// </summary>
			/// <param name="reader">The <see cref="StreamReader"/> to read from.</param>
			/// <param name="separators">A string of separator characters, that separate individual numbers.</param>
			/// <param name="numberFormat">The number text format.</param>
			public Enumerator(StreamReader reader, string separators, NumberFormatInfo numberFormat) {
				r = reader;
				this.separators = separators.ToCharArray();
				if (numberFormat == null) format = NumberFormatInfo.InvariantInfo;
				else format = numberFormat;

			}

			/// <summary>
			/// A constructor that initializes the Enumerator from a <see cref="StreamReader"/>.
			/// </summary>
			/// <param name="reader">The <see cref="StreamReader"/> to read from.</param>
			/// <param name="separators">A string of separator characters, that separate individual numbers.</param>
			public Enumerator(StreamReader reader, string separators) : this(reader, separators, null) { }

			/// <summary>
			/// The current value.
			/// </summary>
			public double Current { get { return x; } }
			object IEnumerator.Current { get { return x; } }

			/// <summary>
			/// Disposes the Enumerator.
			/// </summary>
			public void Dispose() { r.Dispose(); }

			private bool IsSeparator(int pos) {
				char c = buf[pos];
				for (int i = 0; i < separators.Length; i++) {
					if (c == separators[i]) return true;
				}
				return false;
			}

			private string NextToken() {

				string token = null;

				do {
					// read buffer if pos is at end of tokens buffer
					while (pos >= N) {
						x = double.NaN;
						if (N != M && N != 0) {
							if (overlap == "") return null;
							// the last buffer read readed less that M bytes, so the Stream reached its end and there is no overlap.
							else {
								token = overlap;
								overlap = "";
								return token;
							}
						}
						// read char buffer
						N = r.Read(buf, 0, M);
						if (N == 0) return null; // the buffer read read 0 bytes.
						pos = 0;
					}

					if (pos == 0) {	// position is at the beginning of the buffer, include overlap.
						while (pos < N && !IsSeparator(pos)) pos++ ;
						if (pos > 0) {
							if (pos >= N) {
								overlap += new string(buf, 0, N);
								token = null;
							} else {
								token = overlap + new string(buf, 0, pos);
								overlap = "";
							}
						} else {
							token = overlap;
							overlap = "";
						}
					} else { // no overlap
						// skip leading separators
						while (pos < N && IsSeparator(pos)) pos++;
						int start = pos;
						while (pos < N && !IsSeparator(pos)) pos++;	// skip non separators
						if (pos >= N) {
							token = null;
							if (start < pos) overlap = new string(buf, start, pos - start);
							else overlap = "";
						} else {
							if (start < pos) token = new string(buf, start, pos - start);
							else token = null;
							overlap = "";
						}
					}
				} while (token == null);

				return token;
			}

			
			/// <summary>
			/// Moves to the next value.
			/// </summary>
			/// <returns>Returns false if there is no value available.</returns>
			public bool MoveNext() {
				string token = NextToken();

				if (token == null) return false;

				x = double.NaN;
				double.TryParse(token, NumberStyles.Any, format, out x);

				return true;
			}

			/// <summary>
			/// Throws a <see cref="NotSupportedException"/>. 
			/// </summary>
			public void Reset() {
				throw new NotSupportedException("IEnumerator<double>. Reset is not supported in Text.Enumerator");
			}
		}

		/// <summary>
		/// A <see cref="DataReader"/> class that implements reading numbers from a <see cref="StreamReader"/>.
		/// </summary>
		public class DataReader: JohnsHope.FPlot.Library.DataReader {
			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="reader">The <see cref="StreamReader"/> to read from.</param>
			/// <param name="separators">A string of separator characters that separate the individual numbers.</param>
			/// <param name="numberFormat">A <see cref="NumberFormatInfo"/> describing the number format.</param>
			public DataReader(StreamReader reader, string separators, NumberFormatInfo numberFormat) :
				base(new Enumerator(reader, separators, numberFormat)) { }
			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="reader">The <see cref="StreamReader"/> to read from.</param>
			/// <param name="separators">A string of separator characters that separate the individual numbers.</param>
			public DataReader(StreamReader reader, string separators) :
				base(new Enumerator(reader, separators)) { }
		}

		/// <summary>
		/// A <see cref="DataItem.Instance"/> class that implements fast loading of text data. This class is used internally
		/// by the method <see cref="DataItem.LoadText(string, string)"/>.
		/// </summary>
		public class Loader: DataItem.Instance {

			Encoding encoding;
			string separators;
			NumberFormatInfo numberFormat;

			private Loader() {
				encoding = Encoding.UTF8;
				numberFormat = NumberFormatInfo.InvariantInfo;
			}

			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="separators">A string of characters that separate the numbers</param>
			public Loader(string separators): this() {
				this.separators = separators;
			}

			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="separators">A string of characters that separate the numbers</param>
			/// <param name="numberFormat">The number text format.</param>
			public Loader(string separators, NumberFormatInfo numberFormat): this() {
				this.separators = separators;
				this.numberFormat = numberFormat;
			}

			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="encoding">The text encoding the <see cref="Stream"/> uses.</param>
			/// <param name="separators">A string of characters that separate the numbers</param>
			public Loader(string separators, Encoding encoding): this() {
				this.separators = separators;
				this.encoding = encoding;
			}

			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="encoding">The text encoding the <see cref="Stream"/> uses.</param>
			/// <param name="separators">A string of characters that separate the numbers</param>
			/// <param name="numberFormat">The number text format.</param>
			public Loader(string separators, NumberFormatInfo numberFormat, Encoding encoding) {
				this.separators = separators;
				this.numberFormat = numberFormat;
				this.encoding = encoding;
			}

			/// <summary>
			/// Gets or sets the <see cref="Encoding"/> used by the Loader to decode text <see cref="Stream"/>s.
			/// </summary>
			public Encoding Encoding { get { return encoding; } set { encoding = value; } }

			/// <summary>
			/// Gets or sets the <see cref="NumberFormatInfo"/> used by the Loader to parse numbers..
			/// </summary>
			public NumberFormatInfo NumberFormat { get { return numberFormat; } set { numberFormat = value; } }

			/// <summary>
			/// Copies the Loader from another Loader.
			/// </summary>
			public void CopyFrom(Loader src) {
				base.CopyFrom(src);
				encoding = src.encoding;
				separators = src.separators;
				numberFormat = src.numberFormat;
			}

			/// <summary>
			/// Loads the <see cref="DataItem"/> from a <see cref="Stream"/>.
			/// </summary>
			public override void Load(Stream stream) {
				using (StreamReader r = new StreamReader(stream, encoding)) {
					Parent.CopyFrom(Text.Reader(r, separators, numberFormat));
				}
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
		/// Returns a DataReader that reads from a <see cref="StreamReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="StreamReader"/> to read from.</param>
		/// <param name="separators">A string of characters that separate the numbers</param>
		/// <param name="numberFormat">The number text format.</param>
		public static DataReader Reader(StreamReader reader, string separators, NumberFormatInfo numberFormat) {
			return new DataReader(reader, separators, numberFormat);
		}

		/// <summary>
		/// Returns a DataReader that reads from a <see cref="StreamReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="StreamReader"/> to read from.</param>
		/// <param name="separators">A string of characters that separate the numbers</param>
		public static DataReader Reader(StreamReader reader, string separators) {
			return new DataReader(reader, separators);
		}

		/// <summary>
		/// Reads number data from a <see cref="StreamReader"/> and returns it in a <see cref="BigData"/> object.
		/// </summary>
		/// <param name="reader">The <see cref="StreamReader"/> to read from.</param>
		/// <param name="separators">A string of characters that separate the numbers</param>
		/// <param name="numberFormat">The number text format.</param>
		public static BigData Data(StreamReader reader, string separators, NumberFormatInfo numberFormat) {
			return new BigData(new DataReader(reader, separators, numberFormat));
		}

		/// <summary>
		/// Reads number data from a <see cref="StreamReader"/> and returns it in a <see cref="BigData"/> object.
		/// </summary>
		/// <param name="reader">The <see cref="StreamReader"/> to read from.</param>
		/// <param name="separators">A string of characters that separate the numbers</param>
		public static BigData Data(StreamReader reader, string separators) {
			return new BigData(new DataReader(reader, separators));
		}

		/// <summary>
		/// Reads number data from a text <see cref="Stream"/> and returns it in a <see cref="BigData"/> object.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <param name="encoding">The text encoding the <see cref="Stream"/> uses.</param>
		/// <param name="separators">A string of characters that separate the numbers</param>
		/// <param name="numberFormat">The number text format.</param>
		public static BigData Data(Stream stream, Encoding encoding, string separators, NumberFormatInfo numberFormat) {
			using (StreamReader reader = new StreamReader(stream, encoding)) {
				return Data(reader, separators, numberFormat);
			}
		}

		/// <summary>
		/// Reads number data from a text <see cref="Stream"/> and returns it in a <see cref="BigData"/> object.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <param name="encoding">The text encoding the <see cref="Stream"/> uses.</param>
		/// <param name="separators">A string of characters that separate the numbers</param>
		public static BigData Data(Stream stream, Encoding encoding, string separators) {
			using (StreamReader reader = new StreamReader(stream, encoding)) {
				return Data(reader, separators);
			}
		}

		/// <summary>
		/// Reads number data from a text <see cref="Stream"/> and returns it in a <see cref="BigData"/> object.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <param name="separators">A string of characters that separate the numbers</param>
		/// <param name="numberFormat">The number text format.</param>
		public static BigData Data(Stream stream, string separators, NumberFormatInfo numberFormat) {
			using (StreamReader reader = new StreamReader(stream)) {
				return Data(reader, separators, numberFormat);
			}
		}

		/// <summary>
		/// Reads number data from a text <see cref="Stream"/> and returns it in a <see cref="BigData"/> object.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <param name="separators">A string of characters that separate the numbers</param>
		public static BigData Data(Stream stream, string separators) {
			using (StreamReader reader = new StreamReader(stream)) {
				return Data(reader, separators);
			}
		}

		/// <summary>
		/// Returns a source code string that creates the specified encoding.
		/// </summary>
		public static string EncodingSource(Encoding encoding) {
			if (encoding == Encoding.Unicode) return "Encoding.Unicode";
			else if (encoding == null || encoding == Encoding.UTF8) return "Encoding.UTF8";
			else if (encoding == Encoding.UTF7) return "Encoding.UTF7";
			else if (encoding == Encoding.UTF32) return "Encoding.UTF32";
			else if (encoding == Encoding.ASCII) return "Encoding.ASCII";
			else if (encoding == Encoding.BigEndianUnicode) return "Encoding.BigEndianUnicode";
			else return "Encoding.GetEncoding(\"" + encoding.WebName + "\")";
		}

	}

}
