using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Network
{
    public delegate void NewMassageReceivedHandler(string str);
    public delegate void NewVector2iReceivedHandler(Vector2i vect);
    public delegate void NewFieldReceivedHandler(Fields.IField field);
    public delegate void NewGameStartedHandler();
}
