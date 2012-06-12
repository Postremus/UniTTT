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
        #endregion

        public event Network.NewGameRequestedHandler newGameRequestedEvent;
        public event Network.NewGameRequestReceived newGameRequestReceivedEvent;

        public NetworkGame(Logik.Player.Player p1, Logik.IBrettDarsteller bdar, Logik.Fields.Field field, string ip, int port, Network.Network client) 
        {
            client.NewMessageReceivedEvent += ReceiveNewGame;

            newGameRequestedEvent += SendNewGame;
            newGameRequestedEvent += NewGame;
            newGameRequestReceivedEvent += NewGame;
            PlayerMovedEvent += SendVector;

            Initialize(p1, bdar, field, ip, port, client);
        }

        public void Initialize(Logik.Player.Player p1, Logik.IBrettDarsteller bdar, Logik.Fields.Field field, string ip, int port, Network.Network client)
        {
            this.ip = ip;
            this.port = port;
            this.client = client;
            base.Initialize(p1, new Player.NetworkPlayer(UniTTT.Logik.Player.Player.PlayerChange(p1.Symbol, p1.Symbol, 'O'), client), bdar, field);
            if (p1.Symbol != 'X')
            {
                PlayerChange();
            }
        }

        public void SendVector(Vector2i vect)
        {
            client.Send(string.Format("UniTTT!Vector:{0}", vect.ToString()));
        }

        private void ReceiveNewGame(string value)
        {
            if (!value.Contains("UniTTT!NewGame"))
                return;
            OnNewGameRequestReceivedEvent();
        }

        private void SendNewGame()
        {
            client.Send("UniTTT!NewGame");
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

        public override void NewGame()
        {
            base.NewGame();
            if (Player1.Symbol != 'X')
            {
                PlayerChange();
            }
        }
    }
}