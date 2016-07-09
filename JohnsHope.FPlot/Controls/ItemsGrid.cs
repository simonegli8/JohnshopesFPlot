using System;
using System.Collections.Generic;
using System.Text;
using SourceGrid2;
using SourceGrid2.Cells.Real;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using JohnsHope.FPlot.Library;
using JohnsHope.FPlot.Properties;

namespace JohnsHope.FPlot {
	[ToolboxBitmap(typeof(DataGridView))]
	public class ItemsGrid: Grid, IItemEventHandler {
		private ItemsModel items;
		private ItemList itemsOld;
		private BehaviorDelete behaviorDelete;
		private BehaviorEdit behaviorEdit;
		Image glassImg;
		Image deleteImg;
		Image parImg;
		Image parDisabledImg;
		Image upDownImg;
		public MainModel MainModel;

		private void ResetBuff() {
			if (items != null) {
				itemsOld = items.Clone();
				itemsOld.Handlers = new ItemEventHandlerList();
			} else itemsOld = null; 
		}

		public ItemsModel Items { // Represents the Model of a ItemsGrid
			get { return items; }
			set {
				items = value;
				if (items != null) items.Handlers += this;
				ResetBuff();
			}
		}

		public ItemsGrid() {

			//get images from the global resource.
			glassImg = Resources.glass;
			deleteImg = Resources.delete;
			parImg = Resources.par;
			parDisabledImg = Resources.pardisabled;
			upDownImg = Resources.updown;
			behaviorDelete = new BehaviorDelete();
			behaviorEdit = new BehaviorEdit();
			ColumnsCount = 7;
			RowsCount = 1;
			this[0, 0] = new SourceGrid2.Cells.Real.ColumnHeader("");
			((SourceGrid2.Cells.Real.ColumnHeader)this[0, 0]).EnableSort = false;
			this[0, 1] = new SourceGrid2.Cells.Real.ColumnHeader("");
			((SourceGrid2.Cells.Real.ColumnHeader)this[0, 1]).EnableSort = false;
			this[0, 2] = new SourceGrid2.Cells.Real.ColumnHeader("");
			((SourceGrid2.Cells.Real.ColumnHeader)this[0, 2]).EnableSort = false;
			this[0, 3] = new SourceGrid2.Cells.Real.ColumnHeader("");
			((SourceGrid2.Cells.Real.ColumnHeader)this[0, 3]).EnableSort = false;
			this[0, 4] = new SourceGrid2.Cells.Real.ColumnHeader("");
			((SourceGrid2.Cells.Real.ColumnHeader)this[0, 4]).EnableSort = false;
			this[0, 5] = new SourceGrid2.Cells.Real.ColumnHeader(Resources.Name);
			((SourceGrid2.Cells.Real.ColumnHeader)this[0, 5]).EnableSort = false;
			this[0, 6] = new SourceGrid2.Cells.Real.ColumnHeader(Resources.Type);
			((SourceGrid2.Cells.Real.ColumnHeader)this[0, 6]).EnableSort = false;

			this.Columns[0].AutoSizeMode = SourceGrid2.AutoSizeMode.MinimumSize;
			this.Columns[0].Width = 20;
			this.Columns[1].AutoSizeMode = SourceGrid2.AutoSizeMode.MinimumSize;
			this.Columns[1].Width = 20;
			this.Columns[2].AutoSizeMode = SourceGrid2.AutoSizeMode.MinimumSize;
			this.Columns[2].Width = 20;
			this.Columns[3].AutoSizeMode = SourceGrid2.AutoSizeMode.MinimumSize;
			this.Columns[3].Width = 20;
			this.Columns[4].AutoSizeMode = SourceGrid2.AutoSizeMode.MinimumSize;
			this.Columns[4].Width = 20;
			this.Columns[5].AutoSizeMode = SourceGrid2.AutoSizeMode.EnableAutoSize | SourceGrid2.AutoSizeMode.EnableStretch;
			this.Columns[6].AutoSizeMode = SourceGrid2.AutoSizeMode.None;
			this.Columns[6].Width = 80;
			this.AutoSize();
			this.AutoStretchColumnsToFitWidth = true;
			this.StretchColumnsToFitWidth();
			this.CustomSort = false;
		}

