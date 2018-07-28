using DerbyTracker.Common.Messaging.Events.Base;
using System;

namespace DerbyTracker.Common.Messaging.Events.JamClock
{
    public class TimeoutStartedEvent : BaseBoutEvent
    {
        public TimeoutStartedEvent(Guid boutId) : base(boutId)
        { }
    }
}
