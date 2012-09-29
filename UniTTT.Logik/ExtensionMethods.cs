using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace UniTTT.Logik
{
    public static class ExtensionMethods
    {
        public static List<string> GetSubstrs(this string value)
        {
            return new List<string>(value.Split(' '));
        }

        public static int GetHighestIndex<T>(this T list) where T : IList
        {
            int ret = 0;
            int tmp = 0;
            int indexer = 0;
            foreach (int i in list)
            {
                if (i > tmp)
                {
                    ret = indexer;
                }
                indexer++;
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
                throw new ArgumentException("value or str1 isn't seted.", "value or str1");
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

        public static int IndexOfLastChar(this string value, string search)
        {
            if (value == null || search == null)
            {
                throw new ArgumentException("value or search isn't seted.", "value or search");
            }

            if (!value.Contains(search))
            {
                return -1;
            }

            int first = value.IndexOf(search);
            return first + search.Length;
        }
    }
}