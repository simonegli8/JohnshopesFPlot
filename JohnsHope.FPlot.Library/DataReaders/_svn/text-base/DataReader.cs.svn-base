using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;

namespace JohnsHope.FPlot.Library {

	/// <summary>
	/// A class that implements <see cref="IEnumerable{T}">IEnumerable&lt;double&gt;</see> and
	/// <see cref="IEnumerator{T}">IEnumerator&lt;double&gt;</see> and serves as a low memory
	/// consuming source of double values.
	/// </summary>
	public class DataReader: IEnumerable<double>, IEnumerator<double> {

		IEnumerator<double> e;
		bool hasNext = false;
		double x;

		/// <summary>
		/// A constructor that initializes the DataReader with a <see cref="IEnumerable{T}">IEnumerator&lt;double&gt;</see> to read from.
		/// </summary>
		/// <param name="source"></param>
		public DataReader(IEnumerable<double> source) {
			e = source.GetEnumerator();
			hasNext = e.MoveNext();
			x = double.NaN;
		}
		/// <summary>
		/// A constructor that initializes the DataReader with a <see cref="IEnumerable{T}">IEnumerator&lt;double&gt;</see> to read from.
		/// </summary>
		/// <param name="enumerator"></param>
		public DataReader(IEnumerator<double> enumerator) {
			e = enumerator;
			hasNext = e.MoveNext();
			x = double.NaN;
		}
	
		// IEnumerable and IEnumerator methods
		/// <summary>
		/// Returns an enumerator for the DataReader.
		/// </summary>
		public IEnumerator<double> GetEnumerator() { return this; }
		/// <summary>
		/// Returns an enumerator for the DataReader.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator() { return this; }

		/// <summary>
		/// Returns the current double value as in IEnumerator&lt;double&gt;. Returns double.NaN if there is no value.
		/// </summary>
		public double Current { get { return x; } }
		/// <summary>
		/// Gets the current double value.
		/// </summary>
		object IEnumerator.Current { get { return x; } }
		/// <summary>
		/// Disposes the DataReader.
		/// </summary>
		public void Dispose() {	e.Dispose(); }
		/// <summary>
		/// Moves to the next double value as in IEnumerator&lt;double&gt;.
		/// </summary>
		/// <returns>Returns true if there is a next value available.</returns>
		public bool MoveNext(){
			if (hasNext) {
				x = e.Current;
				hasNext = e.MoveNext();
				return true;
			} else return false;
		}
		/// <summary>
		/// Resets the DataReader to the beginning of the double values.
		/// </summary>
		public void Reset() {
			e.Reset();
			hasNext = e.MoveNext();
			x = double.NaN;
		}
		/// <summary>
		/// Gets the next double value in the DataReader and moves to the next value. Returns double.NaN if there is no value available.
		/// </summary>
		public double Read() {
			if (hasNext) {
				x = e.Current;
				hasNext = e.MoveNext();
			} else x = double.NaN;
			return x;
		}
		/// <summary>
		/// Is true if there is a next value.
		/// </summary>
		public bool HasNext { get { return hasNext; } }
	}

}