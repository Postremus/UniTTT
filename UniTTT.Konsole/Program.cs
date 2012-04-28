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

            HumanPlayer hPlayer;
            char kisymb;

            if (parameters.GetString("player") != null)
            {
                hPlayer = new HumanPlayer(parameters.GetString("player")[0]);
                kisymb = parameters.GetString("player")[0] == 'X' ? 'O' : 'X';
            }
            else
            {
                hPlayer = new HumanPlayer('x');
                kisymb = 'O';
            }

            Logik.Player.AbstractPlayer kiplayer = null;
            if (parameters.GetInt("ki") > 0)
            {
                kiplayer = new Logik.Player.KIPlayer(parameters.GetInt("ki"), width, height, kisymb, new OutputDarsteller());
            }
            else if (parameters.GetString("ki") != null)
            {
                if (Enum.IsDefined(typeof(Logik.Player.KIPlayer.KISystems), parameters.GetString("ki")))
                {
                    kiplayer = new Logik.Player.KIPlayer(parameters.GetString("ki"), width, height, kisymb, new OutputDarsteller());
                }
            }
            Logik.Fields.IField field;
            if (parameters.GetString("field") == "string")
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
            if (parameters.GetBool("help"))
            {
                if (parameters.Count == 1)
                {
                    Help();
                }
            }
            else if (parameters.GetBool("learn"))
            {
                if (parameters.GetBool("human"))
                {
                    hPlayer.Learn();
                }
                else
                {
                    if (kiplayer == null) throw new FormatException();
                    Console.Title = string.Format(CultureInfo.CurrentCulture, "UniTTT - {0} Lernmodus: {1}", kiplayer.ToString(), kiplayer.ToString());
                    kiplayer.Learn();
                }
            }
            else if (parameters.GetBool("network"))
            {
                Games.NetworkGame game;
                Logik.Network.Network client;
                string ip = parameters.GetString("ip");
                int port = parameters.GetInt("port");
                if (parameters.GetString("protokoll") == "irc")
                {
                    client = new Logik.Network.IRCClient(ip, port, parameters.GetString("channel"));
                }
                else
                {
                    if (parameters.GetBool("server"))
                    {
                        client = new Logik.Network.TCPServer(ip, port);
                    }
                    else
                    {
                        client = new Logik.Network.TCPClient(ip, port);
                    }
                }
                game = new Games.NetworkGame(width, height, hPlayer, field, ip, port, client);
                game.Run();
            }
            else
            {
                Games.Game game;
                if (!parameters.GetBool("kigame"))
                {
                    if (kiplayer != null)
                    {
                        game = new Games.Game(width, height, hPlayer, kiplayer, field);
                    }
                    else
                    {
                        game = new Games.Game(width, height, hPlayer, new HumanPlayer('O'), field);
                    }
                }
                else
                {
                    game = new Games.Game(width, height, kiplayer, kiplayer, field);
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
            Console.WriteLine("/ki:         KI als Ganzzahl (1-6, oder als Wort).");
            Console.WriteLine("/human       Ruft eine kleine Anleitung zum Spiel ab. (benötigt /learn)");
            Console.WriteLine("/field:      Die Speicher Variante des Spielfeldes.");
            Console.WriteLine("/log         Speichert ein paar Infos.");
            Console.WriteLine("/win:        Gibt die für einen Sieg benötigte Anzahl von X oder O nebeneinader,.. an.");
            Console.WriteLine("/network     Startet ein Netzwerkspiel (/ip, /port und /player wird benötigt).");
            Console.WriteLine("/ip:         Verbindungs-IP für das Netzwerkspiel, funktioniert nur mit /network.");
            Console.WriteLine("/port:       Verbindungs-Port für das Netzwerkspiel, funktioniert nur mit /network.");
            Console.WriteLine("/player:     Spieler für das Netzwerkspiel, fuktioniert nur mit /network.");
            Console.WriteLine("/protokoll:  Das zu verwendende Protokoll in einem Netzwerkspiel. Standard ist tcp");
        }
    }
}