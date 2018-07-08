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
        private readonly IBoutFileService _boutFileService;

        public BoutController(IBoutFileService boutFileService)
        {
            _boutFileService = boutFileService;
        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<BoutListItem> List()
        {
            return _boutFileService.List();
        }

        [HttpPost]
        [Route("Save")]
        public Guid Save(Bout bout)
        {
            _boutFileService.Save(bout);
            return bout.BoutId;
        }

        [HttpGet]
        [Route("Load/{id}")]
        public Bout Load(Guid id)
        {
            return _boutFileService.Load(id);
        }
    }
}