using DerbyTracker.Common.Messaging.Events.Bout;
using System;

namespace DerbyTracker.Common.Messaging.Events.JamClock
{
    public class JamStartedEvent : BaseBoutEvent
    {
        public override string Type => "JAM_STARTED";
        public JamStartedEvent(Guid boutId) : base(boutId)
        { }
    }
}
