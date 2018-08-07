using DerbyTracker.Common.Messaging.Commands.Node;
using DerbyTracker.Common.Messaging.Events.Node;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;

namespace DerbyTracker.Common.Messaging.CommandHandlers.Node
{
    [Handles("AssignRoleToNodeCommand")]
    public class AssignRoleToNodeCommandHandler : CommandHandlerBase<AssignRoleToNodeCommand>
    {
        private readonly INodeService _nodeService;

        public AssignRoleToNodeCommandHandler(INodeService nodeService)
        {
            this._nodeService = nodeService;
        }

        public override ICommandResponse Handle(AssignRoleToNodeCommand command)
        {
            var response = new CommandResponse();
            //If in role, no nothing.
            if (!_nodeService.IsInRole(command.NodeId, command.Role))
            {
                _nodeService.AddRole(command.NodeId, command.Role);
                response.AddEvent(new NodeRolesUpdatedEvent(command.NodeId, _nodeService.GetRoles(command.NodeId)), _nodeService.GetConnection(command.NodeId));
                response.AddEvent(new NodeRolesUpdatedEvent(command.NodeId, _nodeService.GetRoles(command.NodeId)), command.Originator);
            }
            return response;
        }
    }
}
