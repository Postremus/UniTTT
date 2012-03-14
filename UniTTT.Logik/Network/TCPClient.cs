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
        public TCPClient(string ip, int port, string nick, string otherNick, bool allowHolePunching)
        {
            Hostname = ip;
            TargetPort = port;

            if (allowHolePunching)
            {
                try
                {
                    Client = new TcpClient(ip, port);
                    Writer = new StreamWriter(Client.GetStream());
                    base.Send("*klopf klopf*");
                }
                catch (SocketException)
                {
                    HolePuncher p = new HolePuncher(this, nick, otherNick);
                    p.Punche();
                }
            }
            else
            {
                Client = new TcpClient(ip, port);
            }

            sTream = Client.GetStream();
            Writer = new StreamWriter(sTream);
            Reader = new StreamReader(sTream);
            new Thread(Receive).Start();
        }
    }
}