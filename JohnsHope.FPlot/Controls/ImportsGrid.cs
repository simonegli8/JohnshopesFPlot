using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using SourceGrid2;
using SourceGrid2.Cells.Real;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using JohnsHope.FPlot.Library;
using JohnsHope.FPlot.Properties;

namespace JohnsHope.FPlot {
	[ToolboxBitmap(typeof(DataGridView))]
	public class ImportsGrid: Grid, IEnumerable<string> {
		BehaviorDelete behaviorDelete;
		BehaviorEdit behaviorEdit;
		Image glassImg;
		Image deleteImg;

		OpenFileDialog fileDialog;

		public List<string> Imports;

		public ImportsGrid() {
			//get images from the global resource.
			glassImg = Resources.glass;
			deleteImg = Resources.delete;
			behaviorDelete = new BehaviorDelete();
			behaviorEdit = new BehaviorEdit();
			ColumnsCount = 6;
			RowsCount = 2;
			this[0, 0] = new SourceGrid2.Cells.Real.ColumnHeader("");
			((SourceGrid2.Cells.Real.ColumnHeader)this[0, 0]).EnableSort = false;
			this[0, 1] = new SourceGrid2.Cells.Real.ColumnHeader("");
			((SourceGrid2.Cells.Real.ColumnHeader)this[0, 1]).EnableSort = false;
			this[0, 2] = new SourceGrid2.Cells.Real.ColumnHeader("");
			((SourceGrid2.Cells.Real.ColumnHeader)this[0, 2]).EnableSort = false;
			this[0, 3] = new SourceGrid2.Cells.Real.ColumnHeader("Dll");
			((SourceGrid2.Cells.Real.ColumnHeader)this[0, 3]).EnableSort = false;
			this[0, 4] = new SourceGrid2.Cells.Real.ColumnHeader("Path");
			((SourceGrid2.Cells.Real.ColumnHeader)this[0, 4]).EnableSort = false;

			this[0] = "";

			this.Columns[0].AutoSizeMode = SourceGrid2.AutoSizeMode.MinimumSize;
			this.Columns[0].Width = 20;
			this.Columns[1].AutoSizeMode = SourceGrid2.AutoSizeMode.MinimumSize;
			this.Columns[1].Width = 20;
			this.Columns[2].AutoSizeMode = SourceGrid2.AutoSizeMode.MinimumSize;
			this.Columns[2].Width = 20;
			this.Columns[3].AutoSizeMode = SourceGrid2.AutoSizeMode.MinimumSize;
			this.Columns[3].Width = 150;
			this.Columns[4].AutoSizeMode = SourceGrid2.AutoSizeMode.EnableStretch;
			this.AutoSize();
			this.AutoStretchColumnsToFitWidth = true;
			this.StretchColumnsToFitWidth();
			this.CustomSort = false;

			Reset();

			fileDialog = new OpenFileDialog();
			fileDialog.AddExtension = true;
			fileDialog.CheckFileExists = fileDialog.CheckPathExists = false;
			fileDialog.DefaultExt = "dll";
			fileDialog.Filter = "dll's & exe (*.dll, *.exe)|*.dll;*.exe";
			fileDialog.Multiselect = false;
			fileDialog.Title = "Select dll library";
		}

		string this[int i, bool packaged] {
			get { return Imports[i]; }
			set {
				string path;
				if (Path.IsPathRooted(value)) path = value;
				else {
					packaged = false;
					path = "";
				}

				if (i+1 < this.RowsCount) {
					string text;
					if (i == Rows.Count - 2) text = "*";
					else text = i.ToString();
					this[i+1, 0] = new RowHeader(text); // add the row number

					// set the first cell to the glass icon
					this[i+1, 1] = new Link(null);
					SourceGrid2.VisualModels.Common vm = new SourceGrid2.VisualModels.Common();
					vm.Image = glassImg;
					vm.ImageAlignment = SourceLibrary.Drawing.ContentAlignment.MiddleCenter;
					this[i+1, 1].VisualModel = vm;
					this[i+1, 1].Behaviors.Add(new BehaviorEdit());

					// set the second cell to the delete icon
					this[i+1, 2] = new Link(null);
					vm = new SourceGrid2.VisualModels.Common();
					vm.Image = deleteImg;
					vm.ImageAlignment = SourceLibrary.Drawing.ContentAlignment.MiddleCenter;
					this[i+1, 2].VisualModel = vm;
					this[i+1, 2].Behaviors.Add(new BehaviorDelete());

					this[i+1, 3] = new Cell(Path.GetFileName(value), typeof(string)); // add the dll name
					this[i+1, 3].Behaviors.Add(new BehaviorFile()); // add doubleclick functionality

					this[i+1, 4] = new Cell(path); // add the dll path
				} else {
					this[i+1, 3].Value = Path.GetFileName(value);
					this[i+1, 4].Value = path;
				}
		
				if (i+2 < Rows.Count) {
					Imports[i] = value;
				}
			}
		}

