using System;
using System.Collections.Generic;
using System.Drawing;
using JohnsHope.FPlot.Library;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace JohnsHope.FPlot.Library {

	/// <summary>
	/// Describes a PlotForm that can be visible, has bounds and contains a <see cref="PlotModel"/>
	/// </summary>
	public interface IPlotForm {
		/// <summary>
		/// Represents the bounds of a PlotFrom
		/// </summary>
		Rectangle Bounds { get; set; }
		/// <summary>
		/// Indicates if the PlotForm is visible
		/// </summary>
		bool Visible { get; set; }
		/// <summary>
		/// The <see cref="PlotModel"/> of the PlotForm
		/// </summary>
		PlotModel PlotModel { get; set; }
	}
	/// <summary>
	/// Describes a Model for an <see cref="ItemList"/>, that can be stored, loaded and combined with other <see cref="ItemsModel"/>s.
	/// This class serves as a global central repository of <see cref="Item">Items</see> and 
	/// <see cref="ItemsModel.Plot">Plots</see>
	/// </summary>
	[Serializable]
	public class ItemsModel: ItemList {
		/// <summary>
		/// A class that describes a PlotForm
		/// </summary>
		[Serializable]
		public class Plot {
			/// <summary>
			/// The bounds of the Plot
			/// </summary>
			public Rectangle Bounds;
			/// <summary>
			/// The Model of the Plot
			/// </summary>
			public PlotModel PlotModel;
			/// <summary>
			/// If New == true, the Plot has no open PlotForm yet
			/// </summary>
			[NonSerialized]
			public bool New = true;
			[NonSerialized]
			private WeakReference<IPlotForm> form = new WeakReference<IPlotForm>(null);

			[OnDeserialized]
			private void Init(StreamingContext sc) {
				New = true;
				form = new WeakReference<IPlotForm>(null);
			}
			/// <summary>
			/// Initializes the Plot
			/// </summary>
			public Plot(PlotModel m) {
				PlotModel = m;
			}
			/// <summary>
			/// Initializes the Plot
			/// </summary>
			public Plot(IPlotForm pf) {
				PlotModel = pf.PlotModel;
				Bounds = pf.Bounds;
				Form = pf;
			}
			/// <summary>
			/// The IPlotForm corresponding to this Plot
			/// </summary>
			public IPlotForm Form {
				get { return form.Target; }
				set { form.Target = value; }
			}
			/// <summary>
			/// Closes the PlotForm of the Plot.
			/// </summary>
			public void Close() {
				if (form.Target != null) form.Target.Visible = false;
			}
		}
		/// <summary>
		/// A class that describes a List of Plots
		/// </summary>
		[Serializable]
		public class PlotList: List<Plot> {
			/// <summary>
			/// Adds a new Plot with the given PlotModel to the PlotList
			/// </summary>
			public void New(PlotModel m) {
				Add(new Plot(m));
			}
		}
		/// <summary>
		/// All Plots contained in the ItemsModel
		/// </summary>
		public PlotList Plots = new PlotList();
		/// <summary>
		/// Saves the information about open PlotForms in the ItemsModel.
		/// </summary>
		public void SavePlots() {
			for (int i = 0; i < Plots.Count; i++) {
				IPlotForm f = Plots[i].Form;
				if (f == null || !f.Visible) {
					Plots.RemoveAt(i); i--;
				} else {
					Plots[i].Bounds = f.Bounds;
					Plots[i].PlotModel = f.PlotModel;
				}
			}
		}
		/// <summary>
		/// A delegate that opens a PlotForm for a given Plot.
		/// </summary>
		public delegate IPlotForm OpenPlotFormHandler(ItemsModel Model, Plot Plot);
		/// <summary>
		/// Opens a PlotForm for each Plot in the ItemsModel with the delegate OpenPlotForm.
		/// </summary>
		public void OpenPlots(OpenPlotFormHandler OpenPlotForm) {
			foreach (Plot p in Plots) {
				p.PlotModel.ItemsModel = this;
				if (p.New && OpenPlotForm != null) {
					p.Form = OpenPlotForm(this, p);
					p.New = false;
				}
			}
		}
		/// <summary>
		/// Closes all open Plots.
		/// </summary>
		public void ClosePlots() {
			foreach (Plot p in Plots) { p.Close(); }
		}
		/// <summary>
		/// The Filename the ItemsModel is stored with.
		/// </summary>
		public string Filename = "Noname.fplot";
		/// <summary>
		/// The default constructor
		/// </summary>
		public ItemsModel() {}
		/// <summary>
		/// Initializes the ItemsModel with the given filename
		/// </summary>
		public ItemsModel(string filename): this() { Filename = filename; }


		/// <summary>
		/// Combines the current with the given ItemsModel.
		/// </summary>
		public void Combine(ItemsModel m) {
			foreach (Item x in m) {
				if (x is Library) { // do not add duplicate or outdated Libraries
					bool add = true;
					Library l = (Library)x;
					string src = l.GetSource();
					string name = l.GetName();
					DateTime t = l.Version;
					for (int i = 0; i < this.Count; i++) {
						if (this[i] is Library) {
							Library yl = (Library)this[i];
							if (src == yl.GetSource()) { // The two Libraries have the same source.
								add = false;
								break;
							} else if (name == yl.GetName()) { // The two Libraries have the same name.
								if (t <= yl.Version) { // The new Library is outdated
									add = false;
									break;
								} else { // The new Library is newer than the existing Library.
									this.RemoveAt(i); i--;
									break;
								}
							}
						}
					}
					if (add) this.Add(x);
				} else this.Add(x);
			}

			foreach (Plot p in m.Plots) {
				Plots.Add(p);
				p.PlotModel.ItemsModel = this;
			}

		}
		/// <summary>
		/// Creates a ItemsModel that only contains the data needed for the given IPlotForm
		/// </summary>
		public ItemsModel Separate(IPlotForm form) { 
			ItemsModel m = new ItemsModel();
			ItemList list = new ItemList(form.PlotModel); // save a copy of the from.PlotModel
			foreach (Item x in this) { // Add all Libraries
				if (x is Library) {
					m.Add(x);
					list.Remove(x); // remove duplicates from the list
				}
			}
			m.AddRange(list); // Add all Items in the PlotModel
			m.Filename = form.PlotModel.Filename;
			Plot p = new Plot(form.PlotModel); // Add the Plot
			p.Bounds = form.Bounds;
			p.Form = form;
			m.Plots.Add(p);
			return m;
		}
		/// <summary>
		/// Deletes all Items in the ItemsModel and closes all Plots.
		/// </summary>
		public void Discard() {
			foreach (Plot p in Plots) p.Close();
			Plots.Clear();
			this.Clear();
		}
		/// <summary>
		/// Saves the ItemsModel
		/// </summary>
		public void Save() { Save(Filename); }
		/// <summary>
		/// Saves the ItemsModel in the specified file
		/// </summary>
		public void Save(string Filename) {
			this.Filename = Filename;
			using (FileStream stream = new FileStream(Filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
				Save(stream);
			}
		}
		/// <summary>
		/// Saves the ItemsModel in the specified Stream
		/// </summary>
		public void Save(Stream stream) {
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
			using (GZipStream zipstream = new GZipStream(stream, CompressionMode.Compress)) {
				SavePlots();
				formatter.Serialize(zipstream, this);
			}
		}
		/// <summary>
		/// Loads a ItemsModel from the specified Stream
		/// </summary>
		public static ItemsModel Load(Stream stream) {
			ItemsModel m;
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
			try {
				using (GZipStream zipstream = new GZipStream(stream, CompressionMode.Decompress)) {
					object o = formatter.Deserialize(zipstream);
					m = (ItemsModel)o;
				}
			} catch (Exception ex) {
				Console.WriteLine("exception: " + ex.Message);
				throw ex;
			}
			return m;
		}
		/// <summary>
		/// Loads a ItemsModel form the specified file
		/// </summary>
		public static ItemsModel Load(string Filename) {
			ItemsModel m = null;
			using (FileStream stream = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.None)) {
				m = Load(stream);
			}
			if (m != null) {
				m.Filename = Filename;
			}
			return m;
		}
		/// <summary>
		/// Registers an IPlotForm, so its contents will be stored along with the ItemsModel
		/// </summary>
		/// <param name="pf"></param>
		public void RegisterPlotForm(IPlotForm pf) {
			// Handlers += pf.PlotModel;
			foreach (Plot p in Plots) {
				if (p.PlotModel == pf.PlotModel) {
					p.New = false;
					return;
				}
			}
			Plot pnew = new Plot(pf);
			pnew.New = false;
			Plots.Add(pnew);
		}

	}


}
