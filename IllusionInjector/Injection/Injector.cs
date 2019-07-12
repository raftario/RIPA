using System;
using System.IO;
using UnityEngine;

namespace IllusionInjector
{
    public static class Injector
    {
        public static void Inject()
        {
            var bootstrapper = new GameObject("Bootstrapper").AddComponent<Bootstrapper>();
            bootstrapper.Destroyed += BootstrapperDestroyed;
        }

        private static void BootstrapperDestroyed()
        {
            PluginComponent.Create().enabled = true;
        }
    }
}
