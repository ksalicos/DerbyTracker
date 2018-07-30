using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Enums;
using DerbyTracker.Common.Messaging.Commands.ScoreKeeper;
using System;
using System.Threading.Tasks;

namespace DerbyTracker.Master.SignalR
{
    public partial class WheelHub
    {
        public async Task UpdateJammerStatus(string nodeId, Guid boutId, int period, int jam, string team, JammerStatus newStatus)
        {
            var command = new UpdateJammerStatusCommand(boutId, Context.ConnectionId, period, jam, team, newStatus);
            await Dispatch(nodeId, NodeRoles.ScoreKeeper, command);
        }

        public async Task CreatePass(string nodeId, Guid boutId, int period, int jam, string team)
        {
            var command = new CreatePassCommand(boutId, Context.ConnectionId, period, jam, team);
            await Dispatch(nodeId, NodeRoles.ScoreKeeper, command);
        }

        public async Task UpdatePass(string nodeId, Guid boutId, int period, int jam, string team, Pass pass)
        {
            var command = new UpdatePassCommand(boutId, Context.ConnectionId, period, jam, team, pass);
            await Dispatch(nodeId, NodeRoles.ScoreKeeper, command);
        }
    }
}
