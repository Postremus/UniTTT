using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Player
{
    public class NetworkPlayer : Player
    {
        private Network.Network _client;
        private bool _newVector;
        private Vector2i _vect;

        public Network.Network Client
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
            }
        }

        public NetworkPlayer(char symbol, Network.Network client) : base(symbol)
        {
            Client = client;
            Client.NewMessageReceivedEvent += ReceiveVector;
        }

        public override Vector2i Play(Fields.BaseField field)
        {
            while (!_newVector) { };
            _newVector = false;
            return _vect;
        }

        private void ReceiveVector(string value)
        {
            if (!value.Contains("UniTTT!Vector:"))
            {
                return;
            }
            string str = value.Remove(0, value.IndexOfLastChar("UniTTT!Vector:"));
            Vector2i vect = Vector2i.StringToVector(str, true);
            if (vect != null)
            {
                _vect = vect;
                _newVector = true;
            }
        }
    }
}
