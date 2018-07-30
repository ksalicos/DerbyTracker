using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.LineupsTracker;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System.Linq;

namespace DerbyTracker.Common.Messaging.CommandHandlers.LineupsTracker
{
    [Handles("SetSkaterPositionCommand")]
    public class SetSkaterPositionCommandHandler : CommandHandlerBase<SetSkaterPositionCommand>
    {
        private readonly IBoutRunnerService _boutRunner;

        public SetSkaterPositionCommandHandler(IBoutRunnerService boutRunner)
        {
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(SetSkaterPositionCommand command)
        {
            var state = _boutRunner.GetBoutState(command.BoutId);
            var jam = state.Jams.SingleOrDefault(x => x.JamNumber == command.Jam && x.Period == command.Period);
            if (jam == null)
            { throw new JamNotFoundException(command.Period, command.Jam); }

            var roster = jam.Team(command.Team).Roster;
            var skater = roster.SingleOrDefault(x => x.Number == command.Number);
            if (skater == null) return new CommandResponse();

            skater.Position = command.Position;
            return new UpdateBoutStateResponse(state);
        }
    }
}
