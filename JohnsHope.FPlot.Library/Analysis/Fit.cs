using System;
using System.Threading;
using System.Windows.Forms;
using JohnsHope.FPlot.Library;
using System.Diagnostics;
using System.Collections.Generic;

namespace JohnsHope.FPlot.Library {

	/// <summary>
	/// A class that encapsulates fit algorithms for fitting <see cref="Function1DItem"/>s to <see cref="DataItem"/>s.
	/// </summary>
	public class Fit: IAsyncResult {
		/// <summary>
		/// Describes different Fit algorihtms
		/// </summary>
		public enum FitAlgorithm {
			/// <summary>
			/// Describes a Marquardt fit algorithm.
			/// </summary>
			Marquardt,
			/// <summary>
			/// Describes a fit algorithm after Nelder &amp; Mead.
			/// </summary>
			NelderMead
		}

		private enum RunMode { Synchronous, ThreadPool, Thread }

		/// <summary>
		/// An event handler that is called after each step of the fit algorithm.
		/// </summary>
		/// <param name="fit">A reference to the corresponding <see cref="Fit"/> object.</param>
		public delegate void StepEventHandler(Fit fit);
		/// <summary>
		/// The step event, that is fired after each step of the fit algorithm. Note that if you started fitting through
		/// the routine <see cref="BeginSolve"/> the Step handler is called by that <see cref="System.Threading.Thread">Thread</see>.
		/// </summary>
		public event StepEventHandler Step;


		class UndoElement {
			SmallData p;
			float[,] covar;
			float chisq;
			bool[] fitp;
			int neval;

			public UndoElement(Fit fit) {
				bool deepCopy = fit.f0.p.deepCopy; fit.f0.p.deepCopy = true;
				p = fit.f0.p.Clone();
				fit.f0.p.deepCopy = deepCopy;
				if (fit.fitp != null) fitp = (bool[])fit.fitp.Clone();
				else fitp = null;

				if (fit.covar != null) covar = (float[,])fit.covar.Clone();
				else covar = null;

				chisq = fit.chisq;
				neval = fit.neval;
			}

			public void Restore(Fit fit) {
				Function1DItem f = fit.f0;
				if (f.p.Length != p.Length) throw new Exception("undo parameter length and current parameter length differ.");
				f.p = p;
				if (covar != null) fit.covar = covar;
				else fit.covar = null;
				fit.chisq = chisq;
				if (fitp != null) fit.fitp = fitp;
				else fit.fitp = null;
				fit.neval = neval;
			}

			public bool CanRestore(Fit fit) {
				return fit.f0.p.Length == p.Length;
			}
		}

		private bool[] fitp;
		private float chisq = float.NaN;
		private float[,] covar = null;
		private Function1DItem f, f0;
		private DataItem data;
		private int t0 = 0;
		private int neval = 0;
		private bool fitting;
		private Stack<UndoElement> undo, redo;
		private FitAlgorithm algorithm;
		private Exception ex;
		private AsyncCallback callback;
		private Thread thread; //TODO check if still running thread can be claimed by the GC
		private bool enableUndo;
		private bool useTreadPool;
		private RunMode runMode;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public Fit() {
			algorithm = FitAlgorithm.Marquardt;
			undo = new Stack<UndoElement>();
			redo = new Stack<UndoElement>();
			fitting = false;
			enableUndo = true;
			ex = null;
			useTreadPool = true;
		}

		/// <summary>
		/// Constructor that initializes Fit with the given <see cref="DataItem"/> and fit function.
		/// The fit is initialized to the <see cref="FitAlgorithm">Marquardt algorithm</see>.
		/// </summary>
		/// <param name="data">The <see cref="DataItem"/> that contains the fit data.</param>
		/// <param name="f">The function to fit.</param>
		public Fit(DataItem data, Function1DItem f): this() {
			Data = data;
			Function = f;
		}
		/// <summary>
		/// Constructor that initializes Fit with the given <see cref="DataItem"/> and fit function and a boolean array of parameters
		/// to fit for. The fit is initialized to the <see cref="FitAlgorithm">Marquardt algorithm</see>.
		/// </summary>
		/// <param name="data">The <see cref="DataItem"/> that contains the fit data.</param>
		/// <param name="f">The function to fit.</param>
		/// <param name="fitp">A boolean array denoting the parameters to fit for.</param>
		public Fit(DataItem data, Function1DItem f, bool[] fitp): this(data, f) {
			this.fitp = (bool[])fitp.Clone();
		}

		/// <summary>
		/// Constructor that initializes Fit with the given <see cref="DataItem"/> and fit function and a fitting algorithm.
		/// </summary>
		/// <param name="data">The <see cref="DataItem"/> that contains the fit data.</param>
		/// <param name="f">The function to fit.</param>
		/// <param name="algorithm">The fit algorithm to use.</param>
		public Fit(DataItem data, Function1DItem f, FitAlgorithm algorithm)
			: this(data, f) {
			this.algorithm = algorithm;
		}
		/// <summary>
		/// Constructor that initializes Fit with the given <see cref="DataItem"/> and fit function, a boolean array of parameters
		/// to fit for and a fitting algorithm.
		/// </summary>
		/// <param name="data">The <see cref="DataItem"/> that contains the fit data.</param>
		/// <param name="f">The function to fit.</param>
		/// <param name="fitp">A boolean array denoting the parameters to fit for.</param>
		/// <param name="algorithm">The fit algorithm to use.</param>
		public Fit(DataItem data, Function1DItem f, bool[] fitp, FitAlgorithm algorithm)
			: this(data, f, fitp) {
			this.algorithm = algorithm;
		}

