using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Config
{
    [Serializable()]
    public class ParameterConfig
    {
        private string _configName;
        private List<string> _values;

        public string ConfigName
        {
            get
            {
                return _configName;
            }
            set
            {
                _configName = value;
            }
        }

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
