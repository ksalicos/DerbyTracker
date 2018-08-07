using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.PenaltyBoxTimer;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System.Linq;

namespace DerbyTracker.Common.Messaging.CommandHandlers.PenaltyBoxTimer
{
    [Handles("ButtHitSeatCommand")]
    public class ButtHitSeatCommandHandler : CommandHandlerBase<ButtHitSeatCommand>
    {
        private readonly IBoutRunnerService _boutRunner;

        public ButtHitSeatCommandHandler(IBoutRunnerService boutRunner)
        {
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(ButtHitSeatCommand command)
        {
            var state = _boutRunner.GetBoutState(command.BoutId);

            if (state.PenaltyBox.Any(x => x.Id == command.Chair.Id))
            {
                throw new ButtAlreadyInChairException(command.Chair.Id);
            }

            if (state.Phase == Entities.BoutPhase.Jam)
            {
                command.Chair.StopWatch.Start();
            }

            state.PenaltyBox.Add(command.Chair);
            return new UpdatePenaltySeatResponse(command.BoutId, command.Chair);
        }
    }
}
