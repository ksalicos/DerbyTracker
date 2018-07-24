using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.LineupsTracker;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System.Collections.Generic;
using System.Linq;

namespace DerbyTracker.Common.Messaging.CommandHandlers.LineupsTracker
{
    [Handles("AddSkaterToJamCommand")]
    public class AddSkaterToJamCommandHandler : CommandHandlerBase<AddSkaterToJamCommand>
    {
        private readonly IBoutRunnerService _boutRunner;
        private readonly IBoutDataService _boutData;

        public AddSkaterToJamCommandHandler(IBoutDataService boutData, IBoutRunnerService boutRunner)
        {
            _boutData = boutData;
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(AddSkaterToJamCommand command)
        {
            var bout = _boutData.Load(command.BoutId);
            var state = _boutRunner.GetBoutState(command.BoutId);
            var jam = state.Jams.SingleOrDefault(x => x.JamNumber == command.Jam && x.Period == command.Period);
            if (jam == null)
            { throw new JamNotFoundException(command.Period, command.Jam); }

            List<Skater> roster;
            List<JamParticipant> lineup;

            if (command.Team == "left")
            {
                roster = bout.LeftTeam.Roster;
                lineup = jam.LeftRoster;
            }
            else
            {
                roster = bout.RightTeam.Roster;
                lineup = jam.RightRoster;
            }

            if (roster.All(x => x.Number != command.Number))
            { throw new InvalidSkaterNumberException(command.Team, command.Number); }

            if (lineup.Any(x => x.Number == command.Number))
            { return new CommandResponse(); }

            lineup.Add(new JamParticipant { Number = command.Number, Position = Position.Blocker });

            var response = new UpdateBoutStateResponse(state);
            return response;
        }
    }
}
