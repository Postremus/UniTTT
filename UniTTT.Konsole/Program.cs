using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using System.Windows;

namespace UniTTT.Konsole
{
    class Program
    {
        #region fields
        private static int hoehe = 3, breite = 3, ki_zahl;
        private static bool learn, kigame;
        #endregion

        static void Main(string[] args)
        {
            kom_parameter(args);
            if (learn)
            {
                Logik.Player.KIPlayer kiplayer = new Logik.Player.KIPlayer(ki_zahl, breite, hoehe, 'O');
                Console.Title = string.Format(CultureInfo.CurrentCulture, "TicTacToe - {0} Lernmodus: {1}", kiplayer.ToString(), kiplayer.KI.ToString());
                kiplayer.KI.Lernen();
            }
            else
            {
                Games.Game game = (!kigame) && (ki_zahl == 0) ? new Games.Game(breite, hoehe, new HumanPlayer('X'), new HumanPlayer('O'))
                    : ki_zahl > 0 ? new Games.Game(breite, hoehe, new HumanPlayer('X'), new Logik.Player.KIPlayer(ki_zahl, breite, hoehe, 'O'))
                    : new Games.Game(3, 3, new Logik.Player.KIPlayer(2, 3, 3, 'X'), new Logik.Player.KIPlayer(3, 3, 3, 'O'));
                game.Start();
            }
        }

        //ki:1 = Reinforcement
        //ki:2 = Recursion
        //ki:3 = Minimax
        //ki:4 = Like
        static void kom_parameter(string[] args)
        {
            args = new string[1];
            args[0] = "/ki:4";

            foreach (var arg in args)
            {
                if (arg.Contains("/hoehe:"))
                    hoehe = int.Parse(arg.Replace("/hoehe:", ""), CultureInfo.CurrentCulture);
                if (arg.Contains("/breite:"))
                    breite = int.Parse(arg.Replace("/breite:", ""), CultureInfo.CurrentCulture);
                if (arg.Contains("/ki:"))
                    ki_zahl = int.Parse(arg.Replace("/ki:", ""), CultureInfo.CurrentCulture);
                if (arg.Contains("/learn"))
                    learn = true;
                if (arg.Contains("/kigame"))
                    kigame = true;
                if (true)
                {}
            }
        }
    }
}
