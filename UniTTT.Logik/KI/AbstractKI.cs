using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.KI
{
    public class AbstractKI
    {
        protected AbstractKI(char kispieler, int width, int height)
        {
            KIPlayer = kispieler;
            Width = width;
            Height = height;
        }

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Length { get { return Width * Height; } }
        public char KIPlayer { get; private set; }

        public override string ToString()
        {
            return "KI";
        }
    }
}