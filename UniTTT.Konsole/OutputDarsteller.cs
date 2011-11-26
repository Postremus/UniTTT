using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Konsole
{
    class OutputDarsteller : Logik.IOutputDarsteller
    {
        public string Title
        {
            set
            {
                Console.Title = value;
            }
        }

        public void WinMessage(char player, Logik.FieldHelper.GameStates state)
        {
            if (state == Logik.FieldHelper.GameStates.Gewonnen)
                Console.WriteLine("Spieler {0} hat Gewonnen", player);
            else
                Console.WriteLine(state);
        }

        public void PlayerAusgabe(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
        }

        public void ThrowMessage(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
        }
    }
}
