﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public class GewinnPruefer
    {
        #region Fields
        private int GewinnBedingung;
        #endregion
        public GewinnPruefer(int bedingung)
        {
            GewinnBedingung = bedingung;
        }

        public enum Directories
        {
            Up,
            Down,
            Left,
            Right,
            LeftUp,
            LeftDown,
            RightUp,
            RightDown
        }

        public bool Pruefe(char spieler, char[,] brett)
        {
            int width = brett.GetUpperBound(0)+1;
            int heigth = brett.GetUpperBound(1)+1;
            //Horizontal
            for (int y = 0; y < heigth; y++)
                if (DoCheck(brett, Directories.Right, spieler, new Vector2i(0, y), new Vector2i(width - 1, y)) == GewinnBedingung)
                    return true;

            //Vertikal
            for (int x = 0; x < width; x++)
                if (DoCheck(brett, Directories.Down, spieler, new Vector2i(x, 0), new Vector2i(x, heigth-1)) == GewinnBedingung)
                    return true;

            // Diagonal
            // Oben Links zu unten Rechts
            if (DoCheck(brett, Directories.RightDown, spieler, new Vector2i(0, 0), new Vector2i(width - 1, heigth - 1)) == GewinnBedingung)
                return true;

            // Oben Rechts zu unten Links
            if (DoCheck(brett, Directories.LeftDown, spieler, new Vector2i(width-1, 0), new Vector2i(0, heigth - 1)) == GewinnBedingung)
                return true;
            return false;
        }

        public int DoCheck(char[,] brett, Directories dir, char spieler, Vector2i from, Vector2i to)
        {
            int counter = 0;
            for (int a = 0; a < GewinnBedingung; a++)
            {
                if (brett[from.X, from.Y] == spieler)
                    counter++;
                from = NextField(dir, from);
            }
            return counter;
        }

        public Vector2i NextField(Directories dir, Vector2i vect)
        {
            if (dir == Directories.Right)
                vect.X++;
            else if (dir == Directories.Left)
                vect.X--;
            else if (dir == Directories.Up)
                vect.Y--;
            else if (dir == Directories.Down)
                vect.Y++;
            else if (dir == Directories.LeftUp)
            {
                vect.X--;
                vect.Y--;
            }
            else if (dir == Directories.LeftDown)
            {
                vect.X--;
                vect.Y++;
            }
            else if (dir == Directories.RightUp)
            {
                vect.X++;
                vect.Y--;
            }
            else if (dir == Directories.RightDown)
            {
                vect.X++;
                vect.Y++;
            }
            return vect;
        }
    }
}