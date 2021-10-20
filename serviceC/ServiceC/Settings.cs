using System;
using System.Collections;
using System.Collections.Generic;
using FluentLogger.Interfaces;

namespace ServiceC
{
    public class Settings : IFluentSettings
    {
        public string Url => "http://192.168.5.4:5710";
        public TimeSpan Timeout => TimeSpan.FromSeconds(15);

        public IDictionary<string, object> StaticFields => new Dictionary<string, object>
        {
            { "service", "C" }
        };
    }
}