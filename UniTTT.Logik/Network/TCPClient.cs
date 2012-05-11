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
            sTream = Client.GetStream();
            Writer = new StreamWriter(sTream);
            Reader = new StreamReader(sTream);
            new Thread(Receive).Start();
        }
    }
}
