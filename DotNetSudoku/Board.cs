using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DotNetSudoku {
	using R = IOrderedSet<int>;
	using RC = ReadOnlyCollection<IOrderedSet<int>>;
	
	public abstract class Board<C> : IBoard<C>
		where C : ICell<C>, new()
	{
		private C[] _cells;
		private R[] _affected;
		private bool? _valid;
		private readonly RC _regions;
		public RC Regions { get { return _regions; } }
		
		public abstract C EmptyCell { get; }
		protected abstract IList<R> InitRegions(int count);
		
		protected Board(int cellsCount, int regionsCount) : this(cellsCount, null) {
			_regions = new RC(InitRegions(regionsCount));
		}
		protected Board(int cellsCount, RC regions) {
			_cells = new C[cellsCount];
			_affected = new R[cellsCount];
			_valid = null;
			for(int i = 0; i < cellsCount; ++i)
				_cells[i] = EmptyCell;
			_regions = regions;
		}
		protected Board(Board<C> board) : this(board.CellsCount, board._regions) {
			_valid = board._valid;
		}
		
		public int CellsCount { get { return _cells.GetLength(0); } }
		// int IOrderedSet<int>.Count { get { return CellsCount; } }
		public bool? IsValid { get { return _valid; } }
		public virtual C this[int i] {
			[DebuggerStepThrough]
			get { return _cells[i]; }
			[DebuggerStepThrough]
			set { _cells[i] = value; }
		}
		
		public Board<C> GetClone() {
			Board<C> clone = (Board<C>)MemberwiseClone();
			if(_cells != null) {
				int cc = CellsCount;
				clone._cells = new C[cc];
				Array.Copy(_cells, clone._cells, cc);
			}
			// if(_regions != null) clone._regions = _regions;
			return clone;
		}
		IBoard<C> IBoard<C>.GetClone() { return GetClone(); }
		public IEnumerator<C> GetEnumerator() {
			return ((IEnumerable<C>)_cells).GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
	}
}
