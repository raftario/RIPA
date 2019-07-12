using IllusionPlugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace IllusionInjector
{
    public static class PluginManager
    {
        private static List<IPlugin> _Plugins = null;

        /// <summary>
        /// Gets the list of loaded plugins and loads them if necessary.
        /// </summary>
        public static IEnumerable<IPlugin> GetPlugins(Logging.IPALogger logger)
        {
            if(_Plugins == null)
            {
                LoadPlugins(logger);
            }
            return _Plugins;
        }


        private static void LoadPlugins(Logging.IPALogger logger)
        {
            string pluginDirectory = Path.Combine(Environment.CurrentDirectory, "Plugins");

            // Process.GetCurrentProcess().MainModule crashes the game and Assembly.GetEntryAssembly() is NULL,
            // so we need to resort to P/Invoke
            string exeName = Path.GetFileNameWithoutExtension(AppInfo.StartupPath);
            logger.Debug(exeName, "IPA");
            _Plugins = new List<IPlugin>();

            if (!Directory.Exists(pluginDirectory)) return;
            
            string[] files = Directory.GetFiles(pluginDirectory, "*.dll");
            foreach (var s in files)
            {
                _Plugins.AddRange(LoadPluginsFromFile(Path.Combine(pluginDirectory, s), exeName, logger));
            }

            // DEBUG
            logger.Debug($"Running on Unity {UnityEngine.Application.unityVersion}", "IPA");
            logger.Debug($"Loading plugins from {pluginDirectory} and found {_Plugins.Count}", "IPA");
            foreach (var plugin in _Plugins)
            {
                logger.Debug($"{plugin.Name}: {plugin.Version}", "IPA");
            }
        }

        private static IEnumerable<IPlugin> LoadPluginsFromFile(string file, string exeName, Logging.IPALogger logger)
        {
            List<IPlugin> plugins = new List<IPlugin>();

            if (!File.Exists(file) || !file.EndsWith(".dll", true, null))
                return plugins;

            try
            {
                Assembly assembly = Assembly.LoadFrom(file);

                foreach (Type t in assembly.GetTypes())
                {
                    if (t.GetInterface("IPlugin") != null)
                    {
                        try
                        {

                            IPlugin pluginInstance = Activator.CreateInstance(t) as IPlugin;
                            string[] filter = null;

                            if (pluginInstance is IEnhancedPlugin)
                            {
                                filter = ((IEnhancedPlugin)pluginInstance).Filter;
                            }
                            
                            if(filter == null || Enumerable.Contains(filter, exeName, StringComparer.OrdinalIgnoreCase))
                                plugins.Add(pluginInstance);
                        }
                        catch (Exception e)
                        {
                            logger.Warning($"Could not load plugin {t.FullName} in {Path.GetFileName(file)}. {e}", "IPA");
                        }
                    }
                }

            }
            catch (Exception e)
            {
                logger.Error($"Could not load {Path.GetFileName(file)}. {e}", "IPA");
            }

            return plugins;
        }

        public class AppInfo
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            private static extern int GetModuleFileName(HandleRef hModule, StringBuilder buffer, int length);
            private static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);
            public static string StartupPath
            {
                get
                {
                    StringBuilder stringBuilder = new StringBuilder(260);
                    GetModuleFileName(NullHandleRef, stringBuilder, stringBuilder.Capacity);
                    return stringBuilder.ToString();
                }
            }
        }

    }
}
