using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Command
{
    public class DataReturnManager
    {
        private Dictionary<object, object> _data;
        private bool _dataRecieved;
        private bool _dataForReceive;

        public bool DataReceived
        {
            get
            {
                return _dataRecieved;
            }
        }

        public bool DataForReceive
        {
            get
            {
                return _dataForReceive;
            }
        }

        public DataReturnManager()
        {
            _data = new Dictionary<object, object>();
        }

        public void ReceiveReturnedData(object sender, object data)
        {
            if (sender != null && data != null)
            {
                _data.Add(sender, data);
                _dataRecieved = true;
                _dataForReceive = true;
            }
        }

        public ReturnData Get(object key)
        {
            if (key != null)
            {
                _dataRecieved = false;
                return new ReturnData(_data[key]);
            }
            return new ReturnData(null);
        }
    }
}
