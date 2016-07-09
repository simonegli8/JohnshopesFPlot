using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// A class that implements basic graphics objects like <see cref="GraphicsBase.Point">Point</see> and
	/// <see cref="GraphicsBase.Matrix">Matrix</see>.
	/// </summary>
	[Serializable]
	public class GraphicsBase {
		/// <summary>
		/// A constant that defines the white-space border of the control.
		/// </summary>
		public const int Border = 12;		
		/// <summary>
		/// A struct that denotes a two- or threedimensional point in world coordinates.
		/// </summary>
		[Serializable]
		public struct Point {
			/// <summary>
			/// The x coordinate of the Point.
			/// </summary>
			public double x;
						/// <summary>
			/// The y coordinate of the Point.
			/// </summary>
			public double y;
						/// <summary>
			/// The z coordinate of the Point.
			/// </summary>
			public double z;
			/// <summary>
			/// Returns a string representation of the Point.
			/// </summary>
			/// <returns></returns>
			public override string ToString() {
				return "(" + x + ", " + y + ", " + z + ")";
			}
			/// <summary>
			/// Initializes a 2D Point
			/// </summary>
			public Point(double x, double y) {
				this.x = x; this.y = y; this.z = 1;
			}
			/// <summary>
			/// Initializes a 3D Point
			/// </summary>
			public Point(double x, double y, double z) {
				this.x = x; this.y = y; this.z = z;
			}
		}
		/// <summary>
		/// A class that denotes a square matrix used to transform Points.
		/// </summary>
		[Serializable]
		public class Matrix {
			/// <summary>
			/// A two dimensional array containing the fields of the matrix. 
			/// </summary>
			public double[,] Elements;
			/// <summary>
			/// Gets or sets an individual field of the matrix.
			/// </summary>
			/// <param name="row">The row of the field</param>
			/// <param name="column">The column of the field</param>
			/// <returns></returns>
			public double this[int row, int column] {
				get { return Elements[row, column]; }
				set { Elements[row, column] = value; }
			}
			/// <summary>
			/// Resets the matrix to the identity matrix.
			/// </summary>
			public void Reset() {
				int n = Elements.GetLength(0);
				for (int r = 0; r < n; r++) {
					for (int c = 0; c < n; c++) {
						if (r != c) Elements[r, c] = 0;
						else Elements[r, c] = 1;
					}
				}
			}
			/// <summary>
			/// Gets or sets the dimensionality of the matrix. If the dimensionality is changed the matrix is reset to the identity matrix.
			/// </summary>
			public virtual int N {
				get { return Elements.GetLength(0); }
				set {
					if (Elements == null || value != Elements.GetLength(0)) {
						Elements = new double[value, value];
						Reset();
					}
				}
			}
			/// <summary>
			/// Creates a new Matrix of size NxN.
			/// </summary>
			public Matrix(int N) {
				this.N = N;
			}
			/// <summary>
			/// Creates a new Matrix with the supplied Elements.
			/// </summary>
			public Matrix(double[,] Elements) {
				this.Elements = (double[,])Elements.Clone();
			}
			/// <summary>
			/// Returns a string representation of the matrix
			/// </summary>
			/// <returns></returns>
			public override string ToString() {
				int n = Elements.GetLength(0);
				StringBuilder s = new StringBuilder("[");
				for (int r = 0; r < n; r++) {
					s.Append("[");
					for (int c = 0; c < n; c++) {
						s.Append(Elements[r, c]);
						if (c < n-1) s.Append(", ");
						else s.Append("]");
					}
					if (r < n-1) s.Append("\n");
					else s.Append("]\n");
				}
				return s.ToString();
			}
			/// <summary>
			/// Copies from another Matrix.
			/// </summary>
			public void CopyFrom(Matrix A) {
				Elements = (double[,])A.Elements.Clone();
			}
			/// <summary>
			/// Creates a copy of the Matrix
			/// </summary>
			public Matrix Clone() {
				Matrix A = new Matrix(this.N);
				A.CopyFrom(this);
				return A;
			}
			/// <summary>
			/// Multiplies the Matrix with the matrix A
			/// </summary>
			/// <param name="A"></param>
			public void Multiply(Matrix A) { Multiply(A, MatrixOrder.Append); }
			/// <summary>
			/// Multiplies the Matrix with the Matrix A
			/// </summary>
			/// <param name="A">The Matrix to multiply with</param>
			/// <param name="order">The MatirxOrder specifying the order in which to multiply the matrices.
			/// MatrixOrder.Prepend corresponds to this*A and MatrixOrder.Append to A*this</param>
			public void Multiply(Matrix A, MatrixOrder order) {
				int i, j, k, n;
				n = Elements.GetLength(0);
				if (n != A.Elements.GetLength(0)) throw new ArgumentException("Matrix.Multiply: Matrices must be of the same size");
				double[,] C = (double[,])Elements.Clone();
				for (i = 0; i < n; i++) {
					for (j = 0; j < n; j++) {
						double sum = 0;
						if (order == MatrixOrder.Prepend) {
							for (k = 0; k < n; k++) sum += C[i, k]*A.Elements[k, j];
						} else {
							for (k = 0; k < n; k++) sum += A.Elements[i, k]*C[k, j];
						}
						Elements[i, j] = sum;
					}
				}
			}
			/// <summary>
			/// Matrix multiplication.
			/// </summary>
			public static Matrix operator*(Matrix A, Matrix B) {
				int i, j, k, n;
				n = A.Elements.GetLength(0);
				if (n != B.Elements.GetLength(0)) throw new ArgumentException("Matrix.*: Matrices must be of the same size");
				Matrix C = new Matrix(n);
				for (i = 0; i < n; i++) {
					for (j = 0; j < n; j++) {
						double sum = 0;
						for (k = 0; k < n; k++) sum += A.Elements[i, k]*B.Elements[k, j];
						C.Elements[i, j] = sum;
					}
				}
				return C;
			}
			/// <summary>
			/// Inverts the Matrix.
			/// </summary>
			public void Invert() {
				int i, pcol = 0, prow = 0, j, k, l, n;
				n = Elements.GetLength(0);
				int[] c = new int[n], r = new int[n], pivot = new int[n];
				double big, pinv, t;
				double[,] M = Elements;

				for (i = 0; i < n; i++) {
					big = 0;
					for (j = 0; j < n; j++) { // search pivot
						if (pivot[j] != 1) {
							for (k = 0; k < n; k++) {
								if (pivot[k] == 0) {
									if (Math.Abs(M[j, k]) >= big) {
										big = Math.Abs(M[j, k]);
										prow = j; pcol = k;
									}
								} else if (pivot[k] > 1) throw new System.ArgumentException("Matrix.Invert: Singular Matrix");
							}
						}
					}
					pivot[pcol]++;

					if (prow != pcol) {
						for (k = 0; k < n; k++) {
							t = M[prow, k]; M[prow, k] = M[pcol, k]; M[pcol, k] = t;
						}
					}
					if (M[pcol, pcol] == 0) throw new System.ArgumentException("Matrix.Invert: Singular Matrix");
					r[i] = prow;
					c[i] = pcol;
					pinv = 1/M[pcol, pcol];
					M[pcol, pcol] = 1;	// reduce rows
					for (k = 0; k < n; k++) M[pcol, k] *= pinv;
					for (k = 0; k < n; k++) {
						if (k != pcol) {
							t = M[k, pcol];
							M[k, pcol] = 0;
							for (l = 0; l < N; l++) M[k, l] -= M[pcol, l]*t;
						}
					}
				}
				for (l = n-1; l >= 0; l--) { // swap column ordering
					if (r[l] != c[l]) {
						for (k = 0; k < n; k++) {
							t = M[k, r[l]]; M[k, r[l]] = M[k, c[l]]; M[k, c[l]] = t;
						}
					}
				}
			}
			/// <summary>
			///  Transforms the Point p
			/// </summary>
			public void TransformPoint(ref Point p) {
				int n;
				double x, y, z;
				n = Elements.GetLength(0);
				switch (n) {
				case 2:
					x = Elements[0, 0]*p.x + Elements[0, 1]*p.y;
					y = Elements[1, 0]*p.x + Elements[1, 1]*p.y;
					p.x = x; p.y = y;
					break;
				case 3:
					x = Elements[0, 0]*p.x + Elements[0, 1]*p.y + Elements[0, 2]*p.z;
					y = Elements[1, 0]*p.x + Elements[1, 1]*p.y + Elements[1, 2]*p.z;
					z = Elements[2, 0]*p.x + Elements[2, 1]*p.y + Elements[2, 2]*p.z;
					p.x = x; p.y = y; p.z = z;
					break;
				case 4:
					x = Elements[0, 0]*p.x + Elements[0, 1]*p.y + Elements[0, 2]*p.z + Elements[0, 3];
					y = Elements[1, 0]*p.x + Elements[1, 1]*p.y + Elements[1, 2]*p.z + Elements[1, 3];
					z = Elements[2, 0]*p.x + Elements[2, 1]*p.y + Elements[2, 2]*p.z + Elements[2, 3];
					p.x = x; p.y = y; p.z = z;
					break;
				default:
					throw new ArgumentException("TransformPoint: Invalid matrix size");
				}
			}
			/// <summary>
			/// Transforms all Points in p.
			/// </summary>
			public void TransformPoints(Point[] p) {
				for (int i = 0; i < p.Length; i++) {
					TransformPoint(ref p[i]);
				}
			}
		}

		/// <summary>
		/// Draws a Legend
		/// </summary>
		/// <param name="g">The <see cref="Graphics"/> to paint to</param>
		/// <param name="plot">The <see cref="Plot"/> for which to paint a legend</param>
		public static void DrawLegend(Graphics g, Plot plot) {
			if (plot.Model.Legend) {

				SizeF size = g.MeasureString("0.5", plot.Model.ScaleFont);
				float fd = Math.Max(1, (int)(size.Height + 0.5F));
				size = g.MeasureString("0.5", plot.Model.LegendFont);
				float fld = Math.Max(1, (int)(size.Height + 0.5F));

				Pen pen = new Pen(plot.Model.ScaleColor);
				SolidBrush brush = new SolidBrush(plot.Model.ScaleColor);
				float lw = 0;
				int n, m = 0;
				for (n = 0; n < plot.Model.Count; n++) {
					if (plot.Model[n] is Function1DItem ||
						(plot.Model[n] is DataItem && ((DataItem)plot.Model[n]).Lines)) {
						m++;
						size = g.MeasureString(plot.Model[n].Name, plot.Model.LegendFont);
						lw = Math.Max(lw, size.Width);
					}
				}
				float w = (float)Math.Round(lw+0.5) + (7*fd)/2, h = fld*m + Border;

				if (plot.Model.LegendBorder) {
					brush.Color = Color.White;
					g.FillRectangle(brush, plot.Bounds.X + plot.Bounds.Width - 2*fd - w, 2*fd, w, h);
					g.DrawRectangle(pen, plot.Bounds.X + plot.Bounds.Width - 2*fd - w, 2*fd, w, h);
				}

				m = 0;
				for (n = 0; n < plot.Model.Count; n++) {
					if (plot.Model[n] is ILine && (!(plot.Model[n] is DataItem) || ((DataItem)plot.Model[n]).Lines)) {
						brush.Color = plot.Model.ScaleColor;
						g.DrawString(plot.Model[n].Name, plot.Model.LegendFont, brush, plot.Bounds.X + plot.Bounds.Width + fd - w, 2*fd + Border/2 + m*fld);
						pen.DashStyle = ((ILine)plot.Model[n]).LineStyle;
						pen.Width = ((ILine)plot.Model[n]).LineWidth;
						g.DrawLine(pen, plot.Bounds.X + plot.Bounds.Width - w - (3*fd)/2, 2*fd + Border/2 + m*fld + fld/2,
							plot.Bounds.X + plot.Bounds.Width - w + fd/2, 2*fd + Border/2 + m*fld + fld/2);
						m++;
					}
				}
			}
		}
	}
}
