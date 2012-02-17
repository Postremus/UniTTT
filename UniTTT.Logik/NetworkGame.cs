using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public class NetworkGame
    {
        #region privates
        private string ip;
        private int port;
        private Network.TCPServer server;
        private Network.INetwork client;
        private bool isSending;
        private bool isClient;
        private const string connectionString = "UniTTT";
        private event Network.NewVector2iReceivedHandler newVector2iReceivedEvent;
        private bool isNewVector2iReceivedEventRaised;
        #endregion

        #region Propertys
        public Fields.IField Field
        {
            get;
            set;
        }
        public Logik.IBrettDarsteller BDarsteller { get; private set; }
        public Logik.IOutputDarsteller ODarsteller { get; private set; }
        public Player.AbstractPlayer Player { get; private set; }
        public Player.AbstractPlayer Player1 { get; private set; }
        public Player.AbstractPlayer Player2 { get; private set; }
        #endregion

        public NetworkGame(Logik.Player.AbstractPlayer p1, Logik.IBrettDarsteller bdar, Logik.IOutputDarsteller odar, Logik.Fields.IField field, string ip, int port, bool isServer)
        {
            this.ip = ip;
            this.port = port;

            if (isServer)
            {
                server = new Network.TCPServer(port);
                isClient = false;
                isSending = true;
            }
            else
            {
                client = new Network.IRCClient(ip, port);
                client.NewMassegeReceivedEvent += ReceiveVector;
                isSending = true;
                isClient = true;
            }
            newVector2iReceivedEvent += SetVectorOnField;

            Initialize(p1, new Player.HumanPlayer(SitCodeHelper.ToPlayer(SitCodeHelper.PlayerChange(SitCodeHelper.PlayertoSitCode(p1.Symbol)))), bdar, odar, field);
        }

        public void Initialize(Logik.Player.AbstractPlayer p1, Logik.Player.AbstractPlayer p2, Logik.IBrettDarsteller bdar, Logik.IOutputDarsteller odar, Logik.Fields.IField field)
        {
            if (field == null)
            {
                Field = new Fields.Brett(bdar.Width, bdar.Height);
            }
            else
            {
                Field = field;
            }
            BDarsteller = bdar;
            ODarsteller = odar;
            Player1 = p1;
            Player2 = p2;
            Initialize();
        }

        public void Initialize()
        {
            if (IsODarstellerValid())
            {
                ODarsteller.Title = "UniTTT - " + this.ToString();
            }
            if (IsBDarstellerValid())
            {
                BDarsteller.Update(Field);
                BDarsteller.Draw();
            }
        }

        public void Logik()
        {
            PlayerChange();
            if (IsODarstellerValid())
            {
                ODarsteller.PlayerAusgabe(Player.Ausgabe());
            }

            if (isSending)
            {
                Vector2i vect = Player.Play(Field);
                SendVector(vect);
                isSending = false;
                SetVectorOnField(vect);
            }
            else
            {
                while (!isNewVector2iReceivedEventRaised) { }
                isSending = true;
                isNewVector2iReceivedEventRaised = false;
            }

            if (IsBDarstellerValid())
            {
                BDarsteller.Update(Field);
                BDarsteller.Draw();
            }
        }

        public void SetVectorOnField(Vector2i vect)
        {
            if (Field.IsFieldEmpty(vect))
            {
                Field.SetField(vect, Player.Symbol);
            }
        }

        private void ReceiveVector(string value)
        {
            //UNITTT!X:5|Y:1
            Vector2i vect = null;
            if (!value.Contains("UniTTT!"))
            {
                return;
            }
            List<string> subs = value.GetSubstrs();
            vect = Vector2i.StringToVector(subs[subs.Count-1].Replace(":UniTTT!", null), true);
            OnNewVector2iReceivedEvent(vect);
        }

        private void SendVector(Vector2i vect)
        {
            string sendStr = string.Format("UniTTT!X:{0}|Y:{1}", vect.X, vect.Y);
            if (isClient)
            {
                client.Send(sendStr);
            }
            else
            {
                server.Send(sendStr);
            }
        }

        public void LogikLoop()
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

        public void OnNewVector2iReceivedEvent(Vector2i vect)
        {
            isNewVector2iReceivedEventRaised = false;
            Network.NewVector2iReceivedHandler vector2iReceivedEvent = newVector2iReceivedEvent;
            if (vector2iReceivedEvent != null)
            {
                vector2iReceivedEvent(vect);
                isNewVector2iReceivedEventRaised = true;
            }
        }

        public bool IsBDarstellerValid()
        {
            return BDarsteller != null;
        }

        public bool IsODarstellerValid()
        {
            return ODarsteller != null;
        }

        public bool IsFieldValid()
        {
            return Field != null;
        }

        public void PlayerChange()
        {
            Player = Player1 == Player ? Player2 : Player1;
        }

        public void NewGame()
        {
            Field.Initialize();
            Player = null;
            if (IsBDarstellerGraphical())
            {
                ((Logik.IGraphicalBrettDarsteller)BDarsteller).DeLock();
            }
            BDarsteller.Update(Field);
            BDarsteller.Draw();
        }

        public bool IsBDarstellerGraphical()
        {
            return BDarsteller is IGraphicalBrettDarsteller;
        }
    }
}