		private int stepInterval = 250;

		/// <summary>
		/// A constant that can be assigned to <see cref="StepInterval" /> so the Step event will never be fired, and the algorithm will
		/// not be interrupted.
		/// </summary>
		public const int Never = -1;

		/// <summary>
		/// Denotes the minimal time delay between to invocations of the Step event in milliseconds. The default value is 250.
		/// If you assign Never, the Step event is never raised.
		/// </summary>
		public int StepInterval {
			get { return stepInterval; }
			set { stepInterval = value; }
		}
		/// <summary>
		/// This routine fires the <see cref="Step"/> event, at a minimal time interval of <see cref="StepInterval"/> during fitting.
		/// </summary>
		public virtual void OnStep() {
			if (stepInterval != Never) {
				try {
					int now = System.Environment.TickCount;
					if (Step != null && (!fitting || now - t0 >= stepInterval)) {
						Step(this);
						t0 = now;
					}
				} catch (Exception ex) {
					this.ex = ex;
				}
			}
		}
		/// <summary>
		/// The ChiSquare of the fit. This property is read-only.
		/// </summary>
		public float ChiSquare {
			get { return chisq; }
		}
		/// <summary>
		/// The goodness-of-fit value Q = gammq(0.5*(N-M),0.5*chisquare), that indicates the probability of the fit.
		/// This value is read-only.
		/// </summary>
		public float Q {
			get { return gammq(0.5F*(data.Length-f.p.Length), 0.5F*ChiSquare); }
		}
		/// <summary>
		/// This property contains the covariance matrix of the fit. This value is read-only. This property is only valid if the fit
		/// terminated regularly without exception, or if not, <see cref="EvalCovarianceMatrix"/> has been called.
		/// </summary>
		public float[,] CovarianceMatrix {
			get {
				if (fitting) throw new InvalidOperationException(Properties.Resources.CovarianceMatrixException);
				return covar;
			}
		}
		/// <summary>
		/// Calculates the covariance matrix if the fit did not terminate regularly.
		/// </summary>
		/// <returns>Returns the covariance matrix or null if there was an exception</returns>
		public float[,] EvalCovarianceMatrix() {
			lock (this) {
				if (fitting) throw new InvalidOperationException(Properties.Resources.CovarianceMatrixException);
				fitting = true;
				signal.Reset();
			}
			if (covar == null && neval > 0 && f0 != null && data != null) { // calculate covariance matrix
				try {
					covar = new float[0, 0];
					float[,] alpha = new float[0, 0];
					float alamda;
					chisq = 0;
					if (!f0.HasDerivatives) {
						f = new DerivFunction(f0);
						if (f is DerivFunction) ((DerivFunction)f).fast = false;
					} else f = f0;

					alamda = -1;
					mrqmin(fitp, ref covar, ref alpha, ref chisq, ref alamda);
					alamda = 0;
					mrqmin(fitp, ref covar, ref alpha, ref chisq, ref alamda);
				} catch (Exception ex) {
					this.ex = ex;
					covar = null;
				}
			}
			lock (this) {
				fitting = false;
				signal.Set();
			}
			return covar;
		}

		/// <summary>
		/// Sets or gets the function to fit.
		/// </summary>
		public Function1DItem Function {
			get {return f0;}
			set {
				if (f0 != value) {
					f0 = value;
					if (f0 != null) {
						fitp = new bool[f0.p.Length];
						for (int i = 0; i < f0.p.Length; i++) {
							fitp[i] = true;
						}
					}
					neval = 0;
					chisq = 0;
					covar = null;
					undo.Clear();
					redo.Clear();
				}
			}
		}
		/// <summary>
		/// Sets the function parameters to fit for of the fit-function.
		/// </summary>
		public bool[] Fitp {
			get {return fitp;}
			set {
				if (fitting) throw new System.InvalidOperationException(Properties.Resources.FitpException);
				fitp = (bool[])value.Clone();
			}
		}

		/// <summary>
		/// Returns the errors of the fit parameters, if the fit terminated regularly or if <see cref="EvalCovarianceMatrix"/> has
		/// been called, otherwise returns a NaN array.
		/// </summary>
		public float[] Errors {
			get {
				float[] err = new float[f0.p.Length];
				if (covar != null) {
					for (int i = 0, j = 0; i < f0.p.Length; i++) {
						if (fitp[i]) err[i] = covar[j, j]*covar[j, j++];
						else err[i] = 0;
					}
				} else {
					for (int i = 0; i < f0.p.Length; i++) err[i] = float.NaN;
				}
				return err;
			}
		}
		/// <summary>
		/// Sets or gets the <see cref="DataItem"/> with the fit-data.
		/// </summary>
		public DataItem Data {
			get {return data;}
			set {
				if (data != value) {
					data = value;
					neval = 0;
					chisq = 0;
					covar = null;
					undo.Clear();
					redo.Clear();
				}
			}
		}
		/// <summary>
		/// Gets the number of function evaluations.
		/// </summary>
		public int NEval { get { return neval; } }

