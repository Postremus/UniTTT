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
            Unentschieden,
            Laufend
        }

        public static int GetFullFields(Fields.IField field)
        {
            int count = 0;
            for (int i = 0; i < field.Length; i++)
            {
                if (!field.IsFieldEmpty(i))
                    count++;
            }
            return count;
        }

        public static List<char> GetAllPlayerSymbols(Fields.IField field)
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

        public static bool HasEmptyFields(Fields.IField field)
        {
            for (int i = 0; i < field.Length; i++)
            {
                if (field.IsFieldEmpty(i))
                    return true;
            }
            return false;
        }

        public static FieldHelper.GameStates GetGameState(Fields.IField field, char spieler)
        {
            FieldHelper.GameStates state = FieldHelper.GameStates.Laufend;
            bool gewbl = WinChecker.Pruefe(spieler, field);

            if (gewbl)
                state = FieldHelper.GameStates.Gewonnen;
            if ((!gewbl) && (!FieldHelper.HasEmptyFields(field)))
                state = FieldHelper.GameStates.Unentschieden;
            return state;
        }

        public static string Calculate(Fields.IField field)
        {
            string ret = null;
            for (int i = 0; i < field.Length; i++)
            {
                ret += field.GetField(i);
            }
            return ret;
        }
    }
}
