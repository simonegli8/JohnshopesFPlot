using System;
using System.Collections.Generic;
using JohnsHope.Analysis;

namespace JohnsHope.FPlot.Library {

	public partial class DefaultClass {
		/// <summary>
		/// A one dimensional function.
		/// </summary>
		public interface IFunction1D: JohnsHope.Analysis.IFunction1D {}
		/// <summary>
		/// A two dimensional function.
		/// </summary>
		public interface IFunction2D: JohnsHope.Analysis.IFunction2D {}
		/// <summary>
		/// This variable represents a global static <see cref="Item"/> repository.
		/// </summary>
		public static ItemList Items;
		/// <summary>
		/// Returns a delegate to a 1D function according to its name from a <see cref="Items">global Item repository</see>.
		/// </summary>
		public static IFunction1D Function1D(string name) { return Items[name] as IFunction1D;  }
		/// <summary>
		/// Returns a delegate to a 2D function according to its name from a <see cref="Items">global Item repository</see>.
		/// </summary>
		public static IFunction2D Function2D(string name) { return Items[name] as IFunction2D; }

		#region IFunction implementations
		/// <summary>
		/// An alias for the routine <see cref="dfdx(JohnsHope.Analysis.IFunction1D, double, out double)"/>.
		/// </summary>
		public static double diff(JohnsHope.Analysis.IFunction1D f, double x, out double abserr) { return Diff.dfdx(f, x, out abserr); }
		/// <summary>
		/// Returns df/dx for a one dimensional function <c>f</c>. This routine is an alias for
		/// <see cref="JohnsHope.Analysis.Diff.dfdx(JohnsHope.Analysis.IFunction1D, double, out double)"/>.
		/// </summary>
		/// <param name="f">The function</param>
		/// <param name="x">The x coordinate</param>
		/// <param name="abserr">The absolute error of the derivative</param>
		/// <returns>The value of the derivative of f at x</returns>
		public static double dfdx(JohnsHope.Analysis.IFunction1D f, double x, out double abserr) { return Diff.dfdx(f, x, out abserr); }
		/// <summary>
		/// An alias for the routine <see cref="dfdp(JohnsHope.Analysis.IParametricFunction1D, double, int, out double)"/>.
		/// </summary>
		public static double diffp(JohnsHope.Analysis.IParametricFunction1D f, double x, int n, out double abserr) { return Diff.dfdp(f, x, n, out abserr); }
		/// <summary>
		/// Returns df/dp for a one dimensional parametric function <c>f</c>. This routine corresponds to
		/// <see cref="JohnsHope.Analysis.Diff.dfdp(JohnsHope.Analysis.IParametricFunction1D, double, int, out double)"/>.
		/// </summary>
		/// <param name="f">The function</param>
		/// <param name="x">The x coordinate</param>
		/// <param name="n">The index of the parameter to derive for</param>
		/// <param name="abserr">The absolute error of the derivative</param>
		/// <returns>The value of the derivative of f at x</returns>
		public static double dfdp(JohnsHope.Analysis.IParametricFunction1D f, double x, int n, out double abserr) { return Diff.dfdp(f, x, n, out abserr); }
		/// <summary>
		/// Returns df/dx for a two dimensional function <c>f</c>. This routine corresponds to
		/// <see cref="JohnsHope.Analysis.Diff.dfdx(JohnsHope.Analysis.IFunction2D, double, double, out double)"/>.
		/// </summary>
		/// <param name="f">The function</param>
		/// <param name="x">The x coordinate</param>
		/// <param name="y">The y coordinate</param>
		/// <param name="abserr">The absolute error of the derivative</param>
		/// <returns>The value of the derivative of df/dx at (x, y)</returns>
		public static double dfdx(JohnsHope.Analysis.IFunction2D f, double x, double y, out double abserr) { return Diff.dfdx(f, x, y, out abserr); }
		/// <summary>
		/// Returns df/dy for a two dimensional function <c>f</c>. This routine corresponds to
		/// <see cref="JohnsHope.Analysis.Diff.dfdy(JohnsHope.Analysis.IFunction2D, double, double, out double)"/>.
		/// </summary>
		/// <param name="f">The function</param>
		/// <param name="x">The x coordinate</param>
		/// <param name="y">The y coordinate</param>
		/// <param name="abserr">The absolute error of the derivative</param>
		/// <returns>The value of the derivative of df/dy at (x, y)</returns>
		public static double dfdy(JohnsHope.Analysis.IFunction2D f, double x, double y, out double abserr) { return Diff.dfdy(f, x, y, out abserr); }
		/// <summary>
		/// Returns df/dp for a one dimensional parametric function <c>f</c>. This routine corresponds to
		/// <see cref="JohnsHope.Analysis.Diff.dfdp(JohnsHope.Analysis.IParametricFunction2D, double, double, int, out double)"/>.
		/// </summary>
		/// <param name="f">The function</param>
		/// <param name="x">The x coordinate</param>
		/// <param name="y">The y coordinate</param>
		/// <param name="n">The index of the parameter to derive for</param>
		/// <param name="abserr">The absolute error of the derivative</param>
		/// <returns>The value of the derivative of <c>f</c> at <c>x</c></returns>
		public static double dfdp(JohnsHope.Analysis.IParametricFunction2D f, double x, double y, int n, out double abserr) { return Diff.dfdp(f, x, y, n, out abserr); }
		/// <summary>
		/// An alias for the routine <see cref="dfdp(JohnsHope.Analysis.IParametricFunction2D, double, double, int, out double)"/>.
		/// </summary>
		public static double diffp(JohnsHope.Analysis.IParametricFunction2D f, double x, double y, int n, out double abserr) { return Diff.dfdp(f, x, y, n, out abserr); }
		/// <summary>
		/// An alias for the routine <see cref="dfdx(JohnsHope.Analysis.IFunction2D, double, double, out double)"/>.
		/// </summary>
		public static double diffx(JohnsHope.Analysis.IFunction2D f, double x, double y, out double abserr) { return Diff.dfdx(f, x, y, out abserr); }
		/// <summary>
		/// An alias for the routine <see cref="dfdy(JohnsHope.Analysis.IFunction2D, double, double, out double)"/>.
		/// </summary>
		public static double diffy(JohnsHope.Analysis.IFunction2D f, double x, double y, out double abserr) { return Diff.dfdy(f, x, y, out abserr); }

