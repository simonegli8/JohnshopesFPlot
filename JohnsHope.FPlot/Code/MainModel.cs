using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using JohnsHope.FPlot.Library;
using JohnsHope.FPlot.Properties;
using System.IO;

namespace JohnsHope.FPlot {
	public interface IEditForm {
		void Edit(SourceLocation loc);
	}

	public class MainModel { // The Items for the complete UI

		public Dictionary<Item, Form> EditForms = new Dictionary<Item, Form>();
			// All edit forms of the model's Items
		private ItemsGrid grid; // A grid containing all Items
		private ItemsModel items; // The Items containing all Items
		public Form MainForm;
		public ConsoleForm ConsoleForm;

		public MainModel(Form MainForm, ItemsGrid grid) {
			this.MainForm = MainForm;
			Items = new ItemsModel(Resources.Noname);
			Grid = grid;
			ConsoleForm = new ConsoleForm(this);
			Init();
		}
		
		public ItemsGrid Grid {
			get { return grid; }
			set { grid = value;
				grid.MainModel = this;
				grid.Items = items;
			}
		}

		public ItemsModel Items {
			get { return items; }
			set {
				items = value;
				if (grid != null) grid.Items = items;
				DefaultClass.Items = items;
			}
		}
 
		public Form NewEditForm(Item x) { // Create a new edit form for Item x
			Form f = null;
			if (EditForms.ContainsKey(x) && !EditForms[x].IsDisposed) f = EditForms[x];
			else if (x is JohnsHope.FPlot.Library.Library) f = new LibraryForm(this, x as JohnsHope.FPlot.Library.Library);
			else if (x is DataItem) f = new DataForm(this, x as DataItem);
			else if (x is Function1DItem || x is Function2DItem || x is FunctionColorItem)
				f = new FunctionForm(this, x as FunctionItem);
			if (f != null) EditForms[x] = f; 
			return f;
		}

		public void ResetEditForms() {
			Dictionary<Item, bool> b = new Dictionary<Item, bool>();
			foreach (Item x in EditForms.Keys) { b[x] = false; }
			foreach (Item x in Items) { b[x] = true; }
			foreach (Item x in b.Keys) {
				if (b[x] == false) { // Close and remove all edit Forms that are no longer in the Items.
					if (EditForms[x] != null) EditForms[x].Close();
					EditForms.Remove(x);
				}
			}
			foreach (Item x in Items) { // Create a new Form for all Items that don't have one yet.
				if (!EditForms.ContainsKey(x) || EditForms[x] == null) { EditForms[x] = NewEditForm(x); }
			}
		}

		private void ResetTitle() { // Reset the main Form's title
			if (MainForm != null) MainForm.Text = Resources.MainTitle + System.IO.Path.GetFileNameWithoutExtension(items.Filename);
		}

		private void Init() {
			ResetEditForms();
			ResetTitle();
			//grid.ResetGrid();
			PlotForm.OpenPlots(items);
		}

		public void Load(Stream s) { items.Combine(ItemsModel.Load(s)); Init(); }
		public void Load(string filename) { items.Combine(ItemsModel.Load(filename)); items.Filename = filename; Init(); }

		public void Save() { items.Save(); }
		public void Save(Stream s) { items.Save(s);	}
		public void Save(string filename) { items.Filename = filename; Save(); ResetTitle(); }

		public void Reset() { 
			if (Items != null) Items.Discard();
			Items.Filename = Resources.Noname;
			Init();
		}

		public void Reset(string filename) {
			if (Items != null) Items.Discard();
			Items.Filename = filename;
			Init();
		}

		public void Edit(Item x) {
			if (!EditForms.ContainsKey(x) || EditForms[x] == null || 
				EditForms[x].IsDisposed) EditForms[x] = NewEditForm(x);
			Form f = EditForms[x];
			f.Show();
			if (f.WindowState == FormWindowState.Minimized) f.WindowState = FormWindowState.Normal;
			f.BringToFront();
		}

		public void Edit(SourceLocation loc) {
			if (loc.Source is Item) {
				Item y = (Item)loc.Source;
				if (!EditForms.ContainsKey(y) || EditForms[y] == null || 
				EditForms[y].IsDisposed) EditForms[y] = NewEditForm(y);
				Form f = EditForms[y];
				f.Show();
				if (f.WindowState == FormWindowState.Minimized) f.WindowState = FormWindowState.Normal;
				f.BringToFront();
				if (f is IEditForm) ((IEditForm)f).Edit(loc);
			} else if (loc.Source is DataColumn) { Edit(((DataColumn)loc.Source).Parent);
			} else if (loc.Source is Command) {
				if (ConsoleForm.IsDisposed) ConsoleForm = new ConsoleForm(this);
				ConsoleForm.EditCommand(loc);
				ConsoleForm.Show();
				if (ConsoleForm.WindowState == FormWindowState.Minimized) ConsoleForm.WindowState = FormWindowState.Normal;
				ConsoleForm.BringToFront();
			}
		}

		public void EditStyle(Item x) {
			if (x is DataItem) DataLineStyleForm.New(this, (DataItem)x);
			else if (x is Function1DItem) FunctionLineStyleForm.New(this, (Function1DItem)x);
			else if (x is Function2DItem) {
				((Function2DItem)x).Gradient = GradientForm.ShowDialog();
				this.Items.Update(x);
			}
		}

	}
}
