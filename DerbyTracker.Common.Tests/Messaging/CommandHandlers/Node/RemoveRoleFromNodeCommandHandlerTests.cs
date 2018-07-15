using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.CommandHandlers.Node;
using DerbyTracker.Common.Messaging.Commands.Node;
using DerbyTracker.Common.Messaging.Events.Node;
using DerbyTracker.Common.Services;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.Node
{
    public class RemoveRoleFromNodeCommandHandlerTests
    {
        private readonly INodeService _nodeService;

        public RemoveRoleFromNodeCommandHandlerTests()
        {
            IBoutRunnerService boutRunnerService = new BoutRunnerService();
            _nodeService = new NodeService(boutRunnerService);
            _nodeService.ConnectNode("nodeId", "connectionId");
            _nodeService.AddRole("nodeId", "role");
        }

        [Fact]
        public void RemovingRoleRemovesRole()
        {
            var command = new RemoveRoleFromNodeCommand("nodeId", "role", "nodeId");
            var handler = new RemoveRoleFromNodeCommandHandler(_nodeService);
            handler.Handle(command);
            Assert.DoesNotContain(_nodeService.GetRoles("nodeId"), x => x == "role");
        }

        [Fact]
        public void RemovingRoleCreatesEvent()
        {
            var command = new RemoveRoleFromNodeCommand("nodeId", "role", "nodeId");
            var handler = new RemoveRoleFromNodeCommandHandler(_nodeService);
            var result = handler.Handle(command);
            Assert.Contains(result.Events, x => x.Event.GetType() == typeof(NodeRolesUpdatedEvent));
        }

        [Fact]
        public void RemovingUnassignedRoleDoesntThrow()
        {
            var command = new RemoveRoleFromNodeCommand("nodeId", "unassigned", "nodeId");
            var handler = new RemoveRoleFromNodeCommandHandler(_nodeService);
            handler.Handle(command);
        }

        [Fact]
        public void PassingInvalidNodeIdThrows()
        {
            Assert.Throws<NoSuchNodeException>(() =>
            {
                var command = new RemoveRoleFromNodeCommand("invalidNodeId", "role", "nodeId");
                var handler = new RemoveRoleFromNodeCommandHandler(_nodeService);
                handler.Handle(command);
            });
        }
    }
}
