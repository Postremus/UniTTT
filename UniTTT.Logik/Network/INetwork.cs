using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Network
{
    public interface INetwork
    {
        event NewMassageReceivedHandler NewMassegeReceivedEvent;
        void OnNewMassageReceivedEvent(string value);
        void Send(string message);
        void Receive();
    }
}
