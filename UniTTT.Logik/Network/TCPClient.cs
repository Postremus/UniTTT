using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace UniTTT.Logik.Network
{
    public class TCPClient
    {
        #region privates
        private IPAddress connectTo;
        private TcpClient client;
        private string ip;
        private int port;
        #endregion

        public TCPClient(string ip, int port=5500)
        {
            this.ip = ip;
            this.port = port;

            connectTo = System.Net.IPAddress.Parse(ip);
            client = new TcpClient();
            client.Connect(connectTo, port);
        }

        public void Send(object obj)
        {
            if (!client.Connected)
            {
                client = new TcpClient();
                client.Connect(ip, port);
            }
            Stream s = client.GetStream();
            s.Write(obj.GetBytes(), 0, obj.GetBytes().Length);
            s.Flush();
            s.Close();
        }

        public Vector2i Receive()
        {
            if (!client.Connected)
            {
                client = new TcpClient();
                client.Connect(ip, port);
            }
            Stream s = client.GetStream();
            byte[] buffer = new byte[100000];
            s.Read(buffer, 0, buffer.Length);
            s.Flush();
            s.Close();
            return (Vector2i)buffer.GetObject();
        }
    }
}
