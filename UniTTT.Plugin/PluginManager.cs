using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using UniTTT.PathSystem;

[assembly: CLSCompliant(true)]
namespace UniTTT.Plugin
{
    public class PluginManager
    {
        private Dictionary<string, IPlugin> _plugins;
        private FileSystemWatcher _watcher;
        private string _path;
        private PathWrapper _wrapper;
        private List<Type> _interfaceTyps;

        public PluginManager()
        {
            _plugins = new Dictionary<string, IPlugin>();
            _wrapper = new PathSystem.PathWrapper();
            _path = _wrapper.GetPathForCurrentOS("data/plugins");
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            _watcher = new FileSystemWatcher(_path);
            _watcher.Changed += PluginsChanged;
            _watcher.Renamed += PluginsRenamed;

            _interfaceTyps = new List<Type>(Assembly.LoadFile("UniTTT.Interfaces.dll").GetTypes());

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
                if (t.GetInterfaces().Count(f => _interfaceTyps.Contains(f)) > 0)
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

        public List<T> GetPlugins<T>(PluginTypes pluginType)
        {
            List<T> ret = new List<T>();
            foreach (IPlugin item in _plugins.Values)
            {
                if (item.PluginType == pluginType)
                {
                    ret.Add((T)Convert.ChangeType(item, typeof(T)));
                }
            }
            return ret;
        }
    }
}
