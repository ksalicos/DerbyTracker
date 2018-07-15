using DerbyTracker.Common.Messaging.CommandHandlers.Node;
using DerbyTracker.Common.Messaging.Commands.Node;
using DerbyTracker.Common.Messaging.Events.Node;
using DerbyTracker.Common.Services;
using System.Linq;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.Node
{
    public class ConnectNodeCommandHandlerTests
    {
        private readonly INodeService _nodeService;

        public ConnectNodeCommandHandlerTests()
        {
            IBoutRunnerService boutRunnerService = new BoutRunnerService();
            _nodeService = new NodeService(boutRunnerService);
        }

        [Fact]
        public void NewNodeCanConnect()
        {
            var handler = new ConnectNodeCommandHandler(_nodeService);
            var command = new ConnectNodeCommand("NodeId", "ConnectionId");
            var result = handler.Handle(command);
            var @event = result.Events.FirstOrDefault(x => x.Event.type == "NODE_CONNECTED")?.Event as NodeConnectedEvent;
            Assert.NotNull(@event);
            Assert.Equal("NodeId", @event.data.NodeId);
        }

        [Fact]
        public void NodeGetsSameNumberOnReconnect()
        {
            var handler = new ConnectNodeCommandHandler(_nodeService);
            var command = new ConnectNodeCommand("NodeId", "ConnectionId");
            var result = handler.Handle(command);
            var @event = result.Events.FirstOrDefault(x => x.Event.type == "NODE_CONNECTED")?.Event as NodeConnectedEvent;
            var result2 = handler.Handle(command);
            var event2 = result2.Events.FirstOrDefault(x => x.Event.type == "NODE_CONNECTED")?.Event as NodeConnectedEvent;
            Assert.NotNull(@event);
            Assert.NotNull(event2);
            Assert.Equal(@event.data.ConnectionNumber, event2.data.ConnectionNumber);
        }
    }
}
