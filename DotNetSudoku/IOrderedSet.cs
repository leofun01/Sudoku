using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DotNetSudoku {
	[ImmutableObject(true)]
	[ReadOnly(true)]
	public interface IOrderedSet<T> : IEnumerable<T>
		// , IEquatable<IOrderedSet<T>>
		where T : IEquatable<T>, IComparable<T>
	{
		int Count { get; }
		bool Contains(T i);
	}
}
