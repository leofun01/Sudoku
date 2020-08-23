using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DotNetSudoku {
	// using C = CellInt16;
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
		
		public static readonly C InvalidCell = (new C()).Not();
		
		public int CellsCount { get { return _cells.GetLength(0); } }
		// int IOrderedSet<int>.Count { get { return CellsCount; } }
		public bool? IsValid { get { return _valid; } }
		public virtual C this[int i] {
			[DebuggerStepThrough]
			get { return _cells[i]; }
			[DebuggerStepThrough]
			set {
				C ci = this[i];
				C ni = ci.And(value);
				if(ci.Equals(ni)) return;
				_cells[i] = ni;
				ni = this[i];
				if(ni.HasValue) {
					// Console.SetCursorPosition(0, 0);
					// System.Threading.Thread.Sleep(200);
					// // Console.Clear();
					// _cells[i] = InvalidCell;
					// Console.WriteLine(this);
					// _cells[i] = ni;
					if(ni.Equals(InvalidCell))
						_valid = false;
					else
						foreach(int a in GetAffected(i))
							if(i != a) this[a] = ni.Not();
				}
			}
		}
		
		private int _getUndefinedFirst() {
			int i = 0, count = CellsCount;
			for(; i < count && this[i].HasValue; ++i)
				if(this[i].Equals(InvalidCell)) _valid = false;
			if(i == count && !_valid.HasValue) _valid = true;
			return i;
		}
		private int _getUndefinedMinSet() {
			int i = 0, count = CellsCount;
			int minSet = int.MaxValue, minIdx = count;
			bool? valid = true;
			for(; i < count; ++i) {
				C c = this[i];
				if(c.HasValue) continue;
				if(c.Equals(InvalidCell)) {
					valid = false;
					continue;
				}
				int set = 0;
				foreach(int _ in (IEnumerable<int>)c) ++set;
				if(set > 1 && set < minSet) {
					minSet = set;
					minIdx = i;
					if(valid.HasValue && valid.Value)
						valid = null;
				}
			}
			_valid = valid;
			return minIdx;
		}
		public bool IsComplete() {
			return _getUndefinedFirst() >= CellsCount;
		}
		public R GetAffected(int i) {
			if(_affected[i] != null) return _affected[i];
			R a = OrderedSet<int>.Empty;
			foreach(R r in _regions)
				if(r.Contains(i))
					a = a.Union<int>(r);
			_affected[i] = a;
			return a;
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
