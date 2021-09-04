using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DotNetSudoku {
	public abstract class OrderedSet<T> : IOrderedSet<T>
		where T : IEquatable<T>, IComparable<T>
	{
		public abstract int Count { get; }
		public virtual bool Contains(T item) {
			foreach(T t in this)
				if(ReferenceEquals(t, item) ||
					!ReferenceEquals(t, null) && t.Equals(item)
				) return true;
			return false;
		}
		public abstract IEnumerator<T> GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
		
		public static readonly OrderedSet<T> Empty = new EmptySet();
		
		private sealed class EmptySet : OrderedSet<T> {
			internal EmptySet() { }
			public override int Count { get { return 0; } }
			public override bool Contains(T item) { return false; }
			public override IEnumerator<T> GetEnumerator() { yield break; }
		}
	}
}
