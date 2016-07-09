using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.CodeDom.Compiler;

namespace JohnsHope.FPlot.Library {

	/// <summary>
	/// Used internally.
	/// </summary>
	[Serializable]
	public class DataColumnSerializer: BigData {
	
		/// <summary>
		/// Used internally.
		/// </summary>
		protected string source = null;

		/// <summary>
		/// The parent <see cref="DataItem"/>.
		/// </summary>
		protected DataItem Parent;

		/// <summary>
		/// The base type of the DataColumn.
		/// </summary>
		protected Type Type = typeof(double);

		/// <summary>
		/// The default constructor.
		/// </summary>
		public DataColumnSerializer(): base() {
			if (!(this is DataColumn)) {
				throw new NotSupportedException("Cannot create a DataColumnSerializer instance.");
			}
		}

		/// <summary>
		/// A constructor setting the parent <see cref="DataItem">DataItem</see>.
		/// </summary>
		public DataColumnSerializer(IEnumerable<double> data): base(data) {
			if (!(this is DataColumn)) {
				throw new NotSupportedException("Cannot create a DataColumnSerializer instance.");
			}
		}

	}
}
