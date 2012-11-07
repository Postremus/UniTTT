﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public static class WinChecker
    {
        private static int _winCondition = -1;
        #region Fields
        public static int WinCondition { get { return _winCondition; } set { _winCondition = value; } }
        #endregion

        private enum Directories
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
            if (WinCondition == -1)
            {
                WinCondition = field.Width > field.Height ? field.Height : field.Width;
            }

            List<Fields.FieldRegion> panels = field.FieldRegions;

            return panels.Count(f => f.Count<char>(player) == WinCondition) > 0;
        }

        /// <summary>
        /// Prüft von einer angebenen Position aus, ob der Spieler in der angegebenen Richtung gewonnen hat.
        /// </summary>
        /// <param name="field">Das Spielfeld</param>
        /// <param name="dir">Die Richtung, in die überprüft werden soll.</param>
        /// <param name="player">Der Spieler auf den geprüft wird.</param>
        /// <param name="from">Der inklusive untere Vector der Startposition.</param>
        /// <returns></returns>
        private static int DoCheck(Fields.Field field, Directories dir, char player, Vector2i from)
        {
            int counter = 0;
            for (int a = 0; a < WinCondition; a++)
            {
                if (field.GetField(from) == player)
                    counter++;
                else break;
                from = NextField(dir, from);
            }
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