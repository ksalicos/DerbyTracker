using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;

namespace DerbyTracker.Common.Messaging.CommandHandlers.JamClock
{
    [Handles("StartPeriodCommand")]
    public class StartPeriodCommandHandler : CommandHandlerBase<StartPeriodCommand>
    {
        private readonly IBoutDataService _boutDataService;
        private readonly IBoutRunnerService _boutRunnerService;

        public StartPeriodCommandHandler(IBoutRunnerService boutRunnerService, IBoutDataService boutDataService)
        {
            _boutRunnerService = boutRunnerService;
            _boutDataService = boutDataService;
        }

        public override ICommandResponse Handle(StartPeriodCommand command)
        {
            //Bout must be running
            var bout = _boutDataService.Load(command.BoutId);
            if (!_boutRunnerService.IsRunning(bout.BoutId))
            { throw new BoutNotRunningException(bout.BoutId); }

            //Game Must Be in pregame or halftime
            var state = _boutRunnerService.GetBoutState(command.BoutId);
            if (state.Phase != BoutPhase.Pregame && state.Phase != BoutPhase.Halftime)
            { return new CommandResponse(); }

            state.Phase = BoutPhase.Lineup;
            state.GameClock.Clear();

            var response = new UpdateBoutStateResponse(state);

            return response;
        }
    }
}
