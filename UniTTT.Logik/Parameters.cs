using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public sealed class Parameters
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

        public Parameters()
        {
            dic = new Dictionary<string, object>();
            _args = new List<string>();
        }

        public void Add(string key, object value)
        {
            dic.Add(key, value);
        }

        public bool GetBool(string key)
        {
            object ret;
            if (dic.TryGetValue(key, out ret))
                return (bool)ret;
            else
                return false;
        }

        /// <summary>
        /// Holt einen Int. Wenn nicht vorhanden, wird -1 zurückgegeben.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetInt(string key)
        {
            int ret;
            if (int.TryParse(GetString(key), out ret))
                return ret;
            else
                return -1;
        }

        public string GetString(string key)
        {
            object ret;
            if (dic.TryGetValue(key, out ret))
                return (string)ret;
            else
                return null;
        }

        public static Parameters InterpretCommandLine(string[] args)
        {
            Parameters ret = new Parameters();
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
                    }
                }
            }
            else
                return ret;
            return ret;
        }
    }
}
