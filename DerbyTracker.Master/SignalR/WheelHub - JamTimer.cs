using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Enums;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using System;
using System.Threading.Tasks;

namespace DerbyTracker.Master.SignalR
{
    public partial class WheelHub
    {
        public async Task ExitPregame(string nodeId, Guid boutId)
        {
            var command = new EnterLineupPhaseCommand(boutId, Context.ConnectionId);
            await _dispatcher.Dispatch(command);
        }

        public async Task StartJam(string nodeId, Guid boutId)
        {
            var command = new StartJamCommand(boutId, Context.ConnectionId);
            await Dispatch(nodeId, NodeRoles.JamTimer, command);
        }

        public async Task StopJam(string nodeId, Guid boutId)
        {
            var command = new StopJamCommand(boutId, Context.ConnectionId);
            await Dispatch(nodeId, NodeRoles.JamTimer, command);
        }

        public async Task StartTimeout(string nodeId, Guid boutId)
        {
            var command = new StartTimeoutCommand(boutId, Context.ConnectionId);
            await Dispatch(nodeId, NodeRoles.JamTimer, command);
        }

        public async Task StopTimeout(string nodeId, Guid boutId)
        {
            var command = new StopTimeoutCommand(boutId, Context.ConnectionId);
            await Dispatch(nodeId, NodeRoles.JamTimer, command);
        }

        public async Task SetTimeoutType(string nodeId, Guid boutId, TimeoutType timeoutType)
        {
            var command = new SetTimeoutTypeCommand(boutId, Context.ConnectionId, timeoutType);
            await Dispatch(nodeId, NodeRoles.JamTimer, command);
        }

        public async Task SetLoseOfficialReview(string nodeId, Guid boutId, bool loseReview)
        {
            var command = new SetLoseOfficialReviewCommand(boutId, Context.ConnectionId, loseReview);
            await Dispatch(nodeId, NodeRoles.JamTimer, command);
        }
    }
}
