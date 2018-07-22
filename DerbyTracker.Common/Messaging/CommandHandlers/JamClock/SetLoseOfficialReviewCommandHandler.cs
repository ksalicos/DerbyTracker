using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;

namespace DerbyTracker.Common.Messaging.CommandHandlers.JamClock
{
    [Handles("SetLoseOfficialReviewCommand")]
    public class SetLoseOfficialReviewCommandHandler : CommandHandlerBase<SetLoseOfficialReviewCommand>
    {
        private readonly IBoutRunnerService _boutRunner;

        public SetLoseOfficialReviewCommandHandler(IBoutRunnerService boutRunner)
        {
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(SetLoseOfficialReviewCommand command)
        {
            var state = _boutRunner.GetBoutState(command.BoutId);
            if (state.Phase != BoutPhase.Timeout)
            { throw new InvalidBoutPhaseException(state.Phase); }

            if (command.LoseOfficialReview == state.LoseOfficialReview)
            { return new CommandResponse(); }

            state.LoseOfficialReview = command.LoseOfficialReview;

            var response = new UpdateBoutStateResponse(state);
            return response;
        }
    }
}
