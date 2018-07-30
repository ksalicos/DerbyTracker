using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Events.Base;
using System;

namespace DerbyTracker.Common.Messaging.Events.ScoreKeeper
{
    public class JamUpdatedEvent : BaseBoutEvent
    {
        public override string Type => "JAM_UPDATED";

        public Jam Jam { get; set; }
        public JamUpdatedEvent(Guid boutId, Jam jam) : base(boutId)
        {
            Jam = jam;
        }
    }
}
