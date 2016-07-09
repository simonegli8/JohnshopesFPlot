using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// A class that allows fast access to the pixels of a bitmap
	/// </summary>
	public class BitmapBuilder {
		private Bitmap bitmap;
		private BitmapData data = null;
		private Graphics g;
		/// <summary>
		/// The <see cref="Bitmap"/>.
		/// </summary>
		public Bitmap Bitmap {
			get {
				lock(this) {
					return bitmap;
				}
			}
			set {
				lock(this) {
					bitmap = value;
				}
			}
		}
		/// <summary>
		/// A Graphics object that can be used to draw to the bitmap.
		/// </summary>
		public Graphics Graphics {
			get { return g; }
		}
		/// <summary>
		/// Locks the bitmap to allow accessing individual pixels.
		/// </summary>
		public void Lock() {
			Monitor.Enter(this);
			data = bitmap.LockBits(new Rectangle(0, 0, Bounds.Width, Bounds.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		}
		/// <summary>
		/// Unlocks the bitmap.
		/// </summary>
		public void Unlock() {
			bitmap.UnlockBits(data);
			data = null;
			Monitor.Exit(this);
		}
		/// <summary>
		/// The bitmaps bounds.
		/// </summary>
		public Rectangle Bounds;
		/// <summary>
		/// The default constructor.
		/// </summary>
		public BitmapBuilder() {}
		/// <summary>
		/// Creates a bitmap of the specified size with the <see cref="PixelFormat.Format32bppArgb"/>.
		/// </summary>
		public BitmapBuilder(Rectangle Bounds) {
			this.Bounds = Bounds;
			bitmap = new Bitmap(Bounds.Width, Bounds.Height, PixelFormat.Format32bppArgb);
			g = Graphics.FromImage(bitmap);
			Matrix T = g.Transform;
			T.Translate(-Bounds.X, -Bounds.Y);
			g.Transform = T;
		}
		/// <summary>
		/// Copies a BitmapBuilder from another BitmapBuilder.
		/// </summary>
		/// <param name="b"></param>
		public void CopyFrom(BitmapBuilder b) {
			bitmap = b.bitmap;
			data = b.data;
			g = b.g;
			Bounds = b.Bounds;
		}
		/// <summary>
		/// Creates a copy of the BitmapBuilder.
		/// </summary>
		public BitmapBuilder Clone() {
			BitmapBuilder b = new BitmapBuilder();
			b.CopyFrom(this);
			return b;
		}
		/// <summary>
		/// Returns a <c>int*</c> pointer to the specified pixel. The pixel coordinates are taken relative to the
		/// <c>Bounds.Location</c> passed to the constructor of the BitmapBuilder.
		/// </summary>
		/// <param name="x">The x coordinate of the pixel.</param>
		/// <param name="y">The y coordinate of the pixel.</param>
		public unsafe int* Pixel(int x, int y) {
			int* pixel = (int*)data.Scan0 + (y - Bounds.Y)*Bounds.Width + (x - Bounds.X);
			return pixel;
		}
		/// <summary>
		/// Sets the corresponding pixel. The pixel coordinates are taken relative to the
		/// <c>Bounds.Location</c> passed to the constructor of the BitmapBuilder.
		/// </summary>
		/// <param name="x">The x coordinate of the pixel.</param>
		/// <param name="y">The y coordinate of the pixel.</param>
		/// <param name="color">The color to set the pixel to.</param>
		public void SetPixel(int x, int y, Color color) {
			unsafe {
				int* pixel = (int*)data.Scan0 + (y - Bounds.Y)*Bounds.Width + (x - Bounds.X);
				*pixel = color.ToArgb();
			}
		}
		/// <summary>
		/// Paints the bitmap to the <see cref="Graphics"/> g if the bitmap is not locked. Returns true if the bitmap was painted
		/// </summary>
		public bool TryPaint(Graphics g) {
			if (Monitor.TryEnter(this)) {
				g.DrawImage(bitmap, Bounds.Location);
				Monitor.Exit(this);
				return true;
			}
			return false;
		}
	}
}
