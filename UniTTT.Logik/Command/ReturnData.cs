using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Command
{
    public class ReturnData
    {
        private bool _hasData;
        private object _data;

        public bool HasData
        {
            get
            {
                return _hasData;
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
                _hasData = true;
                _data = data;
            }
        }
    }
}
