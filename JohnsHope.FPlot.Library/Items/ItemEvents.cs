using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Runtime.Serialization;

namespace JohnsHope.FPlot.Library {

	/// <summary>
	/// Describes an object that supports the specified notifications of Item changes
	/// in an <see cref="ItemList">ItemList</see>.
	/// </summary>
	public interface IItemEventHandler {
		/// <summary>
		/// The Item x has changed.
		/// </summary>
		void HandleUpdate(Item x);
		/// <summary>
		/// The Item x has been deleted.
		/// </summary>
		/// <param name="x"></param>
		void HandleRemove(Item x);
		/// <summary>
		/// The Item x was added.
		/// </summary>
		void HandleAdd(Item x);
		/// <summary>
		/// Replace the Item oldItem with Item newItem.
		/// </summary>
		void HandleReplace(Item oldItem, Item newItem);
		/// <summary>
		/// The order of the Items has changed
		/// </summary>
		void HandleReorder(ItemList order);
		/// <summary>
		/// Called when an arbitrary Model change has occured. All Items that have been modified have their Modified field set to true.
		/// </summary>
		void HandleInvalidate();
	}
	/// <summary>
	/// Represents a list of <see cref="WeakReference{T}"/>'s to <see cref="IItemEventHandler"/>s.
	/// New handlers can be added to the list with
	/// the += operator similar to events. Upon Serialization, all handlers that are Serializable are also serialized.
	/// If a handler is reclaimed by the garbage collector, it is removed from the list.
	/// </summary>
	[Serializable] //??? is this necessary?
	public class ItemEventHandlerList: IEnumerable<IItemEventHandler> {

		private class Enumerator: IEnumerator<IItemEventHandler> {
			private int i = -1;
			private List<WeakReference<IItemEventHandler>> Handlers;
			/// <summary>
			/// Initializes the Enumerator;
			/// </summary>
			/// <param name="Handlers"></param>
			public Enumerator(List<WeakReference<IItemEventHandler>> Handlers) {
				this.Handlers = Handlers;
			}
			/// <summary>
			/// Gets the current element
			/// </summary>
			public IItemEventHandler Current {
				get {
					if (i < Handlers.Count) {
						IItemEventHandler h = Handlers[i].Target;
						while (i < Handlers.Count && h == null) {
							Handlers.RemoveAt(i);
							h = Handlers[i].Target;
						}
						if (i < Handlers.Count) { return h; }
					}
					return null;
				}
			}

			object IEnumerator.Current {
				get { return this.Current; }
			}

			public bool MoveNext() { return ++i < Handlers.Count; }
			public void Reset() { i = -1; }

			public void Dispose() { }
		}

		[NonSerialized]
		private List<WeakReference<IItemEventHandler>> Handlers = new List<WeakReference<IItemEventHandler>>();
		private List<IItemEventHandler> s = new List<IItemEventHandler>(); // a temporary list that will be serialized.

		private void Cleanup() { // Removes deallocated IItemEventHandlers form the Handlers WeakReference list.
			for (int i = 0; i < Handlers.Count; i++) {
				if (!Handlers[i].IsAlive || Handlers[i].Target == null) {
					Handlers.RemoveAt(i--);;
				}
			}
		}
		/// <summary>
		/// Adds an <see cref="IItemEventHandler"/> to the ItemEventHandlerList
		/// </summary>
		public static ItemEventHandlerList operator+(ItemEventHandlerList list, IItemEventHandler h) {
			ItemEventHandlerList r = new ItemEventHandlerList();
			r.Handlers.AddRange(list.Handlers);
			r.Handlers.Add(new WeakReference<IItemEventHandler>(h));
			return r;
		}
		/// <summary>
		/// Removes an <see cref="IItemEventHandler"/> from the ItemEventHandlerList
		/// </summary>
		public static ItemEventHandlerList operator-(ItemEventHandlerList list, IItemEventHandler h) {
			ItemEventHandlerList r = new ItemEventHandlerList();
			foreach (WeakReference<IItemEventHandler> wr in list.Handlers) {
				if (wr.Target != h) r.Handlers.Add(wr);
			}
			return r;
		}
		[OnSerializing]
		private void OnSerialize(StreamingContext c) {
			Cleanup();
			s.Clear();
			foreach (WeakReference<IItemEventHandler> wr in Handlers) {
				IItemEventHandler r = wr.Target;
				if (r != null && r.GetType().IsSerializable) s.Add(r);
			}
		}
		[OnSerialized]
		private void OnSerialized(StreamingContext c) { s.Clear(); }

