using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Command
{
    public class ReturnData
    {
        private bool _existsReturnData;
        private object _data;

        public bool ExistsReturnData
        {
            get
            {
                return _existsReturnData;
            }
        }

        public object Data
        {
            get
            {
                return _data;
            }
        }

        public ReturnData(object data)
        {
            if (data != null)
            {
                _existsReturnData = true;
                _data = data;
            }
        }
    }
}
