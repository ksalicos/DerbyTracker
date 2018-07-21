using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.CommandHandlers.JamClock;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Messaging.Events.JamClock;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using DerbyTracker.Messaging.Commands;
using System;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.JamTimer
{
    public class StartTimeoutCommandHandlerTests
    {
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly ICommandResponse _response;

        public StartTimeoutCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.Phase = BoutPhase.Lineup;
            state.ClockRunning = true;
            var command = new StartTimeoutCommand(Guid.Empty, "originator");
            var handler = new StartTimeoutCommandHandler(_boutRunner);
            _response = handler.Handle(command);
        }

        [Fact]
        //The game clock is stopped
        public void StartTimeoutStopsTheClock()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            Assert.False(state.ClockRunning);
        }

        [Fact]
        //A timeout event is sent
        public void StartTimeoutSendsEvent()
        {
            Assert.Contains(_response.Events, (x => x.Event.GetType() == typeof(TimeoutStartedEvent)));
        }

        [Fact]
        public void StartTimeoutSetsPhaseToTimeout()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            Assert.Equal(BoutPhase.Timeout, state.Phase);
        }
    }
}
