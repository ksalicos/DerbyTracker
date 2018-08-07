using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.ScoreKeeper;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System.Linq;

namespace DerbyTracker.Common.Messaging.CommandHandlers.ScoreKeeper
{
    [Handles("UpdatePassCommand")]
    public class UpdatePassCommandHandler : CommandHandlerBase<UpdatePassCommand>
    {
        private readonly IBoutRunnerService _boutRunner;

        public UpdatePassCommandHandler(IBoutRunnerService boutRunner)
        {
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(UpdatePassCommand command)
        {
            if (command.Pass.Score > 0 && command.Pass.Number == 0)
            {
                throw new InvalidPassException("Can't update score on pass zero");
            }

            var state = _boutRunner.GetBoutState(command.BoutId);

            var jam = state.Jams.SingleOrDefault(x => x.JamNumber == command.Jam && x.Period == command.Period);
            if (jam == null)
            { throw new JamNotFoundException(command.Period, command.Jam); }

            var passes = jam.Team(command.Team).Passes;

            var pass = passes.SingleOrDefault(x => x.Number == command.Pass.Number);
            if (pass == null)
            {
                throw new NoSuchPassException(command.Pass.Number);
            }

            pass.Score = command.Pass.Score;
            pass.StarPass = command.Pass.StarPass;

            //If adding score to the last pass of the current running jam, add a pass.
            if (jam == state.Jams.Last() && state.GameClock.Running && pass == passes.Last())
            {
                jam.Team(command.Team).AddPass();
            }

            return new UpdateJamResponse(command.BoutId, jam);
        }
    }
}
