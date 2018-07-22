using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.CommandHandlers.JamClock;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.JamTimer
{
    public class StartPeriodCommandHandlerTests
    {
        private readonly IBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();

        public StartPeriodCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
        }

        [Fact]
        public void ExitPregameGoesToLineupPhase()
        {
            var command = new StartPeriodCommand(Guid.Empty, "connection");
            var handler = new StartPeriodCommandHandler(_boutRunner, _boutData);
            handler.Handle(command);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            Assert.Equal(BoutPhase.Lineup, state.Phase);
        }
    }
}
