using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.CommandHandlers.LineupsTracker;
using DerbyTracker.Common.Messaging.Commands.LineupsTracker;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.LineupsTracker
{
    public class AddSkaterToJamCommandHandlerTests
    {
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly AddSkaterToJamCommand _command =
            new AddSkaterToJamCommand(Guid.Empty, "originator", 1, 1, "left", 8);
        private readonly AddSkaterToJamCommandHandler _handler;

        public AddSkaterToJamCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.Phase = BoutPhase.Lineup;

            _handler = new AddSkaterToJamCommandHandler(_boutData, _boutRunner);
        }

        [Fact]
        public void AddingSkaterToJamDoesSo()
        {
            _handler.Handle(_command);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            var team = state.Jams[0].Team("left");
            Assert.Contains(team.Roster, x => x.Number == 8);
        }

        [Fact]
        public void RightAlsoWorks()
        {
            _command.Team = "right";
            _command.Number = 868;
            _handler.Handle(_command);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            var team = state.Jams[0].Team("right");
            Assert.Contains(team.Roster, x => x.Number == 868);
        }

        [Fact]
        public void AddingSkaterTwiceDoesntThrow()
        {
            _handler.Handle(_command);
            _handler.Handle(_command);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            var team = state.Jams[0].Team("left");
            Assert.Contains(team.Roster, x => x.Number == 8);
        }

        [Fact]
        public void AddingSkaterNotOnRosterThrows()
        {
            _command.Number = -1;
            Assert.Throws<InvalidSkaterNumberException>(() =>
            {
                _handler.Handle(_command);
            });
        }

        [Fact]
        public void CantAddMoreThanSix()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.Jams.Last().Left.Roster = new List<JamParticipant>
            {
                new JamParticipant { },
                new JamParticipant { },
                new JamParticipant { },
                new JamParticipant { },
                new JamParticipant { },
                new JamParticipant { },
            };
            _handler.Handle(_command);
            Assert.Equal(6, state.Jams.Last().Left.Roster.Count);
        }

    }
}
