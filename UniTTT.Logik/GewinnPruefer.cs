using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public static class GewinnPruefer
    {
        #region Fields
        private static int GewinnBedingung;
        #endregion

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

        public static bool Pruefe(char spieler, Fields.IField Field)
        {
            GewinnBedingung = Field.Width > Field.Height ? Field.Width : Field.Height;
            //Horizontal
            for (int y = 0; y < Field.Height; y++)
                if (DoCheck(Field, Directories.Right, spieler, new Vector2i(0, y), new Vector2i(Field.Width - 1, y)) == GewinnBedingung)
                    return true;

            //Vertikal
            for (int x = 0; x < Field.Width; x++)
                if (DoCheck(Field, Directories.Down, spieler, new Vector2i(x, 0), new Vector2i(x, Field.Height - 1)) == GewinnBedingung)
                    return true;

            // Diagonal
            // Oben Links zu unten Rechts
            if (DoCheck(Field, Directories.RightDown, spieler, new Vector2i(0, 0), new Vector2i(Field.Width - 1, Field.Height - 1)) == GewinnBedingung)
                return true;

            // Oben Rechts zu unten Links
            if (DoCheck(Field, Directories.LeftDown, spieler, new Vector2i(Field.Width - 1, 0), new Vector2i(0, Field.Height - 1)) == GewinnBedingung)
                return true;
            return false;
        }

        public static int DoCheck(Fields.IField Field, Directories dir, char spieler, Vector2i from, Vector2i to)
        {
            int counter = 0;
            for (int a = 0; a < GewinnBedingung; a++)
            {
                if (Field.GetField(from) == spieler)
                    counter++;
                from = NextField(dir, from);
            }
            return counter;
        }

        public static Vector2i NextField(Directories dir, Vector2i vect)
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