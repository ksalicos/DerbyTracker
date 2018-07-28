using System;

namespace DerbyTracker.Common.Messaging.Events.Base
{
    public class BaseJamEvent : BaseBoutEvent
    {
        public int Period { get; set; }
        public int Jam { get; set; }

        public BaseJamEvent(Guid boutId, int period, int jam) : base(boutId)
        {
            Period = period;
            Jam = jam;
        }
    }
}
