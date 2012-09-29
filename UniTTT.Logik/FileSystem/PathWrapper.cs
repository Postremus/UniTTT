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
        OS.OSInformation _osInfo;

        public PathWrapper()
        {
            _serializer = new XMLSerializer();
            _paths = _serializer.Deserialize<PathFile>("paths.xml");
            _osInfo = new OS.OSInformationCollector().GetCurrOSInformation();
        }

        public string GetPathForCurrentOS(string pathName)
        {
            string ret = null;
            try
            {
                ret = _paths.Paths.First(f => f.OSName.ToLower() == _osInfo.OSName.ToLower() && f.Key.ToLower() == pathName.ToLower()).Path;
                ret = Environment.ExpandEnvironmentVariables(ret);
            }
            catch (Exception)
            {

                ret = pathName;
            }
            return ret;
        }
    }
}
