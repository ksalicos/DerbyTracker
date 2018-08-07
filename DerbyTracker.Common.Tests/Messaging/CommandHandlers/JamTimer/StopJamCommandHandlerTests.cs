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
    public class StopJamCommandHandlerTests
    {
        private readonly IBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly StopJamCommand _command = new StopJamCommand(Guid.Empty, "connection");
        private readonly StopJamCommandHandler _handler;
        private readonly BoutState _state;

        public StopJamCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            _state = _boutRunner.GetBoutState(Guid.Empty);
            _state.Phase = BoutPhase.Jam;
            _handler = new StopJamCommandHandler(_boutRunner, _boutData);
        }

        [Fact]
        public void NewJamIsCreated()
        {
            _handler.Handle(_command);
            var jam = _state.Jams.Last();
            Assert.Equal(2, jam.JamNumber);
        }

        [Fact]
        public void CorrectEventIsBroadcast()
        {
            var response = _handler.Handle(_command);
            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(BoutStateUpdatedEvent));
        }

        [Fact]
        public void PenaltyBoxTimersStop()
        {
            var chair1 = new Chair
            { StopWatch = new StopWatch { Running = true, LastStarted = DateTime.Now - TimeSpan.FromSeconds(20) } };
            var chair2 = new Chair
            { StopWatch = new StopWatch { Running = true, LastStarted = DateTime.Now - TimeSpan.FromSeconds(20) } };
            var chair3 = new Chair
            { StopWatch = new StopWatch { Running = true, LastStarted = DateTime.Now - TimeSpan.FromSeconds(20) } };
            _state.PenaltyBox.Add(chair1);
            _state.PenaltyBox.Add(chair2);
            _state.PenaltyBox.Add(chair3);
            var response = _handler.Handle(_command);
            Assert.True(_state.PenaltyBox.TrueForAll(x => !x.StopWatch.Running));
            Assert.Equal(3, response.Events.Count(x => x.Event.GetType() == typeof(ChairUpdatedEvent)));
        }

        //IfJamStopsWithTimeOnClockGoToLineup
        //IfJamStopsWithoutTimeOnClockInLastPeriodGoToUnofficialFinal




    }
}
