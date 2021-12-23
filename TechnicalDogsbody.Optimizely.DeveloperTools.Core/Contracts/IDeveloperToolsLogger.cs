using System;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Core.Contracts
{
    public interface IDeveloperToolsLogger<T> where T : class
    {
        void LogInformation(string functionName, string message, params object[] args);
        void LogCritical(string functionName, string message, params object[] args);
        void LogDebug(string functionName, string message, params object[] args);
        void LogError(string functionName, string message, params object[] args);
        void LogError(string functionName, Exception ex, string message, params object[] args);
        void LogTrace(string functionName, string message, params object[] args);
        void LogWarning(string functionName, string message, params object[] args);
    }
}