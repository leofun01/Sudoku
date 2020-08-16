using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace DotNetSudoku {
	using B = Board9x9;
	using C = CellInt16;
	using R = IOrderedSet<int>;
	using RC = ReadOnlyCollection<IOrderedSet<int>>;
	using RangeEx = ArgumentOutOfRangeException;
	
	public class Board9x9 : Board<C>
	{
		private byte _i;
		protected const byte w = 3, h = 3, l = w * h, cc = l * l;
		protected const byte rcount = l * 3;
		
		public override C EmptyCell { get { return 0; } }
		protected override IList<R> InitRegions(int count) {
			IList<R> regions = new List<R>(count);
			for(byte y = 0; y < l; ++y)
				regions.Add(new Row(y));
			for(byte x = 0; x < l; ++x)
				regions.Add(new Column(x));
			for(byte y = 0; y < l; y += h)
				for(byte x = 0; x < l; x += w)
					regions.Add(new Square(y * l + x));
			return regions;
		}
		
		public Board9x9() : base(cc, rcount) { }
		protected Board9x9(int cellsCount, int regionsCount) : base(cellsCount, regionsCount) { }
		protected Board9x9(int cellsCount, RC regions) : base(cellsCount, regions) { }
		protected Board9x9(B board) : base(board) { _i = board._i; }
		
		public static char CellToChar(C c) {
			if(!c.HasValue) return '_';
			/*//
			if(c == none) return 'x';
			C[] m = new C[4] {
				(C)1 | 3 | 5 | 7,
				(C)2 | 3 | 6 | 7,
				(C)4 | 5 | 6 | 7,
				(C)8
			};
			foreach(C mask in m)
				if((c & mask) != none && (c & ~mask) != none)
					return '_';
			//*/
			int i = c.ToInt();
			return i < 0 ? 'x' : (char)('1' + i);
		}
		public static C? CharToCell(char ch) {
			if('_' == ch || ch == '0') return 0;
			if('1' <= ch && ch <= '9') return ch - '1';
			if('x' == ch) return -1;
			return null;
		}
		
		[DebuggerStepThrough]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Add(C c) {
			if(_i >= cc) throw new InvalidOperationException();
			this[_i] = c;
			++_i;
		}
		[DebuggerStepThrough]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Add(char ch) {
			C? c = CharToCell(ch);
			if(c.HasValue) Add(c.Value);
		}
		[DebuggerStepThrough]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Add(string s) {
			int len = s.Length;
			for(int i = 0; i < len; ++i)
				Add(s[i]);
		}
		
		public virtual C this[int x, int y] {
			get { _validate(x, y); return base[y * l + x]; }
			set { _validate(x, y); base[y * l + x] = value; }
		}
		[DebuggerStepThrough]
		private void _validate(int x, int y) {
			if(x < 0 || l <= x) throw new RangeEx("x");
			if(y < 0 || l <= y) throw new RangeEx("y");
		}
		
		public new B GetClone() { return (B)base.GetClone(); }
		public override string ToString() {
			string newLine = "\r\n";
			StringBuilder sb = new StringBuilder("{");
			for(int y = 0, i = 0; i < l; y = i += h) {
				for(; y < h + i; ++y) {
					sb.Append(newLine);
					sb.Append(" ");
					for(int x = 0, j = 0; j < l; x = j += w) {
						C c = this[y * l + x];
						sb.Append("   ");
						sb.Append(CellToChar(c));
						for(++x; x < w + j; ++x) {
							c = this[y * l + x];
							sb.Append(", ");
							sb.Append(CellToChar(c));
						}
						sb.Append(",");
					}
				}
				sb.Append(newLine);
			}
			return sb.Append("}").ToString();
		}
		
		protected struct Row : R
		{
			public readonly byte Y;
			
			public Row(int y) {
				if(y < 0 || l <= y) throw new RangeEx();
				Y = (byte)y;
			}
			
			public int Count { get { return l; } }
			public bool Contains(int i) {
				return i >= 0 && i < cc && i / l == Y;
				// return Y * l <= i && i < (Y + 1) * l;
			}
			public IEnumerator<int> GetEnumerator() {
				for(int x = 0; x < l; ++x)
					yield return Y * l + x;
			}
			IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
		}
		protected struct Column : R
		{
			public readonly byte X;
			
			public Column(int x) {
				if(x < 0 || l <= x) throw new RangeEx();
				X = (byte)x;
			}
			
			public int Count { get { return l; } }
			public bool Contains(int i) {
				return i >= 0 && i < cc && i % l == X;
			}
			public IEnumerator<int> GetEnumerator() {
				for(int y = 0; y < l; ++y)
					yield return y * l + X;
			}
			IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
		}
		protected struct Square : R
		{
			public readonly byte Pos;
			
			public Square(int pos) {
				if(
					pos < 0 || cc <= pos ||
					pos / l > (w - 1) * h ||
					pos % l > w * (h - 1)
				)
					throw new RangeEx();
				Pos = (byte)pos;
			}
			
			public int X { get { return Pos % l; } }
			public int Y { get { return Pos / l; } }
			public int Count { get { return l; } }
			public bool Contains(int i) {
				int x = i % l, y = i / l;
				return i >= 0 && i < cc
					&& X <= x && x < X + w
					&& Y <= y && y < Y + h;
			}
			public IEnumerator<int> GetEnumerator() {
				for(int y = 0; y < h; ++y)
					for(int x = 0; x < w; ++x)
						yield return y * l + x + Pos;
			}
			IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
		}
	}
}
