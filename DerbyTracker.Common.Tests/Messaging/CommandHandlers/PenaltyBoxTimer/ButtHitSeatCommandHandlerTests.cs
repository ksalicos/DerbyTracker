using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.CommandHandlers.PenaltyBoxTimer;
using DerbyTracker.Common.Messaging.Commands.PenaltyBoxTimer;
using DerbyTracker.Common.Messaging.Events.PenaltyBoxTimer;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.PenaltyBoxTimer
{
    public class ButtHitSeatCommandHandlerTests
    {
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly ButtHitSeatCommand _command = new ButtHitSeatCommand(Guid.Empty, "originator", new Chair { Team = "left", IsJammer = false });
        private readonly ButtHitSeatCommandHandler _handler;
        private readonly BoutState _state;

        public ButtHitSeatCommandHandlerTests()
        {
            _handler = new ButtHitSeatCommandHandler(_boutRunner);
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            _state = _boutRunner.GetBoutState(Guid.Empty);
            _state.Phase = BoutPhase.Jam;
        }

        [Fact]
        public void SitIsCreatedForTeam()
        {
            _handler.Handle(_command);
            Assert.Contains(_state.PenaltyBox, x => x.Team == "left");
        }

        [Fact]
        public void ReturnsProperEvent()
        {
            var response = _handler.Handle(_command);
            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(ChairUpdatedEvent));
        }

        [Fact]
        public void CanIndicateJammerChair()
        {
            _command.Chair.IsJammer = true;
            _handler.Handle(_command);
            Assert.Contains(_state.PenaltyBox, x => x.IsJammer);
        }

        [Fact]
        public void CantReAddSit()
        {
            _state.PenaltyBox.Add(_command.Chair);
            Assert.Throws<ButtAlreadyInChairException>(() => _handler.Handle(_command));
        }

    }
}
