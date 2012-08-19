using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace UniTTT.Logik.FileSystem
{
    [Serializable()]
    public class PathFile
    {
        private List<PathItem> _paths;

        public List<PathItem> Paths
        {
            get
            {
                return _paths;
            }
            set
            {
                _paths = value;
            }
        }

        public PathFile()
        {
            _paths = new List<PathItem>();
        }
    }
}
