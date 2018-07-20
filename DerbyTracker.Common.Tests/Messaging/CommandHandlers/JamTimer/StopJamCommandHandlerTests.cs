using DerbyTracker.Common.Messaging.CommandHandlers.JamClock;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Messaging.Events.JamClock;
using DerbyTracker.Common.Services;
using DerbyTracker.Common.Services.Mocks;
using System;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging.CommandHandlers.JamTimer
{
    public class StopJamCommandHandlerTests
    {
        private readonly IBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();

        public StopJamCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
        }

        //This needs a ton of tests, creation pending meeting with NSO

        [Fact]
        public void StopJamDoesTheThingsThatStopJamShouldDo()
        {
            var state = _boutRunner.GetBoutState(Guid.Empty);
            state.Phase = Entities.BoutPhase.Jam;

            var command = new StopJamCommand(Guid.Empty, "connection");
            var handler = new StopJamCommandHandler(_boutRunner, _boutData);
            var response = handler.Handle(command);

            //Assert.Equal(BoutPhase.Jam, state.Phase);
            //Assert.True(state.JamClock().TotalSeconds < 1);
            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(JamEndedEvent));
        }

        //IfJamStopsWithTimeOnClockGoToLineup
        //IfJamStopsWithoutTimeOnClockInLastPeriodGoToUnofficialFinal

    }
}
