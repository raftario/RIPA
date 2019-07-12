using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IllusionInjector
{
    public class PluginComponent : MonoBehaviour
    {
        private CompositePlugin plugins;
        private bool quitting = false;
        private StreamWriter log = File.AppendText(Path.Combine(Path.Combine(Environment.CurrentDirectory, "Logs"), $"{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.log"));

        public static PluginComponent Create()
        {
            return new GameObject("IPAPluginManager").AddComponent<PluginComponent>();
        }

        void Awake()
        {
            log.AutoFlush = true;
#if DEBUG
            // Console.WriteLine("Awake()");
            log.WriteLine("Awake()");
#endif
            DontDestroyOnLoad(gameObject);

            plugins = new CompositePlugin(PluginManager.Plugins);
            plugins.OnApplicationStart();
            
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        void Start()
        {
#if DEBUG
            // Console.WriteLine("Start()");
            log.WriteLine("Start()");
#endif
        }

        void Update()
        {
#if DEBUG
            // Console.WriteLine("Update()");
#endif
            plugins.OnUpdate();
        }

        void LateUpdate()
        {
            plugins.OnLateUpdate();
        }

        void FixedUpdate()
        {
            plugins.OnFixedUpdate();
        }

        void OnDestroy()
        {
#if DEBUG
            // Console.WriteLine("OnDestroy()");
            log.WriteLine("OnDestroy()");
#endif

            if (!quitting)
            {
                Create().enabled = true;
            }
            else
            {
                SceneManager.activeSceneChanged -= OnSceneChanged;
            }
        }
        
        void OnApplicationQuit()
        {
#if DEBUG
            // Console.WriteLine("OnApplicationQuit()");
            log.WriteLine("OnApplicationQuit()");
#endif
            plugins.OnApplicationQuit();

            quitting = true;
        }

        void OnSceneChanged(Scene prev, Scene next)
        {
#if DEBUG
            // Console.WriteLine("OnSceneChanged({0}, {1})", prev.name, next.name);
            log.WriteLine("OnSceneChanged({0}, {1})", prev.name, next.name);
#endif
            plugins.OnSceneChanged(prev, next);
        }

    }
}
