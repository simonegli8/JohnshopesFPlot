using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using JohnsHope.FPlot.Library;
using JohnsHope.FPlot.Properties;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace JohnsHope.FPlot {


	[ToolboxBitmap(typeof(DataGridView))]
	[Browsable(false)]
	public class ItemsGrid: DataGridView, IItemEventHandler {

		public class ItemView {
			public Item Item;

			public ItemView(Item item) {
				Item = item;
			}

			public string Name {
				get {
					if (Item == null) return String.Empty;
					return Item.Name;
				}
				set {
					if (Item != null) Item.Name = value;
				}
			}

			public string Type {
				get {
					if (Item == null) return String.Empty;
					return Item.TypeName();
				}
			}

			public string Style { get { return ""; } }

		}

		public class ItemListView: IList<ItemView>, IItemEventHandler, IBindingList {

			ItemList Items;

			public ItemListView(ItemList items) {
				Items = items;
				Items.Handlers += this;
			}

			#region IList<ItemView> Members

			public int IndexOf(ItemView view) {
				return Items.IndexOf(view.Item);
			}

			public void Insert(int index, ItemView view) {
				Items.Insert(index, view.Item);
			}

			public void RemoveAt(int index) {
				Items.RemoveAt(index);
			}

			public ItemView this[int index] {
				get {
					return new ItemView(Items[index]);
				}
				set {
					Items[index] = value.Item;
				}
			}

			#endregion

			#region ICollection<ItemView> Members

			public void Add(ItemView view) {
				Items.Add(view.Item);
			}

			public void Clear() {
				Items.Clear();
			}

			public bool Contains(ItemView view) {
				return Items.Contains(view.Item);
			}

			public void CopyTo(ItemView[] array, int arrayIndex) {
				foreach (Item x in Items) {
					array[arrayIndex++] = new ItemView(x);
				}
			}

			public int Count {
				get { return Items.Count; }
			}

			public bool IsReadOnly {
				get { return false; }
			}

			public bool Remove(ItemView view) {
				return Items.Remove(view.Item);
			}

			#endregion

			#region IEnumerable<ItemView> Members

			public IEnumerator<ItemView> GetEnumerator() {
				foreach (Item x in Items) {
					yield return new ItemView(x);
				}
			}

			#endregion

			#region IEnumerable Members

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
				foreach (Item x in Items) {
					yield return new ItemView(x);
				}
			}

			#endregion

			#region IItemEventHandler Members

			public void HandleUpdate(Item x) {
				ListChanged(this, new ListChangedEventArgs(ListChangedType.Reset, 0)); 
			}

			public void HandleRemove(Item x) {
				ListChanged(this, new ListChangedEventArgs(ListChangedType.Reset, 0));
			}

			public void HandleAdd(Item x) {
				ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemAdded, Items.IndexOf(x)));
			}

			public void HandleReplace(Item oldItem, Item newItem) {
				ListChanged(this, new	ListChangedEventArgs(ListChangedType.Reset, 0));
			}

			public void HandleReorder(ItemList order) {
				ListChanged(this, new ListChangedEventArgs(ListChangedType.Reset, 0));
			}

			public void HandleInvalidate() {
				ListChanged(this, new ListChangedEventArgs(ListChangedType.Reset, 0));
			}

			#endregion

			#region IBindingList Members

			public void AddIndex(PropertyDescriptor property) {
				throw new NotImplementedException();
			}

			public object AddNew() {
				throw new NotImplementedException();
			}

			public bool AllowEdit {
				get { return true; }
			}

			public bool AllowNew {
				get { return false; }
			}

			public bool AllowRemove {
				get { return true; }
			}

			public void ApplySort(PropertyDescriptor property, ListSortDirection direction) {
				throw new NotImplementedException();
			}

			public int Find(PropertyDescriptor property, object key) {
				throw new NotImplementedException();	
			}

			public bool IsSorted {
				get { return true; }
			}

			public event ListChangedEventHandler ListChanged;

			public void RemoveIndex(PropertyDescriptor property) {
				throw new NotImplementedException();
			}

			public void RemoveSort() {
				throw new NotImplementedException();
			}

			public ListSortDirection SortDirection {
				get { throw new NotImplementedException(); }
			}

			public PropertyDescriptor SortProperty {
				get { throw new NotImplementedException(); }
			}

			public bool SupportsChangeNotification {
				get { return true; }
			}

			public bool SupportsSearching {
				get { return false; }
			}

			public bool SupportsSorting {
				get { return false; }
			}

			#endregion

			#region IList Members

			int IList.Add(object value) {
				Add((ItemView)value);
				return IndexOf((ItemView)value);
			}

			bool IList.Contains(object value) {
				return Contains((ItemView)value);
			}

			int IList.IndexOf(object value) {
				return IndexOf((ItemView)value);
			}

			void IList.Insert(int index, object value) {
				Insert(index, (ItemView)value);
			}

			bool IList.IsFixedSize {
				get { return false; }
			}

			void IList.Remove(object value) {
				Remove((ItemView)value);
			}

			object IList.this[int index] {
				get {
					return this[index];
				}
				set {
					this[index] = (ItemView)value;
				}
			}

			#endregion

			#region ICollection Members

			public void CopyTo(Array array, int index) {
				foreach (Item x in Items) array.SetValue(x, index++);
			}

			public bool IsSynchronized {
				get { throw new NotImplementedException(); }
			}

			public object SyncRoot {
				get { throw new NotImplementedException(); }
			}

			#endregion
		}

		ItemsModel items;

		public MainModel MainModel;

		[Browsable(false)]
		public ItemsModel Items { // Represents the Model of a ItemsGrid
			get { return items; }
			set {
				items = value;
				if (items != null) {
					items.Handlers += this;
					DataSource = new ItemListView(items);
				} else {
					DataSource = new ItemListView(new ItemList());
				}
			}
		}

		public ItemsGrid() {	
			Reset();
			this.ParentChanged += HandleParentChanged;
		}

		private void HandleParentChanged(object sender, EventArgs e) {
			Reset();
		}

		public void Reset() {

			// 
			// nameColumn
			// 
			DataGridViewTextBoxColumn nameColumn, typeColumn, styleColumn;

			nameColumn = new DataGridViewTextBoxColumn();
			nameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			nameColumn.DataPropertyName = "Name";
			nameColumn.HeaderText = Properties.Resources.Name;
			nameColumn.Resizable = DataGridViewTriState.False;
			nameColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
			nameColumn.Name = "Name";
			// 
			// typeColumn
			// 
			typeColumn = new DataGridViewTextBoxColumn();
			typeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			typeColumn.DataPropertyName = "Type";
			typeColumn.HeaderText = Properties.Resources.Type;
			typeColumn.ReadOnly = true;
			typeColumn.Resizable = DataGridViewTriState.False;
			typeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
			typeColumn.Name = "Type";
			typeColumn.Width = 80;

			styleColumn = new DataGridViewTextBoxColumn();
			styleColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			styleColumn.HeaderText = Properties.Resources.Style;
			styleColumn.Name = "Style";
			styleColumn.Resizable = DataGridViewTriState.False;
			styleColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
			styleColumn.Width = 40;
			styleColumn.ReadOnly = true;
			styleColumn.DataPropertyName = "Style";
			styleColumn.DefaultCellStyle.ForeColor = Color.Blue;
			
			// 
			// contextMenuStrip1
			// 
			ContextMenuStrip menu = new ContextMenuStrip();
			var editMenu = new ToolStripMenuItem(Properties.Resources.Edit, null, new EventHandler(MenuEditClick));
			editMenu.Font = new Font(editMenu.Font, FontStyle.Bold);
			menu.Items.Add(editMenu);
			menu.Items.Add(Properties.Resources.Rename, null, new EventHandler(MenuRenameClick));
			menu.Items.Add("-");
			menu.Items.Add(Properties.Resources.Cut, null, new EventHandler(MenuCutClick));
			menu.Items.Add(Properties.Resources.Copy, null, new EventHandler(MenuCopyClick));
			menu.Items.Add(Properties.Resources.Paste, null, new EventHandler(MenuPasteClick));

			/// Grid
			AllowUserToAddRows = false;
			AllowUserToResizeColumns = false;
			AllowUserToResizeRows = false;
			AutoGenerateColumns = false;
			BackgroundColor = System.Drawing.SystemColors.Control;
			base.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			base.Columns.Clear();
			base.Columns.Add(nameColumn);
			base.Columns.Add(styleColumn);
			base.Columns.Add(typeColumn);
			RowTemplate.Height = 18;
			RowHeadersVisible = false;
			RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			ShowCellToolTips = false;
			DefaultCellStyle.SelectionBackColor = Color.FromArgb(
				(SystemColors.Control.R + SystemColors.ControlDark.R)/2,
				(SystemColors.Control.G + SystemColors.ControlDark.G)/2,
				(SystemColors.Control.B + SystemColors.ControlDark.B)/2);
			DefaultCellStyle.SelectionForeColor = Color.Black;
			DefaultCellStyle.Padding = new Padding(0);
			ContextMenuStrip = menu;
			AllowDrop = true;
			EditMode = DataGridViewEditMode.EditProgrammatically;
			VirtualMode = true;

			ClearSelection();

			MouseDown -= HandleMouseDown;
			MouseMove -= HandleMouseMove;
			//DragDrop -= HandleDragDrop;
			DragOver -= HandleDragOver;
			Click -= HandleClick;
			DoubleClick -= HandleDoubleClick;
			KeyPress -= HandleKeyPress;
			CellEndEdit -= HandleEndEdit;
			CellParsing -= HandleParsing;
			CellPainting -= HandleCellPaint;

			MouseDown += HandleMouseDown;
			MouseMove += HandleMouseMove;
			//DragDrop += HandleDragDrop;
			DragOver += HandleDragOver;
			Click += HandleClick;
			DoubleClick += HandleDoubleClick;
			KeyPress += HandleKeyPress;
			CellEndEdit += HandleEndEdit;
			CellParsing += HandleParsing;
			CellPainting += HandleCellPaint;
		}

		Item this[int i] {
			get { return items[i]; }
			set { items[i] = value; }
		}

		public void HandleAdd(Item x) { }

		public void HandleRemove(Item x) { }

		public void HandleReplace(Item oldItem, Item newItem) { }

		public void HandleUpdate(Item x) { }

		public void HandleReorder(ItemList order) {  }

		public void HandleInvalidate() {  }

		public void GetSelection(ItemList items) {
			Item x;
			bool check, added = false;
			foreach (DataGridViewRow row in Rows) {
				check = row.Selected;
				x = ((ItemView)row.DataBoundItem).Item;
				if (check != items.Contains(x)) {
					if (check) {
						items.Add(x); added = true;
					} else items.Remove(x);
				}
			}
			if (added) items.Reorder(Items);
		}

		public void SetSelection(ItemList items) {
			foreach (DataGridViewRow row in Rows) {
				Item x = ((ItemView)row.DataBoundItem).Item;
				row.Selected = items.Contains(x);
			}
		}

		//private void EditClick(object sender, EventArgs e) {

		Rectangle dragBoxFromMouseDown;
		
		int initialRow, previousRow, initialColumn;
		bool initialSelected = false;
		MouseButtons buttons;

		private void HandleMouseMove(object sender, MouseEventArgs e) {
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
				// If the mouse moves outside the rectangle, start the drag.
				if (dragBoxFromMouseDown != Rectangle.Empty &&
					!dragBoxFromMouseDown.Contains(e.X, e.Y)) {

					ClearSelection();
					Rows[initialRow].Selected = true;

					// Cursor.Current = JohnsHope.FPlot.Util.SpecialCursors.PanCursor;

					// Proceed with the drag and drop, passing in the list item.                    
					DragDropEffects dropEffect = DoDragDrop(Rows[initialRow], DragDropEffects.Move);
				}
			}
		}

		private void HandleMouseDown(object sender, MouseEventArgs e) {
			// Get the index of the item the mouse is below.
			var info = HitTest(e.X, e.Y);

			initialRow = info.RowIndex;
			initialColumn = info.ColumnIndex;

			buttons = e.Button;

			if (initialRow != -1 && (e.Button & MouseButtons.Left) == MouseButtons.Left) {
				// Remember the point where the mouse down occurred. 
				// The DragSize indicates the size that the mouse can move 
				// before a drag event should be started.                
				Size dragSize = SystemInformation.DragSize;

				previousRow = initialRow;
				
				// Create a rectangle using the DragSize, with the mouse position being
				// at the center of the rectangle.
				dragBoxFromMouseDown = new Rectangle(
					new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);

				initialSelected = Rows[initialRow].Selected && 
					((ModifierKeys & (Keys.Control | Keys.Shift)) == Keys.None);
			} else {
				// Reset the rectangle if the mouse is not over an item in the ListBox.
				dragBoxFromMouseDown = Rectangle.Empty;
				initialSelected = false;
			}
		}

		private void HandleClick(object sender, EventArgs e) {
			if (initialRow >= 0 && initialRow < Rows.Count && (buttons & MouseButtons.Left) == MouseButtons.Left) {
				if (initialColumn >= 0 && initialColumn < Columns.Count && Columns[initialColumn].Name == "Name") {
					if (initialSelected) BeginEdit(true);
				}
			}
		}
		
		delegate void EditStyleHandler(Item item);

		private void HandleDoubleClick(object sender, EventArgs e) {
			if (initialRow >= 0 && initialRow < Rows.Count && (buttons & MouseButtons.Left) == MouseButtons.Left) {
				Item x = ((ItemView)Rows[initialRow].DataBoundItem).Item;
				if (x != null) {
					if (initialColumn >= 0 && initialColumn < Columns.Count && Columns[initialColumn].Name == "Name") {
						if (initialSelected) EndEdit();
					}
					if (initialColumn >= 0 && initialColumn < Columns.Count && Columns[initialColumn].Name == "Style") {
						MainModel.EditStyle(x);
					} else
						MainModel.Edit(x);
				}
			}
		}



		private void HandleDragOver(object sender, DragEventArgs e) {
			e.Effect = DragDropEffects.Move;
			e.Effect = DragDropEffects.Move | DragDropEffects.Scroll;

			// Cursor = JohnsHope.FPlot.Util.SpecialCursors.PanCursor;

			Point clientPoint = PointToClient(new Point(e.X, e.Y));
			HitTestInfo hit = HitTest(clientPoint.X, clientPoint.Y);
			int newRow = hit.RowIndex;
			if (hit.Type == DataGridViewHitTestType.Cell || hit.Type == DataGridViewHitTestType.RowHeader) {

				if (newRow != previousRow) {
					Console.WriteLine("previousRow: {0}; newRow: {1}", previousRow, newRow);
					if (newRow != -1) { // exchange items
						ClearSelection();
						Items.MoveTo(previousRow, newRow);
						Console.WriteLine("Exchanged {0} {1}", previousRow, newRow);
						previousRow = newRow;
						Rows[newRow].Selected = true;
					}
				}
			} else {
				e.Effect = DragDropEffects.None;
			}
		}

		private void HandleDragDrop(object sender, DragEventArgs e) {
			// Cursor = Cursors.Arrow;
		}

		private ItemList clipboard = new ItemList();

		private void MenuEditClick(object sender, EventArgs e) {
			if (initialRow != -1) {
				Item x = ((ItemView)Rows[initialRow].DataBoundItem).Item;
				if (x != null) MainModel.Edit(x);
			}
		}

		private void MenuCutClick(object sender, EventArgs e) {
			if (initialRow != -1) {
				clipboard.Clear();
				clipboard.Add(((ItemView)Rows[initialRow].DataBoundItem).Item);
				Items.RemoveAt(initialRow);
			}
		}

		private void MenuCopyClick(object sender, EventArgs e) {
			if (initialRow != -1) {
				clipboard.Clear();
				clipboard.Add(((ItemView)Rows[initialRow].DataBoundItem).Item);
			}
		}
		
		private void MenuPasteClick(object sender, EventArgs e) {
			if (initialRow != -1) {
				Items.InsertRange(initialRow, clipboard);
			} else {
				Items.InsertRange(Items.Count, clipboard);
			}
		}

		private void MenuRenameClick(object sender, EventArgs e) {
			if (initialRow != -1) {
				CurrentCell = Rows[initialRow].Cells[0];
				BeginEdit(true);
			}
		}

		public void Cut() {
			Copy();
			foreach (Item x in clipboard) Items.Remove(x);
		}

		public void Copy() {
			GetSelection(clipboard);
		}

		public void Paste() {
			int index;
			if (SelectedRows.Count == 1) index = SelectedRows[0].Index;
			else index = Items.Count;
			Items.InsertRange(index, clipboard);
		}

		private void HandleKeyPress(object sender, KeyPressEventArgs e) {
			if ((CurrentCell != null && CurrentCell.IsInEditMode) || (char.IsControl(e.KeyChar) && e.KeyChar != '\u000D')) return;
			if (SelectedRows.Count == 1)  initialRow = SelectedRows[0].Index;
			if (e.KeyChar == '\u000D') initialRow--;
			Console.WriteLine(((int)e.KeyChar).ToString());
			if (initialRow != -1 && SelectedRows.Count > 0) {
				CurrentCell = Rows[initialRow].Cells[0];
				if ((e.KeyChar == '\u0008' || e.KeyChar == '\u007F')  &&
					MessageBox.Show(Properties.Resources.DeleteQ, Properties.Resources.DeleteQTitle, MessageBoxButtons.YesNo) ==DialogResult.Yes) {
					items.RemoveAt(initialRow);
				}
			}
			if (CurrentCell != null) {
				CurrentCell = Rows[CurrentCell.RowIndex].Cells[0];
				BeginEdit(true);
			}
		}

		private void HandleParsing(object sender, DataGridViewCellParsingEventArgs e) {
			e.ParsingApplied = true;
		}

		private void HandleEndEdit(object sender, DataGridViewCellEventArgs e) {
			try {
				BeginInvoke(new EventHandler(ClearCurrentCell), this, EventArgs.Empty);
			} catch { }
		}

		private void ClearCurrentCell(object sender, EventArgs e) {
			CurrentCell = null;
		}

		// custom painting for the Style column
		// Paints the custom selection background for selected rows.
		void HandleCellPaint(object sender, DataGridViewCellPaintingEventArgs e) {

			if (e.RowIndex < 0 || Columns[e.ColumnIndex].Name != "Style") {
				e.Handled = false;
			} else {
				e.PaintBackground(e.ClipBounds, (e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected);

				Debug.Assert(e.RowIndex >= 0 && e.RowIndex <= items.Count, "Invalid RowIndex in HandleCellPaint");
				Item x = items[e.RowIndex];

				int m = (e.CellBounds.Top + e.CellBounds.Bottom) / 2;

				if (x is Function1DItem) {
					var f = (Function1DItem)x;
					Pen pen = new Pen(new SolidBrush(f.Color), 4);
					pen.DashStyle = f.LineStyle;
					e.Graphics.DrawLine(pen, new Point(e.CellBounds.Left+1, m), new Point(e.CellBounds.Right-2, m));
				} else if (x is DataItem) {
					var d = (DataItem)x;
					Pen pen = new Pen(new SolidBrush(d.Color), 4);
					pen.DashStyle = d.LineStyle;
					e.Graphics.DrawLine(pen, new Point(e.CellBounds.Left+1, m), new Point(e.CellBounds.Right-2, m));
				} else if (x is Function2DItem) {
					var f = (Function2DItem)x;
					GradientPainter.FillRectangle(e.Graphics, f.Gradient,
						new Rectangle(e.CellBounds.Left+1, e.CellBounds.Top+1, e.CellBounds.Width-3, e.CellBounds.Height-3),
						GradientPainter.Direction.Left);
				} else e.PaintContent(e.ClipBounds);


				e.Handled = true;
			}

		}

	}
}
