using System;

namespace JohnsHope.Analysis {
	/* integration/qk15.c
	 * 
	 * Copyright (C) 1996, 1997, 1998, 1999, 2000 Brian Gough
	 * 
	 * This program is free software; you can redistribute it and/or modify
	 * it under the terms of the GNU General Public License as published by
	 * the Free Software Foundation; either version 2 of the License, or (at
	 * your option) any later version.
	 * 
	 * This program is distributed in the hope that it will be useful, but
	 * WITHOUT ANY WARRANTY; without even the implied warranty of
	 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
	 * General Public License for more details.
	 * 
	 * You should have received a copy of the GNU General Public License
	 * along with this program; if not, write to the Free Software
	 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.
	 */


	/* Gauss quadrature weights and kronrod quadrature abscissae and
		 weights as evaluated with 80 decimal digit arithmetic by
		 L. W. Fullerton, Bell Labs, Nov. 1981. */

	public static partial class Int {

		delegate void IntegrationRule(IFunction1D f, double a, double b,
													out double result, out double abserr,
													out double resabs, out double resasc, Workspace workspace);

		delegate void IntegrationRuleDelegate(Function1DDelegate f, double a, double b,
											out double result, out double abserr,
											out double resabs, out double resasc, Workspace workspace);


		static IntegrationRule r15 = new IntegrationRule(gsl_integration_qk15);
		static IntegrationRule r21 = new IntegrationRule(gsl_integration_qk21);
		static IntegrationRule r31 = new IntegrationRule(gsl_integration_qk31);
		static IntegrationRule r41 = new IntegrationRule(gsl_integration_qk41);
		static IntegrationRule r51 = new IntegrationRule(gsl_integration_qk51);
		static IntegrationRule r61 = new IntegrationRule(gsl_integration_qk61);

		static IntegrationRuleDelegate r15d = new IntegrationRuleDelegate(gsl_integration_qk15);
		static IntegrationRuleDelegate r21d = new IntegrationRuleDelegate(gsl_integration_qk21);
		static IntegrationRuleDelegate r31d = new IntegrationRuleDelegate(gsl_integration_qk31);
		static IntegrationRuleDelegate r41d = new IntegrationRuleDelegate(gsl_integration_qk41);
		static IntegrationRuleDelegate r51d = new IntegrationRuleDelegate(gsl_integration_qk51);
		static IntegrationRuleDelegate r61d = new IntegrationRuleDelegate(gsl_integration_qk61);


		class Workspace {
			public int limit = 0, size, nrmax, i, maximum_level;
			public double[] alist, blist, rlist, elist;
			public int[] order, level;

			public Workspace(int n) {
				if (n == 0) { throw new ArgumentException("workspace length n must be positive integer"); }
				if (n > limit) {
					size = 0;
					limit = n;
					maximum_level = 0;
					alist = new double[n];
					blist = new double[n];
					rlist = new double[n];
					elist = new double[n];
					order = new int[n];
					level = new int[n];
				}
			}
			
			public void initialise(double a, double b) {
				size = 0;
				nrmax = 0;
				i = 0;
				alist[0] = a;
				blist[0] = b;
				rlist[0] = 0.0;
				elist[0] = 0.0;
				order[0] = 0;
				level[0] = 0;
				maximum_level = 0;
			}
	
			public void set_initial_result(double result, double error) {
				size = 1;
				rlist[0] = result;
				elist[0] = error;
			}

			public void update(double a1, double b1, double area1, double error1, double a2, double b2, double area2, double error2) {

				int i_max = i;
				int i_new = size;

				int new_level = level[i_max] + 1;

				/* append the newly-created intervals to the list */

				if (error2 > error1) {
					alist[i_max] = a2;        /* blist[maxerr] is already == b2 */
					rlist[i_max] = area2;
					elist[i_max] = error2;
					level[i_max] = new_level;

					alist[i_new] = a1;
					blist[i_new] = b1;
					rlist[i_new] = area1;
					elist[i_new] = error1;
					level[i_new] = new_level;
				} else {
					blist[i_max] = b1;        /* alist[maxerr] is already == a1 */
					rlist[i_max] = area1;
					elist[i_max] = error1;
					level[i_max] = new_level;

					alist[i_new] = a2;
					blist[i_new] = b2;
					rlist[i_new] = area2;
					elist[i_new] = error2;
					level[i_new] = new_level;
				}

				size++;

				if (new_level > maximum_level) maximum_level = new_level;

				qpsrt();
			}

