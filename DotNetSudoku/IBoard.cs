using System.Collections.Generic;

namespace DotNetSudoku {
	using R = IOrderedSet<int>;

	public interface IBoard<C> : IEnumerable<C>
		where C : ICell<C>, new()
	{
		R Regions { get; }
		int CellsCount { get; }
		C this[int i] { get; set; }
		IBoard<C> GetClone();
	}
}
