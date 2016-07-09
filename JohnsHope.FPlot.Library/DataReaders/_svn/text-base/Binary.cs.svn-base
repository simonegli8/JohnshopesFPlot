using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JohnsHope.FPlot.Library {

	/// <summary>
	/// A class that implements reading of numbers from binary Streams.
	/// </summary>
	public class Binary {

		/// <summary>
		/// A class that implements a <see cref="IEnumerator{T}">IEnumerator&lt;double&gt;</see> that reads from a <see cref="Stream"/>.
		/// </summary>
		public class Enumerator: IEnumerator<double> {
			const int M = 1024;

			enum NumericType { Byte, SByte, Short, UShort, Int, UInt, Long, ULong, Float, Double }

			bool swap;
			NumericType type;
			Stream s;
			int size, pos = 0, N = 0;
			long start, length;
			double x = double.NaN;
			byte[] buf = null;

			/// <summary>
			/// Constructor that initializes a Binary.Enumerator on a <see cref="Stream"/>.
			/// </summary>
			/// <param name="stream">The <see cref="Stream"/> to read from.</param>
			/// <param name="length">The maximum number of bytes to read.</param>
			/// <param name="type">The binary number data type.</param>
			/// <param name="bigEndian">If true, numbers are stored in big endian format in the <see cref="Stream"/>.</param>
			public Enumerator(Stream stream, long length, Type type, bool bigEndian) {
				s = stream;
				this.length = length;
				start = s.Position;
				if (type == typeof(byte)) {
					this.type = NumericType.Byte; size = 1;
				} else if (type == typeof(sbyte)) {
					this.type = NumericType.SByte; size = 1;
				} else if (type == typeof(short)) {
					this.type = NumericType.Short; size = 2;
				} else if (type == typeof(ushort)) {
					this.type = NumericType.UShort; size = 2;
				} else if (type == typeof(int)) {
					this.type = NumericType.Int; size = 4;
				} else if (type == typeof(uint)) {
					this.type = NumericType.UInt; size = 4;
				} else if (type == typeof(long)) {
					this.type = NumericType.Long; size = 8;
				} else if (type == typeof(ulong)) {
					this.type = NumericType.ULong; size = 8;
				} else if (type == typeof(float)) {
					this.type = NumericType.Float; size = 4;
				} else if (type == typeof(double)) {
					this.type = NumericType.Double; size = 8;
				} else throw new ArgumentException("Type not supported.");
				swap = BitConverter.IsLittleEndian == bigEndian;
			}

			/// <summary>
			/// Constructor that initializes a Binray.Enumerator on a <see cref="Stream"/>.
			/// </summary>
			/// <param name="stream">The Stream to read from.</param>
			/// <param name="type">The binary number data type.</param>
			/// <param name="bigEndian">If true, numbers are stored in big endian format in the stream.</param>
			public Enumerator(Stream stream, Type type, bool bigEndian) : this(stream, long.MaxValue, type, bigEndian) { }

			/// <summary>
			/// Constructor that initializes a Binary.Enumerator on a <see cref="Stream"/>.
			/// </summary>
			/// <param name="stream">The Stream to read from.</param>
			/// <param name="length">The maximum number of bytes to read.</param>
			/// <param name="type">The binary number data type.</param>
			public Enumerator(Stream stream, long length, Type type) : this(stream, length, type, false) { }

			/// <summary>
			/// Constructor that initializes a Binary.Enumerator on a <see cref="Stream"/>.
			/// </summary>
			/// <param name="stream">The Stream to read from.</param>
			/// <param name="type">The binary number data type.</param>
			public Enumerator(Stream stream, Type type) : this(stream, long.MaxValue, type, false) { }

			/// <summary>
			/// Returns the current double value.
			/// </summary>
			public double Current { get { return x; } }
			object IEnumerator.Current { get { return x; } }

			/// <summary>
			/// Disposes the enumerator.
			/// </summary>
			public void Dispose() { buf = null; pos = N = 0; x = double.NaN; }

			/// <summary>
			/// Moves to the next value as with IEnumerator&lt;double&gt; and returns true if there is a value available.
			/// </summary>
			public bool MoveNext() {

				// read buffer if pos is at end of buffer
				if (pos >= N) {
					x = double.NaN;
					if (N != M && N != 0) return false; // the last buffer read readed less that M bytes, so the Stream reached its end.

					// read buffer
					long n = Math.Max(M, start + length - s.Position);
					if (buf == null) buf = new byte[M];
					if (n > 0) N = s.Read(buf, 0, Math.Min(M, (int)n));
					else N = 0;
					pos = 0;
					if (N == 0) return false; // the buffer read read 0 bytes.

					if (swap) { // swap byte ordering
						for (int i = size - 1; 2*i >= size; i--) {
							for (int p = 0, p1 = i; p1 < N; p += size) {
								byte t = buf[p];
								buf[p] = buf[p1];
								buf[p1] = t;
								p1 += size;
							}
						}
					}
				}

				switch (type) {
				case NumericType.Byte:
					x = (double)buf[pos++];
					return true;
				case NumericType.SByte:
					if (buf[pos] > 127) x = (double)(buf[pos++] - 256);
					else x = (double)buf[pos++];
					return true;
				case NumericType.Short:
					x = (double)BitConverter.ToInt16(buf, pos);
					pos += 2;
					return true;
				case NumericType.UShort:
					x = (double)BitConverter.ToUInt16(buf, pos);
					pos += 2;
					return true;
				case NumericType.Int:
					x = (double)BitConverter.ToInt32(buf, pos);
					pos += 4;
					return true;
				case NumericType.UInt:
					x = (double)BitConverter.ToUInt32(buf, pos);
					pos += 4;
					return true;
				case NumericType.Long:
					x = (double)BitConverter.ToInt64(buf, pos);
					pos += 8;
					return true;
				case NumericType.ULong:
					x = (double)BitConverter.ToUInt64(buf, pos);
					pos += 8;
					return true;
				case NumericType.Float:
					x = (double)BitConverter.ToSingle(buf, pos);
					pos += 4;
					return true;
				case NumericType.Double:
					x = (double)BitConverter.ToDouble(buf, pos);
					pos += 8;
					return true;
				}
				return false;
			}

			/// <summary>
			/// Resets the enumerator to the beginning.
			/// </summary>
			public void Reset() { s.Position = start; buf = null; pos = N = 0; x = double.NaN; }
		}

		/// <summary>
		/// A <see cref="DataReader"/> class that implements reading from a <see cref="Stream"/> of binary number values.
		/// </summary>
		public class DataReader: JohnsHope.FPlot.Library.DataReader {
			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="stream">The <see cref="Stream"/> to read from.</param>
			/// <param name="length">The maximum number of bytes to read.</param>
			/// <param name="type">The number type.</param>
			/// <param name="bigEndian">Set to true if the numbers are stored in big endian format in the <see cref="Stream"/>.</param>
			public DataReader(Stream stream, long length, Type type, bool bigEndian) :
				base(new Enumerator(stream, length, type, bigEndian)) { }
			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="stream">The <see cref="Stream"/> to read from.</param>
			/// <param name="length">The maximum number of bytes to read.</param>
			/// <param name="type">The number type.</param>
			public DataReader(Stream stream, long length, Type type) : base(new Enumerator(stream, length, type, false)) { }
			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="stream">The <see cref="Stream"/> to read from.</param>
			/// <param name="type">The number type.</param>
			/// <param name="bigEndian">Set to true if the numbers are stored in big endian format in the <see cref="Stream"/>.</param>
			public DataReader(Stream stream, Type type, bool bigEndian) :
				base(new Enumerator(stream, long.MaxValue, type, bigEndian)) { }
			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="stream">The <see cref="Stream"/> to read from.</param>
			/// <param name="type">The number type.</param>
			public DataReader(Stream stream, Type type) : base(new Enumerator(stream, long.MaxValue, type, false)) { }
		}

		/// <summary>
		/// A <see cref="DataItem.Instance"/> class that implements fast loading of text data. This class is used internally
		/// by the method <see cref="DataItem.LoadBinary(string, System.Type)"/>.
		/// </summary>
		public class Loader: DataItem.Instance {

			Type type;
			bool bigEndian;
			long length;

			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="type">The binary number type of the numbers in the underlying <see cref="Stream"/>.</param>
			public Loader(Type type) {
				this.type = type;
				bigEndian = false;
				length = long.MaxValue;
			}

			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="type">The binary number type of the numbers in the underlying <see cref="Stream"/>.</param>
			/// <param name="bigEndian">If true, the numbers will be read in big endian format.</param>
			public Loader(Type type, bool bigEndian)
				: this(type) {
				this.bigEndian = bigEndian;
			}

			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="type">The binary number type of the numbers in the underlying <see cref="Stream"/>.</param>
			/// <param name="length">The maximum number of bytes to read.</param>
			/// <param name="bigEndian">If true, the numbers will be read in big endian format.</param>
			public Loader(Type type, long length, bool bigEndian)
				: this(type) {
				this.length = length;
				this.bigEndian = bigEndian;
			}

			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="type">The binary number type of the numbers in the underlying <see cref="Stream"/>.</param>
			/// <param name="length">The maximum number of bytes to read.</param>
			public Loader(Type type, long length)
				: this(type) {
				this.length = length;
			}

			/// <summary>
			/// Gets or sets the <see cref="Type"/> used by the Loader to read binary numbers.
			/// </summary>
			public Type Type { get { return type; } set { type = value; } }

			/// <summary>
			/// Gets or sets the maximum bytes read by the Loader.
			/// </summary>
			public long ReadLength { get { return length; } set { length = value; } }

			/// <summary>
			/// Gets or sets the endian format of the numbers read by the Loader.
			/// </summary>
			public bool BigEndian { get { return bigEndian; } set { bigEndian = value; } }

			/// <summary>
			/// Copies the Loader from another Loader.
			/// </summary>
			public void CopyFrom(Loader src) {
				base.CopyFrom(src);
				type = src.type;
				length = src.length;
				bigEndian = src.bigEndian;
			}

			/// <summary>
			/// Loads the <see cref="DataItem"/> from a <see cref="Stream"/>.
			/// </summary>
			public override void Load(Stream stream) {
				Parent.CopyFrom(Binary.Reader(stream, length, type, bigEndian));
			}

			/// <summary>
			/// Creates a copy of the Loader.
			/// </summary>
			public override Item Clone() {
				Loader l = new Loader(type);
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
		/// Returns a <see cref="DataReader">Binary.DataReader</see> that reads from the given <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <param name="length">The maximum number of bytes to read.</param>
		/// <param name="type">The number type.</param>
		/// <param name="bigEndian">Set to true if the numbers are stored in big endian format in the <see cref="Stream"/>.</param>
		public static DataReader Reader(Stream stream, long length, Type type, bool bigEndian) {
			return new DataReader(stream, length, type, bigEndian);
		}

		/// <summary>
		/// Returns a <see cref="DataReader">Binary.DataReader</see> that reads from the given <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <param name="length">The maximum number of bytes to read.</param>
		/// <param name="type">The number type.</param>
		public static DataReader Reader(Stream stream, long length, Type type) {
			return new DataReader(stream, length, type, false);
		}

		/// <summary>
		/// Returns a <see cref="DataReader">Binary.DataReader</see> that reads from the given <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <param name="type">The number type.</param>
		/// <param name="bigEndian">Set to true if the numbers are stored in big endian format in the <see cref="Stream"/>.</param>
		public static DataReader Reader(Stream stream, Type type, bool bigEndian) {
			return new DataReader(stream, long.MaxValue, type, bigEndian);
		}

		/// <summary>
		/// Returns a <see cref="DataReader">Binary.DataReader</see> that reads from the given <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <param name="type">The number type.</param>
		public static DataReader Reader(Stream stream, Type type) {
			return new DataReader(stream, long.MaxValue, type, false);
		}

		/// <summary>
		/// Returns a BigData object that contains the binary numbers from the given <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <param name="length">The maximum number of bytes to read.</param>
		/// <param name="type">The number type.</param>
		/// <param name="bigEndian">Set to true if the numbers are stored in big endian format in the <see cref="Stream"/>.</param>
		public static BigData Data(Stream stream, long length, Type type, bool bigEndian) {
			return new BigData(new DataReader(stream, length, type, bigEndian));
		}

		/// <summary>
		/// Returns a BigData object that contains the binary numbers from the given <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <param name="length">The maximum number of bytes to read.</param>
		/// <param name="type">The number type.</param>
		public static BigData Data(Stream stream, long length, Type type) {
			return new BigData(new DataReader(stream, length, type));
		}

		/// <summary>
		/// Returns a BigData object that contains the binary numbers from the given <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <param name="type">The number type.</param>
		/// <param name="bigEndian">Set to true if the numbers are stored in big endian format in the <see cref="Stream"/>.</param>
		public static BigData Data(Stream stream, Type type, bool bigEndian) {
			return new BigData(new DataReader(stream, type, bigEndian));
		}

		/// <summary>
		/// Returns a BigData object that contains the binary numbers from the given <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <param name="type">The number type.</param>
		public static BigData Data(Stream stream, Type type) {
			return new BigData(new DataReader(stream, type));
		}
	}
}
