using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
// using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotNetSudoku {
	public partial class SudokuControl9x9 : UserControl
	{
		private readonly Board9x9 _b;
		public Board9x9 Board { get { return _b; } }

		public SudokuControl9x9() {
			_b = new Board9x9();
			InitializeComponent();
			TabStop = true;
		}
		protected override void OnEnter(EventArgs e) {
			base.OnEnter(e);
			Invalidate(false);
		}
		protected override void OnLeave(EventArgs e) {
			base.OnLeave(e);
			Invalidate(false);
		}
		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			Invalidate(false);
		}
		protected override void OnPaint(PaintEventArgs e) {
			SuspendLayout();
			// Graphics g = e.Graphics;
			Graphics g = base.CreateGraphics();
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
			g.Clear(BackColor);
			int w = ClientSize.Width;
			int h = ClientSize.Height;
			int m = w < h ? w : h;
			int lw = m / 200;
			if(lw < 1) lw = 1;
			int c = (m - lw * 14) / 9;
			m = c * 9 + lw * 14;
			int top = (h - m) / 2;
			int left = (w - m) / 2;
			int sq = (lw + c) * 3;
			// Brush back = new SolidBrush(BackColor);
			Brush back = new SolidBrush(Color.White);
			Brush fore = new SolidBrush(Color.Black);
			g.FillRectangle(back, left, top, m, m);
			g.FillRectangle(fore, left, top, lw, m);
			g.FillRectangle(fore, left, top, m, lw);
			for(int i = 0; i < 3; ++i) {
				for(int j = 0; j <= 3; ++j) {
					int pos = lw + (lw + sq) * i + (lw + c) * j;
					g.FillRectangle(fore, left + pos, top, lw, m);
					g.FillRectangle(fore, left, top + pos, m, lw);
					// g.FillRectangle(fore, left + lw + (i * 3 + j) * (lw + c) + i * lw, top, lw, m);
					// g.FillRectangle(fore, left, top + lw + (i * 3 + j) * (lw + c) + i * lw, m, lw);
				}
			}
			g.FillRectangle(fore, left + m - lw, top, lw, m);
			g.FillRectangle(fore, left, top + m - lw, m, lw);
			ResumeLayout(false);
			// base.OnPaint(e);
		}
	}
}
