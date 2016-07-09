using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;


namespace JohnsHope.FPlot.Library
{
	/// <summary>
	/// This class supports generic behavior of a plot.
	/// </summary>
	public partial class Plot {
		/// <summary>
		/// Draws the Plot to a Bitmap of size bounds and with PixelFormat pfmt.
		/// </summary>
		/// <param name="bounds">The size of the Bitmap</param>
		/// <param name="pfmt">The PixelFormat of the Bitmap</param>
		public Bitmap DrawToImage(Rectangle bounds, PixelFormat pfmt) {
			Rectangle bounds0 = new Rectangle(0, 0, bounds.Width, bounds.Height);
			Bitmap bmp = new Bitmap(bounds.Width, bounds.Height, pfmt);
			bool synch = SynchDraw;
			SynchDraw = true;
			Plot copy = this.Clone();
			copy.Model = this.Model.Clone();
			Graphics g = Graphics.FromImage(bmp);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			copy.Draw(g, bounds0);
			SynchDraw = synch;
			return bmp;
		}
		/// <summary>
		/// Draw the Plot to a Bitmap of size bounds and with PixelFormat.Format24bppRgb.
		/// </summary>
		/// <param name="bounds">The size of the Bitmap</param>
		public Bitmap DrawToImage(Rectangle bounds) { return DrawToImage(bounds, PixelFormat.Format24bppRgb); }
		/// <summary>
		/// Saves the Plot into an image file.
		/// </summary>
		/// <param name="filename">The filename where to store the file</param>
		/// <param name="bounds">The bounds of the image</param>
		/// <param name="format">The image format used to save the image</param>
		public void SaveAsImage(string filename, Rectangle bounds, ImageFormat format) {
			if (format == ImageFormat.Icon || format == ImageFormat.MemoryBmp ||
					format == ImageFormat.Wmf || format == ImageFormat.Emf) 
				throw new NotSupportedException("Plot.SaveAsImage: Image format not supported.");
			Bitmap img = DrawToImage(bounds);
			img.Save(filename, format);
		}
		/// <summary>
		/// Saves the Plot into a metafile. The Graphics object that is passed must have a valid HDC value,
		/// retrieved by g.GetHdc().
		/// </summary>
		/// <param name="g">A Graphics object with a valid HDC value (retrieved by g.GetHdc)</param>
		/// <param name="filename">The filename where to store the file</param>
		/// <param name="bounds">The bounds of the image</param>
		public void SaveAsMetafile(Graphics g, string filename, Rectangle bounds) {
			Rectangle bounds0 = new Rectangle(0, 0, bounds.Width, bounds.Height);
			// Graphics g2 = plot.CreateGraphics();
			IntPtr hdc = g.GetHdc();
			Metafile m = new Metafile(filename, hdc, bounds0, MetafileFrameUnit.Pixel, EmfType.EmfPlusDual);
			// Metafile m = new Metafile(filename, hdc, EmfType.EmfPlusDual);
			Plot copy = this.Clone();
			copy.Model = this.Model.Clone();
			Graphics g2 = Graphics.FromImage(m);
			bool synch = SynchDraw;
			SynchDraw = true;
			g2.SmoothingMode = SmoothingMode.HighQuality;
			copy.Draw(g2, bounds0);
			g2.Dispose();
			m.Dispose();
			g.ReleaseHdc();
		}
		/// <summary>
		/// Returns an ImageFormat accoring to the file extension of the supplied filename.
		/// </summary>
		public static ImageFormat GetImageFormat(string filename) {
			string ext = Path.GetExtension(filename).ToLower();
			switch (ext) {
				case ".gif": return ImageFormat.Gif;
				case ".jpg":
				case ".jpeg": return ImageFormat.Jpeg;
				case ".bmp": return ImageFormat.Bmp;
				case ".tif":
				case ".tiff": return ImageFormat.Tiff;
				case ".png": return ImageFormat.Png;
				case ".exif": return ImageFormat.Exif;
				case ".emf": return ImageFormat.Emf;
				case ".wmf": return ImageFormat.Wmf;
				case ".ico": return ImageFormat.Icon;
				default: throw new NotSupportedException("Plot.SaveAsImage: This file format is not supported.");
			}
		}
		/// <summary>
		/// Saves the Plot into a bitmap file. The ImageFormat is choosen according to the supplied filename.
		/// </summary>
		/// <param name="filename">The filename</param>
		/// <param name="bounds">The bounds of the image</param>
		public void SaveAsImage(string filename, Rectangle bounds) {
			ImageFormat fmt = GetImageFormat(filename);
			if (fmt == ImageFormat.Emf || fmt == ImageFormat.Icon || fmt ==	ImageFormat.MemoryBmp ||
				fmt == ImageFormat.Wmf) throw new NotSupportedException("Plot.SaveAsImage: Image format not supported.");
			SaveAsImage(filename, bounds, fmt);
		}
	}
}
