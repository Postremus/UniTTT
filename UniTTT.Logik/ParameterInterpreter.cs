using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public sealed class ParameterInterpreter
    {
        public ParameterInterpreter()
        {
        }

        public static Parameterdata InterpretCommandLine(string[] args)
        {
            Parameterdata ret = new Parameterdata();
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
                        }
                        else if (CManager.IsStringKeyWord(arg))
                        {
                            Command.ReturnData data = CManager.ExecuteReturner(String.Join(" ", args));
                            if (data.HasData)
                            {
                                return InterpretCommandLine(((Parameterdata)data.Data).Arguments.ToArray());
                            }
                            break;
                        }
                    }
                }
            }
            return ret;
        }
    }
}
