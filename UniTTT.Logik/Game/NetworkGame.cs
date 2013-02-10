using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UniTTT.Logik.Network;

namespace UniTTT.Logik.Game
{
    public class NetworkGame : Game
    {
        #region privates
        private Network.Network _client;
        private bool _isServer;
        private bool _toSomeoneConnected;
        private string _enemyNick;
        private int _handShakeSended;
        private bool _isHandShakeCorrect;
        private List<string> _servers;
        private List<string> _joinRequests;
        #endregion

        public delegate void ServerListSizeChangedHandler();
        public delegate void JoinRequestListSizeChangedHandler();

        public event Network.NewGameRequestReceived NewGameRequestReceivedEvent;
        public event Network.CanJoinAnswerReceivedHandler CanJoinAnswerReceivedEvent;
        public event HandShakeAnswerReceivedHandler HandShakeAnswerReceivedEvent;
        public event NewNetworkMessageForMeHandler NewNetworkMessageForMeEvent;
        public event ServerListSizeChangedHandler ServerListSizeChangedEvent;
        public event JoinRequestListSizeChangedHandler JoinRequestListSizeChangedEvent;
        public List<string> Servers
        {
            get
            {
                lock (_servers)
                {
                    return _servers;
                }
            }
            private set
            {
                lock (_servers)
                {
                    _servers = value;
                }
            }
        }
        public List<string> JoinRequests
        {
            get
            {
                lock (_joinRequests)
                {
                    return _joinRequests;
                }
            }
            private set
            {
                lock (_joinRequests)
                {
                    _joinRequests = value;
                }
            }
        }

        public NetworkGame(Logik.Player.Player p1, Logik.Player.Player p2, Logik.IBrettDarsteller bdar, Logik.Fields.Field field, ref Network.Network client, bool isServer)
            : base(p1, p2, bdar, field)
        {
            _servers = new List<string>();
            _joinRequests = new List<string>();
            _isServer = isServer;
            _client = client;

            _client.NewNetworkMessageReceivedEvent += OnNewNetworkMessageForMeEvent;

            NewNetworkMessageForMeEvent += ReceiveHandShakeAnswer;
            NewNetworkMessageForMeEvent += ReceiveHandShakeRequest;
            NewNetworkMessageForMeEvent += ReceiveIsPlaceFree;
            NewNetworkMessageForMeEvent += ReceiveIsPlaceFreeAnswer;
            NewNetworkMessageForMeEvent += ReceiveIsServer;
            NewNetworkMessageForMeEvent += ReceiveIsServerAnswer;
            NewNetworkMessageForMeEvent += ReceiveJoinRequest;
            NewNetworkMessageForMeEvent += ReceiveJoinRequestAnswer;
            NewNetworkMessageForMeEvent += ReceiveNewGame;

            NewGameEvent += SendNewGame;
            NewGameRequestReceivedEvent += NewGame;
            CanJoinAnswerReceivedEvent += StartHandshakeAfterJoinRequest;

            _client.Connect();

            if (!_isServer)
            {
                HandShakeAnswerReceivedEvent += StartGameSettingsAfterHandshake;
                NewNetworkMessageForMeEvent += ReceiveGameSettings;
            }
            else
            {
                NewNetworkMessageForMeEvent += ReceiveGameSettingsRequest;
            }

            if (p2 is Player.NetworkPlayer)
            {
                NewNetworkMessageForMeEvent += ((UniTTT.Logik.Player.NetworkPlayer)p2).ReceiveVector;
            }

            PlayerMovedEvent += SendVector;

            Initialize(p1, bdar, field, client);
        }

        public void Initialize(Logik.Player.Player p1, Logik.IBrettDarsteller bdar, Logik.Fields.Field field, Network.Network client)
        {
            this._client = client;
            if (p1 != null && p1.Symbol != 'X')
            {
                PlayerChange();
            }
        }

        public void SendVector(Vector2i vect)
        {
            if (!(Player is Player.NetworkPlayer))
            {
                _client.SendTo(string.Format("UniTTT!Vector:{0}", vect.ToString()), _enemyNick);
            }
        }

        public void SetEnemyNick(string nick)
        {
            _enemyNick = nick;
        }

        private void SendGameSettingsRequest()
        {
            _client.SendTo("UniTTT!RequestGameSettings", _enemyNick);
        }

