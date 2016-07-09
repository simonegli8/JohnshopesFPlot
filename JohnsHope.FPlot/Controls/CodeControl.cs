using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.CodeDom.Compiler;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using JohnsHope.FPlot.Library;
using System.ComponentModel;

namespace JohnsHope.FPlot {
	[ToolboxBitmap(typeof(TextBox))]
	public class CodeControl: Control {
		
		public int ErrorRowHeight = 22;
		public int MaxErrorsDisplayed = 4;

		private TextEditorControl editor;
		private TextBox errorst;
		public DataGridView errors;

		private SplitContainer splitter;

		public CodeControl() {
			editor = new TextEditorControl();
			editor.RightToLeft = System.Windows.Forms.RightToLeft.No;
			editor.AllowDrop = true;
			editor.AutoScroll = true;
			editor.TabIndent = 2;
			editor.ShowEOLMarkers = false;
			editor.ShowInvalidLines = false;
			editor.ShowSpaces = false;
			editor.ShowTabs = false;
			editor.ShowVRuler = false;
			editor.UseAntiAliasFont = true;
			editor.IsIconBarVisible = false;
			editor.Dock = DockStyle.Fill;
			editor.Document.HighlightingStrategy =
				ICSharpCode.TextEditor.Document.HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
			editor.Document.DocumentChanged += OnDocumentChanged;
			
			errorst = new TextBox();
			errorst.Dock = DockStyle.Fill;
			errorst.ReadOnly = true;
			errorst.Multiline = true;
			errorst.BackColor = Color.White;

			errors = new DataGridView();
			errors.ReadOnly = true;
			errors.Dock = DockStyle.Fill;
			errors.ColumnHeadersVisible = false;
			errors.RowHeadersVisible = false;
			errors.ColumnCount = 3;
			errors.ScrollBars = ScrollBars.Vertical;
			errors.AllowUserToAddRows = false;
			errors.Columns[0].Width = 16;
			errors.Columns[0].Resizable = DataGridViewTriState.False;
			errors.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			errors.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			errors.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			errors.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			errors.CellMouseClick += DoCellClick;
			errors.BorderStyle = BorderStyle.None;
			errors.GridColor = SystemColors.Control;
			errors.DefaultCellStyle.SelectionBackColor = errors.DefaultCellStyle.BackColor; // hide selections
			errors.DefaultCellStyle.SelectionForeColor = errors.DefaultCellStyle.ForeColor;
			
			splitter = new SplitContainer();
			splitter.Dock = DockStyle.Fill;
			splitter.Orientation = Orientation.Horizontal;
			splitter.Panel1.Controls.Add(editor);
			splitter.Panel2.Controls.Add(errors);
			//splitter.FixedPanel = FixedPanel.Panel2;

			splitter.Panel2Collapsed = true;

			Controls.Add(splitter);
		}

		public void SetCaret(int line, int col) {
			editor.Focus();
			TextAreaControl ctl = editor.ActiveTextAreaControl;
			if (ctl != null) ctl.Caret.Position = new Point(col - 1, line - 1);
		}

		[Browsable(true)]
		public override Color BackColor { get { return base.BackColor; } set { errors.BackgroundColor = base.BackColor = value; } }

		protected override void OnParentChanged(EventArgs e) {
			base.OnParentChanged(e);
			BackColor = Parent.BackColor;
		}

		private void DoCellClick(object sender, DataGridViewCellMouseEventArgs e) {
			DataGridViewCell cell = errors[2, e.RowIndex];
			if (cell.Tag != null && cell.Tag is CompilerError) {
				CompilerError err = (CompilerError)cell.Tag;
				if (err.Line >= 0) SetCaret(err.Line, err.Column);
				else {
					cell = errors[0, e.RowIndex];
					if (cell.Tag != null && cell.Tag is ICompilable) {
						ICompilable ic = (ICompilable)cell.Tag;
						if (ic.FirstSourceLine + err.Line <= 0) CompilerOptionsForm.MarkErrors(ic, err);
					}
				}
			}
		}


		public void HideErrors() {
			splitter.Panel2Collapsed = true;
		}

		public void ShowErrors() {
			//int rows = Math.Min(MaxErrorsDisplayed, errors.Rows.Count);
			int height = errors.PreferredSize.Height - 4;
/*			if (rows > 0) {
				for (int i = 0; i < rows; i++) {
					height += errors.Rows[i].Height + 3;
				}
			}
	*/
			height = Math.Min(ErrorRowHeight*MaxErrorsDisplayed, height);

			splitter.SplitterDistance = splitter.Height - height;
			splitter.Panel2Collapsed = errors.Rows.Count == 0;
		}

