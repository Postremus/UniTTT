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

        public void WinMessage(char player, Logik.BrettHelper.GameStates state)
        {
            if (state == Logik.BrettHelper.GameStates.Gewonnen)
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