        private void ReceiveGameSettings(NetworkMessage received)
        {
            if (!received.Content.Contains("UniTTT!GameSettings"))
                return;
            string value = received.Content.Replace("UniTTT!GameSettings:", "");
            List<string> splited = new List<string>(value.Split(';'));
            int width = int.Parse(splited[0].SubStringBetween(":", null));
            int height = int.Parse(splited[1].SubStringBetween(":", null));
            char symbol = splited[2].SubStringBetween(":", null)[0];
            char enemySymbol = splited[3].SubStringBetween(":", null)[0];
            bool meStarts = bool.Parse(splited[4].SubStringBetween(":", null));

            base.Field.Width = width;
            base.Field.Height = height;
            if (!meStarts)
            {
                PlayerChange();
            }
            base.Player1.Symbol = symbol;
            base.Player2.Symbol = enemySymbol;
            OnGameReadyStanteChangedEvent(!GameReady);
        }

        private void ReceiveGameSettingsRequest(NetworkMessage received)
        {
            if (received.Content != "UniTTT!RequestGameSettings")
                return;
            SendGameSettings();
        }

        public void SendGameSettings()
        {
            string settings = string.Format("FieldWidth:{0};FieldHeight:{1};YourSymbol:{2};MySymbol:{3};YouStarts:{4}", Field.Width, Field.Height, Player2.Symbol, Player1.Symbol, Player == Player2);
            _client.SendTo("UniTTT!GameSettings:" + settings, _enemyNick);
            OnGameReadyStanteChangedEvent(!GameReady);
        }

        private void ReceiveHandShakeRequest(NetworkMessage received)
        {
            if (!received.Content.Contains("UniTTT!HandShake1!"))
                return;
            SendHanschakeAnswer(received);
        }

        private void SendHanschakeAnswer(NetworkMessage received)
        {
            int idx = received.Content.IndexOfLastChar("!");
            string value = received.Content.Remove(0, 18);
            _client.SendTo(string.Format("UniTTT!HandShake2!{0}", value), _enemyNick);
        }

        public void SendHandShakeRequest()
        {
            _handShakeSended = Statics.Rnd.Next(0, 512);
            _client.SendTo(string.Format("UniTTT!HandShake1!{0}", _handShakeSended), _enemyNick);
        }

        private void ReceiveHandShakeAnswer(NetworkMessage received)
        {
            if (!received.Content.Contains("UniTTT!HandShake2!"))
                return;
            string value = received.Content.Remove(0, 18);
            _isHandShakeCorrect = value == _handShakeSended.ToString();
            OnHandShakeAnswerReceivedEvent(_isHandShakeCorrect);
        }

        private void StartGameSettingsAfterHandshake(bool isCorrect)
        {
            if (isCorrect)
            {
                SendGameSettingsRequest();
            }
        }

        public void SendIsServer()
        {
            _client.SendTo("UniTTT!IsServer", _enemyNick);
        }

        private void ReceiveIsServerAnswer(NetworkMessage received)
        {
            if (!received.Content.Contains("UniTTT!IsServerAnswer!"))
                return;

            string bl = received.Content.Replace("UniTTT!IsServerAnswer!", "");

            bool isServer = bool.Parse(bl);
            if (isServer)
            {
                if (!Servers.Contains(received.Transmitter))
                {
                    Servers.Add(received.Transmitter);
                }
                OnServerListSizeChangedEvent();
            }
        }

        private void ReceiveIsServer(NetworkMessage received)
        {
            if (received.Content != "UniTTT!IsServer")
                return;
            _client.SendTo(string.Format("UniTTT!IsServerAnswer!{0}", _isServer), received.Transmitter);
        }

        public void SendIsPlaceFree()
        {
            _client.SendTo("UniTTT!PlaceFree", _enemyNick);
        }

        private void ReceiveIsPlaceFreeAnswer(NetworkMessage received)
        {
            if (!received.Content.Contains("UniTTT!PlaceFreeAnswer!"))
                return;

        }

        private void ReceiveIsPlaceFree(NetworkMessage received)
        {
            if (received.Content != "UniTTT!PlaceFree")
                return;
            _client.SendTo(string.Format("UniTTT!PlaceFreeAnswer!{0}", !_toSomeoneConnected), _enemyNick);
        }

        public void SendJoinRequest()
        {
            _client.SendTo("UniTTT!CanJoin", _enemyNick);
        }

