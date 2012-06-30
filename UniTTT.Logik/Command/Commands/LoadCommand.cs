using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Command.Commands
{
    public class LoadCommand : MemoryCommand, IDataReturner
    {
        public event DataReturnHandler DataReturnEvent;

        public LoadCommand()
        {
            base.KeyWords.Add("Load");
        }

        private void OnDataReturn(object sender, object data)
        {
            DataReturnHandler dataReturnEvent = DataReturnEvent;
            if (dataReturnEvent != null)
            {
                dataReturnEvent(sender, data);
            }
        }

        public override void Execute(string value)
        {
            ConfigStream stream = new ConfigStream(value);
            OnDataReturn(this, stream.Read());
            base.Execute(value);
        }
    }
}