		string this[int i] {
			get { return Imports[i]; }
			set { this[i, false] = value; }
		}

		public void Add(string file) {
			Imports.Add(file);
			int n = Imports.Count;
			this.Rows.Insert(n);
			this[n - 1] = file;
		}

		public void Reset() {
			while (Rows.Count > 2) Rows.Remove(1);
			Imports = new List<string>(Compiler.Options.Imports);

			if (Imports.Count > 0) this.Rows.InsertRange(1, Imports.Count);
			for (int r = 0; r < Imports.Count; r++) {
				this[r] = Imports[r];
			}
			//this[Rows.Count - 2] = "";
		}

		public void Commit() {
			Compiler.Options.Imports = Imports;
			Compiler.Options.SortImports();
		}

		public void FileDialog(SourceGrid2.PositionEventArgs e) {
			if (e.Position.Row < Rows.Count - 1) fileDialog.FileName = Imports[e.Position.Row - 1];
			else fileDialog.FileName = "";

			bool global = !Path.IsPathRooted(fileDialog.FileName);

			string globalPath = Path.GetDirectoryName(typeof(object).Module.FullyQualifiedName);

			if (global) fileDialog.FileName = Path.Combine(globalPath, "");

			if (fileDialog.ShowDialog() == DialogResult.OK) {

				string file = fileDialog.FileName;
	
				if (file.Contains(globalPath)) file = Path.GetFileName(file);

				if (e.Position.Row == Rows.Count - 1) {
					Rows.Insert(Rows.Count - 1);
					Imports.Add(file);
				}
				this[e.Position.Row - 1] = file;
			}
		}

		public void CommitEdit(SourceGrid2.PositionEventArgs e) {
			Cell cell = (Cell)this[e.Position.Row, 3];
			string dll = cell.Value as string;
			if (dll != null && dll != "" && Path.GetExtension(dll) != ".dll" && Path.GetExtension(dll) != ".exe") dll += ".dll";
			if (e.Position.Row == Rows.Count - 1 && dll != null && dll != "" ) {
				cell.Value = null;
				Rows.Insert(Rows.Count - 1);
				Imports.Add(dll);
			}
			this[e.Position.Row - 1] = dll;
		}

		public IEnumerator<string> GetEnumerator() {
			return Imports.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return Imports.GetEnumerator();
		}

		private class BehaviorDelete: SourceGrid2.BehaviorModels.BehaviorModelGroup {
			
			public override void OnClick(SourceGrid2.PositionEventArgs e) {
				base.OnClick(e);
				ImportsGrid g = (ImportsGrid)e.Grid;

				int i = e.Position.Row - 1;
				if (i < g.Rows.Count - 2) {
					g.Imports.RemoveAt(i);
					g.Rows.Remove(e.Position.Row);
				}
			}
		}

		private class BehaviorEdit: SourceGrid2.BehaviorModels.BehaviorModelGroup {
			public override void OnClick(SourceGrid2.PositionEventArgs e) {
				base.OnClick(e);
				ImportsGrid g = (ImportsGrid)e.Grid;
				g.FileDialog(e);
			}
		}

		private class BehaviorFile:SourceGrid2.BehaviorModels.BehaviorModelGroup {
			/*public override bool CanReceiveFocus {
				get { return true; }
			}*/
			public override void OnDoubleClick(PositionEventArgs e) {
				base.OnDoubleClick(e);
				ImportsGrid g = (ImportsGrid)e.Grid;
				g.FileDialog(e);
			}

			public override void OnClick(PositionEventArgs e) {
				base.OnClick(e);
				e.Cell.StartEdit(e.Position, true);
			}

			public override void OnEditEnded(PositionCancelEventArgs e) {
				base.OnEditEnded(e);
				ImportsGrid g = (ImportsGrid)e.Grid;
				g.CommitEdit(e);
			}
		}

		private void InitializeComponent() {
			this.SuspendLayout();
			this.ResumeLayout(false);
		}
	}
}