			public void retrieve(out double a, out double b, out double r, out double e) {
				a = alist[i];
				b = blist[i];
				r = rlist[i];
				e = elist[i];
			}

			public double sum_results() {
				int n = size;
				double result_sum = 0;

				for (int k = 0; k < n; k++) result_sum += rlist[k];

				return result_sum;
			}

			public bool increase_nrmax() {
				int k;
				int id = nrmax;
				int jupbnd;

				int last = size - 1;

				if (last > (1 + limit / 2)) {
					jupbnd = limit + 1 - last;
				} else {
					jupbnd = last;
				}

				for (k = id; k <= jupbnd; k++) {
					int i_max = order[nrmax];

					i = i_max;

					if (level[i_max] < maximum_level) {
						return true;
					}

					nrmax++;
				}
				return false;
			}

			public bool large_interval() { return level[i] < maximum_level; }

			public void qpsrt() {
				int last = size - 1;

				double errmax;
				double errmin;
				int i, k, top;

				int i_nrmax = nrmax;
				int i_maxerr = order[i_nrmax];

				/* Check whether the list contains more than two error estimates */

				if (last < 2) {
					order[0] = 0;
					order[1] = 1;
					i = i_maxerr;
					return;
				}

				errmax = elist[i_maxerr];

				/* This part of the routine is only executed if, due to a difficult
					 integrand, subdivision increased the error estimate. In the normal
					 case the insert procedure should start after the nrmax-th largest
					 error estimate. */

				while (i_nrmax > 0 && errmax > elist[order[i_nrmax - 1]]) {
					order[i_nrmax] = order[i_nrmax - 1];
					i_nrmax--;
				}

				/* Compute the number of elements in the list to be maintained in
					 descending order. This number depends on the number of
					 subdivisions still allowed. */

				if (last < (limit/2 + 2)) {
					top = last;
				} else {
					top = limit - last + 1;
				}

				/* Insert errmax by traversing the list top-down, starting
					 comparison from the element elist(order(i_nrmax+1)). */

				i = i_nrmax + 1;

				/* The order of the tests in the following line is important to
					 prevent a segmentation fault */

				while (i < top && errmax < elist[order[i]]) {
					order[i-1] = order[i];
					i++;
				}

				order[i-1] = i_maxerr;

				/* Insert errmin by traversing the list bottom-up */

				errmin = elist[last];

				k = top - 1;

				while (k > i - 2 && errmin >= elist[order[k]]) {
					order[k+1] = order[k];
					k--;
				}

				order[k+1] = last;

				/* Set i_max and e_max */

				i_maxerr = order[i_nrmax];

				i = i_maxerr;
				nrmax = i_nrmax;
			}

		}

		// static Workspace workspace = new Workspace();

