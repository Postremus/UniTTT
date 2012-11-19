using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.PathSystem
{
    [Serializable()]
    public class PathItem
    {
        private string _osName;
        private string _version;
        private string _key;
        private string _path;
        private string _defaultPath;

        public string OSName
        {
            get
            {
                return _osName;
            }
            set
            {
                _osName = value;
            }
        }

        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }

        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }

        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
            }
        }

        public string DefaultPath
        {
            get
            {
                return _defaultPath;
            }
            set
            {
                _defaultPath = value;
            }
        }

        public PathItem()
        { 
        }

        public PathItem(OS.OSInformation osInformation, string key, string path)
        {
            _osName = osInformation.OSName;
            _version = osInformation.Version;
            _key = key;
            _path = path;
        }
    }
}
