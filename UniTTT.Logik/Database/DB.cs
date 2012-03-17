using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace UniTTT.Logik.Database
{
    static class DB
    {
        /// <summary>
        /// Fubktion zur Umwandlung eines Sit_codes in VB Like Form.
        /// </summary>
        /// <param name="sit_code">Der Sit_code, der in der Db gesucht werden soll.</param>
        /// <returns></returns>
        public static string ToVBLike(string sit_code)
        {
            string ret = null;
            foreach (char field in sit_code)
                ret += field == '1' ? '*' : field;

            return ret;
        }

        /// <summary>
        /// Gibt eine Liste mit den Indezes der ähnlichen Zeichenketten zurück.
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<int> Like(List<string> elements, string value)
        {
            List<int> ret = new List<int>();
            bool insert = true;
            for (int x = 0; x < elements.Count; x++)
			{
                insert = true;
                for (int i = 0; i < elements[x].Length; i++)
                {
                    if (elements[x][i] != ('_' | '*'))
                    {
                        if (elements[x][i] != value[i] && insert)
                            insert = false;
                    }
                }
                if (insert == true)
                {
                    ret.Add(x);
                }
            }

            return ret;
        }

        public static bool Like(string searchstr, string str2)
        {
            for (int i = 0; i < Math.Ceiling(((double)searchstr.Length + str2.Length)/2); i++)
            {
                if (searchstr[i] != '*')
                {
                    if (searchstr[i] != str2[i])
                    {
                        return false;
                    }
                }
            }
            return true; ;
        }
    }
}