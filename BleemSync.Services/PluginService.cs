using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace BleemSync.Services
{
    public class PluginService
    {
        public static void LoadAll(string pluginsPath)
        {
            var pluginFiles = Directory.GetFiles(pluginsPath, "*.dll");

            foreach (var pluginFile in pluginFiles)
            {
                var assemblyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), pluginFile);
                
                AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
            }
        }
    }
}
