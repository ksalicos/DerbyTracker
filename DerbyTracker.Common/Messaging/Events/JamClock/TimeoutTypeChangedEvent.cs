using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Events.Bout;
using System;

namespace DerbyTracker.Common.Messaging.Events.JamClock
{
    public class TimeoutTypeChangedEvent : BaseBoutEvent
    {
        public TimeoutType NewType { get; set; }

        public TimeoutTypeChangedEvent(TimeoutType newType, Guid boutId) : base(boutId)
        { NewType = newType; }
    }
}
