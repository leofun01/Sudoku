using System;

namespace DotNetSudoku {
	public static class Program {
		public static void Main(string[] args) {
			Console.WriteLine("Started.");
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
			};
			int depth = -1;
			string s;
			do {
				// b = a.GetClone();
				b.TryToSolve(++depth);
				b.PrintLog(Console.Out, true);
				// Console.SetCursorPosition(0, 0);
				Console.WriteLine("Depth {1} : {0}", b, depth);
				if(b.IsValid.HasValue || b.IsComplete()) {
					if(b.IsValid.Value) {
						Console.WriteLine("Solved. Depth: {0}", depth);
						break;
					}
					else
						Console.WriteLine("This sudoku is invalid. Depth: {0}", depth);
					Console.ReadLine();
					break;
				}
				Console.WriteLine("Press <Enter> to continue ... ");
				Console.WriteLine("Input 'q' to quit ... ");
				s = Console.ReadLine();
			} while(!b.IsValid.HasValue && s != "q");
			Console.Write("Press <Enter> to exit ... ");
			Console.ReadLine();
		}
	}
}