		/// <summary>
		/// Determines the <see cref="FitAlgorithm">algorithm used for fitting</see>, either Marquartd or NelderMead.
		/// </summary>
		public FitAlgorithm Algorithm {
			get { return algorithm; }
			set { algorithm = value; }
		}
		/// <summary>
		/// The <see cref="System.Exception">Exception</see> that occured during fitting, or null if there was no exception.
		/// </summary>
		public Exception Exception { get { return ex; } }

		/// <summary>
		/// Is true if there were errors during the fit.
		/// </summary>
		public bool Error { get { return ex != null; } }


		private void Solve(RunMode mode, AsyncCallback callback, object state) {
			lock (this) {
				if (fitting == true) throw new InvalidOperationException(Properties.Resources.AlreadyFittingError);
				fitting = true;
				this.runMode = mode;
				signal.Reset();
			}
			// set callback
			this.callback = callback;
			this.asyncState = state;

			// save state to undo stack
			redo.Clear();
			if (enableUndo) undo.Push(new UndoElement(this));

			try {
				switch (algorithm) {
				case FitAlgorithm.Marquardt: MarquardtSolve(); break;
				case FitAlgorithm.NelderMead: NelderMeadSolve(true); break;
				}
			} catch (Exception ex) {
				this.ex = ex;
			}
		}

		/// <summary>
		/// Finds the solution of the fitting problem. This method runs on the current thread and blocks the thread until the
		/// solution is found or an error occurs. In contrast, the method <see cref="BeginSolve"/> runs on a separate thread and does
		/// not block.
		/// </summary>
		/// <exception cref="Exception">May throw exceptions if the fitting algorithm encounters singularities etc.</exception>
		public void Solve() { Solve(RunMode.Synchronous, null, null); }

		/// <summary>
		/// Starts fitting on a separate <see cref="System.Threading.Thread">Thread</see>. You can wait for the fitting threads termination by calling
		/// <see cref="EndSolve"/>.
		/// In contrast, the mehtod <see cref="Solve()"/> runs on the current thread and blocks the current thread until the solution is
		/// found or an error occurs.
		/// </summary>
		/// <param name="callback">A method that is called when the fitting is finished.</param>
		/// <param name="state"></param>
		public IAsyncResult BeginSolve(AsyncCallback callback, object state) {
			RunMode mode;
			if (useTreadPool) mode = RunMode.ThreadPool;
			else mode = RunMode.Thread;
			Solve(mode, callback, state);
			return this;
		}

		/// <summary>
		/// Blocks until fitting is finished.
		/// </summary>
		public void EndSolve(IAsyncResult result) {
			if (!(result is Fit)) throw new ApplicationException(Properties.Resources.EndSolveException); 
			result.AsyncWaitHandle.WaitOne();
		}

		/// <summary>
		/// Returns the <see cref="System.Threading.Thread">Thread</see> the fitting runs on, when fitting was started with <see cref="BeginSolve"/>, or null when
		/// fitting was started synchronously with <see cref="Solve()"/>, or if fitting runs in the <see cref="ThreadPool"/> and not on
		/// its own <see cref="System.Threading.Thread">Thread</see>, or if fitting has finished.
		/// </summary>
		public Thread Thread { get { return thread; } }

		/// <summary>
		/// If set to true, the asynchronous fitting algorithm will use a <see cref="ThreadPool"/> thread instead of its own thread.
		/// This is faster, but you cannot access the thread via the <see cref="Thread"/> property and you cannot abort the threat via the method Abort.
		/// The default setting of this property is true.
		/// </summary>
		public bool UseThreadPool {
			get { return useTreadPool; }
			set { useTreadPool = value; }
		}

		// Finishes the execution of the fitting thread.
		private void End() {
			bool end = false;
			lock (this) {
				if (fitting) {
					fitting = false;
					end = true;
					thread = null;
					signal.Set();
				}
			}
			if (end) {
				try {
					OnStep();
					if (callback != null) callback(this);
				} catch (Exception ex) {
					this.ex = ex;
				}
			}
		}

		/// <summary>
		/// Aborts the execution of the fitting thread.
		/// </summary>
		/// <returns>True if the thread was successfully aborted, false if there was no fitting thread running or if fitting
		/// could not be aborted.</returns>
		public bool Abort() {
			bool abort = false;
			lock (this) {
				if (fitting && thread != null) {
					thread.Abort();
					fitting = false;
					thread = null;
					abort = true;
					covar = null;
					signal.Set();
				}
			}
			if (abort) {
				try {
					OnStep();
					if (callback != null) callback(this);
				} catch (Exception ex) {
					this.ex = ex;
				}
			}
			return abort;
		}

		/// <summary>
		/// If this property is set to false, no undo information is stored. It is set to true by default.
		/// You need to turn off undo only if you want to save memory or want to speed up fitting initialization by a small amount.
		/// (This is only necessary if you fit thousands of parameters, because the covariance matrix's size if of the order of the
		/// square of the number of fit parameters.)
		/// </summary>
		public bool EnableUndo {
			get { return enableUndo; }
			set {
				enableUndo = value;
				if (!enableUndo) {
					undo.Clear();
					redo.Clear();
				}
			}
		}

