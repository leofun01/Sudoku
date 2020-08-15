using System;
using System.Collections.Generic;

namespace DotNetSudoku {
	public interface IOrderedSet<T> : IEnumerable<T>
		where T : IEquatable<T>, IComparable<T>
	{
		int Count { get; }
		bool Contains(T i);
	}
}
