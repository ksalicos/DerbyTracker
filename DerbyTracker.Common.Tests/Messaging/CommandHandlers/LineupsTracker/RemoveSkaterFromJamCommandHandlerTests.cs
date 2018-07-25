using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.CommandHandlers.LineupsTracker;
using DerbyTracker.Common.Messaging.Commands.LineupsTracker;
using DerbyTracker.Common.Messaging.Events.Bout;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.LineupsTracker
{
    public class RemoveSkaterFromJamCommandHandlerTests
    {
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly RemoveSkaterFromJamCommand _command =
            new RemoveSkaterFromJamCommand(Guid.Empty, "originator", 1, 1, "left", 8);
        private readonly RemoveSkaterFromJamCommandHandler _handler;

        public RemoveSkaterFromJamCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.Phase = BoutPhase.Lineup;
            state.Jams[0].LeftRoster.Add(new JamParticipant { Number = 8, Position = Position.Blocker });
            _handler = new RemoveSkaterFromJamCommandHandler(_boutRunner);
        }

        [Fact]
        public void SkaterIsRemoved()
        {
            _handler.Handle(_command);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            Assert.DoesNotContain(state.Jams[0].LeftRoster, x => x.Number == 8);
        }

        [Fact]
        public void MisisngSkaterDoesNothing()
        {
            _command.Number = -1;
            _handler.Handle(_command);
        }

        [Fact]
        public void ReturnsCorrectEvent()
        {
            var response = _handler.Handle(_command);
            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(BoutStateUpdatedEvent));
        }
    }
}
