using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public static class FieldHelper
    {
        public enum GameStates
        {
            Gewonnen,
            Unentschieden,
            Laufend
        }

        public static int GetFullFields(Fields.IField Field)
        {
            int count = 0;
            for (int i = 0; i < Field.Length; i++)
            {
                if (Field.IsFieldEmpty(i))
                    count++;
            }
            return count;
        }

        public static Vector2i GetBrettDimensions(char[,] brett)
        {
            Vector2i ret;
            int x = brett.GetUpperBound(0);
            int y = brett.GetUpperBound(1);
            ret = new Vector2i(x, y);
            return ret;
        }

        public static List<char> GetAllPlayerSymbols(char[,] brett)
        {
            List<char> playersymbols = new List<char>();
            foreach (char field in brett)
                if ((!playersymbols.Contains(field)) && (field != ' '))
                    playersymbols.Add(field);
            return playersymbols;
        }

        public static bool HasEmptyFields(Fields.IField Field)
        {
            for (int i = 0; i < Field.Length; i++)
            {
                if (Field.IsFieldEmpty(i))
                    return true;
            }
            return false;
        }

        public static FieldHelper.GameStates GetGameState(Fields.IField Field, char spieler)
        {
            FieldHelper.GameStates state = FieldHelper.GameStates.Laufend;
            bool gewbl = GewinnPruefer.Pruefe(spieler, Field);

            if (gewbl)
                state = FieldHelper.GameStates.Gewonnen;
            if ((!gewbl) && (!FieldHelper.HasEmptyFields(Field)))
                state = FieldHelper.GameStates.Unentschieden;
            return state;
        }
    }
}
