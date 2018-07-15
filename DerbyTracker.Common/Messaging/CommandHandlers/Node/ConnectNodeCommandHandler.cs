using DerbyTracker.Common.Messaging.Commands.Node;
using DerbyTracker.Common.Messaging.Events.Node;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;

namespace DerbyTracker.Common.Messaging.CommandHandlers.Node
{
    [Handles("ConnectNodeCommand")]
    public class ConnectNodeCommandHandler : CommandHandlerBase<ConnectNodeCommand>
    {
        private readonly INodeService _nodeService;

        public ConnectNodeCommandHandler(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        public override ICommandResponse Handle(ConnectNodeCommand command)
        {
            var response = new CommandResponse();
            var connection = _nodeService.ConnectNode(command.Originator, command.ConnectionId);

            response.AddEvent(new NodeConnectedEvent(connection), Audiences.All);

            return response;
        }
    }
}
