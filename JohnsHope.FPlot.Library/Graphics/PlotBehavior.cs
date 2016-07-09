using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace JohnsHope.FPlot.Library {
	public partial class Plot {

		/// <summary>
		/// Reacts on Control.OnMouseDown events
		/// </summary>
		public virtual void OnMouseDown(MouseEventArgs e) { }
		/// <summary>
		/// Reacts on Control.OnMouseMove events
		/// </summary>
		public virtual void OnMouseMove(MouseEventArgs e) { }
		/// <summary>
		/// Reacts on Control.OnMouseUp events
		/// </summary>
		public virtual void OnMouseUp(MouseEventArgs e) { }
		/// <summary>
		/// Reacts on Control.OnMouseWheel events
		/// </summary>
		public virtual void OnMouseWheel(MouseEventArgs e) { }
		/// <summary>
		/// Reacts on resizing of the control.
		/// </summary>
		public virtual void OnResize(EventArgs e) { }

		/// <summary>
		/// This delegate is a function that is called upon mouse cursor moves inside the PlotControl
		/// </summary>
		public delegate void NotifyCursorHandler(double x, double y, double z);
		/// <summary>
		/// This event notifies about mouse cursor moves inside the PlotControl
		/// </summary>
		public event NotifyCursorHandler NotifyCursor;
		/// <summary>
		/// Fires the NotifyCursor event
		/// </summary>
		public void FireNotifyCursor(double x, double y, double z) {
			if (NotifyCursor != null) NotifyCursor(x, y, z);
		}
	}
}
