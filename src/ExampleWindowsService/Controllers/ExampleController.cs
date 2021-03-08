using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace ExampleWindowsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExampleController : ControllerBase
    {
        private readonly ILogger<ExampleController> _logger;

        public ExampleController(ILogger<ExampleController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetCurrentTime")]
        public IActionResult GetCurrentTime()
        {
            var currentTime = DateTime.Now;
            _logger.LogInformation($"GetCurrentTime: {currentTime:G}");
            return Ok(currentTime);
        }
    }
}
