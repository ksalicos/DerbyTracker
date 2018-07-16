using DerbyTracker.Common.Messaging.Commands.JamClock;
using System;
using System.Threading.Tasks;

namespace DerbyTracker.Master.SignalR
{
    public partial class WheelHub
    {
        public async Task ExitPregame(Guid boutId)
        {
            var command = new ExitPregameCommand(boutId, Context.ConnectionId);
            await _dispatcher.Dispatch(command);
        }

        public async Task StartJam(Guid boutId)
        {
            var command = new StartJamCommand(boutId, Context.ConnectionId);
            await _dispatcher.Dispatch(command);
        }
    }
}
