using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniTTT.Plugin;

namespace UniTTT.Logik
{
    static class Statics
    {
        private static Random _rnd;
        private static PluginManager _pManager;

        public static Random Rnd
        {
            get
            {
                return _rnd;
            }
        }
        public static PluginManager PManager
        {
            get
            {
                return _pManager;
            }
        }

        static Statics()
        {
            _rnd = new Random();
            _pManager = new PluginManager();
        }
    }
}
