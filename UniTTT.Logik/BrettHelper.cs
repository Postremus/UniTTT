using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public class BrettHelper
    {
        public enum GameStates
        {
            Gewonnen,
            Unentschieden,
            Laufend
        }

        public int GetFullFields(char[,] brett)
        {
            int count = 0;
            foreach (char field in brett)
                if (field != ' ')
                    count++;
            return count;
        }

        public List<int> GetBrettDimensions(char[,] brett)
        {
            List<int> dimensions = new List<int>();
            dimensions.Add(brett.GetUpperBound(0));
            dimensions.Add(brett.GetUpperBound(1));
            return dimensions;
        }

        public List<char> GetAllPlayerSymbols(char[,] brett)
        {
            List<char> playersymbols = new List<char>();
            foreach (char field in brett)
                if ((!playersymbols.Contains(field)) && (field != ' '))
                    playersymbols.Add(field);
            return playersymbols;
        }

        public bool HasEmptyFields(char[,] brett)
        {
            foreach (char field in brett)
                if (field == ' ')
                    return true;
            return false;
        }
    }
}
