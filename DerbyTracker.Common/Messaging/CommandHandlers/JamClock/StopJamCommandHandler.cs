using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Messaging.Events.PenaltyBoxTimer;
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
            var bout = _boutDataService.Load(command.BoutId);
            var state = _boutRunnerService.GetBoutState(command.BoutId);

            //State must be in jam
            if (state.Phase != BoutPhase.Jam)
            { throw new InvalidBoutPhaseException(state.Phase); }

            //Check to see if bout is over
            if (state.GameClock.Elapsed.Seconds < bout.RuleSet.PeriodDurationSeconds)
            {
                //Still good
                state.LineupStart = DateTime.Now;
                state.Phase = BoutPhase.Lineup;
                state.CreateNextJam();

                //keep the jam clock going in case of undo
            }
            else
            {
                //Intermission
                if (state.Period < bout.RuleSet.NumberOfPeriods)
                {
                    state.JamNumber = 1;
                    state.Period++;
                    state.GameClock.Clear();
                    state.Phase = BoutPhase.Halftime;
                }
                else
                //Game Over
                {
                    state.Phase = BoutPhase.UnofficialFinal;
                }
            }


            state.JamStart = DateTime.Now;
            state.GameClock.Start();

            var response = new UpdateBoutStateResponse(state);

            state.PenaltyBox.ForEach(x =>
            {
                x.StopWatch.Stop();
                response.AddEvent(new ChairUpdatedEvent(state.BoutId, x), Audiences.Bout(state.BoutId));
            });

            return response;
        }
    }
}
