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
    public class TCPClient : Network
    {
        public TCPClient(string ip, int port)
        {
            Hostname = ip;
            TargetPort = port;

            Client = new TcpClient(ip, port);

            sTream = Client.GetStream();
            Writer = new StreamWriter(sTream);
            Reader = new StreamReader(sTream);
            new Thread(Receive).Start();
        }
    }
}