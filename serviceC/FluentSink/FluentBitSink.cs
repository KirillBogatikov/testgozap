using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Serilog.Core;
using Serilog.Events;

namespace ServiceC
{
    public class FluentBitSink : ILogEventSink
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly IList<string> excludedProperties = new List<string>
        {
            "ActionId", "RequestId", "ConnectionId"
        };

        public void Emit(LogEvent logEvent)
        {
            var dictionary = new Dictionary<string, object>
            {
                {"level", logEvent.Level.ToString().ToUpper()},
                {"timestamp", logEvent.Timestamp.ToString("yyyy-MM-ddTHH:mm:sszzz")},
                {"message", logEvent.RenderMessage()}
            };

            if (logEvent.Exception != null)
            {
                dictionary.Add("error", logEvent.Exception.Message);
                dictionary.Add("stacktrace", logEvent.Exception.StackTrace);
            }

            foreach (var (key, value) in logEvent.Properties)
            {
                if (excludedProperties.Contains(key))
                {
                    continue;
                }
                
                dictionary.Add(key, value.ToString());
            }

            if (dictionary.TryGetValue("SourceContext", out var context) && context != null)
            {
                var s = context.ToString();
                if (!string.IsNullOrEmpty(s) && (s.StartsWith("\"Microsoft.Hosting") || s.StartsWith("\"Microsoft.Extensions.Hosting") || s.StartsWith("\"Microsoft.AspNetCore")))
                {
                    return;
                }
            }
            
            var json = JsonConvert.SerializeObject(dictionary).Replace("\\\"", "");
            Console.WriteLine(json);
            
            var content = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            _client.PostAsync("http://192.168.5.4:5710", content);
        }
    }
}