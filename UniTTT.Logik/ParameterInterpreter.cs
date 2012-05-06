using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public sealed class ParameterInterpreter
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

        public ParameterInterpreter()
        {
            dic = new Dictionary<string, object>();
            _args = new List<string>();
        }

        public void Add(string key, object value)
        {
            dic.Add(key, value);
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

        public static ParameterInterpreter InterpretCommandLine(string[] args)
        {
            ParameterInterpreter ret = new ParameterInterpreter();
            Command.ComandManager CManager = new Command.ComandManager();
            if (args.Length >= 0)
            {
                int idx;
                foreach (string arg in args)
                {
                    if (arg != null)
                    {
                        if (arg[0] == '/' || arg[0] == '-' || arg[0] == '?')
                        {
                            idx = arg.Contains(':') ? arg.IndexOf(':') : arg.Contains('=') ? arg.IndexOf('=') : -1;

                            if (idx >= 0)
                            {
                                string key = arg.Substring(1, idx - 1);
                                ret.Add(key, arg.Substring(idx + 1));
                            }
                            else
                            {
                                string key = arg.Substring(1);
                                ret.Add(key, true);
                            }
                            ret._args.Add(arg);
                        }
                        else if (CManager.IsStringKeyWord(arg))
                        {
                            Command.ReturnData data = CManager.ExecuteReturner(String.Join(" ", args));
                            if (data.ExistsReturnData)
                            {
                                return InterpretCommandLine(((Config.ParameterConfig)data.Data).Values.ToArray());
                            }
                            break;
                        }
                    }
                }
            }
            else
                return ret;
            return ret;
        }
    }
}
