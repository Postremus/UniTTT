using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe.Logik
{
    public interface IGraphicalBrettDarsteller : IBrettDarsteller
    {
        void Sperren();
        void EntSperren();
        void Erstellen();
    }
}
