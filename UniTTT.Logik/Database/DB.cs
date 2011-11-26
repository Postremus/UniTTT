using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Globalization;

namespace UniTTT.Logik.Database
{//73
    static class DB
    {
        /// <summary>
        /// Funktion zur Umwandlung eines Sit_codes in Like Form.
        /// </summary>
        /// <param name="sit_code">Der Sit_code, der in der Db gesucht werden soll.</param>
        /// <param name="felder_anzahl">Die Größe des Sit_Codes</param>
        /// <returns></returns>
        public static string ToDBLike(string sit_code, int felder_anzahl)
        {
            string ret = null;
            foreach (char field in sit_code)
                ret += field == '1' ? '_' : field;

            if (ret[0] == '_')
            {
                do
                {
                    ret.Remove(1);
                } while (ret[1] == '_');
            }
            if (ret[ret.Length - 1] == '_')
            {
                do
                {
                    ret.Remove(ret.Length - 2);
                } while (ret[ret.Length - 2] == '_');
            }
            return ret;
        }

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

        public static List<int> Like(List<string> elements, string mom_sit_code)
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
                        if (elements[x][i] != mom_sit_code[i] && insert)
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
    }
}