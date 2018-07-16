using System;

namespace DerbyTracker.Common.Messaging.Events.Bout
{
    public class BoutRunningEvent : BaseBoutEvent
    {
        public override string Type => "BOUT_RUNNING";
        public string Title { get; set; }

        public BoutRunningEvent(Guid boutId, string title) : base(boutId)
        {
            Title = title;
        }
    }
}
