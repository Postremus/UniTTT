using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace UniTTT.Logik.Network
{
    public class HolePuncher
    {
        private string nick;
        private string otherNick;
        private Network client;
        private IRCClient irc;
        private TCPServer server;

        public event NewPortReceivedHandler NewPortReceivedEvent;
        public event ConnectionSuccessHandler ConnectionSucessEvent;

        public HolePuncher(Network client, string nick, string otherNick)
        {
            this.nick = nick;
            this.otherNick = otherNick;
            this.client = client;

            irc = new IRCClient("wolfe.freenode.net", 6665, "#UniTTT", nick);
            irc.NewMessageReceivedEvent += ReceiveConnectionRequest;
        }

        public Network Punche()
        {
            Stopwatch st = new Stopwatch();
            irc.SendCommand("NAMES");
            st.Start();
            do
            {
                if (st.ElapsedMilliseconds % 1000 == 0 || irc.People == null)
                {
                    irc.SendCommand("NAMES");
                }
            } while (!irc.People.Contains(otherNick));
            st.Stop();



            client.Client.Connect(client.Hostname, client.SourcePort);
            irc.Send(string.Format("ConnectToPort:{0}", client.SourcePort));
            return client;
        }

        public void ReceiveConnectionRequest(string message)
        {
            if (message.Contains(string.Format("ConnectToPort:")))
            {
                int port = int.Parse(message.Substring(message.IndexOf(':')));
                OnNewPortReceivedEvent(port);
            }
        }

        public void OnNewPortReceivedEvent(int port)
        {
            NewPortReceivedHandler newPortReceived = NewPortReceivedEvent;
            if (newPortReceived != null)
            {
                newPortReceived(port);
            }
        }

        public void CreateServer(int port)
        {
            server = new TCPServer(client.Hostname, client.SourcePort, nick, otherNick, false);
            server.NewMessageReceivedEvent += ReceiveConnectionSuccess;
        }

        public void ReceiveConnectionSuccess(string message)
        {
            if (message.Contains("ConnectionSuccess"))
            {
                OnConnectionSuccess();
            }
        }

        public void OnConnectionSuccess()
        {
            ConnectionSuccessHandler connectionSuccesEvent = ConnectionSucessEvent;
            if (connectionSuccesEvent != null)
            {
                connectionSuccesEvent();
            }
        }
    }
}
