using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public class SitCodeHelper
    {
        public static string Calculate(Fields.IField field)
        {
            string ret = null;
            char c;
            for (int i = 0; i < field.Length; i++)
            {
                c = field.GetField(i);
                c = c == 'X' ? '2' : c == 'O' ? '3' : c == ' ' ? '1' : c;
                ret += c;
            }
            return ret;
        }
         
        public static char PlayertoSitCode(char spieler)
        {
            return spieler == 'X' ? '2' : spieler == 'O' ? '3' : spieler == ' ' ? '1' : spieler;
        }

        public static char ToPlayer(char spieler)
        {
            return spieler == '2' ? 'X' : spieler == '3' ? 'O' : spieler == '1' ? ' ' : spieler;
        }

        public static string SetEmpty(int felderAnzahl)
        {
            string sit_code = string.Empty;
            while (sit_code.Length < felderAnzahl)
                sit_code += "1";
            return sit_code;
        }
    }
}
