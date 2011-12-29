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
            //args = new string[3];
            //args[0] = "/ki:1";
            //args[1] = "/learn";
            //args[2] = "/human";
            //args[2] = "/log";
            Logik.Parameters parameters = Logik.Parameters.InterpretCommandLine(args);

            int breite = parameters.GetInt("breite");
            int hoehe = parameters.GetInt("hoehe");

            if (breite == -1)
                breite = 3;
            if (hoehe == -1)
                hoehe = 3;

            Logik.Logger l = null;
            if (parameters.GetBool("log"))
            {
                l = new Logik.Logger(new OutputDarsteller());
                l.Start();
            }

            Logik.Player.AbstractPlayer kiplayer = null;
            if (parameters.GetInt("ki") > 0)
            {
                kiplayer = new Logik.Player.KIPlayer(parameters.GetInt("ki"), breite, hoehe, 'O', new OutputDarsteller());
            }
            else if (parameters.GetString("ki") != null)
            {
                if (Enum.IsDefined(typeof(Logik.Player.KIPlayer.KISystems), parameters.GetString("ki")))
                {
                    kiplayer = new Logik.Player.KIPlayer(parameters.GetString("ki"), breite, hoehe, 'O', new OutputDarsteller());
                }
            }
            if (parameters.GetBool("help"))
            {
                if (parameters.Count() == 1)
                {
                    Help();
                }
            }
            else if (parameters.GetBool("learn"))
            {
                if (parameters.GetBool("human"))
                {
                    new HumanPlayer('X').Learn();
                }
                else
                {
                    if (kiplayer == null) throw new FormatException();
                    Console.Title = string.Format(CultureInfo.CurrentCulture, "UniTTT - {0} Lernmodus: {1}", kiplayer.ToString(), kiplayer.ToString());
                    kiplayer.Learn();
                }
            }
            else
            {
                Games.Game game;
                Logik.Fields.IField field;
                if (parameters.GetString("field") ==  "string")
                {
                    field = new Logik.Fields.SitCode(breite, hoehe);
                }
                else if (parameters.GetString("field") == "array")
                {
                    field = new Logik.Fields.Brett(breite, hoehe);
                }
                else
                {
                    field = new Logik.Fields.Brett(breite, hoehe);
                }
                if (!parameters.GetBool("kigame"))
                {
                    if (kiplayer != null)
                    {
                        game = new Games.Game(breite, hoehe, new HumanPlayer('X'), kiplayer, field);
                    }
                    else
                    {
                        game = new Games.Game(breite, hoehe, new HumanPlayer('X'), new HumanPlayer('O'), field);
                    }
                }
                else
                {
                    game = new Games.Game(3, 3, kiplayer, kiplayer, field);
                }
                game.Start();
            }
            if (l != null)
                l.Dispose();
        }

        private static void Help()
        {
            Console.WriteLine("/help        Gibt diese Hilfe aus");
            Console.WriteLine("/kigame      Startet ein Spiel zwischen zwei KIs.");
            Console.WriteLine("/breite:     Breite des Spielfeldes");
            Console.WriteLine("/hoehe:      Hoehe des Spielfeldes");
            Console.WriteLine("/ki:         KI als Ganzzahl (1-6, oder als Wort.");
            Console.WriteLine("/human       Ruft eine kleine Anleitung zum Spiel ab. (benötigt /learn)");
            Console.WriteLine("/field:      Die Speicher Variante des Spielfeldes.");
            Console.WriteLine("/log         Speichert ein paar Infos.");
        }
    }
}