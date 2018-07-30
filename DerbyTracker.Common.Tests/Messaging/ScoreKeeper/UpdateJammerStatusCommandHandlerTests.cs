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
    public class UpdateJammerStatusCommandHandlerTests
    {
        private readonly MockBoutDataService _boutData = new MockBoutDataService();
        private readonly IBoutRunnerService _boutRunner = new BoutRunnerService();
        private readonly UpdateJammerStatusCommandHandler _handler;
        private readonly UpdateJammerStatusCommand _command
            = new UpdateJammerStatusCommand(Guid.Empty, "originator", 1, 1, "left", JammerStatus.Lead);
        private readonly BoutState _state;

        public UpdateJammerStatusCommandHandlerTests()
        {
            var bout = _boutData.Load(Guid.Empty);
            _boutRunner.StartBout(bout);
            _state = _boutRunner.GetBoutState(Guid.Empty);
            _state.Phase = BoutPhase.Jam;

            _handler = new UpdateJammerStatusCommandHandler(_boutRunner);
        }

        [Fact]
        public void JammerStatusIsUpdated()
        {
            var response = _handler.Handle(_command);
            var team = _state.Jams[0].Team("left");

            Assert.Equal(JammerStatus.Lead, team.JammerStatus);
            Assert.Contains(response.Events, x => x.Event.GetType() == typeof(JamUpdatedEvent));
        }

        [Fact]
        public void InitialPassCreated()
        {
            _handler.Handle(_command);
            var team = _state.Jams[0].Team("left");
            Assert.Contains(team.Passes, x => x.Number == 1);
        }

        //TODO: Consult designer to see if we want to enforce this
        //[Fact]
        //public void CannotBecomeLeadIfOtherJammerIs()
        //{
        //    _state.Jams[0].RightJammerStatus = JammerStatus.Lead;
        //    Assert.Throws<InvalidJammerStatusException>(() => _handler.Handle(_command));
        //}

        //[Fact]
        //public void CantBecomeLeadAfterLosingLead()
        //{
        //    _state.Jams[0].LeftJammerStatus = JammerStatus.LostLead;
        //    Assert.Throws<InvalidJammerStatusException>(() => _handler.Handle(_command));
        //}

        //[Fact]
        //public void CantBecomeLeadIfCantBecomeLead()
        //{
        //    _state.Jams[0].LeftJammerStatus = JammerStatus.CantGainLead;
        //    Assert.Throws<InvalidJammerStatusException>(() => _handler.Handle(_command));
        //}

        //[Fact]
        //public void CantLoseLeadIfNotLead()
        //{
        //    _command.NewStatus = JammerStatus.LostLead;
        //    Assert.Throws<InvalidJammerStatusException>(() => _handler.Handle(_command));
        //}
    }
}
