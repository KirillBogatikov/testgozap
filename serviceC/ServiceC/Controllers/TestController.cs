using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ServiceC.Controllers
{
    [Controller]
    [Route("test")]
    public class TestController : Controller
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
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