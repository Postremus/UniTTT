using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Command.Commands
{
    public class Save : MemoryCommand
    {
        public Save()
        {
            KeyWords.Add("Save");
        }

        public override void Execute(string value)
        {
            MemoryData = ParameterInterpreter.InterpretCommandLine(value.TrimStart(' ').TrimEnd(' ').Split(' '));

            base.Execute(value);
        }
    }
}
