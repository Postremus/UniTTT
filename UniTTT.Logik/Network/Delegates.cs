using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Network
{
    public delegate void NewMessageReceivedHandler(string str);
    public delegate void NewVector2iReceivedHandler(Vector2i vect);
    public delegate void NewFieldReceivedHandler(Fields.IField field);
    public delegate void NewGameRequestedHandler();
    public delegate void NewGameRequestReceived();
    public delegate void NewPortReceivedHandler(int port);
    public delegate void ConnectionSuccessHandler();
    public delegate void PlayerMovedHandler(Vector2i vect);
    public delegate void PlayerChangeHandler();
    public delegate void WindowTitleChangeHandler(string title);
    public delegate void PlayerOutputHandler(string message);
    public delegate void WinMessageHandler(char symbol, FieldHelper.GameStates gameState);
}
