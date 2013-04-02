using System;

namespace Tetris
{
	class ShapeElement
	{
		public int X { get; set; }
		public int Y { get; set; }

		public ShapeElement(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
	}
}
