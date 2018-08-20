using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.Node;
using DerbyTracker.Common.Messaging.Events.Bout;
using DerbyTracker.Common.Messaging.Events.Node;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;

namespace DerbyTracker.Common.Messaging.CommandHandlers.Node
{
    [Handles("AddNodeToBoutCommand")]
    public class AddNodeToBoutCommandHandler : CommandHandlerBase<AddNodeToBoutCommand>
    {
        private readonly IBoutRunnerService _boutRunnerService;
        private readonly INodeService _nodeService;
        private readonly IBoutDataService _boutData;

        public AddNodeToBoutCommandHandler(IBoutRunnerService boutRunnerService, INodeService nodeService, IBoutDataService boutData)
        {
            _boutRunnerService = boutRunnerService;
            _nodeService = nodeService;
            _boutData = boutData;
        }

        public override ICommandResponse Handle(AddNodeToBoutCommand command)
        {
            var response = new CommandResponse();

            if (!_boutRunnerService.IsRunning(command.BoutId))
            {
                throw new BoutNotFoundException(command.BoutId);
            }

            var state = _boutRunnerService.GetBoutState(command.BoutId);
            var bout = _boutData.Load(command.BoutId);


            _nodeService.AddToBout(command.NodeId, command.BoutId);
            response.AddEvent(new InitializeBoutEvent(bout, state), _nodeService.GetConnection(command.NodeId));
            response.AddEvent(new NodeJoinedBoutEvent(command.NodeId, command.BoutId), command.Originator);
            return response;
        }
    }
}
