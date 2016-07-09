using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using JohnsHope.FPlot.Library;

namespace JohnsHope.FPlot
{
	//BUG entering text into double cell causes exception
	/// <summary>
	/// Summary description for ParForm.
	/// </summary>
	public class ParForm : System.Windows.Forms.Form, IItemEventHandler {
		private System.Windows.Forms.Button ok;
		private System.Windows.Forms.Button cancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private SourceGrid2.Grid grid;
		private FunctionItem item;
		private MainModel Model;


		public ParForm(MainModel Model) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.Model = Model;

			Model.Items.Handlers += this;
		}

		private static Dictionary<Item, ParForm> Forms = new Dictionary<Item, ParForm>();

		public static ParForm New(MainModel m, FunctionItem x) {

			// cleanup Forms
			List<Item> closedKeys = new List<Item>();
			foreach (Item y in Forms.Keys) {
				if (!Forms[y].Visible) closedKeys.Add(y);
			}
			foreach (Item y in closedKeys) Forms.Remove(y);

			ParForm f = null;
			Forms.TryGetValue(x, out f);
			if (f == null) f = new ParForm(m);
			f.Reset(x);
			Forms[x] = f;
			if (f.WindowState == FormWindowState.Minimized) f.WindowState = FormWindowState.Normal;
			f.Show();
			f.BringToFront();
			return f;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
				Model.Items.Handlers -= this;
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParForm));
			this.ok = new System.Windows.Forms.Button();
			this.cancel = new System.Windows.Forms.Button();
			this.grid = new SourceGrid2.Grid();
			this.SuspendLayout();
			// 
			// ok
			// 
			this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ok.Location = new System.Drawing.Point(8, 214);
			this.ok.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.ok.Name = "ok";
			this.ok.Size = new System.Drawing.Size(64, 22);
			this.ok.TabIndex = 1;
			this.ok.Text = "Ok";
			this.ok.Click += new System.EventHandler(this.okClick);
			// 
			// cancel
			// 
			this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancel.Location = new System.Drawing.Point(80, 214);
			this.cancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(64, 22);
			this.cancel.TabIndex = 3;
			this.cancel.Text = "Cancel";
			this.cancel.Click += new System.EventHandler(this.cancelClick);
			// 
			// grid
			// 
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.AutoSizeMinHeight = 10;
			this.grid.AutoSizeMinWidth = 10;
			this.grid.AutoStretchColumnsToFitWidth = false;
			this.grid.AutoStretchRowsToFitHeight = false;
			this.grid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.grid.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None;
			this.grid.CustomSort = false;
			this.grid.GridToolTipActive = true;
			this.grid.Location = new System.Drawing.Point(0, 0);
			this.grid.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(156, 210);
			this.grid.SpecialKeys = ((SourceGrid2.GridSpecialKeys)(((((((((((SourceGrid2.GridSpecialKeys.Ctrl_C | SourceGrid2.GridSpecialKeys.Ctrl_V) 
            | SourceGrid2.GridSpecialKeys.Ctrl_X) 
            | SourceGrid2.GridSpecialKeys.Delete) 
            | SourceGrid2.GridSpecialKeys.Arrows) 
            | SourceGrid2.GridSpecialKeys.Tab) 
            | SourceGrid2.GridSpecialKeys.PageDownUp) 
            | SourceGrid2.GridSpecialKeys.Enter) 
            | SourceGrid2.GridSpecialKeys.Escape) 
            | SourceGrid2.GridSpecialKeys.Control) 
            | SourceGrid2.GridSpecialKeys.Shift)));
			this.grid.TabIndex = 4;
			// 
			// ParForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(155, 239);
			this.Controls.Add(this.grid);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.ok);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Name = "ParForm";
			this.ShowInTaskbar = false;
			this.Text = "Function parameters";
			this.ResumeLayout(false);

		}
		#endregion

		public void Reset(FunctionItem item) {
			this.item = item;

			if (item != null) {
				grid.ColumnsCount = 2;
				grid.RowsCount = item.p.Length + 1;
				grid[0, 0] = new SourceGrid2.Cells.Real.Header("n");
				grid[0, 1] = new SourceGrid2.Cells.Real.ColumnHeader("p[n]");
				for (int r = 0; r < item.p.Length; r++) {
					grid[r+1, 0] = new SourceGrid2.Cells.Real.RowHeader(r);
					grid[r+1, 1] = new FpCell(item, r, Model);
				}
				grid.Columns[0].AutoSizeMode = SourceGrid2.AutoSizeMode.MinimumSize;
				grid.Columns[1].AutoSizeMode = SourceGrid2.AutoSizeMode.MinimumSize;
				grid.AutoSize();
			}
		}

		public void Apply() {
			for (int r = 0; r < item.p.Length; r++) {
				item.p[r] = (double)grid[r+1, 1].Value;
			}
			Model.Items.Update(item);
		}

		private void okClick(object sender, System.EventArgs e) {
			Apply();
			this.Visible = false;
		}

		private void cancelClick(object sender, System.EventArgs e) {
			this.Visible = false;
		}

		public void HandleUpdate(Item item) { if (item == this.item && item is FunctionItem) Reset((FunctionItem)item); }

		public void HandleAdd(Item item) { }

		public void HandleRemove(Item item) { }

		public void HandleReplace(Item oldItem, Item newItem) {
			if (oldItem == this.item && newItem is FunctionItem) Reset((FunctionItem)newItem);
		}

		public void HandleReorder(ItemList list) { }

		public void HandleInvalidate() { Reset(item); }

	}

	public class FpCell: SourceGrid2.Cells.Real.Cell {

		private FunctionItem f;
		private int i;
		private MainModel Model;


		public FpCell(FunctionItem f, int index, MainModel Model)
			: base(f.p[index], typeof(double)) {
			this.f = f;
			i = index;
			this.Model = Model;
		}

		public override void OnEditEnded(SourceGrid2.PositionCancelEventArgs e) {
			base.OnEditEnded(e);
			f.p[i] = (double)Value;
			Model.Items.Update(f);
		}
	}

}
