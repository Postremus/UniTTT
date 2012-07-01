using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniTTT.Logik.Fields;

namespace UniTTT.Logik
{
    public static class FieldHelper
    {
        public enum GameStates
        {
            Gewonnen,
            Verloren,
            Unentschieden,
            Laufend
        }

        public static int GetFullFields(Fields.Field field)
        {
            int count = 0;
            for (int i = 0; i < field.Length; i++)
            {
                if (!field.IsFieldEmpty(i))
                    count++;
            }
            return count;
        }

        public static List<char> GetAllPlayerSymbols(Fields.Field field)
        {
            List<char> ret = new List<char>();
            for (int i = 0; i < field.Length; i++)
            {
                if (!ret.Contains(field.GetField(i)))
                {
                    ret.Add(field.GetField(i));
                }
            }
            return ret;
        }

        public static bool HasEmptyFields(Fields.Field field)
        {
            for (int i = 0; i < field.Length; i++)
            {
                if (field.IsFieldEmpty(i))
                    return true;
            }
            return false;
        }

        public static FieldHelper.GameStates GetGameState(Fields.Field field, Player.Player currentPlayer, Player.Player player1)
        {
            if (currentPlayer == null)
            {
                return GameStates.Laufend;
            }
            if (WinChecker.Pruefe(currentPlayer.Symbol, field))
            {
                if (currentPlayer == player1)
                {
                    return GameStates.Gewonnen;
                }
                else
                {
                    return GameStates.Verloren;
                }
            }
            else if (!FieldHelper.HasEmptyFields(field))
            {
                return GameStates.Unentschieden;
            }
            return GameStates.Laufend;
        }

        public static string GetString(Fields.Field field)
        {
            string ret = null;
            for (int i = 0; i < field.Length; i++)
            {
                ret += field.GetField(i);
            }
            return ret;
        }

        public static Fields.Field GetRandomField(Fields.Field field)
        {
            char player = '2';
            for (int i = 0; i < field.Length; i++)
            {
                field.SetField(GetRandomZug(field), player);
                player = Player.Player.PlayerChange(player);
            }
            return field;
        }

        private static Random _rnd = new Random();

        public static int GetRandomZug(Fields.Field field)
        {
            int zug = -1;
            do
            {
                zug = _rnd.Next(0, field.Length);
            } while (!field.IsFieldEmpty(zug));
            return zug;
        }
    }
}