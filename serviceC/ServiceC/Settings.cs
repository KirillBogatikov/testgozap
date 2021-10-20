using System;
using FluentLogger.Interfaces;

namespace ServiceC
{
    public class Settings : IFluentSettings
    {
        public string Url => "http://192.168.5.4:5710";
        public TimeSpan Timeout => TimeSpan.FromSeconds(15);
    }
}