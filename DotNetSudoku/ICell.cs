using System;
using System.Collections.Generic;

namespace DotNetSudoku {
	public interface ICell<C> : IEquatable<C>
		where C : ICell<C>, new()
	{
		bool this[int i] { get; set; }
		bool HasValue { get; }
		int ToInt();
		
		C Not();
		C And(C c);
		C Xor(C c);
		C Or(C c);
	}
}
