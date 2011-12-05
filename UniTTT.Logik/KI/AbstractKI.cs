using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.KI
{
    public class AbstractKI
    {
        protected AbstractKI(char spieler, int width, int height)
        {
            Kiplayer = spieler;
            Width = width;
            Height = height;
            Rnd = new Random();
        }


        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int FelderAnzahl { get { return Width * Height; } }
        public char Kiplayer { get; private set; }
        public Random Rnd { get; private set; }

        public virtual void Learn()
        {
            throw new NotImplementedException();
        }

        public virtual int Play(Fields.IField field, char spieler)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "KI";
        }

        protected char PlayerChange(char spieler)
        {
            return spieler == '2' ? '3' : '2';
        }

        public int GetRandomZug(string sitcode)
        {
            int zug = -1;
            do
            {
                zug = Rnd.Next(0, 9);
            } while (sitcode[zug] != '1');
            return zug;
        }
    }
}