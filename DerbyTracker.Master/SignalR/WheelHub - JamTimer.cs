using DerbyTracker.Common.Messaging.Commands.JamClock;
using System;
using System.Threading.Tasks;

namespace DerbyTracker.Master.SignalR
{
    public partial class WheelHub
    {
        public async Task ExitPregame(string nodeId, Guid boutId)
        {
            var command = new ExitPregameCommand(nodeId, boutId, Context.ConnectionId);
            await _dispatcher.Dispatch(command);
        }

        public async Task StartJam(string nodeId, Guid boutId)
        {
            var command = new StartJamCommand(nodeId, boutId, Context.ConnectionId);
            await _dispatcher.Dispatch(command);
        }
    }
}
