using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Game
{
    public class NetworkGame : Game
    {
        #region privates
        private Network.Network client;
        #endregion

        public event Network.NewGameRequestReceived newGameRequestReceivedEvent;

        public NetworkGame(Logik.Player.Player p1, Logik.IBrettDarsteller bdar, Logik.Fields.BaseField field, Network.Network client) 
        {
            client.NewMessageReceivedEvent += ReceiveNewGame;

            NewGameEvent += SendNewGame;
            PlayerMovedEvent += SendVector;

            Initialize(p1, bdar, field, client);
        }

        public void Initialize(Logik.Player.Player p1, Logik.IBrettDarsteller bdar, Logik.Fields.BaseField field, Network.Network client)
        {
            this.client = client;
            base.Initialize(p1, new Player.NetworkPlayer(UniTTT.Logik.Player.Player.PlayerChange(p1.Symbol), client), bdar, field);
            if (p1.Symbol != 'X')
            {
                PlayerChange();
            }
        }

        public void SendVector(Vector2i vect)
        {
            if (!(Player is Player.NetworkPlayer))
            {
                client.Send(string.Format("UniTTT!Vector:{0}", vect.ToString()));
            }
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