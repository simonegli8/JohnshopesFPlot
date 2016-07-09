using System;
using System.Collections.Generic;
using JohnsHope.FPlot.Library;
using System.Windows.Forms;

namespace JohnsHope.FPlot {
	public class FormStack<FormType> where FormType: Form  {

		private List<Library.WeakReference<FormType>> forms = new List<Library.WeakReference<FormType>>();
		private EventHandler activateHandler;
		private EventHandler closeHandler;
		public static event EventHandler FormActivated;

		public FormStack() {
			activateHandler = new EventHandler(OnActivated);
			closeHandler = new EventHandler(OnClosed);
		}

		private void OnActivated(object sender, EventArgs e) {
			if (sender is FormType) {
				Push((FormType)sender);
				FormActivated(sender, e);
			}
		}

		private void OnClosed(object sender, EventArgs e) {
			if (sender is FormType) {
				Pop((FormType)sender);
				FormActivated(Top, e);
			}
		}


		public void Pop(FormType form) {
			for (int i = 0; i < forms.Count; i++) {
				if (forms[i].Target == form) forms.RemoveAt(i--);
			}
			if (form != null && !form.IsDisposed) {
				form.Activated -= activateHandler;
				form.Disposed -= closeHandler;
			}
		}

		public void Push(FormType form) {
			Pop(form);
			forms.Add(new Library.WeakReference<FormType>(form));
			form.Activated += activateHandler;
			form.Disposed += closeHandler;
		}

		public FormType Top {
			get {
				int i = forms.Count - 1;
				while (i >= 0) {
					FormType form = forms[i].Target;
					if (form != null && !form.IsDisposed) return form;
					Pop(form);
					i--;
				}
				return null;
			}				
			set { Push(value); }
		}

		public int Index(FormType form) {
			int i = 0;
			while (i < forms.Count && forms[i+1].Target != form) i++;
			if (i == forms.Count) return -1;
			else return i;
		}

	}
}
