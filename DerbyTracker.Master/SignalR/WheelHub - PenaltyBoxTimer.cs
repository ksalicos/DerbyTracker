using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Enums;
using DerbyTracker.Common.Messaging.Commands.PenaltyBoxTimer;
using System;
using System.Threading.Tasks;

namespace DerbyTracker.Master.SignalR
{
    public partial class WheelHub
    {
        public async Task ButtHitSeat(string nodeId, Guid boutId, Chair chair)
        {
            var command = new ButtHitSeatCommand(boutId, Context.ConnectionId, chair);
            await Dispatch(nodeId, NodeRoles.PenaltyBoxTimer, command);
        }
        public async Task CancelSit(string nodeId, Guid boutId, Guid id)
        {
            var command = new CancelSitCommand(boutId, Context.ConnectionId, id);
            await Dispatch(nodeId, NodeRoles.PenaltyBoxTimer, command);
        }
        public async Task ReleaseSkater(string nodeId, Guid boutId, Guid id)
        {
            var command = new ReleaseSkaterCommand(boutId, Context.ConnectionId, id);
            await Dispatch(nodeId, NodeRoles.PenaltyBoxTimer, command);
        }
        public async Task UpdateChair(string nodeId, Guid boutId, Chair chair)
        {
            var command = new UpdateChairCommand(boutId, Context.ConnectionId, chair);
            await Dispatch(nodeId, NodeRoles.PenaltyBoxTimer, command);
        }
    }
}
