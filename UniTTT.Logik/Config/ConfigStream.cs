﻿using System;
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

        public ConfigStream()
        {
            _defaultDir = "data/config/";
        }

        public void Write(ParameterConfig config)
        {
            _stream = new FileStream(_defaultDir + config.ConfigName + ".xml", FileMode.CreateNew);
            _serializer = new XmlSerializer(typeof(ParameterConfig));
            _serializer.Serialize(_stream, config);
        }

        public object Read(string fileName)
        {
            _stream = new FileStream(_defaultDir + fileName + ".xml", FileMode.Open);
            _serializer = new XmlSerializer(typeof(ParameterConfig));
            return (ParameterConfig)_serializer.Deserialize(_stream);
        }
    }
}