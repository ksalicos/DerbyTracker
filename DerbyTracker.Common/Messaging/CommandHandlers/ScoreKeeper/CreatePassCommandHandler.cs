using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.ScoreKeeper;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System.Linq;

namespace DerbyTracker.Common.Messaging.CommandHandlers.ScoreKeeper
{
    [Handles("CreatePassCommand")]
    public class CreatePassCommandHandler : CommandHandlerBase<CreatePassCommand>
    {
        private readonly IBoutRunnerService _boutRunner;

        public CreatePassCommandHandler(IBoutRunnerService boutRunner)
        {
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(CreatePassCommand command)
        {
            var state = _boutRunner.GetBoutState(command.BoutId);

            var jam = state.Jams.SingleOrDefault(x => x.JamNumber == command.Jam && x.Period == command.Period);
            if (jam == null)
            { throw new JamNotFoundException(command.Period, command.Jam); }

            var passes = jam.Team(command.Team).Passes;

            //If command.Number is default, we're creating the next pass.
            //If not, we are creating a certain pass, probably zero for "Eat the Baby"
            var passNumber = command.Number == -1
                ? (passes.Any()
                    ? passes.Max(x => x.Number) + 1
                    : 1)
                : command.Number;

            passes.Add(new Pass { Number = passNumber });

            return new UpdateJamResponse(command.BoutId, jam);
        }
    }
}
