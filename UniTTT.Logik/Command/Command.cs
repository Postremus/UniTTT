using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Command
{
    public class Command
    {
        private List<string> _keyWords;
        private List<Type> _neededCommands;
        public List<string> KeyWords { get { return _keyWords; } }
        public List<Type> NeededCommands { get { return _neededCommands; } }

        public Command()
        {
            _keyWords = new List<string>();
            _neededCommands = new List<Type>();
        }

        public virtual void Execute(string value)
        {
        }

        public virtual void Execute(string value, KeyValuePair<Command, string> neededCommand)
        {
        }
    }
}
