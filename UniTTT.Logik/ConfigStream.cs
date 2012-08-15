using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Poc;
using Poc.Serializer;

namespace UniTTT.Logik
{
    public class ConfigStream
    {
        private FileStream _stream;
        private ISerializer _serializer;
        private string _defaultDir;
        private string _defaultExtension;
        private string _fileName;
        private string _path;

        public ConfigStream(string fileName) : this(fileName, ".xml") { }

        public ConfigStream(string fileName, string extension)
        {
            _defaultDir = "data/config/";
            _defaultExtension = extension;
            _fileName = fileName;
            _path = Path.Combine(_defaultDir, _fileName + _defaultExtension);
            _serializer = new XMLSerializer();
            if (!Directory.Exists(_defaultDir))
            {
                Directory.CreateDirectory(_defaultDir);
            }
        }

        public void Write(object config)
        {
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }
            _serializer.Serialize<object>(_path, config);
        }

        public object Read()
        {
            return _serializer.Deserialize<object>(_path);
        }

        public void Delete()
        {
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }
        }
    }
}
