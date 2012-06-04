using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace UniTTT.Logik.Plugin
{
    public enum PluginTypes
    {
        Field
    }

    class PluginManager
    {
        private Dictionary<string, IPlugin> _plugins;
        private FileSystemWatcher _watcher;

        public PluginManager()
        {
            _plugins = new Dictionary<string, IPlugin>();
            _watcher = new FileSystemWatcher("data/plugins");
            _watcher.Changed += PluginsChanged;
            _watcher.Renamed += PluginsRenamed;
        }

        private void PluginsChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                _plugins.Add(e.FullPath, LoadPluginFromAssembly(e.FullPath));
            }
            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                _plugins.Remove(e.FullPath);
            }
        }

        private void PluginsRenamed(object sender, RenamedEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Renamed)
            {
                IPlugin plug = _plugins[e.OldFullPath];
                _plugins.Remove(e.OldFullPath);
                _plugins.Add(e.FullPath, plug);
            }
        }

        private void LoadAllPlugins()
        {
            foreach(string path in Directory.GetFiles("data/plugins"))
            {
                LoadPluginFromAssembly(path);
            }
        }

        private IPlugin LoadPluginFromAssembly(string path)
        {
            return (IPlugin)Activator.CreateInstanceFrom(path, typeof(IPlugin).Name);
        }

        public IPlugin Get(int index, PluginTypes type)
        {
            var res = _plugins.Where(f => f.Value.PluginType == type);
            return res.GetEnumerator().Current.Value;
        }
    }
}
