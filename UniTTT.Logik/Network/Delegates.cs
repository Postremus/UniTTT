﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Network
{
    public delegate void NewMessageReceivedHandler(string str);
    public delegate void NewVector2iReceivedHandler(Vector2i vect);
    public delegate void NewFieldReceivedHandler(Fields.IField field);
    public delegate void NewGameStartedHandler();
    public delegate void NewPortReceivedHandler(int port);
    public delegate void ConnectionSuccessHandler();
    public delegate void PlayerMovedHandler(Vector2i vect);
    public delegate void PlayerChangeHandler();

}
