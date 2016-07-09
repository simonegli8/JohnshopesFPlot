using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Globalization;




namespace JohnsHope.FPlot.Library {

	/// <summary>
	/// Base class of all Data implementations. Data is basically an array of double values.
	/// The array can be set to check indices, or to automatically adapt its size upon access.
	/// </summary>
	[Serializable]
	public class Data: IList<double>, ICloneable {

		/// <summary>
		/// The <see cref="IEnumerator{T}">IEnumerator&lt;double&gt;</see> of a Data array.
		/// </summary>
		public class Enumerator:IEnumerator<double> {
			private Data d;
			private int index;
			/// <summary>
			/// The constructor of the enumerator.
			/// </summary>
			/// <param name="d"></param>
			public Enumerator(Data d) { this.d = d; }
			/// <summary>
			/// Resets the enumerator to the beginning of the list.
			/// </summary>
			public void Reset() { index = -1; }
			/// <summary>
			/// Moves to the next item.
			/// </summary>
			/// <returns></returns>
			public bool MoveNext() { index++; return index < d.Length; }
			/// <summary>
			/// Returns the current item.
			/// </summary>
			public double Current {
				get {
					if (index >= 0 && index < d.Length) return d[index];
					else return double.NaN;
				}
			}
			/// <summary>
			/// Returns the current item.
			/// </summary>
			object IEnumerator.Current {
				get {
					if (index >= 0 && index < d.Length) return d[index];
					else return double.NaN;
				}
			}
			/// <summary>
			/// Disposes the enumerator.
			/// </summary>
			public void Dispose() { }
		}

		/// <exclude/>
		protected int length = 0;
		/// <exclude/>
		protected bool autoResize = false;

		/// <exclude/>
		[NonSerialized]
		protected bool modified = true;
		/// <summary>
		/// This value indicates if the data should be copied by a deep or a shallow copy. The default value is true.
		/// </summary>
		[NonSerialized]
		public bool deepCopy = true;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public Data() { }

		/// <summary>
		/// Constructor that initializes the Data from an <see cref="IEnumerable{T}">IEnumerable&lt;double&gt;</see>.
		/// </summary>
		/// <param name="data">The <see cref="IEnumerable{T}">IEnumerable&lt;double&gt;</see> to initialize the Data with.</param>
		public Data(IEnumerable<double> data) {
			CopyFrom(data);
		}

		[OnDeserialized]
		private void Init(StreamingContext sc) {
			deepCopy = modified = true;
		}

		/// <summary>
		/// The Length of the array. This value can be set to adjust the length.
		/// </summary>
		public virtual int Length { get { return length; } set { length = value; } }
		/// <summary>
		/// Returns the number of items in the data array.
		/// </summary>
		public virtual int Count { get { return Length; } }
		/// <summary>
		/// This value indicates if the index should be checked upon access, or if the size of the array
		/// should automatically be adapted upon access. The default value is <c>false</c>.
		/// </summary>
		public virtual bool AutoResize { get { return autoResize; } set { autoResize = value; } }
		/// <summary>
		/// Indicates if the array was modified. This value is automatically set.
		/// </summary>
		public virtual bool Modified {
			get { return modified; }
			set { modified = value; }
		}
		/// <summary>
		/// array indexer.
		/// </summary>
		public virtual double this[int i] {
			get {
				if (i >= length) {
					if (autoResize) length = i+1;
					else throw new System.IndexOutOfRangeException();
				} else if (i < 0) throw new System.IndexOutOfRangeException();
				return double.NaN;
			}
			set {
				if (i < 0 || (i >= length && !autoResize)) throw new System.IndexOutOfRangeException();
				else if (i >= length) length = i+1;
				modified = true;
			}
		}
		/// <summary>
		/// The indexer with a double argument.
		/// </summary>
		public double this[double n] {
			get { return this[(int)n]; }
			set { this[(int)n] = value; }
		}
		/// <summary>
		/// Copies from another data array either with a deep or a shallow copy, depending on the 
		/// <see cref="deepCopy">deepCopy</see> value. 
		/// </summary>
		public void CopyFrom(Data d) {
			Modified = d.Modified;
			deepCopy = d.deepCopy;
			if (d.deepCopy) {
				Length = d.Length;
				for (int i = 0; i < length; i++) {
					this[i] = d[i];
				}
			} else {
				length = d.length;
			}
			autoResize = d.autoResize;
		}
		/// <summary>
		/// Copies from a <see cref="IEnumerable{T}">IEnumerable&lt;double&gt;</see>. 
		/// </summary>
		public virtual void CopyFrom(IEnumerable<double> d) {
			bool autoResize = this.autoResize;
			this.autoResize = true;
			IEnumerator<double> e = d.GetEnumerator();
			int i = 0;
			while (e.MoveNext()) this[i++] = e.Current;
			this.autoResize = autoResize;
			e.Dispose();
		}

