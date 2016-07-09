using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using JohnsHope.FPlot.Library;
using JohnsHope.FPlot.Properties;
using System.Threading;

namespace JohnsHope.FPlot {

	[ToolboxBitmap(typeof(DataGridView))]
	public class ImportsGrid: DataGridView, IEnumerable<string> {

		OpenFileDialog fileDialog;

		public List<string> Imports = new List<string>();
		public List<bool> Packaged = new List<bool>();

		public ImportsGrid() {
			ColumnHeadersVisible = true;
			RowHeadersVisible = false;
			ColumnCount = 5;

			DataGridViewRow row;
			DataGridViewImageCell glass, delete;
			DataGridViewCheckBoxCell pack;
			DataGridViewTextBoxCell name, path;

			glass = new GlassImageCell();
			delete = new DeleteImageCell();
			pack = new DataGridViewCheckBoxCell();

			name = new DataGridViewTextBoxCell();
			name.Value = "";

			path = new DataGridViewTextBoxCell();
			path.Value = "";

			row = new DataGridViewRow();
			row.Cells.AddRange(glass, delete, pack, name, path);
			path.ReadOnly = true;

			RowTemplate = row;

			Columns[0].Width = 16;
			Columns[0].Resizable = DataGridViewTriState.False;
			Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

			Columns[1].Width = 16;
			Columns[1].Resizable = DataGridViewTriState.False;
			Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
			
			Columns[2].Resizable = DataGridViewTriState.False;
			Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
	
			Columns[3].Resizable = DataGridViewTriState.False;
			Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;

			Columns[4].Resizable = DataGridViewTriState.False;
			Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;

			AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			AllowUserToAddRows = true;
			ScrollBars = ScrollBars.Both;
			AllowUserToDeleteRows = true;
			AllowUserToResizeColumns = false;
			AllowUserToResizeRows = false;
			ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			CellMouseClick += DoCellClick;
			CellEndEdit += DoEndEdit;
			CellPainting += DoCellPainting;
			DataError += DoDataError;
			BorderStyle = BorderStyle.None;
			GridColor = SystemColors.Control;

			Columns[2].HeaderCell.ToolTipText = Properties.Resources.PackageToolTip;
			Columns[3].HeaderCell.Value = "Assembly";
			Columns[4].HeaderCell.Value = "Path";

			// Reset();

			fileDialog = new OpenFileDialog();
			fileDialog.AddExtension = true;
			fileDialog.CheckFileExists = fileDialog.CheckPathExists = false;
			fileDialog.DefaultExt = "dll";
			fileDialog.Filter = "dll's & exe (*.dll, *.exe)|*.dll;*.exe";
			fileDialog.Multiselect = false;
			fileDialog.Title = "Select dll library";
		}

		void DoCellPainting(object sender, DataGridViewCellPaintingEventArgs e) { // draw zip icon for 2. header cell, draw icons for first and second column
			if (e.RowIndex == -1) {
				if (e.ColumnIndex == 2) {
					Icon icon = Properties.Resources.SmallZip;
					e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground); // paint cell background
					Rectangle r = new Rectangle(e.CellBounds.X + e.CellBounds.Width/2 - icon.Width/2,
						e.CellBounds.Y + e.CellBounds.Height/2 - icon.Height/2, icon.Width, icon.Height);
					e.Graphics.DrawIconUnstretched(icon, r);
					e.Handled = true;
				}
			} 
			/*
			else if (e.ColumnIndex == 1 || e.ColumnIndex == 2) { // first & second column: paint image
				Image img = Properties.Resources.delete;
				e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground); // paint cell background
				e.Graphics.DrawImageUnscaled(img);
			}
			*/
		}

		public class GlassImageCell: DataGridViewImageCell {
			public GlassImageCell() : base() { Value = Properties.Resources.glass; }
		}

		public class DeleteImageCell: DataGridViewImageCell {
			public DeleteImageCell() : base() { Value = Properties.Resources.delete; }
		}

		public class GlassImageColumn: DataGridViewColumn {
		
		}

