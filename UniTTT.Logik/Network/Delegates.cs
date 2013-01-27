using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Network
{
    public delegate void NewMessageReceivedHandler(string str);
    public delegate void NewNetworkMessageReceivedHandler(NetworkMessage received);
    public delegate void NewNetworkMessageForMeHandler(NetworkMessage received);
    public delegate void NewIsServerRequestReceived();
    public delegate void NewIsServerAnswerReceivedHandler(bool isServer);
    public delegate void CanJoinAnswerReceivedHandler(bool canJoin);
    public delegate void HandShakeAnswerReceivedHandler(bool correct);
    public delegate void NewGameRequestReceived();
}