		/// <summary>
		/// Returns either a deep or a shallow copy, depending on the 
		/// <see cref="deepCopy">deepCopy</see> value.
		/// </summary>
		public Data Clone() {
			Data d = new Data();
			d.CopyFrom(this);
			return d;
		}
		object ICloneable.Clone() { return Clone(); }
		/// <summary>
		/// Deletes the array and sets its length to zero.
		/// </summary>
		public virtual void Clear() { Length = 0; }
		/// <summary>
		/// Checks the index and throws a <see cref="System.IndexOutOfRangeException"/> if the index is out of bounds.
		/// </summary>
		protected void CheckIndex(int i) {
			if ((i < 0) || (i >= Length)) throw new System.IndexOutOfRangeException();
		}
		/// <summary>
		/// Returns an <see cref="IEnumerator{T}">IEnumerator&lt;double&gt;</see> for this Data-array.
		/// </summary>
		public IEnumerator<double> GetEnumerator() { return new Enumerator(this); }

		IEnumerator IEnumerable.GetEnumerator() { return new Enumerator(this); }
		/// <summary>
		/// Returns the index of the first occurence of x.
		/// </summary>
		public int IndexOf(double x) {
			for (int i = 0; i < Length; i++) {
				if (x == this[i]) return i;
			}
			return -1;
		}
		/// <summary>
		/// Inserts the value x at the specified index.
		/// </summary>
		public void Insert(int index, double x) {
			Length++;
			for (int i = Length; i > index; i--) {
				this[i] = this[i - 1];
			}
			this[index] = x;
		}
		/// <summary>
		/// Removes the value at the specified index.
		/// </summary>
		public void RemoveAt(int index) {
			for (int i = index + 1; i < Length; i++) {
				this[i - 1] = this[i];
			}
			Length--;
		}
		/// <summary>
		/// Adds the value x at the end of the array.
		/// </summary>
		public void Add(double x) {
			this[++Length-1] = x;
		}
		/// <summary>
		/// Returns true if the value x is contained in the array.
		/// </summary>
		public bool Contains(double x) {
			return IndexOf(x) != -1;
		}
		/// <summary>
		/// Removes the value x from the array. 
		/// </summary>
		public bool Remove(double x) {
			int i = IndexOf(x);
			if (i != -1) {
				RemoveAt(i);
				return true;
			} else return false;
		}
		/// <summary>
		/// Always returns false.
		/// </summary>
		public bool IsReadOnly { get { return false; } }
		/// <summary>
		/// Copies the data to an array.
		/// </summary>
		/// <param name="a">The array to copy to.</param>
		/// <param name="index">The index where to start copying.</param>
		public void CopyTo(double[] a, int index) {
			int i, n;
			n = a.Length-index;
			if (n > Length) n = Length;
			for (i = 0; i < n; i++) {
				a[i] = this[i];
			}
		}