        private void ReceiveJoinRequestAnswer(NetworkMessage received)
        {
            if (!received.Content.Contains("UniTTT!CanJoinAnswer!"))
                return;
            bool canJoin = bool.Parse(received.Content.Replace("UniTTT!CanJoinAnswer!", ""));
            OnCanJoinAnswerReceivedEvent(canJoin);
        }


        private void ReceiveJoinRequest(NetworkMessage received)
        {
            if (received.Content != "UniTTT!CanJoin")
                return;
            if (!JoinRequests.Contains(received.Transmitter))
            {
                JoinRequests.Add(received.Transmitter);
                OnJoinRequestListSizeChangedEvent();
            }
        }

        public void SendJoinAnswer()
        {
            if (!JoinRequests.Contains(_enemyNick))
            {
                return;
            }
            _client.SendTo(string.Format("UniTTT!CanJoinAnswer!{0}", !_toSomeoneConnected), _enemyNick);
            foreach (string req in JoinRequests)
            {
                if (req != _enemyNick)
                {
                    _client.SendTo("UniTTT!CanJoinAnswer!False", req);
                }
            }
            if (!_toSomeoneConnected)
            {
                _toSomeoneConnected = true;
            }
        }

        private void StartHandshakeAfterJoinRequest(bool canJoin)
        {
            if (canJoin)
            {
                SendHandShakeRequest();
            }
        }

        private void ReceiveNewGame(NetworkMessage received)
        {
            if (!received.Content.Contains("UniTTT!NewGame"))
                return;
            OnNewGameRequestReceivedEvent();
        }

        private void SendNewGame()
        {
            _client.SendTo("UniTTT!NewGame", _enemyNick);
        }

        public void OnNewGameRequestReceivedEvent()
        {
            Network.NewGameRequestReceived newGameRequestReceived = NewGameRequestReceivedEvent;
            if (newGameRequestReceived != null)
            {
                newGameRequestReceived();
            }
        }

        public void OnHandShakeAnswerReceivedEvent(bool isCorrect)
        {
            Network.HandShakeAnswerReceivedHandler handShakeAnswerReceivedEvent = HandShakeAnswerReceivedEvent;
            if (handShakeAnswerReceivedEvent != null)
            {
                handShakeAnswerReceivedEvent(isCorrect);
            }
        }

        public void OnCanJoinAnswerReceivedEvent(bool canJoin)
        {
            Network.CanJoinAnswerReceivedHandler canJoinAnswerReceivedEvent = CanJoinAnswerReceivedEvent;
            if (canJoinAnswerReceivedEvent != null)
            {
                canJoinAnswerReceivedEvent(canJoin);
            }
        }

        public void OnNewNetworkMessageForMeEvent(NetworkMessage received)
        {
            
            if (received.Content == null || received.Receiver == null || received.Receiver != ((IRCClient)_client).MyNick)
            {
                return;
            }

            NewNetworkMessageForMeHandler newNetworkMessageForMeEvent = NewNetworkMessageForMeEvent;
            if (newNetworkMessageForMeEvent != null)
            {
                newNetworkMessageForMeEvent(received);
            }
        }

        public void OnServerListSizeChangedEvent()
        {
            ServerListSizeChangedHandler serverListSizeChangedEvent = ServerListSizeChangedEvent;
            if (serverListSizeChangedEvent != null)
            {
                serverListSizeChangedEvent();
            }
        }

        public void OnJoinRequestListSizeChangedEvent()
        {
            JoinRequestListSizeChangedHandler joinReqeustListSizeChangedEvent = JoinRequestListSizeChangedEvent;
            if (joinReqeustListSizeChangedEvent != null)
            {
                joinReqeustListSizeChangedEvent();
            }
        }

        public void UpdateServerListStarter()
        {
            new System.Threading.Thread(UpdateServerList).Start();
        }

        private void UpdateServerList()
        {
            _servers = new List<string>();
            if (_client is IRCClient)
            {
                ((IRCClient)_client).SendCommandToChannel("names");
                if (((IRCClient)_client).People == null)
                {
                    return;
                }
                foreach (string nick in ((IRCClient)_client).People)
                {
                    if (nick != ((IRCClient)_client).MyNick)
                    {
                        SetEnemyNick(nick);
                        SendIsServer();
                    }
                }
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