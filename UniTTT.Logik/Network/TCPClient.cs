using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace UniTTT.Logik.Network
{
    public class TCPClient : Network
    {
        public TCPClient(string host, int port)
        {
            Hostname = host;
            TargetPort = port;
            Client = new System.Net.Sockets.TcpClient(host, port);
            ClientStream = Client.GetStream();
            Writer = new StreamWriter(ClientStream);
            Reader = new StreamReader(ClientStream);
            new Thread(ReceiveMessages).Start();
        }
    }
}
