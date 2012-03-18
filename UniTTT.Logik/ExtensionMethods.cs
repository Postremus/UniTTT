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

        public static int GetHighestIndex(this int[,] value)
        {
            int ret = 0;
            int tmp = 0;
            for (int i = 0; i < value.GetUpperBound(0) + 1; i++)
            {
                for (int a = 0; a < value.GetUpperBound(1) + 1; a++)
                {
                    if (value[i, a] > tmp)
                    {
                        ret = (i + 1) * (a + 1) - 1;
                        tmp = value[i, a];
                    }
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

        public static byte[] GetBytes(this string value)
        {
            if (value == null)
                return null;
            int t;
            if (!int.TryParse(value, out t))
            {
                return null;
            }

            byte[] ret = new byte[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                ret[i] = (byte)value[i];
            }
            return ret;
        }

        public static object GetObject(this byte[] value)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(value, 0, value.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = null;
            try
            {
                obj = (Object)binForm.Deserialize(memStream);
            }
            catch (System.Runtime.Serialization.SerializationException)
            {

                
            }
            return obj;
        }

        public static object GetObject(this string value)
        {
            byte[] b = value.GetBytes();
            if (b == null)
            {
                return value;
            }
            return b.GetObject();
        }
    }
}