using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Popas;

namespace UniTTT.Logik.Command.Commands
{
    public class SaveCommand : MemoryCommand
    {
        public SaveCommand()
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
