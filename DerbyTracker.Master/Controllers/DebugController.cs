using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DerbyTracker.Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly ILogger _logger;

        public DebugController(ILogger<DebugController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("trace")]
        public void Trace([FromForm]string message)
        {
            _logger.Log(LogLevel.Critical, message);
        }
    }
}