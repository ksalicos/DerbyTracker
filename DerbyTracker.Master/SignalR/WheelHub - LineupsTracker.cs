using DerbyTracker.Common.Enums;
using DerbyTracker.Common.Messaging.Commands.LineupsTracker;
using System;
using System.Threading.Tasks;

namespace DerbyTracker.Master.SignalR
{
    public partial class WheelHub
    {
        public async Task AddSkaterToJam(string nodeId, Guid boutId, int period, int jam, string team, int number)
        {
            var command = new AddSkaterToJamCommand(boutId, Context.ConnectionId, period, jam, team, number);
            await Dispatch(nodeId, NodeRoles.LineupsTracker, command);
        }
    }
}