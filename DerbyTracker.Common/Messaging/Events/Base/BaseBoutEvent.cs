using DerbyTracker.Messaging.Events;
using System;

namespace DerbyTracker.Common.Messaging.Events.Base
{
    public abstract class BaseBoutEvent : BaseEvent
    {
        public Guid BoutId { get; set; }
        public DateTime ServerTime { get; set; }

        protected BaseBoutEvent(Guid boutId)
        {
            BoutId = boutId;
            ServerTime = DateTime.Now;
        }
    }
}
