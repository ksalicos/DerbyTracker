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
    public class CancelSitCommandHandlerTests
    {
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly Chair _toUpdate = new Chair { Team = "left", IsJammer = false, Number = 8 };
        private readonly CancelSitCommand _command;
        private readonly CancelSitCommandHandler _handler;
        private readonly BoutState _state;


        public CancelSitCommandHandlerTests()
        {
            _command = new CancelSitCommand(Guid.Empty, "o", _toUpdate.Id);
            _handler = new CancelSitCommandHandler(_boutRunner);
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            _state = _boutRunner.GetBoutState(Guid.Empty);
            _state.PenaltyBox.Add(_toUpdate);
            _state.Phase = BoutPhase.Jam;
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
    }
}
