using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;

namespace DerbyTracker.Common.Messaging.CommandHandlers.JamClock
{
    [Handles("StartTimeoutCommand")]
    public class StartTimeoutCommandHandler : CommandHandlerBase<StartTimeoutCommand>
    {
        private readonly IBoutRunnerService _boutRunner;

        public StartTimeoutCommandHandler(IBoutRunnerService boutRunner)
        {
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(StartTimeoutCommand command)
        {
            var state = _boutRunner.GetBoutState(command.BoutId);
            if (state.Phase != BoutPhase.Lineup)
            { throw new InvalidBoutPhaseException(state.Phase); }
            state.TimeOutStart = command.ServerTime;
            state.LoseOfficialReview = false;
            state.TimeoutType = TimeoutType.Official;
            state.Phase = BoutPhase.Timeout;
            state.GameClock.Stop();
            var response = new UpdateBoutStateResponse(state);
            return response;
        }
    }
}
