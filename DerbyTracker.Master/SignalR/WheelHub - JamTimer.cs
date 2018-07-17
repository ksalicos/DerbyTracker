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
    }
}
