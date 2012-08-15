using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poc.Serializer;
using Poc;
using System.IO;

namespace UniTTT.Logik.FileSystem
{
    public class PathWrapper
    {
        PathFile _paths;
        ISerializer _serializer;

        public PathWrapper()
        {
            _serializer = new XMLSerializer();
            _paths = _serializer.Deserialize<PathFile>("paths.xml");
        }

        public string GetPathForCurrentOS(string pathName)
        {
            OS.OSInformationCollector osCol = new OS.OSInformationCollector();
            OS.OSInformation osInfo = osCol.GetCurrOSInformation();
            return _paths.Paths.First(f => f.Key.OSName == osInfo.OSName && f.Value == pathName).Value;
        }
    }
}
