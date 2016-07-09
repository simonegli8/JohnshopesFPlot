using System;
using System.Collections.Generic;
using System.Text;
using JohnsHope.FPlot.Library;
using SourceGrid2.Cells.Real;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace JohnsHope.FPlot {

	public class SourceLocation: Compiler.SourceLocation {
		
		public SourceLocation(Compiler.SourceLocation ss) {
			if (ss != null) {
				File = ss.File;
				Method = ss.Method;
				Source = ss.Source;
				Line = ss.Line;
				Column = ss.Column;
				Exception = ss.Exception;
			}
		}

		public override string ToString() {
			string s = "";
			if (Source != null) {
				if (Source is Item) s += ((Item)Source).Name + ": ";
				else if (Source is ICompilableClass) s += ((ICompilableClass)Source).ClassName + ": ";
			}
			if (Source == null || Source is ICompilableLibrary) s += Method + " ";
			if (Source != null) s += Properties.Resources.Line + " " + Line;
			return s;
		}
	}

	public class StackTrace: List<SourceLocation> {
	
		public StackTrace(Exception e) {
			Compiler.StackTrace st = new Compiler.StackTrace(e);
			foreach (Compiler.SourceLocation ss in st) Add(new SourceLocation(ss));		
		}

		public override string ToString() {
			string s = "";
			foreach (SourceLocation ss in this) {
				s += ss.ToString() + "\r\n";
			}
			return s;
		}

	}
	[ToolboxBitmap(typeof(DataGridView))]
	public class StackGrid: SourceGrid2.Grid {
		private StackTrace stack;
		private Image glassImg, glassDisabledImg;
		private MainModel model;
		private BehaviorEdit behaviorEdit;

		public StackGrid(MainModel Model) {
			model = Model;

			//get images from the global resource.
			glassImg = Properties.Resources.glass;
			glassDisabledImg = Properties.Resources.lupedisabled;
			behaviorEdit = new BehaviorEdit(model);
			ColumnsCount = 4;
			RowsCount = 1;
			this[0, 0] = new SourceGrid2.Cells.Real.ColumnHeader("");
			((SourceGrid2.Cells.Real.ColumnHeader)this[0, 0]).EnableSort = false;
			this[0, 1] = new SourceGrid2.Cells.Real.ColumnHeader(Properties.Resources.Item);
			((SourceGrid2.Cells.Real.ColumnHeader)this[0, 1]).EnableSort = false;
			this[0, 2] = new SourceGrid2.Cells.Real.ColumnHeader(Properties.Resources.Line);
			((SourceGrid2.Cells.Real.ColumnHeader)this[0, 2]).EnableSort = false;
			this[0, 3] = new SourceGrid2.Cells.Real.ColumnHeader(Properties.Resources.Method);
			((SourceGrid2.Cells.Real.ColumnHeader)this[0, 3]).EnableSort = false;

			this.Columns[0].AutoSizeMode = SourceGrid2.AutoSizeMode.MinimumSize;
			this.Columns[0].Width = 20;
			this.Columns[1].AutoSizeMode = SourceGrid2.AutoSizeMode.MinimumSize;
			this.Columns[1].Width = 120;
			this.Columns[2].AutoSizeMode = SourceGrid2.AutoSizeMode.MinimumSize;
			this.Columns[2].Width = 35;
			this.Columns[3].AutoSizeMode = SourceGrid2.AutoSizeMode.EnableAutoSize | SourceGrid2.AutoSizeMode.EnableStretch;
			this.Columns[3].Width = 50;
			this.AutoSize();
			this.AutoStretchColumnsToFitWidth = true;
			this.StretchColumnsToFitWidth();
			this.CustomSort = false;
		}

		StackGrid(): this(null) { }

		public MainModel Model {
			get { return model; }
			set { behaviorEdit.Model = model = value; }
		}

		public void Reset(Exception e) {
			stack = new StackTrace(e);
			if (this.Rows.Count > 1) this.Rows.RemoveRange(1, this.Rows.Count - 1);
			int row = 1;
			foreach (SourceLocation sl in stack) {
				this.Rows.Insert(row);
				// set the first cell to the glass icon
				this[row, 0] = new Link(null);
				SourceGrid2.VisualModels.Common vm = new SourceGrid2.VisualModels.Common();
				if (sl.Source != null) vm.Image = glassImg;
				else vm.Image = glassDisabledImg;
				vm.ImageAlignment = SourceLibrary.Drawing.ContentAlignment.MiddleCenter;
				this[row, 0].VisualModel = vm;
				if (sl.Source != null) {
					this[row, 0].Behaviors.Add(behaviorEdit);
					this[row, 0].Tag = sl; // assign the Source to the Tag field.
				}
				this[row, 1] = new Cell(sl.Source);
				this[row, 2] = new Cell(sl.Line);
				this[row, 3] = new Cell(sl.Method);
				row++;
			}
		}

		private class BehaviorEdit:SourceGrid2.BehaviorModels.BehaviorModelGroup {
			public MainModel Model;

			public BehaviorEdit(MainModel Model) {
				this.Model = Model;
			}

			public override void OnClick(SourceGrid2.PositionEventArgs e) {
				base.OnClick(e);
				StackGrid g = (StackGrid)e.Grid;
				SourceLocation sl = (SourceLocation)(g[e.Position.Row, 0].Tag);
				Model.Edit(sl);
			}
		}
	}
}
