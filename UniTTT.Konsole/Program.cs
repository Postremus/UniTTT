using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using UniTTT.Logik;

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
            args = new string[4];
            //args[0] = "/breite:3";
            //args[1] = "/hoehe:5";
            //args[2] = "/human";
            //args[2] = "/log";
            //args[0] = "/win:3";
            //args[1] = "/breite:6";
            //args[2] = "/hoehe:6";
            //args[3] = "/log";
            Logik.Parameters parameters = Logik.Parameters.InterpretCommandLine(args);

            int width = parameters.GetInt("breite");
            int height = parameters.GetInt("hoehe");

            if (width == -1)
                width = 3;
            if (height == -1)
                height = 3;

            Logik.Logger l = null;
            if (parameters.GetBool("log"))
            {
                l = new Logik.Logger(new OutputDarsteller());
                l.Start();
            }

            Logik.Player.AbstractPlayer kiplayer = null;
            if (parameters.GetInt("ki") > 0)
            {
                kiplayer = new Logik.Player.KIPlayer(parameters.GetInt("ki"), width, height, 'O', new OutputDarsteller());
            }
            else if (parameters.GetString("ki") != null)
            {
                if (Enum.IsDefined(typeof(Logik.Player.KIPlayer.KISystems), parameters.GetString("ki")))
                {
                    kiplayer = new Logik.Player.KIPlayer(parameters.GetString("ki"), width, height, 'O', new OutputDarsteller());
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
                Game game;
                Logik.Fields.IField field;
                if (parameters.GetInt("win") != -1)
                {
                    WinChecker.GewinnBedingung = parameters.GetInt("win");
                }
                if (parameters.GetString("field") ==  "string")
                {
                    field = new Logik.Fields.SitCode(width, height);
                }
                else if (parameters.GetString("field") == "array")
                {
                    field = new Logik.Fields.Brett(width, height);
                }
                else
                {
                    field = new Logik.Fields.Brett(width, height);
                }
                if (!parameters.GetBool("kigame"))
                {
                    if (kiplayer != null)
                    {
                        game = new Game(width, height, new HumanPlayer('X'), kiplayer, field);
                    }
                    else
                    {
                        game = new Game(width, height, new HumanPlayer('X'), new HumanPlayer('O'), field);
                    }
                }
                else
                {
                    game = new Game(3, 3, kiplayer, kiplayer, field);
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
            Console.WriteLine("/win:        Gibt die für einen Sieg benötigte Anzahl von X oder O nebeneinader,.. an.");
        }
    }
}