using System;
using UnityEngine;
using CookingGame.Core.Logging;

namespace CookingGame.Infrastructure.Logging
{
    public class DebugLogger : ILogger
    {
        public void Info(string message, params object[] args)
        {
            Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, "[INFO] " + message, args);
        }

        public void Warning(string message, params object[] args)
        {
            Debug.LogFormat(LogType.Warning, LogOption.NoStacktrace, null, "[WARNING] " + message, args);
        }

        public void Error(string message, params object[] args)
        {
            Debug.LogFormat(LogType.Error, LogOption.NoStacktrace, null, "[ERROR] " + message, args);
        }

        public void Debug(string message, params object[] args)
        {
            Debug.LogFormat(LogType.Debug, LogOption.NoStacktrace, null, "[DEBUG] " + message, args);
        }

        public void DomainEvent(string eventName, string details)
        {
            Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, "[DOMAIN EVENT] {0}: {1}", eventName, details);
        }
    }
}
