using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace UniTTT.Logik.Plugin
{
    public class PluginManager
    {
        private Dictionary<string, IPlugin> _plugins;
        private FileSystemWatcher _watcher;
        private string _path;
        private FileSystem.PathWrapper _wrapper;

        public PluginManager()
        {
            _plugins = new Dictionary<string, IPlugin>();
            _wrapper = new FileSystem.PathWrapper();
            _path = _wrapper.GetPathForCurrentOS("data/plugins");
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            _watcher = new FileSystemWatcher(_path);
            _watcher.Changed += PluginsChanged;
            _watcher.Renamed += PluginsRenamed;
            LoadAllPlugins();
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
            foreach (string path in Directory.GetFiles(_path))
            {
                IPlugin plug = LoadPluginFromAssembly(path);
                if (plug != null)
                {
                    _plugins.Add(path, plug);
                }
            }
        }

        private IPlugin LoadPluginFromAssembly(string path)
        {
            Assembly asm = Assembly.LoadFile(Path.GetFullPath(path));

            foreach (Type t in asm.GetTypes())
            {
                if (t.GetInterfaces().Count(f => f == typeof(IPlugin)) > 0)
                {
                    try
                    {
                        return (IPlugin)asm.CreateInstance(t.FullName);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return null;
        }

        private PluginTypes GetPluginType(Type t)
        {
            return (PluginTypes)t.GetProperty("PluginType").GetValue(t, null);
        }

        public IPlugin Get(string name, PluginTypes type)
        {
            foreach (KeyValuePair<string, IPlugin> pair in _plugins)
            {
                if (pair.Value.PluginName == name && pair.Value.PluginType == type)
                {
                    return pair.Value;
                }
            }
            return null;
        }
    }
}
