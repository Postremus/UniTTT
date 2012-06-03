using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Network
{
    public delegate void NewMessageReceivedHandler(string str);
    public delegate void NewGameRequestedHandler();
    public delegate void NewGameRequestReceived();
}
