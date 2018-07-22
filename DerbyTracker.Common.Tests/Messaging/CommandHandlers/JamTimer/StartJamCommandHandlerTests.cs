
using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.CommandHandlers.JamClock;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Messaging.Events.JamClock;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using DerbyTracker.Common.Messaging.Events.Bout;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.JamTimer
{
    public class StartJamCommandHandlerTests
    {
        private readonly IBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();

        public StartJamCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
        }

        [Fact]
        public void StartJamDoesTheThingsThatStartJamShouldDo()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.Phase = BoutPhase.Lineup;

            var command = new StartJamCommand(Guid.Empty, "connection");
            var handler = new StartJamCommandHandler(_boutRunner, _boutData);
            var response = handler.Handle(command);

            Assert.True(state.ClockRunning);
            Assert.Equal(BoutPhase.Jam, state.Phase);
            Assert.True(state.JamClock().TotalSeconds < 1);
            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(BoutStateUpdatedEvent));
        }
    }
}
