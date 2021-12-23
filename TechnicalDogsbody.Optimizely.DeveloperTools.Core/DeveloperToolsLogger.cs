using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalDogsbody.Optimizely.DeveloperTools.Core.Contracts;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Core
{
    public class DeveloperToolsLogger<T> : IDeveloperToolsLogger<T> where T : class
    {
        private readonly ILogger _logger;

        /// <summary>Initializes a new instance of the <see cref="DeveloperToolsLogger{T}"/> class.</summary>
        /// <param name="logger">The logger.</param>
        public DeveloperToolsLogger(
            ILogger<T> logger)
        {
            _logger = logger;
        }

        /// <summary>Logs the critical.</summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void LogCritical(string functionName, string message, params object[] args)
        {
            _logger.LogCritical(GetLogMessage(functionName, message), args);
        }

        /// <summary>Logs the debug.</summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void LogDebug(string functionName, string message, params object[] args)
        {
            _logger.LogDebug(GetLogMessage(functionName, message), args);
        }

        /// <summary>Logs the error.</summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void LogError(string functionName, string message, params object[] args)
        {
            _logger.LogError(GetLogMessage(functionName, message), args);
        }

        /// <summary>Logs the error.</summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="ex">The exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void LogError(string functionName, Exception ex, string message, params object[] args)
        {
            _logger.LogError(ex, GetLogMessage(functionName, message), args);
        }

        /// <summary>Logs the information.</summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void LogInformation(string functionName, string message, params object[] args)
        {
            _logger.LogInformation(GetLogMessage(functionName, message), args);
        }

        /// <summary>Logs the trace.</summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void LogTrace(string functionName, string message, params object[] args)
        {
            _logger.LogTrace(GetLogMessage(functionName, message), args);
        }

        /// <summary>Logs the warning.</summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void LogWarning(string functionName, string message, params object[] args)
        {
            _logger.LogWarning(GetLogMessage(functionName, message), args);
        }

        /// <summary>Gets the log message.</summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="message">The message.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns></returns>
        private string GetLogMessage(string functionName, string message)
        {
            functionName = $"Function: {functionName}";
            message = $"Message: {message}";

            return $"{functionName} - {message}";
        }
    }
}
