using System;

namespace Tetris
{
	class ReverseSBlock : Shape
	{
		public ReverseSBlock(int midpoint)
		{
            base.ShapeElements = new ShapeElement[] 
            { 
                new ShapeElement(midpoint, 0), 
                new ShapeElement(midpoint + 1, 0), 
                new ShapeElement(midpoint + 1, 1), 
                new ShapeElement(midpoint + 2, 1) 
            };
		}
	}
}
