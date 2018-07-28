using DerbyTracker.Common.Messaging.Events.Base;
using System;

namespace DerbyTracker.Common.Messaging.Events.JamClock
{
    public class JamEndedEvent : BaseBoutEvent
    {
        public override string Type => "JAM_ENDED";
        public JamEndedEvent(Guid boutId) : base(boutId)
        { }
    }
}
