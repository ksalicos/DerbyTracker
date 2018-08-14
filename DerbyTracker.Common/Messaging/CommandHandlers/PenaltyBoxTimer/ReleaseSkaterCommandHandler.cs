using DerbyTracker.Common.Messaging.Commands.PenaltyBoxTimer;
using DerbyTracker.Common.Messaging.Events.PenaltyBoxTimer;
using DerbyTracker.Common.Messaging.Events.PenaltyTracker;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System.Linq;

namespace DerbyTracker.Common.Messaging.CommandHandlers.PenaltyBoxTimer
{
    [Handles("ReleaseSkaterCommand")]
    public class ReleaseSkaterCommandHandler : CommandHandlerBase<ReleaseSkaterCommand>
    {
        private readonly IBoutRunnerService _boutRunner;

        public ReleaseSkaterCommandHandler(IBoutRunnerService boutRunner)
        {
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(ReleaseSkaterCommand command)
        {
            var response = new CommandResponse();
            var state = _boutRunner.GetBoutState(command.BoutId);
            var sit = state.PenaltyBox.SingleOrDefault(x => x.Id == command.ChairId);
            if (sit == null) return response;

            var timeServed = sit.SecondsOwed;
            var penalties = state.Penalties.Where(x => x.Team == sit.Team && x.Number == sit.Number).ToList();
            while (timeServed > 0 && penalties.Any())
            {
                var penalty = penalties.First();
                if (timeServed > penalty.SecondsOwed)
                {
                    timeServed -= penalty.SecondsOwed;
                    penalty.SecondsOwed = 0;
                    penalties.Remove(penalty);
                }
                else
                {
                    penalty.SecondsOwed -= timeServed;
                    timeServed = 0;
                }

                response.AddEvent(new PenaltyUpdatedEvent(command.BoutId, penalty), Audiences.Bout(command.BoutId));
            }

            state.PenaltyBox.Remove(sit);
            response.AddEvent(new ChairRemovedEvent(command.BoutId, command.ChairId),
                Audiences.Bout(command.BoutId));
            return response;
        }
    }
}