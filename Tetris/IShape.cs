using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    public interface IShape
    {
        void MoveLeft();
        void MoveRight();
        void MoveDown();
    }
}