		Item this[int i] {
			get { return items[i]; }
			set {
				// set the first cell to the glass icon
				this[i+1, 0] = new Link(null);
				SourceGrid2.VisualModels.Common vm = new SourceGrid2.VisualModels.Common();
				vm.Image = glassImg;
				vm.ImageAlignment = SourceLibrary.Drawing.ContentAlignment.MiddleCenter;
				this[i+1, 0].VisualModel = vm;
				this[i+1, 0].Behaviors.Add(new BehaviorEdit());
				this[i+1, 0].Tag = items[i]; // assign the item to the Tag field.

				// set the second cell to the delete icon
				this[i+1, 1] = new Link(null);
				vm = new SourceGrid2.VisualModels.Common();
				vm.Image = deleteImg;
				vm.ImageAlignment = SourceLibrary.Drawing.ContentAlignment.MiddleCenter;
				this[i+1, 1].VisualModel = vm;
				this[i+1, 1].Behaviors.Add(new BehaviorDelete());
				this[i+1, 1].Tag = i; // assign the index to the Tag field.

				// set the third cell to the par icon
				vm = new SourceGrid2.VisualModels.Common();
				vm.ImageAlignment = SourceLibrary.Drawing.ContentAlignment.MiddleCenter;
				if (value is FunctionItem && ((FunctionItem)value).p.Count > 0) {
					this[i+1, 2] = new Link(null);
					vm.Image = parImg;
					this[i+1, 2].Behaviors.Add(new BehaviorPar());
					ParForm f = new ParForm(MainModel);
					f.Reset((FunctionItem)value);
					this[i+1, 2].Tag = f;
				} else {
					this[i+1, 2] = new SourceGrid2.Cells.Real.Cell();
					vm.Image = parDisabledImg;
				}
				this[i+1, 2].VisualModel = vm;

				// set the fourth cell to the updown icon
				if (i < items.Count) {
					this[i+1, 3] = new Link(null);
					vm = new SourceGrid2.VisualModels.Common();
					vm.Image = upDownImg;
					vm.ImageAlignment = SourceLibrary.Drawing.ContentAlignment.MiddleCenter;
					this[i+1, 3].Behaviors.Add(new BehaviorDown());
					this[i+1, 3].VisualModel = vm;
				} else {
					this[i+1, 3] = new Cell();
				}

				this[i+1, 4] = new SourceGrid2.Cells.Real.CheckBox(true); // add a checkbox
				this[i+1, 5] = new Cell(value.Name); // add the item name
				this[i+1, 5].Behaviors.Add(new BehaviorName()); // add doubleclick functionality
				this[i+1, 6] = new Cell(value.TypeName()); // add the item's typename

				// Set ItemsModel
				if (items[i] != value) items[i] = value;
				itemsOld[i] = value;
			}
		}

		public void ResetGrid() {
			if (this.Rows.Count > 1) this.Rows.RemoveRange(1, this.Rows.Count - 1);
			ResetBuff();
			for (int r = 0; r < Items.Count; r++) {
				this.Rows.Insert(r + 1);
				this[r] = Items[r];
			}
		}

		public void HandleAdd(Item x) {
			int i = items.IndexOf(x);
			this.Rows.Insert(i + 1);
			itemsOld.Insert(i, x);
			this[i] = x;
		}

		public void HandleRemove(Item x) {
			if (itemsOld.Contains(x)) {
				for (int i = 1; i < this.Rows.Count; i++) {
					if ((Item)this[i, 0].Tag == x) this.Rows.Remove(i);
				}
				itemsOld.Remove(x);
			}
		}

