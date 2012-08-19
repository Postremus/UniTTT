using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.FileSystem
{
    public class PathItem
    {
        private OS.OSInformation _osInformation;
        private string _key;
        private string _path;

        public OS.OSInformation PathOSInformation
        {
            get
            {
                return _osInformation;
            }
        }

        public string Key
        {
            get
            {
                return _key;
            }
        }

        public string Path
        {
            get
            {
                return _path;
            }
        }
    }
}
