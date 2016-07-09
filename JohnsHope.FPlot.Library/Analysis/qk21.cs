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

	public partial class Int {

		static double[] xgk_21 = new double[11]  /* abscissae of the 21-point kronrod rule */
		{
			0.995657163025808080735527280689003,
			0.973906528517171720077964012084452,
			0.930157491355708226001207180059508,
			0.865063366688984510732096688423493,
			0.780817726586416897063717578345042,
			0.679409568299024406234327365114874,
			0.562757134668604683339000099272694,
			0.433395394129247190799265943165784,
			0.294392862701460198131126603103866,
			0.148874338981631210884826001129720,
			0.000000000000000000000000000000000
		};

		/* xgk[1], xgk[3], ... abscissae of the 10-point gauss rule. 
			 xgk[0], xgk[2], ... abscissae to optimally extend the 10-point gauss rule */

		static double[] wg_21 = new double[5]     /* weights of the 10-point gauss rule */
		{
			0.066671344308688137593568809893332,
			0.149451349150580593145776339657697,
			0.219086362515982043995534934228163,
			0.269266719309996355091226921569469,
			0.295524224714752870173892994651338
		};

		static double[] wgk_21 = new double[11]  /* weights of the 21-point kronrod rule */
		{
			0.011694638867371874278064396062192,
			0.032558162307964727478818972459390,
			0.054755896574351996031381300244580,
			0.075039674810919952767043140916190,
			0.093125454583697605535065465083366,
			0.109387158802297641899210590325805,
			0.123491976262065851077958109831074,
			0.134709217311473325928054001771707,
			0.142775938577060080797094273138717,
			0.147739104901338491374841515972068,
			0.149445554002916905664936468389821
		};

		static double[] fv1_21 = new double[11], fv2_21 = new double[11];

		static void gsl_integration_qk21(IFunction1D f, double a, double b,
													out double result, out double abserr,
													out double resabs, out double resasc, Workspace workspace) {
			lock (fv1_21) {
				gsl_integration_qk(11, xgk_21, wg_21, wgk_21, fv1_21, fv2_21, f, a, b, out result, out abserr, out resabs, out resasc, workspace);
			}
		}

		static void gsl_integration_qk21(Function1DDelegate f, double a, double b,
												out double result, out double abserr,
												out double resabs, out double resasc, Workspace workspace) {
			lock (fv1_21) {
				gsl_integration_qk(11, xgk_21, wg_21, wgk_21, fv1_21, fv2_21, f, a, b, out result, out abserr, out resabs, out resasc, workspace);
			}
		}
	}
}