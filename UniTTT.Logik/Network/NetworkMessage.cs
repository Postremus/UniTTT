using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Network
{
    public class NetworkMessage
    {
        private DateTime _receivingTime;
        private string _transmitter;
        private string _receiver;
        private string _content;

        public DateTime ReceivingTime
        {
            get
            {
                return _receivingTime;
            }
        }

        public string Transmitter
        {
            get
            {
                return _transmitter;
            }
        }

        public string Receiver
        {
            get
            {
                return _receiver;
            }
        }

        public string Content
        {
            get
            {
                return _content;
            }
        }

        private NetworkMessage()
        {
        }

        public static NetworkMessage ParseMessage(string message)
        {
            NetworkMessage ret = new NetworkMessage();

            ret._receivingTime = DateTime.Now;

            //":Postremus!~Martin@p54AB2495.dip.t-dialin.net PRIVMSG #unittt-play :postremus123456: UniTTT!CanJoinAnswer!true"

            //irc Nachricht behandlung

            if (message.Split(' ').First().Contains('!'))
            {
                ret._transmitter = message.SubStringBetween(":", "!");
            }

            message = message.Replace(": ", ":");
            message = message.Split(' ').Last();
            if (message.StartsWith(":"))
            {
                message = message.Remove(0, 1);
            }

            int idx = message.IndexOf(":");
            if (idx != -1)
            {
                ret._receiver = message.Substring(0, idx);
                ret._content = message.Substring(idx + 1);
            }
            else
            {
                ret._content = message;
            }
            return ret;
        }
    }
}
