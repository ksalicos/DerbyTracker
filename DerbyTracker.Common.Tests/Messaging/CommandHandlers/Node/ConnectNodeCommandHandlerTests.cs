using DerbyTracker.Common.Messaging.CommandHandlers.Node;
using DerbyTracker.Common.Messaging.Commands.Node;
using DerbyTracker.Common.Messaging.Events.Node;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System.Linq;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.Node
{
    public class ConnectNodeCommandHandlerTests
    {
        private readonly INodeService _nodeService;
        private readonly IBoutRunnerService _boutRunnerService = new BoutRunnerService();
        private readonly IBoutDataService _boutData = new MockBoutDataService();

        public ConnectNodeCommandHandlerTests()
        {
            _nodeService = new NodeService(_boutRunnerService);
        }

        [Fact]
        public void NewNodeCanConnect()
        {
            var handler = new ConnectNodeCommandHandler(_nodeService, _boutRunnerService, _boutData);
            var command = new ConnectNodeCommand("NodeId", "ConnectionId");
            var result = handler.Handle(command);
            var @event = result.Events.FirstOrDefault(x => x.Event.Type == "NODE_CONNECTED")?.Event as NodeConnectedEvent;
            Assert.NotNull(@event);
            Assert.Equal("NodeId", @event.Data.NodeId);
        }

        [Fact]
        public void NodeGetsSameNumberOnReconnect()
        {
            var handler = new ConnectNodeCommandHandler(_nodeService, _boutRunnerService, _boutData);
            var command = new ConnectNodeCommand("NodeId", "ConnectionId");
            var result = handler.Handle(command);
            var @event = result.Events.FirstOrDefault(x => x.Event.Type == "NODE_CONNECTED")?.Event as NodeConnectedEvent;
            var result2 = handler.Handle(command);
            var event2 = result2.Events.FirstOrDefault(x => x.Event.Type == "NODE_CONNECTED")?.Event as NodeConnectedEvent;
            Assert.NotNull(@event);
            Assert.NotNull(event2);
            Assert.Equal(@event.Data.ConnectionNumber, event2.Data.ConnectionNumber);
        }
    }
}
