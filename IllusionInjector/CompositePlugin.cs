using IllusionPlugin;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IllusionInjector
{
    public class CompositePlugin : IPlugin
    {
        IEnumerable<IPlugin> plugins;

        private delegate void CompositeCall(IPlugin plugin);

        public CompositePlugin(IEnumerable<IPlugin> plugins)
        {
            this.plugins = plugins;  
        }

        public void OnApplicationStart()
        {
            Invoke(plugin => plugin.OnApplicationStart());
        }

        public void OnApplicationQuit()
        {
            Invoke(plugin =>  plugin.OnApplicationQuit());
        }

        public void OnSceneChanged(Scene prev, Scene next)
        {
            foreach (var plugin in plugins)
            {
                try
                {
                    plugin.OnSceneChanged(prev, next);
                }
                catch (Exception ex)
                {
                    // Console.WriteLine("{0}: {1}", plugin.Name, ex);
                }
            }
        }


        private void Invoke(CompositeCall callback)
        {
            foreach (var plugin in plugins)
            {
                try
                {
                    callback(plugin);
                }
                catch (Exception ex)
                {
                    // Console.WriteLine("{0}: {1}", plugin.Name, ex);
                }
            }
        }
  

        public void OnUpdate()
        {
            Invoke(plugin => plugin.OnUpdate());
        }

        public void OnFixedUpdate()
        {
            Invoke(plugin => plugin.OnFixedUpdate());
        }


        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public string Version
        {
            get { throw new NotImplementedException(); }
        }

        public void OnLateUpdate()
        {
            Invoke(plugin =>
            {
                if (plugin is IEnhancedPlugin)
                    ((IEnhancedPlugin)plugin).OnLateUpdate();
            });
        }
    }
}
