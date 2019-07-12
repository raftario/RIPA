using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using IllusionPlugin.Logging;

namespace IllusionInjector
{
    public class PluginComponent : MonoBehaviour
    {
        private CompositePlugin plugins;
        private bool quitting = false;
        private Logging.IPALogger logger = new Logging.IPALogger();
        private PluginLogger[] pluginLoggers;

        public static PluginComponent Create()
        {
            return new GameObject("IPAPluginManager").AddComponent<PluginComponent>();
        }

        void Awake()
        {
#if DEBUG
            logger.Debug("Awake()", "IPA");
#endif
            DontDestroyOnLoad(gameObject);

            plugins = new CompositePlugin(PluginManager.GetPlugins(logger), logger);
            plugins.OnApplicationStart();

            pluginLoggers = FindObjectsOfType<PluginLogger>();
            foreach (PluginLogger pluginLogger in pluginLoggers)
            {
                pluginLogger.Message += logger.Log;
            }
            
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        void Start()
        {
#if DEBUG
            logger.Debug("Start()", "IPA");
#endif
        }

        void Update()
        {
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
            logger.Debug("OnDestroy()", "IPA");
#endif

            if (!quitting)
            {
                Create().enabled = true;
            }
            else
            {
                SceneManager.activeSceneChanged -= OnSceneChanged;
                foreach (PluginLogger pl in pluginLoggers)
                {
                    pl.Message -= logger.Log;
                }
            }
        }
        
        void OnApplicationQuit()
        {
#if DEBUG
            logger.Debug("OnApplicationQuit()", "IPA");
#endif
            plugins.OnApplicationQuit();

            quitting = true;
        }

        void OnSceneChanged(Scene prev, Scene next)
        {
#if DEBUG
            logger.Debug($"OnSceneChanged({prev.name}, {next.name})", "IPA");
#endif
            plugins.OnSceneChanged(prev, next);
        }

    }
}
