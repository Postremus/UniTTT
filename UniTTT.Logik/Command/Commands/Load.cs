using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Command
{
    public class Load : MemoryCommand, IDataReturner
    {
        public event DataReturnHandler DataReturnEvent;

        public Load()
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
            Config.ConfigStream stream = new Config.ConfigStream(value);
            OnDataReturn(this, stream.Read());
            base.Execute(value);
        }
    }
}
