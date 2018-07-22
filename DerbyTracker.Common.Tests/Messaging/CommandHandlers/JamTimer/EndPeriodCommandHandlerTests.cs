using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.CommandHandlers.JamClock;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.JamTimer
{




    public class EndPeriodCommandHandlerTests
    {
        private readonly IBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();

        private readonly EndPeriodCommand _command = new EndPeriodCommand(Guid.Empty, "originator");
        private readonly EndPeriodCommandHandler _handler;

        public EndPeriodCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            _handler = new EndPeriodCommandHandler(_boutRunner, _boutData);
        }

        [Fact]
        public void MustBeInLineup()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.Phase = Entities.BoutPhase.Jam;
            Assert.Throws<InvalidBoutPhaseException>(() => { _handler.Handle(_command); });
        }

        [Fact]
        public void DoesNothingIfTimeRemains()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.Phase = Entities.BoutPhase.Lineup;
            _handler.Handle(_command);
            Assert.Equal(Entities.BoutPhase.Lineup, state.Phase);
        }

        [Fact]
        public void StartsHalftimeIfFirstPeriod()
        {
            var bout = _boutData.Load(Guid.Empty);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.Phase = Entities.BoutPhase.Lineup;
            state.GameClock.Nudge(TimeSpan.FromSeconds(bout.RuleSet.PeriodDurationSeconds + 1));

            _handler.Handle(_command);
            Assert.Equal(Entities.BoutPhase.Halftime, state.Phase);
        }

        [Fact]
        public void StartsUnofficialFinalIfSecondPeriod()
        {
            var bout = _boutData.Load(Guid.Empty);
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.Period = 2;
            state.Phase = Entities.BoutPhase.Lineup;
            state.GameClock.Nudge(TimeSpan.FromSeconds(bout.RuleSet.PeriodDurationSeconds + 1));

            _handler.Handle(_command);
            Assert.Equal(Entities.BoutPhase.UnofficialFinal, state.Phase);
        }

    }
}
