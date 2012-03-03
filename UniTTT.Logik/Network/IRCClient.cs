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
    public class IRCClient : TCPClient
    {
        private string nick;
        private string user;
        private string channel;
        private string connectingFrom;

        public IRCClient(string server, int port, string channel, string nick) : base(server, port)
        {
            Hostname = server;
            Port = port;

            this.nick = nick;
            this.user = "UniTTT UniTTT UniTTT UniTTT";
            this.channel = channel;

            ConnectToChannel();
            NewMassegeReceivedEvent += SetConnectingFrom;
            NewMassegeReceivedEvent += Pong;
            new Thread(Receive).Start();
        }

        public void ConnectToChannel()
        {
            SendCommand("USER " + user);
            SendCommand("NICK " + nick);
            SendCommand("JOIN " + channel);
        }

        public void SendCommand(string message)
        {
            base.Send(message);
        }

        public override void Send(string message)
        {
            base.Send(string.Format("PRIVMSG {0} {1}", channel, message));
        }

        public void SetConnectingFrom(string message)
        {
            if (message.Contains("JOIN"))
            {
                message = message.Replace(":", null);
                connectingFrom = message.Substring(0, message.IndexOf(' '));
            }
        }

        public void Pong(string message)
        {
            if (message.Contains("PING"))
            {
                if (connectingFrom == null)
                {
                    SendCommand(string.Format("/whois {0}", nick));
                }
                else
                {
                    SendCommand(string.Format("PONG {0} {1}", connectingFrom, Hostname));
                }
            }
        }
    }
}