		/// <summary>
		/// Reads text numbers from a <see cref="Stream"/> and returns them as a <see cref="BigData"/> object.
		/// This method is obsolete, use <see cref="Text.Data(Stream, string, NumberFormatInfo)">Text.Data</see>
		/// instead.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from</param>
		/// <param name="separators">The characters that separate the numbers</param>
		/// <param name="numberFormat">A <see cref="NumberFormatInfo"/> object that describes the number format,
		/// for example 3,141 instead of 3.141. You can get a NumberFormat object for the current culture from
		/// <see cref="NumberFormatInfo.CurrentInfo"/>.</param>
		/// <returns>A <see cref="Data"/> of double values read. Data derives from <see cref="IList{T}">IList&lt;double&gt;</see>.
		/// </returns>
		[Obsolete("Use Text.Data instead.")]
		public static Data ASCIIData(Stream stream, string separators, NumberFormatInfo numberFormat) {
			return Text.Data(stream, separators, numberFormat);

			#region old implementation
			/* old implementation
			BigData d = new BigData(); d.AutoResize = true;
			int n = 0;
			string[] tokens = reader.ReadToEnd().Split(separators.ToCharArray());
			foreach (string s in tokens) {
				try {
					if (numberFormat != null) d[n] = double.Parse(s.Trim(), numberFormat);
					else d[n] = double.Parse(s.Trim());
					n++;
				} catch { }
			}
			return d;
			*/
			#endregion
		}
		/// <summary>
		/// Reads text numbers from a <see cref="Stream"/> and returns them as a <see cref="BigData"/> object.
		/// This method is obsolete, use <see cref="Text.Data(Stream, string)">Text.Data</see> instead.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from</param>
		/// <param name="separators">The characters that separate the numbers</param>
		/// <returns>A <see cref="Data"/> of double values read. Data derives from <see cref="IList{T}">IList&lt;double&gt;</see>.
		/// </returns>
		[Obsolete("Use Text.Data instead.")]
		public static Data ASCIIData(Stream stream, string separators) { return Text.Data(stream, separators); }

		/// <summary>
		/// Converts a number to a double. If <c>BigEndian</c> is true, the byte ordering is swapped on little endian machines.
		/// </summary>
		/// <param name="data">The number to convert</param>
		/// <param name="BigEndian">Specifies if the number is in big-endian format.</param>
		public static double Convert(object data, bool BigEndian) {
			double x = double.NaN;
			Type type = data.GetType();
			try {
				if (BitConverter.IsLittleEndian == BigEndian &&
					(type == typeof(short) || type == typeof(int) || type == typeof(long) ||
					type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong))) { // Swap bytes
					byte[] b = null;

					if (type == typeof(short)) {
						b = BitConverter.GetBytes((short)data);
					} else if (type == typeof(ushort)) {
						b = BitConverter.GetBytes((ushort)data);
					} else if (type == typeof(int)) {
						b = BitConverter.GetBytes((int)data);
					} else if (type == typeof(uint)) {
						b = BitConverter.GetBytes((uint)data);
					} else if (type == typeof(long)) {
						b = BitConverter.GetBytes((long)data);
					} else if (type == typeof(ulong)) {
						b = BitConverter.GetBytes((ulong)data);
					} else if (type == typeof(byte[])) {
						b = (byte[])data;
					}

					// swap bytes
					for (int i = 0; i < b.Length / 2; i++) {
						byte t = b[i]; b[i] = b[b.Length-1-i]; b[b.Length-1-i] = t;
					}

					if (type == typeof(short)) {
						x = (double)BitConverter.ToInt16(b, 0);
					} else if (type == typeof(ushort)) {
						x = (double)BitConverter.ToUInt16(b, 0);
					} else if (type == typeof(int)) {
						x = (double)BitConverter.ToInt32(b, 0);
					} else if (type == typeof(uint)) {
						x = (double)BitConverter.ToUInt32(b, 0);
					} else if (type == typeof(long)) {
						x = (double)BitConverter.ToInt64(b, 0);
					} else if (type == typeof(ulong)) {
						x = (double)BitConverter.ToUInt64(b, 0);
					}
				} else {
					if (type == typeof(short)) {
						x = (double)((short)data);
					} else if (type == typeof(ushort)) {
						x = (double)((ushort)data);
					} else if (type == typeof(int)) {
						x = (double)((int)data);
					} else if (type == typeof(uint)) {
						x = (double)((uint)data);
					} else if (type == typeof(long)) {
						x = (double)((long)data);
					} else if (type == typeof(ulong)) {
						x = (double)((ulong)data);
					} else if (type == typeof(sbyte)) {
						x = (double)((sbyte)data);
					} else if (type == typeof(byte)) {
						x = (double)((byte)data);
					} else if (type == typeof(float)) {
						x = (double)((float)data);
					} else if (type == typeof(double)) {
						x = (double)data;
					} else if (type == typeof(decimal)) {
						x = (double)((decimal)data);
					}
				}
			} catch { }
			return x;
		}