		string this[int i] {
			get {
				string s4, s5;
				s4 = Rows[i+1].Cells[3].Value as string;
				s5 = Rows[i+1].Cells[4].Value as string;
				if (s5 == "") return s4;
				else return Path.Combine(s5, s4);
			}
			set {
				bool packaged;

				if (i < Packaged.Count) packaged = Packaged[i];
				else packaged = false;

				string path, name;
				if (Path.IsPathRooted(value)) {
					path = Path.GetDirectoryName(value);
					if (!File.Exists(value)) packaged = false;
				} else {
					packaged = false;
					path = "";
				}
				name = Path.GetFileName(value);

				if (i <= Rows.Count - 1) {
					Rows[i].Cells[2].Value = packaged;
					//((DataGridViewCheckBoxCell)Rows[i].Cells[2]).Disabled = path == "";
					Rows[i].Cells[3].Value = name;
					Rows[i].Cells[4].Value = path;
				} else throw new IndexOutOfRangeException("Invalid row index.");

				if (i < Imports.Count) {
					Imports[i] = value;
					Packaged[i] = packaged;
				} else if (i == Imports.Count) {
					Imports.Add(value);
					Packaged.Add(packaged);
				} else throw new IndexOutOfRangeException();
			}
		}

		[Browsable(true)]
		public new Color BackColor { get { return base.BackColor; } set { BackgroundColor = base.BackColor = value; } }

		protected override void OnParentChanged(EventArgs e) {
			base.OnParentChanged(e);
			BackColor = Parent.BackColor;
		}

		public void Add(string file) {
			this[Rows.Count-1] = file;
		}

		public void Reset() {
			while (Rows.Count > 1) Rows.RemoveAt(0);

			Imports = new List<string>(Compiler.Options.Imports);
			int n = Imports.Count;
			Imports.AddRange(Compiler.Options.PackagedImports);
			Packaged = new List<bool>(Imports.Count);
			for (int i = 0; i < n; i++) Packaged.Add(false);
			for (int i = n+1; i < Imports.Count; i++) Packaged.Add(true);

			for (int r = 0; r < Imports.Count; r++) {
				this[r] = Imports[r];
			}
		}

		public void Commit() {
			List<string> imp = new List<string>(), packimp = new List<string>();
			Packaged.Clear();
			Imports.Clear();
			for (int i = 0; i < imp.Count; i++) {
				Packaged.Add((bool)Rows[i+1].Cells[3].Value);
				Imports.Add(this[i]);
				if (Packaged[i]) packimp.Add(Imports[i]);
				else imp.Add(Imports[i]);
			}
			Compiler.Options.Imports = Compiler.Options.SortImports(imp);
			Compiler.Options.PackagedImports = new CompilerOptions.PackagedAssemblyCollection(packimp);
		}

		public void DoCellClick(object sender, DataGridViewCellMouseEventArgs e) {
			switch (e.ColumnIndex) {
			case 0: FileDialog(e); break;
			case 1: Delete(e); break;
			case 3:
				if (e.Clicks > 1) FileDialog(e);
				break;
			default: break;
			}
		}

		public void Delete(DataGridViewCellMouseEventArgs e) {
			int row = e.RowIndex;
			if (row > 0 && row < Rows.Count-1) {
				Rows.RemoveAt(row);
				Imports.RemoveAt(row-1);
				Packaged.RemoveAt(row-1);
			}
		}

		public void FileDialog(DataGridViewCellMouseEventArgs e) {
			if (e.RowIndex < Rows.Count - 1) fileDialog.FileName = this[e.RowIndex];
			else fileDialog.FileName = "";

			bool global = !Path.IsPathRooted(fileDialog.FileName);

			string globalPath = Path.GetDirectoryName(typeof(object).Module.FullyQualifiedName);

			if (global) fileDialog.FileName = Path.Combine(globalPath, "");

			if (fileDialog.ShowDialog() == DialogResult.OK) {

				string file = fileDialog.FileName;
	
				if (file.StartsWith(globalPath)) file = Path.GetFileName(file);

				this[e.RowIndex] = file;
			}
		}

		public void DoEndEdit(object sender, DataGridViewCellEventArgs e) {
			int row = e.RowIndex;
			string s4, s5, ext, dll;
			s4 = Rows[row].Cells[3].Value as string;
			s5 = Rows[row].Cells[4].Value as string;
			if (s4 == null) s4 = "";
			if (s5 == null) s5 = "";
			ext = Path.GetExtension(s4);
			if (s4 != "" && ext != ".dll" && ext != ".exe") s4 += ".dll";
			dll = Path.Combine(s5, s4);
			this[row] = dll;
		}

		public IEnumerator<string> GetEnumerator() {
			return Imports.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return Imports.GetEnumerator();
		}

		public void DoDataError(object sender, DataGridViewDataErrorEventArgs e) {

			DataGridViewCell cell = Rows[e.RowIndex].Cells[e.ColumnIndex];

		}

	}
}
