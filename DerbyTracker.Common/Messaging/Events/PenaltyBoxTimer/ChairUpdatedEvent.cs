using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Events.Base;
using System;

namespace DerbyTracker.Common.Messaging.Events.PenaltyBoxTimer
{
    public class ChairUpdatedEvent : BaseBoutEvent
    {
        public override string Type => "CHAIR_UPDATED";
        public Chair Chair { get; set; }

        public ChairUpdatedEvent(Guid boutId, Chair chair) : base(boutId)
        {
            Chair = chair;
        }
    }
}
