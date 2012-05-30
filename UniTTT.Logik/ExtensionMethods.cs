using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace UniTTT.Logik
{
    public static class ExtensionMethods
    {
        public static List<string> GetSubstrs(this string value)
        {
            return new List<string>(value.Split(' '));
        }

        public static int GetHighestIndex(this int[] value)
        {
            int ret = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] > value[ret])
                {
                    ret = i;
                }
            }
            return ret;
        }

        public static int GetHighestIndex(this int[,] value)
        {
            int ret = 0;
            int tmp = 0;
            for (int i = 0; i < value.GetUpperBound(0) + 1; i++)
            {
                for (int a = 0; a < value.GetUpperBound(1) + 1; a++)
                {
                    if (value[i, a] > tmp)
                    {
                        ret = (i + 1) * (a + 1) - 1;
                        tmp = value[i, a];
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Gibt die Zeichen zwischen str1 und str2 in value zurück. Sollte str2 null sein, wird der String von str1 bis zum Ende zurückgegeben.
        /// </summary>
        /// <param name="value">Der String, in dem gesucht wird.</param>
        /// <param name="str1">Die Anfangspositons als String in value. Muss gesetzt sein.</param>
        /// <param name="str2">Die Endposition als String in value</param>
        /// <returns></returns>
        public static string SubStringBetween(this string value, string str1, string str2)
        {
            if (value == null || str1 == null)
            {
                throw new NullReferenceException();
            }

            if (!value.Contains(str1))
            {
                return null;
            }

            int first = value.IndexOf(str1) + str1.Length;
            int length = 0;
            if (str2 == null)
            {
                length = value.Length - first;
            }
            else
            {
                length = value.Length - first - (value.Length - value.IndexOf(str2));
            }
            return value.Substring(first, length);
        }
    }
}