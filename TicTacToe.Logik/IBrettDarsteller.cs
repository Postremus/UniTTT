using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe.Logik
{
    public interface IBrettDarsteller
    {
        int Breite { get; }
        int Hoehe { get; }

        void Update(char[,] brett);
        void Draw();
    }
}
