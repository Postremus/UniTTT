using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;

namespace UniTTT.Logik.Command
{
    public class ComandManager
    {
        private List<ICommand> _command;

        public List<ICommand> Command
        {
            get
            {
                return _command;
            }
            set
            {
                _command = value;
            }
        }

        public ComandManager()
        {
            _command = new List<ICommand>();
            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (Type t in asm.GetTypes().Where(t => t.IsSubclassOf(typeof(ICommand))))
            {
                _command.Add((ICommand)Activator.CreateInstance(t));
            }
        }

        public void Execute(string str)
        {
            Dictionary<string, string> keyWords = GetAllKeyWords(str);
            foreach (var command in GetCommands(keyWords))
            {
                command.Key.Execute(command.Value);
            }
        }

        private Dictionary<string, string> GetAllKeyWords(string str)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string[] split = str.Split(' ');
            string tmp = str;
            for (int i = 0; i < split.Length; i++)
            {
                if (_command.Count(c => c.KeyWords.Contains(split[i])) > 0)
                {
                    bool moreCommandsexists = false;
                    int indexOf = 0;
                    int length;
                    foreach (string item in split)
                    {
                        if (_command.Count(c => c.KeyWords.Contains(item)) > 0)
                        {
                            if (!moreCommandsexists)
                            {
                                moreCommandsexists = true;
                                indexOf = tmp.IndexOf(item);
                                length = item.Length;
                            }
                        }
                    }
                    if (moreCommandsexists)
                    {
                        ret.Add(split[i], tmp.Substring(tmp.IndexOf(split[i]) + split[i].Length, tmp.Length - split[i].Length - (tmp.Length - indexOf)));
                    }
                    else
                    {
                        ret.Add(split[i], tmp.Substring(tmp.IndexOf(split[i]) + split[i].Length, tmp.Length - split[i].Length));
                    }
                }
            }
            str = tmp;
            return ret;
        }

        private Dictionary<ICommand, string> GetCommands(Dictionary<string, string> keyWords)
        {
            Dictionary<ICommand, string> ret = new Dictionary<ICommand, string>();
            List<int> moeglicheIndexe = new List<int>();
            List<string> commandKeyWords = new List<string>();
            foreach (var command in keyWords)
            {
                if (_command.Count(c => c.KeyWords.Contains(command.Key)) > 0)
                {
                    for (int i = 0; i < _command.Count(c => c.KeyWords.Contains(command.Key)); i++)
                    {
                        moeglicheIndexe.Add(i);
                        commandKeyWords.Add(command.Key);
                        bool same = true;
                        if (commandKeyWords.Count == _command[i].KeyWords.Count)
                        {
                            for (int a = 0; a < _command[i].KeyWords.Count; a++)
                            {
                                if (same && _command[i].KeyWords[a] != commandKeyWords[a])
                                {
                                    same = false;
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }
    }
}
