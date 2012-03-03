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
    public class TCPServer : INetwork
    {
        #region privates
        private TcpListener listener;
        private TcpClient client;
        private int port;
        private string ip;
        private Stream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;
        #endregion

        public event NewMassageReceivedHandler NewMassegeReceivedEvent;

        #region Propertys
        public Stream sTream { get { return _stream; } set { _stream = value; } }
        public StreamReader Reader { get { return _reader; } set { _reader = value; } }
        public StreamWriter Writer { get { return _writer; } set { _writer = value; } }
        #endregion

        public TCPServer(string ip, int port)
        {
            this.ip = ip;
            this.port = port;

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            client = listener.AcceptTcpClient();
            sTream = client.GetStream();
            Reader = new StreamReader(sTream);
            Writer = new StreamWriter(sTream);
        }

        public void Send(string message)
        {
            Writer.Write(message);
            Writer.Flush();
        }

        public void Receive()
        {
            string str = null;
            do
            {
                if (str != null)
                {
                    OnNewMassageReceivedEvent(str);
                }
                str = Reader.ReadLine();
            } while (true);
        }

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
