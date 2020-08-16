using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetSudoku {
	using R = IOrderedSet<int>;
	using RC = ReadOnlyCollection<IOrderedSet<int>>;
	
	public interface IBoard<C> : IEnumerable<C>
		where C : ICell<C>, new()
	{
		RC Regions { get; }
		C EmptyCell { get; }
		int CellsCount { get; }
		C this[int i] { get; set; }
		IBoard<C> GetClone();
	}
}
