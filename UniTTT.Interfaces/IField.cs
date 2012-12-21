using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniTTT.Logik;

namespace UniTTT.Interfaces
{
    public interface IField
    {
        int Width { get; }
        int Height { get; }

        char GetField(int idx);
        char GetField(Vector2i vect);
        void SetField(int idx, char value);
        void SetField(Vector2i vect, char value);
    }
}
