using DerbyTracker.Common.Messaging.Events.Base;
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
