using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using IllusionPlugin.Logging;

namespace IllusionInjector.Logging
{
    public class IPALogger
    {
        private readonly DateTime created = DateTime.Now;
        private StreamWriter logFile;
        private StreamWriter unityLogFile;
        private UnityLogInterceptor interceptor;

        public IPALogger()
        {
            string folder = $"{created.ToString("yyyy-MM-dd")}";
            folder = Path.Combine(Path.Combine(Environment.CurrentDirectory, "Logs"), folder);
            string file = $"{created.ToString("HH-mm-ss")}";
            file = Path.Combine(folder, file);
            string unityFile = file + ".unity.log";
            file += ".log";

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            logFile = File.AppendText(file);
            unityLogFile = File.AppendText(unityFile);
            logFile.AutoFlush = true;
            unityLogFile.AutoFlush = true;

            interceptor = new GameObject("LogInterceptor").AddComponent<UnityLogInterceptor>();
            interceptor.Message += UnityLog;
        }

        private void UnityLog(LogLevel level, string message)
        {
            Log(level, message, "UnityEngine");
        }
        
        public void Log(LogLevel level, string message, string sender)
        {
            var sent = DateTime.Now;
            string sentString = sent.ToString("HH:mm:ss");
            string typeString = level.ToString("F");
            
            if (sender == "UnityEngine")
            {
                unityLogFile.WriteLine($"[{sentString}][{typeString}][{sender}] {message}");
            }
            else
            {
                logFile.WriteLine($"[{sentString}][{typeString}][{sender}] {message}");
            }
        }
        
        public void Debug(string message, string sender)
        {
            Log(LogLevel.Debug, message, sender);
        }
        
        public void Notice(string message, string sender)
        {
            Log(LogLevel.Notice, message, sender);
        }
        
        public void Warning(string message, string sender)
        {
            Log(LogLevel.Warning, message, sender);
        }
        
        public void Error(string message, string sender)
        {
            Log(LogLevel.Error, message, sender);
        }
    }
}
