using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Game
{
    public class NetworkGame : Game
    {
        #region privates
        private string ip;
        private int port;
        private Network.Network client;
        private const string connectionString = "UniTTT";
        private bool isNewVector2iReceivedEventRaised;
        #endregion

        public event Network.NewVector2iReceivedHandler newVector2iReceivedEvent;
        public event Network.NewFieldReceivedHandler newFieldReceivedEvent;
        public event Network.NewGameRequestedHandler newGameRequestedEvent;
        public event Network.NewGameRequestReceived newGameRequestReceivedEvent;

        public NetworkGame(Logik.Player.AbstractPlayer p1, Logik.IBrettDarsteller bdar, Logik.IOutputDarsteller odar, Logik.Fields.IField field, string ip, int port, Network.Network client)
        {
            client.NewMessageReceivedEvent += ReceiveVector;
            client.NewMessageReceivedEvent += ReceiveField;
            client.NewMessageReceivedEvent += ReceiveNewGame;
            newVector2iReceivedEvent += SetVectorOnField;

            newFieldReceivedEvent += EqualFieldSizes;
            newGameRequestedEvent += SendNewGame;
            newGameRequestedEvent += NewGame;
            newGameRequestReceivedEvent += NewGame;

            Initialize(p1, bdar, odar, field, ip, port, client);
        }

        public void Initialize(Logik.Player.AbstractPlayer p1, Logik.IBrettDarsteller bdar, Logik.IOutputDarsteller odar, Logik.Fields.IField field, string ip, int port, Network.Network client)
        {
            this.ip = ip;
            this.port = port;
            this.client = client;
            base.Initialize(p1, new Player.HumanPlayer(SitCodeHelper.ToPlayer(SitCodeHelper.PlayerChange(SitCodeHelper.PlayertoSitCode(p1.Symbol)))), bdar, odar, field);
            if (p1.Symbol != 'X')
            {
                PlayerChange();
            }
        }

        public void Logik()
        {
            PlayerChange();
            if (!HasStarted)
            {
                HasStarted = true;
            }

            if (HasStoped)
            {
                return;
            }

            if (IsODarstellerValid())
            {
                ODarsteller.PlayerAusgabe(Player.Ausgabe());
            }

            if (IsSending())
            {
                Vector2i vect = Player.Play(Field);
                client.Send(string.Format("UniTTT!{0}", vect.ToString()));
                SetVectorOnField(vect);
            }
            else
            {
                while (!isNewVector2iReceivedEventRaised) { }
                isNewVector2iReceivedEventRaised = false;
            }

            if (IsBDarstellerValid())
            {
                BDarsteller.Update(Field);
                BDarsteller.Draw();
            }
        }

        public void LogikLoop()
        {
            HasStoped = false;
            HasStarted = true;
            do
            {
                Logik();
            } while (!HasEnd());
            HasStoped = true;
            HasStarted = false;
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
            if (!value.Contains("UniTTT!"))
            {
                return;
            }
            string str = value.Remove(0, value.IndexOf("UniTTT!") + 7);
            Vector2i vect = Vector2i.StringToVector(str, true);
            OnNewVector2iReceivedEvent(vect);
        }

        private void ReceiveField(string value)
        {
            if (!value.Contains("Field:"))
            {
                return;
            }

            Fields.IField field = (Fields.IField)value.GetObject();
            if (field != null)
            {
                OnNewFieldReceivedEvent(field);
            }
        }

        private void ReceiveNewGame(string value)
        {
            if (!value.Contains("NewGame"))
                return;
            OnNewGameRequestReceivedEvent();
        }

        private void SendNewGame()
        {
            client.Send("NewGame");
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

        public void OnNewFieldReceivedEvent(Fields.IField field)
        {
            Network.NewFieldReceivedHandler fieldreceivedEvent = newFieldReceivedEvent;
            if (fieldreceivedEvent != null)
            {
                fieldreceivedEvent(field);
            }
        }

        public void OnNewGameRequestedEvent()
        {
            Network.NewGameRequestedHandler gameStartedEvent = newGameRequestedEvent;
            if (gameStartedEvent != null)
            {
                gameStartedEvent();
            }
        }

        public void OnNewGameRequestReceivedEvent()
        {
            Network.NewGameRequestReceived newGameRequestReceived = newGameRequestReceivedEvent;
            if (newGameRequestReceived != null)
            {
                newGameRequestReceived();
            }
        }

        public void NewGame()
        {
            Field.Initialize();
            Player = null;
            if (IsBDarstellerGraphical())
            {
                ((Logik.IGraphicalBrettDarsteller)BDarsteller).Enabled = true;
            }
            BDarsteller.Update(Field);
            BDarsteller.Draw();

            if (Player1.Symbol != 'X')
            {
                PlayerChange();
            }
            HasStoped = false;
            HasStarted = true;
        }

        public void EqualFieldSizes(Fields.IField field)
        {
            HasStoped = base.Field.Width != field.Width && base.Field.Height != field.Height;
        }

        public bool IsSending()
        {
            return Player == Player1;
        }

        public override string ToString()
        {
            return "Networkgame";
        }
    }
}