using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Events.Base;
using System;

namespace DerbyTracker.Common.Messaging.Events.PenaltyTracker
{
    public class PenaltyUpdatedEvent : BaseBoutEvent
    {
        public override string Type => "PENALTY_UPDATED";
        public Penalty Penalty { get; set; }

        public PenaltyUpdatedEvent(Guid boutId, Penalty penalty) : base(boutId)
        { Penalty = penalty; }
    }
}