		/// <summary>
		/// Returns the integral of a one dimensional function <c>f</c>. The integral is computed until the error is below either
		/// <c>epsabs</c> or <c>epsrel</c>. This routine corresponds to
		/// <see cref="JohnsHope.Analysis.Int.q(JohnsHope.Analysis.IFunction1D, double, double, double, double, out double)"/>.
		/// the Gnu Scientific Library.
		/// </summary>
		/// <param name="f">The function to integrate.</param>
		/// <param name="a">The lower bound of the integral</param>
		/// <param name="b">The upper bound of the integral</param>
		/// <param name="epsabs">The required absoulte error</param>
		/// <param name="epsrel">The required relative error</param>
		/// <param name="abserr">The absolute error of the computed integral</param>
		/// <returns>The computed value of the integral</returns>
		public static double intq(JohnsHope.Analysis.IFunction1D f, double a, double b, double epsabs, double epsrel,
												 out double abserr) {
			return Int.q(f, a, b, epsabs, epsrel, out abserr);
		}
		/// <summary>
		/// Retruns the integral of a one dimensional function <c>f</c>. The integral is computed until the error is below either
		/// <c>epsabs</c>
		/// or <c>epsrel</c>. This routine corresponds to
		/// <see cref="JohnsHope.Analysis.Int.qa(JohnsHope.Analysis.IFunction1D, double, double, double, double, int, float, out double)"/>.
		/// </summary>
		/// <param name="f">The function to integrate</param>
		/// <param name="a">The lower bound of the integral</param>
		/// <param name="b">The upper bound of the integral</param>
		/// <param name="epsabs">The required absolute error</param>
		/// <param name="epsrel">The required relative error</param>
		/// <param name="memlimit">The maximum amount of memory used, in bytes</param>
		/// <param name="smoothness">The smoothness of the funtion, 1 meaning a maximal smooth function and 0 meaning a function
		/// with maximun local difficulties</param>
		/// <param name="abserr">The absolute error of the computed integral</param>
		/// <returns>The computed value of the integral</returns>
		public static double intqa(JohnsHope.Analysis.IFunction1D f, double a, double b, double epsabs, double epsrel, int memlimit,
												 float smoothness, out double abserr) {
			return Int.qa(f, a, b, epsabs, epsrel, memlimit, smoothness, out abserr);
		}
		#endregion

