﻿using System;
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
        //ki:5 = Random7
        //ki:6 = Bot
        static void Main(string[] args)
        {
            Logik.ParameterInterpreter parameters = Logik.ParameterInterpreter.InterpretCommandLine(args);
            Logik.Plugin.PluginManager plugManager = new Logik.Plugin.PluginManager();

            int width = parameters.GetValue<int>("breite");
            int height = parameters.GetValue<int>("hoehe");

            Logik.Fields.Field field = new Logik.Fields.Brett(width, height);
            if (parameters.IsDefined<string>("plugin"))
            {
                Logik.Plugin.IPlugin plugin = plugManager.Get(parameters.GetValue<string>("plugin"), Logik.Plugin.PluginTypes.Field);
                if (plugin is Logik.Fields.Field)
                {
                    field = (Logik.Fields.Field)plugin;
                }
            }

            if (!parameters.IsDefined<int>("breite"))
            {
                width = 3;
            }
            if (!parameters.IsDefined<int>("hoehe"))
            {
                height = 3;
            }

            HumanPlayer hPlayer;
            char kisymb;
            if (parameters.IsDefined<char>("player"))
            {
                char symb = parameters.GetValue<char>("player");
                kisymb = symb == 'X' ? 'O' : 'X';
                hPlayer = new HumanPlayer(symb);
            }
            else
            {
                hPlayer = new HumanPlayer('X');
                kisymb = 'O';
            }

            Logik.Player.Player kiplayer = null;
            if (parameters.IsDefined<int>("ki"))
            {
                kiplayer = new Logik.Player.KIPlayer(parameters.GetValue<int>("ki"), width, height, kisymb);
            }
            else if (parameters.IsDefined<int>("ki"))
            {
                kiplayer = new Logik.Player.KIPlayer(parameters.GetValue<string>("ki"), width, height, kisymb);
            }

            if (parameters.GetValue<bool>("help"))
            {
                if (parameters.Count == 1)
                {
                    Help();
                }
            }
            else if (parameters.GetValue<bool>("learn"))
            {
                if (parameters.GetValue<bool>("human"))
                {
                    hPlayer.Learn();
                }
                else
                {
                    if (kiplayer == null) throw new FormatException();
                    Console.Title = string.Format(CultureInfo.CurrentCulture, "UniTTT - {0} Lernmodus", kiplayer.ToString());
                    ((Logik.Player.KIPlayer)kiplayer).KI.ShowMessageEvent += Console.WriteLine;
                    ((Logik.Player.KIPlayer)kiplayer).KI.GetStringEvent += Console.ReadLine;
                    ((Logik.Player.KIPlayer)kiplayer).KI.GetIntEvent += GetInt;
                    kiplayer.Learn();
                }
            }
            else if (parameters.GetValue<bool>("network"))
            {
                Logik.Network.Network client;
                string ip = parameters.GetValue<string>("ip");
                int port = parameters.GetValue<int>("port");
                if (parameters.GetValue<string>("protokoll") == "irc")
                {
                    client = new Logik.Network.IRCClient(ip, port, parameters.GetValue<string>("channel"));
                }
                else
                {
                    if (parameters.GetValue<bool>("server"))
                    {
                        client = new Logik.Network.TCPServer(ip, port);
                    }
                    else
                    {
                        client = new Logik.Network.TCPClient(ip, port);
                    }
                }
                
                Logik.Game.Game gameMode = new Logik.Game.NetworkGame(hPlayer, new BrettDarsteller(width, height), field, ip, port, client);
                Game g = new Game(gameMode);
                g.Run();
            }
            else
            {
                Logik.Game.Game gameMode;
                BrettDarsteller bdar = new BrettDarsteller(width, height);
                if (parameters.GetValue<bool>("kigame"))
                {
                    gameMode = new Logik.Game.NormalGame(kiplayer, kiplayer, bdar, field);
                }
                else
                {
                    if (kiplayer != null)
                    {
                        gameMode = new Logik.Game.NormalGame(hPlayer, kiplayer, bdar, field);
                    }
                    else
                    {
                        gameMode = new Logik.Game.NormalGame(new HumanPlayer('X'), new HumanPlayer('O'), bdar, field);
                    }
                }
                Game g = new Game(gameMode);
                g.Run();
            }
        }

        private static int GetInt()
        {
            int ret;
            int.TryParse(Console.ReadLine(), out ret);
            return ret;
        }

        private static void Help()
        {
            Console.WriteLine("/help        Gibt diese Hilfe aus");
            Console.WriteLine("/kigame      Startet ein Spiel zwischen zwei KIs.");
            Console.WriteLine("/breite:     Breite des Spielfeldes");
            Console.WriteLine("/hoehe:      Hoehe des Spielfeldes");
            Console.WriteLine("/ki:         KI als Ganzzahl (1-6, oder als Wort).");
            Console.WriteLine("/human       Ruft eine kleine Anleitung zum Spiel ab. (benötigt /learn)");
            Console.WriteLine("/win:        Gibt die für einen Sieg benötigte Anzahl von X oder O nebeneinader,.. an.");
            Console.WriteLine("/network     Startet ein Netzwerkspiel (/ip, /port und /player wird benötigt).");
            Console.WriteLine("/ip:         Verbindungs-IP für das Netzwerkspiel, funktioniert nur mit /network.");
            Console.WriteLine("/port:       Verbindungs-Port für das Netzwerkspiel, funktioniert nur mit /network.");
            Console.WriteLine("/player:     Spieler für das Netzwerkspiel, fuktioniert nur mit /network.");
            Console.WriteLine("/protokoll:  Das zu verwendende Protokoll in einem Netzwerkspiel. Standard ist tcp");
            Console.WriteLine("Save /paramn As configname   Speichert die Parameter param unter den name configname");
            Console.WriteLine("Load configname  Lädt die Parametkonfiguration configname");
            Console.WriteLine("Delete configname Löscht die Parameterkonfiguration configname");
        }
    }
}