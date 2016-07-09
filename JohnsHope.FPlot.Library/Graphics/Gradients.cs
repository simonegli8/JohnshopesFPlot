using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;

namespace JohnsHope.FPlot.Library {

	/// <summary>
	/// A class that represents a <see cref="ComboBox"/> where one can select a gradient.
	/// </summary>
	[ToolboxBitmap(typeof(resfinder), "JohnsHope.FPlot.Library.Resources.Gradient.ico")]
	public class GradientChooser: ComboBox {
		/// <summary>
		/// The default constructor.
		/// </summary>
		public GradientChooser(): base() {
			Reset();
			this.ParentChanged += HandleParentChanged;
		}

		private void HandleParentChanged(object sender, EventArgs e) {
			Reset();
		}

		private void DrawGradientItem(object sender, System.Windows.Forms.DrawItemEventArgs e) {
			e.DrawBackground();
			if (e.Index >= 0 && e.Index < Gradients.List.Count) {
				IGradient g = Gradients.List[e.Index];
				GradientPainter.FillRectangle(e.Graphics, g, e.Bounds, GradientPainter.Direction.Right);
			}
			e.DrawFocusRectangle();
		}
		/// <summary>
		/// The selected gradient.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IGradient SelectedGradient {
			get { return Gradients.List[SelectedIndex]; }
			set {
				int n = Math.Max(0, Gradients.List.IndexOf(value));
				n = Math.Min(Gradients.List.Count-1, n);
				SelectedIndex = n;
			}
		}
		/// <summary>
		/// Resets the GradientChooser (and reloads all gradients from the gradients directory).
		/// </summary>
		public void Reset() {
			DropDownStyle = ComboBoxStyle.DropDownList;
			DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;

			DrawItem -= DrawGradientItem;
			DrawItem += DrawGradientItem;

			Gradients.Reset();
			base.Items.Clear();
			for (int n = 0; n < Gradients.List.Count; n++) {
				base.Items.Add(n.ToString());
			}
			SelectedIndex = 0;
		}

