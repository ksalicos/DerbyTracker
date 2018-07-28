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
    public class CreatePenaltyCommandHandlerTests
    {
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly CreatePenaltyCommand _command = new CreatePenaltyCommand(Guid.Empty, "originator", 1, 1, "left");
        private readonly CreatePenaltyCommandHandler _handler;
        private readonly BoutState _state;

        public CreatePenaltyCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            _state = _boutRunner.GetBoutState(Guid.Empty);
            _state.Phase = BoutPhase.Lineup;

            _handler = new CreatePenaltyCommandHandler(_boutRunner);
        }

        [Fact]
        public void PenaltyIsAdded()
        {
            var response = _handler.Handle(_command);
            var penalty = _state.Penalties[0];
            Assert.Equal("left", penalty.Team);
            Assert.Equal(-1, penalty.Number);
            Assert.Null(penalty.PenaltyCode);
            Assert.Equal(1, penalty.Period);
            Assert.Equal(1, penalty.JamNumber);

            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(PenaltyUpdatedEvent));
        }
    }
}
