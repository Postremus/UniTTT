using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe.Logik
{
    public class GewinnPrüfer
    {
        int GewinnBedingung;
        public GewinnPrüfer(int bedingung)
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
                if (DoCheck(brett, Directories.Right, spieler, new Vector(0, y), new Vector(width - 1, y)) == GewinnBedingung)
                    return true;

            //Vertikal
            for (int x = 0; x < width; x++)
                if (DoCheck(brett, Directories.Down, spieler, new Vector(x, 0), new Vector(x, heigth-1)) == GewinnBedingung)
                    return true;

            // Diagonal
            // Oben Links zu unten Rechts
            if (DoCheck(brett, Directories.RightDown, spieler, new Vector(0, 0), new Vector(width - 1, heigth - 1)) == GewinnBedingung)
                return true;

            // Oben Rechts zu unten Links
            if (DoCheck(brett, Directories.LeftDown, spieler, new Vector(width-1, 0), new Vector(0, heigth - 1)) == GewinnBedingung)
                return true;
            return false;
        }

        public int DoCheck(char[,] brett, Directories dir, char spieler, Vector vect, Vector to)
        {
            int counter = 0;
            for (int a = 0; a < GewinnBedingung; a++)
            {
                if (brett[vect.X, vect.Y] == spieler)
                    counter++;
                vect = NextField(dir, vect);
            }

            return counter;
        }

        public Vector NextField(Directories dir, Vector vect)
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