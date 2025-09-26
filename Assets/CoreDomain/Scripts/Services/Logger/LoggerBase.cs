using System;

namespace CoreDomain.Scripts.Services.Logger
{
    public abstract class LoggerBase : ILogger
    {
        public abstract void Log(string message);

        public abstract void LogWarning(string message);

        public abstract void LogError(string message);

        public abstract void LogException(Exception message);
    }
}