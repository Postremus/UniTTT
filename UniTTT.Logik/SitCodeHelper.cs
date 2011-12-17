using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public class SitCodeHelper
    {    
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

        public static string StringToSitCode(string var)
        {
            string ret = null;
            foreach (char x in var)
            {
                ret += PlayertoSitCode(x);
            }
            return ret;
        }

        public static char PlayerChange(char spieler)
        {
            return spieler == '2' ? '3' : '2';
        }

        public static Random Rnd = new Random();

        public static int GetRandomZug(string sitcode)
        {
            int zug = -1;
            do
            {
                zug = Rnd.Next(0, 9);
            } while (sitcode[zug] != '1');
            return zug;
        }
    }
}
