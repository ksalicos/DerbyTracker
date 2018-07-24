using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Services;
using DerbyTracker.Master.MVC;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DerbyTracker.Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RestrictToLocalhost]
    public class BoutController : ControllerBase
    {
        private readonly IBoutDataService _boutDataService;
        private readonly IBoutRunnerService _boutRunnerService;

        public BoutController(IBoutDataService boutDataService, IBoutRunnerService boutRunnerService)
        {
            _boutDataService = boutDataService;
            _boutRunnerService = boutRunnerService;
        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<BoutListItem> List()
        {
            return _boutDataService.List();
        }

        [HttpPost]
        [Route("Save")]
        public Guid Save(BoutData boutData)
        {
            _boutDataService.Save(boutData);
            return boutData.BoutId;
        }

        [HttpGet]
        [Route("Load/{id}")]
        public BoutData Load(Guid id)
        {
            return _boutDataService.Load(id);
        }

        [HttpGet]
        [Route("Running")]
        public List<RunningBout> Running()
        {
            return _boutRunnerService.RunningBouts();
        }
    }
}