﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using System.Windows;

namespace UniTTT.Konsole
{
    class Program
    {
        //ki:1 = Reinforcement
        //ki:2 = Recursion
        //ki:3 = Minimax
        //ki:4 = Like
        //ki:5 = Random
        //ki:6 = Bot
        static void Main(string[] args)
        {
            Logik.Parameters parameters = Logik.Parameters.InterpretCommandLine(args);

            int breite = parameters.GetInt("breite");
            int hoehe = parameters.GetInt("hoehe");
            int ki_zahl = parameters.GetInt("ki");

            if (breite == -1)
                breite = 3;
            if (hoehe == -1)
                hoehe = 3;
            if (parameters.GetBool("help"))
            {
                Help();
            }
            else if (parameters.GetBool("learn"))
            {
                Logik.Player.KIPlayer kiplayer = new Logik.Player.KIPlayer(ki_zahl, breite, hoehe, 'O');
                Console.Title = string.Format(CultureInfo.CurrentCulture, "UniTTT - {0} Lernmodus: {1}", kiplayer.ToString(), kiplayer.ToString());
                kiplayer.Learn();
            }
            else
            {
                Games.Game game;
                if (!parameters.GetBool("kigame"))
                {
                    if (ki_zahl > 0)
                    {
                        game = new Games.Game(breite, hoehe, new HumanPlayer('X'), new Logik.Player.KIPlayer(ki_zahl, breite, hoehe, 'O'));
                    }
                    else
                    {
                        game = new Games.Game(breite, hoehe, new HumanPlayer('X'), new HumanPlayer('O'));
                    }
                }
                else
                {
                    game = new Games.Game(3, 3, new Logik.Player.KIPlayer(ki_zahl, 3, 3, 'X'), new Logik.Player.KIPlayer(ki_zahl, 3, 3, 'O'));
                }
                game.Start();
            }
        }

        private static void Help()
        {
            Console.WriteLine("/help        Gibt diese Hilfe aus");
            Console.WriteLine("/kigame      Startet ein Spiel zwischen zwei KIs. /ki: ist benötigt");
            Console.WriteLine("/breite:     Breite des Spielfeldes");
            Console.WriteLine("/hoehe:      Hoehe des Spielfeldes");
            Console.WriteLine("/ki:         KI");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}