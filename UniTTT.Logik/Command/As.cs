using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Command
{
    public class As : MemoryWriteCommand
    {
        public As()
        {
            KeyWords.Add("As");
            NeededCommands.Add(typeof(MemoryCommand));
        }

        public override void Execute(string value, KeyValuePair<Command, string> neededCommand)
        {
            Config.ConfigStream stream = new Config.ConfigStream(value);
            ((MemoryCommand)neededCommand.Key).Execute(neededCommand.Value);
            stream.Write(((MemoryCommand)neededCommand.Key).MemoryData);
            base.Execute(value);
        }
    }
}
