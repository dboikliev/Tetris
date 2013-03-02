using System;

namespace Tetris
{
    class SBlock : Shape
    {
        public SBlock(int midpoint)
        {
            base.ShapeElements = new ShapeElement[] 
            {
                new ShapeElement(midpoint, 1), 
                new ShapeElement(midpoint + 1, 1), 
                new ShapeElement(midpoint + 1, 0),
                new ShapeElement(midpoint + 2, 1)
            };
        }
    }
}