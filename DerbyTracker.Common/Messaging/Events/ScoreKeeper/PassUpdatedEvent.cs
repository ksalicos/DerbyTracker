using DerbyTracker.Common.Messaging.Events.Base;
using System;

namespace DerbyTracker.Common.Messaging.Events.ScoreKeeper
{
    public class PassUpdatedEvent : BaseJamEvent
    {
        public override string Type => "PASS_UPDATED";
        public string Team { get; set; }

        public PassUpdatedEvent(Guid boutId, int period, int jam, string team) : base(boutId, period, jam)
        { Team = team; }
    }
}
