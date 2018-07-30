using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.ScoreKeeper;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System.Linq;

namespace DerbyTracker.Common.Messaging.CommandHandlers.ScoreKeeper
{
    [Handles("UpdateJammerStatusCommand")]
    public class UpdateJammerStatusCommandHandler : CommandHandlerBase<UpdateJammerStatusCommand>
    {
        private readonly IBoutRunnerService _boutRunner;

        public UpdateJammerStatusCommandHandler(IBoutRunnerService boutRunner)
        {
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(UpdateJammerStatusCommand command)
        {
            var state = _boutRunner.GetBoutState(command.BoutId);
            var jam = state.Jams.SingleOrDefault(x => x.JamNumber == command.Jam && x.Period == command.Period);
            if (jam == null)
            { throw new JamNotFoundException(command.Period, command.Jam); }

            var team = jam.Team(command.Team);
            team.JammerStatus = command.NewStatus;

            if ((command.NewStatus == JammerStatus.Lead || command.NewStatus == JammerStatus.NotLead)
                && !team.Passes.Any(x => x.Number > 0))
            {
                team.AddPass();
            }

            return new UpdateJamResponse(command.BoutId, jam);
        }
    }
}
