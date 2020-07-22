using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SimpleInjector;

namespace Dimmy.Cli.Application
{
    public class PluginLoader
    {
        public static IEnumerable<Assembly> Load(Container container)
        {
            // create plugin loaders
            var pluginsDir = Path.Combine(AppContext.BaseDirectory, "plugins");

            if(!Directory.Exists(pluginsDir))
            {
                Directory.CreateDirectory(pluginsDir);
            }

            foreach (var dir in Directory.GetDirectories(pluginsDir))
            {
                var dirName = Path.GetFileName(dir);
                var pluginDll = Path.Combine(dir, dirName + ".dll");
                if (!File.Exists(pluginDll)) continue;

                var pluginLoader = McMaster.NETCore.Plugins.PluginLoader.CreateFromAssemblyFile(
                    pluginDll,
                    new[] { typeof(IPlugin) });


                var defaultAssembly = pluginLoader.LoadDefaultAssembly();

                var pluginType = defaultAssembly.GetTypes()
                    .Single(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract);

                var plugin = (IPlugin)Activator.CreateInstance(pluginType);

                plugin.Bootstrap(container);

                yield return defaultAssembly;
            }
        }
    }
}
