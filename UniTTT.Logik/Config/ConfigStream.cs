using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace UniTTT.Logik.Config
{
    public class ConfigStream
    {
        private FileStream _stream;
        private XmlSerializer _serializer;
        private string _defaultDir;
        private string _defaultExtension;
        private string _fileName;
        private string _path;

        public ConfigStream(string FileName)
        {
            _defaultDir = "data/config/";
            _defaultExtension = ".xml";
            _fileName = FileName;
            _path = _defaultDir + _fileName + _defaultExtension;
            _serializer = new XmlSerializer(typeof(ParameterConfig));
        }

        public void Write(ParameterConfig config)
        {
            if (!Directory.Exists(_defaultDir))
            {
                Directory.CreateDirectory(_defaultDir);
            }
            _stream = new FileStream(_path, FileMode.Create);
            _serializer.Serialize(_stream, config);
        }

        public object Read()
        {
            _stream = new FileStream(_path, FileMode.Open);
            object ret = _serializer.Deserialize(_stream);
            _stream.Close();
            return ret;
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
