using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using JohnsHope.FPlot.Library;

namespace JohnsHope.FPlot {
	public partial class ConsoleForm: Form, IDisposable {
	
		class EditorWriter: TextWriter {
			public StringBuilder buf;
			TextWriter oldOut;
			TextWriterTraceListener Listener;
			public List<DocumentWriter> Listeners = new List<DocumentWriter>();

			public EditorWriter(StringBuilder buf) {
				this.buf = buf;

				oldOut = Console.Out;
				Console.SetOut(this);
				Debug.Listeners.Clear();
				Listener = new TextWriterTraceListener(this);
				Debug.Listeners.Add(Listener);
			}

			~EditorWriter() {
				Console.SetOut(oldOut);
				Debug.Listeners.Remove(Listener);
			}
			
			public override void Write(char ch) {
				buf.Append(ch);
				foreach (DocumentWriter w in Listeners) w.Write(ch);
				oldOut.Write(ch);
			}
			public override void Write(string s) {
				buf.Append(s);
				foreach (DocumentWriter w in Listeners) w.Write(s);
				oldOut.Write(s);
			}

			public override void WriteLine(string s) {
				buf.Append(s + "\n");
				foreach (DocumentWriter w in Listeners) w.WriteLine(s);
				oldOut.WriteLine(s);
			}

			public override Encoding Encoding {
				get { return Encoding.Unicode; }
			}

			public virtual void Clear() {
				buf.Remove(0, buf.Length);
				foreach (DocumentWriter w in Listeners) w.Clear();
			}
		}

		static EditorWriter ew = new EditorWriter(new StringBuilder());

		class DocumentWriter: TextWriter, IDisposable {
			IDocument document;
			TextEditorControl editor;
			Control Parent;

			public DocumentWriter(IDocument Document, TextEditorControl Editor, Control Parent) {
				document = Document;
				this.Parent = Parent;
				editor = Editor;
				document.ReadOnly = false;
				document.Insert(0, ew.buf.ToString());
				document.ReadOnly = true;
				ew.Listeners.Add(this);
				editor.Refresh();
			}

			delegate void WriteHandler(string txt, bool refresh);

			public void HandleWrite(string txt, bool refresh) {
				lock (this) {
					if (document != null) {
						document.ReadOnly = false;
						document.Insert(document.TextLength, txt);
						document.ReadOnly = true;
						if (refresh) editor.Refresh();
					}
				}
			}

			public override void Write(char ch) {
				if (Parent.IsHandleCreated) Parent.Invoke(new WriteHandler(HandleWrite), ch.ToString(), true);
				else HandleWrite(ch.ToString(), false);
			}

			public override void Write(string s) {
				if (Parent.IsHandleCreated) Parent.Invoke(new WriteHandler(HandleWrite), s, true);
				else HandleWrite(s, false);
			}

			public override void WriteLine(string s) {
				if (Parent.IsHandleCreated) Parent.Invoke(new WriteHandler(HandleWrite), s + "\n", true);
				else HandleWrite(s + "\n", false);
			}

			public override Encoding Encoding {
				get { return Encoding.Unicode; }
			}

			public void Clear() {
				if (document != null) {
					document.ReadOnly = false;
					document.Remove(0, document.TextLength);
					document.ReadOnly = true;
					editor.Refresh();
				}
			}

			public new void Dispose() {
				base.Dispose();
				ew.Listeners.Remove(this);
			}
		}

		DocumentWriter dw;
		MainModel Model;

		public ConsoleForm(MainModel Model) {
			this.Model = Model;
			InitializeComponent();
			text.ShowEOLMarkers = text.ShowHRuler = text.ShowSpaces = text.ShowTabs = text.ShowVRuler = text.ShowInvalidLines =
				text.IsIconBarVisible = false;
			text.Document.ReadOnly = true;
			dw = new DocumentWriter(text.Document, text, Model.MainForm);
		}

		public new void Dispose() {
			base.Dispose();
			dw.Dispose();
		}

		private void Clear(object sender, EventArgs e) {
			ew.Clear();
		}

		private void Close(object sender, EventArgs e) {
			Hide();
			Dispose();
		}
	
		private void Close(object sender, FormClosedEventArgs e) {
			Hide();
			Dispose();
		}

		private void ShowHelp(object sender, EventArgs e) {
			Help.ShowHelp(this, Properties.Resources.HelpFile, "ConsoleForm.html");
		}

		private void RunClick(object sender, EventArgs e) {
			Command c = new Command(command.Text);
			command.MarkErrors(c);
			if (c.Exception != null) {
				ExceptionForm exForm = new ExceptionForm(Model, c.Exception);
				exForm.Show();
				exForm.BringToFront();
			}
		}

		public void EditCommand(SourceLocation loc) {
			if (!(loc.Source is Command)) throw new Exception("loc.Source is no Command");
			command.Text = ((Command)loc.Source).Source;
			command.Edit(loc);
		}

		public void helpLibraryClick(object sender, EventArgs e) {
			Help.ShowHelp(this, Properties.Resources.LibraryHelpFile);
		}

	}
}