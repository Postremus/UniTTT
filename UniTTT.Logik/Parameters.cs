using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public sealed class Parameters
    {
        private Dictionary<string, object> dic;
        public Parameters()
        {
            dic = new Dictionary<string, object>();
        }

        public void Add(string key, bool value)
        {
            dic.Add(key, (bool)value);
        }

        public void Add(string key, int value)
        {
            dic.Add(key, (int)value);
        }

        public void Add(string key, string value)
        {
            dic.Add(key, (string)value);
        }

        public bool GetBool(string key)
        {
            object ret;
            dic.TryGetValue(key, out ret);
            if (ret == null)
                return false;
            return (bool)ret;
        }

        public int GetInt(string key)
        {
            object ret;
            dic.TryGetValue(key, out ret);
            if (ret == null)
                return -1;
            return (int)ret;
        }

        public string GetString(string key)
        {
            object ret;
            dic.TryGetValue(key, out ret);
            if (ret == null)
                return null;
            return (string)ret;
        }

        public static Parameters InterpretCommandLine(string[] args)
        {
            Parameters ret = new Parameters();
            if (args.Length >= 0)
            {
                int idx;
                foreach (string arg in args)
                {
                    if (arg[0] == ('-' | '/' | '?'))
                    {
                        idx = arg.Contains(':') ? arg.IndexOf(':') : arg.Contains('=') ? arg.IndexOf('=') : -1;

                        if (idx >= -1)
                        {
                            string key = arg.Substring(1, idx - 1);
                            ret.Add(key, arg.Substring(idx+1));
                        }
                        else
                        {
                            string key = arg.Substring(1);
                            ret.Add(key, true);
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
