using System;

namespace Tetris
{
	class JBlock : Shape
	{
		public JBlock(int midpoint) 
			: base(midpoint, new ShapeElement[] { new ShapeElement(midpoint, 0), new ShapeElement(midpoint, 1), new ShapeElement(midpoint, 2), new ShapeElement(midpoint - 1, 2) })
		{
			
		}
	}
}