		static bool subinterval_too_small(double a1, double a2, double b2) { 
			double e = GSL_DBL_EPSILON;
			double u = GSL_DBL_MIN;

			double tmp = (1 + 100 * e) * (Math.Abs(a2) + 1000 * u);
			return  Math.Abs(a1) <= tmp && Math.Abs(b2) <= tmp;
		}



	
		/// <summary>
		/// Integration of the function f with adaptive stepsize. The integration region is divided into subintervals,
		///	and on each iteration the subinterval with the largest estimated error is bisected. This reduces the overall error
		///	rapidly, as the subintervals become concentrated around local difficulties in the integrand. The function allocates static
		/// memory for the used workspace accoring to the parameter memlimit.  
		/// This function applies an integration rule adaptively until an estimate of the integral of f over (a,b) is achieved within
		/// the desired absolute and relative error limits, epsabs and epsrel. The integration rule is determined by the value of smoothness,
		/// which should be chosen between 1 for smooth functions and 0 for functions that contain local difficulties, such as discontinuities.
		/// </summary>
		/// <param name="f">The function to integrate</param>
		/// <param name="a">The lower bound of the integration</param>
		/// <param name="b">The upper bound of the integration</param>
		/// <param name="epsabs">The desired absolute error</param>
		/// <param name="epsrel">The desired relative error</param>
		/// <param name="memlimit">The maximum memory consumption of the routine, in bytes</param>
		/// <param name="smoothness">The smoothness of the funtion. 1 indicates a smooth function and 0 indicates a function with local
		/// difficulties.</param>
		/// <param name="abserr">The absolute error of the integral</param>
		/// <returns></returns>
		public static double qa(IFunction1D f,
												 double a, double b,
												 double epsabs, double epsrel, int memlimit,
												 float smoothness,
												 out double abserr) {
			IntegrationRule integration_rule = r15;
			int limit = memlimit / (4*sizeof(double) + 2*sizeof(int));
			double result = double.NaN;
			Workspace workspace = new Workspace(limit);

			if (smoothness < 0) smoothness = 0;
			else if (smoothness >= 1) smoothness = 1;

			switch ((int)(smoothness*6)) {
			case 0:
				integration_rule = r15;
				break;
			case 1:
				integration_rule = r21;
				break;
			case 2:
				integration_rule = r31;
				break;
			case 3:
				integration_rule = r41;
				break;
			case 4:
				integration_rule = r51;
				break;
			case 5:
			case 6:
				integration_rule = r61;
				break;
			default: break;
			}

			qag_work(f, a, b, epsabs, epsrel, limit, out result, out abserr, integration_rule, workspace);
	
			return result;
		}

		static void qag_work(IFunction1D f,
				double a, double b,
				double epsabs, double epsrel,
				int limit, out double result, out double abserr,
				 IntegrationRule q, Workspace workspace) {
			double area, errsum;
			double result0, abserr0, resabs0, resasc0;
			double tolerance;
			int iteration = 0;
			int roundoff_type1 = 0, roundoff_type2 = 0, error_type = 0;

			double round_off;

			/* Initialize results */

			workspace.initialise(a, b);

			result = 0;
			abserr = 0;

			if (limit > workspace.limit) { throw new ArgumentOutOfRangeException("iteration limit exceeds available workspace"); }

			if (epsabs <= 0 && (epsrel < 50 * GSL_DBL_EPSILON || epsrel < 0.5e-28)) {
				throw new ArgumentOutOfRangeException("tolerance cannot be acheived with given epsabs and epsrel");
			}

			/* perform the first integration */

			q(f, a, b, out result0, out abserr0, out resabs0, out resasc0, workspace);

			workspace.set_initial_result(result0, abserr0);

			/* Test on accuracy */

			tolerance = Math.Max(epsabs, epsrel * Math.Abs(result0));

			/* need IEEE rounding here to match original quadpack behavior */

			round_off = (50 * GSL_DBL_EPSILON * resabs0);

			if (abserr0 <= round_off && abserr0 > tolerance) {
				result = result0;
				abserr = abserr0;
				if (ThrowOnErrors) throw new ArithmeticException("cannot reach tolerance because of roundoff error on first attempt");
				return;
			} else if ((abserr0 <= tolerance && abserr0 != resasc0) || abserr0 == 0.0) {
				result = result0;
				abserr = abserr0;
				return;
			} else if (limit == 1) {
				result = result0;
				abserr = abserr0;
				if (ThrowOnErrors) throw new ArithmeticException("a maximum of one iteration was insufficient");
				return;
			}

			area = result0;
			errsum = abserr0;

			iteration = 1;

			do {
				double a1, b1, a2, b2;
				double a_i, b_i, r_i, e_i;
				double area1 = 0, area2 = 0, area12 = 0;
				double error1 = 0, error2 = 0, error12 = 0;
				double resasc1, resasc2;
				double resabs1, resabs2;

				/* Bisect the subinterval with the largest error estimate */

				workspace.retrieve(out a_i, out b_i, out r_i, out e_i);

				a1 = a_i;
				b1 = 0.5 * (a_i + b_i);
				a2 = b1;
				b2 = b_i;

				q(f, a1, b1, out area1, out error1, out resabs1, out resasc1, workspace);
				q(f, a2, b2, out area2, out error2, out resabs2, out resasc2, workspace);

				area12 = area1 + area2;
				error12 = error1 + error2;

				errsum += (error12 - e_i);
				area += area12 - r_i;

				if (resasc1 != error1 && resasc2 != error2) {
					double delta = r_i - area12;

					if (Math.Abs(delta) <= 1.0e-5 * Math.Abs(area12) && error12 >= 0.99 * e_i) {
						roundoff_type1++;
					}
					if (iteration >= 10 && error12 > e_i) {
						roundoff_type2++;
					}
				}

				tolerance = Math.Max(epsabs, epsrel * Math.Abs(area));

				if (errsum > tolerance) {
					if (roundoff_type1 >= 6 || roundoff_type2 >= 20) {
						error_type = 2;   /* round off error */
					}

					/* set error flag in the case of bad integrand behaviour at
					 a point of the integration range */

					if (subinterval_too_small(a1, a2, b2)) {
						error_type = 3;
					}
				}

				workspace.update(a1, b1, area1, error1, a2, b2, area2, error2);

				workspace.retrieve(out a_i, out b_i, out r_i, out e_i);

				iteration++;

			} while (iteration < limit && error_type == 0 && errsum > tolerance);

			result = workspace.sum_results();
			abserr = errsum;

			if (errsum <= tolerance) {
				return;
			} else if (error_type == 2) {
				if (ThrowOnErrors) throw new ArithmeticException("roundoff error prevents tolerance from being achieved");
			} else if (error_type == 3) {
				if (ThrowOnErrors) throw new ArithmeticException("bad integrand behavior found in the integration interval");
			} else if (iteration == limit) {
				if (ThrowOnErrors) throw new ArithmeticException("maximum number of subdivisions reached, increase memlimit.");
			} else {
				if (ThrowOnErrors) throw new ArithmeticException("could not integrate function");
			}
		}

