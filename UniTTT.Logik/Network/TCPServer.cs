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
    public class TCPServer : Network
    {
        #region Privates
        private TcpListener listener;
        #endregion

        public TCPServer(string host, int port)
        {
            Hostname = host;
            TargetPort = port;

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Client = listener.AcceptTcpClient();

            ClientStream = Client.GetStream();
            Reader = new StreamReader(ClientStream);
            Writer = new StreamWriter(ClientStream);
            new Thread(ReceiveMessages).Start();
        }
    }
}
