using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace DotNetSudoku {
	using C = CellInt16;
	using IC = ICell<CellInt16>;
	
	[DebuggerDisplay("{ToString(),nq}, Bits = {_bits,h}")]
	[ImmutableObject(true)]
	[Serializable]
	public struct CellInt16 : IC
	{
		private short _bits;
		private CellInt16(int bits) { _bits = (short)bits; }
		
		private const byte _s = 16;
		
		public bool this[int i] {
			get { return (1 << i & _bits) == 0; }
			set {
				if(value)
					_bits &= (short)~(1 << i);
					// _bits &= (short)(1 << i ^ -1);
				else
					_bits |= (short)(1 << i);
			}
		}
		public bool HasValue {
			get {
				int b = -1 << _s | _bits;
				return (b + 1 | b) == -1;
			}
		}
		public int ToInt() {
			if(_bits == -1) return -1;
			int b = ~(-1 << _s | _bits);
			// int b = (-1 << _s | _bits) ^ -1;
			if((b - 1 & b) != 0)
				throw new InvalidOperationException();
			int i = 0;
			int s = _s;
			while(b > 1) {
				s >>= 1;
				if((-1 << s & b) != 0) {
					b >>= s;
					i += s;
				}
			}
			return i;
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
		IEnumerator<C> ICell<C>.GetEnumerator() {
			// foreach(C c in this) yield return c;
			for(int i = 0; i < _s; ++i)
				if(this[i]) yield return (C)i;
		}
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
		public bool Equals(C c) { return _bits == c._bits; }
		public override bool Equals(object o) { return o is C && Equals((C)o); }
		
		C IC.Not() { return new C(~_bits); }
		C IC.And(C c) { return new C(_bits | c._bits); }
		C IC.Xor(C c) { return new C(~(_bits ^ c._bits)); }
		C IC.Or(C c) { return new C(_bits & c._bits); }
		
		public static bool operator ==(C l, C r) { return l.Equals(r); }
		public static bool operator !=(C l, C r) { return !l.Equals(r); }
		
		public static C operator ~(C c) { return ((IC)c).Not(); }
		public static C operator &(C l, C r) { return ((IC)l).And(r); }
		public static C operator ^(C l, C r) { return ((IC)l).Xor(r); }
		public static C operator |(C l, C r) { return ((IC)l).Or(r); }
		
		public static implicit operator C(int value) {
			if(value == -1) return new C(-1);
			if((value & -_s) != 0)
				throw new ArgumentOutOfRangeException();
			return new C(~(1 << value));
			// return new C(1 << value ^ -1);
		}
	}
}
