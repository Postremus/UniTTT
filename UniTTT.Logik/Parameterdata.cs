using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public class Parameterdata
    {
        private Dictionary<string, object> _dic;

        public int Count
        {
            get
            {
                return _dic.Count;
            }
        }

        public List<string> Arguments
        {
            get
            {
                return _dic.Keys.ToList();
            }
        }

        public Parameterdata()
        {
            _dic = new Dictionary<string, object>();
        }

        public void Add(string key, object value)
        {
            _dic.Add(key, value);
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
            if (!_dic.ContainsKey(key))
            {
                return false;
            }
            try
            {
                value = (T)Convert.ChangeType(_dic[key], typeof(T));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsDefined(string key)
        {
            object tmp;
            return TryGetValue(key, out tmp);
        }

        /// <summary>
        /// Liest value von key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">der Parameter</param>
        /// <returns>Rückgabewert ist im Fehlerfall default(T), ansonsten der Variablen Wert</returns>
        public T GetValue<T>(string key)
        {
            T ret = default(T);
            TryGetValue<T>(key, out ret);
            return ret;
        }
    }
}
