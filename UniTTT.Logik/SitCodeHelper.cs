using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public class SitCodeHelper
    {    
        public static char PlayertoSitCode(char value)
        {
            return value == 'X' ? '2' : value == 'O' ? '3' : value == ' ' ? '1' : value;
        }

        public static char ToPlayer(char value)
        {
            return value == '2' ? 'X' : value == '3' ? 'O' : value == '1' ? ' ' : value;
        }

        public static string SetEmpty(int length)
        {
            return new string('1', length);
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

        private static Random Rnd = new Random();

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