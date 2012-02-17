using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace UniTTT.Logik.Network
{
    public class TCPServer : INetwork
    {
        #region privates
        private TcpListener listener;
        private TcpClient client;
        private int port;
        #endregion


        public TCPServer(int port=5500)
        {
            this.port = port;

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            client = listener.AcceptTcpClient();
        }

        public void Send(string message)
        {
            if (!client.Connected)
            {
                client = listener.AcceptTcpClient();
            }
            Stream s = client.GetStream();
            s.Write(message.GetBytes(), 0, message.Length);
            s.Flush();
            s.Close();
        }

        public void Receive()
        {
            if (!client.Connected)
            {
                client = listener.AcceptTcpClient();
            }
            throw new NotImplementedException();
        }

        int Port
        {
            get { throw new NotImplementedException(); }
        }

        public event NewMassageReceivedHandler NewMassegeReceivedEvent;

        public void OnNewMassageReceivedEvent(string value)
        {
            NewMassageReceivedHandler newMassageReceivedEvent = NewMassegeReceivedEvent;
            if (newMassageReceivedEvent != null)
            {
                NewMassegeReceivedEvent(value);
            }
        }
    }
}
