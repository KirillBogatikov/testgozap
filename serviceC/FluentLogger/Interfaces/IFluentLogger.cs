using System;
using Microsoft.Extensions.Logging;

namespace FluentLogger.Interfaces
{
    public interface IFluentLogger
    {
        public void LogException(LogLevel level, Exception exception, string message, params object[] properties);
        public void Log(LogLevel level, string message, params object[] properties);
    }
}