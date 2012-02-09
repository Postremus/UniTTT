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

        public static byte[] GetBytes(this object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public static object GetObject(this byte[] value)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(value, 0, value.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }
    }
}