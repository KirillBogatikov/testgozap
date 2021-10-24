using System;
using FluentLogger.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ServiceC.Controllers
{
    [Controller]
    [Route("log")]
    public class LogController : Controller
    {
        private IFluentLogger _fluentLogger;

        public LogController(IFluentLogger fluentLogger)
        {
            _fluentLogger = fluentLogger;
        }

        [HttpGet]
        public IActionResult Get(LogLevel level, string message)
        {
            _fluentLogger.Log(level, message);
            return Ok(level);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Entity entity)
        {
            _fluentLogger.Log(entity.Level, entity.Message);
            return Ok(entity.Level);
        }
    }

    public class Entity
    {
        public LogLevel Level { get; set; }
        public string Message { get; set; }
    }
}