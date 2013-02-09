﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace UniTTT.Logik.Network
{
    public abstract class Network
    {
        #region Privates
        private TcpClient _client;
        private NetworkStream _clientStream;
        private StreamReader _reader;
        private StreamWriter _writer;
        private string _hostname;
        private int _targetPort;
        private string _myNick;
        private Thread _receiverThread;
        #endregion

        #region Propertys
        public event NewMessageReceivedHandler NewMessageReceivedEvent;
        public event NewNetworkMessageReceivedHandler NewNetworkMessageReceivedEvent;
        public TcpClient Client { get { return _client; } set { _client = value; } }
        public NetworkStream ClientStream { get { return _clientStream; } set { _clientStream = value; } }
        public StreamReader Reader { get { return _reader; } set { _reader = value; } }
        public StreamWriter Writer { get { return _writer; } set { _writer = value; } }
        public string Hostname { get { return _hostname; } set { _hostname = value; } }
        public int TargetPort { get { return _targetPort; } set { _targetPort = value; } }
        public string MyNick { get { return _myNick; } set { _myNick = value; } }
        public string SourceHost
        {
            get
            {
                string str = ((IPEndPoint)Client.Client.LocalEndPoint).ToString();
                return str.Substring(0, str.IndexOf(":"));
            }
        }
        public int SourcePort
        {
            get
            {
                string str = ((IPEndPoint)Client.Client.LocalEndPoint).ToString();
                return int.Parse(str.Substring(str.IndexOf(':')));
            }
        }
        #endregion

        public Network()
        {
            NewMessageReceivedEvent += MakeNetworkMessage;
        }

        public virtual void OnNewMassageReceivedEvent(string value)
        {
            NewMessageReceivedHandler newMessageReceived = NewMessageReceivedEvent;
            if (newMessageReceived != null)
            {
                newMessageReceived(value);
            }
        }

        public virtual void Send(string message)
        {
            Writer.WriteLine(message);
            Writer.Flush();
        }

        public void SendTo(string message, string nick)
        {
            Send(string.Format("{0}: {1}", nick, message));
        }

        public virtual void ReceiveMessages()
        {
            string str = null;
            do
            {
                if (str != null)
                {
                    OnNewMassageReceivedEvent(str);
                    /*if (!File.Exists("netlog.txt"))
                    {
                        File.Create("netlog.txt").Close();
                    }
                    File.AppendAllText("netlog.txt", str + Environment.NewLine);*/
                }
                str = Reader.ReadLine();
            } while (true);
        }

        public virtual void Connect()
        {
            if (_receiverThread == null)
            {
                _receiverThread = new Thread(ReceiveMessages);
                _receiverThread.IsBackground = true;
            }
            if (_receiverThread.ThreadState != ThreadState.Background)
            {
                _receiverThread.Start();
            }
        }

        private void MakeNetworkMessage(string message)
        {
            OnNewNetworkMessageReceivedevent(NetworkMessage.ParseMessage(message));
        }

        public void OnNewNetworkMessageReceivedevent(NetworkMessage received)
        {
            NewNetworkMessageReceivedHandler newNetworkMessageReceived = NewNetworkMessageReceivedEvent;
            if (newNetworkMessageReceived != null)
            {
                newNetworkMessageReceived(received);
            }
        }
    }
}