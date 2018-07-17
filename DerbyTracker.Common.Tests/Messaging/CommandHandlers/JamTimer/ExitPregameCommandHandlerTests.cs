using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Enums;
using DerbyTracker.Common.Messaging.CommandHandlers.JamClock;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.JamTimer
{
    public class ExitPregameCommandHandlerTests
    {
        private readonly IBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly INodeService _nodeService;

        public ExitPregameCommandHandlerTests()
        {
            _nodeService = new NodeService(_boutRunner);
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            _nodeService.ConnectNode("nodeId", "connection");
            _nodeService.AddRole("nodeId", NodeRoles.JamTimer);
        }

        [Fact]
        public void ExitPregameGoesToLineupPhase()
        {
            var command = new ExitPregameCommand("test", Guid.Empty, "connection");
            var handler = new ExitPregameCommandHandler(_boutRunner, _boutData, _nodeService);
            handler.Handle(command);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            Assert.Equal(BoutPhase.Lineup, state.Phase);
        }
    }
}
