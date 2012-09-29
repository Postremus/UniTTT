using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Command.Commands
{
    public class AsCommand : MemoryCommand
    {
        public AsCommand()
        {
            KeyWords.Add("As");
            NeededCommands.Add(typeof(SaveCommand));
        }

        public override void Execute(string value, KeyValuePair<Command, string> neededCommand)
        {
            ConfigStream stream = new ConfigStream(value);
            ((MemoryCommand)neededCommand.Key).Execute(neededCommand.Value);
            stream.Write(((MemoryCommand)neededCommand.Key).MemoryData);
            base.Execute(value);
        }
    }
}
