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
        private List<Command> _command;
        private DataReturnManager _dataManager;
        private List<object> _dataReturners;

        public List<Command> Command
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
        public event DataReturnHandler DataReturnEvent;

        public ComandManager()
        {
            _command = new List<Command>();
            _dataManager = new DataReturnManager();
            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (Type t in asm.GetTypes().Where<Type>(t => t.IsSubclassOf(typeof(Command)) && t != typeof(MemoryCommand) && t != typeof(MemoryWriteCommand)))
            {
                _command.Add((Command)Activator.CreateInstance(t));
            }

            DataReturnEvent += _dataManager.ReceiveReturnedData;
        }

        public void Execute(string str)
        {
            Dictionary<string, string> keyWords = GetAllKeyWords(str);
            Dictionary<Command, string> InstancedCommands = GetCommands(keyWords);
            if (CheckForFailures(InstancedCommands))
            {
                KeyValuePair<Command, string> lastCommand = new KeyValuePair<Logik.Command.Command, string>();
                bool update = true;
                foreach (var commandValuePaar in InstancedCommands)
                {
                    if (commandValuePaar.GetType() == typeof(IDataReturner))
                    {
                        ((IDataReturner)commandValuePaar.Key).DataReturnEvent += OnDataReturn;
                        _dataReturners.Add(commandValuePaar.Key);
                    }

                    if (update)
                    {
                        lastCommand = commandValuePaar;
                        update = false;
                    }
                    if (commandValuePaar.Key.NeededCommands.Count > 0)
                    {
                        commandValuePaar.Key.Execute(commandValuePaar.Value, lastCommand);
                        update = true;
                    }
                    else
                    {
                        lastCommand.Key.Execute(lastCommand.Value);
                        lastCommand = commandValuePaar;
                    }
                }
            }
        }

        public object ExecuteReturner(string str)
        {
            Execute(str);
            while (!_dataManager.DataReceived || _dataManager.DataForReceive) ;
            return _dataManager.Get(_dataReturners[0]);
        }

        public void OnDataReturn(object sender, object data)
        {
            DataReturnHandler dataReturnEvent = DataReturnEvent;
            if (dataReturnEvent != null)
            {
                dataReturnEvent(sender, data);
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
                    int length = 0;
                    for (int a = i; a < split.Length; a++)
                    {
                        if (_command.Count(c => c.KeyWords.Contains(split[a])) > 0)
                        {
                            if (!moreCommandsexists && split[a] != split[i])
                            {
                                moreCommandsexists = true;
                                indexOf = tmp.IndexOf(split[a]);
                                length = split[a].Length;
                            }
                        }
                    }
                    if (moreCommandsexists)
                    {
                        int idxOf = tmp.IndexOf(split[i]) + split[i].Length;
                        int o = tmp.Length - split[i].Length - (tmp.Length - indexOf);
                        ret.Add(split[i], tmp.Substring(idxOf, o).Trim());
                    }
                    else
                    {
                        int idxOf = tmp.IndexOf(split[i]);
                        idxOf += +split[i].Length;
                        int o = tmp.Length - idxOf;
                        ret.Add(split[i], tmp.Substring(idxOf, o).Trim());
                    }
                }
            }
            str = tmp;
            return ret;
        }

        private Dictionary<Command, string> GetCommands(Dictionary<string, string> keyWords)
        {
            Dictionary<Command, string> ret = new Dictionary<Command, string>();
            foreach (var command in keyWords)
            {
                foreach (Command instancedCommaned in _command)
                {
                    if (instancedCommaned.KeyWords.All(k => k == command.Key))
                    {
                        ret.Add(instancedCommaned, command.Value);
                    }
                }
            }

            return ret;
        }

        private bool CheckForFailures(Dictionary<Command, string> commands)
        {
            System.Collections.Generic.SortedList<Type, bool> nextNeededCommands = new SortedList<Type, bool>();
            for (int i = 0; i < commands.Count; i++)
            {
                foreach (Type t in commands.Keys.ElementAt(i).NeededCommands)
                {
                    nextNeededCommands.Add(t, true);
                }

                if (nextNeededCommands.Count > 0)
                {
                    for (int a = 0; a < nextNeededCommands.Count; a++)
                    {
                        if (commands.Keys.Count(f => f.GetType().BaseType == nextNeededCommands.Keys.ElementAt(a)) == 1)
                        {
                            nextNeededCommands.RemoveAt(a);
                        }
                    }
                }
            }
            return nextNeededCommands.Count(f => f.Value == true) == 0;
        }
    }
}
