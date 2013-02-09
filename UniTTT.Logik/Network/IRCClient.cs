using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace UniTTT.Logik.Network
{
    public class IRCClient : Network
    {
        private string user;
        private string channel;
        private string connectingFrom;
        private int _peopleCount;
        private List<string> _people;
        private bool _isConnected;

        public int PeopleCount { get { return _peopleCount; } set { _peopleCount = value; } }
        public List<string> People { get { return _people; } set { _people = value; } }
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
        }

        public IRCClient(string host, int port, string channel, string myNick)
        {
            this.user = "UniTTT UniTTT UniTTT UniTTT";
            this.channel = channel;
            base.MyNick = myNick;

            Client = new TcpClient(host, port);

            ClientStream = Client.GetStream();
            Reader = new StreamReader(ClientStream);
            Writer = new StreamWriter(ClientStream);

            NewMessageReceivedEvent += SetConnectingFrom;
            NewMessageReceivedEvent += Pong;
            NewMessageReceivedEvent += CountPeople;
            NewMessageReceivedEvent += VisiblePeople;
        }

        public override void Connect()
        {
            base.Connect();
            ConnectToChannel();
        }

        private void ConnectToChannel()
        {
            SendCommand("USER " + user);
            SendCommand("NICK " + MyNick);
            SendCommand("Join " + channel);
        }

        public void SendCommand(string message)
        {
            base.Send(message);
        }

        public void SendCommandToChannel(string message)
        {
            base.Send(string.Format("{0} {1}", message, channel));
        }

        public override void Send(string message)
        {
            base.Send(string.Format("PRIVMSG {0} :{1}", channel, message));
        }

        private void CountPeople(string message)
        {
            if (message.Contains("353"))
            {
                string value = message.Substring(message.LastIndexOf(':'));
                PeopleCount = value.GetSubstrs().Count;
            }
        }

        private void VisiblePeople(string message)
        {
            if (message.Contains("353"))
            {
                string value = message.Substring(message.LastIndexOf(':')+1);
                
                List<string> tmp = value.Split(new char[]{ ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                People = new List<string>();
                foreach (string nick in tmp)
                {
                    if (nick.StartsWith("@"))
                    {
                        People.Add(nick.Substring(1));
                    }
                    else
                    {
                        People.Add(nick);
                    }
                }
                _isConnected = true;
            }
        }

        private void SetConnectingFrom(string message)
        {
            if (message.Contains("JOIN"))
            {
                message = message.Replace(":", null);
                if (connectingFrom == null || string.IsNullOrEmpty(connectingFrom.Trim()))
                {
                    connectingFrom = message.Substring(0, message.IndexOf(' '));
                }
            }
        }

        private void Pong(string message)
        {
            if (message.Contains("PING"))
            {
                if (connectingFrom == null)
                {
                    SendCommand(string.Format("/whois {0}", MyNick));
                }
                else
                {
                    SendCommand(string.Format("PONG {0} {1}", connectingFrom, Hostname));
                }
            }
        }
    }
}