using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace IllusionPlugin.Logging
{
    /// <summary>
    /// Logger class usable by plugins.
    /// </summary>
    public class PluginLogger : MonoBehaviour
    {
        /// <summary>
        /// The name that will prefix log messages
        /// </summary>
        private string pluginName;

        public event Action<LogLevel, string, string> Message = delegate {};

        public static PluginLogger Create(string name)
        {
            var newLogger = new GameObject($"IPAPluginLogger{name}").AddComponent<PluginLogger>();
            newLogger.pluginName = name;
            return newLogger;
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="level">Log level (type)</param>
        /// <param name="message">Log message (content)</param>
        public void Log(LogLevel level, string message)
        {
            Message(level, message, pluginName);
        }

        /// <summary>
        /// Shortcut for Logger.Log(LogLevel.Debug, ...)
        /// </summary>
        public void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        /// <summary>
        /// Shortcut for Logger.Log(LogLevel.Notice, ...)
        /// </summary>
        public void Notice(string message)
        {
            Log(LogLevel.Notice, message);
        }

        /// <summary>
        /// Shortcut for Logger.Log(LogLevel.Warning, ...)
        /// </summary>
        public void Warning(string message)
        {
            Log(LogLevel.Warning, message);
        }

        /// <summary>
        /// Shortcut for Logger.Log(LogLevel.Error, ...)
        /// </summary>
        public void Error(string message)
        {
            Log(LogLevel.Error, message);
        }
    }
}
