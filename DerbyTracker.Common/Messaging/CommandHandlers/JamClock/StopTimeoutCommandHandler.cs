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

            if (state.TimeoutType == TimeoutType.LeftReview && state.LoseOfficialReview)
            {
                state.LeftTeamState.OfficialReviews--;
            }

            if (state.TimeoutType == TimeoutType.RightReview && state.LoseOfficialReview)
            {
                state.RightTeamState.OfficialReviews--;
            }

            var response = new UpdateBoutStateResponse(state);
            return response;
        }
    }
}
