using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System;

namespace DerbyTracker.Common.Messaging.CommandHandlers.JamClock
{
    [Handles("StopTimeoutCommand")]
    public class StopTimeoutCommandHandler : CommandHandlerBase<StopTimeoutCommand>
    {
        private readonly IBoutRunnerService _boutRunner;

        public StopTimeoutCommandHandler(IBoutRunnerService boutRunner)
        {
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(StopTimeoutCommand command)
        {
            var state = _boutRunner.GetBoutState(command.BoutId);
            state.Phase = BoutPhase.Lineup;
            state.LineupStart = DateTime.Now;

            switch (state.TimeoutType)
            {
                case TimeoutType.Official:
                    break;
                case TimeoutType.LeftTeam:
                    state.LeftTeamState.TimeOutsRemaining--;
                    break;
                case TimeoutType.RightTeam:
                    state.RightTeamState.TimeOutsRemaining--;
                    break;
                case TimeoutType.LeftReview:
                    if (state.LoseOfficialReview)
                    { state.LeftTeamState.OfficialReviews--; }
                    break;
                case TimeoutType.RightReview:
                    if (state.LoseOfficialReview)
                    { state.RightTeamState.OfficialReviews--; }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            state.TimeoutType = TimeoutType.Official;

            var response = new UpdateBoutStateResponse(state);
            return response;
        }
    }
}
