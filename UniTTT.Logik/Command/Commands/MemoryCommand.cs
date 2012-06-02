using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Command.Commands
{
    public class MemoryCommand : Command
    {
        private Config.ParameterConfig _memoryData;

        public Config.ParameterConfig MemoryData
        {
            get
            {
                return _memoryData;
            }
            set
            {
                _memoryData = value;
            }
        }
    }
}
