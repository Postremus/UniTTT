using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public static class BrettHelper
    {
        public enum GameStates
        {
            Gewonnen,
            Unentschieden,
            Laufend
        }

        public static int GetFullFields(char[,] brett)
        {
            int count = 0;
            foreach (char field in brett)
                if (field != ' ')
                    count++;
            return count;
        }

        public static List<int> GetBrettDimensions(char[,] brett)
        {
            List<int> dimensions = new List<int>();
            dimensions.Add(brett.GetUpperBound(0));
            dimensions.Add(brett.GetUpperBound(1));
            return dimensions;
        }

        public static List<char> GetAllPlayerSymbols(char[,] brett)
        {
            List<char> playersymbols = new List<char>();
            foreach (char field in brett)
                if ((!playersymbols.Contains(field)) && (field != ' '))
                    playersymbols.Add(field);
            return playersymbols;
        }

        public static bool HasEmptyFields(char[,] brett)
        {
            foreach (char field in brett)
                if (field == ' ')
                    return true;
            return false;
        }

        public static Vector ZugToVector(int zug, int breite, int hoehe)
        {
            Vector vect = null;
            for (int x = 0; x < breite; x++)
            {
                for (int y = 0; y < hoehe; y++)
                {
                    if (((x) * 3) + (y + 1) - 1 == zug)
                        vect = new Vector(x, y);
                }
            }
            return vect;
        }
    }
}
