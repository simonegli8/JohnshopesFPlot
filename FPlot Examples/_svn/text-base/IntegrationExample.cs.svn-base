using System;
using System.Collections.Generic;
using JohnsHope.FPlot.Library;
using JohnsHope.Analysis;

namespace IntegrationExample {

	public class SinePeak {
	
		public SmallData Parameters;
	
		public double Peak(double x) {
		
			IList<double> p = Parameters;
			// p[0]: x-position of the peak
			// p[1]: peak amplitude
			// p[2]: peak width
			// p[3]: sine frequency

			double arg, ex, sin;
			arg = (x - p[0])/p[2];
			ex = p[1]*Math.Exp(-arg*arg);
			sin = Math.Sin(2*p[3]*Math.PI*x);
			return ex*sin;
		}

		public double Integral(double x) {
			double err;
			return Int.q(Peak, 0, x, 0, 0.2, out err);
		}
	}
}
