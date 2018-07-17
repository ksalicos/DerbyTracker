using DerbyTracker.Common.Messaging.Events.Bout;
using System;

namespace DerbyTracker.Common.Messaging.Events.JamClock
{
    public class BoutEndedEvent : BaseBoutEvent
    {
        public override string Type => "BOUT_ENDED";
        public BoutEndedEvent(Guid boutId) : base(boutId)
        { }
    }
}
