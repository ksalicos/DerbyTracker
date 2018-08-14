using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.CommandHandlers.PenaltyBoxTimer;
using DerbyTracker.Common.Messaging.Commands.PenaltyBoxTimer;
using DerbyTracker.Common.Messaging.Events.PenaltyBoxTimer;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using System.Linq;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.PenaltyBoxTimer
{
    public class ButtHitSeatCommandHandlerTests
    {
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly ButtHitSeatCommand _command = new ButtHitSeatCommand(Guid.Empty, "originator",
            new Chair { Team = "left", IsJammer = false });
        private readonly ButtHitSeatCommandHandler _handler;
        private readonly BoutState _state;
        private readonly BoutStateBuilder _builder;

        public ButtHitSeatCommandHandlerTests()
        {
            _handler = new ButtHitSeatCommandHandler(_boutRunner);
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            _state = _boutRunner.GetBoutState(Guid.Empty);
            _builder = new BoutStateBuilder(_state);
            _builder.SetPhase(BoutPhase.Jam);
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

        [Fact]
        public void JammerNumberIsPopulated()
        {
            _builder.AddSkaterToJam(team: "left", number: 8, position: Position.Jammer);
            _command.Chair.IsJammer = true;
            _handler.Handle(_command);
            Assert.Contains(_state.PenaltyBox, x => x.Number == 8);
        }

        [Fact]
        public void JammerSwapHappens()
        {
            _builder.AddSkaterToBox(team: "right", number: 868, chairNumber: 2, isJammer: true, secondsServed: 15);
            _command.Chair.IsJammer = true;

            _handler.Handle(_command);

            Assert.Contains(_state.PenaltyBox, x => x.Team == "left" && x.SecondsOwed == 15);
            Assert.Contains(_state.PenaltyBox, x => x.Team == "right" && x.SecondsOwed == 15);
        }

        [Fact]
        public void JammerWithDoubleAndOtherSits()
        {
            _builder.AddSkaterToBox(team: "right", number: 868, chairNumber: 2, isJammer: true, secondsOwed: 60);
            _command.Chair.IsJammer = true;

            _handler.Handle(_command);

            Assert.Contains(_state.PenaltyBox, x => x.Team == "left" && x.SecondsOwed == 0);
            Assert.Contains(_state.PenaltyBox, x => x.Team == "right" && x.SecondsOwed == 30);
        }

        [Fact]
        public void JammerSwapSendsCorrectNumberOfEvents()
        {
            _builder.AddSkaterToBox(team: "right", number: 868, chairNumber: 2, isJammer: true, secondsServed: 15);
            _command.Chair.IsJammer = true;

            var r = _handler.Handle(_command);

            Assert.Equal(2, r.Events.Count(x => x.Event.GetType() == typeof(ChairUpdatedEvent)));
        }
    }
}
