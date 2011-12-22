using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace UniTTT.Logik.Player
{
    public class AbstractPlayer
    {
        protected AbstractPlayer(char startspieler) 
        {
            Symbol = startspieler;
            WinCounter = 0;
        }

        public char Symbol { get; set; }
        public int WinCounter { get; set; }

        public virtual Vector2i Play(Fields.IField field)
        {
            throw new NotImplementedException();
        }

        public virtual void Learn()
        {
            throw new NotImplementedException();
        }

        //Ausgeben, wer momentan dran ist.
        public string Ausgabe()
        {
            return string.Format(CultureInfo.CurrentCulture, "Spieler {0} ist an der Reihe.", Symbol);
        }

        public override string ToString()
        {
            return "Player";
        }
    }
}