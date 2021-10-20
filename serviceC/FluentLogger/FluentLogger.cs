using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FluentLogger.Interfaces;
using Microsoft.Extensions.Logging;

namespace FluentLogger
{
    public class FluentLogger : IFluentLogger
    {
        private readonly IFluentSettings settings;
        private readonly ILogger<FluentLogger> defaultLogger;
        private readonly HttpClient httpClient;

        public FluentLogger(IFluentSettings settings, ILogger<FluentLogger> defaultLogger)
        {
            this.settings = settings;
            this.defaultLogger = defaultLogger;
            httpClient = new HttpClient();
            httpClient.Timeout = settings.Timeout;
        }

        public void LogException(LogLevel level, Exception exception, string message, params object[] properties)
        {
            var props = new List<object>
            {
                "error", exception.Message,
                "stacktrace", exception.StackTrace ?? ""
            };
            props.AddRange(properties);
            
            LogInternal(level, message, props.ToArray());
        }

        public void Log(LogLevel level, string message, params object[] properties)
        {
            LogInternal(level, message, properties);
        }

        private void LogInternal(LogLevel level, string message, params object[] properties)
        {
            if (!defaultLogger.IsEnabled(level))
            {
                return;
            }
            
            try
            {
                var levelName = level.ToString();

                if (settings.LevelNames.TryGetValue(level, out var value) && !string.IsNullOrEmpty(value))
                {
                    levelName = value;
                }

                var stackTrace = Environment.StackTrace
                    .Split("\r\n")
                    .FirstOrDefault(l => !(settings.ExcludedStackTrace?.IsMatch(l) ?? false));

                var data = new Dictionary<string, object>
                {
                    { settings.LevelField, levelName },
                    { settings.MessageField, message },
                    { settings.CallerField, stackTrace?.Trim(' ', '\t') },
                    { settings.TimestampField, DateTime.UtcNow.ToString(settings.TimestampFormat) }
                };

                for (var i = 0; i < properties.Length; i += 2)
                {
                    if (properties[i] != null)
                    {
                        data.Add($"{properties[i]}", properties[i + 1]);
                    }
                }

                var json = JsonSerializer.Serialize(data);
                defaultLogger.Log(level, json);
                
                var content = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpClient.PostAsync(settings.Url, content);
            }
            catch (Exception e)
            {
                defaultLogger.LogError(e, "Failed to send logs to FluentBit");
            }
        }
    }
}