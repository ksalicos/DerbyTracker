using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Messaging.Events.Bout;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;

namespace DerbyTracker.Common.Messaging.CommandHandlers.JamClock
{
    [Handles("SetTimeoutTypeCommand")]
    public class SetTimeoutTypeCommandHandler : CommandHandlerBase<SetTimeoutTypeCommand>
    {
        private readonly IBoutRunnerService _boutRunner;

        public SetTimeoutTypeCommandHandler(IBoutRunnerService boutRunner)
        {
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(SetTimeoutTypeCommand command)
        {
            var response = new CommandResponse();
            var state = _boutRunner.GetBoutState(command.BoutId);

            if (state.TimeoutType == command.TimeoutType)
            { return response; }

            if ((command.TimeoutType == TimeoutType.LeftTeam && state.LeftTeamState.TimeOutsRemaining == 0)
                || (command.TimeoutType == TimeoutType.RightTeam && state.RightTeamState.TimeOutsRemaining == 0))
            { throw new NoTimeoutsRemainingException(); }

            state.TimeoutType = command.TimeoutType;

            response.AddEvent(new BoutStateUpdatedEvent(state), Audiences.All);
            return response;
        }
    }
}