		/// <summary>
		/// Integration of the function f with adaptive stepsize. The integration region is divided into subintervals,
		///	and on each iteration the subinterval with the largest estimated error is bisected. This reduces the overall error
		///	rapidly, as the subintervals become concentrated around local difficulties in the integrand. The function allocates static
		/// memory for the used workspace accoring to the parameter memlimit.  
		/// This function applies an integration rule adaptively until an estimate of the integral of f over (a,b) is achieved within
		/// the desired absolute and relative error limits, epsabs and epsrel. The integration rule is determined by the value of smoothness,
		/// which should be chosen between 1 for smooth functions and 0 for functions that contain local difficulties, such as discontinuities.
		/// </summary>
		/// <param name="f">The function to integrate</param>
		/// <param name="a">The lower bound of the integration</param>
		/// <param name="b">The upper bound of the integration</param>
		/// <param name="epsabs">The desired absolute error</param>
		/// <param name="epsrel">The desired relative error</param>
		/// <param name="memlimit">The maximum memory consumption of the routine, in bytes</param>
		/// <param name="smoothness">The smoothness of the funtion. 1 indicates a smooth function and 0 indicates a function with local
		/// difficulties.</param>
		/// <param name="abserr">The absolute error of the integral</param>
		/// <returns></returns>
		public static double qa(Function1DDelegate f,
												 double a, double b,
												 double epsabs, double epsrel, int memlimit,
												 float smoothness,
												 out double abserr) {
			IntegrationRuleDelegate integration_rule = r15d;
			int limit = memlimit / (4*sizeof(double) + 2*sizeof(int));
			double result = double.NaN;
			Workspace workspace = new Workspace(limit);

			if (smoothness < 0) smoothness = 0;
			else if (smoothness >= 1) smoothness = 1;

			switch ((int)(smoothness*6)) {
			case 0:
				integration_rule = r15d;
				break;
			case 1:
				integration_rule = r21d;
				break;
			case 2:
				integration_rule = r31d;
				break;
			case 3:
				integration_rule = r41d;
				break;
			case 4:
				integration_rule = r51d;
				break;
			case 5:
			case 6:
				integration_rule = r61d;
				break;
			default: break;
			}

			qag_work(f, a, b, epsabs, epsrel, limit, out result, out abserr, integration_rule, workspace);

			return result;
		}

