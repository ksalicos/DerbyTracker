using DerbyTracker.Common.Messaging.Events.Bout;
using System;

namespace DerbyTracker.Common.Messaging.Events.JamClock
{
    public class PeriodEndedEvent : BaseBoutEvent
    {
        public override string Type => "PERIOD_ENDED";
        public PeriodEndedEvent(Guid boutId) : base(boutId)
        { }
    }
}
