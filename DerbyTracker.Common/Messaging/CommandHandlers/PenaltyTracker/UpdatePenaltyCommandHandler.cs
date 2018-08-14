using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.PenaltyTracker;
using DerbyTracker.Common.Messaging.Events.PenaltyTracker;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System.Linq;

namespace DerbyTracker.Common.Messaging.CommandHandlers.PenaltyTracker
{
    [Handles("UpdatePenaltyCommand")]
    public class UpdatePenaltyCommandHandler : CommandHandlerBase<UpdatePenaltyCommand>
    {
        private readonly IBoutRunnerService _boutRunner;

        public UpdatePenaltyCommandHandler(IBoutRunnerService boutRunner)
        {
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(UpdatePenaltyCommand command)
        {
            var state = _boutRunner.GetBoutState(command.BoutId);
            var target = state.Penalties.SingleOrDefault(x => x.Id == command.Penalty.Id);
            if (target == null) throw new NoSuchPenaltyException(command.Penalty.Id);
            var source = command.Penalty;
            target.Team = source.Team;
            if (target.Number != source.Number)
            {
                // If the penalty is moving off of a skater in the box, remove time
                // If it's moving to a skater in the box, add time
                var skaterInBox = state.PenaltyBox.SingleOrDefault(x => x.Number == target.Number);
                if (skaterInBox != null)
                {
                    skaterInBox.SecondsOwed -= 30;
                }

                target.Number = source.Number;

                skaterInBox = state.PenaltyBox.SingleOrDefault(x => x.Number == target.Number);
                if (skaterInBox != null)
                {
                    skaterInBox.SecondsOwed += 30;
                }
            }

            target.SecondsOwed = source.SecondsOwed;
            target.PenaltyCode = source.PenaltyCode;
            target.Period = source.Period;
            target.JamNumber = source.JamNumber;
            target.GameClock = source.GameClock;

            var response = new CommandResponse();
            response.AddEvent(new PenaltyUpdatedEvent(command.BoutId, target), Audiences.Bout(command.BoutId));
            return response;
        }
    }
}
