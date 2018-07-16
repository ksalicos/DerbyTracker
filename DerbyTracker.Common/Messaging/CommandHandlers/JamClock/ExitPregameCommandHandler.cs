using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Enums;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Messaging.Events.Bout;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;

namespace DerbyTracker.Common.Messaging.CommandHandlers.JamClock
{
    [Handles("ExitPregameCommand")]
    public class ExitPregameCommandHandler : CommandHandlerBase<ExitPregameCommand>
    {
        private readonly IBoutDataService _boutDataService;
        private readonly IBoutRunnerService _boutRunnerService;
        private readonly INodeService _nodeService;

        public ExitPregameCommandHandler(IBoutRunnerService boutRunnerService, IBoutDataService boutDataService, INodeService nodeService)
        {
            _boutRunnerService = boutRunnerService;
            _boutDataService = boutDataService;
            _nodeService = nodeService;
        }

        public override ICommandResponse Handle(ExitPregameCommand command)
        {
            var response = new CommandResponse();

            //Node Must be jam timer
            if (!_nodeService.ConnectionIsInRole(command.Originator, NodeRoles.JamTimer))
            {
                throw new NodeNotAuthorizedException(command.Originator, command.GetType().Name);
            }

            //Bout must be running
            var bout = _boutDataService.Load(command.BoutId);
            if (!_boutRunnerService.IsRunning(bout.BoutId))
            { throw new BoutNotRunningException(bout.BoutId); }

            //Game Must Be in pregame
            var state = _boutRunnerService.GetBoutState(command.BoutId);
            if (state.Phase != BoutPhase.Pregame)
            { return response; }

            state.Phase = BoutPhase.Lineup;
            response.AddEvent(new BoutPhaseChangedEvent(BoutPhase.Lineup, command.BoutId), Audiences.All);

            return response;
        }
    }
}