		[OnDeserializing]
		private void OnDeseriazing(StreamingContext c) {
			Handlers = new List<WeakReference<IItemEventHandler>>();
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext c) {
			foreach (IItemEventHandler r in s) { Handlers.Add(new WeakReference<IItemEventHandler>(r)); }
			s.Clear();
		}
		/// <summary>
		/// Returns an Enumerator for the ItemEventHandlerList
		/// </summary>
		public IEnumerator<IItemEventHandler> GetEnumerator() {
			return new Enumerator(Handlers);
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return new Enumerator(Handlers);
		}
		/// <summary>
		/// Calls the UpdateItem method of all registered IItemEventHandlers.
		/// </summary>
		public void Update(Item x) {
			for (int i = 0; i < Handlers.Count; i++) {
				WeakReference<IItemEventHandler> wr = Handlers[i];
				IItemEventHandler r = wr.Target;
				if (wr.IsAlive && r != null) r.HandleUpdate(x);
				else { Handlers.RemoveAt(i--); }
			}
		}
		/// <summary>
		/// Calls the AddItem method of all registered IItemEventHandlers.
		/// </summary>
		public void Add(Item x) {
			for (int i = 0; i < Handlers.Count; i++) {
				WeakReference<IItemEventHandler> wr = Handlers[i];
				IItemEventHandler r = wr.Target;
				if (wr.IsAlive && r != null) r.HandleAdd(x);
				else { Handlers.RemoveAt(i--); }
			}
		}
		/// <summary>
		/// Calls the RemoveItem method of all registered IItemEventHandlers.
		/// </summary>
		public void Remove(Item x) {
			for (int i = 0; i < Handlers.Count; i++) {
				WeakReference<IItemEventHandler> wr = Handlers[i];
				IItemEventHandler r = wr.Target;
				if (wr.IsAlive && r != null) r.HandleRemove(x);
				else { Handlers.RemoveAt(i--); }
			}
		}
		/// <summary>
		/// Calls the ReplaceItem method of all registered IItemEventHandlers.
		/// </summary>
		public void Replace(Item oldItem, Item newItem) {
			for (int i = 0; i < Handlers.Count; i++) {
				WeakReference<IItemEventHandler> wr = Handlers[i];
				IItemEventHandler r = wr.Target;
				if (wr.IsAlive && r != null) r.HandleReplace(oldItem, newItem);
				else { Handlers.RemoveAt(i--); }
			}
		}
		/// <summary>
		/// Calls the ReorderItems method of all registered IItemEventHandlers.
		/// </summary>
		public void Reorder(ItemList order) {
			for (int i = 0; i < Handlers.Count; i++) {
				WeakReference<IItemEventHandler> wr = Handlers[i];
				IItemEventHandler r = wr.Target;
				if (wr.IsAlive && r != null) r.HandleReorder(order);
				else { Handlers.RemoveAt(i--); }
			}
		}
		/// <summary>
		/// Calls the ReorderItems method of all registered IItemEventHandlers.
		/// </summary>
		public void Invalidate() {
			for (int i = 0; i < Handlers.Count; i++) {
				WeakReference<IItemEventHandler> wr = Handlers[i];
				IItemEventHandler r = wr.Target;
				if (wr.IsAlive && r != null) r.HandleInvalidate();
				else { Handlers.RemoveAt(i--); }
			}
		}

	}

}
