using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.OS
{
    [Serializable()]
    public class OSInformation
    {
        string _version;
        string _osName;

        public string Version
        {
            get
            {
                return _version;
            }
        }

        public string OSName
        {
            get
            {
                return _osName;
            }
        }

        public OSInformation()
        {
            _version = "";
            _osName = "";
        }

        public OSInformation(string version, string osName)
        {
            _version = version;
            _osName = osName;
        }
    }
}
