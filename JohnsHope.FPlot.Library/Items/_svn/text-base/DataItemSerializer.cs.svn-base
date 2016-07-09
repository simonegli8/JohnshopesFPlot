using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// A class used internally for backwards compatibility to older versions of FPlot.
	/// </summary>
	[Serializable]
	public class DataItemSerializer: Item {
		/// <summary>
		/// Used internally.
		/// </summary>
		protected int length;

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
		public string Source;

		/// <summary>
		/// Used internally.
		/// </summary>
		protected DataColumn x, y, z, dx, dy, dz;

		/// <summary>
		/// Used internally.
		/// </summary>
		protected int dim;

		/// <summary>
		/// Used internally.
		/// </summary>
		protected bool err;

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

		/// <summary>
		/// Used internally.
		/// </summary>
		protected Color color = Color.Black;

		/// <summary>
		/// Used internally.
		/// </summary>
		protected float lineWidth = 1;

		/// <summary>
		/// Used internally.
		/// </summary>
		protected DashStyle lineStyle = DashStyle.Solid;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public DataItemSerializer() { }

		/*
		/// <summary>
		/// If passed to <see cref="SetLoadWAVSource(int, DataColumn)">SetLoadWAVSource</see>, all channels are loaded.
		/// </summary>
		const int AllChannels = WAV.AllChannels;
		*/
	}
}
