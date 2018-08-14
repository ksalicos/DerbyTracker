using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Commands.PenaltyTracker;
using DerbyTracker.Common.Messaging.Events.PenaltyTracker;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;

namespace DerbyTracker.Common.Messaging.CommandHandlers.PenaltyTracker
{
    [Handles("CreatePenaltyCommand")]
    public class CreatePenaltyCommandHandler : CommandHandlerBase<CreatePenaltyCommand>
    {
        private readonly IBoutRunnerService _boutRunner;
        private readonly IBoutDataService _boutData;

        public CreatePenaltyCommandHandler(IBoutRunnerService boutRunner, IBoutDataService boutData)
        {
            _boutRunner = boutRunner;
            _boutData = boutData;
        }

        public override ICommandResponse Handle(CreatePenaltyCommand command)
        {
            var state = _boutRunner.GetBoutState(command.BoutId);
            var data = _boutData.Load(command.BoutId);

            var penalty = new Penalty(command.Team, command.Period, command.Jam, state.GameClock.Elapsed, data.RuleSet.PenaltyDurationSeconds);
            state.Penalties.Add(penalty);

            var response = new CommandResponse();
            response.AddEvent(new PenaltyUpdatedEvent(command.BoutId, penalty), Audiences.Bout(command.BoutId));
            return response;
        }
    }
}
