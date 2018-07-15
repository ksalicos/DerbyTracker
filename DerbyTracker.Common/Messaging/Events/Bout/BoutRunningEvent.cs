using DerbyTracker.Messaging.Events;
using System;

namespace DerbyTracker.Common.Messaging.Events.Bout
{
    public class BoutRunningEvent : BaseEvent
    {
        public override string type => "BOUT_RUNNING";
        public Guid BoutId { get; set; }
        public string Title { get; set; }

        public BoutRunningEvent(Guid boutId, string title)
        {
            BoutId = boutId;
            Title = title;
        }
    }
}
