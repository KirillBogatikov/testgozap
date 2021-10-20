using System;
using FluentLogger.Interfaces;
using Microsoft.Extensions.Logging;

namespace FluentLogger.Extensions
{
    public static class FluentLoggerExtensions
    {
        public static void LogTrace(this IFluentLogger logger, string message, params object[] properties)
        {
            logger.Log(LogLevel.Trace, message, properties);
        }
        
        public static void LogDebug(this IFluentLogger logger, string message, params object[] properties)
        {
            logger.Log(LogLevel.Debug, message, properties);
        }
        
        public static void LogInformation(this IFluentLogger logger, string message, params object[] properties)
        {
            logger.Log(LogLevel.Information, message, properties);
        }
        
        public static void LogError(this IFluentLogger logger, Exception exception, string message, params object[] properties)
        {
            logger.LogException(LogLevel.Error, exception, message, properties);
        }
        
        public static void LogCritical(this IFluentLogger logger, Exception exception, string message, params object[] properties)
        {
            logger.LogException(LogLevel.Critical, exception, message, properties);
        }
    }
}