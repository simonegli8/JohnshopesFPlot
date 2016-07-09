using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using JohnsHope.FPlot;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// A class that implements 3D objects like 3D <see cref="Graphics3D.Matrix">Matrix</see> and 
	/// <see cref="Graphics3D.View">View</see>.
	/// This class is currently in alpha version stage.
	/// </summary>
	public class Graphics3D: GraphicsBase {

		/// <summary>
		/// A class derived from <see cref="GraphicsBase.Matrix"/>. Implements a special method <see cref="Reset"/> that can be
		/// used to intialize the matrix with a <see cref="PlotModel"/> to scale the Plot area to the cube from (-1, -1, -1) to (1, 1, 1) and a
		/// given rotational angle.
		/// </summary>
		[Serializable]
		public new class Matrix: GraphicsBase.Matrix {
			/// <summary>
			/// Resets the Matrix3D so that the bounds of the <see cref="PlotModel"/> are scaled to the cube from (-1, -1, -1) to
			/// (1, 1, 1), and turned by the angles specified in Angle.
			/// </summary>
			/// <param name="Model">The <see cref="PlotModel"/> to fit into the (-1, -1, -1)-(1, 1, 1) cube</param>
			/// <param name="Angle">The viewing angle</param>
			public void Reset(PlotModel Model, Point Angle) {
				double[,] M = Elements;
				// set Matrix so that the Models range fits into the (-1, -1, -1)-(1, 1, 1) cube
				double diff = Model.x1 - Model.x0, sum = Model.x1 + Model.x0;
				M[0, 0] = 2/diff; M[0, 1] = M[0, 2] = 0; M[0, 3] = -sum/diff;
				diff = Model.y1 - Model.y0; sum = Model.y1 + Model.y0;
				M[1, 0] = 0; M[1, 1] = 2/diff; M[1, 2] = 0; M[1, 3] = -sum/diff;
				diff = Model.z1 - Model.z0; sum = Model.z1 + Model.z0;
				M[2, 0] = M[2, 1] = 0; M[2, 2] = 2/diff; M[2, 3] = -sum/diff;
				M[3, 0] = M[3, 1] = M[3, 2] = 0; M[3, 3] = 1;

				Matrix rot = new Matrix();

				// rotate around z-axis
				rot.Reset();
				double cos = Math.Cos(Angle.z), sin = Math.Sin(Angle.z);
				rot[0, 0] = rot[1, 1] = cos;
				rot[0, 1] = -sin; rot[1, 0] = sin;
				Multiply(rot);

				// rotate around x-axis
				rot.Reset();
				cos = Math.Cos(Angle.x); sin = Math.Sin(Angle.x);
				rot[1, 1] = rot[2, 2] = cos;
				rot[1, 2] = -sin; rot[2, 1] = sin;
				Multiply(rot);

				// rotate around y-axis
				rot.Reset();
				cos = Math.Cos(Angle.y); sin = Math.Sin(Angle.y);
				rot[0, 0] = rot[2, 2] = cos;
				rot[0, 2] = -sin; rot[2, 0] = sin;
				Multiply(rot);
			}
			/// <summary>
			/// Initializes the matrix to the identity 4x4 matrix.
			/// </summary>
			public Matrix(): base(4) { }
			/// <summary>
			/// Creates a copy of the Matrix
			/// </summary>
			public new Matrix Clone() {
				Matrix A = new Matrix();
				A.CopyFrom(this);
				return A;
			}
		}
		/// <summary>
		/// This class describes the view properties of a 3D Plot. 
		/// </summary>
		[Serializable]
		public class View: GraphicsBase {
			[NonSerialized]
			Graphics3D.Matrix T3D;
			[NonSerialized]
			Graphics3D.Matrix T3Dinv;
			[NonSerialized]
			System.Drawing.Drawing2D.Matrix T2D;
			[NonSerialized]
			System.Drawing.Drawing2D.Matrix T2Dinv;
 
			/// <summary>
			/// The position of the screen. The screen position is then (0, Screen, 0).
			/// </summary>
			public double Screen;
			/// <summary>
			/// The position of the eye. The eye position is then (0, Eye, 0).
			/// </summary>
			public double Eye;
			private Point angle, depth;
			/// <summary>
			/// If true the Plot is drawn with perspective, if false, the Plot is drawn with parallelogramm mode.
			/// </summary>
			public bool Perspective = true;
			PlotModel model;
			/// <summary>
			/// The default constructor.
			/// </summary>
			public View() {
				T3D = new Graphics3D.Matrix();
				T3Dinv = new Graphics3D.Matrix();
				T2D = new System.Drawing.Drawing2D.Matrix();
				T2Dinv = new System.Drawing.Drawing2D.Matrix();
				Screen = 2;
				Eye = 3;
				depth = new Point(0.3, 0.3);
			}
			/// <summary>
			/// Copies from another View.
			/// </summary>
			public void CopyFrom(View src) {
				Screen = src.Screen;
				Eye = src.Eye;
				model = src.model;
				depth = src.depth;
				angle = src.angle;
				Perspective = src.Perspective;
				Reset();
			}
			/// <summary>
			/// Creates copy of the View.
			/// </summary>
			public View Clone() {
				View v = new View();
				v.CopyFrom(this);
				return v;
			}

			[OnDeserialized]
			void Deserialized(StreamingContext sc) {
				T3D = new Graphics3D.Matrix();
				T3Dinv = new Graphics3D.Matrix();
				T2D = new System.Drawing.Drawing2D.Matrix();
				T2Dinv = new System.Drawing.Drawing2D.Matrix();
				Model = model;
			}

			private void Reset() {
				T3D.Reset(model, angle);
				T3Dinv.Reset(model, angle);
				T3Dinv.Invert();
			}
			/// <summary>
			/// The <see cref="PlotModel"/> the View belongs to.
			/// </summary>
			public PlotModel Model {
				get { return model; }
				set {	model = value; Reset();	}
			}
			/// <summary>
			/// The rotation angle aroung the x axis.
			/// </summary>
			public double AngleX {
				get { return angle.x; }
				set { angle.x = value; Reset(); }
			}
			/// <summary>
			/// The rotation angle aroung the y axis.
			/// </summary>
			public double AngleY {
				get { return angle.y; }
				set { angle.y = value; Reset(); }
			}
			/// <summary>
			/// The rotation angle aroung the z axis.
			/// </summary>
			public double AngleZ {
				get { return angle.z; }
				set { angle.z = value; Reset(); }
			}
			/// <summary>
			/// Transforms the world coordinates to device coordinates.
			/// </summary>
			/// <param name="world">An array of world coordinates</param>
			/// <param name="device">An array of the resulting device coordinates. This array must be of the
			/// same length as world.</param>
			public void DeviceCoordinates(Point[] world, PointF[] device) {
				if (world.Length != device.Length) throw new ArgumentException("Graphics3D.DeviceCoordinates: world and device point arrays " +
					"must be of the same size."); 
				for (int i = 0; i < world.Length; i++) {
					device[i] = DeviceCoordinate(world[i]);
				} 
			}
			/// <summary>
			/// Transforms the given world coordinate into a device coordinate.
			/// </summary>
			/// <returns>Returns the corresponding device coordinate.</returns>
			public PointF DeviceCoordinate(Point world) {
				T3D.TransformPoint(ref world);
				float x, y;
				if (Perspective) {
					double f = Eye/(Screen + Eye + world.y);
					x = (float)(world.x*f);
					y = (float)(world.z*f);
				} else {
					x = (float)(world.x + world.y*depth.x);
					y = (float)(world.z + world.y*depth.y);
				}
				// Transform x and y accoring to T2D
				float[] M = T2D.Elements;
				return new PointF(M[0]*x + M[1]*y + M[4], M[2]*x + M[3]*y + M[5]);
			}
			/// <summary>
			/// Returns the world-length of the supplied device length at the specified point.
			/// </summary>
			/// <param name="length">The length in device coordinates</param>
			/// <param name="location">The point at which the lenght is measured</param>
			/// <returns>Returns the length in world coordinates.</returns>
			public double WorldSize(float length, Point location) {
				double l;
				Point p;

				if (Perspective) {
					T3D.TransformPoint(ref location);
					double f = Eye/(Screen + Eye + location.y);
					l = length/f;
				} else l = length;
				
				p.x = l; p.y = 0; p.z = 0;
				T3Dinv.TransformPoint(ref p);
				return Math.Sqrt(p.x*p.x + p.y*p.y + p.z*p.z);
			}
			/// <summary>
			/// Returns the edges of a cube that has the size of the <see cref="PlotModel"/>'s bounds.
			/// </summary>
			/// <returns></returns>
			public GraphicsBase.Point[] BoundsCube() {
				GraphicsBase.Point[] cube = new GraphicsBase.Point[8];
				cube[0].x = model.x0;	cube[0].y = model.y0;	cube[0].z = model.z0;
				cube[1].x = model.x1;	cube[1].y = model.y0;	cube[1].z = model.z0;
				cube[2].x = model.x1;	cube[2].y = model.y1;	cube[2].z = model.z0;
				cube[3].x = model.x0;	cube[3].y = model.y1;	cube[3].z = model.z0;
				cube[4].x = model.x0;	cube[4].y = model.y0;	cube[4].z = model.z1;
				cube[5].x = model.x1;	cube[5].y = model.y0;	cube[5].z = model.z1;
				cube[6].x = model.x1;	cube[6].y = model.y1;	cube[6].z = model.z1;
				cube[7].x = model.x0;	cube[7].y = model.y1;	cube[7].z = model.z1;
				return cube;
			}
			/// <summary>
			/// Resets the Bounds of the View to an indeterminated value.
			/// </summary>
			public void ResetBounds() {
				T2D = new System.Drawing.Drawing2D.Matrix();
				T2Dinv = new System.Drawing.Drawing2D.Matrix();
			}
			/// <summary>
			/// Returns a rectangle with the device coordinate bounds of the <see cref="PlotModel"/>'s bounds.
			/// </summary>
			/// <param name="Model"></param>
			/// <returns></returns>
			public RectangleF Bounds(PlotModel Model) {
				Point[] cube = BoundsCube();
				PointF[] cubed = new PointF[8];
				Norms norms = new Norms();
				DeviceCoordinates(cube, cubed);
				norms.Add(cubed);
				return norms.Bounds;
			}
			/// <summary>
			/// Sets the View parameters so that the <see cref="PlotModel"/>'s bounds world coordinates transform to device
			/// coordinates will fit in to the <see cref="Rectangle"/> bounds.
			/// </summary>
			public void SetBounds(PlotModel Model, Rectangle bounds) {
				ResetBounds();
				RectangleF b = Bounds(Model);
				PointF[] v = new PointF[3];
				v[0].X = bounds.X; v[0].Y = bounds.Y;
				v[1].X = bounds.X + bounds.Width; v[1].Y = bounds.Y;
				v[2].X = bounds.X; v[2].Y = bounds.Y + bounds.Height;
				T2D = new System.Drawing.Drawing2D.Matrix(b, v);
				T2Dinv = T2D.Clone();
				T2Dinv.Invert();
			}
		}
	}
}