		#region FunctionDelegate implementations
		/// <summary>
		/// An alias for the routine <see cref="dfdx(JohnsHope.Analysis.Function1DDelegate, double, out double)"/>.
		/// </summary>
		public static double diff(JohnsHope.Analysis.Function1DDelegate f, double x, out double abserr) { return Diff.dfdx(f, x, out abserr); }
		/// <summary>
		/// Returns df/dx for a one dimensional function <c>f</c>. This routine is an alias for
		/// <see cref="JohnsHope.Analysis.Diff.dfdx(JohnsHope.Analysis.Function1DDelegate, double, out double)"/>.
		/// </summary>
		/// <param name="f">The function</param>
		/// <param name="x">The x coordinate</param>
		/// <param name="abserr">The absolute error of the derivative</param>
		/// <returns>The value of the derivative of f at x</returns>
		public static double dfdx(JohnsHope.Analysis.Function1DDelegate f, double x, out double abserr) { return Diff.dfdx(f, x, out abserr); }
		/// <summary>
		/// An alias for the routine <see cref="dfdp(JohnsHope.Analysis.IParametricFunction1D, double, int, out double)"/>.
		/// </summary>
		public static double diffp(JohnsHope.Analysis.ParametricFunction1DDelegate f, double x, IList<double> p, int n, out double abserr) { return Diff.dfdp(f, x, p, n, out abserr); }
		/// <summary>
		/// Returns df/dp for a one dimensional parametric function <c>f</c>. This routine corresponds to
		/// <see cref="JohnsHope.Analysis.Diff.dfdp(JohnsHope.Analysis.IParametricFunction1D, double, int, out double)"/>.
		/// </summary>
		/// <param name="f">The function</param>
		/// <param name="x">The x coordinate</param>
		/// <param name="p">The parametrs of the function f</param>
		/// <param name="n">The index of the parameter to derive for</param>
		/// <param name="abserr">The absolute error of the derivative</param>
		/// <returns>The value of the derivative of f at x</returns>
		public static double dfdp(JohnsHope.Analysis.ParametricFunction1DDelegate f, double x, IList<double> p, int n, out double abserr) { return Diff.dfdp(f, x, p, n, out abserr); }
		/// <summary>
		/// Returns df/dx for a two dimensional function <c>f</c>. This routine corresponds to
		/// <see cref="JohnsHope.Analysis.Diff.dfdx(JohnsHope.Analysis.IFunction2D, double, double, out double)"/>.
		/// </summary>
		/// <param name="f">The function</param>
		/// <param name="x">The x coordinate</param>
		/// <param name="y">The y coordinate</param>
		/// <param name="abserr">The absolute error of the derivative</param>
		/// <returns>The value of the derivative of df/dx at (x, y)</returns>
		public static double dfdx(JohnsHope.Analysis.Function2DDelegate f, double x, double y, out double abserr) { return Diff.dfdx(f, x, y, out abserr); }
		/// <summary>
		/// Returns df/dy for a two dimensional function <c>f</c>. This routine corresponds to
		/// <see cref="JohnsHope.Analysis.Diff.dfdy(JohnsHope.Analysis.Function2DDelegate, double, double, out double)"/>.
		/// </summary>
		/// <param name="f">The function</param>
		/// <param name="x">The x coordinate</param>
		/// <param name="y">The y coordinate</param>
		/// <param name="abserr">The absolute error of the derivative</param>
		/// <returns>The value of the derivative of df/dy at (x, y)</returns>
		public static double dfdy(JohnsHope.Analysis.Function2DDelegate f, double x, double y, out double abserr) { return Diff.dfdy(f, x, y, out abserr); }
		/// <summary>
		/// Returns df/dp for a one dimensional parametric function <c>f</c>. This routine corresponds to
		/// <see cref="JohnsHope.Analysis.Diff.dfdp(JohnsHope.Analysis.IParametricFunction2D, double, double, int, out double)"/>.
		/// </summary>
		/// <param name="f">The function</param>
		/// <param name="x">The x coordinate</param>
		/// <param name="y">The y coordinate</param>
		/// <param name="p">The parametrs of the function f</param>
		/// <param name="n">The index of the parameter to derive for</param>
		/// <param name="abserr">The absolute error of the derivative</param>
		/// <returns>The value of the derivative of <c>f</c> at <c>x</c></returns>
		public static double dfdp(JohnsHope.Analysis.ParametricFunction2DDelegate f, double x, double y, IList<double> p, int n, out double abserr) { return Diff.dfdp(f, x, y, p, n, out abserr); }
		/// <summary>
		/// An alias for the routine <see cref="dfdp(JohnsHope.Analysis.IParametricFunction2D, double, double, int, out double)"/>.
		/// </summary>
		public static double diffp(JohnsHope.Analysis.ParametricFunction2DDelegate f, double x, double y, IList<double> p, int n, out double abserr) { return Diff.dfdp(f, x, y, p, n, out abserr); }
		/// <summary>
		/// An alias for the routine <see cref="dfdx(JohnsHope.Analysis.IFunction2D, double, double, out double)"/>.
		/// </summary>
		public static double diffx(JohnsHope.Analysis.Function2DDelegate f, double x, double y, out double abserr) { return Diff.dfdx(f, x, y, out abserr); }
		/// <summary>
		/// An alias for the routine <see cref="dfdy(JohnsHope.Analysis.IFunction2D, double, double, out double)"/>.
		/// </summary>
		public static double diffy(JohnsHope.Analysis.Function2DDelegate f, double x, double y, out double abserr) { return Diff.dfdy(f, x, y, out abserr); }

