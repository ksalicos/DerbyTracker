using DerbyTracker.Common.Messaging.Commands.Bout;
using DerbyTracker.Common.Messaging.Commands.Node;
using System;
using System.Threading.Tasks;

namespace DerbyTracker.Master.SignalR
{
    public partial class WheelHub
    {
        public async Task RunBout(string nodeId, Guid boutId)
        {
            var command = new RunBoutCommand(nodeId, boutId, Context.ConnectionId);
            await _dispatcher.Dispatch(command);
        }

        public async Task AssignRole(string nodeId, string role)
        {
            var command = new AssignRoleToNodeCommand(nodeId, role, Context.ConnectionId);
            await _dispatcher.Dispatch(command);
        }

        public async Task RemoveRole(string nodeId, string role)
        {
            var command = new RemoveRoleFromNodeCommand(nodeId, role, Context.ConnectionId);
            await _dispatcher.Dispatch(command);
        }
    }
}
