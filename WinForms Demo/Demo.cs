using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using JohnsHope.FPlot.Library;

namespace FPlotDemo {

	public class Demo { // a class that implements demo routines

		public static void Sin(PlotControl plot) {
			
			// We create a new Function1DItem, set its parameters and add it to the
			// PlotModel plot.
			Function1DItem sin = new Function1DItem();
			// The source represents the body of the following function:
			//
			// double[] p, dfdp;
			// double f(double x) {
			//  ...
			// }
			sin.Source = "return sin(x);";
			sin.Compile();
			sin.Color = Color.Blue;
			sin.LineWidth = 2;
			plot.Model.Add(sin);
		}

		// Here we implement a custom function.
		private class CosFunction: CustomFunction1D {
			public override double f(double x) {
				NEval++; // Icreases the counter of the number of function
					//evaluations
				return Math.Cos(x);
			}
		}

		public static void Cos(PlotControl plot) {

			// We create a new CosFunction, set its parameters and add it to the
			// PlotModel plot.
			CosFunction cos = new CosFunction();
			cos.Color = Color.Red;
			cos.LineWidth = 2;
			cos.LineStyle = DashStyle.Dash;
			plot.Model.Add(cos);
		}

		public static void Gauss(PlotControl plot) {

			// Here we create a new Function1DItem, directly setting and compiling
			// its source throught the constructor.
			Function1DItem gauss = new Function1DItem(
			// Here the source accesses the array p, an array of function parameters.
			// When you compile the item, the size of p is automatically set to the
			// highest element referred to in the source.
				@"
					double arg = (x-p[0])/p[1];
					return p[2]*exp(-arg*arg);
				");

			gauss.p[0] = 1; // The position of the curve's peak
			gauss.p[1] = 1; // The width of the peak
			gauss.p[2] = 4; // The height of the peak
			plot.Model.Add(gauss);
		}

		public static void Mandelbrot(PlotControl plot) {
			// Here we add the Mandelbrot set to the plot.

			Function2DItem m = new Function2DItem(
			// The source represents the body of the following function:
			// double[] p, dfdp;
			// double f(double x, double y) {
			//   ...
			// }
				@"
					double xn = 0, yn = 0, x2 = 0, y2 = 0;
					for (int n = 0; n < 500; n++) {
						yn = 2*xn*yn + y;
						xn = x2 - y2 + x;
						x2 = xn*xn; y2 = yn*yn; 
						if (x2 + y2 > 4) return n;
					}
					return -1;
				");

			plot.Model.FixXtoY = true; // We fix the x-plotrange to the y-plotrange, so the
				// proportions of the plot will be correct.
			plot.SetRange(plot.x0, plot.x1, plot.y0, plot.y1, 0, 20);
				// We set the view range of the plot, so the Mandelbrot set will be
				// fully visible. We set the z-plotrange from 0 to 20.
			plot.Model.Add(m);
		}

		public static void Text(PlotControl plot) {
			// Loads a DataItem from a text csv file

			DataItem data = new DataItem();
			data.Dimensions = 2; // Load the x and y column
			data.ErrorColumns = true; // Load the dx and dy column
			try {
				// Load Text data
				data.LoadText("data.csv", " ,;\n\r\t"); // Load text data where the
					// numbers are separated by
				// spaces, commas, semicolons, newlines, carriage-returns, and tabs. 
			} catch (Exception ex) {
				MessageBox.Show("Could not open the file data.csv\n" + ex.Message);
			}
			plot.Model.Add(data);
		}

		public static void WAV(PlotControl plot) {
			DataItem data = new DataItem();
			data.Lines = true;	// join data points with a line
			data.Marks = false; // draw no error marks
			data.Color = Color.Green;
			data.Dimensions = 2;	// use x and y columns of the DataItem
			data.ErrorColumns = false; // WAV data contains no error info
			try {
				// Load WAV data
				data.LoadWAV("test.wav");
			} catch (Exception ex) {
				MessageBox.Show("Could not open the file test.wav\n" + ex.Message);
			}
			plot.SetRange(0, 1, -1, 1);
			plot.Model.Add(data);
		}


	}
}
