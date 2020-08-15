using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetSudoku {
	using C = CellInt32;
	using IC = ICell<CellInt32>;
	
	public struct CellInt32 : IC
	{
		private int _bits;
		private CellInt32(int bits) { _bits = bits; }
		
		private const byte _s = 32;
		
		public bool this[int i] {
			get { return (1 << i & _bits) == 0; }
			set {
				if(value)
					_bits &= ~(1 << i);
					// _bits &= 1 << i ^ -1;
				else
					_bits |= 1 << i;
			}
		}
		public bool HasValue {
			get { return (_bits + 1 | _bits) == -1; }
		}
		public override string ToString() {
			StringBuilder sb = new StringBuilder("{");
			IEnumerator<int> e = GetEnumerator();
			if(e.MoveNext()) {
				sb.Append(" ");
				sb.Append(e.Current);
			}
			while(e.MoveNext()) {
				sb.Append(", ");
				sb.Append(e.Current);
			}
			return sb.Append(" }").ToString();
		}
		public override int GetHashCode() { return _bits; }
		public IEnumerator<int> GetEnumerator() {
			for(int i = 0; i < _s; ++i)
				if(this[i]) yield return i;
		}
		public bool Equals(C c) { return _bits == c._bits; }
		public override bool Equals(object o) { return o is C && Equals((C)o); }
		
		public static bool operator ==(C l, C r) { return l.Equals(r); }
		public static bool operator !=(C l, C r) { return !l.Equals(r); }
		
		public static implicit operator C(int value) {
			if(value == -1) return new C(-1);
			if((value & -_s) != 0)
				throw new ArgumentOutOfRangeException();
			return new C(~(1 << value));
			// return new C(1 << value ^ -1);
		}
	}
}