		[Browsable(false)]
		public TextEditorControl Editor {
			get { return editor; }
		}
		[Browsable(false)]
		public override string Text {
			get { return editor.Text; }
			set { RemoveMarks(); editor.Text = value; editor.Refresh(); }
		}

		public void RemoveMarks() {
			editor.Document.MarkerStrategy.RemoveAll(new Predicate<TextMarker>(delegate(TextMarker m) { return true; }));
			HideErrors();
		}

		public void Edit(SourceLocation loc) {
			if (loc.Exception != null) {
				LineSegment ls = editor.Document.GetLineSegment(loc.Line-1);
				TextMarker marker = new TextMarker(ls.Offset, ls.Length, TextMarkerType.SolidBlock, Color.Yellow);
				marker.ToolTip = loc.Exception.Message;
				editor.Document.MarkerStrategy.AddMarker(marker);
			} else {
				editor.Focus();
				TextAreaControl ctl = editor.ActiveTextAreaControl;
				if (ctl != null) ctl.Caret.Position = new Point(loc.Column - 1, loc.Line - 1);
			}
			editor.Refresh();
		}

		public TextMarker ErrMarker(CompilerError err) {
			IDocument doc = editor.Document;
			LineSegment line = null;

			if (err.Line > 0 && err.Line < doc.LineSegmentCollection.Count + 1) line = doc.GetLineSegment(err.Line - 1);
			if (line != null) {
				int start = line.Offset + err.Column - 1;
				int end = line.Offset + line.Length;
				int offset = start;
				TextMarker marker;
				if (offset < end) {
					TextUtilities.CharacterType type = TextUtilities.GetCharacterType(doc.GetCharAt(offset++));

					while (offset < end && (TextUtilities.GetCharacterType(doc.GetCharAt(offset)) == type)) offset++;
				}
				Color color;
				if (err.IsWarning) color = Color.Blue;
				else color = Color.Red;
				marker = new TextMarker(start, offset-start, TextMarkerType.WaveLine, color);				
				marker.ToolTip = err.ErrorText;
				return marker;
			}
			return null;
		}

		public static int ErrorLine(ICompilable item, bool sourceHeader, int line) {
			if (line < 0) {
				if (sourceHeader) return Compiler.Options.Header.Lines + item.FirstSourceLine + line;
			}
			return line;
		}

		private CompilerErrorCollection Errors(ICompilable item, bool sourceHeader) {
			CompilerErrorCollection err = new CompilerErrorCollection();
			foreach (CompilerError e in item.CompilerResults.Errors) { // deep copy error collection
				err.Add(new CompilerError(e.FileName, e.Line, e.Column, e.ErrorNumber, e.ErrorText));
			}
			for (int i = 0; i < err.Count; i++) { // adapt errors according to sourceHeader
				if (err[i].Line < 0) {
					if (sourceHeader) err[i].Line = ErrorLine(item, sourceHeader, err[i].Line);
				} else if (sourceHeader) err.RemoveAt(i--); // don't mark ordinary errors in SourceHeader.
			}
			return err;
		}

		public void MarkErrors(ICompilable item, bool sourceHeader) {
			RemoveMarks();
			MarkerStrategy markers = editor.Document.MarkerStrategy;
			string filename = Compiler.SourceFile(item).ToLower();
			
			errors.Rows.Clear();

			foreach (CompilerError err in Errors(item, sourceHeader)) {
				if (filename == err.FileName.ToLower()) {
					TextMarker marker = ErrMarker(err);
					if (marker != null) markers.AddMarker(marker);
					
					// update data grid
					DataGridViewRow row = new DataGridViewRow();

					DataGridViewImageCell img = new DataGridViewImageCell();
					if (err.IsWarning) img.Value = Properties.Resources.warningimg;
					else img.Value = Properties.Resources.errorimg;
					img.Tag = item;
					row.Cells.Add(img);

					DataGridViewTextBoxCell pos = new DataGridViewTextBoxCell();
					pos.Value = err.Line.ToString() + ":" + err.Column.ToString();
					row.Cells.Add(pos);
					
					DataGridViewTextBoxCell text = new DataGridViewTextBoxCell();
					text.Value = CompilerErrors.Message(err);
					text.Style.WrapMode = DataGridViewTriState.True;
					text.Tag = err;
					row.Cells.Add(text);
					
					errors.Rows.Add(row);
					row.Resizable = DataGridViewTriState.False;
				}

			}

			errors.AutoResizeRows();
			if (item.CompilerResults.Errors.Count > 0) ShowErrors();
			editor.Refresh();
		}

		public void MarkErrors(ICompilable item) { MarkErrors(item, false); }

		public new event EventHandler TextChanged;
	
		private void OnDocumentChanged(object sender, DocumentEventArgs e) {
			if (TextChanged != null) TextChanged(this, e);
		}
		
	}
}
