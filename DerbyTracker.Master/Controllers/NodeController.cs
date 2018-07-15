using DerbyTracker.Common.Services;
using DerbyTracker.Master.MVC;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DerbyTracker.Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RestrictToLocalhost]
    public class NodeController : ControllerBase
    {
        private readonly INodeService _nodeService;

        public NodeController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpGet]
        [Route("List")]
        public List<NodeConnection> List()
        {
            return _nodeService.ListConnected();
        }

    }
}