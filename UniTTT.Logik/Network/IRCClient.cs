﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace UniTTT.Logik.Network
{
    public class IRCClient : TCPClient
    {
        private string nick;
        private string user;
        private string channel;
        private string connectingFrom;
        private int _peopleCount;
        private List<string> _people;

        public int PeopleCount { get { return _peopleCount; } set { _peopleCount = value; } }
        public List<string> People { get { return _people; } set { _people = value; } }

        public IRCClient(string server, int port, string channel, string nick) : base(server, port, null, null, false)
        {

            this.nick = nick;
            this.user = "UniTTT UniTTT UniTTT UniTTT";
            this.channel = channel;

            ConnectToChannel();
            NewMessageReceivedEvent += SetConnectingFrom;
            NewMessageReceivedEvent += Pong;
            NewMessageReceivedEvent += CountPeople;
            NewMessageReceivedEvent += VisiblePeople;
        }

        public void ConnectToChannel()
        {
            SendCommand("USER " + user);
            SendCommand("NICK " + nick);
            SendCommand("JOIN " + channel);
        }

        public void SendCommand(string message)
        {
            base.Send(message);
        }

        public override void Send(string message)
        {
            base.Send(string.Format("PRIVMSG {0} {1}", channel, message));
        }

        public void CountPeople(string message)
        {
            if (message.Contains("353"))
            {
                string value = message.Substring(message.LastIndexOf(':'));
                PeopleCount = value.GetSubstrs().Count;
            }
        }

        public void VisiblePeople(string message)
        {
            if (message.Contains("353"))
            {
                string value = message.Substring(message.LastIndexOf(':'));
                People = value.GetSubstrs();
            }
        }

        public void SetConnectingFrom(string message)
        {
            if (message.Contains("JOIN"))
            {
                message = message.Replace(":", null);
                connectingFrom = message.Substring(0, message.IndexOf(' '));
            }
        }

        public void Pong(string message)
        {
            if (message.Contains("PING"))
            {
                if (connectingFrom == null)
                {
                    SendCommand(string.Format("/whois {0}", nick));
                }
                else
                {
                    SendCommand(string.Format("PONG {0} {1}", connectingFrom, Hostname));
                }
            }
        }
    }
}