using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.CommandHandlers.ScoreKeeper;
using DerbyTracker.Common.Messaging.Commands.ScoreKeeper;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using System.Linq;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.ScoreKeeper
{
    public class UpdatePassCommandHandlerTests
    {
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly UpdatePassCommand _command =
            new UpdatePassCommand(Guid.Empty, "originator", 1, 1, "left", new Pass { Number = 1 });
        private readonly UpdatePassCommandHandler _handler;
        private readonly BoutState _state;

        public UpdatePassCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            _state = _boutRunner.GetBoutState(Guid.Empty);
            _state.Phase = BoutPhase.Jam;
            var team = _state.Jams[0].Team("left");
            team.Passes.Add(new Pass { Number = 1 });
            _handler = new UpdatePassCommandHandler(_boutRunner);
        }

        [Fact]
        public void ScoreIsUpdated()
        {
            _command.Pass.Score = 4;
            _handler.Handle(_command);
            var team = _state.Jams[0].Team("left");
            Assert.Equal(4, team.Passes[1].Score);
        }

        [Fact]
        public void StarPassIsUpdated()
        {
            _command.Pass.StarPass = true;
            _handler.Handle(_command);
            var team = _state.Jams[0].Team("left");
            Assert.True(team.Passes[1].StarPass);
        }

        [Fact]
        public void ThrowsIfPassMissing()
        {
            _command.Pass.Number = 42;
            Assert.Throws<NoSuchPassException>(() => _handler.Handle(_command));
        }

        [Fact]
        public void CantUpdateScoreOnPassZero()
        {
            _command.Pass.Number = 0;
            _command.Pass.Score = 1;
            Assert.Throws<InvalidPassException>(() => _handler.Handle(_command));
        }

        [Fact]
        public void AddingScoreAddsPassIfClockRunningAndItsLastPass()
        {
            _state.GameClock.Start();
            _command.Pass.Score = 1;
            _handler.Handle(_command);
            Assert.Equal(3, _state.Jams.Last().Left.Passes.Count);
        }

        [Fact]
        public void AddingScoreDoesntAddPassIfClockNotRunning()
        {
            _command.Pass.Score = 1;
            _handler.Handle(_command);
            Assert.Equal(2, _state.Jams.Last().Left.Passes.Count);
        }

        [Fact]
        public void AddingScoreDoesntAddPassIfItsNotLastPass()
        {
            _state.GameClock.Start();
            var team = _state.Jams[0].Team("left");
            team.Passes.Add(new Pass { Number = 2 });
            _command.Pass.Score = 1;
            _handler.Handle(_command);
            Assert.Equal(3, _state.Jams.Last().Left.Passes.Count);
        }
    }
}
