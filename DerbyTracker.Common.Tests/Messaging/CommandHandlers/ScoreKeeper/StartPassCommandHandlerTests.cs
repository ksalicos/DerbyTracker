using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.CommandHandlers.ScoreKeeper;
using DerbyTracker.Common.Messaging.Commands.ScoreKeeper;
using DerbyTracker.Common.Messaging.Events.ScoreKeeper;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.ScoreKeeper
{
    public class CreatePassCommandHandlerTests
    {
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly CreatePassCommand _command = new CreatePassCommand(Guid.Empty, "originator", 1, 1, "left");
        private readonly CreatePassCommandHandler _handler;
        private readonly BoutState _state;

        public CreatePassCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            _state = _boutRunner.GetBoutState(Guid.Empty);
            _state.Phase = BoutPhase.Jam;

            _handler = new CreatePassCommandHandler(_boutRunner);
        }

        [Fact]
        public void PassIsAdded()
        {
            var response = _handler.Handle(_command);
            var team = _state.Jams[0].Team("left");

            Assert.Single(team.Passes, x => x.Number == 1);
            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(JamUpdatedEvent));
        }

        [Fact]
        public void SecondPassIsNumberedCorrectly()
        {
            var team = _state.Jams[0].Team("left");
            team.Passes.Add(new Pass { Number = 1 });
            _handler.Handle(_command);
            Assert.Single(team.Passes, x => x.Number == 2);
        }
    }
}
