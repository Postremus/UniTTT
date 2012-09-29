using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UniTTT.Logik.OS
{
    public class OSInformationCollector
    {
        private OperatingSystem _os;

        public OSInformationCollector()
        {
            _os = Environment.OSVersion;
        }

        public OSInformation GetCurrOSInformation()
        {
            if (_os.Platform == PlatformID.Unix || _os.Platform == PlatformID.MacOSX)
            {
                return GetLinuxInformation();
            }
            else
            {
                return new OSInformation(_os.Version.ToString(), _os.Platform.ToString());
            }
        }

        private OSInformation GetLinuxInformation()
        {
            string path = "/etc/lsb-release";
            OSInformation ret = null;
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                string name = lines[0].Split('=')[1];
                string version = lines[1].Split('=')[1];
                ret = new OSInformation(version, name);
            }
            else
            {
                ret = new OSInformation(null, null);
            }
            return ret;
        }

        public string GetCurrOSName()
        {
            return GetCurrOSInformation().OSName;
        }

        public string GetCurrOSVersion()
        {
            return GetCurrOSInformation().Version;
        }
    }
}