		/// <summary>
		/// Reads a A <see cref="Data"/> collection of double values from a binary stream.
		/// Data derives from <see cref="IList{T}">IList&lt;double&gt;</see>.
		/// </summary>
		/// <param name="stream">The stream to read from.</param>
		/// <param name="type">The binary type of the numbers.</param>
		/// <param name="length">Specifies the maximum number of bytes to read from the stream.</param>
		/// <param name="bigEndian">If true, the numbers are read in big-endian format.</param>
		/// <returns>A <see cref="Data"/> of double values read. Data derives from <see cref="IList{T}">IList&lt;double&gt;</see>.
		/// </returns>
		[Obsolete("Use Binary.Data instead.")]
		public static Data BinaryData(Stream stream, long length, Type type, bool bigEndian) {
			return Binary.Data(stream, length, type, bigEndian);

			#region old implementation
			/* old implementation
			BigData d = new BigData(); d.AutoResize = true;
			int n = 0;
			using (BinaryReader r = new BinaryReader(stream)) {
				try {
					if (type == typeof(byte)) {
						while (true) d[n++] = Convert(r.ReadByte(), BigEndian);
					} else if (type == typeof(sbyte)) {
						while (true) d[n++] = Convert(r.ReadSByte(), BigEndian);
					} else if (type == typeof(short)) {
						while (true) d[n++] = Convert(r.ReadInt16(), BigEndian);
					} else if (type == typeof(ushort)) {
						while (true) d[n++] = Convert(r.ReadUInt16(), BigEndian);
					} else if (type == typeof(int)) {
						while (true) d[n++] = Convert(r.ReadInt32(), BigEndian);
					} else if (type == typeof(uint)) {
						while (true) d[n++] = Convert(r.ReadUInt32(), BigEndian);
					} else if (type == typeof(long)) {
						while (true) d[n++] = Convert(r.ReadInt64(), BigEndian);
					} else if (type == typeof(ulong)) {
						while (true) d[n++] = Convert(r.ReadUInt64(), BigEndian);
					} else if (type == typeof(float)) {
						while (true) d[n++] = Convert(r.ReadSingle(), BigEndian);
					} else if (type == typeof(double)) {
						while (true) d[n++] = Convert(r.ReadDouble(), BigEndian);
					} else if (type == typeof(decimal)) {
						while (true) d[n++] = Convert(r.ReadDecimal(), BigEndian);
					}
				} catch { }
			}
			return d;
			*/
			#endregion
		}

