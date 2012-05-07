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

        public TCPServer(string ip, int port)
        {
            Hostname = ip;
            TargetPort = port;

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Client = listener.AcceptTcpClient();

            sTream = Client.GetStream();
            Reader = new StreamReader(sTream);
            Writer = new StreamWriter(sTream);
            new Thread(Receive).Start();
        }
    }
}