		static void qag_work(Function1DDelegate f,
				double a, double b,
				double epsabs, double epsrel,
				int limit, out double result, out double abserr,
				 IntegrationRuleDelegate q, Workspace workspace) {
			double area, errsum;
			double result0, abserr0, resabs0, resasc0;
			double tolerance;
			int iteration = 0;
			int roundoff_type1 = 0, roundoff_type2 = 0, error_type = 0;

			double round_off;

			/* Initialize results */

			workspace.initialise(a, b);

			result = 0;
			abserr = 0;

			if (limit > workspace.limit) { throw new ArgumentOutOfRangeException("iteration limit exceeds available workspace"); }

			if (epsabs <= 0 && (epsrel < 50 * GSL_DBL_EPSILON || epsrel < 0.5e-28)) {
				throw new ArgumentOutOfRangeException("tolerance cannot be acheived with given epsabs and epsrel");
			}

			/* perform the first integration */

			q(f, a, b, out result0, out abserr0, out resabs0, out resasc0, workspace);

			workspace.set_initial_result(result0, abserr0);

			/* Test on accuracy */

			tolerance = Math.Max(epsabs, epsrel * Math.Abs(result0));

			/* need IEEE rounding here to match original quadpack behavior */

			round_off = (50 * GSL_DBL_EPSILON * resabs0);

			if (abserr0 <= round_off && abserr0 > tolerance) {
				result = result0;
				abserr = abserr0;
				if (ThrowOnErrors) throw new ArithmeticException("cannot reach tolerance because of roundoff error on first attempt");
				return;
			} else if ((abserr0 <= tolerance && abserr0 != resasc0) || abserr0 == 0.0) {
				result = result0;
				abserr = abserr0;
				return;
			} else if (limit == 1) {
				result = result0;
				abserr = abserr0;
				if (ThrowOnErrors) throw new ArithmeticException("a maximum of one iteration was insufficient");
				return;
			}

			area = result0;
			errsum = abserr0;

			iteration = 1;

			do {
				double a1, b1, a2, b2;
				double a_i, b_i, r_i, e_i;
				double area1 = 0, area2 = 0, area12 = 0;
				double error1 = 0, error2 = 0, error12 = 0;
				double resasc1, resasc2;
				double resabs1, resabs2;

				/* Bisect the subinterval with the largest error estimate */

				workspace.retrieve(out a_i, out b_i, out r_i, out e_i);

				a1 = a_i;
				b1 = 0.5 * (a_i + b_i);
				a2 = b1;
				b2 = b_i;

				q(f, a1, b1, out area1, out error1, out resabs1, out resasc1, workspace);
				q(f, a2, b2, out area2, out error2, out resabs2, out resasc2, workspace);

				area12 = area1 + area2;
				error12 = error1 + error2;

				errsum += (error12 - e_i);
				area += area12 - r_i;

				if (resasc1 != error1 && resasc2 != error2) {
					double delta = r_i - area12;

					if (Math.Abs(delta) <= 1.0e-5 * Math.Abs(area12) && error12 >= 0.99 * e_i) {
						roundoff_type1++;
					}
					if (iteration >= 10 && error12 > e_i) {
						roundoff_type2++;
					}
				}

				tolerance = Math.Max(epsabs, epsrel * Math.Abs(area));

				if (errsum > tolerance) {
					if (roundoff_type1 >= 6 || roundoff_type2 >= 20) {
						error_type = 2;   /* round off error */
					}

					/* set error flag in the case of bad integrand behaviour at
					 a point of the integration range */

					if (subinterval_too_small(a1, a2, b2)) {
						error_type = 3;
					}
				}

				workspace.update(a1, b1, area1, error1, a2, b2, area2, error2);

				workspace.retrieve(out a_i, out b_i, out r_i, out e_i);

				iteration++;

			} while (iteration < limit && error_type == 0 && errsum > tolerance);

			result = workspace.sum_results();
			abserr = errsum;

			if (errsum <= tolerance) {
				return;
			} else if (error_type == 2) {
				if (ThrowOnErrors) throw new ArithmeticException("roundoff error prevents tolerance from being achieved");
			} else if (error_type == 3) {
				if (ThrowOnErrors) throw new ArithmeticException("bad integrand behavior found in the integration interval");
			} else if (iteration == limit) {
				if (ThrowOnErrors) throw new ArithmeticException("maximum number of subdivisions reached, increase memlimit.");
			} else {
				if (ThrowOnErrors) throw new ArithmeticException("could not integrate function");
			}
		}
	}
}