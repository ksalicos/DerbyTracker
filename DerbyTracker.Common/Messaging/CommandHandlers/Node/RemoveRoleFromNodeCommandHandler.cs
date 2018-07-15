using DerbyTracker.Common.Messaging.Commands.Node;
using DerbyTracker.Common.Messaging.Events.Node;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;

namespace DerbyTracker.Common.Messaging.CommandHandlers.Node
{
    [Handles("RemoveRoleFromNodeCommand")]
    public class RemoveRoleFromNodeCommandHandler : CommandHandlerBase<RemoveRoleFromNodeCommand>
    {
        private readonly INodeService _nodeService;

        public RemoveRoleFromNodeCommandHandler(INodeService nodeService)
        {
            this._nodeService = nodeService;
        }

        public override ICommandResponse Handle(RemoveRoleFromNodeCommand command)
        {
            var response = new CommandResponse();
            if (!_nodeService.IsInRole(command.NodeId, command.Role)) return response;
            _nodeService.RemoveRole(command.NodeId, command.Role);
            response.AddEvent(new NodeRolesUpdatedEvent(command.NodeId, _nodeService.GetRoles(command.NodeId)), Audiences.All);
            return response;
        }
    }
}
