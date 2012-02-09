using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public class NetworkGame : NormalGame
    {
        #region privates
        private string ip;
        private int port;
        private Network.TCPServer server;
        private Network.TCPClient client;
        private bool isSending;
        private bool isClient;
        private UniTTT.Logik.Player.AbstractPlayer dumie;
        #endregion

        public NetworkGame(Logik.Player.AbstractPlayer p1, Logik.IBrettDarsteller bdar, Logik.IOutputDarsteller odar, Logik.Fields.IField field, string ip, int port) : base(p1, null, bdar, odar, field)
        {
            this.ip = ip;
            this.port = port;
            dumie = new Player.HumanPlayer(SitCodeHelper.ToPlayer(SitCodeHelper.PlayerChange(SitCodeHelper.PlayertoSitCode(Player1.Symbol))));
            if (ip == "127.0.0.1")
            {
                server = new Network.TCPServer(port);
                isClient = false;
                isSending = true;
            }
            else
            {
                client = new Network.TCPClient(ip, port);
                isSending = false;
                isClient = true;
            }
        }

        public override void Logik()
        {
            if (IsODarstellerValid())
            {
                if (isSending)
                {
                    ODarsteller.PlayerAusgabe(Player1.Ausgabe());
                }
                else
                {
                    ODarsteller.PlayerAusgabe(dumie.Ausgabe());
                }
            }
            Vector2i vect;

            char symbol;
            if (isSending)
            {
                vect = Player1.Play(Field);
                SendVector(vect);
                isSending = false;
                symbol = Player1.Symbol;
            }
            else
            {
                vect = ReceiveVector();
                isSending = true;
                symbol = dumie.Symbol;
            }

            if (!Field.IsFieldEmpty(vect))
            {
                throw new ArgumentException();
            }

            Field.SetField(vect, symbol);
            if (IsBDarstellerValid())
            {
                BDarsteller.Update(Field);
                BDarsteller.Draw();
            }
        }

        private Vector2i ReceiveVector()
        {
            Vector2i vect;
            if (isClient)
            {
                vect = client.Receive();
            }
            else
            {
                vect = server.Receive();
            }
            isSending = true;
            return vect;
        }

        private void SendVector(Vector2i vect)
        {
            if (isClient)
            {
                client.Send(vect);
            }
            else
            {
                server.Send(vect);
            }
        }

        public override void LogikLoop()
        {
            do
            {
                Logik();
            } while (!HasEnd());
        }

        public bool HasEnd()
        {
            if (FieldHelper.GetGameState(Field, Player1.Symbol) != UniTTT.Logik.FieldHelper.GameStates.Laufend)
                return true;
            return false;
        }
    }
}
