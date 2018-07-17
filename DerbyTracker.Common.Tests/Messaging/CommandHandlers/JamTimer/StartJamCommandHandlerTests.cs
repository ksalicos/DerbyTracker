
using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Enums;
using DerbyTracker.Common.Messaging.CommandHandlers.JamClock;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Messaging.Events.JamClock;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.JamTimer
{
    public class StartJamCommandHandlerTests
    {
        private readonly IBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly INodeService _nodeService;

        public StartJamCommandHandlerTests()
        {
            _nodeService = new NodeService(_boutRunner);
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            _nodeService.ConnectNode("nodeId", "connection");
            _nodeService.AddRole("nodeId", NodeRoles.JamTimer);
        }

        [Fact]
        public void StartJamDoesTheThingsThatStartJamShouldDo()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.Phase = Entities.BoutPhase.Lineup;

            var command = new StartJamCommand("nodeId", Guid.Empty, "connection");
            var handler = new StartJamCommandHandler(_boutRunner, _boutData);
            var response = handler.Handle(command);

            Assert.True(state.ClockRunning);
            Assert.Equal(BoutPhase.Jam, state.Phase);
            Assert.True(state.JamClock().TotalSeconds < 1);
            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(JamStartedEvent));
        }
    }
}
