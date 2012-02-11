using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace UniTTT.Logik.Network
{
    public class TCPServer
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

        public void Send(object obj)
        {
            if (!client.Connected)
            {
                client = listener.AcceptTcpClient();
            }
            Stream s = client.GetStream();
            s.Write(obj.GetBytes(), 0, obj.GetBytes().Length);
            s.Flush();
            s.Close();
        }


        public object Receive()
        {
            if (!client.Connected)
            {
                client = listener.AcceptTcpClient();
            }
            byte[] buffer = new byte[100000];
            Stream s = client.GetStream();
            s.Read(buffer, 0, buffer.Length);
            s.Flush();
            s.Close();
            return buffer.GetObject();
        }
    }
}
