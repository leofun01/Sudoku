using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Permutation;
using DotNetTransformer.Math.Set;
using DotNetTransformer.Math.Transform;
using P = DotNetTransformer.Math.Permutation.PermutationInt32;

namespace DotNetSudoku {
	public static class Program {
		private const int ExpectedCount1 = 2016; // (8!/20)
		private static IEnumerable<string> StrEnum(string s, int length, int select, int step) {
			if(length > s.Length || select > length)
				throw new ArgumentOutOfRangeException();
			StringBuilder sb = new StringBuilder();
			P p0 = new P(), p = p0;
			do {
				for(int i = length - select; i < length; ++i)
					sb.Append(s[p[i]]);
				yield return sb.ToString();
				sb.Clear();
				for(int i = 0; i < step; ++i)
					p = p.GetNextPermutation(length);
			} while(p != p0);
		}
		private static int Count3x3_8sub() {
			List<P> l1 = new List<P>(ExpectedCount1);
			List<P> l2 = new List<P>(ExpectedCount1);
			P p0 = new P();
			{
				Predicate<P> cond1 = (P p) =>
					p[3] != 2 &&	p[0] != 0 &&	p[6] != 5 &&
					p[3] != 3 &&	p[0] != 1 &&	p[6] != 6 &&
					p[3] != 4 &&	p[1] != 0 &&	p[6] != 7 &&
					p[4] != 2 &&	p[1] != 1 &&	p[7] != 5 &&
					p[4] != 3 &&	p[2] != 0 &&	p[7] != 6 &&
					p[4] != 4 &&	p[2] != 1 &&	p[7] != 7 &&
					p[5] != 2 &&
					p[5] != 3 &&
					p[5] != 4 ;
				Predicate<P> cond2 = (P p) =>
					p[1] != 0 &&	p[0] != 2 &&	p[2] != 1 &&
					p[1] != 3 &&	p[0] != 5 &&	p[2] != 4 &&
					p[1] != 6 &&	p[3] != 2 &&	p[2] != 7 &&
					p[4] != 0 &&	p[3] != 5 &&	p[5] != 1 &&
					p[4] != 3 &&	p[6] != 2 &&	p[5] != 4 &&
					p[4] != 6 &&	p[6] != 5 &&	p[5] != 7 &&
					p[7] != 0 &&
					p[7] != 3 &&
					p[7] != 6 ;
				P p1 = p0, p2 = p0;
				do {
					if(cond1(p1)) l1.Add(p1);
					p1 = p1.GetNextPermutation(8);
				} while(p1 != p0);
				do {
					if(cond2(p2)) l2.Add(p2);
					p2 = p2.GetNextPermutation(8);
				} while(p2 != p0);
			}
			Console.WriteLine("Expected list size : {0}", ExpectedCount1);
			Console.WriteLine("Actual list 1 size : {0}", l1.Count);
			Console.WriteLine("Actual list 2 size : {0}", l2.Count);
			Func<P, P, P, bool> cond3 = (P p1, P p2, P p3) => {
				int p10 = p1[0], p20 = p2[0], p30 = p3[0],
					p11 = p1[1], p21 = p2[1], p31 = p3[1],
					p12 = p1[2], p22 = p2[2], p32 = p3[2],
					p13 = p1[3], p23 = p2[3], p33 = p3[3],
					p14 = p1[4], p24 = p2[4], p34 = p3[4],
					p15 = p1[5], p25 = p2[5], p35 = p3[5],
					p16 = p1[6], p26 = p2[6], p36 = p3[6],
					p17 = p1[7], p27 = p2[7], p37 = p3[7];
				return
					p30 != p20 &&	p33 != p23 &&	p35 != p26 &&
					p30 != p21 &&	p33 != p24 &&	p35 != p27 &&
					p30 != p22 &&	p33 != p25 &&	p36 != p26 &&
					p31 != p20 &&	p34 != p23 &&	p36 != p27 &&
					p31 != p21 &&	p34 != p24 &&	p37 != p26 &&
					p31 != p22 &&	p34 != p25 &&	p37 != p27 &&
					p32 != p20 &&
					p32 != p21 &&
					p32 != p22 &&
					p30 != p10 &&	p31 != p11 &&	p32 != p12 &&
					p30 != p13 &&	p31 != p14 &&	p32 != p15 &&
					p30 != p16 &&	p31 != p17 &&	p34 != p12 &&
					p33 != p10 &&	p36 != p11 &&	p34 != p15 &&
					p33 != p13 &&	p36 != p14 &&	p37 != p12 &&
					p33 != p16 &&	p36 != p17 &&	p37 != p15 &&
					p35 != p10 &&
					p35 != p13 &&
					p35 != p16 //
				;
			};
			int c1 = 0, c2 = 0, cs = 0;
			var dict = new Dictionary<P, SortedDictionary<int, int>>();
			string s0 = p0.ToString();
			//*/
			foreach(P p2 in l2) {
				string
					p2_sub1 = p2.ToString().Substring(0, 3),
					p2_sub2 = p2.ToString().Substring(3, 3),
					p2_sub3 = p2.ToString().Substring(6, 2);
				c2 = 0;
				var d2 = new SortedDictionary<int, int>();
				dict.Add(p2, d2);
				foreach(P p1 in l1) {
					c1 = 0;
					string s_top = s0, s_middle, s_bottom;
					foreach(char ch in p2_sub1)
						s_top = s_top.Replace(ch.ToString(), "");
					if(s_top.Length != 5) throw new Exception();
					foreach(string s1 in StrEnum(s_top, 5, 3, 2)) {
						{
							int s10 = s1[0] - '0',
								s11 = s1[1] - '0',
								s12 = s1[2] - '0';
							if(
								s10 == p1[0] || s11 == p1[1] || s12 == p1[2] ||
								s10 == p1[3] || s11 == p1[4] || s12 == p1[5] ||
								s10 == p1[6] || s11 == p1[7]
							) continue;
						}
						s_middle = s0;
						foreach(char ch in string.Concat(p2_sub2, s1))
							s_middle = s_middle.Replace(ch.ToString(), "");
						int lm = s_middle.Length, fm = 1;
						if(lm < 2 || 5 < lm) throw new Exception();
						for(int i = 2; i <= lm - 2; ++i) fm *= i;
						foreach(string s2 in StrEnum(s_middle, lm, 2, fm)) {
							{
								int s20 = s2[0] - '0',
									s21 = s2[1] - '0';
								if(
									s20 == p1[0] || s21 == p1[2] ||
									s20 == p1[3] || s21 == p1[5] ||
									s20 == p1[6]
								) continue;
							}
							s_bottom = s0;
							foreach(char ch in string.Concat(p2_sub3, s1, s2))
								s_bottom = s_bottom.Replace(ch.ToString(), "");
							if(s_bottom.Length > 3) throw new Exception();
							if(s_bottom.Length < 3) continue;
							foreach(string s3 in StrEnum(s_bottom, 3, 3, 1)) {
								{
									int s30 = s3[0] - '0',
										s31 = s3[1] - '0',
										s32 = s3[2] - '0';
									if(
										s30 == p1[0] || s31 == p1[1] || s32 == p1[2] ||
										s30 == p1[3] || s31 == p1[4] || s32 == p1[5] ||
										s30 == p1[6] || s31 == p1[7]
									) continue;
								}
								P p3 = s1 + s2 + s3;
								++cs; ++ c1; ++c2;
								// if(cond3(p1, p2, p3)) { ++cs; ++ c1; ++c2; }
								// else throw new Exception();
							}
						}
					}
					//
				//	{
				//		P p3 = p0;
				//		do {
				//			if(cond3(p1, p2, p3)) { ++cs; ++ c1; ++c2; }
				//			p3 = p3.GetNextPermutation(8);
				//		} while(p3 != p0);
				//	}
					// Console.WriteLine(" p1 = {1}\t c1 = {0}", c1, p1);
					if(c1 > 0) {
						if(d2.ContainsKey(c1)) ++d2[c1];
						else d2.Add(c1, 1);
					}
				}
				foreach(var pair in d2) {
					Console.WriteLine(" key = {0}, count = {1}", pair.Key, pair.Value);
				}
				Console.WriteLine(" p2 = {1}\t c2 = {0}\r\n", c2, p2);
			}
			/*/
			foreach(P p1 in l1) {
				c1 = 0;
				var d1 = new SortedDictionary<int, int>();
				dict.Add(p1, d1);
				foreach(P p2 in l2) {
					string
						p2_sub1 = p2.ToString().Substring(0, 3),
						p2_sub2 = p2.ToString().Substring(3, 3),
						p2_sub3 = p2.ToString().Substring(6, 2);
					c2 = 0;
					string s_top = s0, s_middle, s_bottom;
					foreach(char ch in p2_sub1)
						s_top = s_top.Replace(ch.ToString(), "");
					if(s_top.Length != 5) throw new Exception();
					foreach(string s1 in StrEnum(s_top, 5, 3, 2)) {
						{
							int s10 = s1[0] - '0',
								s11 = s1[1] - '0',
								s12 = s1[2] - '0';
							if(
								s10 == p1[0] || s11 == p1[1] || s12 == p1[2] ||
								s10 == p1[3] || s11 == p1[4] || s12 == p1[5] ||
								s10 == p1[6] || s11 == p1[7]
							) continue;
						}
						s_middle = s0;
						foreach(char ch in string.Concat(p2_sub2, s1))
							s_middle = s_middle.Replace(ch.ToString(), "");
						int lm = s_middle.Length, fm = 1;
						if(lm < 2 || 5 < lm) throw new Exception();
						for(int i = 2; i <= lm - 2; ++i) fm *= i;
						foreach(string s2 in StrEnum(s_middle, lm, 2, fm)) {
							{
								int s20 = s2[0] - '0',
									s21 = s2[1] - '0';
								if(
									s20 == p1[0] || s21 == p1[2] ||
									s20 == p1[3] || s21 == p1[5] ||
									s20 == p1[6]
								) continue;
							}
							s_bottom = s0;
							foreach(char ch in string.Concat(p2_sub3, s1, s2))
								s_bottom = s_bottom.Replace(ch.ToString(), "");
							if(s_bottom.Length > 3) throw new Exception();
							if(s_bottom.Length < 3) continue;
							foreach(string s3 in StrEnum(s_bottom, 3, 3, 1)) {
								{
									int s30 = s3[0] - '0',
										s31 = s3[1] - '0',
										s32 = s3[2] - '0';
									if(
										s30 == p1[0] || s31 == p1[1] || s32 == p1[2] ||
										s30 == p1[3] || s31 == p1[4] || s32 == p1[5] ||
										s30 == p1[6] || s31 == p1[7]
									) continue;
								}
								P p3 = s1 + s2 + s3;
								++cs; ++ c1; ++c2;
								// if(cond3(p1, p2, p3)) { ++cs; ++ c1; ++c2; }
								// else throw new Exception();
							}
						}
					}
					//
				//	{
				//		P p3 = p0;
				//		do {
				//			if(cond3(p1, p2, p3)) { ++cs; ++ c1; ++c2; }
				//			p3 = p3.GetNextPermutation(8);
				//		} while(p3 != p0);
				//	}
					// Console.WriteLine(" p2 = {1}\t c2 = {0}", c2, p2);
					if(c2 > 0) {
						if(d1.ContainsKey(c2)) ++d1[c2];
						else d1.Add(c2, 1);
					}
				}
				foreach(var pair in d1) {
					Console.WriteLine(" key = {0}, count = {1}", pair.Key, pair.Value);
				}
				Console.WriteLine(" p1 = {1}\t c1 = {0}\r\n", c1, p1);
			}
			//*/
			return cs;
		}
		public static void Main(string[] args) {
			{
				// Console.WriteLine("{0}", Count3x3_8sub());
				// Console.ReadLine();
				// return;
				//
				// FormTestSudoku f = new FormTestSudoku();
				// System.Windows.Forms.Application.Run(f);
				/*//
				Board9x9 b = new Board9x9() {
					"_ 2 _   6 _ _   _ 3 8",
					"_ _ _   _ _ 4   _ _ _",
					"_ _ _   _ 7 _   _ _ _",

					"7 _ _   _ _ 5   4 _ _",
					"_ 8 _   _ _ _   _ 2 _",
					"_ _ _   _ _ _   _ _ _",

					"4 _ _   _ _ _   1 _ _",
					"1 _ _   2 _ _   _ _ _",
					"_ _ _   3 8 _   _ _ _",
				};//*/
				Board9x9 b = new Board9x9() {
					(
						"8        " +
						"  36     " +
						" 7  9 2  " +
						" 5   7   " +
						"    457  " +
						"   1   3 " +
						"  1    68" +
						"  85   1 " +
						" 9    4  "
					).Replace(' ','_')
					/*//
					(
						"9  5 8   " +
						"         " +
						"64  97  5" +
						"8 64  59 " +
						"         " +
						"7      64" +
						" 65 4273 " +
						"  9 7  56" +
						" 8 6 54  "
					).Replace(' ','_')
					//*/
				//	"......6894..1..7...8...2...2..5..3...1...8.........251..1.9..36..4.1..72.32.76..."
				//	"123457689496183725587962143279541368315628497648739251751294836864315972932876514"
				//	.Replace('.','_')
				};
				//	Board9x9 b;
				int depth = -1;
				string s;
				//
				do {
					//	b = a.GetClone();
					b.TryToSolve(++depth);
					b.PrintLog(Console.Out, true);
					// Console.SetCursorPosition(0, 0);
					Console.WriteLine("Depth {1} : {0}", b, depth);
					if(b.IsValid.HasValue || b.IsComplete()) {
						if(b.IsValid.Value)
							Console.WriteLine("Solved. Depth: {0}", depth);
						else
							Console.WriteLine("This sudoku is invalid. Depth: {0}", depth);
						Console.ReadLine();
						break;
					}
					s = Console.ReadLine();
				} while(!b.IsValid.HasValue && s != "q");
				//	} while(!b.IsComplete() && s != "q");
				// f.Show();
				//
				// System.Threading.Thread.Sleep(1000);
				// Console.Clear();
				//
				// Console.WriteLine(b);
				// Console.WriteLine();
				// Console.ReadLine();
			}
			return;
			{
				CellInt16 c1 = ~new CellInt16();
				c1 |= 0;
				Console.WriteLine(" c1 = {0}", c1);
				Console.WriteLine();
				CellInt16 _ = new CellInt16();
				// _ = 8;
				// _ = Board9x9.CharToCell('9').Value;
				Board9x9 board = new Board9x9();
				Board9x9 board1 = new Board9x9() {
					"_ _ _   _ _ _   _ _ _",
					"_ _ _   _ _ _   _ _ _",
					"_ _ _   _ _ _   _ _ _",

					"_ _ _   _ _ _   _ _ _",
					"_ _ _   _ _ _   _ _ _",
					"_ _ _   _ _ _   _ _ _",

					"_ _ _   _ _ _   _ _ _",
					"_ _ _   _ _ _   _ _ _",
					"_ _ _   _ _ _   _ _ _",
				};
				Board9x9 b001 = new Board9x9() {
					"1 _ 7   _ 5 _   _ _ 3",
					"_ _ _   _ _ _   _ _ 9",
					"_ 5 _   9 _ 8   _ 6 _",

					"_ _ _   3 _ 9   _ _ _",
					"_ _ _   _ _ _   _ _ _",
					"_ 8 _   7 _ 6   _ 1 _",

					"_ 1 _   5 _ 7   _ 3 _",
					"7 _ _   _ _ _   _ _ 6",
					"2 _ 9   _ 6 _   4 _ 5",
				};
				Board9x9 b002 = new Board9x9() {
					"_ _ _   _ _ _   3 _ 2",
					"_ _ 4   6 _ _   _ _ _",
					"_ _ _   3 5 9   _ 1 _",

					"9 _ 6   _ _ _   1 _ _",
					"5 _ _   _ 4 _   _ _ 7",
					"_ _ 1   _ _ _   9 _ 8",

					"_ 8 _   1 7 5   _ _ _",
					"_ _ _   _ _ 8   7 _ _",
					"1 _ 3   _ _ _   _ _ _",
				};
				Board9x9 b003 = new Board9x9() {
					"_ _ _   3 _ _   6 _ _",
					"7 _ _   _ 4 _   _ _ _",
					"1 _ _   _ _ _   2 _ _",

					"_ _ _   _ _ _   _ 1 5",
					"_ _ _   8 _ 2   _ _ _",
					"_ _ _   4 _ _   _ 7 _",

					"_ 6 _   _ _ _   3 _ _",
					"5 _ _   _ 1 _   _ _ _",
					"_ 2 _   _ _ _   _ _ _",
				};
				Board9x9 b004_hard_4 = new Board9x9() {
					"_ _ _   3 _ _   6 _ _",
					"7 _ _   _ _ _   _ _ _",
					"1 _ _   _ _ _   2 _ _",

					"_ _ _   _ _ _   _ 1 5",
					"_ _ _   8 _ 2   _ _ _",
					"_ _ _   4 _ _   _ 7 _",

					"_ 6 _   _ _ _   3 _ _",
					"5 _ _   _ 1 _   _ _ _",
					"_ 2 _   _ _ _   _ _ _",
				};
				Board9x9 b005_hard_6 = new Board9x9() {
					"_ _ _   3 _ _   6 _ _",
					"7 _ _   _ 4 _   _ _ _",
					"_ _ _   _ _ _   2 _ _",

					"_ _ _   _ _ _   _ 1 5",
					"_ _ _   8 _ 2   _ _ _",
					"_ _ _   4 _ _   _ 7 _",

					"_ 6 _   _ _ _   3 _ _",
					"5 _ _   _ 1 _   _ _ _",
					"_ 2 _   _ _ _   _ _ _",
				};
				Board9x9 b006_hard_7 = new Board9x9() {
					"_ _ _   3 _ _   6 _ _",
					"7 _ _   _ 4 _   _ _ _",
					"1 _ _   _ _ _   2 _ _",

					"_ _ _   _ _ _   _ 1 _",
					"_ _ _   8 _ 2   _ _ _",
					"_ _ _   4 _ _   _ 7 _",

					"_ 6 _   _ _ _   3 _ _",
					"5 _ _   _ 1 _   _ _ _",
					"_ 2 _   _ _ _   _ _ _",
				};
				Board9x9 b007_hard_5 = new Board9x9() {
					"_ _ _   3 _ _   6 _ _",
					"7 _ _   _ 4 _   _ _ _",
					"1 _ _   _ _ _   2 _ _",

					"_ _ _   _ _ _   _ 1 5",
					"_ _ _   8 _ 2   _ _ _",
					"_ _ _   _ _ _   _ 7 _",

					"_ 6 _   _ _ _   3 _ _",
					"5 _ _   _ 1 _   _ _ _",
					"_ 2 _   _ _ _   _ _ _",
				};
				Board9x9 b008_hard_8 = new Board9x9() {
					"_ _ _   3 _ _   6 _ _",
					"7 _ _   _ 4 _   _ _ _",
					"1 _ _   _ _ _   2 _ _",

					"_ _ _   _ _ _   _ 1 5",
					"_ _ _   8 _ 2   _ _ _",
					"_ _ _   4 _ _   _ 7 _",

					"_ _ _   _ _ _   3 _ _",
					"5 _ _   _ 1 _   _ _ _",
					"_ 2 _   _ _ _   _ _ _",
				};
				Board9x9 b009_hard_4 = new Board9x9() {
					"_ _ _   3 _ _   6 _ _",
					"7 _ _   _ 4 _   _ _ _",
					"1 _ _   _ _ _   2 _ _",

					"_ _ _   _ _ _   _ 1 5",
					"_ _ _   8 _ 2   _ _ _",
					"_ _ _   4 _ _   _ 7 _",

					"_ 6 _   _ _ _   3 _ _",
					"_ _ _   _ 1 _   _ _ _",
					"_ 2 _   _ _ _   _ _ _",
				};
				Board9x9 b010_hard_7 = new Board9x9() {
					"_ _ _   3 _ _   6 _ _",
					"7 _ _   _ 4 _   _ _ _",
					"1 _ _   _ _ _   2 _ _",

					"_ _ _   _ _ _   _ 1 5",
					"_ _ _   8 _ 2   _ _ _",
					"_ _ _   4 _ _   _ 7 _",

					"_ 6 _   _ _ _   3 _ _",
					"5 _ _   _ 1 _   _ _ _",
					"_ _ _   _ _ _   _ _ _",
				};
				Board9x9 b011_hard_9 = new Board9x9() {
					"_ _ _   3 _ _   6 _ _",
					"7 _ _   _ 4 _   _ _ _",
					"1 _ _   _ _ _   2 _ _",

					"_ _ _   _ _ _   _ 1 5",
					"_ _ _   8 _ 2   _ _ _",
					"_ _ _   4 _ _   _ 7 _",

					"_ 6 _   _ _ _   3 _ _",
					"5 _ _   _ _ _   _ _ _",
					"_ 2 _   _ _ _   _ _ _",
				};
				Board9x9 board2 = new Board9x9() {
					'1', '2', '3',   '4', '5', '6',   '7', '8', '9',
					'1', '2', '3',   '4', '5', '6',   '7', '8', '9',
					'1', '2', '3',   '4', '5', '6',   '7', '8', '9',

					'1', '2', 'x',   '4', '5', '6',   '7', '8', '9',
					'1', '2', '3',   '4', 'x', '6',   '7', '8', '9',
					'1', '2', '3',   '4', '5', '6',   'x', '8', '9',

					'1', '2', '3',   '4', '5', '6',   '7', '8', '9',
					'1', '2', '3',   '4', '5', '6',   '7', '8', '9',
					'1', '2', '3',   '4', '5', '6',   '7', '8', '9',
				};
				Board9x9 board3 = new Board9x9() {
					0, 1, 2,   3, 4, 5,   6, 7, 8,
					3, 4, 5,   6, 7, 8,   0, 1, 2,
					6, 7, _,   0, 1, 2,   3, 4, 5,

					1, 2, 3,   4, 5, 6,   7, 8, 0,
					4, 5, 6,   7, _, 0,   1, 2, 3,
					7, 8, 0,   1, 2, 3,   4, 5, 6,

					2, 3, 4,   5, 6, 7,   _, 0, 1,
					5, 6, 7,   8, 0, 1,   2, 3, 4,
					8, 0, 1,   2, 3, 4,   5, 6, 7,
				};
				Console.WriteLine(" Board cells count = {0}", board.CellsCount);
				Console.WriteLine();
				int i = 0;
				foreach(IOrderedSet<int> r in board.Regions) {
					Console.WriteLine(
						" Region {0}, cells count = {1}, region type = {2}",
						i, r.Count, r.GetType()
					);
					Console.Write("{");
					foreach(int index in r) {
						Console.Write(" {0}", index);
					}
					Console.WriteLine(" }");
					++i;
				}
				Console.WriteLine();
				Console.WriteLine(board);
				Console.WriteLine();
				Console.WriteLine(board1);
				Console.WriteLine();
				Console.WriteLine(board2);
				Console.WriteLine();
				Console.WriteLine(board3);
				Console.WriteLine();
				Console.ReadLine();
			}
		}
	}
}
