using System;

namespace CoreDomain.Scripts.Services.Logger
{
    public class UnityLogger : ILogger
    {
        public void Log(string message)
        {
            LogImpl(message);
        }
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        private void LogImpl(string message)
        {
            UnityEngine.Debug.Log("[NORMAL] " + message);
        }

        public void LogWarning(string message)
        {
            LogWarningImpl(message);
        }
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        private void LogWarningImpl(string message)
        {
            UnityEngine.Debug.LogWarning("[WARNING] " + message);
        }
        
        public void LogError(string message)
        {
            LogErrorImpl(message);
        }
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        private void LogErrorImpl(string message)
        {
            UnityEngine.Debug.LogError("[ERROR] " + message);
        }

        public void LogException(Exception message)
        {
            LogExceptionImpl(message);
        }
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        private void LogExceptionImpl(Exception message)
        {
            UnityEngine.Debug.LogException(message);
        }
    }
}