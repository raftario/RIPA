using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using IllusionPlugin.Logging;

namespace IllusionInjector.Logging
{
    class UnityLogInterceptor : MonoBehaviour
    {
        public event Action<LogLevel, string> Message = delegate {};

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            Message(LogLevel.Debug, "IPA UnityLogInterceptor enabled");
            Application.logMessageReceivedThreaded += EmitMessage;
        }

        private void OnDisable()
        {
            Message(LogLevel.Debug, "IPA UnityLogInterceptor disabled");
            Application.logMessageReceivedThreaded -= EmitMessage;
        }

        private LogLevel UnityToInternalLevel(LogType unityType)
        {
            switch (unityType)
            {
                case LogType.Log:
                    return LogLevel.Debug;
                case LogType.Warning:
                    return LogLevel.Warning;
                default:
                    return LogLevel.Error;
            }
        }

        private void EmitMessage(string logString, string stackTrace, LogType type)
        {
            if (!logString.ToUpper().Contains("System.Threading.Monitor.Enter".ToUpper()))
                Message(UnityToInternalLevel(type), logString);
        }
    }
}
