namespace JohnsHope.FPlot.Library {

	/// <summary>
	///  A class that can be used to implement your own 1D functions. Example:
	/// <code>
	///	class SineFunction: CustomFunction1D {
	///		public override double f(double x) {
	///			NEval++; // counts the number of function evaluations.
	///			return System.Math.Sin(x);
	///		}
	///	}
	///...
	/// // Add a SineFunction to a Plot:
	/// Plot.Model.Add(new SineFunction());
	/// </code>
	/// </summary>
	public class CustomFunction1D: Function1DItem {
		/// <summary>
		/// Always retruns null, so this item has no source that will be compiled.
		/// </summary>
		/// <returns></returns>
		public override string GetSource() { return null; }
		/// <summary>
		/// The default implementation of the function. Increments <see cref="FunctionItem.NEval"/> and returns <c>double.NaN</c>.
		/// </summary>
		public override double f(double x) {
			NEval++;
			return double.NaN;
		}
	}

	/// <summary>
	///  A class that can be used to implement your own 2D functions.
	/// <code>
	///	class CircleFunction: CustomFunction1D {
	///		public override double f(double x, double y) {
	///			NEval++; // counts the number of function evaluations.
	///			return System.Math.Sqrt(x*x + y*y);
	///		}
	///	}
	///...
	/// // Add a CircleFunction to a Plot:
	/// Plot.Model.Add(new CircleFunction());
	/// </code>
	/// </summary>
	public class CustomFunction2D: Function2DItem {
		/// <summary>
		/// Always retruns null, so this item has no source that will be compiled.
		/// </summary>
		/// <returns></returns>
		public override string GetSource() { return null; }
		/// <summary>
		/// The default implementation of the function. Increments <c>NEval</c> and returns <c>double.NaN</c>.
		/// </summary>
		public override double f(double x, double y) {
			NEval++;
			return double.NaN;
		}
	}

	/// <summary>
	///  A class that can be used to implement your own color functions.
	/// <code>
	///	class BlueFunction: CustomFunction1D {
	///		public override double f(double x, double y) {
	///			NEval++; // counts the number of function evaluations.
	///			return System.Drawing.Color.Blue;
	///		}
	///	}
	///...
	/// // Add a BlueFunction to a Plot:
	/// Plot.Model.Add(new BlueFunction());
	/// </code>
	/// </summary>
	public class CustomFunctionColor: FunctionColorItem {
		/// <summary>
		/// Always retruns null, so this item has no source that will be compiled.
		/// </summary>
		/// <returns></returns>
		public override string GetSource() { return null; }
		/// <summary>
		/// The default implementation of the function. Increments <see cref="FunctionItem.NEval"/> and returns <c>double.NaN</c>.
		/// </summary>
		public override System.Drawing.Color f(double x, double y) {
			NEval++;
			return System.Drawing.Color.Black;
		}
	}


}