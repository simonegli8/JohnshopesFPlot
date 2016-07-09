using System;
using JohnsHope.FPlot.Library;

public partial class _Default : System.Web.UI.Page {

	protected void Page_Load(object sender, EventArgs e) {
		if (!Page.IsPostBack) {
			FunctionChanged(null, null); // reset the plot's content
			CreateMandelbrot(); // create the Mandelbrot function.
		}
	}
	
	protected void FunctionChanged(object sender, EventArgs e) {
		plot.Model.Clear(); // The state of the plot is saved across page requests, so we need to clear the Model.
		plot.Model.SetRange(-10, 10, -1.1, 1.1); // set the plotting range for plot
		if (list.SelectedValue == "Sine") {
			Function1DItem sin = new Function1DItem("return sin(x);");
			plot.Model.Add(sin);
		} else if (list.SelectedValue == "Cosine") {
			Function1DItem cos = new Function1DItem("return cos(x);");
			plot.Model.Add(cos);
		}
		plot.Model.y.fix = true; // disable zooming in the y scale.
	}

	void CreateMandelbrot() {
		Function2DItem mandelbrot = new Function2DItem(); // create a new 2D function

		//The source below represents the body of the following function:
		//double[] p, dfdp;
		//double f(double x, double y) {
		//  ...
		//}
		//The array p is an array of function parameters (that can be fitted for).
		//dfdp is an array of the derivatives of the function
		//(this array is also used for fitting).

		mandelbrot.Source = @"
			double xn = 0, yn = 0, x2 = 0, y2 = 0;
			for (int n = 0; n < p[0]; n++) {
				yn = 2*xn*yn + y;
				xn = x2 - y2 + x;
				x2 = xn*xn; y2 = yn*yn;
				if (x2 + y2 > 4) return n;
			}
			return -1;
		";
		mandelbrot.Compile(); // compile the function
		mandelbrot.p[0] = 500; // set the maximum of iterations value to 500
		mandelbrot.Gradient = new RainbowGradient(); // sets the color gradient used to plot the function to a rainbow gradient.

		plot2.Model.SetRange(-2.3, 1.3, -1.3, 1.3, 0, 20); // sets the plot range.
		plot2.Model.FixXtoY = true; // set FixXtoY to true, so x- and y-scale will be proportional.
		plot2.Model.Add(mandelbrot);
	}
}
