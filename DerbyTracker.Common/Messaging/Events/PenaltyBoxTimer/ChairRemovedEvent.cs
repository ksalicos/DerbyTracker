using DerbyTracker.Common.Messaging.Events.Base;
using System;

namespace DerbyTracker.Common.Messaging.Events.PenaltyBoxTimer
{
    public class ChairRemovedEvent : BaseBoutEvent
    {
        public override string Type => "CHAIR_REMOVED";
        public Guid Id { get; set; }

        public ChairRemovedEvent(Guid boutId, Guid chairId) : base(boutId)
        {
            Id = chairId;
        }
    }
}
