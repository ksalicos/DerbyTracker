using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.CommandHandlers.Bout;
using DerbyTracker.Common.Messaging.Commands.Bout;
using DerbyTracker.Common.Messaging.Events.Bout;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.Bout
{
    public class RunBoutCommandHandlerTests
    {
        private readonly IBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();

        [Fact]
        public void BoutCanBeRun()
        {
            var command = new RunBoutCommand(Guid.Empty, "Test");
            var handler = new RunBoutCommandHandler(_boutRunner, _boutData);
            var result = handler.Handle(command);
            Assert.Contains(result.Events, x => x.Event.GetType() == typeof(BoutRunningEvent));
        }

        [Fact]
        public void BoutCantBeRunTwice()
        {
            var command = new RunBoutCommand(Guid.Empty, "Test");
            var handler = new RunBoutCommandHandler(_boutRunner, _boutData);
            handler.Handle(command);
            Assert.Throws<BoutAlreadyRunningException>(() => { handler.Handle(command); });
        }
    }
}
