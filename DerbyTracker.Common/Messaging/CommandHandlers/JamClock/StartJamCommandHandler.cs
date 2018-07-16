using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Messaging.Events.JamClock;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System;

namespace DerbyTracker.Common.Messaging.CommandHandlers.JamClock
{
    [Handles("StartJamCommand")]
    public class StartJamCommandHandler : CommandHandlerBase<StartJamCommand>
    {
        private readonly IBoutDataService _boutDataService;
        private readonly IBoutRunnerService _boutRunnerService;

        public StartJamCommandHandler(IBoutRunnerService boutRunnerService, IBoutDataService boutDataService)
        {
            _boutRunnerService = boutRunnerService;
            _boutDataService = boutDataService;
        }

        public override ICommandResponse Handle(StartJamCommand command)
        {
            var response = new CommandResponse();
            var bout = _boutDataService.Load(command.BoutId);
            var state = _boutRunnerService.GetBoutState(command.BoutId);

            //State must be in lineup
            if (state.Phase != BoutPhase.Lineup)
            { throw new InvalidBoutPhaseException(state.Phase); }

            //There must be time left on the game clock
            if (state.GameClock().TotalSeconds > bout.RuleSet.PeriodDurationSeconds)
            { return response; } //ignore, probably an end-of-period event on the way

            state.Phase = BoutPhase.Jam;
            state.JamStart = DateTime.Now;
            state.ClockRunning = true;

            response.AddEvent(new JamStartedEvent(command.BoutId), Audiences.All);

            return response;
        }
    }
}
