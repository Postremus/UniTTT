using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public static class WinChecker
    {
        private static int _gewinnbedungung = -1;
        #region Fields
        public static int GewinnBedingung { get { return _gewinnbedungung; } set { _gewinnbedungung = value; } }
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

        public static bool Pruefe(char spieler, Fields.IField field)
        {
            if (GewinnBedingung == -1)
            {
                GewinnBedingung = field.Width > field.Height ? field.Height : field.Width;
            }

            //Horizontal
            for (int y = 0; y < field.Height; y++)
                if (DoCheck(field, Directories.Right, spieler, new Vector2i(0, y), new Vector2i(field.Width, y)) == GewinnBedingung)
                    return true;

            //Vertikal
            for (int x = 0; x < field.Width; x++)
                if (DoCheck(field, Directories.Down, spieler, new Vector2i(x, 0), new Vector2i(x, field.Height)) == GewinnBedingung)
                    return true;

            // Diagonal
            // Oben Links zu unten Rechts
            for (int x = 0; x < field.Width; x++)
            {
                for (int y = 0; y < field.Height; y++)
                {
                    if (x + (GewinnBedingung - 1) < field.Width && y + (GewinnBedingung - 1) < field.Height)
                    {
                        if (DoCheck(field, Directories.RightDown, spieler, new Vector2i(x, y), new Vector2i(x + (field.Width - 1), y + (field.Height - 1))) == GewinnBedingung)
                            return true;
                    }
                }
            }

            // Oben Rechts zu unten Links
            for (int x = 0; x < field.Width; x++)
            {
                for (int y = 0; y < field.Height; y++)
                {
                    if ((x - GewinnBedingung >= 0) && y - GewinnBedingung >= 0)
                    {
                        if (DoCheck(field, Directories.LeftDown, spieler, new Vector2i(x, y), new Vector2i(x + (field.Height - 1), y + (field.Height - 1))) == GewinnBedingung)
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// TODO: verbessern
        /// </summary>
        /// <param name="field">Das Spielfeld</param>
        /// <param name="dir">Die Richtung, in die überprüft werden soll.</param>
        /// <param name="spieler"></param>
        /// <param name="from">Der inklusive untere Vector der Startposition.</param>
        /// <param name="to">Der inklusive untere Vector der Endposition. (Unwichtig, null reicht auch)</param>
        /// <returns></returns>
        public static int DoCheck(Fields.IField field, Directories dir, char spieler, Vector2i from, Vector2i to)
        {
            int counter = 0;
            for (int a = 0; a < GewinnBedingung; a++)
            {
                if (field.GetField(from) == spieler)
                    counter++;
                else break;
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