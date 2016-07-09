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

		static double[] xgk_15 = new double[8]   /* abscissae of the 15-point kronrod rule */
		{
			0.991455371120812639206854697526329,
			0.949107912342758524526189684047851,
			0.864864423359769072789712788640926,
			0.741531185599394439863864773280788,
			0.586087235467691130294144838258730,
			0.405845151377397166906606412076961,
			0.207784955007898467600689403773245,
			0.000000000000000000000000000000000
		};

		/* xgk[1], xgk[3], ... abscissae of the 7-point gauss rule. 
			 xgk[0], xgk[2], ... abscissae to optimally extend the 7-point gauss rule */

		static double[] wg_15 = new double[4]    /* weights of the 7-point gauss rule */
		{
			0.129484966168869693270611432679082,
			0.279705391489276667901467771423780,
			0.381830050505118944950369775488975,
			0.417959183673469387755102040816327
		};

		static double[] wgk_15 = new double[8]  /* weights of the 15-point kronrod rule */
		{
			0.022935322010529224963732008058970,
			0.063092092629978553290700663189204,
			0.104790010322250183839876322541518,
			0.140653259715525918745189590510238,
			0.169004726639267902826583426598550,
			0.190350578064785409913256402421014,
			0.204432940075298892414161999234649,
			0.209482141084727828012999174891714
		};

		static double[] fv1_15 = new double[8], fv2_15 = new double[8];

		static void gsl_integration_qk15(IFunction1D f, double a, double b,
					out double result, out double abserr,
					out double resabs, out double resasc, Workspace workspace) {
			lock (fv1_15) {
				gsl_integration_qk(8, xgk_15, wg_15, wgk_15, fv1_15, fv2_15, f, a, b, out result, out abserr, out resabs, out resasc, workspace);
			}
		}

		static void gsl_integration_qk15(Function1DDelegate f, double a, double b,
			out double result, out double abserr,
			out double resabs, out double resasc, Workspace workspace) {
			lock (fv1_15) {
				gsl_integration_qk(8, xgk_15, wg_15, wgk_15, fv1_15, fv2_15, f, a, b, out result, out abserr, out resabs, out resasc, workspace);
			}
		}
	}
}