		/// <summary>
		/// Returns the integral of a one dimensional function <c>f</c>. The integral is computed until the error is below either
		/// <c>epsabs</c> or <c>epsrel</c>. This routine corresponds to
		/// <see cref="JohnsHope.Analysis.Int.q(JohnsHope.Analysis.Function1DDelegate, double, double, double, double, out double)"/>.
		/// the Gnu Scientific Library.
		/// </summary>
		/// <param name="f">The function to integrate.</param>
		/// <param name="a">The lower bound of the integral</param>
		/// <param name="b">The upper bound of the integral</param>
		/// <param name="epsabs">The required absoulte error</param>
		/// <param name="epsrel">The required relative error</param>
		/// <param name="abserr">The absolute error of the computed integral</param>
		/// <returns>The computed value of the integral</returns>
		public static double intq(JohnsHope.Analysis.Function1DDelegate f, double a, double b, double epsabs, double epsrel,
												 out double abserr) {
			return Int.q(f, a, b, epsabs, epsrel, out abserr);
		}
		/// <summary>
		/// Retruns the integral of a one dimensional function <c>f</c>. The integral is computed until the error is below either
		/// <c>epsabs</c>
		/// or <c>epsrel</c>. This routine corresponds to
		/// <see cref="JohnsHope.Analysis.Int.qa(JohnsHope.Analysis.Function1DDelegate, double, double, double, double, int, float, out double)"/>.
		/// </summary>
		/// <param name="f">The function to integrate</param>
		/// <param name="a">The lower bound of the integral</param>
		/// <param name="b">The upper bound of the integral</param>
		/// <param name="epsabs">The required absolute error</param>
		/// <param name="epsrel">The required relative error</param>
		/// <param name="memlimit">The maximum amount of memory used, in bytes</param>
		/// <param name="smoothness">The smoothness of the funtion, 1 meaning a maximal smooth function and 0 meaning a function
		/// with maximun local difficulties</param>
		/// <param name="abserr">The absolute error of the computed integral</param>
		/// <returns>The computed value of the integral</returns>
		public static double intqa(JohnsHope.Analysis.Function1DDelegate f, double a, double b, double epsabs, double epsrel, int memlimit,
												 float smoothness, out double abserr) {
			return Int.qa(f, a, b, epsabs, epsrel, memlimit, smoothness, out abserr);
		}
		#endregion
	}
}