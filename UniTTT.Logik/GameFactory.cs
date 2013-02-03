using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniTTT.Logik.Game;
using UniTTT.Logik.Fields;
using UniTTT.Logik.Network;
using UniTTT.Logik.Player;
using System.Globalization;

namespace UniTTT.Logik
{
    public class GameFactory
    {
        public static Logik.Game.Game CreateGame(Parameterdata data)
        {
            return CreateGame(data, null);
        }

        public static Logik.Game.Game CreateGame(Parameterdata data, Player.Player humanPlayer)
        {
            if (humanPlayer == null)
            {
                humanPlayer = new Player.Player('X');
            }
            int width = data.GetValue<int>("breite");
            int height = data.GetValue<int>("hoehe");

            if (!data.IsDefined("breite"))
            {
                width = 3;
            }
            if (!data.IsDefined("hoehe"))
            {
                height = 3;
            }

            Logik.Fields.Field field = new Logik.Fields.Brett(width, height);
            char aisymb;
            Player.Player hPlayer = humanPlayer;
            if (data.IsDefined("player"))
            {
                char symb = data.GetValue<char>("player");
                aisymb = Logik.Player.Player.PlayerChange(symb);
                hPlayer.Symbol = symb;
            }
            else
            {
                hPlayer.Symbol = 'X';
                aisymb = 'O';
            }

            Logik.Player.Player aiplayer = null;
            if (data.GetValue<int>("ki") != default(int))
            {
                aiplayer = new Logik.Player.AIPlayer(data.GetValue<int>("ki"), width, height, aisymb);
            }
            else if (data.GetValue<string>("ki") != default(string))
            {
                aiplayer = new Logik.Player.AIPlayer(data.GetValue<string>("ki"), width, height, aisymb);
            }

            if (data.IsDefined("network"))
            {
                Logik.Network.Network client;
                string ip = data.GetValue<string>("ip");
                int port = data.GetValue<int>("port");
                string nick = data.GetValue<string>("nick");
                if (data.GetValue<string>("protokoll") == "irc")
                {
                    client = new Logik.Network.IRCClient(ip, port, data.GetValue<string>("channel"), nick);
                }
                else
                {
                    if (data.IsDefined("server"))
                    {
                        client = new Logik.Network.TCPServer(ip, port, nick);
                    }
                    else
                    {
                        client = new Logik.Network.TCPClient(ip, port, nick);
                    }
                }

                return new Logik.Game.NetworkGame(hPlayer, new Logik.Player.NetworkPlayer(Logik.Player.Player.PlayerChange(hPlayer.Symbol), ref client), null, field, ref client, true);
            }
            else
            {
                Logik.Game.Game gameMode;
                if (data.IsDefined("kigame"))
                {
                    gameMode = new Logik.Game.Game(aiplayer, aiplayer, null, field);
                }
                else
                {
                    if (aiplayer != null)
                    {
                        gameMode = new Logik.Game.Game(hPlayer, aiplayer, null, field);
                    }
                    else
                    {
                        Player.Player hPlayer1 = humanPlayer;
                        hPlayer1.Symbol = 'X';
                        Player.Player hPlayer2 = humanPlayer;
                        hPlayer2.Symbol = 'O';
                        gameMode = new Logik.Game.Game(hPlayer1, hPlayer2, null, field);
                    }
                }
                return gameMode;
            }
        }
    }
}