		public void HandleReplace(Item oldItem, Item newItem) {
			if (itemsOld.Contains(oldItem)) {
				for (int i = 1; i < this.Rows.Count; i++) {
					if ((Item)this[i, 0].Tag == oldItem) this[i-1] = newItem;
				}
			}
		}

		public void HandleUpdate(Item x) {
			if (items.Contains(x)) {
				for (int i = 1; i < this.Rows.Count; i++) {
					if ((Item)this[i, 0].Tag == x) this[i-1] = x;
				}
			}
		}

		public void HandleReorder(ItemList order) { ResetGrid(); }

		public void HandleInvalidate() { ResetGrid(); }

		public void GetSelection(ItemList items) {
			Item x;
			bool check, added = false;
			for (int i = 1; i < this.RowsCount; i++) {
				check = ((SourceGrid2.Cells.Real.CheckBox)this[i, 4]).Checked;
				x = (Item)this[i, 0].Tag;
				if (check != items.Contains(x)) {
					if (check) {
						items.Add(x); added = true;
					} else items.Remove(x);
				}
			}
			if (added) items.Reorder(Items);
		}

		public void SetSelection(ItemList plot) {
			for (int i = 1; i < Items.Count+1; i++) {
				Item x = (Item)this[i, 0].Tag;
				((SourceGrid2.Cells.Real.CheckBox)this[i, 4]).Checked = plot.Contains(x);
			}
		}

		private class BehaviorDelete: SourceGrid2.BehaviorModels.BehaviorModelGroup {
			
			public override void OnClick(SourceGrid2.PositionEventArgs e) {
				base.OnClick(e);
				ItemsGrid g = (ItemsGrid)e.Grid;

				if (MessageBox.Show(g, Properties.Resources.DeleteQ, Properties.Resources.DeleteQTitle, MessageBoxButtons.YesNo,
					MessageBoxIcon.Question) == DialogResult.Yes) {
					Item x = (Item)(g[e.Position.Row, 0].Tag);
					g.Items.Remove(x);
				}
			}
		}

		private class BehaviorEdit: SourceGrid2.BehaviorModels.BehaviorModelGroup {
			public override void OnClick(SourceGrid2.PositionEventArgs e) {
				base.OnClick(e);
				ItemsGrid g = (ItemsGrid)e.Grid;

				Item x = (Item)(g[e.Position.Row, 0].Tag);
				g.MainModel.Edit(x);
			}
		}

		private class BehaviorPar: SourceGrid2.BehaviorModels.BehaviorModelGroup {
			public override void OnClick(SourceGrid2.PositionEventArgs e) {
				base.OnClick(e);
				ItemsGrid g = (ItemsGrid)e.Grid;

				ParForm f = (g[e.Position.Row, 2].Tag) as ParForm;
				FunctionItem x = (FunctionItem)(g[e.Position.Row, 0].Tag);
				if (f != null) {
					if (f.IsDisposed) {
						f = new ParForm(((ItemsGrid)e.Grid).MainModel);
						f.Reset(x);
					}
					f.Show();
					f.BringToFront();
				}
			}
		}

		private class BehaviorDown:SourceGrid2.BehaviorModels.BehaviorModelGroup {
			public override void OnClick(SourceGrid2.PositionEventArgs e) {
				base.OnClick(e);
				ItemsGrid g = (ItemsGrid)e.Grid;

				Item x = (Item)(g[e.Position.Row, 0].Tag);
				int n = g.Items.IndexOf(x);
				if (n == g.Items.Count-1) n--;
				if (n >= 0 && n < g.Items.Count-1) {
					g.Items.ExchangeOrder(g.Items[n], g.Items[n+1]);
				}
			}
		}

		private class BehaviorName:SourceGrid2.BehaviorModels.BehaviorModelGroup {
			public override void OnDoubleClick(PositionEventArgs e) {
				base.OnDoubleClick(e);
				ItemsGrid g = (ItemsGrid)e.Grid;

				Item x = (Item)(g[e.Position.Row, 0].Tag);
				g.MainModel.Edit(x);
			}
		}
	}
}
