using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.FileSystem
{
    [Serializable()]
    public class PathFile
    {
        private Dictionary<OS.OSInformation, string> _paths;

        public Dictionary<OS.OSInformation, string> Paths
        {
            get
            {
                return _paths;
            }
        }
    }
}
