using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public static class SitCodeHelper
    {    
        
        public static char PlayertoSitCode(char value)
        {
            value = value.ToString().ToLower()[0];
            return value == 'x' ? '2' : value == 'o' ? '3' : value == ' ' ? '1' : value;
        }

        public static char ToPlayer(char value)
        {
            return value == '2' ? 'X' : value == '3' ? 'O' : value == '1' ? ' ' : value;
        }

        public static char PlayerChange(char curr)
        {
            return curr == '2' ? '3' : '2';
        }

        public static string GetEmpty(int length)
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
    }
}