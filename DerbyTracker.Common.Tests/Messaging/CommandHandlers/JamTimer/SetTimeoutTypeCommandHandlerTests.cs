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
    public class SetTimeoutTypeCommandHandlerTests
    {
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly SetTimeoutTypeCommand _command = new SetTimeoutTypeCommand(Guid.Empty, "originator", TimeoutType.Official);
        private readonly SetTimeoutTypeCommandHandler _handler;

        public SetTimeoutTypeCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.Phase = BoutPhase.Timeout;
            state.GameClock.Running = false;
            _handler = new SetTimeoutTypeCommandHandler(_boutRunner);
        }

        [Fact]
        public void SettingCurrentTypeDoesNothing()
        {
            var response = _handler.Handle(_command);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            Assert.Equal(TimeoutType.Official, state.TimeoutType);
            Assert.DoesNotContain(response.Events, x => x.Event.GetType() == typeof(BoutStateUpdatedEvent));
        }

        [Fact]
        public void SettingNewTypeSendsEvent()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.TimeoutType = TimeoutType.LeftTeam;
            var response = _handler.Handle(_command);
            Assert.Equal(TimeoutType.Official, state.TimeoutType);
            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(BoutStateUpdatedEvent));
        }

        [Fact]
        public void SettingLeftTeamTimeoutRemovesTimeout()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            Assert.Equal(3, state.LeftTeamState.TimeOutsRemaining);
            _command.TimeoutType = TimeoutType.LeftTeam;
            _handler.Handle(_command);
            Assert.Equal(TimeoutType.LeftTeam, state.TimeoutType);
            Assert.Equal(2, state.LeftTeamState.TimeOutsRemaining);
        }

        [Fact]
        public void SettingRightTeamTimeoutRemovesTimeout()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            Assert.Equal(3, state.RightTeamState.TimeOutsRemaining);
            _command.TimeoutType = TimeoutType.RightTeam;
            _handler.Handle(_command);
            Assert.Equal(TimeoutType.RightTeam, state.TimeoutType);
            Assert.Equal(2, state.RightTeamState.TimeOutsRemaining);
        }

        [Fact]
        public void SettingLeftReviewSetsState()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            _command.TimeoutType = TimeoutType.LeftReview;
            _handler.Handle(_command);
            Assert.Equal(TimeoutType.LeftReview, state.TimeoutType);
        }

        [Fact]
        public void SettingRightReviewSetsState()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            _command.TimeoutType = TimeoutType.RightReview;
            _handler.Handle(_command);
            Assert.Equal(TimeoutType.RightReview, state.TimeoutType);
        }

        [Fact]
        public void SwitchingAwayFromLeftTeamTimeoutReturnsTimeout()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.TimeoutType = TimeoutType.LeftTeam;
            state.LeftTeamState.TimeOutsRemaining = 2;
            _command.TimeoutType = TimeoutType.Official;
            _handler.Handle(_command);
            Assert.Equal(3, state.LeftTeamState.TimeOutsRemaining);
        }

        [Fact]
        public void SwitchingAwayFromRightTeamTimeoutReturnsTimeout()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.TimeoutType = TimeoutType.RightTeam;
            state.RightTeamState.TimeOutsRemaining = 2;
            _command.TimeoutType = TimeoutType.Official;
            _handler.Handle(_command);
            Assert.Equal(3, state.RightTeamState.TimeOutsRemaining);
        }
    }
}
