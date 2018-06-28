using DerbyTracker.Common.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        public IEnumerable<BoutListItem> List()
        {
            return _boutService.List();
        }
    }
}