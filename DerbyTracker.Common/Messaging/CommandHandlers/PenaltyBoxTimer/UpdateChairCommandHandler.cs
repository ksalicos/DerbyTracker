using DerbyTracker.Common.Messaging.Commands.PenaltyBoxTimer;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System.Linq;

namespace DerbyTracker.Common.Messaging.CommandHandlers.PenaltyBoxTimer
{
    [Handles("UpdateChairCommand")]
    public class UpdateChairCommandHandler : CommandHandlerBase<UpdateChairCommand>
    {
        private readonly IBoutRunnerService _boutRunner;

        public UpdateChairCommandHandler(IBoutRunnerService boutRunner)
        {
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(UpdateChairCommand command)
        {
            var state = _boutRunner.GetBoutState(command.BoutId);
            var chair = state.PenaltyBox.Single(x => x.Id == command.Chair.Id);
            chair.Number = command.Chair.Number;

            return new UpdatePenaltySeatResponse(command.BoutId, chair);
        }
    }
}
