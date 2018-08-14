using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.CommandHandlers.PenaltyTracker;
using DerbyTracker.Common.Messaging.Commands.PenaltyTracker;
using DerbyTracker.Common.Messaging.Events.PenaltyTracker;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.PenaltyTracker
{
    public class UpdatePenaltyCommandHandlerTests
    {
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly Penalty _penalty1;
        private readonly Penalty _penalty2 = new Penalty("left", 2, 3, TimeSpan.FromSeconds(1), 30, 8, "G");
        private readonly UpdatePenaltyCommandHandler _handler;
        private readonly UpdatePenaltyCommand _command;
        private readonly BoutState _state;
        private readonly BoutStateBuilder _builder;

        public UpdatePenaltyCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            _state = _boutRunner.GetBoutState(Guid.Empty);
            _builder = new BoutStateBuilder(_state);
            _penalty1 = _builder.AddPenalty();

            _handler = new UpdatePenaltyCommandHandler(_boutRunner);
            _penalty2.Id = _penalty1.Id;
            _penalty2.Number = 8;
            _command = new UpdatePenaltyCommand(Guid.Empty, "originator", _penalty2);
        }

        [Fact]
        public void PenaltyIsUpdated()
        {
            var response = _handler.Handle(_command);
            var penalty = _state.Penalties[0];
            Assert.Equal(_penalty1.Id, penalty.Id);
            Assert.Equal(_penalty2.JamNumber, penalty.JamNumber);
            Assert.Equal(_penalty2.Period, penalty.Period);
            Assert.Equal(_penalty2.Team, penalty.Team);
            Assert.Equal(_penalty2.Number, penalty.Number);
            Assert.Equal(_penalty2.PenaltyCode, penalty.PenaltyCode);
            Assert.Equal(_penalty2.SecondsOwed, penalty.SecondsOwed);

            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(PenaltyUpdatedEvent));
        }

        [Fact]
        public void SkaterInBoxGetsTimeAdded()
        {
            _builder.AddSkaterToBox(number: 8);
            _handler.Handle(_command);
            Assert.Contains(_state.PenaltyBox, x => x.Number == 8 && x.SecondsOwed == 60);
        }

        [Fact]
        public void SkaterInBoxGetsTimeRemoved()
        {
            _command.Penalty.Number = 23;
            _handler.Handle(_command);
            _builder.AddSkaterToBox(number: 23, secondsOwed: 60);
            _command.Penalty.Number = 8;
            _handler.Handle(_command);
            Assert.Contains(_state.PenaltyBox, x => x.Number == 23 && x.SecondsOwed == 30);
        }
    }
}
