using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Messaging.Events.JamClock;
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
            var response = new CommandResponse();
            var state = _boutRunner.GetBoutState(command.BoutId);
            state.Phase = BoutPhase.Timeout;
            state.StopGameClock();
            response.AddEvent(new TimeoutStartedEvent(command.BoutId), Audiences.All);
            return response;
        }
    }
}
