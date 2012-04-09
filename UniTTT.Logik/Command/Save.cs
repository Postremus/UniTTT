using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Command
{
    public class Save : MemoryCommand
    {
        public Save()
        {
            KeyWords.Add("Save");
        }

        public override void Execute(string value)
        {
            MemoryData = new Config.ParameterConfig();
            ParameterInterpreter parameter = ParameterInterpreter.InterpretCommandLine(value.TrimStart(' ').TrimEnd(' ').Split(' '));
            MemoryData.Values = parameter.Arguments;

            base.Execute(value);
        }
    }
}
