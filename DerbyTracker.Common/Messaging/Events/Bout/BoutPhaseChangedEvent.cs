using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Events.Base;
using System;

namespace DerbyTracker.Common.Messaging.Events.Bout
{
    public class BoutPhaseChangedEvent : BaseBoutEvent
    {
        public override string Type => "BOUT_PHASE_CHANGED";
        public BoutPhase NewPhase { get; set; }

        public BoutPhaseChangedEvent(BoutPhase newPhase, Guid boutId) : base(boutId)
        {
            NewPhase = newPhase;
        }
    }
}
