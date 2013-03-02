using System;

namespace Tetris
{
    class TBlock : Shape
    {
        public TBlock(int midpoint)
        {
            base.ShapeElements = new ShapeElement[] 
            {
                new ShapeElement(midpoint, 1),
                new ShapeElement(midpoint + 1, 1),
                new ShapeElement(midpoint + 1, 0),
                new ShapeElement(midpoint + 2, 0)
            };
        }
    }
}