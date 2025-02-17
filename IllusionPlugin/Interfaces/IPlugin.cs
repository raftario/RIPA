﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IllusionPlugin
{
    /// <summary>
    /// Interface for generic Illusion unity plugins. Every class that implements this will be loaded if the DLL is placed at
    /// data/Managed/Plugins.
    /// </summary>
    public interface IPlugin
    {

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the version of the plugin.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets invoked when the application is started.
        /// </summary>
        void OnApplicationStart();

        /// <summary>
        /// Gets invoked when the application is closed.
        /// </summary>
        void OnApplicationQuit();

        /// <summary>
        /// Gets invoked whenever the active scene changes.
        /// </summary>
        /// <param name="prev">Previous scene</param>
        /// <param name="next">Next scene</param>
        void OnSceneChanged(Scene prev, Scene next);

        /// <summary>
        /// Gets invoked on every graphic update.
        /// </summary>
        void OnUpdate();


        /// <summary>
        /// Gets invoked on ever physics update.
        /// </summary>
        void OnFixedUpdate();
    }
}
