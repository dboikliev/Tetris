using System;

namespace Tetris
{
	class SquareBlock : Shape
	{
		public SquareBlock(int midpoint) : base(midpoint, new ShapeElement[] { new ShapeElement(midpoint, 0), new ShapeElement(midpoint + 1, 0), new ShapeElement(midpoint, 1), new ShapeElement(midpoint + 1, 1) })
		{
		}
	}
}
