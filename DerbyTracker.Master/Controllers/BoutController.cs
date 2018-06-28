using DerbyTracker.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace DerbyTracker.Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoutController : ControllerBase
    {
        private readonly IBoutService _boutService;

        public BoutController(IBoutService boutService)
        {
            _boutService = boutService;
        }

        [Route("List")]
        public object List()
        {
            return Ok(_boutService.List());
        }
    }
}