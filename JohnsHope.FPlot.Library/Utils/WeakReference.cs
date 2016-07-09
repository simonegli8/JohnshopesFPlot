using System;
using System.Collections.Generic;
using System.Text;

namespace JohnsHope.FPlot.Library {
	/// <summary>
	/// This is a generic <see cref="WeakReference"/> type
	/// </summary>
	public class WeakReference<Type>: WeakReference where Type: class {
		/// <summary>
		/// The constructor of the WeakReference
		/// </summary>
		public WeakReference(Type target): base(target) { }
		/// <summary>
		/// The object the WeakReference points to. null if the object doesn't exist anymore.
		/// </summary>
		public new Type Target {
			get { return base.Target as Type; }
			set { base.Target = value; }
		}
	}
}
