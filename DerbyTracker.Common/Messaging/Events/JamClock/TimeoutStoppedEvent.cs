using DerbyTracker.Common.Messaging.Events.Base;
using System;

namespace DerbyTracker.Common.Messaging.Events.JamClock
{
    public class TimeoutStoppedEvent : BaseBoutEvent
    {
        public TimeoutStoppedEvent(Guid boutId) : base(boutId)
        { }
    }
}
