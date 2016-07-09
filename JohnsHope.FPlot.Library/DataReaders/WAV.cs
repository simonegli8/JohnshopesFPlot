using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// A class that reads data from a WAV file. Only the PCM WAV-format is supported.
	/// </summary>
	public class WAV {
		/// <summary>
		/// If passed as channels parameter, all channels are read.
		/// </summary>
		public const int AllChannels = -1;
		/// <summary>
		/// An exception that indicates a file format that does not correspond to the WAV standard.
		/// </summary>
		public class WrongFileFormatException: Exception {
			/// <summary>
			/// Creates a new WrongFileFormatException with the supplied message string.
			/// </summary>
			public WrongFileFormatException(string message): base(message) { }
		}

		#region Data Chunk classes

		private class Chunk {
			public const int MinSize = 8;
			public long Position;
			public string id;
			public Int32 size;
			public Stream stream;
			public BinaryReader r;
			
			public bool EndOfFile {
				get { return stream.Position >= stream.Length; }
				set { if (value) stream.Position = stream.Length; }
			}

			public bool EndOfChunk {
				get { return stream.Position >= Position + MinSize + size; }
			}

			public virtual bool StillReadable(int bytes) {
				return stream.Position + bytes <= Position + MinSize + size;
			}

			public string ReadID() {
				byte[] b = r.ReadBytes(4);
				return ((char)b[0]).ToString() + ((char)b[1]).ToString() + ((char)b[2]).ToString() + ((char)b[3]).ToString();
			}

			public void CopyFrom(Chunk chunk) {
				Position = chunk.Position;
				id = chunk.id;
				size = chunk.size;
				stream = chunk.stream;
				r = chunk.r;
			}

			public Chunk(Chunk chunk) { CopyFrom(chunk); }

			public Chunk(Stream stream) {
				this.stream = stream;
				Position = stream.Position;
				r = new BinaryReader(stream);
				if (StillReadable(MinSize)) {
					id = ReadID();
					size = r.ReadInt32();
				} else EndOfFile = true;
			}

			public void Skip() {
				stream.Position = Position + 8 + size;
			}
		}

		private class RIFFChunk: Chunk {
			public string riffType;

			public RIFFChunk(Stream stream): base(stream) {
			if (id != "RIFF") throw new WrongFileFormatException("Not a WAV file");
				BinaryReader r = new BinaryReader(stream);
				try {
					riffType = ReadID();
				} catch {
					throw new WrongFileFormatException("Invalid file format");
				}
			}
		}

		private class FormatChunk: Chunk {
			public Int16 format;
			public UInt16 channels;
			public UInt32 samplesPerSecond;
			public UInt32 avgBytesPerSecond;
			public UInt16 blockAlign;
			public UInt16 bitsPerSample;

			public FormatChunk(Chunk chunk): base(chunk) {
				if (id != "fmt ") throw new WrongFileFormatException("Invalid file format");
				try {
					format = r.ReadInt16();
					channels = r.ReadUInt16();
					samplesPerSecond = r.ReadUInt32();
					avgBytesPerSecond = r.ReadUInt32();
					blockAlign = r.ReadUInt16();
					bitsPerSample = r.ReadUInt16();
				} catch {
					throw new WrongFileFormatException("Invalid file format");
				}

			}
		}

		private class DataChunk: Chunk {
			public FormatChunk format;

			public int pos = 0;

			public DataChunk(Chunk chunk, FormatChunk format): base(chunk) {
				if (id != "data") throw new WrongFileFormatException("Invalid file format");
				this.format = format;
			}

			public double ReadSample() {
				int bytes = (format.bitsPerSample + 7) / 8;
				if (StillReadable(bytes)) {
					byte[] d0 = r.ReadBytes(bytes), d;
					
					if (d0.Length != 3) d = d0; // pad a byte if d0.Length == 3
					else d = new byte[4]{ 0, d0[0], d0[1], d0[2] };

					if (!BitConverter.IsLittleEndian) {	// swap bytes
						for (int i = 0; i < d.Length / 2; i++) {
							byte t = d[i]; d[i] = d[d.Length-1-i]; d[d.Length-1-i] = t;
						}
					}
					switch (bytes) {
						case 1:	return (double)((int)d[0] - 0x80)/0x80;
						case 2: return (double)BitConverter.ToInt16(d, 0)/0x8000;
						case 3: return (double)BitConverter.ToInt32(d, 0)/0x800000;
						case 4: return (double)BitConverter.ToInt32(d, 0)/0x80000000;
						default: throw new NotSupportedException("Invalid file format: Only 32 bits per sample are supported"); 
					}
				}
				return double.NaN;
			}

			public double ReadSample(int channel) {
				double d = double.NaN;
				if (channel >= format.channels) throw new ArgumentException("There is no channel " + channel.ToString() + " in this WAV file");
				for (int i = 0; i < format.channels; i++) {
					if (i == channel) d = ReadSample();
					else ReadSample();
				}
				return d;
			}
		}
		#endregion

		/// <summary>
		/// A class that implements a <see cref="IEnumerator{T}">IEnumerator&lt;double&gt;</see> that reads from a WAV <see cref="Stream"/>.
		/// </summary>
		public class Enumerator: IEnumerator<double> {

			Stream s;
			int channel;
			double x = double.NaN, sampleRate;
			DataChunk data;
			FormatChunk format;

			/// <summary>
			/// Constructor that initializes a WAV.Enumerator on a <see cref="Stream"/>.
			/// </summary>
			/// <param name="stream">The <see cref="Stream"/> to read from.</param>
			/// <param name="channel">The channel to read, or <see cref="AllChannels"/>.</param>
			/// <param name="SampleRate">The sample rate of the WAV data in Hertz units.</param>
			public Enumerator(Stream stream, int channel, out double SampleRate) {
				data = null;
				s = stream;
				this.channel = channel;

				// read RIFF chunk
				RIFFChunk riff = new RIFFChunk(stream);
				if (riff.riffType != "WAVE") throw new WrongFileFormatException("Not a WAV file");

				// read format chunk
				Chunk chunk = new Chunk(stream); 
				if (chunk.id != "fmt ") throw new WrongFileFormatException("Invalid file format");
				format = new FormatChunk(chunk);
				if (format.format != 1) throw new NotSupportedException("Only PCM WAV format is supported");
				SampleRate = sampleRate = format.samplesPerSecond;

				// skip non data chunks
				chunk = new Chunk(stream);
				while (!chunk.EndOfFile && chunk.id != "data") {
					chunk.Skip();
					chunk = new Chunk(stream);
				}

				if (chunk.EndOfFile) throw new WrongFileFormatException("Invalid file format");

				//inititalize data chunk
				data = new DataChunk(chunk, format);
			}

			/// <summary>
			/// Constructor that initializes a WAV.Enumerator on a <see cref="Stream"/>.
			/// </summary>
			/// <param name="stream">The <see cref="Stream"/> to read from.</param>
			/// <param name="SampleRate">The sample rate of the WAV data in Hertz units.</param>
			public Enumerator(Stream stream, out double SampleRate) : this(stream, AllChannels, out SampleRate) { }

			/// <summary>
			/// The current value.
			/// </summary>
			public double Current { get { return x; } }
			object IEnumerator.Current { get { return x; } }

			/// <summary>
			/// Disposes the Enumerator.
			/// </summary>
			public void Dispose() { data = null; format = null; x = double.NaN; }

			/// <summary>
			/// Moves to the next value.
			/// </summary>
			/// <returns>Returns false if there is no value available.</returns>
			public bool MoveNext() {
				if (data == null || data.EndOfChunk) return false;

				if (channel == AllChannels) {
					x = data.ReadSample();
				} else {
					x = data.ReadSample(channel);
				}
				return true;
			}

			/// <summary>
			/// Resets the enumerator to the beginning.
			/// </summary>
			public void Reset() {
				if (data != null) {
					s.Position = data.Position;
					data = new DataChunk(new Chunk(s), format);
					x = double.NaN;
				}
			}

		}

		/// <summary>
		/// A <see cref="DataReader"/> class that implements reading from a WAV <see cref="Stream"/>.
		/// </summary>
		public class DataReader: JohnsHope.FPlot.Library.DataReader {
			/// <summary>
			/// A constructor that initializes a DataReader on a WAV <see cref="Stream"/>.
			/// </summary>
			/// <param name="stream">The <see cref="Stream"/> to read from.</param>
			/// <param name="channel">The channel to read, or <see cref="AllChannels"/>.</param>
			/// <param name="SampleRate">The sample rate of the WAV data in Hertz units.</param>
			public DataReader(Stream stream, int channel, out double SampleRate) : base(new Enumerator(stream, channel, out SampleRate)) { }
			/// <summary>
			/// A constructor that initializes a DataReader on a WAV <see cref="Stream"/>.
			/// </summary>
			/// <param name="stream">The <see cref="Stream"/> to read from.</param>
			/// <param name="SampleRate">The sample rate of the WAV data in Hertz units.</param>
			public DataReader(Stream stream, out double SampleRate) : base(new Enumerator(stream, out SampleRate)) { }
		}

		/// <summary>
		/// A <see cref="DataItem.Instance"/> class that implements fast loading of text data. This class is used internally
		/// by the method <see cref="DataItem.LoadWAV(string, out DataColumn)"/>.
		/// </summary>
		public class Loader: DataItem.Instance {

			double sampleRate = 0;
			int channel;

			/// <summary>
			/// The default constructor.
			/// </summary>
			public Loader(): this(AllChannels) { }

			/// <summary>
			/// A constructor.
			/// </summary>
			/// <param name="channel">The WAV channel to read or <see cref="WAV.AllChannels"/> for all channels.</param>
			public Loader(int channel) {
				this.channel = channel;
			}

			/// <summary>
			/// Gets or sets the WAV channel used by the Loader.
			/// </summary>
			public int Channel { get { return channel; } set { channel = value; } }

			/// <summary>
			/// Gets the sample rate of the WAV data read by the Loader.
			/// </summary>
			public double SampleRate { get { return sampleRate; } }

			/// <summary>
			/// Gets a <see cref="SamplesColumn"/> for the Loader.
			/// </summary>
			public SamplesColumn Samples() { return new SamplesColumn(sampleRate); }

			/// <summary>
			/// Copies the Loader from another Loader.
			/// </summary>
			public void CopyFrom(Loader src) {
				base.CopyFrom(src);
				sampleRate = src.sampleRate;
				channel = src.channel;
			}

			/// <summary>
			/// Loads the <see cref="DataItem"/> from a <see cref="Stream"/>.
			/// </summary>
			public override void Load(Stream stream) {
				Parent.CopyFrom(WAV.Reader(stream, channel, out sampleRate));
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
		/// A <see cref="DataColumn"/> representing equally spaced sample points, that you can assign to one of the 
		/// <see cref="DataColumn"/>s of a <see cref="DataItem"/>.
		/// </summary>
		public class SamplesColumn: DataColumn {
			double sampleRate = 1;

			/// <summary>
			/// The default constructor.
			/// </summary>
			public SamplesColumn(): base() { }

			/// <summary>
			/// A constructor that initializes a SamplesColumn with a sample rate.
			/// </summary>
			/// <param name="sampleRate">The sample rate in Hertz units.</param>
			public SamplesColumn(double sampleRate) : base() { this.sampleRate = sampleRate; Source = OriginalSource; }
			
			/// <summary>
			/// Gets or sets the sample rate of the SamplesColumn in Hertz units.
			/// </summary>
			public double SampleRate { get { return sampleRate; } set { sampleRate = value; Source = OriginalSource; } }

			/// <summary>
			/// Returns a source string that corresponds to the <see cref="SampleRate"/>.
			/// </summary>
			public string OriginalSource {
				get {
					if (sampleRate == 1) return "n";
					return "n/" + sampleRate.ToString();
				}
			} 

			/// <summary>
			/// The indexer of the SamplesColumn.
			/// </summary>
			public override double this[int i] {
				get {
					if (Source == OriginalSource) return (double)i / sampleRate;
					return base[i];
				}
				set {
					throw new InvalidOperationException("data cannot be changed.");
				}
			}

			/// <summary>
			/// Returns a compilable source. Used internally be the <see cref="Compiler"/>.
			/// </summary>
			public override string GetSource() {
				if (Source == OriginalSource) return null;
				return base.GetSource();
			}

			/// <summary>
			/// Copies from another SamplesColum. 
			/// </summary>
			public void CopyFrom(SamplesColumn src) {
				base.CopyFrom(src);
				sampleRate = src.sampleRate;
				Source = OriginalSource;
				Source = src.Source;
			}

			/// <summary>
			/// Creates a copy of the SamplesColumn.
			/// </summary>
			/// <param name="Parent">The parent <see cref="DataItem">DataItem</see> of the copy.</param>
			[Obsolete("Use Clone() instead.")]
			public override DataColumn Clone(DataItem Parent) {
				SamplesColumn d = new SamplesColumn();
				d.Parent = Parent;
				d.CopyFrom(this);
				return d;
			}

			/// <summary>
			/// Creates a copy of the SamplesColumn.
			/// </summary>
			public override DataColumn Clone() {
				SamplesColumn d = new SamplesColumn();
				d.CopyFrom(this);
				return d;
			}
		}


		/// <summary>
		/// Returns a <see cref="DataReader"/> that reads data from a WAV <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <param name="channel">The channel to read, or <see cref="AllChannels"/>.</param>
		/// <param name="SampleRate">The sample rate of the WAV data in Hertz units.</param>
		public static DataReader Reader(Stream stream, int channel, out double SampleRate) {
			return new DataReader(stream, channel, out SampleRate);
		}

		/// <summary>
		/// Returns a <see cref="DataReader"/> that reads data from a WAV <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <param name="SampleRate">The sample rate of the WAV data in Hertz units.</param>
		public static DataReader Reader(Stream stream, out double SampleRate) {
			return new DataReader(stream, out SampleRate);
		}

		/// <summary>
		/// Reads data from a WAV <see cref="Stream"/> and returns it in a <see cref="BigData"/> object.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <param name="channel">The channel to read, or <see cref="AllChannels"/>.</param>
		/// <param name="SampleRate">The sample rate of the WAV data in Hertz units.</param>
		public static BigData Data(Stream stream, int channel, out double SampleRate) {
			return new BigData(new DataReader(stream, channel, out SampleRate));
		}

		/// <summary>
		/// Reads data from a WAV <see cref="Stream"/> and returns it in a <see cref="BigData"/> object.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <param name="SampleRate">The sample rate of the WAV data in Hertz units.</param>
		public static BigData Data(Stream stream, out double SampleRate) {
			return new BigData(new DataReader(stream, out SampleRate));
		}

		/// <summary>
		/// Returns a <see cref="SamplesColumn"/> to represent equally spaced sample points.
		/// </summary>
		/// <param name="sampleRate">The sample rate in Hertz units.</param>
		public static SamplesColumn Samples(double sampleRate) {
			return new SamplesColumn(sampleRate);
		}
		/// <summary>
		/// Reads data from a WAV (PCM) file stream and returns them as a <see cref="BigData">BigData</see> object.
		/// </summary>
		/// <param name="stream">The WAV file stream to read from</param>
		/// <param name="channel">The channel of the WAV data that is read. Only this channel will be read.</param>
		/// <param name="SampleRate">The sampling rate of the WAV data in Hz.</param>
		/// <returns>An <c>IList</c> of double values read.</returns>
		[Obsolete("Use Data instead.")]
		public static IList<double> Load(Stream stream, int channel, out double SampleRate) { return Data(stream, channel, out SampleRate); }

		/// <summary>
		/// Reads data from a WAV (PCM) file stream and returns them as a <see cref="BigData">BigData</see> object.
		/// All channels of the WAV file are read.
		/// </summary>
		/// <param name="stream">The WAV file stream to read from</param>
		/// <param name="SampleRate">The sampling rate of the WAV data in Hz.</param>
		/// <returns>An <c>IList</c> of double values read.</returns>
		[Obsolete("Use Data instead.")]
		public static IList<double> Load(Stream stream, out double SampleRate) { return Data(stream, AllChannels, out SampleRate); }

	}

}
