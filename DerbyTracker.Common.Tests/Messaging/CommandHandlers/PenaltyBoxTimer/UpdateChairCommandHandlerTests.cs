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
        private readonly Chair _toUpdate = new Chair { Team = "left", IsJammer = false, Number = 8 };
        private readonly UpdateChairCommand _command;
        private readonly UpdateChairCommandHandler _handler;
        private readonly BoutState _state;

        public UpdateChairCommandHandlerTests()
        {
            _command = new UpdateChairCommand(Guid.Empty, "originator", _toUpdate);
            _handler = new UpdateChairCommandHandler(_boutRunner);
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            _state = _boutRunner.GetBoutState(Guid.Empty);
            _state.PenaltyBox.Add(_toUpdate);
            _state.Phase = BoutPhase.Jam;
        }

        [Fact]
        public void CanUpdateNumber()
        {
            _handler.Handle(_command);
            Assert.Equal(8, _state.PenaltyBox[0].Number);
        }

        [Fact]
        public void ReturnsProperEvent()
        {
            var response = _handler.Handle(_command);
            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(ChairUpdatedEvent));
        }
    }
}
