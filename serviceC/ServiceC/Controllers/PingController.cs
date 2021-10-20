using System;
using FluentLogger.Extensions;
using FluentLogger.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ServiceC.Controllers
{
    [Controller]
    [Route("/ping")]
    public class PingController : Controller
    {
        private readonly IFluentLogger _logger;

        public PingController(IFluentLogger logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Ping()
        {
            _logger.LogDebug("ping");
            try
            {
                throw new Exception();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "fail");
            }
            return Ok();
        }
    }
}