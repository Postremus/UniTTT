using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Command
{
    public class SaveAs : ICommand
    {
        private List<string> _keyWords;

        public List<string> KeyWords
        {
            get { return _keyWords; }
        }

        public SaveAs()
        {
            _keyWords = new List<string>();
            KeyWords.Add("Save");
            KeyWords.Add("As");
        }

        public void Execute(string value)
        {
            Config.ConfigStream stream = new Config.ConfigStream();
            Config.ParameterConfig paraConfig = new Config.ParameterConfig();
            ParameterInterpreter parameter = ParameterInterpreter.InterpretCommandLine(value.Split(' '));
            paraConfig.Values = parameter.Arguments;
            stream.Write(paraConfig);
        }
    }
}
