using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Plugin
{
    public interface IPlugin
    {
        PluginTypes PluginType { get; }
    }
}
