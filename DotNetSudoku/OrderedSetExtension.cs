using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace DotNetSudoku {
	public static class OrderedSetExtension {
		[ReadOnly(true)]
		private class InternalOrderedSet<T> : OrderedSet<T>
			where T : IEquatable<T>, IComparable<T>
		{
			private int? _count;
			private Func<int> _getCount;
			private Predicate<T> _contains;
			private readonly IEnumerable<T> _enumer;
			
			internal InternalOrderedSet(IEnumerable<T> enumer) {
				_validate(enumer);
				_enumer = enumer;
			}
			internal InternalOrderedSet(IEnumerable<T> enumer, Predicate<T> contains)
				: this(enumer) { _contains = contains; }
			internal InternalOrderedSet(IEnumerable<T> enumer, Predicate<T> contains, int count)
				: this(enumer, contains) { _count = count; }
			internal InternalOrderedSet(IEnumerable<T> enumer, Predicate<T> contains, Func<int> getCount)
				: this(enumer, contains) { _getCount = getCount; }
			
			public override int Count {
				get {
					if(!_count.HasValue) {
						int c = 0;
						if(_getCount != null) c = _getCount();
						else foreach(T _ in this) ++c;
						_count = c;
					}
					return _count.Value;
				}
			}
			public override bool Contains(T item) {
				if(_contains == null) _contains = base.Contains;
				return _contains(item);
			}
			public override IEnumerator<T> GetEnumerator() {
				foreach(T t in _enumer)
					if(_contains(t))
						yield return t;
			}
		}
		
		private static IEnumerable<T> _unionEnumerable<T>(IOrderedSet<T> r0, IOrderedSet<T> r1)
			where T : IEquatable<T>, IComparable<T>
		{
			var e0 = r0.GetEnumerator();
			var e1 = r1.GetEnumerator();
			bool m0 = e0.MoveNext();
			bool m1 = e1.MoveNext();
			while(m0 && m1) {
				T i0 = e0.Current;
				T i1 = e1.Current;
				bool lt = true;
				bool gt = true;
				if(!ReferenceEquals(i0, i1)) {
					bool nn = !ReferenceEquals(i0, null);
					lt = nn ? i0.CompareTo(i1) <= 0 : i1.CompareTo(i0) >= 0;
					gt = nn ? i0.CompareTo(i1) >= 0 : i1.CompareTo(i0) <= 0;
				}
				yield return lt ? i0 : i1;
				if(lt) m0 = e0.MoveNext();
				if(gt) m1 = e1.MoveNext();
			}
			if(m0) do yield return e0.Current; while(e0.MoveNext());
			if(m1) do yield return e1.Current; while(e1.MoveNext());
		}
		private static IOrderedSet<T> _intersect<T>(IOrderedSet<T> r0, IOrderedSet<T> r1)
			where T : IEquatable<T>, IComparable<T>
		{
			return new InternalOrderedSet<T>(
				_unionEnumerable(r0, r1),
				i => r0.Contains(i) && r1.Contains(i)
			);
		}
		public static IOrderedSet<T> Intersect<T>(this IOrderedSet<T> _this, IOrderedSet<T> other)
			where T : IEquatable<T>, IComparable<T>
		{
			if(ReferenceEquals(_this, other)) return _this;
			_validate(_this); _validate(other);
			return _intersect(_this, other);
		}
		public static IOrderedSet<T> Union<T>(this IOrderedSet<T> _this, IOrderedSet<T> other)
			where T : IEquatable<T>, IComparable<T>
		{
			if(ReferenceEquals(_this, other)) return _this;
			_validate(_this); _validate(other);
			return new InternalOrderedSet<T>(
				_unionEnumerable(_this, other),
				i => _this.Contains(i) || other.Contains(i),
				() => _this.Count + other.Count - _intersect(_this, other).Count
			);
		}
		public static IOrderedSet<T> SymmetricExcept<T>(this IOrderedSet<T> _this, IOrderedSet<T> other)
			where T : IEquatable<T>, IComparable<T>
		{
			if(ReferenceEquals(_this, other)) return OrderedSet<T>.Empty;
			_validate(_this); _validate(other);
			return new InternalOrderedSet<T>(
				_unionEnumerable(_this, other),
				i => _this.Contains(i) ^ other.Contains(i),
				() => _this.Count + other.Count - _intersect(_this, other).Count * 2
			);
		}
		public static IOrderedSet<T> Except<T>(this IOrderedSet<T> _this, IOrderedSet<T> other)
			where T : IEquatable<T>, IComparable<T>
		{
			if(ReferenceEquals(_this, other)) return OrderedSet<T>.Empty;
			_validate(_this); _validate(other);
			return new InternalOrderedSet<T>(
				_unionEnumerable(_this, other),
				i => _this.Contains(i) && !other.Contains(i),
				() => _this.Count - _intersect(_this, other).Count
			);
		}
		
		public static bool _exist<T>(IOrderedSet<T> _this, Predicate<T> match)
			where T : IEquatable<T>, IComparable<T>
		{
			foreach(T t in _this)
				if(match(t)) return true;
			return false;
		}
		public static bool Exist<T>(this IOrderedSet<T> _this, Predicate<T> match)
			where T : IEquatable<T>, IComparable<T>
		{
			_validate(_this); _validate(match);
			return _exist<T>(_this, match);
		}
		public static bool All<T>(this IOrderedSet<T> _this, Predicate<T> match)
			where T : IEquatable<T>, IComparable<T>
		{
			_validate(_this); _validate(match);
			return !_exist<T>(_this, t => !match(t));
		}
		
		[DebuggerStepThrough]
		private static void _validate(object o) {
			if(ReferenceEquals(o, null))
				throw new ArgumentNullException();
		}
	}
}
