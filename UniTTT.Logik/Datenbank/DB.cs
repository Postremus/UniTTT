using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.Data.SQLite;
using System.Globalization;

namespace UniTTT.Datenbank
{
    static class DB
    {
        /// <summary>
        /// Fubktion zur Umwandlung eines Sit_codes in Like Form.
        /// </summary>
        /// <param name="sit_code">Der Sit_code, der in der Db gesucht werden soll.</param>
        /// <param name="felder_anzahl">Die Größe des Sit_Codes</param>
        /// <returns></returns>
        public static string ToDBLike(string sit_code, int felder_anzahl)
        {
            // 1. Durchgang: 1 zu _ und 1 zu %, ungeachtet deren Position
            // schleife

            string str = null;
            // 1. Durchgang: 1 zu _ und 1 zu %, ungeachtet deren Position
            // schleife
            for (int i = 0; i < felder_anzahl; i++)
                str += sit_code[i] == '1' ? '_' : sit_code[i];

            // Bei der ersten und Letzten Position ein % setzen, falls die leer sind.
            if (str[0] == '_')
                str = str.Remove(0, 1).Insert(0, "%");
            else if (str[str.Length - 1] == '_')
                str = str.Remove(str.Length - 1, 1).Insert(str.Length - 1, "%");

            // 2. Durchgang: Noch mal alles anpassen, dabei aber die Positionen beachten.
            // Beispiel: 
            // 2___3___% wäre flasch
            // 2___3% ist richtig
            int count = 0;
            bool modzeichen = false, spielerzeichen = false;
            for (int i = 0; i < felder_anzahl; i++)
            {
                if (str[i] != '%')
                {
                    if (str[i] == '_')
                        count++;
                    else
                    {
                        if (spielerzeichen)
                            count = 0;
                        else
                            spielerzeichen = true;
                    }
                }
                else
                    modzeichen = true;

                if ((spielerzeichen && modzeichen) || (modzeichen))
                    str = str.Remove(i - count, count);
            }

            // und jetzt denn verändertend string an die Methode zurückgeben, die diese Methode aufgerufen hat.
            return str;
        }

        /// <summary>
        /// Fubktion zur Umwandlung eines Sit_codes in VB Like Form.
        /// </summary>
        /// <param name="sit_code">Der Sit_code, der in der Db gesucht werden soll.</param>
        /// <returns></returns>
        public static string ToVBLike(string sit_code)
        {
            string str = null;
            foreach (char field in sit_code)
                str += field == '1' ? '*' : field;

            return str;
        }

        public static List<int> Like(List<string> elements, string mom_sit_code)
        {
            List<int> rt_lst = new List<int>();
            for (int i = 0; i < elements.Count; i++)
                if (LikeOperator.LikeString(elements[i], mom_sit_code, CompareMethod.Text))
                    rt_lst.Add(i);
            return rt_lst;
        }
    }
}