		private void InitializeComponent() {
			this.SuspendLayout();
			this.ResumeLayout(false);

		}
	}
	/// <summary>
	/// An interface that represents color gradients.
	/// </summary>
	public interface IGradient {
		/// <summary>
		/// This routine returns a color depending on the parameter x.
		/// </summary>
		/// <param name="x">A value between 0 and 1.</param>
		Color Color(double x);
		/// <summary>
		/// Creates a deep copy of the gradient.
		/// </summary>
		/// <returns></returns>
		IGradient Clone();
	}
	/// <summary>
	/// A class that fills a rectangle with a <see cref="IGradient">Gradient</see>.
	/// </summary>
	public static class GradientPainter {
		/// <summary>
		/// The direction of the gradient.
		/// </summary>
		public enum Direction {
			/// <summary>
			/// The gradient is drawn downwards.
			/// </summary>
			Down,
			/// <summary>
			/// The gradient is drawn to the right.
			/// </summary>
			Right,
			/// <summary>
			/// The gradient is drawn upwards.
			/// </summary>
			Up,
			/// <summary>
			/// The gradient is drawn to the left.
			/// </summary>
			Left
		}
		/// <summary>
		/// Fills a rectangle with the gradient.
		/// </summary>
		/// <param name="g">The <see cref="Graphics"/> object to draw to.</param>
		/// <param name="gradient">The gradient to use.</param>
		/// <param name="frame">The rectangle to fill with the gradient.</param>
		/// <param name="direction">The direction of the gradient.</param>
		public static void FillRectangle(Graphics g, IGradient gradient, Rectangle frame, Direction direction) {
			BitmapBuilder bmp = null;
			int N = 0;
			bool reverse = false;
			switch (direction) {
				case Direction.Down:
					N = frame.Height;
					bmp = new BitmapBuilder(new Rectangle(0, 0, 1, N));
					reverse = false;
					break;
				case Direction.Up:
					N = frame.Height;
					bmp = new BitmapBuilder(new Rectangle(0, 0, 1, N));
					reverse = true;
					break;
				case Direction.Left:
					N = frame.Width;
					bmp = new BitmapBuilder(new Rectangle(0, 0, N, 1));
					reverse = true;
					break;
				case Direction.Right:
					N = frame.Width;
					bmp = new BitmapBuilder(new Rectangle(0, 0, N, 1));
					reverse = false;
					break;
			}
			bmp.Lock();
			unsafe {
				int* pixel = bmp.Pixel(0, 0); 
				for (double x = 0; x < N; x++) {
					try {
						if (reverse) *pixel = gradient.Color((N - x - 1)/N).ToArgb();
						else *pixel = gradient.Color(x/N).ToArgb();
					} catch {
						*pixel = Color.Transparent.ToArgb();
					}
					pixel++;
				}
			}
			bmp.Unlock();
			TextureBrush t = new TextureBrush(bmp.Bitmap);
			Matrix T0 = g.Transform.Clone();
			Matrix T = g.Transform.Clone();
			T.Translate(frame.X, frame.Y);
			g.Transform = T;
			g.FillRectangle(t, new Rectangle(0, 0, frame.Width, frame.Height));
			g.Transform = T0;
		}
	}
	/// <summary>
	/// This class represents a linear gradient.
	/// </summary>
	[Serializable]
	public class LinearGradient: IGradient {
		/// <summary>
		/// The <see cref="Color"/> at the lower end of the gradient.
		/// </summary>
		public Color LowerColor;
		/// <summary>
		/// The <see cref="Color"/> at the upper end of the gradient.
		/// </summary>
		public Color UpperColor;
		/// <summary>
		/// The contructor for the gradient.
		/// </summary>
		/// <param name="LowerColor">The <see cref="Color"/> at the lower end of the gradient.</param>
		/// <param name="UpperColor">The <see cref="Color"/> at the upper end of the gradient.</param>
		public LinearGradient(Color LowerColor, Color UpperColor) {
			this.LowerColor = LowerColor;
			this.UpperColor = UpperColor;
		}
		/// <summary>
		/// Default constructor. Initializes the gradient with a <see cref="LowerColor"/> = <see cref="System.Drawing.Color.White"/> and
		/// <see cref="UpperColor"/> = <see cref="System.Drawing.Color.Black"/>.
		/// </summary>
		public LinearGradient(): this(System.Drawing.Color.White, System.Drawing.Color.Black) { }

		int Byte(int val) {
			if (val <= 0) return 0;
			if (val >= 255) return 255;
			else return val;
		}

		/// <summary>
		/// Returns a color from a value of x between 0 and 1.
		/// </summary>
		public virtual Color Color(double x) {
			x -= Math.Floor(x);
			return System.Drawing.Color.FromArgb(
				Byte(LowerColor.A + (int)((UpperColor.A - LowerColor.A)*x + 0.5)),
				Byte(LowerColor.R + (int)((UpperColor.R - LowerColor.R)*x + 0.5)),
				Byte(LowerColor.G + (int)((UpperColor.G - LowerColor.G)*x + 0.5)),
				Byte(LowerColor.B + (int)((UpperColor.B - LowerColor.B)*x + 0.5)));
		}
		/// <summary>
		/// Returns a deep copy of the LinearGradient.
		/// </summary>
		/// <returns></returns>
		public IGradient Clone() { return new LinearGradient(LowerColor, UpperColor);  }
	}
	/// <summary>
	/// A rainbow gradient.
	/// </summary>
	[Serializable]
	public class RainbowGradient: IGradient {
		/// <summary>
		/// The function that converts a double into a color.
		/// </summary>
		public virtual Color Color(double x) {
			const double d = 1.0/6.0;
			x -= Math.Floor(x);
			int R, G, B;
			double arg;
			arg = (x - d)/(2*d);
			R = (int)(255*Math.Exp(-arg*arg));
			arg = (x - 3*d)/(2*d);
			G = (int)(255*Math.Exp(-arg*arg));
			arg = (x - 5*d)/(2*d);
			B = (int)(255*Math.Exp(-arg*arg));
			return System.Drawing.Color.FromArgb(R, G, B);
		}
		/// <summary>
		/// Returns a deep copy of the gradient.
		/// </summary>
		public IGradient Clone() { return new RainbowGradient(); }
	}
	/// <summary>
	/// A gradient that always returns <see cref="System.Drawing.Color.Transparent"/>.
	/// </summary>
	[Serializable]
	public class TransparentGradient: IGradient {
		/// <summary>
		/// Returns a color from a value of x between 0 and 1.
		/// </summary>
		public virtual Color Color(double x) {
			return System.Drawing.Color.Transparent;
		}
		/// <summary>
		/// Returns a deep copy of the gradient.
		/// </summary>
		public IGradient Clone() { return new TransparentGradient(); }
	}
	/// <summary>
	/// A gradient that consists of individual segments of other gradients.
	/// </summary>
	[Serializable]
	public class SegmentGradient: IList<SegmentGradient.Segment>, IGradient {
		/// <summary>
		/// A segment of a SegmentGradient.
		/// </summary>
		[Serializable]
		public class Segment {
			/// <summary>
			/// The upper limit of the segment. Must be a value between 0 and 1. The last segment must have a value of 1.
			/// </summary>
			public double x;
			/// <summary>
			/// The gradient used in this segment.
			/// </summary>
			public IGradient Gradient;
			/// <summary>
			/// The default constructor.
			/// </summary>
			public Segment(double x, IGradient Gradient) {
				this.x = x; this.Gradient = Gradient;
			}
			/// <summary>
			/// Creates a deep copy of the segment.
			/// </summary>
			/// <returns></returns>
			public Segment Clone() { return new Segment(x, Gradient); }
		}
		
		private int Compare(Segment a, Segment b) {
			if (a.x < b.x) return -1;
			else if (a.x > b.x) return 1;
			else return 0;
		}

		private List<Segment> list;

		private void Sort() {
			list.Sort(new Comparison<Segment>(Compare));
		}

		#region IList implementation
		/// <summary>
		/// The indexer of a SegmentGradient.
		/// </summary>
		public Segment this[int i] {
			get { return list[i]; }
			set { list[i] = value; Sort(); }
		}
		/// <summary>
		/// Returns the number of segments in the SegmentGradient.
		/// </summary>
		public int Count { get { return list.Count; } }
		/// <summary>
		/// Always returns false.
		/// </summary>
		public bool IsReadOnly { get { return false; } }
		/// <summary>
		/// Returns the index of the specified segment.
		/// </summary>
		public int IndexOf(Segment s) {	return list.IndexOf(s); }
		/// <summary>
		/// Inserts a segment into the gradient. After inserting the gradient is automaitcally sorted.
		/// </summary>
		public void Insert(int index, Segment s) { list.Insert(index, s); Sort(); }
		/// <summary>
		/// Adds a segment to the gradient. After adding the gradient is automaitcally sorted.
		/// </summary>
		public void Add(Segment s) { list.Add(s); Sort(); }
		/// <summary>
		/// Clears the gradient from all segments.
		/// </summary>
		public void Clear() { list.Clear(); }
		/// <summary>
		/// Returns true if the segment is contained in the gradient.
		/// </summary>
		public bool Contains(Segment s) { return list.Contains(s); }
		/// <summary>
		/// Copies the segments to an array.
		/// </summary>
		public void CopyTo(Segment[] array, int index) { list.CopyTo(array, index); }
		/// <summary>
		/// Removes a segment from the gradient.
		/// </summary>
		public bool Remove(Segment s) { return list.Remove(s); }
		/// <summary>
		/// Removes the segment at the given index from the gradient.
		/// </summary>
		public void RemoveAt(int index) { list.RemoveAt(index); }
		/// <summary>
		/// Returns an IEnumerator.
		/// </summary>
		public IEnumerator<Segment> GetEnumerator() { return list.GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return ((IEnumerable)list).GetEnumerator(); }
		#endregion
		/// <summary>
		/// The default constructor.
		/// </summary>
		public SegmentGradient() {
			list = new List<Segment>();
		}
		/// <summary>
		/// Returns a deep copy of the gradient.
		/// </summary>
		/// <returns></returns>
		public IGradient Clone() {
			SegmentGradient g = new SegmentGradient();
			foreach (Segment s in list) {
				g.list.Add(s.Clone());
			}
			return g;
		}
		/// <summary>
		/// Loads a GIMP-Gradient .ggr file 
		/// </summary>
		/// <param name="filename">The path of the file to load.</param>
		public static SegmentGradient LoadGIMP(string filename) {
			SegmentGradient sg = new SegmentGradient();
			try {
				StreamReader r = new StreamReader(filename);
				string s = null;
				int line = 0, segments = 0;
				while (line != 3 && ((s = r.ReadLine()) != null)) { line++; }
				if (s != null) segments = int.Parse(s);
				while (segments > 0 && ((s = r.ReadLine()) != null)) {
					string[] tokens = s.Split(new char[2] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					LinearGradient g = new LinearGradient();
					g.LowerColor = System.Drawing.Color.FromArgb((int)(255*double.Parse(tokens[6]) + 0.5),
						(int)(255*double.Parse(tokens[3]) + 0.5),
						(int)(255*double.Parse(tokens[4]) + 0.5),
						(int)(255*double.Parse(tokens[5]) + 0.5));
					g.UpperColor = System.Drawing.Color.FromArgb((int)(255*double.Parse(tokens[10]) + 0.5),
						(int)(255*double.Parse(tokens[7]) + 0.5),
						(int)(255*double.Parse(tokens[8]) + 0.5),
						(int)(255*double.Parse(tokens[9]) + 0.5));
					sg.Add(new Segment(double.Parse(tokens[2]), g));
				}
			} catch (Exception e0) {
				ArgumentException e1 = new ArgumentException("Invalid GIMP gradient file format", e0);
				e1.Data["filename"] = filename;
				throw e1;
			}
			return sg;
		}

		/// <summary>
		/// Returns a color from a double value x.
		/// </summary>
		public virtual Color Color(double x) {
			x -= Math.Floor(x);
			int n = 0;
			double x0 = 0;
			while (n < list.Count-1 && list[n].x <= x) { x0 = list[n].x; n++; }
			double w = list[n].x - x0;
			return list[n].Gradient.Color((x - x0)/w);
		}
	}

	public delegate void GradientLoader(string path);

	/// <summary>
	/// Represents the available gradients.
	/// </summary>
	public class Gradients {
		/// <summary>
		/// The list of global gradients.
		/// </summary>
		public static List<IGradient> List = null;
		/// <summary>
		/// Represents a linear gradient from white to black.
		/// </summary>
		public static LinearGradient LinearGradient = new LinearGradient(Color.White, Color.Black);

		public static event GradientLoader Extensions;

		class GradientStop {
			public Color Color;
			public double Offset;
		}

		static void LoadXamlGradients(string path) {
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(Color));
			foreach (var file in Directory.GetFiles(path, "*.xaml")) {
				XmlDocument doc = new XmlDocument();
				doc.Load(file);
				foreach (XmlElement g in doc.GetElementsByTagName("LinearGradientBrush")) {
					List<GradientStop> stops = new List<GradientStop>();
					foreach (XmlElement e in g.GetElementsByTagName("GradientStop")) {
						Color color = (Color)converter.ConvertFrom(e.GetAttribute("Color"));
						double offset = 0;
						string os = e.GetAttribute("Offset");
						double.TryParse(os, out offset);
						stops.Add(new GradientStop { Color = color, Offset = offset });
					}
					stops.Sort(delegate(GradientStop a, GradientStop b) { return a.Offset.CompareTo(b.Offset); });

					SegmentGradient sg = new SegmentGradient();
					LinearGradient lg = null;
					for (int i = 0; i < stops.Count; i++) {
						var stop = stops[i];
						if (lg != null) {
							lg.UpperColor = stop.Color;
							sg.Add(new SegmentGradient.Segment(stop.Offset, lg));
							lg = null;
						}
						if (i < stops.Count-1 && stop.Offset == stops[i+1].Offset) continue;
						lg = new LinearGradient();
						lg.LowerColor = stop.Color;
					}
					List.Add(sg);
				}
			}
		}

		static void LoadGIMPGradients(string path) {
			string[] files = Directory.GetFiles(path, "*.ggr");
			foreach (string f in files) {
				List.Add(SegmentGradient.LoadGIMP(f));
			}
		}

		/// <summary>
		/// Resets the global list of gradients.
		/// </summary>
		public static void Reset() {
			List = new List<IGradient>();
			List.Clear();
			List.Add(LinearGradient);
			List.Add(new RainbowGradient());
			string path = Path.GetDirectoryName(typeof(Gradients).Module.FullyQualifiedName);
			path = Path.Combine(path, "..\\gradients");
			if (Directory.Exists(path)) {
				if (Extensions != null) Extensions(path);
				LoadXamlGradients(path);
				LoadGIMPGradients(path);
			}
		}

	}
}