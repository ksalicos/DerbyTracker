using DerbyTracker.Common.Entities;
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

        public async Task RemoveSkaterFromJam(string nodeId, Guid boutId, int period, int jam, string team, int number)
        {
            var command = new RemoveSkaterFromJamCommand(boutId, Context.ConnectionId, period, jam, team, number);
            await Dispatch(nodeId, NodeRoles.LineupsTracker, command);
        }

        public async Task SetSkaterPosition(string nodeId, Guid boutId, int period, int jam, string team, int number, Position position)
        {
            var command = new SetSkaterPositionCommand(boutId, Context.ConnectionId, period, jam, team, number, position);
            await Dispatch(nodeId, NodeRoles.LineupsTracker, command);
        }
    }
}