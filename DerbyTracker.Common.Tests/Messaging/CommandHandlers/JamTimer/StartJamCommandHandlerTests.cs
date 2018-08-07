
using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.CommandHandlers.JamClock;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Messaging.Events.Bout;
using DerbyTracker.Common.Messaging.Events.PenaltyBoxTimer;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using System.Linq;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.JamTimer
{
    public class StartJamCommandHandlerTests
    {
        private readonly IBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly StartJamCommand _command = new StartJamCommand(Guid.Empty, "connection");
        private readonly StartJamCommandHandler _handler;
        private readonly BoutState _state;

        public StartJamCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            _state = _boutRunner.GetBoutState(Guid.Empty);
            _state.Phase = BoutPhase.Lineup;
            _handler = new StartJamCommandHandler(_boutRunner, _boutData);
        }

        [Fact]
        public void CorrectEventIsBroadcast()
        {
            var response = _handler.Handle(_command);
            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(BoutStateUpdatedEvent));
        }

        [Fact]
        public void StoppedGameClockStarts()
        {
            _handler.Handle(_command);
            Assert.True(_state.GameClock.Running);
        }

        [Fact]
        public void PhaseSetToJam()
        {
            _handler.Handle(_command);
            Assert.Equal(BoutPhase.Jam, _state.Phase);
        }

        [Fact]
        public void PenaltyBoxTimersStart()
        {
            _state.PenaltyBox.Add(new Chair());
            _state.PenaltyBox.Add(new Chair());
            _state.PenaltyBox.Add(new Chair());
            var response = _handler.Handle(_command);
            Assert.True(_state.PenaltyBox.TrueForAll(x => x.StopWatch.Running));
            Assert.Equal(3, response.Events.Count(x => x.Event.GetType() == typeof(ChairUpdatedEvent)));
        }

        [Fact]
        public void JamClockIsReset()
        {
            _handler.Handle(_command);
            Assert.True(_state.JamClock().TotalSeconds < 1);
        }
    }
}
