using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Popas;

namespace UniTTT.Logik.Command.Commands
{
    public class MemoryCommand : Command
    {
        private Parameterdata _memoryData;

        public Parameterdata MemoryData
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