		/// <summary>
		/// Reads a A <see cref="Data"/> collection of double values from a binary stream.
		/// Data derives from <see cref="IList{T}">IList&lt;double&gt;</see>.
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <param name="type">The binary type of the numbers</param>
		/// <param name="bigEndian">If true, the numbers are read in big-endian format.</param>
		/// <returns>A <see cref="Data"/> of double values read. Data derives from <see cref="IList{T}">IList&lt;double&gt;</see>.
		/// </returns>
		[Obsolete("Use Binary.Data instead.")]
		public static Data BinaryData(Stream stream, Type type, bool bigEndian) {
			return Binary.Data(stream, type, bigEndian);
		}

		/// <summary>
		/// Reads a A <see cref="Data"/> collection of double values from a binary stream.
		/// Data derives from <see cref="IList{T}">IList&lt;double&gt;</see>.
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <param name="type">The binary type of the numbers</param>
		/// <returns>A <see cref="Data"/> of double values read. Data derives from <see cref="IList{T}">IList&lt;double&gt;</see>.
		/// </returns>
		[Obsolete("Use Binary.Data instead.")]
		public static Data BinaryData(Stream stream, Type type) {
			return Binary.Data(stream, type);
		}
		/// <summary>
		/// Reads data from a WAV file stream and returns them as a <see cref="Data"/> object.
		/// Data derives from <see cref="IList{T}">IList&lt;double&gt;</see>.
		/// </summary>
		/// <param name="stream">The WAV file stream to read from</param>
		/// <param name="channel">The channel of the WAV data that is read. Only this channel will be read.</param>
		/// <param name="SampleRate">The sampling rate of the WAV data in Hz.</param>
		/// <returns>An <see cref="Data"/> object of double values read.</returns>
		[Obsolete("Use WAV.Data instead.")]
		public static Data WAVData(Stream stream, int channel, out double SampleRate) {
			return WAV.Data(stream, channel, out SampleRate);
		}
		/// <summary>
		/// Reads data from a WAV file stream and returns them as a <see cref="Data"/> object.
		/// Data derives from <see cref="IList{T}">IList&lt;double&gt;</see>.
		/// All channels of the WAV file are read.
		/// </summary>
		/// <param name="stream">The WAV file stream to read from</param>
		/// <param name="SampleRate">The sampling rate of the WAV data in Hz.</param>
		/// <returns>An <see cref="Data"/> object of double values read.</returns>
		[Obsolete("Use WAV.Data instead.")]
		public static Data WAVData(Stream stream, out double SampleRate) {
			return WAV.Data(stream, out SampleRate);
		}
		/// <summary>
		/// Gets a boolean value indicating if the array is thread safe. Always returns false.
		/// </summary>
		public bool IsSynchronized { get { return false; } }
		/// <summary>
		/// Gets an object that can be used to snychronize threads. Always returns this.
		/// </summary>
		public object SyncRoot { get { return this; } }
	}

	/// <summary>
	/// A class containing a small amount of data.
	/// </summary>
	[Serializable]
	public class SmallData: Data {
		private double[] data = new double[0];

		/// <summary>
		/// The default constructor.
		/// </summary>
		public SmallData() { }

		/// <summary>
		/// Constructor that initializes the SmallData from an <see cref="IEnumerable{T}">IEnumerable&lt;double&gt;</see>.
		/// </summary>
		/// <param name="data">The <see cref="IEnumerable{T}">IEnumerable&lt;double&gt;</see> to initialize the SmallData with.</param>
		public SmallData(IEnumerable<double> data) {
			List<double> list = new List<double>(data);
			data = list.ToArray();
		}
	
		private void grow(int length, bool resize) {
			if (this.length < length) {
				if (resize) {
					if (data.Length < length) {
						double[] t = new double[length];
						for (int i = 0; i < data.Length; i++) {
							t[i] = data[i];
						}
						for (int i = data.Length; i < length; i++) {
							t[i] = double.NaN;
						}
						data = t;
					}
					this.length = length;
				} else { throw new System.IndexOutOfRangeException(); }
			}
		}
		/// <summary>
		/// Copies from another data array either with a deep or shallow copy, depending on the
		/// <see cref="Data.deepCopy"/> value.
		/// </summary>
		public void CopyFrom(SmallData d) {
			base.CopyFrom(d);
			if (!d.deepCopy) data = d.data;
		}
		/// <summary>
		/// Creates either a deep or shallow copy, depending on the <see cref="Data.deepCopy"/> value.
		/// </summary>
		new public SmallData Clone() {
			SmallData d = new SmallData();
			d.CopyFrom(this);
			return d;
		}
		/// <summary>
		/// Gets or sets the length of the array.
		/// </summary>
		public override int Length {
			get { return length; }
			set { grow(value, true); length = value; Modified = true; }
		}
		/// <summary>
		/// The indexer of the array. if <see	cref="Data.AutoResize">AutoResize</see> is set to true, the length of the array is
		/// automatically adjusted.
		/// </summary>
		public override double this[int i] {
			get { grow(i+1, autoResize); return data[i]; }
			set { grow(i+1, autoResize); data[i] = value; Modified = true; }
		}
	}
	/// <summary>
	/// A class containing big amounts of data.
	/// </summary>
	[Serializable]
	public class BigData: Data {
		const int N = 1024;
		List<double[]> data = new List<double[]>();
		int n = 0;
		[NonSerialized]
		double[] buf;
		[NonSerialized]
		int i = -1;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public BigData() { }

		/// <summary>
		/// Constructor that initializes the BigData from an <see cref="IEnumerable{T}">IEnumerable&lt;double&gt;</see>.
		/// </summary>
		/// <param name="data">The <see cref="IEnumerable{T}">IEnumerable&lt;double&gt;</see> to initialize the BigData with.</param>
		public BigData(IEnumerable<double> data): base(data) { }

		[OnDeserialized]
		private void Deserialize(StreamingContext c) { i = -1; }

		private void grow(int length, bool resize) {
			if (this.length < length) {
				if (resize) {
					while (n <= (length-1) / N) {
						buf = new double[N];
						for (int j = 0; j < N; j++) buf[j] = double.NaN;
						i = n++;
						data.Add(buf);
					}
					this.length = length;
				} else { throw new System.IndexOutOfRangeException(); }
			}
		}
		/// <summary>
		/// Copies from another data array, either with a deep or shallow copy, depending on the 
		/// <see cref="Data.deepCopy">src.deepCopy</see> field.
		/// </summary>
		public void CopyFrom(BigData src) {
			base.CopyFrom(src);
			if (!src.deepCopy) {
				data = src.data;
				n = src.n;
				buf = null;
				i = -1;
			}
		}
		/// <summary>
		/// Returns either a deep or shallow copy, depending on the <see cref="Data.deepCopy">deepCopy</see> field.
		/// </summary>
		new public virtual BigData Clone() {
			BigData d = new BigData();
			d.CopyFrom(this);
			return d;
		}
		/// <summary>
		/// Sets or gets the length of the data array.
		/// </summary>
		public override int Length {
			get { return length; }
			set {
				grow(value, true);
				length = value;
				int m = 0;
				if (length != 0) m = (length-1)/N + 1;
				if (m < n) data.RemoveRange(m, n-m);
				n = m;
				Modified = true;
			}
		}
		/// <summary>
		/// The indexer of the array. If <c>AutoResize"</c> is set, the length of the array is automatically
		/// adjusted upon access.
		/// </summary>
		public override double this[int i] {
			get {
				grow(i+1, autoResize);
				CheckIndex(i);
				if (i/N != this.i) {
					this.i = i/N;
					buf = data[this.i];
				}
				return buf[i%N];
			}
			set {
				grow(i+1, autoResize);
				CheckIndex(i);
				if (i/N != this.i) {
					this.i = i/N;
					buf = data[this.i];
				}
				buf[i%N] = value;
				Modified = true;
			}
		}
	}
}
