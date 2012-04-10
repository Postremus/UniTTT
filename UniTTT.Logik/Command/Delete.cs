using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Command
{
    class Delete : MemoryCommand
    {
        public Delete()
        {
            base.KeyWords.Add("Delete");
        }

        public override void Execute(string value)
        {
            Config.ConfigStream stream = new Config.ConfigStream(value);
            stream.Delete();

            base.Execute(value);
        }
    }
}
