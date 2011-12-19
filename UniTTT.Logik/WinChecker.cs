using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public static class WinChecker
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

        public static bool Pruefe(char spieler, Fields.IField field)
        {
            GewinnBedingung = field.Width > field.Height ? field.Width : field.Height;
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
            if (DoCheck(field, Directories.RightDown, spieler, new Vector2i(0, 0), new Vector2i(field.Width, field.Height)) == GewinnBedingung)
                return true;

            // Oben Rechts zu unten Links
            if (DoCheck(field, Directories.LeftDown, spieler, new Vector2i(field.Width - 1, 0), new Vector2i(0, field.Height)) == GewinnBedingung)
                return true;
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
        private static int DoCheck(Fields.IField field, Directories dir, char spieler, Vector2i from, Vector2i to)
        {
            int counter = 0;
            for (int a = 0; a < GewinnBedingung; a++)
            {
                if (field.GetField(from) == spieler)
                    counter++;
                else break;
                from = NextField(dir, from);
            }
            //while (from != to)
            //{
            //    if (field.GetField(from) == spieler)
            //        counter++;
            //    else break;
            //    from = NextField(dir, from);
            //}
            return counter;
        }

        private static Vector2i NextField(Directories dir, Vector2i vect)
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