using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public static class WinChecker
    {
        private static int _gewinnbeingung = -1;
        #region Fields
        public static int GewinnBedingung { get { return _gewinnbeingung; } set { _gewinnbeingung = value; } }
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

        public static bool Pruefe(char player, Fields.Field field)
        {
            if (GewinnBedingung == -1)
            {
                GewinnBedingung = field.Width > field.Height ? field.Height : field.Width;
            }

            List<Fields.FieldRegion> panels = field.FieldRegions;

            foreach (Fields.FieldRegion region in panels)
            {
                if (region.Count<char>(player) == GewinnBedingung)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field">Das Spielfeld</param>
        /// <param name="dir">Die Richtung, in die überprüft werden soll.</param>
        /// <param name="player"></param>
        /// <param name="from">Der inklusive untere Vector der Startposition.</param>
        /// <returns></returns>
        public static int DoCheck(Fields.Field field, Directories dir, char player, Vector2i from)
        {
            int counter = 0;
            for (int a = 0; a < GewinnBedingung; a++)
            {
                if (field.GetField(from) == player)
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