using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Enums;
using DerbyTracker.Common.Messaging.Commands.PenaltyTracker;
using System;
using System.Threading.Tasks;

namespace DerbyTracker.Master.SignalR
{
    public partial class WheelHub
    {
        public async Task CreatePenalty(string nodeId, Guid boutId, int period, int jam, string team)
        {
            var command = new CreatePenaltyCommand(boutId, Context.ConnectionId, period, jam, team);
            await Dispatch(nodeId, NodeRoles.PenaltyTracker, command);
        }

        public async Task UpdatePenalty(string nodeId, Guid boutId, Penalty penalty)
        {
            var command = new UpdatePenaltyCommand(boutId, Context.ConnectionId, penalty);
            await Dispatch(nodeId, NodeRoles.PenaltyTracker, command);
        }
    }
}
