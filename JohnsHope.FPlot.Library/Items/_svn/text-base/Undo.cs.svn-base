using System.Diagnostics;
using System.Collections.Generic;

namespace JohnsHope.FPlot.Library {

	public class Undo: IItemEventHandler {

		class ItemCopy {
			public Item original, copy;

			public ItemCopy(Item x, int index) {
			}
		}

		class Action {
			Undo undo;

			public enum Type { Update, Add, Remove, Replace, Reorder }

			Item modified, original;
			int index;
			Dictionary<Item, int> order;
			Type type;

			public Action(Undo undo, Type type, params Item[] item) {
				this.undo = undo; 
				this.type = type;

				switch (type) {
				case Type.Update:
					original = undo.Copy[item[0]];
					undo.Snapshot();
					index = undo.Index[original];
					modified = item[0].Clone();
					undo.Copy[item[0]] = modified;
					undo.ReverseSnapshot();
					Debug.Assert(original != null && modified != null);
					break;
				case Type.Add:
					undo.Snapshot();
					modified = undo.Copy[item[0]];
					index = undo.Index[modified];
					Debug.Assert(modified != null);
					break;
				case Type.Remove:
					original = undo.Copy[item[0]];
					index = undo.Index[original];
					undo.Snapshot();
					Debug.Assert(original != null);
					break;
				case Type.Replace:
					original = undo.Copy[item[0]];
					index = undo.Index[original];
					undo.Snapshot();
					modified = undo.Copy[item[1]];
					Debug.Assert(original != null && modified != null);
					break;
				default: break;
				}
			
			}


			public void Undo() {
				undo.Current.Handlers -= undo;
				switch (type) {
				case Type.Update:
					undo.Original[original].CopyFrom(modified);
					break;
				case Type.Add:
					undo.Current.Remove(undo.Original[modified]);
					undo.Original.Remove(modified);
					break;
				case Type.Remove:
					//undo.Current.Insert(indexer, 
					break;
				}
			}

			public void Redo() {
			}

		}

		ItemList Current;
		Dictionary<Item, Item> Copy, Original;
		Dictionary<Item, int> Index;

		bool disabled;

		void Snapshot() {
			Order = new ItemList(Current);
			Dictionary<Item, Item> temp = new Dictionary<Item, Item>();
			foreach (Item x in Current) {
				Item y = Copy[x];
				if (y != null) temp[x] = Copy[x];
				else temp[x] = Clone(x);
			}
			Copy = temp;
		}

		public bool Disabled { get { return disabled; } set { disabled = value; } }

		public Undo(ItemList model) {
			model.Handlers += this;
		}

	}

}