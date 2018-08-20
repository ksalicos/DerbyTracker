using DerbyTracker.Common.Messaging.CommandHandlers.Node;
using DerbyTracker.Common.Messaging.Commands.Node;
using DerbyTracker.Common.Messaging.Events.Bout;
using DerbyTracker.Common.Messaging.Events.Node;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.Node
{
    public class AddNodeToBoutCommandHandlerTests
    {
        private readonly AddNodeToBoutCommand _command = new AddNodeToBoutCommand("nodeId", Guid.Empty, "originator");
        private readonly AddNodeToBoutCommandHandler _handler;

        private readonly IBoutDataService _boutDataService = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunnerService = new BoutRunnerService();
        private readonly INodeService _nodeService;

        public AddNodeToBoutCommandHandlerTests()
        {
            _nodeService = new NodeService(_boutRunnerService);
            _handler = new AddNodeToBoutCommandHandler(_boutRunnerService, _nodeService, _boutDataService);
            _nodeService.ConnectNode("nodeId", "connectionId");
            var bout = _boutDataService.Load(Guid.Empty);
            _boutRunnerService.StartBout(bout);
        }

        [Fact]
        public void NodeIsAddedToBout()
        {
            _handler.Handle(_command);
            Assert.True(_nodeService.IsInBout("nodeId", Guid.Empty));
        }


        [Fact]
        public void ReturnsCorrectEventToMaster()
        {
            var r = _handler.Handle(_command);
            Assert.Contains(r.Events, x => x.Audience == "originator" && x.Event.GetType() == typeof(NodeJoinedBoutEvent));
        }

        [Fact]
        public void ReturnsCorrectEventToClient()
        {
            var r = _handler.Handle(_command);
            Assert.Contains(r.Events, x => x.Audience == "connectionId" && x.Event.GetType() == typeof(InitializeBoutEvent));
        }
    }
}
