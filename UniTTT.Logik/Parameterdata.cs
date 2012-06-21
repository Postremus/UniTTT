using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public class Parameterdata
    {
        private Dictionary<string, object> dic;
        private List<string> _args;

        public int Count
        {
            get
            {
                return dic.Count;
            }
        }

        public List<string> Arguments
        {
            get
            {
                return _args;
            }
        }

        public Parameterdata()
        {
            dic = new Dictionary<string, object>();
            _args = new List<string>();
        }

        public void Add(string key, object value)
        {
            dic.Add(key, value);
            _args.Add(key);
        }

        /// <summary>
        /// Liest value von key. Rückgabewert ist im Fehlerfall default(T)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Der Paramter</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue<T>(string key, out T value)
        {
            value = default(T);
            if (!dic.ContainsKey(key))
            {
                return false;
            }
            try
            {
                value = (T)Convert.ChangeType(dic[key], typeof(T));
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool IsDefined<T>(string key)
        {
            T tmp;
            return TryGetValue<T>(key, out tmp);
        }

        /// <summary>
        /// Liest value von key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">der Parameter</param>
        /// <returns>Rückgabewert ist im Fehlerfall default(T), ansonsten value</returns>
        public T GetValue<T>(string key)
        {
            T ret = default(T);
            TryGetValue<T>(key, out ret);
            return ret;
        }
    }
}
