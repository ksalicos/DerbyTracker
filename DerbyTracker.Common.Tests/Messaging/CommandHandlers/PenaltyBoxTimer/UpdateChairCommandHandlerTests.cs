using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.CommandHandlers.PenaltyBoxTimer;
using DerbyTracker.Common.Messaging.Commands.PenaltyBoxTimer;
using DerbyTracker.Common.Messaging.Events.PenaltyBoxTimer;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.PenaltyBoxTimer
{
    public class UpdateChairCommandHandlerTests
    {
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly UpdateChairCommand _command;
        private readonly UpdateChairCommandHandler _handler;
        private readonly BoutState _state;
        private readonly BoutStateBuilder _builder;

        public UpdateChairCommandHandlerTests()
        {
            _handler = new UpdateChairCommandHandler(_boutRunner, _boutData);
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            _state = _boutRunner.GetBoutState(Guid.Empty);
            _builder = new BoutStateBuilder(_state);
            var inBox = _builder.AddSkaterToBox(number: -1);
            _command = new UpdateChairCommand(Guid.Empty, "originator",
                new Chair { Id = inBox.Id, Team = "left", IsJammer = false, Number = 8 });
        }

        [Fact]
        public void CanUpdateNumber()
        {
            _handler.Handle(_command);
            Assert.Equal(8, _state.PenaltyBox[0].Number);
        }

        [Fact]
        public void CanUpdateTimeOwed()
        {
            _command.Chair.Number = -1;
            _command.Chair.SecondsOwed = 60;
            _handler.Handle(_command);
            Assert.Contains(_state.PenaltyBox, x => x.SecondsOwed == 60);
        }

        [Fact]
        public void ReturnsProperEvent()
        {
            var response = _handler.Handle(_command);
            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(ChairUpdatedEvent));
        }

        [Fact]
        public void AddsTimeIfSkaterReallyNaughty()
        {
            _builder.AddPenalty(number: 8);
            _builder.AddPenalty(number: 8);
            _handler.Handle(_command);
            Assert.Contains(_state.PenaltyBox, x => x.SecondsOwed == 60 && x.Number == 8);
        }

        [Fact]
        public void SetsTimeIfChangedToLessNaughtySkater()
        {
            _builder.AddSkaterToBox(number: 8, secondsOwed: 60);
            _builder.AddPenalty(number: 23);
            _command.Chair.Number = 23;
            _command.Chair.SecondsOwed = 30;
            _handler.Handle(_command);
            Assert.Contains(_state.PenaltyBox, x => x.SecondsOwed == 30 && x.Number == 23);
        }
    }
}
