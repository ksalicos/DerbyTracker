using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.CommandHandlers.JamClock;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Messaging.Events.Bout;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.JamTimer
{
    public class StartTimeoutCommandHandlerTests
    {
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly StartTimeoutCommand _command = new StartTimeoutCommand(Guid.Empty, "originator");
        private readonly StartTimeoutCommandHandler _handler;

        public StartTimeoutCommandHandlerTests()
        {
            _handler = new StartTimeoutCommandHandler(_boutRunner);

            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.Phase = BoutPhase.Lineup;
            state.GameClock.Running = true;
        }

        [Fact]
        //The game clock is stopped
        public void StartTimeoutStopsTheClock()
        {
            _handler.Handle(_command);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            Assert.False(state.GameClock.Running);
        }

        [Fact]
        public void CantStartTimeoutWhileNotInLineup()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.Phase = BoutPhase.Jam;
            Assert.Throws<InvalidBoutPhaseException>(() => { _handler.Handle(_command); });
        }

        [Fact]
        //A timeout event is sent
        public void StartTimeoutSendsEvent()
        {
            var response = _handler.Handle(_command);
            Assert.Contains(response.Events, (x => x.Event.GetType() == typeof(BoutStateUpdatedEvent)));
        }

        [Fact]
        public void StartTimeoutSetsPhaseToTimeout()
        {
            _handler.Handle(_command);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            Assert.Equal(BoutPhase.Timeout, state.Phase);
        }

        [Fact]
        public void TimeoutTypeSetToOfficial()
        {
            _handler.Handle(_command);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            Assert.Equal(TimeoutType.Official, state.TimeoutType);
        }
    }
}
