using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace UniTTT.Logik.Player
{
    public class Player
    {
        public Player(char symbol) 
        {
            Symbol = symbol;
            WinCounter = 0;
        }

        public char Symbol { get; set; }
        public int WinCounter { get; set; }

        public virtual Vector2i Play(Fields.Field field)
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

        public static char PlayerChange(char curr)
        {
            return curr == 'X' ? 'O' : 'X';
        }

        public override string ToString()
        {
            return "Player";
        }
    }
}