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
    public class StopTimeoutCommandHandlerTests
    {
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        readonly StopTimeoutCommand _command = new StopTimeoutCommand(Guid.Empty, "originator");
        readonly StopTimeoutCommandHandler _handler;


        public StopTimeoutCommandHandlerTests()
        {
            _handler = new StopTimeoutCommandHandler(_boutRunner);
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.Phase = BoutPhase.Timeout;
            state.GameClock.Running = false;
        }

        [Fact]
        //The game clock is stopped
        public void StopTimeoutDoesntStartClock()
        {
            _handler.Handle(_command);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            Assert.False(state.GameClock.Running);
        }

        [Fact]
        public void LineupTimerStarts()
        {
            _handler.Handle(_command);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            Assert.True(DateTime.Now - state.LineupStart < TimeSpan.FromSeconds(1));
        }

        [Fact]
        //A stop timeout event is sent
        public void StopTimeoutSendsEvent()
        {
            var response = _handler.Handle(_command);
            Assert.Contains(response.Events, (x => x.Event.GetType() == typeof(BoutStateUpdatedEvent)));
        }

        [Fact]
        public void StopTimeoutSetsPhaseToLineup()
        {
            _handler.Handle(_command);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            Assert.Equal(BoutPhase.Lineup, state.Phase);
        }

        [Fact]
        public void LeftTeamLosesReviewIfTheyLose()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.LoseOfficialReview = true;
            state.RightTeamState.OfficialReviews = 1;
            state.LeftTeamState.OfficialReviews = 1;
            state.TimeoutType = TimeoutType.LeftReview;
            _handler.Handle(_command);
            Assert.Equal(1, state.RightTeamState.OfficialReviews);
            Assert.Equal(0, state.LeftTeamState.OfficialReviews);
        }

        [Fact]
        public void RightTeamLosesReviewIfTheyLose()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.LoseOfficialReview = true;
            state.RightTeamState.OfficialReviews = 1;
            state.LeftTeamState.OfficialReviews = 1;
            state.TimeoutType = TimeoutType.RightReview;
            _handler.Handle(_command);
            Assert.Equal(1, state.LeftTeamState.OfficialReviews);
            Assert.Equal(0, state.RightTeamState.OfficialReviews);
        }

        [Fact]
        public void LeftTeamLosesTimeout()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.TimeoutType = TimeoutType.LeftTeam;
            _handler.Handle(_command);
            Assert.Equal(2, state.LeftTeamState.TimeOutsRemaining);
            Assert.Equal(3, state.RightTeamState.TimeOutsRemaining);
        }

        [Fact]
        public void RightTeamLosesTimeout()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.TimeoutType = TimeoutType.RightTeam;
            _handler.Handle(_command);
            Assert.Equal(3, state.LeftTeamState.TimeOutsRemaining);
            Assert.Equal(2, state.RightTeamState.TimeOutsRemaining);
        }
    }
}
