using System;

namespace Fractals {

	public class Mandelbrot {
  	public static int M = 500;

  	public static double f(double x, double y) {
    	double r = 0, i = 0, r2 = 0, i2 = 0, cr = x, ci = y;
    	for (int n = 0; n < M; n++) {
      	i = 2*r*i + ci;
      	r = r2 - i2 + cr;
      	r2 = r*r;
      	i2 = i*i;
      	if (r2 + i2 > 4) return n;
    	}
    	return -1;
  	}
  }

	public class Julia {
		public static int M = 500;
		
		public static double cr, ci;
		
		public static double f(double x, double y) {
    	double r = x, i = y, r2, i2;
    	for (int n = 0; n < M; n++) {
    		r2 = r*r;
    		i2 = i*i;
       	if (r2 + i2 > 4) return Math.Log(n); // return logarithmic color scale 
       	i = 2*r*i + ci;
      	r = r2 - i2 + cr;

    	}
    	return -1;
  	}
  }

}
