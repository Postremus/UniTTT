using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace UniTTT.Logik.Network
{
    public class TCPClient : INetwork
    {
        #region privates
        private TcpClient _client;
        private string _hostname;
        private int _port;
        private Stream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;
        #endregion

        public event NewMassageReceivedHandler NewMassegeReceivedEvent;

        #region Propertys
        public Stream sTream { get { return _stream; } set { _stream = value; } }
        public StreamReader Reader { get { return _reader; } set { _reader = value; } }
        public StreamWriter Writer { get { return _writer; } set { _writer = value; } }
        public TcpClient Client { get { return _client; } set { _client = value; } }
        public string Hostname { get { return _hostname; } set { _hostname = value; } }
        public int Port { get { return _port; } set { _port = value; } }
        #endregion

        public TCPClient(string ip, int port = 5500)
        {
            Hostname = ip;
            Port = port;

            Client = new TcpClient();
            Client.Connect(ip, port);
            sTream = Client.GetStream();
            Writer = new StreamWriter(sTream);
            Reader = new StreamReader(sTream);
        }

        public virtual void Send(string message)
        {
            if (!Client.Connected)
            {
                Client = new TcpClient();
                Client.Connect(Hostname, Port);
            }
            Writer.WriteLine(message);
            Writer.Flush();
        }

        public virtual void Receive()
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
