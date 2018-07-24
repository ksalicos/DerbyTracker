using DerbyTracker.Common.Messaging.Commands.Node;
using DerbyTracker.Common.Messaging.Events.Bout;
using DerbyTracker.Common.Messaging.Events.Node;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System;

namespace DerbyTracker.Common.Messaging.CommandHandlers.Node
{
    [Handles("ConnectNodeCommand")]
    public class ConnectNodeCommandHandler : CommandHandlerBase<ConnectNodeCommand>
    {
        private readonly INodeService _nodeService;
        private readonly IBoutRunnerService _boutRunnerService;
        private readonly IBoutDataService _boutData;

        public ConnectNodeCommandHandler(INodeService nodeService, IBoutRunnerService boutRunnerService, IBoutDataService boutData)
        {
            _nodeService = nodeService;
            this._boutRunnerService = boutRunnerService;
            _boutData = boutData;
        }

        public override ICommandResponse Handle(ConnectNodeCommand command)
        {
            var response = new CommandResponse();
            var connection = _nodeService.ConnectNode(command.Originator, command.ConnectionId);
            response.AddEvent(new NodeConnectedEvent(connection), Audiences.All);
            if (connection.BoutId != Guid.Empty)
            {
                var bout = _boutData.Load(connection.BoutId);
                var boutState = _boutRunnerService.GetBoutState(connection.BoutId);
                response.AddEvent(new InitializeBoutEvent(bout, boutState), command.ConnectionId);
            }
            return response;
        }
    }
}
