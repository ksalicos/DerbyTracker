using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.CommandHandlers.JamClock;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Messaging.Events.Bout;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.JamTimer
{
    public class SetLoseOfficialReviewCommandHandlerTests
    {
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();

        private readonly SetLoseOfficialReviewCommand _command = new SetLoseOfficialReviewCommand(Guid.Empty, "originator", true);
        private readonly SetLoseOfficialReviewCommandHandler _handler;

        public SetLoseOfficialReviewCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.Phase = BoutPhase.Timeout;
            state.GameClock.Running = false;
            _handler = new SetLoseOfficialReviewCommandHandler(_boutRunner);
        }

        [Fact]
        public void LoseSetToKeep()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.LoseOfficialReview = true;
            _command.LoseOfficialReview = false;
            var response = _handler.Handle(_command);
            Assert.False(state.LoseOfficialReview);
            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(BoutStateUpdatedEvent));
        }

        [Fact]
        public void KeepSetToLose()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.LoseOfficialReview = false;
            _command.LoseOfficialReview = true;
            var response = _handler.Handle(_command);
            Assert.True(state.LoseOfficialReview);
            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(BoutStateUpdatedEvent));
        }
    }
}
