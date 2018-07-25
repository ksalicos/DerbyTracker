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
            state.Jams[0].LeftRoster.Add(new JamParticipant { Number = 8, Position = Position.Blocker });
            state.Phase = BoutPhase.Lineup;

            _handler = new SetSkaterPositionCommandHandler(_boutRunner);
        }

        [Fact]
        public void LethalIsAJammer()
        {
            _handler.Handle(_command);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            Assert.Equal(Position.Jammer, state.Jams[0].LeftRoster[0].Position);
        }

        [Fact]
        public void InvalidSkaterDoesntCrash()
        {
            _command.Number = -1;
            _handler.Handle(_command);
        }
    }
}
