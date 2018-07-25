using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.LineupsTracker;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System.Linq;

namespace DerbyTracker.Common.Messaging.CommandHandlers.LineupsTracker
{
    [Handles("RemoveSkaterFromJamCommand")]
    public class RemoveSkaterFromJamCommandHandler : CommandHandlerBase<RemoveSkaterFromJamCommand>
    {
        private readonly IBoutRunnerService _boutRunner;

        public RemoveSkaterFromJamCommandHandler(IBoutRunnerService boutRunner)
        {
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(RemoveSkaterFromJamCommand command)
        {
            var state = _boutRunner.GetBoutState(command.BoutId);
            var jam = state.Jams.SingleOrDefault(x => x.JamNumber == command.Jam && x.Period == command.Period);
            if (jam == null)
            { throw new JamNotFoundException(command.Period, command.Jam); }

            var roster = command.Team == "left" ? jam.LeftRoster : jam.RightRoster;

            roster.RemoveAll(x => x.Number == command.Number);

            return new UpdateBoutStateResponse(state);
        }
    }
}
