using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JohnsHope.FPlot.Library {

	/// <summary>
	/// A serializable collection of Items that automatically compiles the Items upon deserialization.
	/// Registered IItemEventHandler will be notified of Item mutations. Each Item can be contained only once in the list.
	/// </summary>
	[Serializable]
	public class ItemList: List<Item>, IItemEventHandler {

		private class OrderComparison: IComparer<Item> {
			private ItemList order;

			public OrderComparison(ItemList order) {
				this.order = order;
			}

			public int Compare(Item x, Item y) {
				int n = order.IndexOf(x), m = order.IndexOf(y);
				if (n < m) return -1;
				else if (n > m) return 1;
				else return 0;
			}
		}

		/// <summary>
		/// A list of handlers that notify IItemEventHandler objects of mutations.
		/// </summary>
		public ItemEventHandlerList Handlers = new ItemEventHandlerList();

		private CompilerOptions Options;

		/// <summary>
		/// The empty constructor.
		/// </summary>
		public ItemList() : base() { }

		/// <summary>
		/// Initializes a new ItemList with a shallow copy from source.
		/// </summary>
		public ItemList(IEnumerable<Item> source) : base(source) { }

		// IItemEventHandler methods

		/// <summary>
		/// Handles the IItemEventHanlder UpdateItem call.
		/// </summary>
		public virtual void HandleUpdate(Item x) { Handlers.Update(x); }

		/// <summary>
		/// Handles the IItemEventHanlder AddItem call.
		/// </summary>

		public virtual void HandleAdd(Item x) { Handlers.Add(x); }
		/// <summary>
		/// Handles the IItemEventHanlder RemoveItem call.
		/// </summary>

		public virtual void HandleRemove(Item x) { Handlers.Remove(x); }
		/// <summary>
		/// Handles the IItemEventHanlder ReplaceItem call.
		/// </summary>

		public virtual void HandleReplace(Item oldItem, Item newItem) { Handlers.Replace(oldItem, newItem); }
		/// <summary>
		/// Handles the IItemEventHanlder ReorderItems call.
		/// </summary>

		public virtual void HandleReorder(ItemList order) { Handlers.Reorder(order); }

		/// <summary>
		/// Handles the IItemEventHanlder InvalidateModel call.
		/// </summary>
		public virtual void HandleInvalidate() { Handlers.Invalidate(); }


		/// <summary>
		/// This method handles updates. It is called by Update.
		/// </summary>
		protected virtual void BroadcastUpdate(Item x) { Handlers.Update(x); }

		/// <summary>
		/// This method handles Item additions. It is called by Add, AddRange, Insert and InsertRange
		/// </summary>
		protected virtual void BroadcastAdd(Item x) { Handlers.Add(x); }

		/// <summary>
		/// This method handles Item removals. It is called by Clear, Remove, RemoveAll, RemoveAt and RemoveRange
		/// </summary>
		protected virtual void BroadcastRemove(Item x) { Handlers.Remove(x); }

		/// <summary>
		/// This method handles Item replacements. It is called by the indexer and Replace
		/// </summary>
		protected virtual void BroadcastReplace(Item oldItem, Item newItem) { Handlers.Replace(oldItem, newItem); }

		/// <summary>
		/// This method handles Item reorderings. It is called by Reorder
		/// </summary>
		protected virtual void BroadcastReorder(ItemList order) { Handlers.Reorder(order); }

		/// <summary>
		/// This method handles Model Mutations.
		/// </summary>
		protected virtual void BroadcastInvalidate() { Handlers.Invalidate(); }

		/// <summary>
		/// The indexer of ItemList. Calls BroadcastReplace.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public new virtual Item this[int index] {
			get { return base[index]; }
			set {
				Item x = base[index];
				base[index] = value;
				if (x != value) {
					x.RaiseRemovedFromList(this);
					value.RaiseAddedToList(this);
				}
				BroadcastReplace(x, value);
			}
		}

		[NonSerialized]
		private Item namecache = null;

		/// <summary>
		/// The name indexer of ItemList (readonly).
		/// </summary>
		/// <param name="name">The name of the Item</param>
		public virtual Item this[string name] {
			get {
				if (namecache != null && namecache.Name == name) return namecache;
				foreach (Item x in this) {
					if (x.Name == name) {
						namecache = x;
						return x;
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Adds an Item to the list. Afterwards, BroadcastAdd is called.
		/// </summary>
		public new virtual void Add(Item x) {
			if (base.Contains(x)) throw new Exception("Duplicate Items in ItemList not allowed");
			base.Add(x);
			x.RaiseAddedToList(this);
			BroadcastAdd(x);
		}

		/// <summary>
		/// Adds an Item collection to the list using <see cref="Add"/>.
		/// </summary>
		public new virtual void AddRange(IEnumerable<Item> list) {
			foreach (Item x in list) Add(x);
		}

		/// <summary>
		/// Clears the list. Afterwards, BroadcastRemove(null) is called
		/// </summary>
		public new virtual void Clear() {
			while (base.Count > 0) {
				Item x = base[0];
				base.RemoveAt(0);
				x.RaiseRemovedFromList(this);
				BroadcastRemove(x);
			}
		}

		/// <summary>
		/// Inserts an Item into the list. Afterwards, BroadcastAdd is called.
		/// </summary>
		public new virtual void Insert(int index, Item x) {
			base.Insert(index, x);
			x.RaiseAddedToList(this);
			BroadcastAdd(x);
		}

		/// <summary>
		/// Inserts a collection of Items into the list. Afterwards, BroadcastAdd(null) is called.
		/// </summary>
		public new virtual void InsertRange(int index, IEnumerable<Item> list) {
			foreach (Item x in list) {
				base.Insert(index++, x);
				x.RaiseAddedToList(this);
				BroadcastAdd(x);
			}
		}

		/// <summary>
		/// Removes an Item from the list. Afterwards, BroadcastRemove is called.
		/// </summary>
		public new virtual bool Remove(Item x) {
			if (base.Remove(x)) {
				x.RaiseRemovedFromList(this);
				BroadcastRemove(x);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Removes Items that match the Predicate from the list. Afterwards, BroadcastRemove(null) is called.
		/// </summary>
		public new virtual int RemoveAll(Predicate<Item> match) {
			int n = 0;
			for (int i = 0; i < this.Count; i++) {
				if (match(base[i])) {
					Item x = base[i];
					base.RemoveAt(i); i--; n++;
					x.RaiseRemovedFromList(this);
					BroadcastRemove(x);
				}
			}
			return n;
		}

		/// <summary>
		/// Removes the Item at the specified index from the list. Afterwards, BroadcastRemove is called.
		/// </summary>
		public new virtual void RemoveAt(int index) {
			Item x = base[index];
			base.RemoveAt(index);
			x.RaiseRemovedFromList(this);
			BroadcastRemove(x);
		}

		/// <summary>
		/// Removes a specified range of Items from the list using <see cref="RemoveAt"/>.
		/// </summary>
		public new virtual void RemoveRange(int index, int count) {
			for (int n = 0; n < count; n++) RemoveAt(index);
		}

		/// <summary>
		/// Notifies installed handles about an Item modification. Calls BroadcastUpdate.
		/// </summary>
		/// <param name="x"></param>
		public virtual void Update(Item x) { BroadcastUpdate(x); }
		/// <summary>
		/// Replaces an old Item with a new one. Afterwards, BroadcastReplace is called.
		/// </summary>
		public virtual bool Replace(Item oldItem, Item newItem) {
			if (base.Contains(oldItem)) {
				if (oldItem != newItem && base.Contains(newItem)) throw new Exception("Duplicate Items in ItemList not allowed");
				base[base.IndexOf(oldItem)] = newItem;
				oldItem.RaiseRemovedFromList(this);
				newItem.RaiseAddedToList(this);
				BroadcastReplace(oldItem, newItem);
				return true;
			} else return false;
		}
		/// <summary>
		/// Reorders the ItemList accoring to order
		/// </summary>
		/// <param name="order"></param>
		public void Reorder(ItemList order) {
			base.Sort(new OrderComparison(order));
			BroadcastReorder(order);
		}

		/// <summary>
		/// Moves an item in the list to a new position
		/// </summary>
		/// <param name="src">The index of the item.</param>
		/// <param name="dest">The new index.</param>
		public void MoveTo(int src, int dest) {
			if (0 <= src && src < base.Count && 0 <= dest && dest <= base.Count) {
				Item x = base[src];
				base.RemoveAt(src);
				// if (dest >src) dest--;
				base.Insert(dest, x);
				BroadcastReorder(this);
			}
		}

		/*
		/// <summary>
		/// Exchanges the order of the two Items x and y
		/// </summary>
		public void ExchangeOrder(Item x, Item y) {
			int n = base.IndexOf(x), m = base.IndexOf(y);
			if (n >= 0 && m >= 0) {
				base[n] = y;
				base[m] = x;
				BroadcastReorder(this);
			}
		}*/

		/// <summary>
		/// Notifies the Handlers about a Model mutation.
		/// </summary>
		public virtual void Invalidate() {
			BroadcastInvalidate();
		}
		/// <summary>
		/// Copies all Items from another ItemList.
		/// </summary>
		public void CopyFrom(ItemList src) {
			Clear();
			AddRange(src);
		}
		/// <summary>
		/// Creates a shallow copy of the ItemList. Handlers are not copied.
		/// </summary>
		public ItemList Clone() {
			ItemList l = new ItemList();
			l.CopyFrom(this);
			return l;
		}
		[OnSerializing]
		private void Serialize(StreamingContext c) {
			Options = Compiler.Options;
		}
		[OnDeserialized]
		private void Init(StreamingContext c) {
			// Handlers = new ItemEventHandlerList();
			Compiler.Options.Combine(Options);

			// Set temporarily DefaultClass.Items to this
			ItemList temp = DefaultClass.Items;
			DefaultClass.Items = this;

			// compile libraries first
			foreach (Item x in this) {
				x.RaiseAddedToList(this);
				if (x is ICompilableLibrary) {
					x.TypeIndex = 0;
					Compiler.Add(x);
				}
			}
			foreach (Item x in this) {
				if (x is ICompilableLibrary && !x.Compiled) Compiler.Compile(x);
			}
			// compile others
			foreach (Item x in this) {
				if (!(x is ICompilableLibrary) && !x.Compiled) Compiler.Compile(x);
			}
			DefaultClass.Items = temp;

			Invalidate();
		}
		/// <summary>
		/// Returns true if the <c>ItemList</c> contains data and a function that can be fitted for.
		/// </summary>
		public bool IsFitable() {
			bool func = false, data = false;
			foreach (Item x in this) {
				func = func || (x is FunctionItem && ((FunctionItem)x).Fitable);
				data = data || (x is DataItem);
			}
			return func && data;
		}
		/// <summary>
		/// Returns true if the <c>ItemList</c> contains a function that can be evaluated
		/// </summary>
		public bool IsEvalable() {
			foreach (Item x in this) {
				if (x is Function1DItem || x is Function2DItem) return true;
			}
			return false;
		}
	}
}