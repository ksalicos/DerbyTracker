using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.CommandHandlers.LineupsTracker;
using DerbyTracker.Common.Messaging.Commands.LineupsTracker;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.LineupsTracker
{
    public class SetSkaterPositionCommandHandlerTests
    {
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly SetSkaterPositionCommand _command =
            new SetSkaterPositionCommand(Guid.Empty, "originator", 1, 1, "left", 8, Position.Jammer);
        private readonly SetSkaterPositionCommandHandler _handler;

        public SetSkaterPositionCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            var team = state.Jams[0].Team("left");
            team.Roster.Add(new JamParticipant { Number = 8, Position = Position.Blocker });
            state.Phase = BoutPhase.Lineup;

            _handler = new SetSkaterPositionCommandHandler(_boutRunner);
        }

        [Fact]
        public void LethalIsAJammer()
        {
            _handler.Handle(_command);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            var team = state.Jams[0].Team("left");
            Assert.Equal(Position.Jammer, team.Roster[0].Position);
        }

        [Fact]
        public void InvalidSkaterDoesntCrash()
        {
            _command.Number = -1;
            _handler.Handle(_command);
        }
    }
}
