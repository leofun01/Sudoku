using System;

namespace DotNetSudoku {
	public interface ICell<C> : IEquatable<C>
		where C : ICell<C>, new()
	{
		bool this[int i] { get; set; }
		bool HasValue { get; }
	}
}
