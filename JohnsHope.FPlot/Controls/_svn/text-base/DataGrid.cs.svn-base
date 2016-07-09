using System;
using System.Windows.Forms;
using System.Drawing;
using System.CodeDom.Compiler;

using JohnsHope.FPlot.Library;

namespace JohnsHope.FPlot {

	[ToolboxBitmap(typeof(DataGridView))]
	public class DataGrid: SourceGrid2.GridVirtual {
		private SourceGrid2.Cells.Virtual.CellVirtual colHeaderCell, rowHeaderCell, x, y, z, dx, dy, dz, formulaCell;
		public DataItem data;

		public void SetDim() {
			int f;
			if (data.ErrorColumns) f = 2;
			else f = 1;
			Redim(data.Length + M + 1, 1 + data.Dimensions*f);
		}

		public virtual void LoadDataSource(DataItem data) {
			this.data = data;

			SetDim();

			//Col Header Cell Template
			colHeaderCell = new CellColumnHeaderTemplate(data);
			colHeaderCell.BindToGrid(this);

			//Row Header Cell Template
			rowHeaderCell = new CellRowHeaderTemplate();
			rowHeaderCell.BindToGrid(this);

			//Data Cell Templates
			x = new CellDataTemplate(data);
			x.BindToGrid(this);
			y = new CellDataTemplate(data);
			y.BindToGrid(this);
			z = new CellDataTemplate(data);
			z.BindToGrid(this);
			dx = new CellDataTemplate(data);
			dx.BindToGrid(this);
			dy = new CellDataTemplate(data);
			dy.BindToGrid(this);
			dz = new CellDataTemplate(data);
			dz.BindToGrid(this);

			//Formula Cell Template
			formulaCell = new CellFormulaTemplate(data);
			formulaCell.BindToGrid(this);

			RefreshCellStyle();
		}

		public override SourceGrid2.Cells.ICellVirtual GetCell(int row, int col) {
			try {
				if (data != null) {
					if (row < 1) return colHeaderCell;
					else if (col < 1) return rowHeaderCell;
					else if (row < 2) return formulaCell;
					else {
						switch (data.Index(col-1)) {
						case 0: return x;
						case 1: return y;
						case 2: return z;
						case 3: return dx;
						case 4: return dy;
						case 5: return dz;
						default: return null;
						}
					}
				}
				return null;
			} catch (Exception err) {
				System.Diagnostics.Debug.Assert(false, err.Message);
				return null;
			}
		}

		private void RefreshCellStyle() {
			x.DataModel.EnableEdit = data.x.Source == null;
			y.DataModel.EnableEdit = data.y.Source == null;
			dx.DataModel.EnableEdit = data.dx.Source == null;
			dy.DataModel.EnableEdit = data.dy.Source == null;
		}

		#region Cell class

		const int M = 2;

		public class CellDataTemplate: SourceGrid2.Cells.Virtual.CellVirtual {
			private DataItem data;
			public CellDataTemplate(DataItem data) {
				this.data = data;
				DataModel = SourceGrid2.Utility.CreateDataModel(typeof(double));
			}
			public override object GetValue(SourceGrid2.Position pos) {
				if (pos.Row == Grid.RowsCount-1) return null;
				else return data[pos.Column-1][pos.Row-M];
			}

			public override void SetValue(SourceGrid2.Position pos, object x) {
				if (pos.Row == Grid.RowsCount-1) {
					data.Length = Grid.RowsCount-M;
					Grid.RowsCount += 1;
				}
				data[pos.Column-1][pos.Row-M] = (double)x;
				OnValueChanged(new SourceGrid2.PositionEventArgs(pos, this));
			}
		}

		public class CellFormulaTemplate: SourceGrid2.Cells.Virtual.CellVirtual {
			private DataItem data;
			public CellFormulaTemplate(DataItem data) {
				this.data = data;
				DataModel = SourceGrid2.Utility.CreateDataModel(typeof(string));
			}

			bool EmptyString(string s) {
				return (s == null || s.Trim() == "");
			}

			string Source(string s) {
				if (s == null) return "";
				else return s;
			}

			public override object GetValue(SourceGrid2.Position pos) {
				return Source(data[pos.Column-1].Source);
			}

			private void Compile(DataColumn c) {
				Compiler.Compile(c);
				if (c.CompilerResults.Errors.HasErrors) {
					string msg = "There are errors in the formulas:\n";
					foreach (CompilerError err in c.CompilerResults.Errors) {
						msg += err.Column + ": " + err.ErrorText + "\n";
					}
					MessageBox.Show(msg, "Compile error");
				}
			}

			public override void SetValue(SourceGrid2.Position pos, object x) {
				string s = (string)x;
				DataColumn c = data[pos.Column-1];
				c.Source = s;
				Compile(c);
				((DataGrid)Grid).RefreshCellStyle();
				OnValueChanged(new SourceGrid2.PositionEventArgs(pos, this));
			}
		}

		private class CellColumnHeaderTemplate: SourceGrid2.Cells.Virtual.ColumnHeader {
			private DataItem data;
			public CellColumnHeaderTemplate(DataItem data) {
				this.data = data;
			}
			public override object GetValue(SourceGrid2.Position pos) {
				switch (data.Index(pos.Column-1)) {
				case -1: return "n";
				case 0: return "x";
				case 1: return "y";
				case 2: return "z";
				case 3: return "Δx";
				case 4: return "Δy";
				case 5: return "Δz";
				}
				return null;
			}
			public override void SetValue(SourceGrid2.Position p_Position, object p_Value) {
				throw new ApplicationException("Cannot change this kind of cell");
			}

			public override SourceGrid2.SortStatus GetSortStatus(SourceGrid2.Position p_Position) {
				return new SourceGrid2.SortStatus(SourceGrid2.GridSortMode.None, false);
			}

			public override void SetSortMode(SourceGrid2.Position p_Position, SourceGrid2.GridSortMode p_Mode) {
			}
		}

		private class CellRowHeaderTemplate: SourceGrid2.Cells.Virtual.RowHeader {
			public override object GetValue(SourceGrid2.Position pos) {
				if (pos.Row == 1) return "=";
				else if (pos.Row == Grid.RowsCount-1) return "*";
				else return pos.Row-2;
			}
			public override void SetValue(SourceGrid2.Position p_Position, object p_Value) {
				throw new ApplicationException("Cannot change this kind of cell");
			}
		}

		#endregion
	}
}