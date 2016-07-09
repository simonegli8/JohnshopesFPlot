using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

using JohnsHope.FPlot.Library;

namespace FPlotDemo {
	public class FitDemo {

		PlotControl plot;
		DataItem data;
		Function1DItem f;
		int delay = 300;

		public FitDemo(PlotControl plot) {
			this.plot = plot;

			plot.Model.Clear();

			// create a new DataItem
			data = new DataItem();

			data.Dimensions = 2;	// This property is only used by the LoadText method, it is not required for the fit algorithm.
			data.ErrorColumns = true;	// This property is only used by the LoadText method, it is not required for the fit algorithm.

			// preset the x, dx and dy values to functions:
			data.x.Source = "n/Length*12 - 8"; // place the data in the visible plot range (x0: -8; x1: 4 in this demo application)
			data.dx.Source = "0.5";	// set the x errors to 0.5 (the x errors are not used by the fit algorithm)
			data.dy.Source = "y[n]*0.2"; // set y errors to 20% of the x values
			data.Compile();

			data.LoadText("Gauss data.txt", "\n"); // Load the data from the "Gauss data.txt" text file.

			// Display data
			plot.Model.Add(data);

			// create a new function
			f = new Function1DItem(@"
				// M - gauss curves with derivative information in dfdp.
				const int M = 3;
				double arg, ex, fac, sum = 0;
				for (int n = 0; n < 3*M; n += 3) {
					arg = (x - p[n + 1])/p[n + 2];
					ex = Math.Exp(-arg*arg);
					fac = p[n]*ex*2*arg;

					// Compute derivative information in order to speed up the Marquardt fitting algorithm. Marquardt also works with no
					// derivative information provided by the funciton, so you can also omit this, it then computes numerical derivatives.
					// The NelderMead fitting algorithm uses no derivatives.
					dfdp[n] = ex;
					dfdp[n + 1] = fac/p[n + 2];
					dfdp[n + 2] = fac*arg/p[n + 2];

					// compute the sum over all gauss curves.
					sum += p[n]*ex;
				}
				return sum;
			");
			f.Compile();

			// Set initial fit guess.

			ResetGuess();

			// Display function
			plot.Model.Add(f);
		}

		void ResetGuess() {
			// Set initial fit guess.

			// Set peak heights
			f.p[0] = f.p[3] = f.p[6] = 2;
			// Set peak widths
			f.p[2] = f.p[5] = f.p[8] = 1.5;
			// Set peak positions
			f.p[1] = -5.5;
			f.p[4] = -2.8;
			f.p[7] = 2;
		}

		public void Run() { // runs the fit demo on a ThreadPool thread.
			ThreadPool.QueueUserWorkItem(new WaitCallback(Demo));
		}

		private void Demo(object target) {
	
			// Fit synchronously using Marquardt algorithm

			ResetGuess();
			MessageBox.Show("Synchronous Marquardt fit algorithm");
			// Wait 2 seconds
			Thread.Sleep(2000);

			f.Color = Color.Red; // set function color to red
			plot.Model.Update(f);

			Fit fit = new Fit(data, f, Fit.FitAlgorithm.Marquardt);
			fit.StepInterval = 0; // set minimal time interval between Step events to 0, in order to show the progress of the fit.
			fit.Step += FitStep;

			fit.Solve(); // Start synchronous Solve
			
			f.Color = Color.Black; // fitting finished, set function color back to black
			plot.Model.Update(f);

			if (fit.Error) { // An exception occured in the fitting algorithm
				MessageBox.Show("Error: " + fit.Exception.Message);
			}

			// Fit asynchronously using NelderMead algorithm

			ResetGuess();
			MessageBox.Show("Asynchronous Nelder & Mead fit algorithm");

			// Wait 2 seconds
			Thread.Sleep(2000);

			f.Color = Color.Red; // set function color to red
			plot.Model.Update(f);

			fit = new Fit(data, f, Fit.FitAlgorithm.NelderMead);

			fit.StepInterval = 0; // set minimal time interval between Step events to 0, in order to show the progress of the fit.
			delay = 75; // set up speed of Nelder & Mead fit faster than Marquardt fit, because Nelder & Mead will invoke the Step event
									// more often.
			fit.Step += FitStep;

			IAsyncResult res = fit.BeginSolve(null, null); // Start asynchronous Solve

			fit.EndSolve(res); // wait for Solve completion

			f.Color = Color.Black; // fitting finished, set function color back to black
			plot.Model.Update(f);

			if (fit.Error) { // An exception occured in the fitting algorithm
				MessageBox.Show("Error: " + fit.Exception.Message);
			}

		}

		private void FitStep(Fit fit) {
			plot.Model.Update(fit.Function);
			Thread.Sleep(delay); // sleep 0.3 seconds between fitting steps.
		}
	}
}
