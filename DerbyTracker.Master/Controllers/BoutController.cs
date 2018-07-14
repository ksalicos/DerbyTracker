using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DerbyTracker.Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoutController : ControllerBase
    {
        private readonly IBoutDataService _boutDataService;

        public BoutController(IBoutDataService boutDataService)
        {
            _boutDataService = boutDataService;
        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<BoutListItem> List()
        {
            return _boutDataService.List();
        }

        [HttpPost]
        [Route("Save")]
        public Guid Save(Bout bout)
        {
            _boutDataService.Save(bout);
            return bout.BoutId;
        }

        [HttpGet]
        [Route("Load/{id}")]
        public Bout Load(Guid id)
        {
            return _boutDataService.Load(id);
        }
    }
}