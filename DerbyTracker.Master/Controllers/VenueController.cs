using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Services;
using DerbyTracker.Master.MVC;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DerbyTracker.Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RestrictToLocalhost]
    public class VenueController : ControllerBase
    {
        private readonly IVenueDataService _venueDataService;

        public VenueController(IVenueDataService venueDataService)
        {
            _venueDataService = venueDataService;
        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<Venue> List()
        {
            return _venueDataService.List();
        }

        [HttpPost]
        [Route("Save")]
        public bool Save(Venue venue)
        {
            _venueDataService.Save(venue);
            return true;
        }

    }
}