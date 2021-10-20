using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace FluentLogger.Interfaces
{
    public interface IFluentSettings
    {
        public string Url { get; }
        public TimeSpan Timeout { get; }
        
        public string TimestampField => "timestamp";
        public string TimestampFormat => "yyyy-MM-ddTHH:mm:sszzz";
        
        public string LevelField => "level";
        public IDictionary<LogLevel, string> LevelNames => new Dictionary<LogLevel, string>
        {
            {LogLevel.Trace, "TRACE"},
            {LogLevel.Debug, "DEBUG"},
            {LogLevel.Information, "INFO"},
            {LogLevel.Warning, "WARN"},
            {LogLevel.Error, "ERROR"},
            {LogLevel.Critical, "FATAL"}
        };

        public string MessageField => "message";
        public string CallerField => "caller";
        public string ExceptionField => "error";
        public Regex ExcludedStackTrace => new Regex("at (System.|FluentLogger.)",RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public IDictionary<string, object> StaticFields => new Dictionary<string, object>();
    }
}