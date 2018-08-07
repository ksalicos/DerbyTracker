using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;

namespace DerbyTracker.Common.Messaging.CommandHandlers.JamClock
{
    [Handles("EndPeriodCommand")]
    public class EndPeriodCommandHandler : CommandHandlerBase<EndPeriodCommand>
    {
        private readonly IBoutRunnerService _boutRunner;
        private readonly IBoutDataService _boutData;

        public EndPeriodCommandHandler(IBoutRunnerService boutRunner, IBoutDataService boutData)
        {
            _boutRunner = boutRunner;
            _boutData = boutData;
        }

        public override ICommandResponse Handle(EndPeriodCommand command)
        {
            var state = _boutRunner.GetBoutState(command.BoutId);
            if (state.Phase != Entities.BoutPhase.Lineup)
            { throw new InvalidBoutPhaseException(state.Phase); }
            var bout = _boutData.Load(command.BoutId);

            if (state.GameClock.Elapsed.TotalSeconds < bout.RuleSet.PeriodDurationSeconds)
            { return new CommandResponse(); }

            if (state.Period < bout.RuleSet.NumberOfPeriods)
            {
                state.Phase = BoutPhase.Halftime;
                state.Period++;
                state.JamNumber = 1;
                state.Jams.Add(new Jam(state.Period, state.JamNumber));
            }
            else
            {
                state.Phase = Entities.BoutPhase.UnofficialFinal;
            }

            var response = new UpdateBoutStateResponse(state);
            return response;
        }
    }
}