		/// <summary>
		/// Restores the Fit's state one <see cref="Solve()"/> call back.
		/// </summary>
		/// <returns>Returns true if the undo stack was not empty.</returns>
		/// <exception cref="Exception">Throws an Exception if the number of parameters in the function has changed.</exception>
		public bool Undo() {
			if (CanUndo) {
				redo.Push(new UndoElement(this));
				undo.Pop().Restore(this);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Redo the action that was previously reverted by <see cref="Undo"/>.
		/// </summary>
		/// <returns>Returns true if the redo stack was not empty.</returns>
		/// <exception cref="Exception">Throws an Exception if the number of parameters in the function has changed.</exception>
		public bool Redo() {
			if (CanRedo) {
				undo.Push(new UndoElement(this));
				redo.Pop().Restore(this);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Returns true if there is a step that can be undone.
		/// </summary> 
		public bool CanUndo {
			get {
				if (fitting || undo.Count == 0) return false;
				if (!undo.Peek().CanRestore(this)) {
					undo.Clear();
					return false;
				}
				return true;
			}
		}

		/// <summary>
		/// Return true if there is a step that can be redone.
		/// </summary>
		public bool CanRedo {
			get {
				if (fitting || redo.Count == 0) return false;
				if (!redo.Peek().CanRestore(this)) {
					redo.Clear();
					return false;
				}
				return true;
			}
		}

		private void Run(WaitCallback method) {
			try {
				switch (runMode) {
				case RunMode.Synchronous: method(null); break;
				case RunMode.Thread:
					thread = new Thread(new ParameterizedThreadStart(method));
					thread.Priority = ThreadPriority.AboveNormal;
					thread.Name = "JohnsHope's FPlot Fit Thread";
					thread.Start();
					break;
				case RunMode.ThreadPool:
					ThreadPool.QueueUserWorkItem(method);
					break;
				}
			} catch (Exception ex) {
				this.ex = ex;
			}
		}

		#region Gamma function
		static float gammln(float xx) {
			double x, y, tmp, ser;
			double[] cof = new double[6]{76.18009172947146, -86.50532032941677, 24.01409824083091,
																		-1.231739572450155, 0.1208650973866179e-2, -0.5395239384953e-5};
			int j;

			y = x = xx;
			tmp = x + 5.5F;
			tmp -= (x + 0.5F)*Math.Log(tmp);
			ser = 1.000000000190015;
			for (j = 0; j <= 5; j++) ser += cof[j]/++y;
			return (float)(-tmp + Math.Log(2.5066282746310005*ser/x));
		}

		static void gser(out float gamser, float a, float x, out float gln) {
			const int ITMAX = 100;
			const float EPS = 3.0e7F;
			int n;
			float sum, del, ap;

			gln = gammln(a);
			if (x <= 0) {
				if (x < 0) throw new System.ArgumentException(Properties.Resources.gserException0);
				gamser = 0;
				return;
			} else {
				ap = a;
				del = sum = 1/a;
				for (n = 1; n <= ITMAX; n++) {
					++ap;
					del *= x/ap;
					sum += del;
					if (Math.Abs(del) < Math.Abs(sum)*EPS) {
						gamser = sum*(float)Math.Exp(-x + a*Math.Log(x) - gln);
						return;
					}
				}
				throw new System.ArgumentException(Properties.Resources.gserException1);
			}
		}

		static void gcf(out float gammcf, float a, float x, out float gln) {
			const int ITMAX = 100;
			const float EPS = 3.0e-7F, FPMIN = 1.0e-30F;

			int i;
			float an, b, c, d, del, h;

			gln = gammln(a);
			b = x + 1 - a;
			c = 1/FPMIN;
			d = 1/b;
			h = d;
			for (i = 1; i <= ITMAX; i++) {
				an = -i*(i - a);
				b += 2;
				d = an*d + b;
				if (Math.Abs(d) < FPMIN) d = FPMIN;
				c = b + an/c;
				if (Math.Abs(c) < FPMIN) c = FPMIN;
				d = 1/d;
				del = d*c;
				h *= del;
				if (Math.Abs(del - 1) < EPS) break;
			}
			if (i > ITMAX) throw new System.ArgumentException(Properties.Resources.gcfException);
			gammcf = (float)(Math.Exp(-x + a*Math.Log(x) - gln)*h);
		}

		static float gammq(float a, float x) {
			float gamser, gammcf, gln;

			if (x < 0 || a <= 0) throw new System.ArgumentException(Properties.Resources.gammqException);
			if (x < (a+1)) {
				gser(out gamser, a, x, out gln);
				return 1-gamser;
			} else {
				gcf(out gammcf, a, x, out gln);
				return gammcf;
			}
		}

		#endregion

		#region Numerical Derivatives
		/// <summary>
		/// Generates a <see cref="Function1DItem"/> object that automatically calculates the derivative information (the array dfdp)
		/// of the supplied function numerically.
		/// </summary>
		public class DerivFunction: Function1DItem {
			Function1DItem function;
			/// <summary>
			/// If fast is set to false, a more accurate but slower derivative computation is done.
			/// </summary>
			public bool fast;

			void GetH(double x, ref double h, double temp) {
				h = temp - x;
			}

			void FastDeriv(double x) {
				const double EPS = 1e-10;
				int i;
				double h, p0, temp, f0, f1;

				for (i = 0; i < p.Length; i++) {
					p0 = p[i];
					h = p0 * EPS;
					temp = p0 + h;
					GetH(p0, ref h, temp);
					p[i] = p0 + h;
					f0 = function.f(x);
					p[i] = p0 - h;
					f1 = function.f(x);
					p[i] = p0;
					dfdp[i] = (f0 - f1)/(2*h);
				}
			}

			double[] a = new double[4], d = new double[4];

			void SlowDeriv(double x) {
				// adapted from gsl_diff_central
				const double GSL_SQRT_DBL_EPSILON = 1.4901161193847656e-08;

				/* Construct a divided difference table with a fairly large step
					size to get a very rough estimate of f'''.  Use this to estimate
					the step size which will minimize the error in calculating f'. */

			  int i, k, j;
				double h = GSL_SQRT_DBL_EPSILON, a3, f0, f1, p0, temp;

				for (j = 0; j < p.Length; j++) {

					/* Algorithm based on description on pg. 204 of Conte and de Boor
						(CdB) - coefficients of Newton form of polynomial of degree 3. */
				  
					for (i = 0; i < 4; i++)	{
						a[i] = x + (i - 2) * h;
						p0 = p[j];
						p[j] = a[i];
						d[i] = function.f(x);
						p[j] = p0;
					}

					for (k = 1; k < 5; k++)	{
						for (i = 0; i < 4 - k; i++)	{
							d[i] = (d[i + 1] - d[i]) / (a[i + k] - a[i]);
						}
					}

					/* Adapt procedure described on pg. 282 of CdB to find best
						value of step size. */

					a3 = Math.Abs(d[0] + d[1] + d[2] + d[3]);

					if (a3 < 100 * GSL_SQRT_DBL_EPSILON) {
						a3 = 100 * GSL_SQRT_DBL_EPSILON;
					}

					h = Math.Pow(GSL_SQRT_DBL_EPSILON / (2 * a3), 1.0 / 3.0);

					if (h > 100 * GSL_SQRT_DBL_EPSILON) {
						h = 100 * GSL_SQRT_DBL_EPSILON;
					}

					p0 = p[j];
					temp = p0 + h;
					GetH(p0, ref h, temp);
					p[j] = p0 + h;
					f0 = function.f(x);
					p[j] = p0 - h;
					f1 = function.f(x);
					p[j] = p0;
					dfdp[j] = (f0 - f1)/(2*h);
				}
			}
			/// <summary>
			/// The calculating function.
			/// </summary>
			public override double f(double x) {
				function.NEval = NEval;
				if (fast) FastDeriv(x);
				else SlowDeriv(x);
				double y = function.f(x);
				NEval = function.NEval;
				return y;
			}
			/// <summary>
			/// The constructor.
			/// </summary>
			/// <param name="f">The base function to calculate the derivatives of.</param>
			public DerivFunction(Function1DItem f) {
				function = f;
				p = f.p;
				if (f.dfdp.Length == f.p.Length) {
					dfdp = f.dfdp;
					fast = true;
				} else {
					dfdp = new SmallData();
					fast = true;
					dfdp.Length = p.Length;
				}
			}
		}
		#endregion

		#region Marquardt algorithm

		private int mfit = 0;
		private float ochisq = 0;
		private float[] beta, da;
		private double[] ptemp;
		private float[,] oneda;

		float[,] alpha;
		float oldchisq, alamda;

		private void MarquardtRun(object target) {
			const float BIG = 1e28F;
			try {
				mrqmin(fitp, ref covar, ref alpha, ref chisq, ref alamda);
				while (alamda < BIG && (chisq >= oldchisq || oldchisq - chisq > chisq*1e-5F)) {
					mrqmin(fitp, ref covar, ref alpha, ref chisq, ref alamda);
					OnStep();
					oldchisq = chisq;
				}
				if (f is DerivFunction) {
					((DerivFunction)f).fast = false; // calculate more accurate covar matrix
					mrqmin(fitp, ref covar, ref alpha, ref chisq, ref alamda);
				}
				alamda = 0;
				mrqmin(fitp, ref covar, ref alpha, ref chisq, ref alamda);
			} catch (Exception ex) {
				this.ex = ex;
			} finally {
				End();
			}
		}

		private void MarquardtSolve() {
			chisq = 0; oldchisq = -1e10F; alamda = -1;
			if (f0.HasDerivatives) f = f0;
			else f = new DerivFunction(f0);
			this.ex = null;

			Run(new WaitCallback(MarquardtRun));
		}

		private void mrqmin(bool[] ip, ref float[,] covar, ref float[,] alpha,
			ref float chisq, ref float alamda) {

			int j, k, l, mp = f.p.Length, n = data.Length;

			if (alamda < 0) {
				ptemp = new double[mp];
				beta = new float[mp];
				da = new float[mp];
				covar = new float[mp, mp];
				alpha = new float[mp, mp];
				for (mfit = 0, j = 0; j < mp; j++) {
					if (ip[j]) mfit++;
				}
				oneda = new float[mfit, 1];
				alamda = 0.001F;
				mrqcof(ip, ref alpha, ref beta, ref chisq);
				ochisq = chisq;
			}
			for (j = 0; j < mfit; j++) {
				for (k = 0; k < mfit; k++) covar[j, k] = alpha[j, k];
				covar[j, j] = alpha[j, j]*(1+alamda);
				oneda[j, 0] = beta[j];
			}
			gaussj(covar, mfit, oneda, 1);
			for (j = 0; j < mfit; j++) da[j] = oneda[j, 0];
			if (alamda == 0) {
				covsrt(covar, mp, ip, mfit);
				oneda = null; da = beta = null; ptemp = null;
				return;
			}
			for (j = 0, l = 0; l < mp; l++) {
				ptemp[l] = f.p[l];
				if (ip[l]) f.p[l] += da[j++];
			}
			mrqcof(ip, ref covar, ref da, ref chisq);
			if (chisq < ochisq) {
				alamda *= 0.1F;
				ochisq = chisq;
				for (j = 0; j < mfit; j++) {
					for (k = 0; k < mfit; k++) alpha[j, k] = covar[j, k];
					beta[j] = da[j];
				}
			} else {
				for (l = 0; l < mp; l++) f.p[l] = ptemp[l];
				alamda *= 10;
				chisq = ochisq;
			}
		}

		private void mrqcof(bool[] ip, ref float[,] alpha, ref float[] beta, ref float chisq) {
			int i, j, k, l, m, mfit = 0, mp = f.p.Length, n = data.Length;
			float ymod, wt, sig2i, dy;

			for (j = 0; j < mp; j++) {
				if (ip[j]) mfit++;
			}
			for (j = 0; j < mfit; j++) {
				for (k = 0; k <= j; k++) alpha[j, k] = 0;
				beta[j] = 0;
			}
			chisq = 0;
			for (i = 0; i < n; i++) {
				int t = f.NEval;
				ymod = (float)f.f(data.x[i]);
				neval += f.NEval - t;
				sig2i = 1/(float)(data.dy[i]*data.dy[i]);
				dy = (float)data.y[i] - ymod;
				for (j = 0, l = 0; l < mp; l++) {
					if (ip[l]) {
						wt = (float)f.dfdp[l]*sig2i;
						for (k = 0, m = 0; m <= l; m++) {
							if (ip[m]) alpha[j, k++] += wt*(float)f.dfdp[m];
						}
						beta[j++] += dy*wt;
					}
				}
				chisq += dy*dy*sig2i;
			}
			for (j = 1; j < mfit; j++) {
				for (k = 0; k < j; k++) alpha[k, j] = alpha[j, k];
			}
		}

		const int nstart = 10;
		int nmax = nstart;
		int[] indxc = new int[nstart], indxr = new int[nstart], ipiv = new int[nstart];

		private void gaussj(float[,] a, int n, float[,] b, int m) {
			if (n > nmax) {
				nmax = n;
				indxc = new int[nmax];
				indxr = new int[nmax];
				ipiv = new int[nmax];
			}

			int i, icol = 0, irow = 0, j, k, l, ll;
			float big, dum, pivinv, temp;

			for (j = 0; j < n; j++) ipiv[j] = 0;
			for (i = 0; i < n; i++) {
				big = 0;
				for (j = 0; j < n; j++) {
					if (ipiv[j] != 1) {
						for (k = 0; k < n; k++) {
							if (ipiv[k] == 0) {
								if (Math.Abs(a[j, k]) >= big) {
									big = Math.Abs(a[j, k]);
									irow = j;
									icol = k;
								}
							} else if (ipiv[k] > 1) throw new System.ArgumentException(Properties.Resources.gaussjException0);
						}
					}
				}
				++(ipiv[icol]);

				if (irow != icol) {
					for (l = 0; l < n; l++) {
						temp = a[irow, l]; a[irow, l] = a[icol, l]; a[icol, l] = temp;
					}
					for (l = 0; l < m; l++) {
						temp = b[irow, l]; b[irow, l] = b[icol, l]; b[icol, l] = temp;
					}
				}
				indxr[i] = irow;
				indxc[i] = icol;
				if (a[icol, icol] == 0) throw new System.ArgumentException(Properties.Resources.gaussjException1);
				pivinv = 1/a[icol, icol];
				a[icol, icol] = 1;
				for (l = 0; l < n; l++) a[icol, l] *= pivinv;
				for (l = 0; l < m; l++) b[icol, l] *= pivinv;
				for (ll = 0; ll < n; ll++) {
					if (ll != icol) {
						dum = a[ll, icol];
						a[ll, icol] = 0;
						for (l = 0; l < n; l++) a[ll, l] -= a[icol, l]*dum;
						for (l = 0; l < m; l++) b[ll, l] -= b[icol, l]*dum;
					}
				}
			}
			for (l = n-1; l >= 0; l--) {
				if (indxr[l] != indxc[l]) {
					for (k = 0; k < n; k++) {
						temp = a[k, indxr[l]]; a[k, indxr[l]] = a[k, indxc[l]]; a[k, indxc[l]] = temp;
					}
				}
			}

		}

		private void covsrt(float[,] covar, int ma, bool[] ip, int mfit) {
			int i, j, k;
			float t;
			
			for (i = mfit; i < ma; i++) {
				for (j = 0; j <= i; j++) covar[i, j] = covar[j, i] = 0;
			}
			k = mfit - 1;
			for (j = ma-1; j >= 0; j--) {
				if (ip[j]) {
					for (i = 0; i < ma; i++) {
						t = covar[i, k]; covar[i, k] = covar[i, j]; covar[i, j] = t;
					}
					for (i = 0; i < ma; i++) {
						t = covar[k, i]; covar[k, i] = covar[j, i]; covar[j, i] = t;
					}
					k--;
				}
			}
		}
		#endregion

		#region Nelder & Mead algorithm

		private delegate float ChiSqr(float[] p);

		const float BETA = 3, GAMMA = 0.6F, DELTA = 1, FTOL = 0.001F;
		const int ALPHA = 100, N = 15;
		float[,] p;
		float[] y, pb;
		float yb, T, Tnew;
		int ndim, iter, n;
		ChiSqr chifunc;

		private void NelderMeadRun(object target) {
			try {
				startamo(fitp, out p, out y, out pb, out yb, out ndim, out chifunc);
				if (T > 0) T = yb*DELTA;
				iter = ALPHA;
				for (n = 0; T > 0 && n < N; n++) {
					amebsa(p, y, ndim, pb, ref yb, FTOL, chifunc, ref iter, T);
					Tnew = BETA*(y[0] - yb);
					if (Tnew < T*GAMMA || Tnew >= T) T *= GAMMA;
					else T = Tnew;
					iter = ALPHA;
				}
				iter = 5000;
				amebsa(p, y, ndim, pb, ref yb, FTOL, chifunc, ref iter, 0);
			} catch (Exception ex) {
				this.ex = ex;
			}
			try {
				//stopamo(pb); not needed
			} catch (Exception ex) {
				this.ex = ex;
			}
			End();
		}

		private void NelderMeadSolve(bool T0) {
			const int TFAC = 3;
			chisq = 0;
			f = f0;
			if (T0) T = 0;
			else T = data.Length*TFAC;
			this.ex = null;

			Run(new WaitCallback(NelderMeadRun));
		}

		private double[] p0;
		private bool[] ip0;
		private float[] ptry;

		private float chisqr(float[] p) {
			int i, j;
			float chisq, ymod, sig2i, dy;

			for (i = 0, j = 0; i < f.p.Length; i++) {
				p0[i] = f.p[i];
				if (ip0[i]) f.p[i] = p[j++];
			}

			chisq = 0;
			for (i = 0; i < data.Length; i++) {
				int t = f.NEval;
				ymod = (float)f.f(data.x[i]);
				neval += f.NEval - t;
				sig2i = 1/(float)(data.dy[i]*data.dy[i]);
				dy = (float)data.y[i] - ymod;
				chisq += dy*dy*sig2i;
			}
			for (i = 0; i < f.p.Length; i++) f.p[i] = p0[i];

			return chisq;
		}
		
		private void startamo(bool[] ip, out float[,] p, out float[] y, out float[] pb, out float yb,
			out int ndim, out ChiSqr chisq) {

			int i, j, ilo;
			float[] pp;
			ip0 = ip;
			chisq = new ChiSqr(chisqr);
			ndim = 0;
			for (i = 0; i < ip.Length; i++) {
				if (ip[i]) ndim++;
			}

			p0 = new double[ip.Length];
			p = new float[ndim+1, ndim];
			y = new float[ndim+1];
			ptry = new float[ndim];

			for (i = 0, j = 0; i < ip.Length; i++) {
				if (ip[i]) p[0, j++] = (float)f.p[i];
			}
			for (i = 1; i <= ndim; i++) {
				for (j = 0; j < ndim; j++) {
					if (j == i-1) p[i, j] = p[0, j]*2;
					else p[i, j] = p[0, j];
				}
			}
			
			pp = new float[ndim];
			yb = float.MaxValue;
			ilo = 0;
			for (i = 0; i <= ndim; i++) {
				for (j = 0; j < ndim; j++) pp[j] = p[i, j]; 
				y[i] = chisq(pp);
				if (y[i] < yb) {
					ilo = i;
					yb = y[i];
				}
			}

			pb = new float[ndim];
			for (i = 0; i < ndim; i++) pb[i] = p[ilo, i];

		}

		private void stopamo(float[] pb) {
			int i, j;
			
			float[,] alpha = null;
			float alamda;
			chisq = 0;
			if (!f0.HasDerivatives) {
				f = new DerivFunction(f0);
				if (f is DerivFunction) ((DerivFunction)f).fast = false;
			} else f = f0;

			for (i = 0, j = 0; i < f.p.Length; i++) {
				if (ip0[i]) f.p[i] = pb[j++];
			}

			alamda = -1;
			mrqmin(ip0, ref covar, ref alpha, ref chisq, ref alamda);
			alamda = 0;
			mrqmin(ip0, ref covar, ref alpha, ref chisq, ref alamda);

			ip0 = null;
			p0 = null;
			ptry = null;
		}
		
		private void amebsa(float[,] p, float[] y, int ndim, float[] pb, ref float yb, float ftol, ChiSqr f,
			ref int iter, float temptr) {
			
			int i, ihi, ilo, j, m, n, mpts = ndim + 1;
			float rtol, sum, swap, yhi, ylo, ynhi, ysave, yt, ytry, tt;
			float[] psum = new float[ndim];

			tt = -temptr;
			for (n = 0; n < ndim; n++) {
				for (sum = 0, m = 0; m < mpts; m++) sum += p[m, n];
				psum[n] = sum;
			}
			while (true) {
				ilo = 0;
				ihi = 1;
				ynhi = ylo = y[0] + tt*(float)Math.Log(ran0());
				yhi = y[1] + tt*(float)Math.Log(ran0());
				if (ylo > yhi) {
					ihi = 0;
					ilo = 1;
					ynhi = yhi;
					yhi = ylo;
					ylo = ynhi;
				}
				for (i = 2; i < mpts; i++) {
					yt = y[i] + tt*(float)Math.Log(ran0());
					if (yt <= ylo) {
						ilo = i;
						ylo = yt;
					}
					if (yt > yhi) {
						ynhi = yhi;
						ihi = i;
						yhi = yt;
					} else if (yt > ynhi) {
						ynhi = yt;
					}
				}
				rtol = 2*Math.Abs(yhi - ylo)/(Math.Abs(yhi) + Math.Abs(ylo));
				if (rtol < ftol || iter < 0) {
					swap = y[0]; y[0] = y[ilo]; y[ilo] = swap;
					for (n = 0; n < ndim; n++) {
						swap = p[0, n]; p[0, n] = p[ilo, n]; p[ilo, n] = swap;
					}
					break;
				}
				iter -= 2;
				ytry = amotsa(p, y, psum, ndim, pb, ref yb, f, ihi, ref yhi, temptr, -1);
				if (ytry <= ylo) {
					ytry = amotsa(p, y, psum, ndim, pb, ref yb, f, ihi, ref yhi, temptr, 2);
				} else if (ytry >= ynhi) {
					ysave = yhi;
					ytry = amotsa(p, y, psum, ndim, pb, ref yb, f, ihi, ref yhi, temptr, 0.5F);
					if (ytry >= ysave) {
						for (i = 0; i < mpts; i++) {
							if (i != ilo) {
								for (j = 0; j < ndim; j++) {
									p[i, j] = psum[j] = 0.5F*(p[i, j] + p[ilo, j]);
								}
								y[i] = f(psum);
							}
						}
						iter -= ndim;
						for (n = 0; n < ndim; n++) {
							for (sum = 0, m = 0; m < mpts; m++) sum += p[m, n];
							psum[n] = sum;
						}
					}
				} else ++iter;
				if (Step != null && System.Environment.TickCount - t0 >= stepInterval) { 
					for (i = 0, j = 0; i < this.f.p.Length; i++) { // set this.f.p to best value
						if (ip0[i]) this.f.p[i] = pb[j++];
					}
					OnStep();
				}
			}
		}

		private float amotsa(float[,] p, float[] y, float[] psum, int ndim, float[] pb, ref float yb,
			ChiSqr f, int ihi, ref float yhi, float temptr, float fac) {

			int j;
			float fac1, fac2, yflu, ytry, tt;

			tt = -temptr;
			fac1 = (1 - fac)/ndim;
			fac2 = fac1 - fac;
			for (j = 0; j < ndim; j++) ptry[j] = psum[j]*fac1 - p[ihi, j]*fac2;
			ytry = f(ptry);
			if (ytry <= yb) {
				for (j = 0; j < ndim; j++) pb[j] = ptry[j];
				chisq = yb = ytry;
			}
			yflu = ytry - tt*(float)Math.Log(ran0());
			if (yflu < yhi) {
				y[ihi] = ytry;
				yhi = yflu;
				for (j = 0; j < ndim; j++) {
					psum[j] += ptry[j] - p[ihi, j];
					p[ihi, j] = ptry[j];
				}
			}
			return yflu;
		}

		private int idum = System.Environment.TickCount;

		private float ran0() {
			const int IA = 16807;
			const int IM = 2147483647;
			const float AM = 1.0F/IM;
			const int IQ = 127773;
			const int IR = 2836;
			const int MASK = 123459876;
			int k;
			float ans;

			idum ^= MASK;
			k = idum/IQ;
			idum = IA*(idum - k*IQ) - IR*k;
			if (idum < 0) idum += IM;
			ans = AM*idum;
			idum ^= MASK;
			return ans;
		}	
		#endregion

		#region IAsyncResult Members

		private object asyncState;
		/// <summary>
		/// The <see cref="IAsyncResult.AsyncState"/> object, representing the state of the asynchronous operation.
		/// </summary>
		public object AsyncState {
			get { return asyncState; }
		}

		private AutoResetEvent signal = new	AutoResetEvent(false);
		/// <summary>
		/// A <see cref="WaitHandle"/> that can be used to wait for the fitting to finish, by calling AsyncWaitHandle.<see cref="WaitHandle.WaitOne()"/>.
		/// </summary>
		public WaitHandle AsyncWaitHandle {
			get { return signal; }
		}
		/// <summary>
		/// Return true if the fitting was done synchronously (by the method <see cref="Solve()"/>) and fitting is done.
		/// </summary>
		public bool CompletedSynchronously {
			get { lock (this) return !fitting && runMode == RunMode.Synchronous; }
		}
		/// <summary>
		/// Returns true if the fitting is done.
		/// </summary>
		public bool IsCompleted {
			get { lock (this) return !fitting; }
		}

		#endregion
	}
}
