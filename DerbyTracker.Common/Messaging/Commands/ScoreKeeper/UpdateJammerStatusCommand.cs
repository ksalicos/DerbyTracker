using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.ScoreKeeper
{
    public class UpdateJammerStatusCommand : BaseJamCommand
    {
        public string Team { get; set; }
        public JammerStatus NewStatus { get; set; }

        public UpdateJammerStatusCommand(Guid boutId, string originator, int period, int jam, string team, JammerStatus newStatus) : base(boutId, originator, period, jam)
        {
            Team = team;
            NewStatus = newStatus;
        }
    }
}
