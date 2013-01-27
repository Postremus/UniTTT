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

        public NetworkPlayer(char symbol, ref Network.Network client) : base(symbol)
        {
            Client = client;
        }

        public override Vector2i Play(Fields.Field field)
        {
            while (!_newVector) { };
            _newVector = false;
            return _vect;
        }

        public void ReceiveVector(Network.NetworkMessage received)
        {
            if (!received.Content.Contains("UniTTT!Vector:"))
            {
                return;
            }
            string str = received.Content.Remove(0, received.Content.IndexOfLastChar("UniTTT!Vector:"));
            Vector2i vect = Vector2i.FromString(str, true);
            if (vect != null)
            {
                _vect = vect;
                _newVector = true;
            }
        }
    }
}
