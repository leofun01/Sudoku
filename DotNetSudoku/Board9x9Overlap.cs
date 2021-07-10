using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetSudoku {
	using B = Board9x9Overlap;
	using R = IOrderedSet<int>;
	using RC = ReadOnlyCollection<IOrderedSet<int>>;
	
	public class Board9x9Overlap : Board9x9
	{
		protected new const byte rcount = Board9x9.rcount + 4;
		
		protected override IList<R> InitRegions(int count) {
			IList<R> regions = base.InitRegions(count);
			regions.Add(new Square(1 * l + 1));
			regions.Add(new Square(1 * l + 5));
			regions.Add(new Square(5 * l + 1));
			regions.Add(new Square(5 * l + 5));
			return regions;
		}
		
		public Board9x9Overlap() : base(l * l, rcount) { }
		protected Board9x9Overlap(int cellsCount, int regionsCount) : base(cellsCount, regionsCount) { }
		protected Board9x9Overlap(int cellsCount, RC regions) : base(cellsCount, regions) { }
		protected Board9x9Overlap(B board) : base(board) { }
		
		public new B GetClone() { return (B)base.GetClone(); }
	}
}
