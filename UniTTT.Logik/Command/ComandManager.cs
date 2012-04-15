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
            _dataReturners = new List<object>();
            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (Type t in asm.GetTypes().Where<Type>(t => t.IsSubclassOf(typeof(Command)) && t != typeof(MemoryCommand)))
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
                    if (commandValuePaar.Key.GetType().GetInterfaces().Count(f => f == typeof(IDataReturner)) > 0)
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

        public ReturnData ExecuteReturner(string str)
        {
            Execute(str);
            if (_dataManager.DataForReceive)
            {
                while (!_dataManager.DataReceived) ;
                return _dataManager.Get(_dataReturners[0]);
            }
            return _dataManager.Get(null);
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

            foreach (var item in split)
            {
                if (IsStringKeyWord(item))
                {
                    string nextCommand = null;
                    foreach (string nextCommandTmp in split.Where(f => IsStringKeyWord(f) && !ret.Keys.Contains(f)))
                    {
                        if (item != nextCommandTmp)
                        {
                            nextCommand = nextCommandTmp;
                        }
                    }

                    ret.Add(item, str.SubStringBetween(item, nextCommand).Trim());
                }
            }
            return ret;
        }

        public bool IsStringKeyWord(string str)
        {
            return _command.Count(c => c.KeyWords.Contains(str)) > 0;
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
                        if (commands.Keys.Count(f => f.GetType() == nextNeededCommands.Keys.ElementAt(a)) > 0)
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
