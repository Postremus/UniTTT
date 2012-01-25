using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UniTTT.Logik
{
    public static class ExtensionMethods
    {
        public static List<string> GetSubstrs(this string value)
        {
            List<string> ret = new List<string>();
            int lastidx = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == ' ')
                {
                    ret.Add(value.Substring(lastidx, i - lastidx));
                    lastidx = i;
                }
            }
            ret.Add(value.Substring(lastidx, value.Length - lastidx));
            return ret;
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
    }
}