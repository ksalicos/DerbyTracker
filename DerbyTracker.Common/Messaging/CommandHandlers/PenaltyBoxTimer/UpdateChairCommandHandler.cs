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
        private readonly IBoutDataService _boutData;

        public UpdateChairCommandHandler(IBoutRunnerService boutRunner, IBoutDataService boutData)
        {
            _boutRunner = boutRunner;
            _boutData = boutData;
        }

        public override ICommandResponse Handle(UpdateChairCommand command)
        {
            var state = _boutRunner.GetBoutState(command.BoutId);
            var chair = state.PenaltyBox.Single(x => x.Id == command.Chair.Id);
            chair.SecondsOwed = command.Chair.SecondsOwed;
            if (chair.Number != command.Chair.Number)
            {
                chair.Number = command.Chair.Number;
                chair.SecondsOwed = state.Penalties.Any(x => x.Number == chair.Number)
                    ? state.Penalties.Where(x => x.Number == chair.Number).Sum(x => x.SecondsOwed)
                    : chair.SecondsOwed;
            }
            chair.IsJammer = command.Chair.IsJammer;
            chair.ChairNumber = command.Chair.ChairNumber;

            return new UpdatePenaltySeatResponse(command.BoutId, chair);
        }
    }
}
