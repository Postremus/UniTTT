using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Config
{
    [Serializable()]
    public class ParameterConfig
    {
        private List<string> _values;

        public List<string> Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
            }
        }
    }
}
