using System;
using System.Collections.Generic;

namespace DotNetSudoku {
	public interface ICell<C> : IEquatable<C>
		, IEnumerable<int>
		// , IEnumerable<C>
		where C : ICell<C>, new()
	{
		bool this[int i] { get; set; }
		bool HasValue { get; }
		int ToInt();
		new IEnumerator<C> GetEnumerator();
		
		C Not();
		C And(C c);
		C Xor(C c);
		C Or(C c);
	}
}
