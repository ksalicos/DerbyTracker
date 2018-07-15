using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.CommandHandlers.Node;
using DerbyTracker.Common.Messaging.Commands.Node;
using DerbyTracker.Common.Messaging.Events.Node;
using DerbyTracker.Common.Services;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.Node
{
    public class AssignRoleToNodeCommandHandlerTests
    {
        private readonly INodeService _nodeService;

        public AssignRoleToNodeCommandHandlerTests()
        {
            IBoutRunnerService boutRunnerService = new BoutRunnerService();
            _nodeService = new NodeService(boutRunnerService);
            _nodeService.ConnectNode("nodeId", "connectionId");
        }

        [Fact]
        public void AssigningRoleAddsRole()
        {
            var command = new AssignRoleToNodeCommand("nodeId", "role", "nodeId");
            var handler = new AssignRoleToNodeCommandHandler(_nodeService);
            handler.Handle(command);
            Assert.Contains(_nodeService.GetRoles("nodeId"), x => x == "role");
        }

        [Fact]
        public void AssigningRoleCreatesEvent()
        {
            var command = new AssignRoleToNodeCommand("nodeId", "role", "nodeId");
            var handler = new AssignRoleToNodeCommandHandler(_nodeService);
            var result = handler.Handle(command);
            Assert.Contains(result.Events, x => x.Event.GetType() == typeof(NodeRolesUpdatedEvent));
        }

        [Fact]
        public void PassingInvalidNodeIdThrows()
        {
            Assert.Throws<NoSuchNodeException>(() =>
            {
                var command = new AssignRoleToNodeCommand("invalidNodeId", "role", "nodeId");
                var handler = new AssignRoleToNodeCommandHandler(_nodeService);
                handler.Handle(command);
            });
        }
    }
}
