using FluentLogger.Extensions;
using FluentLogger.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ServiceC.Controllers
{
    [Controller]
    [Route("test")]
    public class TestController : Controller
    {
        private readonly IFluentLogger _logger;

        public TestController(IFluentLogger logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult WriteGet(string message)
        {
            _logger.LogInformation($"writing via GET: {message}");
            return Ok();
        }

        [HttpPost]
        public IActionResult WritePost([FromBody] string message)
        {
            _logger.LogInformation($"writing via POST: {message}");
            return Ok();
        }
    }
}