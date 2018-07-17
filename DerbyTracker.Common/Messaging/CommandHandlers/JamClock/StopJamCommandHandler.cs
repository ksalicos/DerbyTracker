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
    [Handles("StopJamCommand")]
    public class StopJamCommandHandler : CommandHandlerBase<StopJamCommand>
    {

        private readonly IBoutDataService _boutDataService;
        private readonly IBoutRunnerService _boutRunnerService;

        public StopJamCommandHandler(IBoutRunnerService boutRunnerService, IBoutDataService boutDataService)
        {
            _boutRunnerService = boutRunnerService;
            _boutDataService = boutDataService;
        }

        public override ICommandResponse Handle(StopJamCommand command)
        {
            var response = new CommandResponse();
            var bout = _boutDataService.Load(command.BoutId);
            var state = _boutRunnerService.GetBoutState(command.BoutId);

            //State must be in jam
            if (state.Phase != BoutPhase.Jam)
            { throw new InvalidBoutPhaseException(state.Phase); }

            response.AddEvent(new JamEndedEvent(command.BoutId), Audiences.All);

            //Check to see if bout is over
            if (state.GameClock().Seconds < bout.RuleSet.PeriodDurationSeconds)
            {
                //Still good
                state.LineupStart = DateTime.Now;
                state.Phase = BoutPhase.Lineup;
                state.Jam++;

                //reset jam scores here

                //keep the jam clock going in case of undo
            }
            else
            {
                //Intermission
                if (state.Period < bout.RuleSet.NumberOfPeriods)
                {
                    response.AddEvent(new PeriodEndedEvent(command.BoutId), Audiences.All);
                    state.Jam = 1;
                    state.Period++;
                    state.LastClockStart = DateTime.Now;
                    state.Phase = BoutPhase.Halftime;
                }
                else
                //Game Over
                {
                    state.Phase = BoutPhase.UnofficialScore;
                    response.AddEvent(new BoutEndedEvent(command.BoutId), Audiences.All);
                }
            }


            state.JamStart = DateTime.Now;
            state.ClockRunning = true;

            return response;
        }
    }
}
