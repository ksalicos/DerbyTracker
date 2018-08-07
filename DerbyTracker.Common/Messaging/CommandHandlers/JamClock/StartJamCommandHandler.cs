using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Messaging.Events.Bout;
using DerbyTracker.Common.Messaging.Events.PenaltyBoxTimer;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Events;
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
            var elapsed = state.GameClock.Elapsed.TotalSeconds;
            if (elapsed > bout.RuleSet.PeriodDurationSeconds)
            {
                response.AddEvent(
                    MessageBaseEvent.Error($"Call to start jam came too late.  Elapsed: {state.GameClock.Elapsed.TotalSeconds}"),
                    command.Originator);
                return response;
            }

            state.Phase = BoutPhase.Jam;
            state.JamStart = DateTime.Now;
            state.GameClock.Start();

            state.PenaltyBox.ForEach(x =>
            {
                x.StopWatch.Start();
                response.AddEvent(new ChairUpdatedEvent(state.BoutId, x), Audiences.Bout(state.BoutId));
            });

            response.AddEvent(new BoutStateUpdatedEvent(state), Audiences.Bout(state.BoutId));
            return response;
        }
    }
}
