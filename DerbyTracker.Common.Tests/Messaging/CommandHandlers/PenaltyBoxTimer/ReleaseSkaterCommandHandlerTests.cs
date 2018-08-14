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
    public class ReleaseSkaterCommandHandlerTests
    {
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly Chair _toUpdate;
        private readonly ReleaseSkaterCommand _command;
        private readonly ReleaseSkaterCommandHandler _handler;
        private readonly BoutState _state;
        private readonly BoutStateBuilder _builder;

        public ReleaseSkaterCommandHandlerTests()
        {
            _handler = new ReleaseSkaterCommandHandler(_boutRunner);
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            _state = _boutRunner.GetBoutState(Guid.Empty);
            _builder = new BoutStateBuilder(_state);
            _toUpdate = _builder.AddSkaterToBox(team: "left", number: 8);
            _command = new ReleaseSkaterCommand(Guid.Empty, "o", _toUpdate.Id);
        }


        [Fact]
        public void RemovesSit()
        {
            Assert.Contains(_state.PenaltyBox, x => x.Id == _command.ChairId);
            _handler.Handle(_command);
            Assert.DoesNotContain(_state.PenaltyBox, x => x.Id == _command.ChairId);
        }

        [Fact]
        public void ReturnsProperEvent()
        {
            var response = _handler.Handle(_command);
            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(ChairRemovedEvent));
        }

        [Fact]
        public void PenaltyTimeIsCredited()
        {
            _builder.AddPenalty(number: 8);
            _handler.Handle(_command);
            Assert.Equal(0, _state.Penalties[0].SecondsOwed);
        }

    }
}
