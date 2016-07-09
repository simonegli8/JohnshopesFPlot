using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Net;
using System.Windows.Forms;

namespace JohnsHope.Update {

	public partial class DownloadForm: Form {

		DateTime Start = DateTime.Now;

		public DownloadForm() {
			InitializeComponent();
			progress.Maximum = 100;
		}

		WebClient Web;
		string Filename;

		public DownloadForm(string filename, WebClient web): this() {
			Filename = filename;
			File.Text = "...";
			Web = web;
			Speed.Text = "0 KB/s";
		}

		public void Progress(object sender, DownloadProgressChangedEventArgs e) {
			if (InvokeRequired) {
				Invoke(new DownloadProgressChangedEventHandler(Progress), sender, e);
			} else {
				File.Text = Filename;
				double seconds = (DateTime.Now - Start).TotalSeconds;
				double speed = e.BytesReceived / seconds;
				const int K = 1024;
				const int M = K*K;
				if (speed > M) Speed.Text  = string.Format("{0:F0} MB/s", speed / M);
				else if (speed > K) Speed.Text = string.Format("{0:F0} KB/s", speed / K);
				else Speed.Text = string.Format("{0:F0} Bytes/s", speed);
				progress.Value = e.ProgressPercentage;
			}
		}

		public void CancelClick(object sender, EventArgs e) {
			Hide();
			Web.CancelAsync();
			Application.Exit();
		}

	}
}
